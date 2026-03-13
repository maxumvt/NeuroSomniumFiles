using UnityEngine;
using TMPro;
using NeuroSomniumFiles;

public class State
{
    public GameConnection main;
    public State(GameConnection main)
    {
        this.main = main;
    }

    public void Update(bool allowSearch, NeuroConnection connection)
    {
        main.debugFuntion("Update is being done");
    }
    
}