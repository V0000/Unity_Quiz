#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Tools
{
    [CustomEditor(typeof(MenuManager))]
    public class GameDataCleaner : Editor
    {
        public override void OnInspectorGUI()
        {
            // Отображение стандартных полей в инспекторе
            DrawDefaultInspector();

            // Добавление кнопки в инспекторе
            if (GUILayout.Button("Clear score in Game Data"))
            {
                MenuManager manager = (MenuManager)target;
                manager.ClearScore();
            }
        }
    }
}
#endif
