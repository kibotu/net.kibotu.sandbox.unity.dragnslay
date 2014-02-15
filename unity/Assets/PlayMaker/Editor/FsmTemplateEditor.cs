using System.ComponentModel;
using HutongGames.PlayMakerEditor;
using UnityEngine;
using UnityEditor;

#if !UNITY_3_4
[CanEditMultipleObjects]
#endif
[CustomEditor(typeof(FsmTemplate))]
public class FsmTemplateEditor : Editor
{

#if UNITY_3_4    
    // serializedObject was added to Editor in 3.5    
    private SerializedObject serializedObject;
#endif

    private SerializedProperty categoryProperty;
    private SerializedProperty descriptionProperty;

    [Localizable(false)]
    public void OnEnable()
    {
#if UNITY_3_4
        serializedObject = new SerializedObject(target);
#endif
        categoryProperty = serializedObject.FindProperty("category");
        descriptionProperty = serializedObject.FindProperty("fsm.description");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(categoryProperty);

        descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue, GUILayout.MinHeight(60));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button(Strings.FsmTemplateEditor_Open_In_Editor))
        {
            FsmEditorWindow.OpenWindow((FsmTemplate) target);
        }

#if !UNITY_3_4
        EditorGUILayout.HelpBox(Strings.Hint_Exporting_Templates, MessageType.None );
#endif
    }
}
