using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor.AnimatedValues;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace COAD_Delta
{
    public class ComponentTool : MonoBehaviour
    {
    }

    public class ComponentToolEditor : EditorWindow
    {

        Component component;
        private Vector2 pos;
        private Vector2 pos2;

        List<GameObject> compList = new List<GameObject>();

        bool isOpen = false;

        bool endSearch = false;

        [MenuItem("Tools/COAD_Delta/UtilTool/ComponentTool", false)]
        static void CreateWindow()
        {
            var asm = Assembly.Load("UnityEditor.dll");
            var typeWindow = asm.GetType("UnityEditor.SceneView");
            EditorWindow.GetWindow<ComponentToolEditor>("ComponentTool", typeWindow);
        }

        void OnGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.wordWrap = true;

            GUILayout.Label("\nオブジェクトにアタッチされたコンポーネントに色々するツールです。\n" +
                "Hierarchy上のオブジェクトから「特定のコンポーネント」がアタッチされているか検索。\n" +
                "また検索したコンポーネントの一括削除が可能です。\n\n" +
                "※既知のバグとして「Redoする際に参照関係が維持されない」というものがあります。Undoは問題なく可能ですが、Redoは行わないようにしてください。\n", style);

            Rect splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
            splitterRect.x = 0;
            splitterRect.width = position.width;
            EditorGUI.DrawRect(splitterRect, Color.Lerp(Color.gray, Color.black, 0.7f));
            GUILayout.Space(15);
            GUILayout.Label("設定項目", EditorStyles.boldLabel);
            component = EditorGUILayout.ObjectField("Target Component", component, typeof(Component), true) as Component;
            GUILayout.Space(15);

            if (component != null)
            {
                isOpen = EditorGUILayout.BeginFoldoutHeaderGroup(isOpen, "Properties");

                if (isOpen)
                {

                    using (var scrollScope = new EditorGUILayout.ScrollViewScope(pos, GUI.skin.box))
                    {
                        var serializedObject = new SerializedObject(component);
                        var iterator = serializedObject.GetIterator();
                        while (iterator.NextVisible(true))
                        {
                            EditorGUILayout.LabelField(iterator.propertyPath);
                        }

                        pos = scrollScope.scrollPosition;
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            EditorGUI.BeginDisabledGroup(component == null);

            if (GUILayout.Button("Component Search"))
            {
                ComponentSearch();
            }

            EditorGUI.EndDisabledGroup();

            if (endSearch)
            {
                using (var scrollScope = new EditorGUILayout.ScrollViewScope(pos2, GUI.skin.box))
                {
                    int i = 0;
                    foreach(var comp in compList)
                    {
                        EditorGUILayout.LabelField(i + "   " + comp.gameObject.ToString());
                        i++;
                    }

                    pos2 = scrollScope.scrollPosition;
                }
            }

            EditorGUI.BeginDisabledGroup(!endSearch);

            if (GUILayout.Button("Remove"))
            {
                ComponentRemove();
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.Space(15);
            Rect splitterRect2 = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
            splitterRect2.x = 0;
            splitterRect2.width = position.width;
            EditorGUI.DrawRect(splitterRect2, Color.Lerp(Color.gray, Color.black, 0.7f));
            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Refresh"))
            {
                component = null;
                compList.Clear();
                isOpen = false;
                endSearch = false;
                pos = new Vector2();
                pos2 = new Vector2();
            }

            if (GUILayout.Button("Close"))
            {
                Close();
            }

            EditorGUILayout.EndHorizontal();



        }

        private void ComponentRemove()
        {
            foreach(var compObj in compList)
            {
                var comp = compObj.GetComponent(component.GetType());
                Undo.RegisterCompleteObjectUndo(comp, "Remove Component");
                DestroyImmediate(comp);
                

            }
        }

        private void ComponentSearch()
        {
            compList = new List<GameObject>();

            foreach (var gameObject in (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]))
            {
                if (gameObject.GetComponent(component.GetType()) != null) {
                    compList.Add(gameObject);
                }
            }

            Selection.objects = compList.ToArray();
            endSearch = true;
        }
    }
}