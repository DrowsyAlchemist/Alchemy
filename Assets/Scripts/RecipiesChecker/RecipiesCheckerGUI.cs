#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace RecipiesChecker
{
    [CustomEditor(typeof(RecipiesCheckerInterface))]
    public sealed class RecipiesCheckerGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var recipiesChecker = (RecipiesCheckerInterface)target;

            if (GUILayout.Button("Проверить и добавить"))
                recipiesChecker.AddRecipiesByCreationRecipieForAll();

            if (GUILayout.Button("Удалить все"))
                recipiesChecker.RemoveAllRecipies();
        }
    }
}
#endif