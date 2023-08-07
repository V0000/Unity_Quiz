namespace Levels
{
    [System.Serializable]
    public class LevelInfo
    {
        public int levelID;
        public int starsToUnlock;
        public bool isUnlocked;
        public int stars;
        public int score;
        public int minQuestionDifficulty;
        public int maxQuestionDifficulty;
        public int questionsCount;
    }
}