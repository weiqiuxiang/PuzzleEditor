using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace PourWaterPuzzle
{
    [CustomEditor(typeof(BottlesSettor))]
    public class BottlesSettorEditor : Editor
    {
        private ReorderableList bottleDataReorderableList = null;
        private BottlesSettor bottlesSettor_cache = null;
        private BottlesSettor BottlesSettor_target
        {
            get
            {
                if (this.bottlesSettor_cache == null)
                {
                    this.bottlesSettor_cache = target as BottlesSettor;
                }
                return this.bottlesSettor_cache;
            }
        }

        private GUIStyle fontStyle_warning = null;
        private GUIStyle FontStyle_warning
        {
            get
            {
                if (fontStyle_warning == null)
                {
                    fontStyle_warning = new GUIStyle();
                    fontStyle_warning.normal.background = null;    //设置背景填充  
                    fontStyle_warning.normal.textColor = new Color(1, 0, 0);   //设置字体颜色 
                }

                return fontStyle_warning;
            }
        }

        private GUIStyle fontStyle_selected = null;
        private GUIStyle FontStyle_selected
        {
            get
            {
                if (fontStyle_selected == null)
                {
                    fontStyle_selected = new GUIStyle();
                    fontStyle_selected.normal.background = null;    //设置背景填充  
                    fontStyle_selected.normal.textColor = new Color(0, 0, 1);   //设置字体颜色 
                }

                return fontStyle_selected;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            this.drawReference();
            this.drawParameter();
            this.drawBottleDataList();

            serializedObject.ApplyModifiedProperties();
        }

        private void drawReference()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            var puzzleManager = serializedObject.FindProperty("puzzleManager");
            GUI.enabled = false;
            EditorGUILayout.ObjectField(puzzleManager, typeof(PourWaterPuzzleController));
            GUI.enabled = true;
            if (puzzleManager.objectReferenceValue == null)
            {
                puzzleManager.objectReferenceValue = this.BottlesSettor_target.GetComponent<PourWaterPuzzleController>();
            }
            
            var bottleParent = serializedObject.FindProperty("bottleParent");
            EditorGUILayout.ObjectField(bottleParent, typeof(GameObject));

            var bottlePrefab = serializedObject.FindProperty("bottlePrefab");
            EditorGUILayout.ObjectField(bottlePrefab, typeof(GameObject));

            EditorGUILayout.EndVertical();
        }

        private void drawParameter()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            // 最大数
            {
                var maxBottleAmountProperty = serializedObject.FindProperty("MaxBottleAmount");
                maxBottleAmountProperty.intValue = (this.BottlesSettor_target.BottleDatas == null) ? 0 : this.BottlesSettor_target.BottleDatas.Count;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("MaxBottleAmount");
                GUI.enabled = false;
                EditorGUILayout.IntField(maxBottleAmountProperty.intValue);
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }

            // 胜利目标
            {
                var victoryTargetProperty = serializedObject.FindProperty("VictoryTarget");
                EditorGUILayout.IntSlider(victoryTargetProperty, 0, this.BottlesSettor_target.BottleDatas.Count - 1);
                if ((victoryTargetProperty.intValue < 0) || (victoryTargetProperty.intValue > this.BottlesSettor_target.BottleDatas.Count - 1))
                {
                    // victoryTargetProperty.intValue的数值不在列表范围内
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    GUI.contentColor = Color.red;
                    GUILayout.Label("value of victoryTarget is over range", this.FontStyle_warning);
                    EditorGUILayout.EndVertical();
                }
            }

            // 胜利容量
            {
                try
                {
                    var victoryAmountProperty = serializedObject.FindProperty("VictoryAmount");
                    var victoryTargetProperty = serializedObject.FindProperty("VictoryTarget");
                    var data = this.BottlesSettor_target.BottleDatas[victoryTargetProperty.intValue];
                    EditorGUILayout.IntSlider(victoryAmountProperty, 0, data.MaxAmount);
                    if ((victoryAmountProperty.intValue < 0) || (victoryAmountProperty.intValue > data.MaxAmount))
                    {
                        // victoryTargetProperty.intValue的数值不在列表范围内
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        GUILayout.Label("value of victoryAmount is over range", this.FontStyle_warning);
                        EditorGUILayout.EndVertical();
                    }
                }
                catch
                { }
            }

            EditorGUILayout.EndVertical();
        }

        private void drawBottleDataList()
        {
            if (this.bottleDataReorderableList == null)
            {
                // 初始化列表
                this.bottleDataReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("bottleDatas"),
                                                        true, true, true, true);
                this.bottleDataReorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "BottleDatas");

                this.bottleDataReorderableList.elementHeightCallback = (index) =>
                {
                    //var element = bottleDataReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    //var elementHeight = EditorGUI.GetPropertyHeight(element,null,true);
                    return EditorGUIUtility.singleLineHeight * 3.2f;
                };

                this.bottleDataReorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty property = this.bottleDataReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    var IsTarget = property.FindPropertyRelative("IsTarget");
                    var BottleImage = property.FindPropertyRelative("BottleImage");
                    var BottleMask = property.FindPropertyRelative("BottleMask");
                    var CurrentAmount = property.FindPropertyRelative("CurrentAmount");
                    var MaxAmount = property.FindPropertyRelative("MaxAmount");

                    var victoryTargetProperty = serializedObject.FindProperty("VictoryTarget");
                    IsTarget.boolValue = victoryTargetProperty.intValue == index;

                    // rect类初始化
                    rect.height = EditorGUIUtility.singleLineHeight;
                    var leftHalf = new Rect(rect.x, rect.y, rect.width * 0.5f, rect.height);
                    var rightHalf = new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height);
                    // 是否是目标
                    {
                        EditorGUI.LabelField(leftHalf, "IsTarget");
                        GUI.enabled = false;
                        EditorGUI.Toggle(rightHalf, IsTarget.boolValue);
                        GUI.enabled = true;

                        rect.y += EditorGUIUtility.singleLineHeight;
                    }
                    // TODO::描绘贴图
                    {

                    }
                    // 现在容量
                    {
                        leftHalf.y = rect.y;
                        rightHalf.y = rect.y;

                        EditorGUI.LabelField(leftHalf, "CurrentAmount");
                        CurrentAmount.intValue = EditorGUI.IntSlider(rightHalf, CurrentAmount.intValue, 0, MaxAmount.intValue);
                        rect.y += EditorGUIUtility.singleLineHeight;
                    }
                    // 容量满的时候
                    {
                        leftHalf.y = rect.y;
                        rightHalf.y = rect.y;

                        EditorGUI.LabelField(leftHalf, "MaxAmount");
                        MaxAmount.intValue = EditorGUI.IntField(rightHalf, MaxAmount.intValue);
                        MaxAmount.intValue = Mathf.Max(0, MaxAmount.intValue);
                    }
                };

                bottleDataReorderableList.drawElementBackgroundCallback = (rect, index, active, focused) =>
                {
                    Texture2D tex = new Texture2D(1, 1);
                    if (active)
                    {
                        tex.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.8f));
                    }
                    else
                    {
                        SerializedProperty property = this.bottleDataReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                        var IsTarget = property.FindPropertyRelative("IsTarget");
                        if (IsTarget.boolValue)
                        {
                            tex.SetPixel(0, 0, new Color(0, 0.8f, 0));
                        }
                        else
                        {
                            tex.SetPixel(0, 0, Color.white);
                        }
                    }

                    tex.Apply();
                    GUI.DrawTexture(rect, tex as Texture);
                };
            }
            this.bottleDataReorderableList.DoLayoutList();
        }
    }
}