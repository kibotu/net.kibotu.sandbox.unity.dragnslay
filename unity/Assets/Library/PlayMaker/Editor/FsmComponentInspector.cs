// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEditor;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using System.Collections.Generic;

/// <summary>
/// Custom inspector for PlayMakerFSM
/// </summary>
[CustomEditor(typeof(PlayMakerFSM))]
public class FsmComponentInspector : Editor
{
    private static GUIContent restartOnEnableLabel = new GUIContent(Strings.Label_Reset_On_Disable, Strings.Tooltip_Reset_On_Disable);
    private static GUIContent showStateLabelLabel = new GUIContent(Strings.Label_Show_State_Label, Strings.Tooltip_Show_State_Label);
    private static GUIContent enableDebugFlowLabel = new GUIContent(Strings.FsmEditorSettings_Enable_DebugFlow, Strings.FsmEditorSettings_Enable_DebugFlow_Tooltip);
    //private static GUIContent quickLoadLabel = new GUIContent("Quick Load", "Skip data validation when loading FSM. Faster, but can fail if actions have changed since FSM was saved.");

    // Inspector targets

    private PlayMakerFSM fsmComponent;   // Inspector target
    private FsmTemplate fsmTemplate;     // template used by fsmComponent

    // Inspector foldout states

    private bool showControls = true;
    private bool showInfo;
    private bool showStates;
    private bool showEvents;
    private bool showVariables;

    // Collect easily editable references to fsmComponent.Fsm.Variables

    List<FsmVariable> fsmVariables = new List<FsmVariable>();

    public void OnEnable()
    {
        fsmComponent = target as PlayMakerFSM;
        if (fsmComponent == null) return; // shouldn't happen

        fsmTemplate = fsmComponent.FsmTemplate;
            
        RefreshTemplate();
        BuildFsmVariableList();
    }

    public override void OnInspectorGUI()
    {
        if (fsmComponent == null) return; // shouldn't happen

        FsmEditorStyles.Init();

        var fsm = fsmComponent.Fsm;  // grab Fsm for convenience

        if (fsm.States.Length > 100) // a little arbitrary, but better than nothing!
        {
            EditorGUILayout.HelpBox("NOTE: Collapse this inspector for better editor performance with large FSMs.", MessageType.None);
        }

        // Edit FSM name

        EditorGUILayout.BeginHorizontal(); 

        fsm.Name = EditorGUILayout.TextField(fsm.Name);

        if (GUILayout.Button(new GUIContent(Strings.Label_Edit, Strings.Tooltip_Edit_in_the_PlayMaker_Editor), GUILayout.MaxWidth(45)))
        {
            OpenInEditor(fsmComponent);
            GUIUtility.ExitGUI();
        }

        EditorGUILayout.EndHorizontal(); 

        // Edit FSM Template

        EditorGUILayout.BeginHorizontal();
        
        var template = (FsmTemplate)
            EditorGUILayout.ObjectField(new GUIContent(Strings.Label_Use_Template, Strings.Tooltip_Use_Template),
                fsmComponent.FsmTemplate, typeof(FsmTemplate), false);

        if (template != fsmComponent.FsmTemplate)
        {
            SelectTemplate(template);
        }

        if (GUILayout.Button(new GUIContent(Strings.Label_Browse, Strings.Tooltip_Browse_Templates), GUILayout.MaxWidth(45)))
        {
            DoSelectTemplateMenu();
        }

        EditorGUILayout.EndHorizontal();

        // Disable GUI that can't be edited if referencing a template

        if (!Application.isPlaying && fsmComponent.FsmTemplate != null)
        {
            template = fsmComponent.FsmTemplate;
            fsm = template.fsm;

            GUI.enabled = false;
        }

        // Resave warning
        /*
        if (fsm.needsResave)
        {
            EditorGUI.BeginDisabledGroup(false);
            EditorGUILayout.HelpBox("NOTE: Action data has changed since FSM was saved. Please resave FSM to update actions.", MessageType.Warning);
            EditorGUI.EndDisabledGroup();
        }*/

        // Edit Description

        fsm.Description = FsmEditorGUILayout.TextAreaWithHint(fsm.Description, Strings.Label_Description___, GUILayout.MinHeight(60));

        // Edit Help Url (lets the user link to documentation for the FSM)

        EditorGUILayout.BeginHorizontal();

        fsm.DocUrl = FsmEditorGUILayout.TextFieldWithHint(fsm.DocUrl, Strings.Tooltip_Documentation_Url);

        var guiEnabled = GUI.enabled;

        GUI.enabled = !string.IsNullOrEmpty(fsm.DocUrl);

        if (FsmEditorGUILayout.HelpButton())
        {
            Application.OpenURL(fsm.DocUrl);
        }

        EditorGUILayout.EndHorizontal();

        GUI.enabled = guiEnabled;

        // Edit FSM Settings
        
        fsm.RestartOnEnable = GUILayout.Toggle(fsm.RestartOnEnable, restartOnEnableLabel);
        fsm.ShowStateLabel = GUILayout.Toggle(fsm.ShowStateLabel, showStateLabelLabel);
        fsm.EnableDebugFlow = GUILayout.Toggle(fsm.EnableDebugFlow, enableDebugFlowLabel);
        //fsm.QuickLoad = GUILayout.Toggle(fsm.QuickLoad, quickLoadLabel);

        // The rest of the GUI is readonly so we can check for changes here

        if (GUI.changed)
        {
            EditorUtility.SetDirty(fsmComponent);
        }

        // Controls Section

        GUI.enabled = true;

        // Show FSM variables with Inspector option checked

        FsmEditorGUILayout.LightDivider();
        showControls = EditorGUILayout.Foldout(showControls, new GUIContent(Strings.Label_Controls, Strings.Tooltip_Controls), FsmEditorStyles.CategoryFoldout);

        if (showControls)
        {
            //EditorGUIUtility.LookLikeInspector();

            BuildFsmVariableList();

            foreach (var fsmVar in fsmVariables)
            {
                if (fsmVar.ShowInInspector)
                {
                    EditorGUI.BeginChangeCheck();
                    fsmVar.DoValueGUI(new GUIContent(fsmVar.Name, fsmVar.Name + (!string.IsNullOrEmpty(fsmVar.Tooltip) ? ":\n" + fsmVar.Tooltip : "")));
                    if (EditorGUI.EndChangeCheck())
                    {
                        fsmVar.UpdateVariableValue();
                    }
                }
            }

            if (GUI.changed)
            {
                FsmEditor.RepaintAll();
            }
        }

        // Show events with Inspector option checked
        // These become buttons that the user can press to send the events

        if (showControls)
        {
            foreach (var fsmEvent in fsm.ExposedEvents)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(fsmEvent.Name);
                if (GUILayout.Button(fsmEvent.Name))
                {
                    fsm.Event(fsmEvent);
                    FsmEditor.RepaintAll();
                }
                GUILayout.EndHorizontal();
            }
        }

        // Show general info about the FSM

        EditorGUI.indentLevel = 0;

        FsmEditorGUILayout.LightDivider();
        showInfo = EditorGUILayout.Foldout(showInfo, Strings.Label_Info, FsmEditorStyles.CategoryFoldout);

        if (showInfo)
        {
            EditorGUI.indentLevel = 1;

            showStates = EditorGUILayout.Foldout(showStates, string.Format(Strings.Label_States_Count, fsm.States.Length));
            if (showStates)
            {
                string states = "";

                if (fsm.States.Length > 0)
                {
                    foreach (var state in fsm.States)
                    {
                        states += FsmEditorStyles.tab2 + state.Name + FsmEditorStyles.newline;
                    }
                    states = states.Substring(0, states.Length - 1);
                }
                else
                {
                    states = FsmEditorStyles.tab2 + Strings.Label_None_In_Table;
                }

                GUILayout.Label(states);
            }

            showEvents = EditorGUILayout.Foldout(showEvents, string.Format(Strings.Label_Events_Count, fsm.Events.Length));
            if (showEvents)
            {
                var events = "";

                if (fsm.Events.Length > 0)
                {
                    foreach (var fsmEvent in fsm.Events)
                    {
                        events += FsmEditorStyles.tab2 + fsmEvent.Name + FsmEditorStyles.newline;
                    }
                    events = events.Substring(0, events.Length - 1);
                }
                else
                {
                    events = FsmEditorStyles.tab2 + Strings.Label_None_In_Table;
                }

                GUILayout.Label(events);
            }

            showVariables = EditorGUILayout.Foldout(showVariables, string.Format(Strings.Label_Variables_Count, fsmVariables.Count));
            if (showVariables)
            {
                var variables = "";

                if (fsmVariables.Count > 0)
                {
                    foreach (var fsmVar in fsmVariables)
                    {
                        variables += FsmEditorStyles.tab2 + fsmVar.Name + FsmEditorStyles.newline;
                    }
                    variables = variables.Substring(0, variables.Length - 1);
                }
                else
                {
                    variables = FsmEditorStyles.tab2 + Strings.Label_None_In_Table;
                }

                GUILayout.Label(variables);
            }
        }

        // Manual refresh if template has been edited

        if (fsmTemplate != null)
        {
            if (GUILayout.Button(new GUIContent("Refresh Template", "Use this if you've updated the template but don't see the changes here.")))
            {
                RefreshTemplate();
            }
        }
    }

    /// <summary>
    /// Open the specified FSM in the Playmaker Editor
    /// </summary>
    public static void OpenInEditor(PlayMakerFSM fsmComponent)
    {
        if (FsmEditor.Instance == null)
        {
            FsmEditorWindow.OpenWindow(fsmComponent);
        }
        else
        {
            FsmEditor.SelectFsm(fsmComponent.FsmTemplate == null ? fsmComponent.Fsm : fsmComponent.FsmTemplate.fsm);
        }
    }

    /// <summary>
    /// Open the specified FSM in the Playmaker Editor
    /// </summary>
    public static void OpenInEditor(Fsm fsm)
    {
        if (fsm.Owner != null)
        {
            OpenInEditor(fsm.Owner as PlayMakerFSM);
        }
    }

    /// <summary>
    /// Open the first PlayMakerFSM on a GameObject in the Playmaker Editor
    /// </summary>
    public static void OpenInEditor(GameObject go)
    {
        if (go != null)
        {
            OpenInEditor(FsmEditorUtility.FindFsmOnGameObject(go));
        }
    }
    
    /// <summary>
    /// The fsmVariables list contains easily editable references to FSM variables
    /// (Similar in concept to SerializedProperty)
    /// </summary>
    void BuildFsmVariableList()
    {
        fsmVariables = FsmVariable.GetFsmVariableList(fsmComponent.Fsm.Variables, target);
        fsmVariables.Sort();
    }

    #region Templates

    void SelectTemplate(object userdata)
    {
        SelectTemplate(userdata as FsmTemplate);
    }

    void SelectTemplate(FsmTemplate template)
    {
        if (template == fsmComponent.FsmTemplate)
        {
            return; // don't want to lose overridden variables
        }

        fsmComponent.SetFsmTemplate(template);
        fsmTemplate = template;

        BuildFsmVariableList();

        EditorUtility.SetDirty(fsmComponent);

        FsmEditor.RefreshInspector(); // Keep Playmaker Editor in sync
    }

    void ClearTemplate()
    {
        fsmComponent.Reset();
        fsmTemplate = null;

        BuildFsmVariableList();

        // If we were editing the template in the Playmaker editor
        // handle this gracefully by reselecting the base FSM

        if (FsmEditor.SelectedFsmComponent == fsmComponent)
        {
            FsmEditor.SelectFsm(fsmComponent.Fsm);
        }
    }

    /// <summary>
    /// A template can change since it was selected.
    /// This method refreshes the UI to reflect any changes
    /// while keeping any variable overrides that the use has made
    /// </summary>
    void RefreshTemplate()
    {
        if (fsmTemplate == null || Application.isPlaying)
        {
            return;
        }

        // we want to keep the existing overrides
        // so we copy the current FsmVariables

        var currentValues = new FsmVariables(fsmComponent.Fsm.Variables);
        
        // then we update the template

        fsmComponent.SetFsmTemplate(fsmTemplate);

        // finally we apply the original overrides back to the new FsmVariables

        fsmComponent.Fsm.Variables.OverrideVariableValues(currentValues);

        // and refresh the UI
        
        BuildFsmVariableList();

        FsmEditor.RefreshInspector();
    }

    void DoSelectTemplateMenu()
    {
        var menu = new GenericMenu();

        var templates = (FsmTemplate[])Resources.FindObjectsOfTypeAll(typeof(FsmTemplate));

        menu.AddItem(new GUIContent(Strings.Menu_None), false, ClearTemplate);

        foreach (var template in templates)
        {
            const string submenu = "/";
            menu.AddItem(new GUIContent(template.Category + submenu + template.name), fsmComponent.FsmTemplate == template, SelectTemplate, template);
        }

        menu.ShowAsContext();
    }

    #endregion

    /// <summary>
    /// Actions can use OnSceneGUI to display interactive gizmos
    /// </summary>
    public void OnSceneGUI()
    {
        FsmEditor.OnSceneGUI();
    }
}


