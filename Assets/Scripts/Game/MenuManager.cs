using System;
using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelsList;
    [SerializeField] private LevelsGameData gameData;
    [SerializeField] private LocalizationManager localizationManager;
    public static LevelsGameData gameDataStatic;
    
    private void Awake()
    {
        try
        {
            gameData.LoadScriptableObject();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            gameData.ClearScore();
            gameData.SaveData();
        }

        gameDataStatic = gameData;
        LevelsList.SetActive(false);
    }
    private void OnEnable()
    {
        UpdateLang();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
        {
            MinimizeApp();
        }
    }
    public void MinimizeApp()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
    }
 
    public void Continue()
    {
        SceneManager.LoadScene("Level");
    }
    public void ShowLevels(bool isActive)
    {
        LevelsList.SetActive(isActive);
    }
    public void Lang()
    {
        gameData.currentLang = (gameData.currentLang == "en") ? "ru" : "en";
        UpdateLang();
    }
    public void UpdateLang()
    {
        localizationManager.LoadLocalizedText(gameData.langFile, gameData.currentLang);
        localizationManager.UpdateTextObjects();
    }
    public void ClearScore()
    {
        gameData.ClearScore();
    }
    public void Quit()
    {
        gameData.SaveData();
        Application.Quit();
    }
}
