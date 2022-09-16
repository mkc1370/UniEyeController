using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Extensions
{
    /// <summary>
    /// ref : https://tips.hecomi.com/entry/2016/10/15/004144
    /// </summary>
    public static class CustomUI
    {
        public static void FoldoutWithToggle(string title, ref bool isFoldout, ref bool enable)
        {
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.label).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f + 17f + 4f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var foldoutToggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint) {
                EditorStyles.foldout.Draw(foldoutToggleRect, false, false, isFoldout, false);
            }
            
            var toggleRect = new Rect(rect.x + 13f + 4f + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint) {
                GUI.Toggle(toggleRect, enable, GUIContent.none, new GUIStyle("ShurikenToggle"));
            }

            if (e.type == EventType.MouseDown && toggleRect.Contains(e.mousePosition)) {
                enable = !enable;
                e.Use();
                return;
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition)) {
                isFoldout = !isFoldout;
                e.Use();
                return;
            }
        }
        
        public static void Foldout(string title, ref bool isFoldout)
        {
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.label).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint) {
                EditorStyles.foldout.Draw(toggleRect, false, false, isFoldout, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition)) {
                isFoldout = !isFoldout;
                e.Use();
            }
        }
    }
}