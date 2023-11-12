using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using FuzzPhyte.Utility;
using UEditor = UnityEditor.Editor;
using EditorUtil = FuzzPhyte.Utility.Editor.FP_Utility_Editor;

namespace FuzzPhyte.Chain.Editor
{
    [CustomEditor(typeof(SequenceItem))]
    public class SequenceItemEditor : UEditor
    {
        SequenceItem mySequenceItem;
        SerializedProperty m_nextSequences;
        SerializedProperty m_requiredSequences;
        SerializedProperty m_unlockEvent;
        SerializedProperty m_activeEvent;
        SerializedProperty m_lockEvent;
        SerializedProperty m_endEvent;
        Color m_normal;
        private void OnEnable()
        {
            m_normal = GUI.contentColor;
            m_nextSequences = serializedObject.FindProperty("NextSequences");
            m_requiredSequences = serializedObject.FindProperty("RequiredSequences");
            m_unlockEvent = serializedObject.FindProperty("UnlockEvent");
            m_activeEvent = serializedObject.FindProperty("ActiveEvent");
            m_lockEvent = serializedObject.FindProperty("LockEvent");
            m_endEvent = serializedObject.FindProperty("EndEvent");
            mySequenceItem = (SequenceItem)target;
        }
        
        public void OnSceneGUI()
        {
        }
        public override void OnInspectorGUI()
        {
            if (mySequenceItem.DataReference == null)
            {
                GUI.contentColor = EditorUtil.WarningColor;
                EditorGUILayout.LabelField("ERROR: Need a Sequence-Details File", EditorUtil.ReturnStyle(EditorUtil.WarningColor, FontStyle.Bold, TextAnchor.LowerLeft));
            }
            else
            {
                GUI.contentColor = m_normal;
                EditorGUILayout.LabelField($"Sequence Name: {mySequenceItem.DataReference.SequenceUniqueName}", EditorUtil.ReturnStyle(Color.green, FontStyle.Bold, TextAnchor.LowerLeft));
                
            }
            mySequenceItem.DataReference = (SequenceDetails)EditorGUILayout.ObjectField(" Data Reference:", mySequenceItem.DataReference, typeof(SequenceDetails), true);

            if (mySequenceItem.DataReference != null)
            {
                serializedObject.Update();
                //show the rest of the options
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                mySequenceItem.Status = (SequenceStatus)EditorGUILayout.EnumPopup("Sequence Status:", mySequenceItem.Status);

                EditorGUILayout.Space();

                Rect curRect = GUILayoutUtility.GetLastRect();
                
                EditorGUI.indentLevel++;

                Color statusColor = EditorUtil.ReturnColorByStatus(mySequenceItem.Status);
                string htmlColor = ColorUtility.ToHtmlStringRGBA(statusColor);
                
                EditorGUILayout.LabelField("Sequence Related Items",EditorUtil.ReturnStyle(statusColor, FontStyle.BoldAndItalic,TextAnchor.MiddleLeft));
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_nextSequences, new GUIContent("Next Sequences"));
                EditorGUILayout.PropertyField(m_requiredSequences, new GUIContent("Required Sequences"));
                
                Rect endRect = GUILayoutUtility.GetLastRect();
                Rect newRect = new Rect(curRect.min.x, curRect.min.y, curRect.width, endRect.max.y - curRect.min.y);
                EditorUtil.DrawUIBox(newRect, 0, statusColor);
                EditorGUI.indentLevel = 0;
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("State Pathways", EditorUtil.ReturnStyle(m_normal, FontStyle.Bold, TextAnchor.MiddleCenter));
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField($"Standard Pathway: <color=#{htmlColor}> None -> Unlock -> Active -> Finished</color>", 
                    EditorUtil.ReturnStyleRichText(m_normal, FontStyle.Normal, TextAnchor.MiddleCenter));
                EditorGUILayout.LabelField($"Full Pathways: <color=#{htmlColor}>None -> Unlock <-> Locked <-> Active ->Finished</color>", 
                    EditorUtil.ReturnStyleRichText(statusColor, FontStyle.Normal, TextAnchor.MiddleCenter));
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);
                string htmlColorAuto = mySequenceItem.DataReference.AutomaticUnlockToActive ? ColorUtility.ToHtmlStringRGBA(EditorUtil.OkayColor) : ColorUtility.ToHtmlStringRGBA(EditorUtil.WarningColor);
                string htmlColorUseRequirements = mySequenceItem.DataReference.UseSequenceRequirements? ColorUtility.ToHtmlStringRGBA(EditorUtil.OkayColor) : ColorUtility.ToHtmlStringRGBA(EditorUtil.WarningColor);
                string htmlFirstSequence = mySequenceItem.DataReference.FirstSequence ? ColorUtility.ToHtmlStringRGBA(EditorUtil.OkayColor) : ColorUtility.ToHtmlStringRGBA(EditorUtil.WarningColor);
                string htmlEndSequence = mySequenceItem.DataReference.LastSequence? ColorUtility.ToHtmlStringRGBA(EditorUtil.OkayColor) : ColorUtility.ToHtmlStringRGBA(EditorUtil.WarningColor);
                EditorGUILayout.LabelField(
                    $"<b><color=#{htmlFirstSequence}>{mySequenceItem.DataReference.FirstSequence}</color></b>:\tFirst Sequence",
                    EditorUtil.ReturnStyleRichText(m_normal, FontStyle.Normal, TextAnchor.UpperLeft));
                EditorGUILayout.LabelField(
                    $"<b><color=#{htmlEndSequence}>{mySequenceItem.DataReference.LastSequence}</color></b>:\tEnd Sequence",
                    EditorUtil.ReturnStyleRichText(m_normal, FontStyle.Normal, TextAnchor.UpperLeft));
                EditorGUILayout.LabelField(
                    $"<b><color=#{htmlColorUseRequirements}>{mySequenceItem.DataReference.UseSequenceRequirements}</color></b>:\tUse Sequence Requirements before Unlock",
                    EditorUtil.ReturnStyleRichText(m_normal, FontStyle.Normal, TextAnchor.UpperLeft));
                EditorGUILayout.LabelField(
                    $"<b><color=#{htmlColorAuto}>{mySequenceItem.DataReference.AutomaticUnlockToActive}</color></b>:\tAutomatic Unlock to Active State",
                    EditorUtil.ReturnStyleRichText(m_normal,FontStyle.Normal,TextAnchor.UpperLeft));
                EditorGUILayout.Space(30);
               
                mySequenceItem.UseEvents = EditorGUI.Foldout(GUILayoutUtility.GetLastRect(), mySequenceItem.UseEvents, "Use Events");
               
                if (mySequenceItem.UseEvents)
                {
                    EditorGUI.indentLevel++;
                    mySequenceItem.UseUnlockEvent = EditorGUILayout.ToggleLeft(
                        new GUIContent("Unlock Event?","Unity events to invoke when we hit the unlock state"),
                        mySequenceItem.UseUnlockEvent, 
                        EditorStyles.boldLabel);
                    mySequenceItem.UseActiveEvent = EditorGUILayout.ToggleLeft(
                        new GUIContent("Active Event?","Unity events to invoke when we hit the active state"), 
                        mySequenceItem.UseActiveEvent, 
                        EditorStyles.boldLabel);
                    mySequenceItem.UseLockEvent = EditorGUILayout.ToggleLeft(
                        new GUIContent("Lock Event?","Unity events to invoke when we hit the locked state"), 
                        mySequenceItem.UseLockEvent, 
                        EditorStyles.boldLabel);
                    mySequenceItem.UseEndEvent = EditorGUILayout.ToggleLeft(
                        new GUIContent("End Event?","Unity events to invoke when we hit the finished state"), 
                        mySequenceItem.UseEndEvent, 
                        EditorStyles.boldLabel);
                    EditorGUI.indentLevel = 2;
                    if (mySequenceItem.UseUnlockEvent)
                    {
                        EditorGUILayout.PropertyField(m_unlockEvent, false);
                    }
                    if (mySequenceItem.UseActiveEvent)
                    {
                        EditorGUILayout.PropertyField(m_activeEvent, false);
                    }
                    if (mySequenceItem.UseLockEvent)
                    {
                        EditorGUILayout.PropertyField(m_lockEvent, false);
                    }
                    if (mySequenceItem.UseEndEvent)
                    {
                        EditorGUILayout.PropertyField(m_endEvent, false);
                    }
                }
            }
            
            base.serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(mySequenceItem.gameObject);
                if (!Application.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(mySequenceItem.gameObject.scene);
                }
                
            }
        }
    }

}
