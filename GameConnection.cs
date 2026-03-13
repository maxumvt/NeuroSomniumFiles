namespace NeuroSomniumFiles;

using BepInEx;
using UnityEngine;
using System.Data;

[BepInPlugin("com.maxumvt.neurosomniumfiles", "NeuroSomniumFiles", "1.0.0")]
public class GameConnection : BaseUnityPlugin
{
    NeuroConnection connection;
    State state;

    float searchTimer = 0f;
    bool searchAllowed = true;
    

    void Awake()
    {
        // Establish connection
        Logger.LogInfo("NeuroSomniumFiles started");
        connection = new NeuroConnection(this);
        state = new State(this);

    }

    void Update()
    {
        searchTimer += Time.deltaTime;
        if ( searchTimer > 1f) { searchAllowed = true; searchTimer = 0f; }
        else searchAllowed = false;

        state.Update(searchAllowed, connection);
        
    }

    public void debugFuntion(string text)
    {
        Logger.LogInfo(text);
    }
}