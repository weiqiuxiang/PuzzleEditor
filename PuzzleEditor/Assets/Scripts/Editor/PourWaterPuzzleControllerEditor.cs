using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PourWaterPuzzle;

namespace GameEditor
{
    [CustomEditor(typeof(PourWaterPuzzleController))]
    public class PourWaterPuzzleControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            var puzzleManager = target as PourWaterPuzzleController;
            var puzzleAnimation = serializedObject.FindProperty("PuzzleAnimationRef");
            if (puzzleAnimation.objectReferenceValue == null)
            {
                puzzleAnimation.objectReferenceValue = puzzleManager.GetComponent<PourWaterAnimation>();
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
