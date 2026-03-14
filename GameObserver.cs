namespace NeuroSomniumFiles;

using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // if using TextMeshProUGUI
using System;
using System.Linq;
using System.Collections.Generic;


[BepInPlugin("com.yourname.dialoglogger", "NeuroSomniumFiles", "1.0.0")]
public class GameObserver : BaseUnityPlugin
{
    NeuroConnection connection;

    public RawImage characterNamePlate;
    public TextMeshProUGUI characterDialogue;
    public TextMeshProUGUI descriptionDialogue;
    public GameObject lookChoices;

    public UnregisterActionsMessage unregisterActionMessage = new UnregisterActionsMessage();
    
    public string dialogueLastline;
    public string descriptionLastline;
    public bool interactLook = false;

    float searchTimer = 0f;
    bool searchAllowed = true;
    

    void Awake()
    {
        connection = new NeuroConnection();
        connection.Connect();
        Logger.LogInfo("NeuroSomniumFiles started");
    }

    void Update()
    {
        searchTimer += Time.deltaTime;
        if ( searchTimer > 1f) { searchAllowed = true; searchTimer = 0f; }
        else searchAllowed = false;

        CharacterSpeaking(searchAllowed);
        DescriptionText(searchAllowed);
        OptionActions(searchAllowed);
    }

    // SECTION: Extractors
    void CharacterSpeaking(bool allowSearch)
    {
        if (allowSearch && characterNamePlate == null) {characterNamePlate = GameObject.Find("$Root/UICanvas/ScreenScaler/UIOff1/PanelNode/MessageWindow/Rig/Name/Text")?.GetComponent<RawImage>();} // Necessary for finding the object
        if (allowSearch && characterDialogue == null) {characterDialogue = GameObject.Find("$Root/UICanvas/ScreenScaler/UIOff1/PanelNode/MessageWindow/Rig/Background/Text")?.GetComponent<TextMeshProUGUI>();} // Necessary for finding the object
        
        if (characterNamePlate != null && characterDialogue != null)
        {
            string nameText = characterNamePlate.mainTexture.name;
            string dialogueText = characterDialogue.text;

            if (!string.IsNullOrEmpty(dialogueText) && dialogueLastline != dialogueText)
            {
                dialogueLastline = dialogueText;
                ContextMessage cMsg = new ContextMessage($"{nameText} says: {dialogueText}", false);
                connection.SendString(cMsg.ToJson());
            }
        }
    }
    void DescriptionText(bool allowSearch)
    {
        if (allowSearch && descriptionDialogue == null) {  descriptionDialogue = GameObject.Find("$Root/UICanvas/ScreenScaler/UIOff1/PanelNode/NarrationWindow/GameObject/Background/Text")?.GetComponent<TextMeshProUGUI>(); }
        if (descriptionDialogue != null)
        {
            string descrText = descriptionDialogue.text;
            // ERROR Clicking description type windows with one line only, can cause the description not to be logged again. This is bad feedback and needs some kind of solution
            if (!string.IsNullOrEmpty(descrText) && (descrText != descriptionLastline))
            {
                descriptionLastline = descrText;
                ContextMessage cMsg = new ContextMessage($"Description text: {descrText}", false);
                connection.SendString(cMsg.ToJson());
            }
        }
    }
    void OptionActions(bool allowSearch)
    {
        if (lookChoices == null) 
        {
            if (!allowSearch) return;

            lookChoices = GameObject.Find("$Root/CommandCanvas/ScreenScaler/Command/Scale");
            if (lookChoices == null) return;
        }

        // code that depends on lookChoices
        bool look = lookChoices.transform.Find("Look").gameObject.activeSelf;
        
        if (interactLook != look)
        {
            interactLook = look;
            string term = lookChoices.transform.Find("Term/Background/Text")?.GetComponent<TextMeshProUGUI>().text; // IMPROVEMENT This object should be more global, so that the description can also use Terms
            
            List<Action> actions = new List<Action>();

            if (look)
            {
                ContextMessage cMsg = new ContextMessage($"Looking at: {term}", false);
                connection.SendString(cMsg.ToJson());
            }
            else
            {
                ContextMessage cMsg = new ContextMessage("Stopped looking.", false);
                connection.SendString(cMsg.ToJson());
                return;
            }

            bool buttonUp = lookChoices.transform.Find("SelectU").gameObject.activeSelf;
            if (buttonUp)
            {
                string buttonUpText = lookChoices.transform.Find("SelectU/Background/Text")?.GetComponent<TextMeshProUGUI>().text;
                actions.Add(new Action("button_up", buttonUpText));
                
            }

            bool buttonDown = lookChoices.transform.Find("SelectD").gameObject.activeSelf;
            if (buttonDown)
            {
                string buttonDownText = lookChoices.transform.Find("SelectD/Background/Text")?.GetComponent<TextMeshProUGUI>().text;
                actions.Add(new Action("button_down", buttonDownText));
            }

            bool buttonLeft = lookChoices.transform.Find("SelectL").gameObject.activeSelf;
            if (buttonLeft)
            {
                string buttonLeftText = lookChoices.transform.Find("SelectL/Background/Text")?.GetComponent<TextMeshProUGUI>().text;
                actions.Add(new Action("button_left", buttonLeftText));
                
            }

            bool buttonRight = lookChoices.transform.Find("SelectR").gameObject.activeSelf;
            if (buttonRight)
            {
                string buttonRightText = lookChoices.transform.Find("SelectR/Background/Text")?.GetComponent<TextMeshProUGUI>().text;
                actions.Add(new Action("button_right", buttonRightText));
            }

            bool buttonZoom = lookChoices.transform.Find("Zoom").gameObject.activeSelf;
            if (buttonZoom) actions.Add(new Action("zoom", "Zoom view."));
            
            bool buttonThermo = lookChoices.transform.Find("Thermo").gameObject.activeSelf;
            if (buttonThermo) actions.Add(new Action("thermo", "Thermo view."));
            
            bool buttonXray = lookChoices.transform.Find("XRay").gameObject.activeSelf;
            if (buttonXray) actions.Add(new Action("xray", "XRay view."));
            
            bool buttonNV = lookChoices.transform.Find("NV").gameObject.activeSelf; // NOTE NV is Night Vision
            if (buttonNV) actions.Add(new Action("night_vision", "Night vision view."));
            
            bool buttonZoomThermo = lookChoices.transform.Find("ZoomThermo").gameObject.activeSelf;
            if (buttonZoomThermo) actions.Add(new Action("zoom_thermo", "Zoom thermo view."));
            
            bool buttonZoomXray = lookChoices.transform.Find("ZoomXRay").gameObject.activeSelf;
            if (buttonZoomXray) actions.Add(new Action("zoom_xray", "Zoom XRay view."));
            
            bool buttonZoomNV = lookChoices.transform.Find("ZoomNV").gameObject.activeSelf;
            if (buttonZoomNV) actions.Add(new Action("zoom_night_vision", "Zoom night vision view."));

            RegisterActionsMessage ram = new RegisterActionsMessage(actions);
            unregisterActionMessage.setActionNames(actions);
            connection.SendString(ram.ToJson());
        
        }

    }
}
