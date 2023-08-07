using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "New LevelsGameData", menuName = "Game/LevelsGameData")]
    public class LevelsGameData : ScriptableObject
    {
        public List<LevelInfo> levels;
        public int globalScore;
        public int globalStars;
        public int currentLevelID;
        public string currentLang;

        public List<QuestionsFile> questionsFilesDict;
        public TextAsset langFile;

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/data.json", json);
        }

        public void LoadScriptableObject()
        {
            if (File.Exists(Application.persistentDataPath + "/data.json"))
            {
                string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/data.json");
                JsonUtility.FromJsonOverwrite(json, this);
            }
            else
            {
                Debug.Log("Файл не найден: " + Application.persistentDataPath + "/data.json");
                ClearScore();
                SaveData();
            }
        }

        public TextAsset GetQuestionsFile()
        {
            TextAsset defaultFile = new TextAsset();
            foreach (var fileObj in questionsFilesDict)
            {
                defaultFile = fileObj.file;
                if (fileObj.key == currentLang)
                {
                    return fileObj.file;
                }
            }

            Debug.LogWarning("Quiz file with key {currentLang} not found");
            return defaultFile;
        }

        public void ClearScore()
        {
            foreach (LevelInfo level in levels)
            {
                level.isUnlocked = false;
                level.stars = 0;
                level.score = 0;
            }

            levels[0].isUnlocked = true;
            globalScore = 0;
            globalStars = 0;
            currentLevelID = 0;
        }
    }

    [System.Serializable]
    public class QuestionsFile
    {
        public string key;
        public TextAsset file;
    }
}