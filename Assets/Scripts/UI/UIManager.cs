using System;
using System.Collections.Generic;
using QuestionSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private  TMP_Text questionText;
        [SerializeField] private  Button[] answerButtons;
        [SerializeField] private  GameObject detailedExplanationPanel;
        [SerializeField] private  GameObject blockingPanel;
        [SerializeField] private  UIFinalScreen finalScreen;
        [SerializeField] private  TMP_Text questionCounterText;
        [SerializeField] private  string homeScene;

        public  event Action<int> ButtonClickEvent;
        
        private List<string> answers;
        private List<Animator> buttonAnimators;

        private void Awake()
        {
            buttonAnimators = new List<Animator>();
            foreach (Button button in answerButtons)
            {
                buttonAnimators.Add(button.GetComponent<Animator>());
            }

            detailedExplanationPanel.SetActive(false);
            blockingPanel.SetActive(false);
            finalScreen.gameObject.SetActive(false);
        }

        public void SetQuestion(Question question)
        {
            questionText.text = question.Text;

            answers = new List<string>(question.Answers);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[i];
            }

            SetExplanation(question.Explanation);
        }

        public void OnButtonClick(int buttonId)
        {
            ButtonClickEvent?.Invoke(buttonId);
        }

        public void PlayAnswerButtonAnimation(int buttonId, bool isCorrect)
        {
            if (isCorrect)
            {
                buttonAnimators[buttonId].Play("Right");
            }
            else
            {
                buttonAnimators[buttonId].Play("Wrong");
            }
        }

        public void ResetButtonAnimation()
        {
            foreach (Animator animator in buttonAnimators)
            {
                animator.Play("Normal");
            }
        }

        public void SetExplanation(string explanation)
        {
            detailedExplanationPanel.GetComponentInChildren<TMP_Text>().text = explanation;
        }
        
        public void SetQuestionCounter(string questionCounter)
        {
            questionCounterText.GetComponentInChildren<TMP_Text>().text = questionCounter;
        }

        public void ShowExplanation(bool isShow)
        {
            detailedExplanationPanel.SetActive(isShow);
        }
        
        public void ShowFinalScreen(bool isShow)
        {
            finalScreen.gameObject.SetActive(isShow);
        }
        
        public void GoHomeScreen()
        {
            SceneManager.LoadScene(homeScene);
        }

        public void BlockScreen(bool isBloking)
        {
            blockingPanel.SetActive(isBloking);
        }
        
        public void SetFinalScreen(int score, int stars, int lvlID)
        {
            finalScreen.SetScore(score);
            finalScreen.SetStars(stars);
            finalScreen.SetLevelNumber(lvlID+1);
            finalScreen.SetActiveNext(stars>0);
            finalScreen.SetWinInfo(stars>0);
        }

    }
}