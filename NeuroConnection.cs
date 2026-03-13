using WebSocketSharp;
using UnityEngine;
using NeuroSomniumFiles;

public class NeuroConnection
{
    private WebSocket ws;
    private GameConnection main;
    public NeuroConnection(GameConnection main)
    {
        this.main = main;
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
        string text = e.Data;
        main.debugFuntion(text);
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