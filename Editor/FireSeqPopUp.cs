using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FuzzPhyte.Chain
{
    public class FireSeqPopUp : PopupWindowContent
    {
        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 100);
        }
        public override void OnGUI(Rect rect)
        {

            GUILayout.Label("Reset other Fire Sequences can only have 1 OnAwake Fire Sequence", EditorUtil.ReturnStyleWrap(EditorUtil.WarningColor,FontStyle.BoldAndItalic,TextAnchor.UpperCenter,true));
            if (GUILayout.Button("Close"))
            {
                this.editorWindow.Close();
            }
        }
        public override void OnOpen()
        {
            Debug.Log($"Reset Fire Sequence");
        }
        public override void OnClose()
        {
            base.OnClose();
        }
    }

}
