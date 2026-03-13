namespace NeuroSomniumFiles;

using BepInEx;
using UnityEngine;
using BepInEx.Logging;

[BepInPlugin("com.maxumvt.neurosomniumfiles", "NeuroSomniumFiles", "1.0.0")]
public class Main : BaseUnityPlugin
{
    private NeuroConnection connection;
    private State stateManager;


    float searchTimer = 0f;
    bool searchAllowed = true;

    void Awake()
    {
        Logger.LogInfo("NeuroSomniumFiles started");

        connection = new NeuroConnection(Logger);
        connection.Connect();

        stateManager = new State();
    }

    void Update()
    {
        searchTimer += Time.deltaTime;

        if (searchTimer > 1f)
        {
            searchAllowed = true;
            searchTimer = 0f;
        }
        else searchAllowed = false;

        stateManager.Update(searchAllowed, connection);
    }
}