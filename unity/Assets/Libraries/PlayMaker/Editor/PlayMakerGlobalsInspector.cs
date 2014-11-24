// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HutongGames.PlayMakerEditor;

[CustomEditor(typeof(PlayMakerGlobals))]
class PlayMakerGlobalsInspector : Editor
{
	private PlayMakerGlobals globals;
	private bool refresh;

	private List<FsmVariable> variableList;

#if UNITY_3_4
	private GUIStyle warningBox;
#endif

	void OnEnable()
	{
		//Debug.Log("PlayMakerGlobalsInspector: OnEnable");

		globals = target as PlayMakerGlobals;

		BuildVariableList();
	}

	public override void OnInspectorGUI()
	{
#if UNITY_3_4
		if (warningBox == null)
		{
			warningBox = new GUIStyle(EditorStyles.boldLabel) {wordWrap = true};
        }

        GUILayout.Label(Strings.Hint_GlobalsInspector_Shows_DEFAULT_Values, warningBox);
#else
        EditorGUILayout.HelpBox(Strings.Hint_GlobalsInspector_Shows_DEFAULT_Values, MessageType.Info);
#endif
	
		if (refresh)
		{
			Refresh();
			return;
		}

		GUILayout.Label(Strings.Command_Global_Variables, EditorStyles.boldLabel);

		if (variableList.Count > 0)
		{
			foreach (var fsmVariable in variableList)
			{
				var tooltip = fsmVariable.Name;

				if (!string.IsNullOrEmpty(fsmVariable.Tooltip))
				{
					tooltip += "\n" + fsmVariable.Tooltip;
				}

				fsmVariable.DoValueGUI(new GUIContent(fsmVariable.Name, tooltip), true);
			}
		}
		else
		{
			GUILayout.Label(Strings.Label_None_In_Table);
		}

		GUILayout.Label(Strings.Label_Global_Events, EditorStyles.boldLabel);

		if (globals.Events.Count > 0)
		{
			foreach (var eventName in globals.Events)
			{
				GUILayout.Label(eventName);
			}
		}
		else
		{
			GUILayout.Label(Strings.Label_None_In_Table);
		}

        GUILayout.Space(5);

        if (GUILayout.Button(Strings.Command_Export_Globals))
        {
            FsmEditorUtility.ExportGlobals();
        }

        if (GUILayout.Button(Strings.Command_Import_Globals))
        {
            FsmEditorUtility.ImportGlobals();
        }
#if UNITY_3_4
        GUILayout.Label(Strings.Hint_Export_Globals_Notes, warningBox);
#else
        EditorGUILayout.HelpBox(Strings.Hint_Export_Globals_Notes, MessageType.None);
#endif
	}

	void Refresh()
	{
		refresh = false;
		BuildVariableList();
		Repaint();
	}

	void BuildVariableList()
	{
		variableList = FsmVariable.GetFsmVariableList(globals.Variables, globals);
		variableList.Sort();
	}
}
