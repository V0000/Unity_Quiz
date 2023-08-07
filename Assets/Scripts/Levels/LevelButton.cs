using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Levels
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text needStarsToUnlockText;
        [SerializeField] private TMP_Text LevelNumberText;
        [SerializeField] private GameObject LockScreen;
        public GameObject[] stars;

        private LevelInfo currentlevel;

        public void SetLevelInfo(LevelInfo level, int globalStars)
        {
            currentlevel = level;
            LockScreen.SetActive(!level.isUnlocked);
            SetStars(level.stars);
            LevelNumberText.GetComponentInChildren<TMP_Text>().text = (level.levelID + 1).ToString();
            needStarsToUnlockText.GetComponentInChildren<TMP_Text>().text = $"{globalStars}/{level.starsToUnlock}";
            
        }
        
        public void SetStars(int count)
        {
            count = Math.Min(count, stars.Length);

            //отключаем все
            foreach (var t in stars)
            {
                Color  color = t.GetComponentInChildren<Image>().color;
                color.a = 0.3f;
                t.GetComponentInChildren<Image>().color = color;
            }

            //включаем нужное количество
            for (int i = 0; i < count; i++)
            {
                Color  color = stars[i].GetComponentInChildren<Image>().color;
                color.a = 1f;
                stars[i].GetComponentInChildren<Image>().color = color;
            }
        }
        
        public void OnPressed()
        {

            if (currentlevel.isUnlocked)
            {
                MenuManager.gameDataStatic.currentLevelID = currentlevel.levelID;
                MenuManager.gameDataStatic.SaveData();
                SceneManager.LoadScene("Level");
            }
            
        }
    }
}
