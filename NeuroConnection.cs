using WebSocketSharp;
using UnityEngine;
using BepInEx.Logging;

public class NeuroConnection
{
    private WebSocket ws;
    private ManualLogSource logger;

    public NeuroConnection(ManualLogSource logger)
    {
        this.logger = logger;
    }

    public void Connect()
    {
        ws = new WebSocket("ws://localhost:8000");

        ws.OnOpen += OnOpen;
        ws.OnMessage += OnMessage;
        ws.OnError += OnError;
        ws.OnClose += OnClose;

        ws.Connect();
    }

    public void SendText(string text)
    {
        if (ws == null || !ws.IsAlive) return;

        string json = "{\"" + text.Replace("\"", "\\\"") + "\"}";
        ws.Send(json);
    }

    private void OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("[WebSocket] Connected");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("[WebSocket] Received: " + e.Data);
        logger.LogInfo(e.Data);
    }

    private void OnError(object sender, WebSocketSharp.ErrorEventArgs e)
    {
        Debug.Log("[WebSocket] Error: " + e.Message);
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("[WebSocket] Closed: " + e.Code);
    }
}