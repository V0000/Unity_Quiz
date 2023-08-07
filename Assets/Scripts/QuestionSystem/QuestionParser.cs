using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace QuestionSystem
{
    public class QuestionParser
    {
        public List<Question> ParseQuestionsFromJson(string json)
        {
            List<Question> questions = new List<Question>();

            if (json.Length != 0)
            {
                QuestionData questionData = JsonConvert.DeserializeObject<QuestionData>(json);

                if (questionData != null && questionData.Questions != null)
                {
                    questions = questionData.Questions;
                }
                else
                {
                    Debug.LogWarning("Invalid JSON data or missing 'questions' field.");
                }
            }
            else
            {
                Debug.LogWarning("JSON file is empty");
            }

            return questions;
        }
    }
}