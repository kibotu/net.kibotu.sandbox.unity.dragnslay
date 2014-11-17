using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ParticlePlayground;

class PlaygroundParticleWindowC : EditorWindow {
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// PlaygroundParticleWindow variables
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	// Paths
	public static string playgroundPath = "Particle Playground/";
	public static string examplePresetPath = "Playground Assets/Presets/";
	public static string presetPath = "Resources/Presets/";
	public static string iconPath = "Graphics/Editor/Icons/";
	public static string brushPath = "Playground Assets/Brushes/";
	public static string scriptPath = "Scripts/";
	public static string versionUrl = "http://www.polyfied.com/products/playgroundversion.php";
	WWW versionRequest;
	string onlineVersion;

	// Presets
	public static List<PresetObjectC> presetObjects;
	public static bool particlePresetFoldout = true;
	public static int presetListStyle = 0;
	public static int presetExampleUser = 0;
	public static string[] presetNames;
	
	// Editor Window specific
	public static Vector2 scrollPosition;
	public static GUIStyle presetButtonStyle;
	public static string searchString = "";
	public static GUIStyle toolbarSearchSkin;
	public static GUIStyle toolbarSearchButtonSkin;
	bool didSendVersionCheck = false;
	bool updateAvailable = false;
	
	[MenuItem ("Window/Particle Playground")]
	public static void ShowWindow () {
		PlaygroundParticleWindowC window = GetWindow<PlaygroundParticleWindowC>();
		window.title = "Playground";
        window.Show();
	}
	
	public void OnEnable () {
		Initialize();
	}
	
	public void OnProjectChange () {
		Initialize();
	}
	
	public void OnFocus () {
		Initialize();
	}
	
	public void Initialize () {
		presetButtonStyle = new GUIStyle();
		presetButtonStyle.stretchWidth = true;
		presetButtonStyle.stretchHeight = true;

		List<Object> particlePrefabs = new List<Object>();

		// Get all user presets (bound to resources)
		int userPrefabCount = 0;
		Object[] resourcePrefabs = (Object[])Resources.LoadAll ("Presets", typeof(GameObject));
		foreach (Object thisResourcePrefab in resourcePrefabs) {
			particlePrefabs.Add (thisResourcePrefab);
			userPrefabCount++;
		}

		// Get all example presets
		string assetsDataPath = Application.dataPath;
		string editorPresetPath = assetsDataPath+"/"+playgroundPath+examplePresetPath;
		string[] editorPresetPaths = Directory.GetFiles (editorPresetPath);

		foreach (string thisPresetPath in editorPresetPaths) {
			string convertedPresetPath = thisPresetPath.Substring(assetsDataPath.Length-6);
			Object presetPathObject = (Object)AssetDatabase.LoadAssetAtPath(convertedPresetPath, typeof(Object));
			if (presetPathObject!=null && (presetPathObject.GetType().Name)=="GameObject") {
				particlePrefabs.Add (presetPathObject);
			}
		}

		Texture2D particleImageDefault = Resources.LoadAssetAtPath("Assets/"+playgroundPath+iconPath+"Default.png", typeof(Texture2D)) as Texture2D;
		Texture2D particleImage;
		
		presetObjects = new List<PresetObjectC>();
		int i = 0;
		for (; i<particlePrefabs.Count; i++) {
			presetObjects.Add(new PresetObjectC());
			presetObjects[i].presetObject = particlePrefabs[i];
			presetObjects[i].example = (i>=userPrefabCount);
			particleImage = Resources.LoadAssetAtPath("Assets/"+playgroundPath+iconPath+presetObjects[i].presetObject.name+".png", typeof(Texture2D)) as Texture2D;
			
			// Try the asset location if we didn't find it in regular editor folder
			if (particleImage==null) {
				particleImage = Resources.LoadAssetAtPath(Path.GetDirectoryName(AssetDatabase.GetAssetPath(presetObjects[i].presetObject as UnityEngine.Object))+"/"+presetObjects[i].presetObject.name+".png", typeof(Texture2D)) as Texture2D;
			}
			
			// Finally use the specified icon (or the default)
			if (particleImage!=null)
				presetObjects[i].presetImage = particleImage;
			else if (particleImageDefault!=null)
				presetObjects[i].presetImage = particleImageDefault;
		}
		presetNames = new string[presetObjects.Count];
		for (i = 0; i<presetNames.Length; i++) {
			presetNames[i] = presetObjects[i].presetObject.name;
			
			// Filter on previous search
			presetObjects[i].unfiltered = (searchString==""?true:presetNames[i].ToLower().Contains(searchString.ToLower()));
		}

		if (!didSendVersionCheck)
			versionRequest = new WWW(versionUrl);
	}

	void CheckUpdate () {

		// Check if an update is available
		if (!didSendVersionCheck) {
			if (versionRequest.isDone) {
				if (versionRequest.error==null) {
					onlineVersion = versionRequest.text;
					updateAvailable = (onlineVersion!="" && float.Parse (onlineVersion)>float.Parse (PlaygroundC.version));
				}
				didSendVersionCheck = true;
			}
		}
	}
	
	void OnGUI () {
		if (toolbarSearchSkin==null) {
			toolbarSearchSkin = GUI.skin.FindStyle("ToolbarSeachTextField");
			if (toolbarSearchButtonSkin==null)
				toolbarSearchButtonSkin = GUI.skin.FindStyle("ToolbarSeachCancelButton");
		}
		EditorGUILayout.BeginVertical();
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Particle Playground "+PlaygroundC.version+PlaygroundC.specialVersion, EditorStyles.largeLabel, GUILayout.Height(20));
		EditorGUILayout.Separator();

		// New version message
		if (!didSendVersionCheck)
			CheckUpdate();
		if (updateAvailable) {
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Update available");
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("x", EditorStyles.miniButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)})){
				updateAvailable = false;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.LabelField("Update "+onlineVersion+" is available. Please visit the Unity Asset Store to download the new version.", EditorStyles.wordWrappedMiniLabel);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Unity Asset Store", EditorStyles.toolbarButton, GUILayout.Width(100))){
				Application.OpenURL ("http://u3d.as/5ZJ");
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Separator();
		}
		EditorGUILayout.BeginVertical("box");
		
		// Create New-buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Particle Playground System", EditorStyles.toolbarButton)){
			if (PlaygroundC.reference==null)
				CreateManager();
			PlaygroundParticlesC newParticlePlayground = PlaygroundC.Particle();
			Selection.activeGameObject = newParticlePlayground.particleSystemGameObject;
		}
		GUI.enabled = PlaygroundC.reference==null;
		if(GUILayout.Button("Playground Manager", EditorStyles.toolbarButton)){
			PlaygroundC.ResourceInstantiate("Playground Manager");
		}
		GUI.enabled = true;
		if(GUILayout.Button("Preset Wizard", EditorStyles.toolbarButton)){
			PlaygroundCreatePresetWindowC.ShowWindow();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
		
		// Presets
		EditorGUILayout.BeginVertical("box");
		particlePresetFoldout = GUILayout.Toggle(particlePresetFoldout, "Presets", EditorStyles.foldout);
		if (particlePresetFoldout) {
			EditorGUILayout.BeginHorizontal("Toolbar");
			
			// Search
			string prevSearchString = searchString;
			searchString = GUILayout.TextField(searchString, toolbarSearchSkin, new GUILayoutOption[]{GUILayout.ExpandWidth(false), GUILayout.Width(Mathf.FloorToInt(Screen.width)-120), GUILayout.MinWidth(100)});
			if (GUILayout.Button("", toolbarSearchButtonSkin)) {
				searchString = "";
				GUI.FocusControl(null);
			}

			if (prevSearchString!=searchString) {
				for (int p = 0; p<presetNames.Length; p++)
					presetObjects[p].unfiltered = (searchString==""?true:presetNames[p].ToLower().Contains(searchString.ToLower()));
			}
			
			EditorGUILayout.Separator();
			presetExampleUser = GUILayout.Toolbar (presetExampleUser, new string[]{"All","User","Examples"}, EditorStyles.toolbarButton, GUILayout.MaxWidth(170));
			GUILayout.Label ("", EditorStyles.toolbarButton);
			presetListStyle = GUILayout.Toolbar (presetListStyle, new string[]{"Icons","List"}, EditorStyles.toolbarButton, GUILayout.MaxWidth(120));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical("box");
			
			if (presetObjects.Count>0) {
				if (presetListStyle==0) EditorGUILayout.BeginHorizontal();
				int rows = 1;
				int iconwidths = 0;
				int skippedPresets = 0;
				for (int i = 0; i<presetObjects.Count; i++) {
					
					// Filter out by search
					if (!presetObjects[i].unfiltered || presetExampleUser==2 && !presetObjects[i].example || presetExampleUser==1 && presetObjects[i].example) {
						skippedPresets++;
						continue;
					}
					// Preset Object were destroyed
					if (presetObjects[i].presetObject==null) {
						Initialize();
						return;
					}
					
					// List
					if (presetListStyle==1) {
						EditorGUILayout.BeginVertical("box", GUILayout.MinHeight(24));
						EditorGUILayout.BeginHorizontal();
						GUILayout.Label(i.ToString(), EditorStyles.miniLabel, new GUILayoutOption[]{GUILayout.Width(18)});
						EditorGUILayout.LabelField(presetObjects[i].example?"(Example)":"(User)", EditorStyles.miniLabel, new GUILayoutOption[]{GUILayout.Width(50)});
						if (GUILayout.Button (presetObjects[i].presetObject.name, EditorStyles.label)) {
							CreatePresetObject(i);
						}
						EditorGUILayout.Separator();
						if(GUILayout.Button(presetObjects[i].example?"Convert to User":"Convert to Example", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(106), GUILayout.Height(16)})){
							if (presetObjects[i].example) {
								AssetDatabase.MoveAsset (AssetDatabase.GetAssetPath(presetObjects[i].presetObject), "Assets/"+playgroundPath+presetPath+presetObjects[i].presetObject.name+".prefab");
							} else {
								AssetDatabase.MoveAsset (AssetDatabase.GetAssetPath(presetObjects[i].presetObject), "Assets/"+playgroundPath+examplePresetPath+presetObjects[i].presetObject.name+".prefab");
							}
						}
						if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							RemovePreset(presetObjects[i].presetObject);
						}
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.EndVertical();
					}
					
					
					if (presetListStyle==0) {
					
						// Break row for icons
						rows = Mathf.FloorToInt(Screen.width/322);
						iconwidths+=322;
						if (iconwidths>Screen.width && i>0) {
							iconwidths=322;
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.BeginHorizontal();
						}
						
						if (Screen.width>=644) {
							EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth (Mathf.CeilToInt(Screen.width/rows)-(44/rows)));
						} else
							EditorGUILayout.BeginVertical("box");
						EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(46));
						if(GUILayout.Button(presetObjects[i].presetImage, EditorStyles.miniButton, new GUILayoutOption[]{GUILayout.Width(44), GUILayout.Height(44)})){
							CreatePresetObject(i);
						}
						EditorGUILayout.BeginVertical();

						if (GUILayout.Button(presetObjects[i].presetObject.name, EditorStyles.label, GUILayout.Height(18)))
							CreatePresetObject(i);
						EditorGUILayout.LabelField(presetObjects[i].example?"Example":"User", EditorStyles.miniLabel);
						EditorGUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						EditorGUILayout.BeginVertical();

						if(GUILayout.Button("x", EditorStyles.miniButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)})){
							RemovePreset(presetObjects[i].presetObject);
						}
						EditorGUILayout.EndVertical();
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.EndVertical();

					}
				}
				if (skippedPresets==presetObjects.Count) {
					if (searchString!="") {
						EditorGUILayout.HelpBox("No preset found containing \""+searchString+"\".", MessageType.Info);
					} else {
						if (presetExampleUser==0)
							EditorGUILayout.HelpBox("No presets found. Press \"Create\" to make a new preset.", MessageType.Info);
						else if (presetExampleUser==1)
							EditorGUILayout.HelpBox("No user presets found in any \"Resources/Presets\" folder. Press \"Create\" to make a new preset.", MessageType.Info);
						else if (presetExampleUser==2)
							EditorGUILayout.HelpBox("No example presets found. Make sure they are stored in \""+"Assets/"+playgroundPath+examplePresetPath+"\"", MessageType.Info);
					}
				}
				if (presetListStyle==0) EditorGUILayout.EndHorizontal();
			} else {
				EditorGUILayout.HelpBox("No presets found. Make sure that the path to the presets are set to: \""+"Assets/"+playgroundPath+examplePresetPath+"\"", MessageType.Info);
			}
			
			if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))){
				PlaygroundCreatePresetWindowC.ShowWindow();
			}
			
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
		
		PlaygroundInspectorC.RenderPlaygroundSettings();
		
		GUILayout.FlexibleSpace();
		
		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Official Site", EditorStyles.toolbarButton)) {
				Application.OpenURL ("http://playground.polyfied.com/");
			}
			if (GUILayout.Button("Asset Store", EditorStyles.toolbarButton)) {
				Application.OpenURL ("http://u3d.as/5ZJ");
			}
			if (GUILayout.Button("Manual", EditorStyles.toolbarButton)) {
				Application.OpenURL ("http://www.polyfied.com/products/Particle-Playground-2-Next-Manual.pdf");
			}
			if (GUILayout.Button("Support Forum", EditorStyles.toolbarButton)) {
				Application.OpenURL ("http://forum.unity3d.com/threads/215154-Particle-Playground");
			}
			if (GUILayout.Button("Mail Support", EditorStyles.toolbarButton)) {
				Application.OpenURL ("mailto:support@polyfied.com");
			}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		GUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	public void RemovePreset (Object presetObject) {
		if (EditorUtility.DisplayDialog("Permanently delete this preset?", 
		                                "The preset "+presetObject.name+" will be removed, are you sure?", 
		                                "Yes", 
		                                "No")) {
			AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(presetObject));
		}
	}

	// User created preset
	public void CreatePresetObject (int i) {
		PlaygroundParticlesC instantiatedPreset;
		if (!AssetDatabase.GetAssetPath(presetObjects[i].presetObject).Contains ("Resources/"))
			instantiatedPreset = InstantiateEditorPreset(presetObjects[i].presetObject);
		else
			instantiatedPreset = PlaygroundC.InstantiatePreset(presetObjects[i].presetObject.name);
		if (instantiatedPreset!=null)
			instantiatedPreset.EditorYieldSelect();
	}

	// Instantiate a preset by name reference
	public static PlaygroundParticlesC InstantiateEditorPreset (Object presetObject) {
		GameObject presetGo = (GameObject)Instantiate(presetObject);
		PlaygroundParticlesC presetParticles = presetGo.GetComponent<PlaygroundParticlesC>();
		if (presetParticles!=null) {
			if (PlaygroundC.reference==null)
				PlaygroundC.ResourceInstantiate("Playground Manager");
			if (PlaygroundC.reference) {
				if (PlaygroundC.reference.autoGroup && presetParticles.particleSystemTransform.parent==null)
					presetParticles.particleSystemTransform.parent = PlaygroundC.referenceTransform;
				PlaygroundC.particlesQuantity++;
				//PlaygroundC.reference.particleSystems.Add(presetParticles);
				presetParticles.particleSystemId = PlaygroundC.particlesQuantity;
			}
			presetGo.name = presetObject.name;
			return presetParticles;
		} else return null;
	}
	
	public void CreateManager () {
		PlaygroundC.ResourceInstantiate("Playground Manager");
	}
}

public class PresetObjectC {
	public Object presetObject;
	public Texture2D presetImage;
	public bool unfiltered = true;
	public bool example = false;
}