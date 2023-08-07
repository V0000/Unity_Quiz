using System.Collections;
using System.Collections.Generic;
using Levels;
using TMPro;
using UnityEngine;

public class LevelListScreen : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private  TMP_Text score;
    [SerializeField] private  TMP_Text stars;
    void Start()
    {
        foreach (LevelInfo level in MenuManager.gameDataStatic.levels)
        {
            GameObject instance = Instantiate(levelButtonPrefab, transform.position, transform.rotation);
            instance.transform.SetParent(levelsPanel.transform);
            instance.GetComponent<LevelButton>().SetLevelInfo(level, MenuManager.gameDataStatic.globalStars);
        }
        
        score.text = MenuManager.gameDataStatic.globalScore.ToString();
        stars.text = MenuManager.gameDataStatic.globalStars.ToString();
    }

}
