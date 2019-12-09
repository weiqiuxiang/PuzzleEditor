using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;

namespace GameEditor
{
    [CustomEditor(typeof(PuzzleAnimation))]
    public class PuzzleAnimationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            var puzzleAnimation = target as PuzzleAnimation;
            var puzzleManager = serializedObject.FindProperty("puzzleManager");
            if (puzzleManager.objectReferenceValue == null)
            {
                puzzleManager.objectReferenceValue = puzzleAnimation.GetComponent<PuzzleManager>();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
