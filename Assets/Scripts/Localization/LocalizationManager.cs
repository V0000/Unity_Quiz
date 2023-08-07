using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private LocalizedObject[] localizedObjects;
    private Dictionary<string, string> localizedText;
    

    public void UpdateTextObjects()
    {
        foreach (LocalizedObject textObj in localizedObjects)
        {         
            textObj.textObject.text = GetLocalizedText(textObj.key);
        }
    }

    public void LoadLocalizedText(TextAsset langFile, string currentLanguage)
    {
        if (langFile == null)
        {
            Debug.LogWarning("Lang file is not assigned!");
            return;
        }
        
        localizedText = new Dictionary<string, string>();
        Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(langFile.text);

        
        if (json.ContainsKey(currentLanguage))
        {
            Dictionary<string, string> languageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json[currentLanguage].ToString());
            localizedText = languageData;
        }
        else
        {
            Debug.LogWarning($"Lang {currentLanguage} not found");
        }

    }

    public string GetLocalizedText(string key)
    {
        if (localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }
        else
        {
            Debug.LogWarning("Key not found in localization: " + key);
            return key;
        }
    }

}