using System;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class DeepLinkHandler : MonoBehaviour
{
    public TextMeshProUGUI OutputLog;
    
    private void Awake()
    {
        OutputLog?.SetText(string.Empty);
        
        Application.deepLinkActivated += OnDeepLinkActivated;

        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }

    private void OnDeepLinkActivated(string url)
    {
        if (OutputLog)
        {
            OutputLog.text += url + "\n";
        }
    }
}