using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFinalScreen : MonoBehaviour
    {
        public GameObject[] stars;
        public GameObject nextButton;
        public TMP_Text levelScore;
        public TMP_Text levelNumber;
        public TMP_Text winLoseInfo;
        public string winText = "You WIN!";
        public string loseText = "You LOSE!";

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
        
        public void SetScore(int score)
        {
            levelScore.GetComponentInChildren<TMP_Text>().text = score.ToString();
        }
        public void SetWinInfo(bool isWin)
        {
            winLoseInfo.GetComponentInChildren<TMP_Text>().text = isWin ? winText : loseText;
        }
        
        public void SetActiveNext(bool isActive)
        {
            nextButton.SetActive(isActive);
        }
        
        public void SetLevelNumber(int lvlNum)
        {
            levelNumber.GetComponentInChildren<TMP_Text>().text = $"Level {lvlNum}";
        }
        
    }
}
