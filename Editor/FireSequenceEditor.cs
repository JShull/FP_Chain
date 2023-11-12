using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using FuzzPhyte.Utility;
using UEditor = UnityEditor.Editor;
using EditorUtil = FuzzPhyte.Utility.Editor.FP_Utility_Editor;

namespace FuzzPhyte.Chain.Editor
{
    [Serializable]
    [CustomEditor(typeof(FireSequence))]
    public class FireSequenceEditor : UEditor
    {
        //bool m_showDataFile;
        //bool m_showRequirements;
        Vector2 textAreaScroll;
        FireSequence myFireSequence;

        public override void OnInspectorGUI()
        {
           
            //DrawDefaultInspector();
            //base.OnInspectorGUI();
            myFireSequence = (FireSequence)target;
            myFireSequence.AwakeSetupSequences = EditorGUILayout.ToggleLeft("Awake Setup:", myFireSequence.AwakeSetupSequences);
            myFireSequence.FireOnStart = EditorGUILayout.ToggleLeft("Activate on Start:", myFireSequence.FireOnStart);
            //Turn off the other Fire Sequence
            if (myFireSequence.AwakeSetupSequences)
            {
                //go find all of the other ones and turn them off
                var allFireItems = UnityEngine.Object.FindObjectsOfType(typeof(FireSequence)) as FireSequence[];
                
                for (int i = 0; i < allFireItems.Length; i++)
                {
                    var item = allFireItems[i];
                    if (item != myFireSequence)
                    {
                        if (item.AwakeSetupSequences)
                        {
                            item.AwakeSetupSequences = false;
                            EditorUtility.SetDirty(item.gameObject);
                            PopupWindow.Show(GUILayoutUtility.GetLastRect(), new FireSeqPopUp());
                            //EditorGUILayout.LabelField($"Turned off Awake on the other FireSequence.cs {item.gameObject.name}", EditorStyles.wordWrappedLabel);
                           
                        }
                        
                    }
                }
            }
            EditorGUILayout.Space();
            EditorUtil.DrawUILine(Color.white);

            myFireSequence.UseDataFile = EditorGUILayout.ToggleLeft("Use Data File:", myFireSequence.UseDataFile, EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            if (myFireSequence.UseDataFile)
            { 
                myFireSequence.DataRef = (SequenceDetails)EditorGUILayout.ObjectField(" Data Reference:", myFireSequence.DataRef,typeof(SequenceDetails),true);
                myFireSequence.SequenceName = "";
                myFireSequence.SequenceChapter = "";
            }
            else
            {
                myFireSequence.SequenceName = EditorGUILayout.TextField("Sequence Name:", myFireSequence.SequenceName);
                myFireSequence.SequenceChapter = EditorGUILayout.TextField("Sequence Chapter:", myFireSequence.SequenceChapter);
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
            myFireSequence.UseRequirementFile = EditorGUILayout.ToggleLeft("Use Requirements File:", myFireSequence.UseRequirementFile, EditorStyles.boldLabel);
            if (myFireSequence.UseRequirementFile)
            {
                EditorGUI.indentLevel++;
                myFireSequence.PossibleRequirement = (SequenceItem)EditorGUILayout.ObjectField("Requirement:", myFireSequence.PossibleRequirement, typeof(SequenceItem),true);
            }
            else
            {
                myFireSequence.PossibleRequirement = null;
            }
            if (myFireSequence.UseRequirementFile)
            {
                EditorGUI.indentLevel--;
            }
            myFireSequence.LockStatus = (SequenceStatus)EditorGUILayout.EnumPopup("Sequence Status:", myFireSequence.LockStatus);
            
            EditorGUILayout.Separator();
            //EditorUtility.DrawUILine(Color.red);
            GUI.contentColor = EditorUtil.WarningColor;
            
            EditorGUILayout.LabelField("NOTES", EditorStyles.miniBoldLabel);
            textAreaScroll = EditorGUILayout.BeginScrollView(textAreaScroll);
            myFireSequence.FireSequenceNotes = EditorGUILayout.TextArea(myFireSequence.FireSequenceNotes,GUILayout.Height(50));
            EditorGUILayout.EndScrollView();
            base.serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                
                EditorUtility.SetDirty(myFireSequence.gameObject);
                EditorSceneManager.MarkSceneDirty(myFireSequence.gameObject.scene);
                
            }
            
        }   
    }
}
