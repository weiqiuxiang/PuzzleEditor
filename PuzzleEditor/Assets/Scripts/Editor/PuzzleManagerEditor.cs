using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;

namespace GameEditor
{
    [CustomEditor(typeof(PuzzleManager))]
    public class PuzzleManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            var puzzleManager = target as PuzzleManager;
            var puzzleAnimation = serializedObject.FindProperty("PuzzleAnimationRef");
            if (puzzleAnimation.objectReferenceValue == null)
            {
                puzzleAnimation.objectReferenceValue = puzzleManager.GetComponent<PuzzleAnimation>();
            }
            var bottlesSettor = serializedObject.FindProperty("bottlesSettor");
            if (bottlesSettor.objectReferenceValue == null)
            {
                bottlesSettor.objectReferenceValue = puzzleManager.GetComponent<BottlesSettor>();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
