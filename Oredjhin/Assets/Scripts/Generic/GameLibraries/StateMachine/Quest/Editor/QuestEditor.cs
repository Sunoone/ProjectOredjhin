using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace StateMachine.Quests
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : Editor {

        Quest quest;

        private void OnEnable()
        {
            quest = (Quest)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open quest window"))
                QuestWindow.Init(quest);
            base.OnInspectorGUI();
        }

        void DrawStateEditor(Object state)
        {
            Editor editor = CreateEditor(state);
            editor.OnInspectorGUI();
        }
    }
}
