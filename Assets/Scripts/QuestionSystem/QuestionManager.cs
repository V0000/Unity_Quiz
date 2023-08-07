using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game;
using Levels;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace QuestionSystem
{
    public class QuestionManager : MonoBehaviour
    {
        private LevelsGameData gameData;
        private QuestionParser questionParser;
        private List<Question> questions;
        private TextAsset questionsFile;
        private LevelInfo levelInfo;

        private void Start()
        {
            gameData = GameManager.gameDataStatic;
            questionParser = new QuestionParser();
            levelInfo = gameData.levels[gameData.currentLevelID];
            questionsFile = gameData.GetQuestionsFile();
        }

        public List<Question> GetQuestions()
        {
            LoadQuestionsFromFile();

            var quizQuestions = GetQuestionsByDifficultyRange(levelInfo.minQuestionDifficulty, levelInfo.maxQuestionDifficulty);
            quizQuestions = GetRandomQuestions(quizQuestions, levelInfo.questionsCount);
            quizQuestions = ShuffleAnswers(quizQuestions);

            return quizQuestions;
        }

        private void LoadQuestionsFromFile()
        {
            if (questionsFile == null)
            {
                Debug.LogWarning("Questions file is not assigned!");
                return;
            }

            string json = questionsFile.text;

            // Парсинг списка вопросов из JSON
            questions = questionParser.ParseQuestionsFromJson(json);
            Debug.Log($"Loaded {questions.Count} questions.");
        }

        private List<Question> GetQuestionsByDifficultyRange(int minDifficulty, int maxDifficulty)
        {
            return questions
                .Where(question => question.Difficulty >= minDifficulty && question.Difficulty <= maxDifficulty)
                .ToList();
        }

        private List<Question> GetRandomQuestions(List<Question> allQuestions, int count)
        {
            // Если требуемое количество вопросов больше, чем доступное количество, выбираем все доступные вопросы
            if (count >= allQuestions.Count)
            {
                return allQuestions;
            }

            allQuestions = allQuestions.OrderBy(question => Random.value).Take(count).ToList();
            

            return allQuestions;
        }

        private List<Question> ShuffleAnswers(List<Question> allQuestionsForShuffle)
        {
            List<Question> result = new List<Question>();
            List<string> shuffledAnswers;

            foreach (Question question in allQuestionsForShuffle)
            {
                shuffledAnswers = new List<string>(question.Answers);
                int correctAnswerIndex = question.CorrectAnswerIndex;
                string correctAnswer = question.Answers[correctAnswerIndex];

                for (int i = 0; i < shuffledAnswers.Count; i++)
                {
                    int k = Random.Range(0, shuffledAnswers.Count-1);
                    (shuffledAnswers[k], shuffledAnswers[i]) = (shuffledAnswers[i], shuffledAnswers[k]);
                }
                Question questionRes = question;
                questionRes.Answers = shuffledAnswers.ToArray();
                questionRes.CorrectAnswerIndex = shuffledAnswers.IndexOf(correctAnswer);
                result.Add(questionRes);
            }


            return result;
        }
    }
}