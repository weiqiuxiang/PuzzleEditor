using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PourWaterPuzzle;

namespace GameEditor
{
    [CustomEditor(typeof(PourWaterAnimation))]
    public class PourWaterAnimationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            var puzzleAnimation = target as PourWaterAnimation;
            var puzzleManager = serializedObject.FindProperty("puzzleManager");
            if (puzzleManager.objectReferenceValue == null)
            {
                puzzleManager.objectReferenceValue = puzzleAnimation.GetComponent<PourWaterPuzzleController>();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
