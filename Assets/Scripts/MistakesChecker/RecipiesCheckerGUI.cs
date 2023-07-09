#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace RecipiesChecker
{
    [CustomEditor(typeof(RecipiesCheckerInterface))]
    public class RecipiesCheckerGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var recipiesChecker = (RecipiesCheckerInterface)target;

            if (GUILayout.Button("��������� � ��������"))
                recipiesChecker.AddRecipiesByCreationRecipieForAll();

            if (GUILayout.Button("������� ������"))
                recipiesChecker.RemoveExcessRecipies();
        }
    }
}
#endif