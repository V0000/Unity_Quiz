#if UNITY_EDITOR
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;
using QuestionSystem;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class CsvToJsonEditor : EditorWindow
    {
    
        private string _csvFilePath;
        private string _jsonFilePath;
    
        private List<QuestionCSVObject> _questionDataList = new List<QuestionCSVObject>();
        private QuestionData _questionData = new QuestionData();


        [MenuItem("MyTools/Csv To Json Converter")]
        public static void ShowWindow()
        {
            CsvToJsonEditor window = (CsvToJsonEditor)EditorWindow.GetWindow(typeof(CsvToJsonEditor));
            window.Show();
        }
    
        void OnGUI()
        {
            GUILayout.Label("CSV to JSON Converter", EditorStyles.boldLabel);

            _csvFilePath = EditorGUILayout.TextField("CSV File Path:", _csvFilePath);
            _jsonFilePath = EditorGUILayout.TextField("JSON File Path:", _jsonFilePath);

            if (GUILayout.Button("Convert"))
            {
                ConvertCsvToJson();
            }

        }
    
        private void ConvertCsvToJson()
        {
            if (string.IsNullOrEmpty(_csvFilePath) || string.IsNullOrEmpty(_jsonFilePath))
            {
                Debug.LogError("CSV and JSON file paths must be specified.");
                return;
            }

            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Читаем данные из CSV файла в список объектов MyData
                _questionDataList = csv.GetRecords<QuestionCSVObject>().ToList();
            }

            Debug.Log("CSV parsed successfully.");
        
            foreach (var questionCSV in _questionDataList)
            {
                Question question = new Question();
                question.Text = questionCSV.Text;
                question.Answers = new string[] {questionCSV.Answers0,questionCSV.Answers1,questionCSV.Answers2,questionCSV.Answers3};
                question.CorrectAnswerIndex = 0;// в csv первый ответ по умолчанию верный
                question.Explanation = questionCSV.Explanation;
                question.Difficulty = questionCSV.Difficulty;
                _questionData.Questions.Add(question);
            }

            SaveDataToFile(_questionData, _jsonFilePath);
        }
    
        private void SaveDataToFile(QuestionData data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
        }
        public class QuestionCSVObject
        {
            public string Text{ get; set; }
            public string Answers0{ get; set; }
            public string Answers1{ get; set; }
            public string Answers2{ get; set; }
            public string Answers3{ get; set; }
            public string Explanation{ get; set; }
            public int Difficulty{ get; set; }
        }
    }
}

#endif
