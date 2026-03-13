using UnityEngine;
using TMPro;

public class DescriptionReader
{
    private TextMeshProUGUI descriptionDialogue;

    private string lastLine = "";
    private bool descriptionShow = false;

    private MonoBehaviour plugin;

    public DescriptionReader(MonoBehaviour plugin)
    {
        this.plugin = plugin;
    }

    public void Update(bool allowSearch, NeuroConnection connection)
    {
        if (allowSearch && descriptionDialogue == null)
        {
            descriptionDialogue = GameObject
                .Find("$Root/UICanvas/ScreenScaler/UIOff1/PanelNode/NarrationWindow/GameObject/Background/Text")
                ?.GetComponent<TextMeshProUGUI>();
        }

        if (descriptionDialogue == null) return;

        string text = descriptionDialogue.text;

        if (!string.IsNullOrEmpty(text) && (text != lastLine || descriptionShow))
        {
            string line = "[Description]: " + text;
            Debug.Log(line);

            connection.SendText(line);

            lastLine = text;
            descriptionShow = false;
        }
    }
}