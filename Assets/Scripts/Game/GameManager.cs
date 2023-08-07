using System;
using System.Collections;
using System.Collections.Generic;
using Levels;
using QuestionSystem;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private QuestionManager questionManager;
        [SerializeField] private float delayBeforeNextQuestion = 2f;
        [SerializeField] private LevelsGameData gameData;

        public static LevelsGameData gameDataStatic;
        private List<Question> _questions;
        private int _currentQuestionID;
        private int _rightAnswers;
        private int _localScore;
        
        private List<Question> questions;
       

        private void Awake()
        {
            gameDataStatic = gameData;
        }
        private void Start()
        {
            _localScore = 0;
            _rightAnswers = 0;
            _currentQuestionID = 0;
            _questions = questionManager.GetQuestions();
            uiManager.ButtonClickEvent += OnButtonClickEvent;
            QuizStep();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                GoHome();
            }
        }

        public void QuizStep()
        {
            if (_questions.Count > _currentQuestionID)
            {
                uiManager.ResetButtonAnimation();
                uiManager.SetQuestion(_questions[_currentQuestionID]);
                uiManager.SetQuestionCounter($"{_currentQuestionID+1}/{_questions.Count}");
                Debug.Log(_questions[_currentQuestionID].CorrectAnswerIndex);
                _currentQuestionID++;
            }
            else
            {
                FinishingQuiz();
            }
        }

        private void OnButtonClickEvent(int buttonId)
        {
            bool isCorrect = _questions[_currentQuestionID - 1].CorrectAnswerIndex == buttonId;
            uiManager.PlayAnswerButtonAnimation(buttonId, isCorrect);
            StartCoroutine(isCorrect ? RightAnswerCoroutine(buttonId) : WrongAnswerCoroutine(buttonId));
        }

        IEnumerator RightAnswerCoroutine(int buttonId)
        {
            _rightAnswers++;
            _localScore += _questions[_currentQuestionID-1].Difficulty;
            uiManager.BlockScreen(true);
            yield return new WaitForSeconds(delayBeforeNextQuestion);
            uiManager.BlockScreen(false);
            QuizStep();
        }
        
        IEnumerator WrongAnswerCoroutine(int buttonId)
        {
            uiManager.BlockScreen(true);
            yield return new WaitForSeconds(delayBeforeNextQuestion);
            uiManager.BlockScreen(false);
            uiManager.ShowExplanation(true);
        }
        
        private void FinishingQuiz()
        {
            uiManager.ShowFinalScreen(true);
            uiManager.SetFinalScreen(_localScore, CalcStars(), gameData.currentLevelID);

            UpdateLevelsData();
            
            gameData.SaveData();
        }
        
        private void UpdateLevelsData()
        {
            gameData.levels[gameData.currentLevelID].score = Math.Max(_localScore, gameData.levels[gameData.currentLevelID].score);
            gameData.levels[gameData.currentLevelID].stars = Math.Max(CalcStars(), gameData.levels[gameData.currentLevelID].stars);
            
            int globalScoreCalc = 0;
            int globalStarsCalc = 0;
            
            foreach (LevelInfo level in gameData.levels)
            {
                globalScoreCalc += level.score;
                globalStarsCalc += level.stars;
            }
            
            gameData.globalScore = globalScoreCalc;
            gameData.globalStars = globalStarsCalc;

            
            foreach (LevelInfo level in gameData.levels)
            {
                level.isUnlocked = level.starsToUnlock <= gameData.globalStars;
            }

        }
        
        private int CalcStars()
        {
            int mistakes = _questions.Count - _rightAnswers;
            return Math.Max(3 - mistakes, 0);
        }
        
        public void GoHome()
        {
            SceneManager.LoadScene("Menu");
        }
        
        public void RestartLevel()
        {
            SceneManager.LoadScene("Level");
        }
        
        public void NextLevel()
        {
            gameData.currentLevelID = Math.Min(gameData.currentLevelID + 1, gameData.levels.Count-1);
            SceneManager.LoadScene("Level");
        }
        private void OnDestroy()
        {
        
            if (uiManager != null)
            {
                uiManager.ButtonClickEvent -= OnButtonClickEvent;
            }
        }
    }
}