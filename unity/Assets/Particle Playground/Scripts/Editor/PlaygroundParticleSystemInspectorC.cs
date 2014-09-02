using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using ParticlePlayground;

[CustomEditor (typeof(PlaygroundParticlesC))]
class PlaygroundParticleSystemInspectorC : Editor {
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// PlaygroundParticles variables
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static PlaygroundParticlesC playgroundParticlesScriptReference;
	public static SerializedObject playgroundParticles;				// PlaygroundParticlesC
	public static SerializedProperty source;						// SOURCEC
	public static SerializedProperty sorting;						// SORTINGC
	public static SerializedProperty lifetimeSorting;				// AnimationCurve
	public static SerializedProperty nearestNeighborOrigin;			// int
	public static SerializedProperty activeState;					// int
	public static SerializedProperty particleCount;					// int
	public static SerializedProperty emissionRate;					// float
	public static SerializedProperty updateRate;					// int
	public static SerializedProperty worldObjectUpdateVertices;		// boolean
	public static SerializedProperty worldObjectUpdateNormals;		// boolean
	public static SerializedProperty emit;							// boolean
	public static SerializedProperty loop;							// boolean
	public static SerializedProperty disableOnDone;					// boolean
	public static SerializedProperty calculate;						// boolean
	public static SerializedProperty deltaMovementStrength;			// float
	public static SerializedProperty particleTimescale;				// float
	public static SerializedProperty sizeMin;						// float
	public static SerializedProperty sizeMax;						// float
	public static SerializedProperty lifetime;						// float
	public static SerializedProperty lifetimeSize;					// AnimationCurve
	public static SerializedProperty onlySourcePositioning;			// boolean
	public static SerializedProperty applyLifetimeVelocity;			// boolean
	public static SerializedProperty applyInitialVelocity;			// boolean
	public static SerializedProperty applyInitialLocalVelocity;		// boolean
	public static SerializedProperty applyVelocityBending;			// boolean
	public static SerializedProperty velocityBendingType;			// VELOCITYBENDINGTYPE
	public static SerializedProperty lifetimeVelocity;				// Vector3AnimationCurveC
	public static SerializedProperty initialVelocityShape;			// Vector3AnimationCurveC
	public static SerializedProperty overflowOffset;				// Vector3
	public static SerializedProperty overflowMode;					// OVERFLOWMODEC
	public static SerializedProperty initialVelocityMin;			// Vector3
	public static SerializedProperty initialVelocityMax;			// Vector3
	public static SerializedProperty initialLocalVelocityMin;		// Vector3
	public static SerializedProperty initialLocalVelocityMax;		// Vector3
	public static SerializedProperty turbulenceLifetimeStrength;	// AnimationCurve
	public static SerializedProperty lifetimeColor;					// Gradient
	public static SerializedProperty lifetimeColors;				// List<Gradient>
	public static SerializedProperty colorSource;					// COLORSOURCEC
	public static SerializedProperty collision;						// boolean
	public static SerializedProperty affectRigidbodies;				// boolean
	public static SerializedProperty mass;							// float
	public static SerializedProperty collisionRadius;				// float
	public static SerializedProperty collisionMask;					// LayerMask
	public static SerializedProperty collisionType;					// COLLISIONTYPE
	public static SerializedProperty bounciness;					// float
	public static SerializedProperty lifetimeStretching;			// AnimationCurve
	public static Object particleMaterial;							// Material
	
	public static SerializedProperty states;						// ParticleStateC[]
	public static SerializedProperty worldObject;					// WorldObjectC
	public static SerializedProperty worldObjectGameObject;			// GameObject
	public static SerializedProperty skinnedWorldObject;			// SkinnedWorldObjectC
	public static SerializedProperty skinnedWorldObjectGameObject; 	// GameObject
	public static SerializedProperty sourceTransform;				// Transform
	public static SerializedProperty sourcePaint;					// PaintObjectC
	public static SerializedProperty sourceProjection;				// ParticleProjectionC
	
	public static SerializedProperty lifeTimeVelocityX;				// AnimationCurve
	public static SerializedProperty lifeTimeVelocityY;				// AnimationCurve
	public static SerializedProperty lifeTimeVelocityZ;				// AnimationCurve
	
	public static SerializedProperty initialVelocityShapeX;			// AnimationCurve
	public static SerializedProperty initialVelocityShapeY;			// AnimationCurve
	public static SerializedProperty initialVelocityShapeZ;			// AnimationCurve

	public static SerializedProperty movementCompensationLifetimeStrength; // AnimationCurve
	
	public static SerializedProperty manipulators;					// List.<ManipulatorObjectC>
	public static SerializedProperty events;						// List.<PlaygroundEventC>
	public static SerializedProperty snapshots;						// List.<PlaygroundSave>
	public static ParticleSystemRenderer shurikenRenderer;			// ParticleSystemRenderer
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Playground variables
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static PlaygroundC playgroundScriptReference;			// PlaygroundC
	public static SerializedObject playground;						// PlaygroundC
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// PlaygroundParticleSystemInspector variables
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	// States
	public static int meshOrImage;
	public static string addStateName = "";
	public static Object addStateMesh;
	public static Object addStateTexture;
	public static Object addStateTransform;
	public static Object addStateDepthmap;
	public static float addStateDepthmapStrength = 1f;
	public static float addStateSize = 1f;
	public static float addStateScale = 1f;
	public static Vector3 addStateOffset;
	
	// Foldouts
	public static bool particlesFoldout = true;
	public static bool statesFoldout = true;
	public static bool createNewStateFoldout = false;
	public static bool sourceFoldout = false;
	public static bool particleSettingsFoldout = false;
	public static bool forcesFoldout = false;
	public static bool manipulatorsFoldout = false;
	public static bool eventsFoldout = false;
	public static bool collisionFoldout = false;
	public static bool collisionPlanesFoldout = false;
	public static bool renderingFoldout = false;
	public static bool advancedFoldout = false;
	public static bool saveLoadFoldout = false;
	public static  List<bool> statesListFoldout;
	public static bool toolboxFoldout = true;
	public static bool paintToolboxSettingsFoldout = false;
	public static List<bool> manipulatorListFoldout;
	public static List<bool> eventListFoldout;
	
	// Paint variables
	public static int brushListStyle = 0;
	public static Color32 paintColor = new Color(1,1,1,1);
	public static bool useBrushColor = true;
	public static int selectedPaintMode;
	public static GUIStyle sceneBrushStyle;
	public static List<Object> brushPrefabs;
	public static string[] brushNames;
	public static float[] paintSpacings;
	public static bool[] exceedMaxStopsPaintList;
	public static bool inPaintMode = false;
	public static Object paintTexture;
	public static PlaygroundBrushC[] brushPresets;
	public static int selectedBrushPreset = -1;
	public static bool brushPresetFoldout = false;
	public static SerializedProperty paintLayerMask;
	public static SerializedProperty paintCollisionType;
	public static Tool lastActiveTool = Tool.None;
	public static float eraserRadius = 1f;
	private bool showNoAlphaWarning = false;
	
	// Projection variables
	public static SerializedProperty projectionMask;
	public static SerializedProperty projectionCollisionType;
	
	// GUI
	public static GUIStyle boxStyle;

	public static bool currentWireframe;
	private Keyframe[] prevLifetimeSortingKeys;
	private SOURCEC previousSource;
	public static string saveName = "New Snapshot";
	
	void OnEnable () {

		lastActiveTool = Tools.current;
		
		// Playground Particles
		playgroundParticlesScriptReference = target as PlaygroundParticlesC;
		playgroundParticles = new SerializedObject(playgroundParticlesScriptReference);
		
		shurikenRenderer = playgroundParticlesScriptReference.particleSystemGameObject.particleSystem.renderer as ParticleSystemRenderer;
		
		manipulators = playgroundParticles.FindProperty("manipulators");
		events = playgroundParticles.FindProperty("events");
		snapshots = playgroundParticles.FindProperty("snapshots");
		source = playgroundParticles.FindProperty("source");
		sorting = playgroundParticles.FindProperty("sorting");
		lifetimeSorting = playgroundParticles.FindProperty("lifetimeSorting");
		nearestNeighborOrigin = playgroundParticles.FindProperty("nearestNeighborOrigin");
		activeState = playgroundParticles.FindProperty("activeState");
		particleCount = playgroundParticles.FindProperty("particleCount");
		emissionRate = playgroundParticles.FindProperty("emissionRate");
		updateRate = playgroundParticles.FindProperty("updateRate");
		emit = playgroundParticles.FindProperty("emit");
		loop = playgroundParticles.FindProperty("loop");
		disableOnDone = playgroundParticles.FindProperty("disableOnDone");
		calculate = playgroundParticles.FindProperty("calculate");
		deltaMovementStrength = playgroundParticles.FindProperty("deltaMovementStrength");
		particleTimescale = playgroundParticles.FindProperty("particleTimescale");
		sizeMin = playgroundParticles.FindProperty("sizeMin");
		sizeMax = playgroundParticles.FindProperty("sizeMax");
		overflowOffset = playgroundParticles.FindProperty("overflowOffset");
		overflowMode = playgroundParticles.FindProperty("overflowMode");
		lifetime = playgroundParticles.FindProperty("lifetime");
		lifetimeSize = playgroundParticles.FindProperty("lifetimeSize");
		turbulenceLifetimeStrength = playgroundParticles.FindProperty("turbulenceLifetimeStrength");
		lifetimeVelocity = playgroundParticles.FindProperty("lifetimeVelocity");
		initialVelocityShape = playgroundParticles.FindProperty("initialVelocityShape");
		initialVelocityMin = playgroundParticles.FindProperty("initialVelocityMin");
		initialVelocityMax = playgroundParticles.FindProperty("initialVelocityMax");
		initialLocalVelocityMin = playgroundParticles.FindProperty("initialLocalVelocityMin");
		initialLocalVelocityMax = playgroundParticles.FindProperty("initialLocalVelocityMax");
		lifetimeColor = playgroundParticles.FindProperty("lifetimeColor");
		lifetimeColors = playgroundParticles.FindProperty ("lifetimeColors");
		colorSource = playgroundParticles.FindProperty("colorSource");
		collision = playgroundParticles.FindProperty("collision");
		affectRigidbodies = playgroundParticles.FindProperty("affectRigidbodies");
		mass = playgroundParticles.FindProperty("mass");
		collisionRadius = playgroundParticles.FindProperty("collisionRadius");
		collisionMask = playgroundParticles.FindProperty("collisionMask");
		collisionType = playgroundParticles.FindProperty("collisionType");
		bounciness = playgroundParticles.FindProperty("bounciness");
		states = playgroundParticles.FindProperty("states");
		worldObject = playgroundParticles.FindProperty("worldObject");
		skinnedWorldObject = playgroundParticles.FindProperty("skinnedWorldObject");
		sourceTransform = playgroundParticles.FindProperty("sourceTransform");
		worldObjectUpdateVertices = playgroundParticles.FindProperty ("worldObjectUpdateVertices");
		worldObjectUpdateNormals = playgroundParticles.FindProperty("worldObjectUpdateNormals");
		sourcePaint = playgroundParticles.FindProperty("paint");
		sourceProjection = playgroundParticles.FindProperty("projection");
		lifetimeStretching = playgroundParticles.FindProperty("stretchLifetime");

		playgroundParticlesScriptReference.shurikenParticleSystem = playgroundParticlesScriptReference.GetComponent<ParticleSystem>();
		playgroundParticlesScriptReference.particleSystemRenderer = playgroundParticlesScriptReference.shurikenParticleSystem.renderer;
		particleMaterial = playgroundParticlesScriptReference.particleSystemRenderer.sharedMaterial;
		
		onlySourcePositioning = playgroundParticles.FindProperty("onlySourcePositioning");
		
		applyLifetimeVelocity = playgroundParticles.FindProperty("applyLifetimeVelocity");
		lifeTimeVelocityX = lifetimeVelocity.FindPropertyRelative("x");
		lifeTimeVelocityY = lifetimeVelocity.FindPropertyRelative("y");
		lifeTimeVelocityZ = lifetimeVelocity.FindPropertyRelative("z");
		
		initialVelocityShapeX = initialVelocityShape.FindPropertyRelative("x");
		initialVelocityShapeY = initialVelocityShape.FindPropertyRelative("y");
		initialVelocityShapeZ = initialVelocityShape.FindPropertyRelative("z");
		
		applyInitialVelocity = playgroundParticles.FindProperty("applyInitialVelocity");
		applyInitialLocalVelocity = playgroundParticles.FindProperty("applyInitialLocalVelocity");
		applyVelocityBending = playgroundParticles.FindProperty("applyVelocityBending");
		velocityBendingType = playgroundParticles.FindProperty("velocityBendingType");

		movementCompensationLifetimeStrength = playgroundParticles.FindProperty ("movementCompensationLifetimeStrength");
		
		worldObjectGameObject = worldObject.FindPropertyRelative("gameObject");
		skinnedWorldObjectGameObject = skinnedWorldObject.FindPropertyRelative("gameObject");

		// Lifetime colors
		if (playgroundParticlesScriptReference.lifetimeColors==null)
			playgroundParticlesScriptReference.lifetimeColors = new List<PlaygroundGradientC>();

		// Sorting
		prevLifetimeSortingKeys = playgroundParticlesScriptReference.lifetimeSorting.keys;
		
		// Manipulator list
		manipulatorListFoldout = new List<bool>();
		manipulatorListFoldout.AddRange(new bool[playgroundParticlesScriptReference.manipulators.Count]);

		// Events list
		eventListFoldout = new List<bool>();
		eventListFoldout.AddRange(new bool[playgroundParticlesScriptReference.events.Count]);

		// States foldout
		statesListFoldout = new List<bool>();
		statesListFoldout.AddRange(new bool[playgroundParticlesScriptReference.states.Count]);

		previousSource = playgroundParticlesScriptReference.source;
		
		// Playground
		playgroundScriptReference = FindObjectOfType<PlaygroundC>();
		
		
		// Create a manager if no existing instance is in the scene
		if (!playgroundScriptReference && Selection.activeTransform!=null) {
			PlaygroundC.ResourceInstantiate("Playground Manager");
			playgroundScriptReference = FindObjectOfType<PlaygroundC>();
		}
		
		if (playgroundScriptReference!=null) {
			PlaygroundC.reference = playgroundScriptReference;

			// Serialize Playground
			playground = new SerializedObject(playgroundScriptReference);
			
			PlaygroundInspectorC.Initialize(playgroundScriptReference);
			
			
			// Add this PlaygroundParticles if not existing in Playground list
			if (!playgroundParticlesScriptReference.isSnapshot && !playgroundScriptReference.particleSystems.Contains(playgroundParticlesScriptReference) && Selection.activeTransform!=null)
				playgroundScriptReference.particleSystems.Add(playgroundParticlesScriptReference);
				
			// Cache components
			playgroundParticlesScriptReference.particleSystemGameObject = playgroundParticlesScriptReference.gameObject;
			playgroundParticlesScriptReference.particleSystemTransform = playgroundParticlesScriptReference.transform;
			playgroundParticlesScriptReference.particleSystemRenderer = playgroundParticlesScriptReference.renderer;
			playgroundParticlesScriptReference.shurikenParticleSystem = playgroundParticlesScriptReference.particleSystemGameObject.GetComponent<ParticleSystem>();
			playgroundParticlesScriptReference.particleSystemRenderer2 = playgroundParticlesScriptReference.particleSystemGameObject.particleSystem.renderer as ParticleSystemRenderer;
			
			// Set manager as parent 
			if (PlaygroundC.reference.autoGroup && playgroundParticlesScriptReference.particleSystemTransform!=null && playgroundParticlesScriptReference.particleSystemTransform.parent == null && Selection.activeTransform!=null)
				playgroundParticlesScriptReference.particleSystemTransform.parent = PlaygroundC.referenceTransform;
			
			// Issue a quick refresh
			if (!EditorApplication.isPlaying)
				foreach (PlaygroundParticlesC p in PlaygroundC.reference.particleSystems)
					p.Start();
		}

		selectedSort = sorting.intValue;

		// State initial values
		if (addStateTransform==null)
			addStateTransform = (Transform)playgroundParticlesScriptReference.particleSystemTransform;
		
		// Visiblity of Shuriken component in Inspector
		if (!playgroundScriptReference || playgroundScriptReference && !playgroundScriptReference.showShuriken)
			playgroundParticlesScriptReference.shurikenParticleSystem.hideFlags = HideFlags.HideInInspector;
		else
			playgroundParticlesScriptReference.shurikenParticleSystem.hideFlags = HideFlags.None;

		SetWireframeVisibility();

		// Set paint init
		paintLayerMask = sourcePaint.FindPropertyRelative("layerMask");
		paintCollisionType = sourcePaint.FindPropertyRelative("collisionType");
		
		LoadBrushes();
		
		// Set projection init
		projectionMask = sourceProjection.FindPropertyRelative("projectionMask");
		projectionCollisionType = sourceProjection.FindPropertyRelative("collisionType");

		// Snapshots
		if (playgroundParticlesScriptReference.snapshots.Count>0) {
			if (playgroundParticlesScriptReference.snapshots.Count>0) {
				for (int i = 0; i<playgroundParticlesScriptReference.snapshots.Count; i++)
					if (playgroundParticlesScriptReference.snapshots[i].settings==null)
						playgroundParticlesScriptReference.snapshots.RemoveAt(i);
			}
			saveName += " "+(playgroundParticlesScriptReference.snapshots.Count+1).ToString();
		}
	}

	public static void SetWireframeVisibility () {

		if (Selection.activeTransform==null) return;

		// Wireframes in Scene View
		EditorUtility.SetSelectedWireframeHidden(playgroundParticlesScriptReference.particleSystemRenderer, !PlaygroundC.reference.drawWireframe);
		currentWireframe = PlaygroundC.reference.drawWireframe;
	}
	
	public static void LoadBrushes () {

		// Set brush presets and custom brush texture
		brushPrefabs = new List<Object>();
		string assetsDataPath = Application.dataPath;
		string editorBrushPath = assetsDataPath+"/"+PlaygroundParticleWindowC.playgroundPath+PlaygroundParticleWindowC.brushPath;
		string[] editorBrushPaths = Directory.GetFiles (editorBrushPath);
		
		foreach (string thisBrushPath in editorBrushPaths) {
			string convertedBrushPath = thisBrushPath.Substring(assetsDataPath.Length-6);
			Object brushPathObject = (Object)AssetDatabase.LoadAssetAtPath(convertedBrushPath, typeof(Object));
			if (brushPathObject!=null && (brushPathObject.GetType().Name)=="GameObject") {
				brushPrefabs.Add (brushPathObject);
			}
		}


		brushNames = new string[brushPrefabs.Count];
		paintSpacings = new float[brushPrefabs.Count];
		brushPresets = new PlaygroundBrushC[brushPrefabs.Count];
		exceedMaxStopsPaintList = new bool[brushPrefabs.Count];
		for (int i = 0; i<brushPresets.Length; i++) {
			GameObject thisBrushGO = (GameObject)Instantiate (brushPrefabs[i]);
			PlaygroundBrushPresetC thisBrushPrefab = thisBrushGO.GetComponent<PlaygroundBrushPresetC>();
			brushNames[i] = thisBrushPrefab.presetName;
			brushPresets[i] = new PlaygroundBrushC();
			brushPresets[i].texture = thisBrushPrefab.texture as Texture2D;
			brushPresets[i].detail = thisBrushPrefab.detail;
			brushPresets[i].scale = thisBrushPrefab.scale;
			brushPresets[i].distance = thisBrushPrefab.distance;
			
			paintSpacings[i] = thisBrushPrefab.spacing;
			exceedMaxStopsPaintList[i] = thisBrushPrefab.exceedMaxStopsPaint;
			DestroyImmediate (thisBrushGO);
		}
		
		if (source.intValue==5 && paintTexture!=null)
			SetBrush(selectedBrushPreset);
		
		
		if (playgroundParticlesScriptReference.paint!=null && playgroundParticlesScriptReference.paint.brush!=null && playgroundParticlesScriptReference.paint.brush.texture!=null) {
			paintTexture = playgroundParticlesScriptReference.paint.brush.texture;
		}
	}
	
	public static void SetBrush (int i) {
		if (i>=0) {
			TextureImporter tAssetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(brushPresets[i].texture as UnityEngine.Object)) as TextureImporter;
			if (!tAssetImporter.isReadable) {
				Debug.Log(tAssetImporter.assetPath+" is not readable. Please change Read/Write Enabled on its Import Settings.");
				return; 
			}
			selectedBrushPreset = i;
			paintTexture = brushPresets[selectedBrushPreset].texture;
			playgroundParticlesScriptReference.paint.brush.SetTexture(brushPresets[selectedBrushPreset].texture);
			playgroundParticlesScriptReference.paint.brush.scale = brushPresets[selectedBrushPreset].scale;
			playgroundParticlesScriptReference.paint.brush.detail = brushPresets[selectedBrushPreset].detail;
			playgroundParticlesScriptReference.paint.brush.distance = brushPresets[selectedBrushPreset].distance;
			
			playgroundParticlesScriptReference.paint.spacing = paintSpacings[selectedBrushPreset];
			playgroundParticlesScriptReference.paint.exceedMaxStopsPaint = exceedMaxStopsPaintList[selectedBrushPreset];
		} else {
			playgroundParticlesScriptReference.paint.brush.SetTexture(paintTexture as Texture2D);
		}
		
		// Set brush preview style
		sceneBrushStyle = new GUIStyle();
		sceneBrushStyle.imagePosition = ImagePosition.ImageOnly;
		sceneBrushStyle.border = new RectOffset(0,0,0,0);
		sceneBrushStyle.stretchWidth = true;
		sceneBrushStyle.stretchHeight = true;
		SetBrushStyle();
	}
	
	public static void SetBrushStyle () {
		if (playgroundParticlesScriptReference.paint.brush==null || playgroundParticlesScriptReference.paint.brush.texture==null || sceneBrushStyle==null) return;
		float brushScale = playgroundParticlesScriptReference.paint.brush.scale;
		sceneBrushStyle.fixedWidth = playgroundParticlesScriptReference.paint.brush.texture.width*brushScale;
		sceneBrushStyle.fixedHeight = playgroundParticlesScriptReference.paint.brush.texture.height*brushScale;
		sceneBrushStyle.contentOffset = -new Vector2(playgroundParticlesScriptReference.paint.brush.texture.width/2, playgroundParticlesScriptReference.paint.brush.texture.height/2)*brushScale;
	}
	
	void OnDestroy () {
		brushPresets = null;
		inPaintMode = false;
		Tools.current = lastActiveTool;
	}
	
	public override void OnInspectorGUI () {
		if (boxStyle==null)
			boxStyle = GUI.skin.FindStyle("box");
		
		if (Selection.activeTransform==null) {
			EditorGUILayout.LabelField("Please edit this from Hierarchy only.");
			return;
		}
		
		if (Event.current.type == EventType.ValidateCommand &&
			Event.current.commandName == "UndoRedoPerformed") {			
				PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.particleCount);
				LifetimeSorting();
		}
		
		playgroundParticles.Update();
		
		EditorGUILayout.Separator();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Particle Playground "+PlaygroundC.version+PlaygroundC.specialVersion, EditorStyles.largeLabel, GUILayout.Height(20));
		
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Playground Wizard", EditorStyles.toolbarButton)) {
			PlaygroundParticleWindowC.ShowWindow();
		}
		if(GUILayout.Button("Create Preset", EditorStyles.toolbarButton)){
			PlaygroundCreatePresetWindowC.ShowWindow();
		}
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();

		if (playgroundParticlesScriptReference.particleSystemTransform.localScale.x != 1f ||
		    playgroundParticlesScriptReference.particleSystemTransform.localScale.y != 1f ||
		    playgroundParticlesScriptReference.particleSystemTransform.localScale.z != 1f) {
			EditorGUILayout.BeginVertical (boxStyle);
			EditorGUILayout.HelpBox("A local scale of anything else than Vector3 (1,1,1) may result in Shuriken component not rendering.", MessageType.Warning);
			if (GUILayout.Button ("Fix", EditorStyles.toolbarButton, GUILayout.Width (40)))
				playgroundParticlesScriptReference.particleSystemTransform.localScale = new Vector3(1f,1f,1f);
			EditorGUILayout.EndVertical ();
		}
		// Particles
		EditorGUILayout.BeginVertical(boxStyle);
		if (playgroundParticlesScriptReference.eventControlledBy.Count>0)
			particlesFoldout = GUILayout.Toggle(particlesFoldout, "Playground Particles (Event Controlled)", EditorStyles.foldout);
		else if (playgroundParticlesScriptReference.isSnapshot)
			particlesFoldout = GUILayout.Toggle(particlesFoldout, "Playground Particles (Snapshot)", EditorStyles.foldout);
		else
			particlesFoldout = GUILayout.Toggle(particlesFoldout, "Playground Particles", EditorStyles.foldout);
		if (particlesFoldout) {
			
			EditorGUILayout.BeginVertical(boxStyle);
			
			// Source Settings
			if (GUILayout.Button("Source ("+playgroundParticlesScriptReference.source.ToString()+")", EditorStyles.toolbarDropDown)) sourceFoldout=!sourceFoldout;
			if (sourceFoldout) {
				
				EditorGUILayout.Separator();
				
				if (previousSource!=playgroundParticlesScriptReference.source) {
					LifetimeSorting();
				}
				EditorGUILayout.PropertyField(source, new GUIContent(
					"Source", 
					"Source is the target method for the particles in this Particle Playground System.\n\nState: Target position and color in a stored state\n\nTransform: Target transforms live in the scene\n\nWorldObject: Target each vertex in a mesh live in the scene\n\nSkinnedWorldObject: Target each vertex in a skinned mesh live in the scene\n\nScript: Behaviour controlled by custom scripts\n\nPaint: Target painted positions and colors made with a brush\n\nProjection: Target projected positions and colors made with a texture")
				);
				
				EditorGUILayout.Separator();
				
				// Source is State
				if (source.intValue == 0) {
					RenderStateSettings();
					
				// Source is Projection
				} else if (source.intValue == 6) {
					RenderProjectionSettings();
				
				// Source is Transforms
				} else if (source.intValue == 1) {
					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Transform");
					sourceTransform.objectReferenceValue = EditorGUILayout.ObjectField(sourceTransform.objectReferenceValue, typeof(Transform), true);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Points:");
					EditorGUILayout.SelectableLabel(sourceTransform.objectReferenceValue!=null?"1":"0", GUILayout.MaxWidth(80));
					EditorGUILayout.Separator();
					if(GUILayout.Button("Set Particle Count", EditorStyles.toolbarButton)){
						PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, sourceTransform.objectReferenceValue!=null?1:0);
						playgroundParticlesScriptReference.Start();
					}
					GUI.enabled = (sourceTransform.objectReferenceValue!=null);
					if(GUILayout.Button("++", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}))
						particleCount.intValue = particleCount.intValue+1;
					GUI.enabled = true;
					GUILayout.EndHorizontal();
					
				// Source is World Object
				} else if (source.intValue == 2) {
					playgroundParticlesScriptReference.worldObject.gameObject = (GameObject)EditorGUILayout.ObjectField("World Object", playgroundParticlesScriptReference.worldObject.gameObject, typeof(GameObject), true);

					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Points:");
					EditorGUILayout.SelectableLabel((playgroundParticlesScriptReference.worldObject.vertexPositions!=null && playgroundParticlesScriptReference.worldObject.vertexPositions.Length>0)?playgroundParticlesScriptReference.worldObject.vertexPositions.Length.ToString():"No mesh", GUILayout.MaxWidth(80));
					
					EditorGUILayout.Separator();
					if(GUILayout.Button("Set Particle Count", EditorStyles.toolbarButton) && playgroundParticlesScriptReference.worldObject.vertexPositions!=null){
						PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.worldObject.vertexPositions.Length);
						playgroundParticlesScriptReference.Start();
					}
					if(GUILayout.Button("++", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}) && playgroundParticlesScriptReference.worldObject.vertexPositions!=null)
						particleCount.intValue = particleCount.intValue+playgroundParticlesScriptReference.worldObject.vertexPositions.Length;
					GUILayout.EndHorizontal();

					GUILayout.BeginVertical(boxStyle);
					EditorGUILayout.LabelField("Procedural Options");
					EditorGUILayout.PropertyField(worldObjectUpdateVertices, new GUIContent(
						"Mesh Vertices Update",
						"Enable this if the World Object's mesh is procedural and changes vertices over time."
					));
					EditorGUILayout.PropertyField(worldObjectUpdateNormals, new GUIContent(
						"Mesh Normals Update",
						"Enable this if the World Object's mesh is procedural and changes normals over time."
					));
					GUILayout.EndVertical();
					
				// Source is Skinned World Object
				} else if (source.intValue == 3) {
					playgroundParticlesScriptReference.skinnedWorldObject.gameObject = (GameObject)EditorGUILayout.ObjectField("Skinned World Object", playgroundParticlesScriptReference.skinnedWorldObject.gameObject, typeof(GameObject), true);
					
					if (playgroundParticlesScriptReference.skinnedWorldObject.mesh) {
						int prevDownResolutionSkinned = playgroundParticlesScriptReference.skinnedWorldObject.downResolution;
						playgroundParticlesScriptReference.skinnedWorldObject.downResolution = EditorGUILayout.IntSlider("Source Down Resolution", playgroundParticlesScriptReference.skinnedWorldObject.downResolution, 1, Mathf.RoundToInt (playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length/2));
						if (prevDownResolutionSkinned!=playgroundParticlesScriptReference.skinnedWorldObject.downResolution)
							LifetimeSorting();
					}

					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Points:");
					if (playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions!=null && playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length>0) {
						if (playgroundParticlesScriptReference.skinnedWorldObject.downResolution<=1)
							EditorGUILayout.SelectableLabel(playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length.ToString(), GUILayout.MaxWidth(80));
						else
							EditorGUILayout.SelectableLabel((playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length/playgroundParticlesScriptReference.skinnedWorldObject.downResolution).ToString()+" ("+playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length.ToString()+")", GUILayout.MaxWidth(160));
					} else EditorGUILayout.SelectableLabel("No mesh");
					EditorGUILayout.Separator();
					if(GUILayout.Button("Set Particle Count", EditorStyles.toolbarButton) && playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions!=null){
						PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.skinnedWorldObject.downResolution<=1?playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length:playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length/playgroundParticlesScriptReference.skinnedWorldObject.downResolution);
						playgroundParticlesScriptReference.Start();
					}
					if(GUILayout.Button("++", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}) && playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions!=null)
						particleCount.intValue = particleCount.intValue+(playgroundParticlesScriptReference.skinnedWorldObject.vertexPositions.Length/playgroundParticlesScriptReference.skinnedWorldObject.downResolution);
					GUILayout.EndHorizontal();

					GUILayout.BeginVertical(boxStyle);
					EditorGUILayout.LabelField("Procedural Options");
					EditorGUILayout.PropertyField(worldObjectUpdateVertices, new GUIContent(
						"Mesh Vertices Update",
						"Enable this if the Skinned World Object's mesh is procedural and changes vertices over time."
						));
					EditorGUILayout.PropertyField(worldObjectUpdateNormals, new GUIContent(
						"Mesh Normals Update",
						"Enable this if the Skinned World Object's mesh is procedural and changes normals over time."
						));
					GUILayout.EndVertical();
					
				// Source is Script
				} else if (source.intValue == 4) {

					// Controlled by events
					if (playgroundParticlesScriptReference.eventControlledBy.Count>0) {
						GUILayout.BeginVertical (boxStyle);
						int eventCount = 0;
						EditorGUILayout.HelpBox("This Particle Playground System is controlled by events from another particle system.", MessageType.Info);
						for (int i = 0; i<playgroundParticlesScriptReference.eventControlledBy.Count; i++) {
							eventCount = 0;
							for (int x = 0; x<playgroundParticlesScriptReference.eventControlledBy[i].events.Count; x++)
								if (playgroundParticlesScriptReference.eventControlledBy[i].events[x].target==playgroundParticlesScriptReference) eventCount++;
							GUILayout.BeginHorizontal (boxStyle);
							GUILayout.Label (i.ToString(), EditorStyles.miniLabel, new GUILayoutOption[]{GUILayout.Width(18)});
							if (GUILayout.Button (playgroundParticlesScriptReference.eventControlledBy[i].name+" ("+eventCount+")", EditorStyles.label)) 
								Selection.activeGameObject = playgroundParticlesScriptReference.eventControlledBy[i].particleSystemGameObject;
							if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
								if (EditorUtility.DisplayDialog(
									"Remove events from "+playgroundParticlesScriptReference.eventControlledBy[i].name+"?",
									"All events connected to this particle system from "+playgroundParticlesScriptReference.eventControlledBy[i].name+" will be removed. Are you sure you want to remove them?", 
									"Yes", "No")) {
									for (int x = 0; x<playgroundParticlesScriptReference.eventControlledBy[i].events.Count; x++) {
										if (playgroundParticlesScriptReference.eventControlledBy[i].events[x].target == playgroundParticlesScriptReference) {
											playgroundParticlesScriptReference.eventControlledBy[i].events.RemoveAt(x);
											x--;
										}
									}
									playgroundParticlesScriptReference.eventControlledBy.RemoveAt (i);
								}
							}
							GUILayout.EndHorizontal ();
						}
						GUILayout.EndVertical ();
						GUI.enabled = false;
					} else EditorGUILayout.HelpBox("This Particle Playground System is controlled by script. You can only emit particles from script in this source mode using PlaygroundParticlesC.Emit(position, velocity, color) or let another particle system control it by events. Please see the manual for more details.", MessageType.Info);


					EditorGUILayout.Separator();
					
					EditorGUILayout.BeginVertical(boxStyle);
					EditorGUILayout.BeginHorizontal();
					playgroundParticlesScriptReference.scriptedEmissionIndex = EditorGUILayout.IntField("Emission Index", Mathf.Clamp(playgroundParticlesScriptReference.scriptedEmissionIndex, 0, playgroundParticlesScriptReference.particleCount-1));
					if(GUILayout.RepeatButton("Emit ()", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Height(16)})) {
						PlaygroundC.Emit(playgroundParticlesScriptReference);
					}
					EditorGUILayout.EndHorizontal();
					
					playgroundParticlesScriptReference.scriptedEmissionPosition = EditorGUILayout.Vector3Field("Position", playgroundParticlesScriptReference.scriptedEmissionPosition);
					playgroundParticlesScriptReference.scriptedEmissionVelocity = EditorGUILayout.Vector3Field("Velocity", playgroundParticlesScriptReference.scriptedEmissionVelocity);
					playgroundParticlesScriptReference.scriptedEmissionColor = EditorGUILayout.ColorField("Color", playgroundParticlesScriptReference.scriptedEmissionColor);

					EditorGUILayout.EndVertical();

					GUI.enabled = true;
					
				// Source is Paint
				} else if (source.intValue == 5) {
					
					if (playgroundParticlesScriptReference.paint==null) {
						PlaygroundC.PaintObject(playgroundParticlesScriptReference);
					}
					
					// Paint Mode
					EditorGUILayout.BeginVertical(boxStyle);
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Paint Mode");
					selectedPaintMode = GUILayout.Toolbar (selectedPaintMode, new string[]{"Dot","Brush","Eraser"}, EditorStyles.toolbarButton);
					EditorGUILayout.EndHorizontal();
					
					// Dot
					if (selectedPaintMode!=0) {
						EditorGUILayout.Separator();
					}
					
					// Brush
					if (selectedPaintMode==1) {
						EditorGUI.indentLevel++;
						EditorGUILayout.BeginVertical(boxStyle);
						brushPresetFoldout = GUILayout.Toggle(brushPresetFoldout, "Brush Presets", EditorStyles.foldout);
						EditorGUI.indentLevel--;
						if (brushPresetFoldout) {
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.Separator();
							brushListStyle = GUILayout.Toolbar (brushListStyle, new string[]{"Icons","List"}, EditorStyles.toolbarButton, GUILayout.MaxWidth(120));
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Separator();
							int i;
							
							// Icons
							if (brushListStyle==0) {
								GUILayout.BeginHorizontal();
								for (i = 0; i<brushPresets.Length; i++) {
									EditorGUILayout.BeginVertical(new GUILayoutOption[]{GUILayout.Width(50), GUILayout.Height(62)});
									
									if (GUILayout.Button(brushPresets[i].texture, new GUILayoutOption[]{GUILayout.Width(32), GUILayout.Height(32)})){
										selectedBrushPreset = i;
										SetBrush(i);
									}
									if (brushNames.Length>0) {
										EditorGUILayout.LabelField(brushNames[i], EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[]{GUILayout.Width(50), GUILayout.Height(30)});
									}
									EditorGUILayout.EndVertical();
									if (i%(Screen.width/80)==0 && i>0) {
										EditorGUILayout.EndHorizontal();
										EditorGUILayout.BeginHorizontal();
									}
								}
								EditorGUILayout.EndHorizontal();
								
								
							// List
							} else {
								for (i = 0; i<brushPresets.Length; i++) {
									EditorGUILayout.BeginVertical(boxStyle, GUILayout.MinHeight(22));
									EditorGUILayout.BeginHorizontal();
									if (GUILayout.Button(brushNames[i], EditorStyles.label)) {
										selectedBrushPreset = i;
										SetBrush(i);
									}
									EditorGUILayout.Separator();
									if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
										if (EditorUtility.DisplayDialog("Permanently delete this brush?", 
											"The brush "+brushNames[i]+" will be removed, are you sure?", 
											"Yes", 
											"No")) {
												AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(brushPrefabs[i] as UnityEngine.Object));
												LoadBrushes();
											}
									}
									EditorGUILayout.EndHorizontal();
									EditorGUILayout.EndVertical();
								}
							}
							
							// Create new brush
							if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))){
								PlaygroundCreateBrushWindowC.ShowWindow();
							}
						}
						
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						
						GUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Brush Shape");
						paintTexture = EditorGUILayout.ObjectField(paintTexture, typeof(Texture2D), false) as Texture2D;
						GUILayout.EndHorizontal();
						playgroundParticlesScriptReference.paint.brush.detail = (BRUSHDETAILC)EditorGUILayout.EnumPopup("Detail", playgroundParticlesScriptReference.paint.brush.detail);
						playgroundParticlesScriptReference.paint.brush.scale = EditorGUILayout.Slider("Brush Scale", playgroundParticlesScriptReference.paint.brush.scale, playgroundScriptReference.minimumAllowedBrushScale, playgroundScriptReference.maximumAllowedBrushScale);
						playgroundParticlesScriptReference.paint.brush.distance = EditorGUILayout.FloatField("Brush Distance", playgroundParticlesScriptReference.paint.brush.distance);
						
						if (paintTexture!=null && paintTexture!=playgroundParticlesScriptReference.paint.brush.texture) {
							playgroundParticlesScriptReference.paint.brush.SetTexture(paintTexture as Texture2D);
							selectedBrushPreset = -1;
						}
						
						useBrushColor = EditorGUILayout.Toggle("Use Brush Color", useBrushColor);
					}
					
					
					// Eraser
					if (selectedPaintMode==2) {
						eraserRadius = EditorGUILayout.Slider("Eraser Radius", eraserRadius, playgroundScriptReference.minimumEraserRadius, playgroundScriptReference.maximumEraserRadius);
					}
					
					EditorGUILayout.EndVertical();
					EditorGUILayout.Separator();
					
					if (selectedPaintMode==1 && useBrushColor) GUI.enabled = false;
					paintColor = EditorGUILayout.ColorField("Color", paintColor);
					GUI.enabled = true;
					if (showNoAlphaWarning && !useBrushColor) {
						EditorGUILayout.HelpBox("You have no alpha in the color. No particle positions will be painted.", MessageType.Warning);
					}
					showNoAlphaWarning = (paintColor.a == 0);

					EditorGUILayout.PropertyField(paintCollisionType, new GUIContent("Paint Collision Type"));
					if (paintCollisionType.enumValueIndex==1) {
						GUILayout.BeginHorizontal();
						GUILayout.Space (16);
						GUILayout.Label("Depth");
						EditorGUILayout.Separator();
						float minDepth = playgroundParticlesScriptReference.paint.minDepth;
						float maxDepth = playgroundParticlesScriptReference.paint.maxDepth;
						EditorGUILayout.MinMaxSlider(ref minDepth, ref maxDepth, -playgroundScriptReference.maximumAllowedDepth, playgroundScriptReference.maximumAllowedDepth, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
						playgroundParticlesScriptReference.paint.minDepth = minDepth;
						playgroundParticlesScriptReference.paint.maxDepth = maxDepth;
						playgroundParticlesScriptReference.paint.minDepth = EditorGUILayout.FloatField(playgroundParticlesScriptReference.paint.minDepth, GUILayout.Width(50));
						playgroundParticlesScriptReference.paint.maxDepth = EditorGUILayout.FloatField(playgroundParticlesScriptReference.paint.maxDepth, GUILayout.Width(50));
						GUILayout.EndHorizontal();
					}
					EditorGUILayout.PropertyField(paintLayerMask, new GUIContent("Paint Mask"));
					playgroundParticlesScriptReference.paint.spacing = EditorGUILayout.Slider("Paint Spacing", playgroundParticlesScriptReference.paint.spacing, .0f, playgroundScriptReference.maximumAllowedPaintSpacing);
					PlaygroundC.reference.paintMaxPositions = EditorGUILayout.IntSlider("Max Paint Positions", PlaygroundC.reference.paintMaxPositions, 0, playgroundScriptReference.maximumAllowedPaintPositions);
					playgroundParticlesScriptReference.paint.exceedMaxStopsPaint = EditorGUILayout.Toggle("Exceed Max Stops Paint", playgroundParticlesScriptReference.paint.exceedMaxStopsPaint);
					if (playgroundParticlesScriptReference.paint.exceedMaxStopsPaint && playgroundParticlesScriptReference.paint.positionLength>=PlaygroundC.reference.paintMaxPositions) {
						EditorGUILayout.HelpBox("You have exceeded max positions. No new paint positions are possible when Exceed Max Stops Paint is enabled.", MessageType.Warning);
					}
					
					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Paint:");
					ProgressBar((playgroundParticlesScriptReference.paint.positionLength*1f)/PlaygroundC.reference.paintMaxPositions, playgroundParticlesScriptReference.paint.positionLength+"/"+PlaygroundC.reference.paintMaxPositions, Mathf.FloorToInt(Screen.width/2.2f)-65);
					EditorGUILayout.Separator();
					if(GUILayout.Button("Set Particle Count", EditorStyles.toolbarButton)){
						PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.paint.positionLength);
						playgroundParticlesScriptReference.Start();
					}
					if(GUILayout.Button("++", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}))
						particleCount.intValue = particleCount.intValue+playgroundParticlesScriptReference.paint.positionLength;
					GUILayout.EndHorizontal();
					
					EditorGUILayout.Separator();
					
					GUILayout.BeginHorizontal();
					GUI.enabled = !(selectedPaintMode==1 && paintTexture==null);
					if (GUILayout.Button((inPaintMode?"Stop":"Start")+" Paint ", EditorStyles.toolbarButton, GUILayout.Width(80))){
						StartStopPaint();
					}
					
					GUI.enabled = (playgroundParticlesScriptReference.paint.positionLength>0);
					if(GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(50))){
						ClearPaint();
					}
					GUI.enabled = true;
					GUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					
					if (playgroundParticlesScriptReference.paint.positionLength-1>playgroundParticlesScriptReference.particleCount)
						EditorGUILayout.HelpBox("You have more paint positions than particles. Increase Particle Count to see all painted positions.", MessageType.Warning);
					
					if (GUI.changed) {
						SetBrushStyle();
					}
					
				}
				EditorGUILayout.Separator();
				
			}
			
			// Particle Settings
			if (GUILayout.Button("Particle Settings ("+playgroundParticlesScriptReference.particleCount+")", EditorStyles.toolbarDropDown)) particleSettingsFoldout=!particleSettingsFoldout;
			if (particleSettingsFoldout) {
				
				EditorGUILayout.Separator();
				
				if (source.intValue==4)
					EditorGUILayout.HelpBox("Some features are inactivated as this Particle Playground System is running in script mode.", MessageType.Info);
				
				GUILayout.BeginHorizontal();
				particleCount.intValue = EditorGUILayout.IntSlider("Particle Count", particleCount.intValue, 0, playgroundScriptReference.maximumAllowedParticles);
				if(GUILayout.Button("x2", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}))
					particleCount.intValue *= 2;
				GUILayout.EndHorizontal();
				
				GUI.enabled=(source.intValue!=4);
				emissionRate.floatValue = EditorGUILayout.Slider("Emisson Rate", emissionRate.floatValue, 0, 1f);
				GUI.enabled=(source.intValue!=4);
				EditorGUILayout.Separator();

				EditorGUILayout.PropertyField(overflowMode, new GUIContent(
					"Overflow Mode", 
					"The method to align the Overflow Offset by.")
				);
				overflowOffset.vector3Value = EditorGUILayout.Vector3Field("Overflow Offset", overflowOffset.vector3Value);

				EditorGUILayout.Separator();
				GUI.enabled=true;
				
				// Source Scattering
				GUI.enabled=(source.intValue!=4);
				bool prevScatterEnabled = playgroundParticlesScriptReference.applySourceScatter;
				Vector3 prevScatterMin = playgroundParticlesScriptReference.sourceScatterMin;
				Vector3 prevScatterMax = playgroundParticlesScriptReference.sourceScatterMax;
				playgroundParticlesScriptReference.applySourceScatter = EditorGUILayout.ToggleLeft("Source Scatter", playgroundParticlesScriptReference.applySourceScatter);
				GUI.enabled = (source.intValue!=4 && playgroundParticlesScriptReference.applySourceScatter);
					// X
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("X", GUILayout.Width(50));
					EditorGUILayout.Separator();
					float sourceScatterMinX = playgroundParticlesScriptReference.sourceScatterMin.x;
					float sourceScatterMaxX = playgroundParticlesScriptReference.sourceScatterMax.x;
					EditorGUILayout.MinMaxSlider(ref sourceScatterMinX, ref sourceScatterMaxX, -playgroundScriptReference.maximumAllowedSourceScatter, playgroundScriptReference.maximumAllowedSourceScatter, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.sourceScatterMin.x = sourceScatterMinX;
					playgroundParticlesScriptReference.sourceScatterMax.x = sourceScatterMaxX;
					playgroundParticlesScriptReference.sourceScatterMin.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sourceScatterMin.x, GUILayout.Width(50));
					playgroundParticlesScriptReference.sourceScatterMax.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sourceScatterMax.x, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					// Y
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("Y");
					EditorGUILayout.Separator();
					float sourceScatterMinY = playgroundParticlesScriptReference.sourceScatterMin.y;
					float sourceScatterMaxY = playgroundParticlesScriptReference.sourceScatterMax.y;
					EditorGUILayout.MinMaxSlider(ref sourceScatterMinY, ref sourceScatterMaxY, -playgroundScriptReference.maximumAllowedSourceScatter, playgroundScriptReference.maximumAllowedSourceScatter, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.sourceScatterMin.y = sourceScatterMinY;
					playgroundParticlesScriptReference.sourceScatterMax.y = sourceScatterMaxY;
					playgroundParticlesScriptReference.sourceScatterMin.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sourceScatterMin.y, GUILayout.Width(50));
					playgroundParticlesScriptReference.sourceScatterMax.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sourceScatterMax.y, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					// Z
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("Z");
					EditorGUILayout.Separator();
					float sourceScatterMinZ = playgroundParticlesScriptReference.sourceScatterMin.z;
					float sourceScatterMaxZ = playgroundParticlesScriptReference.sourceScatterMax.z;
					EditorGUILayout.MinMaxSlider(ref sourceScatterMinZ, ref sourceScatterMaxZ, -playgroundScriptReference.maximumAllowedSourceScatter, playgroundScriptReference.maximumAllowedSourceScatter, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.sourceScatterMin.z = sourceScatterMinZ;
					playgroundParticlesScriptReference.sourceScatterMax.z = sourceScatterMaxZ;
					playgroundParticlesScriptReference.sourceScatterMin.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sourceScatterMin.z, GUILayout.Width(50));
					playgroundParticlesScriptReference.sourceScatterMax.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sourceScatterMax.z, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					

				GUI.enabled = true;
				
				if (prevScatterEnabled!=playgroundParticlesScriptReference.applySourceScatter || prevScatterMin!=playgroundParticlesScriptReference.sourceScatterMin || prevScatterMax!=playgroundParticlesScriptReference.sourceScatterMax) {
					LifetimeSorting();
					playgroundParticlesScriptReference.RefreshScatter();
				}
				
				EditorGUILayout.Separator();
				
				// Emission
				bool prevEmit = playgroundParticlesScriptReference.emit;
				bool prevLoop = playgroundParticlesScriptReference.loop;
				playgroundParticlesScriptReference.emit = EditorGUILayout.Toggle("Emit Particles", playgroundParticlesScriptReference.emit);
				playgroundParticlesScriptReference.loop = EditorGUILayout.Toggle("Loop", playgroundParticlesScriptReference.loop);
				if (prevEmit!=playgroundParticlesScriptReference.emit || prevLoop!=playgroundParticlesScriptReference.loop&&playgroundParticlesScriptReference.loop) {
					playgroundParticlesScriptReference.simulationStarted = PlaygroundC.globalTime;
					playgroundParticlesScriptReference.loopExceeded = false;
					playgroundParticlesScriptReference.loopExceededOnParticle = -1;
					playgroundParticlesScriptReference.particleSystemGameObject.SetActive(true);
				}
				GUI.enabled = !loop.boolValue;
				disableOnDone.boolValue = EditorGUILayout.Toggle("Disable On Done", disableOnDone.boolValue);
				GUI.enabled = true;
				//calculate.boolValue = EditorGUILayout.Toggle("Calculate Particles", calculate.boolValue);
				
				EditorGUILayout.Separator();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Size");
				EditorGUILayout.Separator();
				float sizeMin = playgroundParticlesScriptReference.sizeMin;
				float sizeMax = playgroundParticlesScriptReference.sizeMax;
				EditorGUILayout.MinMaxSlider(ref sizeMin, ref sizeMax, 0, playgroundScriptReference.maximumAllowedSize, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
				playgroundParticlesScriptReference.sizeMin = sizeMin;
				playgroundParticlesScriptReference.sizeMax = sizeMax;
				playgroundParticlesScriptReference.sizeMin = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sizeMin, GUILayout.Width(50));
				playgroundParticlesScriptReference.sizeMax = EditorGUILayout.FloatField(playgroundParticlesScriptReference.sizeMax, GUILayout.Width(50));
				GUILayout.EndHorizontal();

				playgroundParticlesScriptReference.scale = EditorGUILayout.Slider("Scale", playgroundParticlesScriptReference.scale, 0, playgroundScriptReference.maximumAllowedScale);
				EditorGUILayout.BeginHorizontal();
				playgroundParticlesScriptReference.applyLifetimeSize = EditorGUILayout.ToggleLeft ("Lifetime Size", playgroundParticlesScriptReference.applyLifetimeSize, GUILayout.Width (120));
				GUILayout.FlexibleSpace();
				GUI.enabled = playgroundParticlesScriptReference.applyLifetimeSize;
				lifetimeSize.animationCurveValue = EditorGUILayout.CurveField(lifetimeSize.animationCurveValue);
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Inital Rotation");
				EditorGUILayout.Separator();
				float initialRotationMin = playgroundParticlesScriptReference.initialRotationMin;
				float initialRotationMax = playgroundParticlesScriptReference.initialRotationMax;
				EditorGUILayout.MinMaxSlider(ref initialRotationMin, ref initialRotationMax, -playgroundScriptReference.maximumAllowedRotation, playgroundScriptReference.maximumAllowedRotation, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
				playgroundParticlesScriptReference.initialRotationMin = initialRotationMin;
				playgroundParticlesScriptReference.initialRotationMax = initialRotationMax;
				playgroundParticlesScriptReference.initialRotationMin = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialRotationMin, GUILayout.Width(50));
				playgroundParticlesScriptReference.initialRotationMax = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialRotationMax, GUILayout.Width(50));
				GUILayout.EndHorizontal();
				
				GUI.enabled = !playgroundParticlesScriptReference.rotateTowardsDirection;
				GUILayout.BeginHorizontal();
				GUILayout.Label("Rotation");
				EditorGUILayout.Separator();
				float rotationSpeedMin = playgroundParticlesScriptReference.rotationSpeedMin;
				float rotationSpeedMax = playgroundParticlesScriptReference.rotationSpeedMax;
				EditorGUILayout.MinMaxSlider(ref rotationSpeedMin, ref rotationSpeedMax, -playgroundScriptReference.maximumAllowedRotation, playgroundScriptReference.maximumAllowedRotation, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
				playgroundParticlesScriptReference.rotationSpeedMin = rotationSpeedMin;
				playgroundParticlesScriptReference.rotationSpeedMax = rotationSpeedMax;
				playgroundParticlesScriptReference.rotationSpeedMin = EditorGUILayout.FloatField(playgroundParticlesScriptReference.rotationSpeedMin, GUILayout.Width(50));
				playgroundParticlesScriptReference.rotationSpeedMax = EditorGUILayout.FloatField(playgroundParticlesScriptReference.rotationSpeedMax, GUILayout.Width(50));

				GUILayout.EndHorizontal();
				
				GUI.enabled = true;
				
				playgroundParticlesScriptReference.rotateTowardsDirection = EditorGUILayout.Toggle("Rotate Towards Direction", playgroundParticlesScriptReference.rotateTowardsDirection);
				GUI.enabled = playgroundParticlesScriptReference.rotateTowardsDirection;
					EditorGUI.indentLevel++;
					playgroundParticlesScriptReference.rotationNormal = EditorGUILayout.Vector3Field("Rotation Normal", playgroundParticlesScriptReference.rotationNormal);
					playgroundParticlesScriptReference.rotationNormal.x = Mathf.Clamp(playgroundParticlesScriptReference.rotationNormal.x, -1, 1);
					playgroundParticlesScriptReference.rotationNormal.y = Mathf.Clamp(playgroundParticlesScriptReference.rotationNormal.y, -1, 1);
					playgroundParticlesScriptReference.rotationNormal.z = Mathf.Clamp(playgroundParticlesScriptReference.rotationNormal.z, -1, 1);
					EditorGUI.indentLevel--;
				
				GUI.enabled = true;
				
				EditorGUILayout.Separator();
				
				lifetime.floatValue = EditorGUILayout.Slider("Lifetime", lifetime.floatValue, 0, playgroundScriptReference.maximumAllowedLifetime);

				// Sorting
				GUI.enabled=(source.intValue!=4);
				if (selectedSort!=sorting.intValue || selectedOrigin!=nearestNeighborOrigin.intValue) {
					LifetimeSorting();
					playgroundParticles.ApplyModifiedProperties();
				}
				selectedSort = sorting.intValue;
				selectedOrigin = nearestNeighborOrigin.intValue;
				EditorGUILayout.PropertyField(sorting, new GUIContent(
					"Lifetime Sorting", 
					"Determines how the particles are ordered on rebirth.\nScrambled: Randomly placed.\nScrambled Linear: Randomly placed but never at the same time.\nBurst: Alfa and Omega.\nLinear: Alfa to Omega.\nReversed: Omega to Alfa.")
				);
				
				if (sorting.intValue==5||sorting.intValue==6) {
					EditorGUI.indentLevel++;
					nearestNeighborOrigin.intValue = EditorGUILayout.IntSlider("Sort Origin", nearestNeighborOrigin.intValue, 0, playgroundParticlesScriptReference.particleCount>0?playgroundParticlesScriptReference.particleCount-1:0);
					EditorGUI.indentLevel--;
				}
				
				// Custom lifetime sorting
				if (sorting.intValue==7) {
					EditorGUI.indentLevel++;
					playgroundParticlesScriptReference.lifetimeSorting = EditorGUILayout.CurveField("Custom Sorting", playgroundParticlesScriptReference.lifetimeSorting);
					EditorGUI.indentLevel--;
					bool changed = prevLifetimeSortingKeys.Length!=playgroundParticlesScriptReference.lifetimeSorting.keys.Length;
					if (!changed)
						for (int k = 0; k<prevLifetimeSortingKeys.Length; k++) {
							if (playgroundParticlesScriptReference.lifetimeSorting.keys[k].value != prevLifetimeSortingKeys[k].value || playgroundParticlesScriptReference.lifetimeSorting.keys[k].time != prevLifetimeSortingKeys[k].time) {
								changed = true;
							}
						}
					if (changed) {
						LifetimeSorting();
						prevLifetimeSortingKeys = playgroundParticlesScriptReference.lifetimeSorting.keys;
					}
				}
				
				float prevLifetimeOffset = playgroundParticlesScriptReference.lifetimeOffset;
				playgroundParticlesScriptReference.lifetimeOffset = EditorGUILayout.Slider("Lifetime Offset", playgroundParticlesScriptReference.lifetimeOffset, -playgroundParticlesScriptReference.lifetime, playgroundParticlesScriptReference.lifetime);
				if (prevLifetimeOffset!=playgroundParticlesScriptReference.lifetimeOffset) {
					LifetimeSortingAll();
				}
				GUI.enabled = true;
				
				EditorGUILayout.Separator();

				playgroundParticlesScriptReference.particleMask = EditorGUILayout.IntSlider("Particle Mask", playgroundParticlesScriptReference.particleMask, 0, particleCount.intValue);
				playgroundParticlesScriptReference.particleMaskTime = EditorGUILayout.Slider("Mask Time", playgroundParticlesScriptReference.particleMaskTime, 0, PlaygroundC.reference.maximumAllowedTransitionTime);

				EditorGUILayout.Separator();
			}
						
			// Force Settings
			if (GUILayout.Button(onlySourcePositioning.boolValue||(playgroundParticlesScriptReference.axisConstraints.x&&playgroundParticlesScriptReference.axisConstraints.y&&playgroundParticlesScriptReference.axisConstraints.z)?"Forces (Off)":playgroundParticlesScriptReference.turbulenceType==TURBULENCETYPE.None?"Forces":"Forces (Turbulence)", EditorStyles.toolbarDropDown)) forcesFoldout=!forcesFoldout;
			if (forcesFoldout) {
				
				EditorGUILayout.Separator();
				
				onlySourcePositioning.boolValue = EditorGUILayout.Toggle("Only Source Positions", onlySourcePositioning.boolValue);
				if (onlySourcePositioning.boolValue)
					EditorGUILayout.HelpBox("Particles are bound to their source position during their lifetime.", MessageType.Info);
				
				EditorGUILayout.Separator();
				
				GUI.enabled = !onlySourcePositioning.boolValue;
				
				// Delta Movement
				if (playgroundParticlesScriptReference.source==SOURCEC.State && playgroundParticlesScriptReference.states!=null && playgroundParticlesScriptReference.states.Count>0 && playgroundParticlesScriptReference.states[playgroundParticlesScriptReference.activeState].stateTransform==null) {
					EditorGUILayout.HelpBox("Assign a transform to the active state to enable Delta Movement.", MessageType.Info);
					GUI.enabled = false;
				} else GUI.enabled = (source.intValue!=4 && !onlySourcePositioning.boolValue);
					playgroundParticlesScriptReference.calculateDeltaMovement = EditorGUILayout.ToggleLeft("Delta Movement", playgroundParticlesScriptReference.calculateDeltaMovement);
				GUI.enabled = (GUI.enabled && playgroundParticlesScriptReference.calculateDeltaMovement && !onlySourcePositioning.boolValue);
					EditorGUI.indentLevel++;
					deltaMovementStrength.floatValue = EditorGUILayout.Slider("Delta Movement Strength", deltaMovementStrength.floatValue, 0, playgroundScriptReference.maximumAllowedDeltaMovementStrength);
					EditorGUI.indentLevel--;
				GUI.enabled = !onlySourcePositioning.boolValue;
				EditorGUILayout.Separator();
				
				// Lifetime velocity
				applyLifetimeVelocity.boolValue = EditorGUILayout.ToggleLeft("Lifetime Velocity", applyLifetimeVelocity.boolValue);
				GUI.enabled = (applyLifetimeVelocity.boolValue&&!onlySourcePositioning.boolValue);
				EditorGUI.indentLevel++;
				lifeTimeVelocityX.animationCurveValue = EditorGUILayout.CurveField("X", lifeTimeVelocityX.animationCurveValue);
				lifeTimeVelocityY.animationCurveValue = EditorGUILayout.CurveField("Y", lifeTimeVelocityY.animationCurveValue);
				lifeTimeVelocityZ.animationCurveValue = EditorGUILayout.CurveField("Z", lifeTimeVelocityZ.animationCurveValue);
				EditorGUI.indentLevel--;
				GUI.enabled = !onlySourcePositioning.boolValue;
				EditorGUILayout.Separator();
				
				// Initial Velocity
				EditorGUILayout.Separator();
				applyInitialVelocity.boolValue = EditorGUILayout.ToggleLeft("Initial Velocity", applyInitialVelocity.boolValue);
				GUI.enabled = (applyInitialVelocity.boolValue&&!onlySourcePositioning.boolValue);
					
					// X
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("X", GUILayout.Width(50));
					EditorGUILayout.Separator();
					float initialVelocityMinX = playgroundParticlesScriptReference.initialVelocityMin.x;
					float initialVelocityMaxX = playgroundParticlesScriptReference.initialVelocityMax.x;
					EditorGUILayout.MinMaxSlider(ref initialVelocityMinX, ref initialVelocityMaxX, -playgroundScriptReference.maximumAllowedInitialVelocity, playgroundScriptReference.maximumAllowedInitialVelocity, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.initialVelocityMin.x = initialVelocityMinX;
					playgroundParticlesScriptReference.initialVelocityMax.x = initialVelocityMaxX;
					playgroundParticlesScriptReference.initialVelocityMin.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialVelocityMin.x, GUILayout.Width(50));
					playgroundParticlesScriptReference.initialVelocityMax.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialVelocityMax.x, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					// Y
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("Y");
					EditorGUILayout.Separator();
					float initialVelocityMinY = playgroundParticlesScriptReference.initialVelocityMin.y;
					float initialVelocityMaxY = playgroundParticlesScriptReference.initialVelocityMax.y;
					EditorGUILayout.MinMaxSlider(ref initialVelocityMinY, ref initialVelocityMaxY, -playgroundScriptReference.maximumAllowedInitialVelocity, playgroundScriptReference.maximumAllowedInitialVelocity, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.initialVelocityMin.y = initialVelocityMinY;
					playgroundParticlesScriptReference.initialVelocityMax.y = initialVelocityMaxY;
					playgroundParticlesScriptReference.initialVelocityMin.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialVelocityMin.y, GUILayout.Width(50));
					playgroundParticlesScriptReference.initialVelocityMax.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialVelocityMax.y, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					// Z
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("Z");
					EditorGUILayout.Separator();
					float initialVelocityMinZ = playgroundParticlesScriptReference.initialVelocityMin.z;
					float initialVelocityMaxZ = playgroundParticlesScriptReference.initialVelocityMax.z;
					EditorGUILayout.MinMaxSlider(ref initialVelocityMinZ, ref initialVelocityMaxZ, -playgroundScriptReference.maximumAllowedInitialVelocity, playgroundScriptReference.maximumAllowedInitialVelocity, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.initialVelocityMin.z = initialVelocityMinZ;
					playgroundParticlesScriptReference.initialVelocityMax.z = initialVelocityMaxZ;
					playgroundParticlesScriptReference.initialVelocityMin.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialVelocityMin.z, GUILayout.Width(50));
					playgroundParticlesScriptReference.initialVelocityMax.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialVelocityMax.z, GUILayout.Width(50));
					GUILayout.EndHorizontal();

				GUI.enabled = !onlySourcePositioning.boolValue;
				
				// Initial Local Velocity
				EditorGUILayout.Separator();
				GUI.enabled=(source.intValue!=4 && !onlySourcePositioning.boolValue);
				
				if (source.intValue==4) {
					GUI.enabled = true;
					EditorGUILayout.HelpBox("Initial Local Velocity is controlled by passed in velocity to Emit() in script mode.", MessageType.Info);
					GUI.enabled = false;
				}
				applyInitialLocalVelocity.boolValue = EditorGUILayout.ToggleLeft("Initial Local Velocity", applyInitialLocalVelocity.boolValue);
				if (playgroundParticlesScriptReference.source==SOURCEC.State && playgroundParticlesScriptReference.states!=null && playgroundParticlesScriptReference.states.Count>0 && playgroundParticlesScriptReference.states[playgroundParticlesScriptReference.activeState].stateTransform==null) {
					EditorGUILayout.HelpBox("Assign a transform to the active state to enable Initial Local Velocity.", MessageType.Info);
					GUI.enabled = false;
				} else GUI.enabled = (applyInitialLocalVelocity.boolValue&&!onlySourcePositioning.boolValue&&source.intValue!=4);
					
					// X
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("X", GUILayout.Width(50));
					EditorGUILayout.Separator();
					float initialLocalVelocityMinX = playgroundParticlesScriptReference.initialLocalVelocityMin.x;
					float initialLocalVelocityMaxX = playgroundParticlesScriptReference.initialLocalVelocityMax.x;
					EditorGUILayout.MinMaxSlider(ref initialLocalVelocityMinX, ref initialLocalVelocityMaxX, -playgroundScriptReference.maximumAllowedInitialVelocity, playgroundScriptReference.maximumAllowedInitialVelocity, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.initialLocalVelocityMin.x = initialLocalVelocityMinX;
					playgroundParticlesScriptReference.initialLocalVelocityMax.x = initialLocalVelocityMaxX;
					playgroundParticlesScriptReference.initialLocalVelocityMin.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialLocalVelocityMin.x, GUILayout.Width(50));
					playgroundParticlesScriptReference.initialLocalVelocityMax.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialLocalVelocityMax.x, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					// Y
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("Y");
					EditorGUILayout.Separator();
					float initialLocalVelocityMinY = playgroundParticlesScriptReference.initialLocalVelocityMin.y;
					float initialLocalVelocityMaxY = playgroundParticlesScriptReference.initialLocalVelocityMax.y;
					EditorGUILayout.MinMaxSlider(ref initialLocalVelocityMinY, ref initialLocalVelocityMaxY, -playgroundScriptReference.maximumAllowedInitialVelocity, playgroundScriptReference.maximumAllowedInitialVelocity, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.initialLocalVelocityMin.y = initialLocalVelocityMinY;
					playgroundParticlesScriptReference.initialLocalVelocityMax.y = initialLocalVelocityMaxY;
					playgroundParticlesScriptReference.initialLocalVelocityMin.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialLocalVelocityMin.y, GUILayout.Width(50));
					playgroundParticlesScriptReference.initialLocalVelocityMax.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialLocalVelocityMax.y, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					// Z
					GUILayout.BeginHorizontal();
					GUILayout.Space(16);
					GUILayout.Label("Z");
					EditorGUILayout.Separator();
					float initialLocalVelocityMinZ = playgroundParticlesScriptReference.initialLocalVelocityMin.z;
					float initialLocalVelocityMaxZ = playgroundParticlesScriptReference.initialLocalVelocityMax.z;
					EditorGUILayout.MinMaxSlider(ref initialLocalVelocityMinZ, ref initialLocalVelocityMaxZ, -playgroundScriptReference.maximumAllowedInitialVelocity, playgroundScriptReference.maximumAllowedInitialVelocity, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.initialLocalVelocityMin.z = initialLocalVelocityMinZ;
					playgroundParticlesScriptReference.initialLocalVelocityMax.z = initialLocalVelocityMaxZ;
					playgroundParticlesScriptReference.initialLocalVelocityMin.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialLocalVelocityMin.z, GUILayout.Width(50));
					playgroundParticlesScriptReference.initialLocalVelocityMax.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.initialLocalVelocityMax.z, GUILayout.Width(50));
					GUILayout.EndHorizontal();
					
				EditorGUILayout.Separator();
				GUI.enabled = !onlySourcePositioning.boolValue;
				
				// Initial velocity shape
				playgroundParticlesScriptReference.applyInitialVelocityShape = EditorGUILayout.ToggleLeft("Initial Velocity Shape", playgroundParticlesScriptReference.applyInitialVelocityShape);
				GUI.enabled = (playgroundParticlesScriptReference.applyInitialVelocityShape&&!onlySourcePositioning.boolValue);
				EditorGUI.indentLevel++;
				initialVelocityShapeX.animationCurveValue = EditorGUILayout.CurveField("X", initialVelocityShapeX.animationCurveValue);
				initialVelocityShapeY.animationCurveValue = EditorGUILayout.CurveField("Y", initialVelocityShapeY.animationCurveValue);
				initialVelocityShapeZ.animationCurveValue = EditorGUILayout.CurveField("Z", initialVelocityShapeZ.animationCurveValue);
				EditorGUI.indentLevel--;
				GUI.enabled = !onlySourcePositioning.boolValue;
				
				// Velocity Bending
				EditorGUILayout.Separator();
				applyVelocityBending.boolValue = EditorGUILayout.ToggleLeft("Velocity Bending", applyVelocityBending.boolValue);
				GUI.enabled = (applyVelocityBending.boolValue&&!onlySourcePositioning.boolValue);
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField (velocityBendingType, new GUIContent("Type"));
					playgroundParticlesScriptReference.velocityBending = EditorGUILayout.Vector3Field("", playgroundParticlesScriptReference.velocityBending);
					EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
				GUI.enabled = !onlySourcePositioning.boolValue;

				// Turbulence
				playgroundParticlesScriptReference.turbulenceType = (TURBULENCETYPE)EditorGUILayout.EnumPopup("Turbulence", playgroundParticlesScriptReference.turbulenceType);
				GUI.enabled = (playgroundParticlesScriptReference.turbulenceType!=TURBULENCETYPE.None && !onlySourcePositioning.boolValue);
				EditorGUI.indentLevel++;
				playgroundParticlesScriptReference.turbulenceStrength = EditorGUILayout.Slider("Strength", playgroundParticlesScriptReference.turbulenceStrength, 0f, playgroundScriptReference.maximumAllowedTurbulenceStrength);
				playgroundParticlesScriptReference.turbulenceScale = EditorGUILayout.Slider("Scale", playgroundParticlesScriptReference.turbulenceScale, 0f, playgroundScriptReference.maximumAllowedTurbulenceScale);
				playgroundParticlesScriptReference.turbulenceTimeScale = EditorGUILayout.Slider("Time Scale", playgroundParticlesScriptReference.turbulenceTimeScale, 0f, playgroundScriptReference.maximumAllowedTurbulenceTimeScale);
				EditorGUILayout.BeginHorizontal();
				playgroundParticlesScriptReference.turbulenceApplyLifetimeStrength = EditorGUILayout.ToggleLeft ("Lifetime Strength", playgroundParticlesScriptReference.turbulenceApplyLifetimeStrength, GUILayout.Width (140));
				GUILayout.FlexibleSpace();
				GUI.enabled = (playgroundParticlesScriptReference.turbulenceApplyLifetimeStrength && playgroundParticlesScriptReference.turbulenceType!=TURBULENCETYPE.None && !onlySourcePositioning.boolValue);
				turbulenceLifetimeStrength.animationCurveValue = EditorGUILayout.CurveField(turbulenceLifetimeStrength.animationCurveValue);
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
				GUI.enabled = !onlySourcePositioning.boolValue;
				EditorGUILayout.Separator();

				playgroundParticlesScriptReference.gravity = EditorGUILayout.Vector3Field("Gravity", playgroundParticlesScriptReference.gravity);
				playgroundParticlesScriptReference.damping = EditorGUILayout.Slider("Damping", playgroundParticlesScriptReference.damping, 0f, playgroundScriptReference.maximumAllowedDamping);
				playgroundParticlesScriptReference.maxVelocity = EditorGUILayout.Slider("Max Velocity", playgroundParticlesScriptReference.maxVelocity, 0, playgroundScriptReference.maximumAllowedVelocity);
				
				// Axis constraints
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Axis Constraints", GUILayout.Width(Mathf.FloorToInt(Screen.width/2.2f)-46));
				
				GUILayout.Label("X", GUILayout.Width(10));
				playgroundParticlesScriptReference.axisConstraints.x = EditorGUILayout.Toggle(playgroundParticlesScriptReference.axisConstraints.x, GUILayout.Width(16));
				GUILayout.Label("Y", GUILayout.Width(10));
				playgroundParticlesScriptReference.axisConstraints.y = EditorGUILayout.Toggle(playgroundParticlesScriptReference.axisConstraints.y, GUILayout.Width(16));
				GUILayout.Label("Z", GUILayout.Width(10));
				playgroundParticlesScriptReference.axisConstraints.z = EditorGUILayout.Toggle(playgroundParticlesScriptReference.axisConstraints.z, GUILayout.Width(16));
				GUILayout.EndHorizontal();
				GUI.enabled = true;
				
				EditorGUILayout.Separator();
		
			}

			// Collision Settings
			if (GUILayout.Button(collision.boolValue?collisionType.intValue==0?"Collision (3d)":"Collision (2d)":"Collision (Off)", EditorStyles.toolbarDropDown)) collisionFoldout=!collisionFoldout;
			if (collisionFoldout) {
				
				EditorGUILayout.Separator();
				
				collision.boolValue = EditorGUILayout.ToggleLeft("Collision", collision.boolValue);
				EditorGUI.indentLevel++;
				GUI.enabled = collision.boolValue;
				EditorGUILayout.PropertyField(collisionType, new GUIContent("Collision Type"));
				if (collisionType.enumValueIndex==1) {
					GUILayout.BeginHorizontal();
					GUILayout.Space (16);
					GUILayout.Label("Depth");
					EditorGUILayout.Separator();
					float minDepth = playgroundParticlesScriptReference.minCollisionDepth;
					float maxDepth = playgroundParticlesScriptReference.maxCollisionDepth;
					EditorGUILayout.MinMaxSlider(ref minDepth, ref maxDepth, -playgroundScriptReference.maximumAllowedDepth, playgroundScriptReference.maximumAllowedDepth, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
					playgroundParticlesScriptReference.minCollisionDepth = minDepth;
					playgroundParticlesScriptReference.maxCollisionDepth = maxDepth;
					playgroundParticlesScriptReference.minCollisionDepth = EditorGUILayout.FloatField(playgroundParticlesScriptReference.minCollisionDepth, GUILayout.Width(50));
					playgroundParticlesScriptReference.maxCollisionDepth = EditorGUILayout.FloatField(playgroundParticlesScriptReference.maxCollisionDepth, GUILayout.Width(50));
					GUILayout.EndHorizontal();
				}
				EditorGUILayout.PropertyField(collisionMask, new GUIContent("Collision Mask"));
				affectRigidbodies.boolValue = EditorGUILayout.Toggle("Collide With Rigidbodies", affectRigidbodies.boolValue);
				mass.floatValue = EditorGUILayout.Slider("Mass", mass.floatValue, 0, playgroundScriptReference.maximumAllowedMass);
				collisionRadius.floatValue = EditorGUILayout.Slider("Collision Radius", collisionRadius.floatValue, 0, playgroundScriptReference.maximumAllowedCollisionRadius);
				playgroundParticlesScriptReference.lifetimeLoss = EditorGUILayout.Slider("Lifetime Loss", playgroundParticlesScriptReference.lifetimeLoss, 0f, 1f);
				
				EditorGUILayout.Separator();
				bounciness.floatValue = EditorGUILayout.Slider("Bounciness", bounciness.floatValue, 0, playgroundScriptReference.maximumAllowedBounciness);
				EditorGUILayout.PrefixLabel("Random Bounce");
				// X
				GUILayout.BeginHorizontal();
				GUILayout.Space(32);
				GUILayout.Label("X", GUILayout.Width(50));
				EditorGUILayout.Separator();
				float bounceRandomMinX = playgroundParticlesScriptReference.bounceRandomMin.x;
				float bounceRandomMaxX = playgroundParticlesScriptReference.bounceRandomMax.x;
				EditorGUILayout.MinMaxSlider(ref bounceRandomMinX, ref bounceRandomMaxX, -1f, 1f, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
				playgroundParticlesScriptReference.bounceRandomMin.x = bounceRandomMinX;
				playgroundParticlesScriptReference.bounceRandomMax.x = bounceRandomMaxX;
				playgroundParticlesScriptReference.bounceRandomMin.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.bounceRandomMin.x, GUILayout.Width(50));
				playgroundParticlesScriptReference.bounceRandomMax.x = EditorGUILayout.FloatField(playgroundParticlesScriptReference.bounceRandomMax.x, GUILayout.Width(50));
				GUILayout.EndHorizontal();
				// Y
				GUILayout.BeginHorizontal();
				GUILayout.Space(32);
				GUILayout.Label("Y");
				EditorGUILayout.Separator();
				float bounceRandomMinY = playgroundParticlesScriptReference.bounceRandomMin.y;
				float bounceRandomMaxY = playgroundParticlesScriptReference.bounceRandomMax.y;
				EditorGUILayout.MinMaxSlider(ref bounceRandomMinY, ref bounceRandomMaxY, -1f, 1f, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
				playgroundParticlesScriptReference.bounceRandomMin.y = bounceRandomMinY;
				playgroundParticlesScriptReference.bounceRandomMax.y = bounceRandomMaxY;
				playgroundParticlesScriptReference.bounceRandomMin.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.bounceRandomMin.y, GUILayout.Width(50));
				playgroundParticlesScriptReference.bounceRandomMax.y = EditorGUILayout.FloatField(playgroundParticlesScriptReference.bounceRandomMax.y, GUILayout.Width(50));
				GUILayout.EndHorizontal();
				// Z
				GUILayout.BeginHorizontal();
				GUILayout.Space(32);
				GUILayout.Label("Z");
				EditorGUILayout.Separator();
				float bounceRandomMinZ = playgroundParticlesScriptReference.bounceRandomMin.z;
				float bounceRandomMaxZ = playgroundParticlesScriptReference.bounceRandomMax.z;
				EditorGUILayout.MinMaxSlider(ref bounceRandomMinZ, ref bounceRandomMaxZ, -1f, 1f, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
				playgroundParticlesScriptReference.bounceRandomMin.z = bounceRandomMinZ;
				playgroundParticlesScriptReference.bounceRandomMax.z = bounceRandomMaxZ;
				playgroundParticlesScriptReference.bounceRandomMin.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.bounceRandomMin.z, GUILayout.Width(50));
				playgroundParticlesScriptReference.bounceRandomMax.z = EditorGUILayout.FloatField(playgroundParticlesScriptReference.bounceRandomMax.z, GUILayout.Width(50));
				GUILayout.EndHorizontal();
				
				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
				
				// Collision planes List
				EditorGUILayout.BeginVertical(boxStyle);
				collisionPlanesFoldout = GUILayout.Toggle(collisionPlanesFoldout, "Collision Planes ("+playgroundParticlesScriptReference.colliders.Count+")", EditorStyles.foldout);
				if (collisionPlanesFoldout) {
					if (playgroundParticlesScriptReference.colliders.Count>0) {
						for (int c = 0; c<playgroundParticlesScriptReference.colliders.Count; c++) {
							EditorGUILayout.BeginVertical(boxStyle, GUILayout.MinHeight(26));
							EditorGUILayout.BeginHorizontal();
							
							playgroundParticlesScriptReference.colliders[c].enabled = EditorGUILayout.Toggle("", playgroundParticlesScriptReference.colliders[c].enabled, GUILayout.Width(16));
							GUI.enabled = (playgroundParticlesScriptReference.colliders[c].enabled&&collision.boolValue);
							playgroundParticlesScriptReference.colliders[c].transform = EditorGUILayout.ObjectField("", playgroundParticlesScriptReference.colliders[c].transform, typeof(Transform), true) as Transform;
							playgroundParticlesScriptReference.colliders[c].offset = EditorGUILayout.Vector3Field("", playgroundParticlesScriptReference.colliders[c].offset, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-142));
							GUI.enabled = collision.boolValue;
							
							EditorGUILayout.Separator();
							if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
								playgroundParticlesScriptReference.colliders.RemoveAt(c);
							}
							
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.EndVertical();
						}
					} else {
						EditorGUILayout.HelpBox("No collision planes created.", MessageType.Info);
					}
					
					if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))){
						playgroundParticlesScriptReference.colliders.Add(new PlaygroundColliderC());
					}
					
					EditorGUILayout.Separator();
					playgroundScriptReference.collisionPlaneScale = EditorGUILayout.Slider("Gizmo Scale", playgroundScriptReference.collisionPlaneScale, 0, 1);
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				GUI.enabled = true;
				
				EditorGUILayout.Separator();
			}
			
			// Render Settings
			if (GUILayout.Button("Rendering ("+playgroundParticlesScriptReference.colorSource+")", EditorStyles.toolbarDropDown)) renderingFoldout=!renderingFoldout;
			if (renderingFoldout) {
				
				EditorGUILayout.Separator();
				
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Material");
				Material currentMat = particleMaterial as Material;
				particleMaterial = EditorGUILayout.ObjectField(particleMaterial, typeof(Material), false);
				if (currentMat!=particleMaterial) 
					PlaygroundParticlesC.SetMaterial(playgroundParticlesScriptReference, particleMaterial as Material);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginVertical (boxStyle);
				EditorGUILayout.PropertyField(colorSource, new GUIContent("Color Source"));
				switch (playgroundParticlesScriptReference.colorSource) {
				case COLORSOURCEC.Source: 
					EditorGUILayout.PropertyField(lifetimeColor, new GUIContent("Lifetime Color"));
					playgroundParticlesScriptReference.sourceUsesLifetimeAlpha = EditorGUILayout.Toggle("Source Uses Lifetime Alpha", playgroundParticlesScriptReference.sourceUsesLifetimeAlpha);
					break;
				case COLORSOURCEC.LifetimeColor: 
					EditorGUILayout.PropertyField(lifetimeColor, new GUIContent("Lifetime Color")); 
					break;
				case COLORSOURCEC.LifetimeColors:
					
					if (lifetimeColors.arraySize>0) {
						
						SerializedProperty thisLifetimeColor;
						for (int c = 0; c<lifetimeColors.arraySize; c++) {
							thisLifetimeColor = lifetimeColors.GetArrayElementAtIndex(c).FindPropertyRelative("gradient");
							GUILayout.BeginHorizontal(boxStyle);
							EditorGUILayout.PropertyField (thisLifetimeColor);
							if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})) {
								lifetimeColors.DeleteArrayElementAtIndex(c);
								playgroundParticles.ApplyModifiedProperties();
								
							}
							GUILayout.EndHorizontal();
						}
						
					} else {
						EditorGUILayout.HelpBox("No lifetime colors created.", MessageType.Info);
					}
					if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))) {
						playgroundParticlesScriptReference.lifetimeColors.Add (new PlaygroundGradientC());
						playgroundParticlesScriptReference.lifetimeColors[playgroundParticlesScriptReference.lifetimeColors.Count-1].gradient = new Gradient();
						playgroundParticles.ApplyModifiedProperties();
					}
					break;
				}
				
				GUILayout.EndVertical ();
				
				EditorGUILayout.Separator();
				
				GUILayout.BeginVertical(boxStyle);
				
				// Render mode
				shurikenRenderer.renderMode = (ParticleSystemRenderMode)EditorGUILayout.EnumPopup("Render Mode", shurikenRenderer.renderMode);
				switch (shurikenRenderer.renderMode) {
				case ParticleSystemRenderMode.Stretch:
					EditorGUI.indentLevel++;
					shurikenRenderer.cameraVelocityScale = EditorGUILayout.Slider("Camera Scale", shurikenRenderer.cameraVelocityScale, -playgroundScriptReference.maximumRenderSliders, playgroundScriptReference.maximumRenderSliders);
					shurikenRenderer.velocityScale = EditorGUILayout.Slider("Speed Scale", shurikenRenderer.velocityScale, -playgroundScriptReference.maximumRenderSliders, playgroundScriptReference.maximumRenderSliders);
					shurikenRenderer.lengthScale = EditorGUILayout.Slider("Length Scale", shurikenRenderer.lengthScale, -playgroundScriptReference.maximumRenderSliders, playgroundScriptReference.maximumRenderSliders);
					playgroundParticlesScriptReference.stretchSpeed = EditorGUILayout.Slider("Stretch Speed", playgroundParticlesScriptReference.stretchSpeed, .1f, playgroundScriptReference.maximumAllowedStretchSpeed);
					playgroundParticlesScriptReference.stretchStartDirection = EditorGUILayout.Vector3Field ("Start Stretch", playgroundParticlesScriptReference.stretchStartDirection);
					EditorGUILayout.BeginHorizontal();
					playgroundParticlesScriptReference.applyLifetimeStretching = EditorGUILayout.ToggleLeft ("Lifetime Stretch", playgroundParticlesScriptReference.applyLifetimeStretching, GUILayout.Width (140));
					GUILayout.FlexibleSpace();
					GUI.enabled = (playgroundParticlesScriptReference.applyLifetimeStretching);
					lifetimeStretching.animationCurveValue = EditorGUILayout.CurveField(lifetimeStretching.animationCurveValue);
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel--;
					break;
				case ParticleSystemRenderMode.Mesh:
					shurikenRenderer.mesh = EditorGUILayout.ObjectField(shurikenRenderer.mesh, typeof(Mesh), false) as Mesh;
					GUI.enabled = false;
					break;
				}
				GUI.enabled = true;
				shurikenRenderer.maxParticleSize = EditorGUILayout.Slider("Max Particle Size", shurikenRenderer.maxParticleSize, 0f, 1f);

				GUILayout.EndVertical();
				
				// Sort order/layer
				/*
				GUILayout.BeginVertical(boxStyle);
				playgroundParticlesScriptReference.particleSystemRenderer.sortingOrder = EditorGUILayout.IntField("Sorting Order", playgroundParticlesScriptReference.particleSystemRenderer.sortingOrder);
				playgroundParticlesScriptReference.particleSystemRenderer.sortingLayerName = EditorGUILayout.TextField("Sorting Layer Name", playgroundParticlesScriptReference.particleSystemRenderer.sortingLayerName);
				GUILayout.EndVertical();
				*/
				EditorGUILayout.Separator();
			}
			
			// Manipulators Settings
			if (GUILayout.Button("Manipulators ("+playgroundParticlesScriptReference.manipulators.Count+")", EditorStyles.toolbarDropDown)) manipulatorsFoldout=!manipulatorsFoldout;
			if (manipulatorsFoldout) {
				
				EditorGUILayout.Separator();
				
				if (playgroundParticlesScriptReference.manipulators.Count>0) {
					string mName;	
					for (int i = 0; i<playgroundParticlesScriptReference.manipulators.Count; i++) {
						if (!playgroundParticlesScriptReference.manipulators[i].enabled)
							GUI.contentColor = Color.gray;
						if (playgroundParticlesScriptReference.manipulators[i].transform.available) {
							mName = playgroundParticlesScriptReference.manipulators[i].transform.transform.name;
							if (mName.Length>24)
								mName = mName.Substring(0, 24)+"...";
						} else {
							GUI.color = Color.red;
							mName = "(Missing Transform!)";
						}
						EditorGUILayout.BeginVertical("box");
						
						EditorGUILayout.BeginHorizontal();
						
						GUILayout.Label(i.ToString(), EditorStyles.miniLabel, GUILayout.Width(18));
						manipulatorListFoldout[i] = GUILayout.Toggle(manipulatorListFoldout[i], PlaygroundInspectorC.ManipulatorTypeName(playgroundParticlesScriptReference.manipulators[i].type), EditorStyles.foldout, GUILayout.Width(Screen.width/4));
						if (playgroundParticlesScriptReference.manipulators[i].transform.available) {
							if (GUILayout.Button(" ("+mName+")", EditorStyles.label)) {
								Selection.activeGameObject = playgroundParticlesScriptReference.manipulators[i].transform.transform.gameObject;
							}
						} else {
							GUILayout.Button(PlaygroundInspectorC.ManipulatorTypeName(playgroundParticlesScriptReference.manipulators[i].type)+" (Missing Transform!)", EditorStyles.label);
						}
						GUI.contentColor = Color.white;
						EditorGUILayout.Separator();
						GUI.enabled = (playgroundParticlesScriptReference.manipulators.Count>1);
						if(GUILayout.Button("U", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							manipulators.MoveArrayElement(i, i==0?playgroundParticlesScriptReference.manipulators.Count-1:i-1);
						}
						if(GUILayout.Button("D", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							manipulators.MoveArrayElement(i, i<playgroundParticlesScriptReference.manipulators.Count-1?i+1:0);
						}
						GUI.enabled = true;
						if(GUILayout.Button("+", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							playgroundParticlesScriptReference.manipulators.Add(playgroundParticlesScriptReference.manipulators[i].Clone());
							manipulatorListFoldout.Add(manipulatorListFoldout[i]);
						}
						if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							
							if (playgroundParticlesScriptReference.manipulators[i].transform.transform==null || EditorUtility.DisplayDialog(
								"Remove "+PlaygroundInspectorC.ManipulatorTypeName(playgroundParticlesScriptReference.manipulators[i].type)+" Manipulator "+i+"?",
								"Are you sure you want to remove the Manipulator assigned to "+mName+"? (GameObject in Scene will remain intact)", 
								"Yes", "No")) {
									manipulators.DeleteArrayElementAtIndex(i);
									manipulatorListFoldout.RemoveAt(i);
									playgroundParticles.ApplyModifiedProperties();
									return;
								}
						}

						GUI.color = Color.white;
						
						EditorGUILayout.EndHorizontal();
						
						if (manipulatorListFoldout[i] && i<manipulators.arraySize) {
							PlaygroundInspectorC.RenderManipulatorSettings(playgroundParticlesScriptReference.manipulators[i], manipulators.GetArrayElementAtIndex(i), false);
						}

						GUI.enabled = true;
						EditorGUILayout.Separator();
						EditorGUILayout.EndVertical();
					}
					
				} else {
					EditorGUILayout.HelpBox("No manipulators created.", MessageType.Info);
				}
				
				if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))){
					if (Selection.gameObjects.Length>0)
						PlaygroundC.ManipulatorObject(null, playgroundParticlesScriptReference);
					else
						manipulators.InsertArrayElementAtIndex(manipulators.arraySize);
					manipulatorListFoldout.Add(true);
					SceneView.RepaintAll();
				}
				
				EditorGUILayout.Separator();
			}

			// Event Settings
			if (GUILayout.Button("Events ("+playgroundParticlesScriptReference.events.Count+")", EditorStyles.toolbarDropDown)) eventsFoldout=!eventsFoldout;
			if (eventsFoldout) {
				
				EditorGUILayout.Separator();

				if (playgroundParticlesScriptReference.events.Count>0) {
					string eName;
					for (int i = 0; i<playgroundParticlesScriptReference.events.Count; i++) {
						if (playgroundParticlesScriptReference.events[i].broadcastType!=EVENTBROADCASTC.EventListeners) {
							if (playgroundParticlesScriptReference.events[i].target!=null) {
								eName = playgroundParticlesScriptReference.events[i].target.name;
								if (eName.Length>24)
									eName = eName.Substring(0, 24)+"...";
							} else eName = "(No target)";
						} else eName = "(Event Listeners)";

						EditorGUILayout.BeginVertical("box");
						
						EditorGUILayout.BeginHorizontal();
						
						GUILayout.Label(i.ToString(), EditorStyles.miniLabel, GUILayout.Width(18));
						eventListFoldout[i] = GUILayout.Toggle(eventListFoldout[i], playgroundParticlesScriptReference.events[i].eventType.ToString(), EditorStyles.foldout, GUILayout.Width(Screen.width/4));
						if (playgroundParticlesScriptReference.events[i].target!=null) {
							if (GUILayout.Button(" ("+eName+")", EditorStyles.label)) {
								Selection.activeGameObject = playgroundParticlesScriptReference.events[i].target.gameObject;
							}
						} else {
							GUILayout.Button(eName, EditorStyles.label);
						}

						EditorGUILayout.Separator();
						GUI.enabled = (playgroundParticlesScriptReference.events.Count>1);
						if(GUILayout.Button("U", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							events.MoveArrayElement(i, i==0?playgroundParticlesScriptReference.events.Count-1:i-1);
						}
						if(GUILayout.Button("D", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							events.MoveArrayElement(i, i<playgroundParticlesScriptReference.events.Count-1?i+1:0);
						}
						GUI.enabled = true;
						if(GUILayout.Button("+", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							playgroundParticlesScriptReference.events.Add(playgroundParticlesScriptReference.events[i].Clone());
							eventListFoldout.Add(eventListFoldout[i]);
						}
						if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
							
							if (playgroundParticlesScriptReference.events[i].target==null || EditorUtility.DisplayDialog(
								"Remove "+playgroundParticlesScriptReference.events[i].eventType.ToString()+" Event "+i+"?",
								"Are you sure you want to remove this event?", 
								"Yes", "No")) {
								PlaygroundC.RemoveEvent (i, playgroundParticlesScriptReference);
								eventListFoldout.RemoveAt(i);
								playgroundParticles.ApplyModifiedProperties();
								return;
							}
						}
						
						EditorGUILayout.EndHorizontal();
						
						if (eventListFoldout[i] && i<events.arraySize) {
							RenderEventSettings(playgroundParticlesScriptReference.events[i], events.GetArrayElementAtIndex(i));
						}
						GUI.enabled = true;
						EditorGUILayout.Separator();
						EditorGUILayout.EndVertical();
					}
				} else {
					EditorGUILayout.HelpBox("No events created.", MessageType.Info);
				}

				if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))){
					PlaygroundC.CreateEvent(playgroundParticlesScriptReference);
					eventListFoldout.Add(true);
				}
				
				EditorGUILayout.Separator();
			}

			// Snapshot Settings
			if (!playgroundParticlesScriptReference.isSnapshot) {
				if (GUILayout.Button("Snapshots ("+playgroundParticlesScriptReference.snapshots.Count+")", EditorStyles.toolbarDropDown)) saveLoadFoldout=!saveLoadFoldout;
				if (saveLoadFoldout) {

					EditorGUILayout.Separator();
					bool setThisLoadFrom = false;
					string loadModeButton = "";
					if (playgroundParticlesScriptReference.snapshots.Count>0) {
						GUILayout.BeginHorizontal();
						playgroundParticlesScriptReference.loadTransition = EditorGUILayout.ToggleLeft("Transition Time", playgroundParticlesScriptReference.loadTransition, GUILayout.Width (Mathf.CeilToInt((Screen.width-140)/2)));
						GUI.enabled = playgroundParticlesScriptReference.loadTransition;
						playgroundParticlesScriptReference.loadTransitionTime = EditorGUILayout.Slider(playgroundParticlesScriptReference.loadTransitionTime, 0, PlaygroundC.reference.maximumAllowedTransitionTime);
						GUILayout.EndHorizontal();
						EditorGUI.indentLevel++;
						playgroundParticlesScriptReference.loadTransitionType = (TRANSITIONTYPEC)EditorGUILayout.EnumPopup("Transition Type", playgroundParticlesScriptReference.loadTransitionType);
						EditorGUI.indentLevel--;
						GUI.enabled = true;
						EditorGUILayout.Separator();
						playgroundParticlesScriptReference.loadFromStart = EditorGUILayout.ToggleLeft("Load From Start", playgroundParticlesScriptReference.loadFromStart);

						for (int i = 0; i<playgroundParticlesScriptReference.snapshots.Count; i++) {
							setThisLoadFrom = false;
							GUILayout.BeginHorizontal(boxStyle);

							if (playgroundParticlesScriptReference.loadFrom == i) {
								EditorGUILayout.Toggle (true, EditorStyles.radioButton, GUILayout.Width(14));
							} else
								setThisLoadFrom = EditorGUILayout.Toggle (setThisLoadFrom, EditorStyles.radioButton, GUILayout.Width(14));
							if (setThisLoadFrom)
								playgroundParticlesScriptReference.loadFrom = i;
							GUILayout.Label(i.ToString(), EditorStyles.miniLabel, GUILayout.Width(18));
							if (PlaygroundC.reference.showSnapshotsInHierarchy) {
								PlaygroundParticlesC currentSnapshot = playgroundParticlesScriptReference.snapshots[i].settings;
								playgroundParticlesScriptReference.snapshots[i].settings = (PlaygroundParticlesC)EditorGUILayout.ObjectField(playgroundParticlesScriptReference.snapshots[i].settings, typeof(PlaygroundParticlesC), true, GUILayout.MaxWidth (220));
								if (playgroundParticlesScriptReference.snapshots[i].settings!=currentSnapshot) {
									if (!playgroundParticlesScriptReference.snapshots[i].settings.isSnapshot) {
										EditorUtility.DisplayDialog(
											playgroundParticlesScriptReference.snapshots[i].settings.name+" is not a snapshot.",
											"You can only add snapshots into this slot. First create a snapshot from the new particle system you wish to assign.",
											"Ok"
										);
										playgroundParticlesScriptReference.snapshots[i].settings = currentSnapshot;
										continue;
									}
								}
								playgroundParticlesScriptReference.snapshots[i].settings.name = EditorGUILayout.TextField(playgroundParticlesScriptReference.snapshots[i].settings.name, EditorStyles.toolbarTextField);
								playgroundParticlesScriptReference.snapshots[i].name = playgroundParticlesScriptReference.snapshots[i].settings.name;
								GUI.enabled = playgroundParticlesScriptReference.loadTransition;
								playgroundParticlesScriptReference.snapshots[i].transitionType = (INDIVIDUALTRANSITIONTYPEC)EditorGUILayout.EnumPopup(playgroundParticlesScriptReference.snapshots[i].transitionType, GUILayout.MaxWidth (100));
								playgroundParticlesScriptReference.snapshots[i].transitionMultiplier = EditorGUILayout.FloatField(playgroundParticlesScriptReference.snapshots[i].transitionMultiplier, GUILayout.MaxWidth (30));
								GUI.enabled = true;
							} else {
								if (GUILayout.Button(playgroundParticlesScriptReference.snapshots[i].settings.name, EditorStyles.label, GUILayout.MinWidth (100)))
									playgroundParticlesScriptReference.loadFrom = i;
							}

							playgroundParticlesScriptReference.snapshots[i].loadTransform = EditorGUILayout.ToggleLeft ("Transform", playgroundParticlesScriptReference.snapshots[i].loadTransform, EditorStyles.miniButton, GUILayout.Width(80));
							switch (playgroundParticlesScriptReference.snapshots[i].loadMode) {
							case 0: loadModeButton = "Settings & Particles"; break;
							case 1: loadModeButton = "Settings Only"; break;
							case 2: loadModeButton = "Particles Only"; break;
							default: loadModeButton = "Settings & Particles"; break;
							}
							if (GUILayout.Button(loadModeButton, EditorStyles.toolbarButton, GUILayout.Width(110))){
								playgroundParticlesScriptReference.snapshots[i].loadMode++;
								playgroundParticlesScriptReference.snapshots[i].loadMode = playgroundParticlesScriptReference.snapshots[i].loadMode%3;
							}
							GUI.enabled = (playgroundParticlesScriptReference.snapshots.Count>1);
							if(GUILayout.Button("U", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
								snapshots.MoveArrayElement(i, i==0?playgroundParticlesScriptReference.snapshots.Count-1:i-1);
							}
							if(GUILayout.Button("D", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
								snapshots.MoveArrayElement(i, i<playgroundParticlesScriptReference.snapshots.Count-1?i+1:0);
							}
							GUI.enabled = true;
							if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
								if (EditorUtility.DisplayDialog(
									"Remove "+playgroundParticlesScriptReference.snapshots[i].name+"?",
									"Are you sure you want to remove the snapshot "+playgroundParticlesScriptReference.snapshots[i].name+" at list position "+i.ToString()+"?", 
									"Yes", "No")) {
									DestroyImmediate (playgroundParticlesScriptReference.snapshots[i].settings.gameObject);
									playgroundParticlesScriptReference.snapshots.RemoveAt(i);
									if (playgroundParticlesScriptReference.loadFrom>=playgroundParticlesScriptReference.snapshots.Count)
										playgroundParticlesScriptReference.loadFrom = playgroundParticlesScriptReference.snapshots.Count-1;
									return;
								}
							}
							GUILayout.EndHorizontal();
						}
						EditorGUILayout.Separator();
					} else {
						EditorGUILayout.HelpBox("No snapshots created.", MessageType.Info);
					}

					GUILayout.BeginHorizontal();
					if(GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(50))){
						saveName = "New Snapshot "+(playgroundParticlesScriptReference.snapshots.Count+1).ToString();
						playgroundParticlesScriptReference.Save(saveName);
						if (playgroundParticlesScriptReference.loadFrom>=playgroundParticlesScriptReference.snapshots.Count && playgroundParticlesScriptReference.snapshots.Count>0)
							playgroundParticlesScriptReference.loadFrom = playgroundParticlesScriptReference.snapshots.Count-1;
					}
					GUI.enabled = (playgroundParticlesScriptReference.snapshots.Count>0);
					if(GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(50))){
						playgroundParticlesScriptReference.Load(playgroundParticlesScriptReference.loadFrom);
					}
					GUILayout.FlexibleSpace();
					if(GUILayout.Button(playgroundScriptReference.showSnapshotsInHierarchy?"Simple":"Advanced", EditorStyles.toolbarButton, GUILayout.Width(70))){
						playgroundScriptReference.showSnapshotsInHierarchy = !playgroundScriptReference.showSnapshotsInHierarchy;
						PlaygroundInspectorC.UpdateSnapshots();
					}
					if(GUILayout.Button("Remove All", EditorStyles.toolbarButton, GUILayout.Width(70))){
						if (EditorUtility.DisplayDialog(
							"Remove all snapshots?",
							"Are you sure you want to remove all snapshot?", 
							"Yes", "No")) {
							for (int s = 0; s<playgroundParticlesScriptReference.snapshots.Count; s++) {
								DestroyImmediate (playgroundParticlesScriptReference.snapshots[s].settings.gameObject);
							}
							playgroundParticlesScriptReference.snapshots.Clear ();
							return;
						}
					}
					GUI.enabled = true;
					GUILayout.EndHorizontal();

					EditorGUILayout.Separator();
				}
			}

			// Advanced Settings
			if (GUILayout.Button("Advanced ("+playgroundParticlesScriptReference.particleSystem.simulationSpace+" Space)", EditorStyles.toolbarDropDown)) advancedFoldout=!advancedFoldout;
			if (advancedFoldout) {
				
				EditorGUILayout.Separator();
				
				// Update rate
				updateRate.intValue = EditorGUILayout.IntSlider("Update Rate (Frames)", updateRate.intValue, playgroundScriptReference.minimumAllowedUpdateRate, 1);
				
				EditorGUILayout.Separator();
				GUILayout.BeginVertical(boxStyle);
				GUI.enabled = (playgroundParticlesScriptReference.source!=SOURCEC.Projection);
				playgroundParticlesScriptReference.particleSystem.simulationSpace = (ParticleSystemSimulationSpace)EditorGUILayout.EnumPopup("Simulation Space", playgroundParticlesScriptReference.particleSystem.simulationSpace);
				GUI.enabled = true;
				if (playgroundParticlesScriptReference.particleSystem.simulationSpace==ParticleSystemSimulationSpace.Local && playgroundParticlesScriptReference.source!=SOURCEC.Projection) {
					/*
					if (playgroundParticlesScriptReference.source!=SOURCEC.Transform || playgroundParticlesScriptReference.source==SOURCEC.Transform && playgroundParticlesScriptReference.sourceTransform!=playgroundParticlesScriptReference.particleSystemTransform)
						if ((playgroundParticlesScriptReference.particleSystemTransform.position!=Vector3.zero || playgroundParticlesScriptReference.particleSystemTransform.rotation.eulerAngles!=Vector3.zero))
							EditorGUILayout.HelpBox("Keep the particle system global transform position and rotation at Vector3.zero to not offset your particles from your source position.", MessageType.Warning);
					*/

					playgroundParticlesScriptReference.applyLocalSpaceMovementCompensation = EditorGUILayout.ToggleLeft ("Movement Compensation", playgroundParticlesScriptReference.applyLocalSpaceMovementCompensation);
					GUI.enabled = playgroundParticlesScriptReference.applyLocalSpaceMovementCompensation;
					GUILayout.BeginHorizontal();
					EditorGUI.indentLevel++;
					playgroundParticlesScriptReference.applyMovementCompensationLifetimeStrength = EditorGUILayout.ToggleLeft ("Compensation Lifetime Strength", playgroundParticlesScriptReference.applyMovementCompensationLifetimeStrength);
					GUI.enabled = playgroundParticlesScriptReference.applyLocalSpaceMovementCompensation && playgroundParticlesScriptReference.applyMovementCompensationLifetimeStrength;
					movementCompensationLifetimeStrength.animationCurveValue = EditorGUILayout.CurveField(movementCompensationLifetimeStrength.animationCurveValue);
					GUILayout.EndHorizontal();
					GUI.enabled = true;
					EditorGUI.indentLevel--;

				} else if (playgroundParticlesScriptReference.source==SOURCEC.Projection) {
					EditorGUILayout.HelpBox("Projection can only run in world space.", MessageType.Info);
				}

				GUILayout.EndVertical();

				EditorGUILayout.Separator();
				GUILayout.BeginVertical(boxStyle);
				EditorGUILayout.LabelField("Rebirth Options");
				playgroundParticlesScriptReference.applyRandomSizeOnRebirth = EditorGUILayout.Toggle ("Random Size", playgroundParticlesScriptReference.applyRandomSizeOnRebirth);
				playgroundParticlesScriptReference.applyRandomRotationOnRebirth = EditorGUILayout.Toggle ("Random Rotation", playgroundParticlesScriptReference.applyRandomRotationOnRebirth);
				playgroundParticlesScriptReference.applyRandomScatterOnRebirth = EditorGUILayout.Toggle ("Random Scatter", playgroundParticlesScriptReference.applyRandomScatterOnRebirth);
				playgroundParticlesScriptReference.applyRandomInitialVelocityOnRebirth = EditorGUILayout.Toggle ("Random Velocity", playgroundParticlesScriptReference.applyRandomInitialVelocityOnRebirth);
				playgroundParticlesScriptReference.applyInitialColorOnRebirth = EditorGUILayout.Toggle ("Force Initial Color", playgroundParticlesScriptReference.applyInitialColorOnRebirth);
				GUILayout.EndVertical();
				EditorGUILayout.Separator();

				GUILayout.BeginVertical(boxStyle);
				playgroundParticlesScriptReference.applyLockPosition = EditorGUILayout.ToggleLeft("Lock Position", playgroundParticlesScriptReference.applyLockPosition);
				GUI.enabled = playgroundParticlesScriptReference.applyLockPosition;
				playgroundParticlesScriptReference.lockPosition = EditorGUILayout.Vector3Field ("Position", playgroundParticlesScriptReference.lockPosition);
				playgroundParticlesScriptReference.lockPositionIsLocal = EditorGUILayout.Toggle("Position Is Local", playgroundParticlesScriptReference.lockPositionIsLocal);
				GUI.enabled = true;
				EditorGUILayout.Separator();
				playgroundParticlesScriptReference.applyLockRotation = EditorGUILayout.ToggleLeft("Lock Rotation", playgroundParticlesScriptReference.applyLockRotation);
				GUI.enabled = playgroundParticlesScriptReference.applyLockRotation;
				playgroundParticlesScriptReference.lockRotation = EditorGUILayout.Vector3Field ("Rotation", playgroundParticlesScriptReference.lockRotation);
				playgroundParticlesScriptReference.lockRotationIsLocal = EditorGUILayout.Toggle("Rotation Is Local", playgroundParticlesScriptReference.lockRotationIsLocal);
				GUI.enabled = true;
				EditorGUILayout.Separator();
				playgroundParticlesScriptReference.applyLockScale = EditorGUILayout.ToggleLeft("Lock Scale", playgroundParticlesScriptReference.applyLockScale);
				GUI.enabled = playgroundParticlesScriptReference.applyLockScale;
				playgroundParticlesScriptReference.lockScale = EditorGUILayout.Vector3Field ("Scale", playgroundParticlesScriptReference.lockScale);
				GUI.enabled = true;
				GUILayout.EndVertical();
				EditorGUILayout.Separator();

				playgroundParticlesScriptReference.syncPositionsOnMainThread = EditorGUILayout.Toggle ("Sync Threaded Positions", playgroundParticlesScriptReference.syncPositionsOnMainThread);
				playgroundParticlesScriptReference.pauseCalculationWhenInvisible = EditorGUILayout.Toggle ("Auto-Pause Calculation", playgroundParticlesScriptReference.pauseCalculationWhenInvisible);
				EditorGUILayout.Separator();

				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Particle Pool");
				
				// Clear
				if(GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(50))){
					PlaygroundParticlesC.Clear(playgroundParticlesScriptReference);
				}
				
				// Rebuild
				if(GUILayout.Button("Rebuild", EditorStyles.toolbarButton, GUILayout.Width(50))){
					PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.particleCount);
					playgroundParticlesScriptReference.Start();
					//LifetimeSorting();
				}
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				
			}

			EditorGUILayout.EndVertical();
			
			if (playgroundParticlesScriptReference.shurikenParticleSystem.isPaused || playgroundParticlesScriptReference.shurikenParticleSystem.isStopped)
				playgroundParticlesScriptReference.shurikenParticleSystem.Play();
			
			previousSource = playgroundParticlesScriptReference.source;
			playgroundParticles.ApplyModifiedProperties();
			
		}
		
		EditorGUILayout.EndVertical();
		
		// Playground Manager - Particle Systems, Manipulators
		PlaygroundInspectorC.RenderPlaygroundSettings();

		// Wireframes in Scene View
		if (currentWireframe!=PlaygroundC.reference.drawWireframe)
			SetWireframeVisibility();
	}
	
	public void ProgressBar (float val, string label, float width) {
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		rect.width = width;
		rect.height = 16;
		if (val<0) val = 0;
		EditorGUI.ProgressBar (rect, val, label);
		EditorGUILayout.Space ();
	}

	bool triedToAssignSelfTarget = false;
	bool triedToAssignSnapshot = false;
	public void RenderEventSettings (PlaygroundEventC thisEvent, SerializedProperty serializedEvent) {
		thisEvent.enabled = EditorGUILayout.ToggleLeft("Enabled", thisEvent.enabled);
		GUI.enabled = thisEvent.enabled;

		// Event Broadcast Type
		EditorGUILayout.PropertyField(serializedEvent.FindPropertyRelative("broadcastType"), new GUIContent("Broadcast Type", "Set to broadcast to a Target and/or Event Listeners."));

		// Target
		if (thisEvent.broadcastType!=EVENTBROADCASTC.EventListeners) {
			PlaygroundParticlesC currentTarget = thisEvent.target;
			thisEvent.target = EditorGUILayout.ObjectField("Target", thisEvent.target, typeof(PlaygroundParticlesC), true) as PlaygroundParticlesC;
			if (currentTarget!=thisEvent.target && thisEvent.target!=null) {

				// Assign new target
				if (thisEvent.target == playgroundParticlesScriptReference) {
					thisEvent.target = null;
					triedToAssignSelfTarget = true;
					triedToAssignSnapshot = false;
				} else if (thisEvent.target.isSnapshot) {
					thisEvent.target = null;
					triedToAssignSnapshot = true;
					triedToAssignSelfTarget = false;
				} else {
					triedToAssignSelfTarget = false;
					triedToAssignSnapshot = false;
					if (thisEvent.target.source!=SOURCEC.Script && EditorUtility.DisplayDialog("Switch to Script Mode?", "The event target of "+thisEvent.target.name+" is running in "+thisEvent.target.source.ToString()+" mode. All events must be received by Script Mode.", "Switch", "Cancel"))
						thisEvent.target.source = SOURCEC.Script;
				}
			}
			if (triedToAssignSelfTarget)
				EditorGUILayout.HelpBox("A particle system cannot send events to itself. Please choose another particle system in your Scene.", MessageType.Warning);
			else if (triedToAssignSnapshot)
				EditorGUILayout.HelpBox("A particle system cannot send events to a snapshot. Please choose another particle system in your Scene.", MessageType.Warning);
		}

		EditorGUILayout.Separator();

		// Type
		EditorGUILayout.PropertyField(serializedEvent.FindPropertyRelative("eventType"), new GUIContent("Type", "The type of event."));

		// Type: Collision
		if (thisEvent.eventType==EVENTTYPEC.Collision) {
			if (!playgroundParticlesScriptReference.collision)
				EditorGUILayout.HelpBox("You must enable collision on this particle system to send collision events.", MessageType.Info);
			thisEvent.collisionThreshold = EditorGUILayout.FloatField ("Collision Threshold", thisEvent.collisionThreshold);
		}

		// Type: Time
		if (thisEvent.eventType == EVENTTYPEC.Time)
			thisEvent.eventTime = EditorGUILayout.FloatField ("Time", thisEvent.eventTime);

		EditorGUILayout.Separator();

		// Settings with inheritance options
		EditorGUILayout.PropertyField(serializedEvent.FindPropertyRelative("eventInheritancePosition"), new GUIContent("Position", "The inheritance for position."));
		if (thisEvent.eventInheritancePosition == EVENTINHERITANCEC.User) {
			EditorGUI.indentLevel++;
			thisEvent.eventPosition = EditorGUILayout.Vector3Field (" ", thisEvent.eventPosition);
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Separator();

		EditorGUILayout.PropertyField(serializedEvent.FindPropertyRelative("eventInheritanceVelocity"), new GUIContent("Velocity", "The inheritance for velocity."));
		if (thisEvent.eventInheritanceVelocity == EVENTINHERITANCEC.User)
			thisEvent.eventVelocity = EditorGUILayout.Vector3Field (" ", thisEvent.eventVelocity);
		thisEvent.velocityMultiplier = EditorGUILayout.FloatField("Velocity Multiplier", thisEvent.velocityMultiplier);

		EditorGUILayout.Separator();

		EditorGUILayout.PropertyField(serializedEvent.FindPropertyRelative("eventInheritanceColor"), new GUIContent("Color", "The inheritance for color."));
		if (thisEvent.eventInheritanceColor == EVENTINHERITANCEC.User) {
			EditorGUI.indentLevel++;
			thisEvent.eventColor = EditorGUILayout.ColorField(" ", thisEvent.eventColor);
			EditorGUI.indentLevel--;
		}

		GUI.enabled = true;
	}
	
	public void RenderStateSettings () {
		
		GUI.enabled = (states.arraySize>0);
			activeState.intValue = EditorGUILayout.IntSlider("Active State", activeState.intValue, 0, states.arraySize-1);
		GUI.enabled = true;
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginVertical(boxStyle);
		statesFoldout = GUILayout.Toggle(statesFoldout, "States ("+states.arraySize+")", EditorStyles.foldout);
		if (statesFoldout) {
			if (states.arraySize>0) {
				SerializedProperty thisState;
				SerializedProperty thisName;
				SerializedProperty thisPoints;
				SerializedProperty thisTexture;
				SerializedProperty thisMesh;
				SerializedProperty thisDepthmap;
				SerializedProperty thisDepthmapStrength;
				SerializedProperty thisTransform;
				SerializedProperty thisStateScale;
				SerializedProperty thisStateOffset;
				
				for (int i = 0; i<states.arraySize; i++) {
					thisState = states.GetArrayElementAtIndex(i);
					
					GUILayout.BeginVertical(boxStyle);
					GUILayout.BeginHorizontal(GUILayout.MinHeight(20));
					
					// State title with foldout
					if (playgroundParticlesScriptReference.activeState==i) GUILayout.BeginHorizontal(boxStyle);
					
					GUI.enabled = (playgroundParticlesScriptReference.states.Count>1);
					if (GUILayout.Button(i.ToString(), EditorStyles.toolbarButton, GUILayout.Width(20))) playgroundParticlesScriptReference.activeState=i;
					GUI.enabled = true;
					
					statesListFoldout[i] = GUILayout.Toggle(statesListFoldout[i], playgroundParticlesScriptReference.states[i].stateName, EditorStyles.foldout);

					EditorGUILayout.Separator();
					GUI.enabled = (playgroundParticlesScriptReference.states.Count>1);
					if(GUILayout.Button("U", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
						int moveUp = i==0?playgroundParticlesScriptReference.states.Count-1:i-1;
						if (playgroundParticlesScriptReference.activeState==i) playgroundParticlesScriptReference.activeState = moveUp;
						playgroundParticlesScriptReference.previousActiveState = playgroundParticlesScriptReference.activeState;
						states.MoveArrayElement(i, moveUp);
						playgroundParticles.ApplyModifiedProperties();
						
						playgroundParticlesScriptReference.states[i].Initialize();
						playgroundParticlesScriptReference.states[moveUp].Initialize();
						
					}
					if(GUILayout.Button("D", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
						int moveDown = i<playgroundParticlesScriptReference.states.Count-1?i+1:0;
						if (playgroundParticlesScriptReference.activeState==i) playgroundParticlesScriptReference.activeState = moveDown;
						playgroundParticlesScriptReference.previousActiveState = playgroundParticlesScriptReference.activeState;
						states.MoveArrayElement(i, moveDown);
						playgroundParticles.ApplyModifiedProperties();
						
						playgroundParticlesScriptReference.states[i].Initialize();
						playgroundParticlesScriptReference.states[moveDown].Initialize();
					}
					GUI.enabled = true;
					if(GUILayout.Button("+", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
						PlaygroundC.Add(playgroundParticlesScriptReference, playgroundParticlesScriptReference.states[i].Clone());
						statesListFoldout.Add(statesListFoldout[i]);
						if (!playgroundParticlesScriptReference.states[playgroundParticlesScriptReference.states.Count-1].stateName.Contains("(Clone)"))
							playgroundParticlesScriptReference.states[playgroundParticlesScriptReference.states.Count-1].stateName = playgroundParticlesScriptReference.states[playgroundParticlesScriptReference.states.Count-1].stateName+" (Clone)";
					}
					if(GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(16)})){
						if (EditorUtility.DisplayDialog(
							"Remove "+playgroundParticlesScriptReference.states[i].stateName+"?",
							"Are you sure you want to remove the state "+playgroundParticlesScriptReference.states[i].stateName+" at list position "+i.ToString()+"?", 
							"Yes", "No")) {
								RemoveState(i);
								statesListFoldout.RemoveAt(i);
								playgroundParticles.ApplyModifiedProperties();
								return;
							}
					}
					if (playgroundParticlesScriptReference.activeState==i) GUILayout.EndHorizontal();
					GUILayout.EndHorizontal();
					
					if (statesListFoldout[i]) {
						
						if (i<states.arraySize) {
							
							EditorGUILayout.Separator();
							
							thisName = thisState.FindPropertyRelative("stateName");
							EditorGUILayout.PropertyField(thisName, new GUIContent("Name"));
							
							thisMesh = thisState.FindPropertyRelative("stateMesh");
							EditorGUILayout.PropertyField(thisMesh, new GUIContent("Mesh", "The source mesh to construct particles from vertices. When a mesh is used the texture is used to color each vertex."));
							
							thisTexture = thisState.FindPropertyRelative("stateTexture");
							EditorGUILayout.PropertyField(thisTexture, new GUIContent("Texture", "The source texture to construct particles from pixels. When a mesh is used this texture is used to color each vertex."));
							
							thisDepthmap = thisState.FindPropertyRelative("stateDepthmap");
							EditorGUILayout.PropertyField(thisDepthmap, new GUIContent("Depthmap", "The source texture to apply depthmap onto Texture's pixels. Not compatible with meshes."));
							if (thisDepthmap.objectReferenceValue!=null) {
								thisDepthmapStrength = thisState.FindPropertyRelative("stateDepthmapStrength");
								float currentDS = thisDepthmapStrength.floatValue;
								EditorGUILayout.PropertyField(thisDepthmapStrength, new GUIContent("Depthmap Strength", "How much the grayscale of the depthmap will affect Z-value."));
								if (currentDS!=thisDepthmapStrength.floatValue)
									playgroundParticlesScriptReference.states[i].Initialize();
							}
							
							thisTransform = thisState.FindPropertyRelative("stateTransform");
							EditorGUILayout.PropertyField(thisTransform, new GUIContent("Transform", "The transform to parent this state."));
							
							thisStateScale = thisState.FindPropertyRelative("stateScale");
							EditorGUILayout.PropertyField(thisStateScale, new GUIContent("Scale", "The scale of width-height."));
							
							thisStateOffset = thisState.FindPropertyRelative("stateOffset");
							EditorGUILayout.PropertyField(thisStateOffset, new GUIContent("Offset", "The offset from Particle System origin."));
							
							GUILayout.BeginHorizontal();
							EditorGUILayout.PrefixLabel("Points:");
							thisPoints = thisState.FindPropertyRelative("positionLength");
							EditorGUILayout.SelectableLabel(thisPoints.intValue.ToString(), GUILayout.MaxWidth(80));
							EditorGUILayout.Separator();
							if(GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(50))){
								ParticleStateC thisStateClass;
								thisStateClass = playgroundParticlesScriptReference.states[i];
								thisStateClass.Initialize();
							}
							if(GUILayout.Button("Set Particle Count", EditorStyles.toolbarButton)){
								PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, thisPoints.intValue);
								playgroundParticlesScriptReference.Start();
							}
							if(GUILayout.Button("++", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}))
								particleCount.intValue = particleCount.intValue+thisPoints.intValue;
							GUILayout.EndHorizontal();
							
						}
					}
					GUILayout.EndVertical();
				}
			} else {
				EditorGUILayout.HelpBox("No states created.", MessageType.Info);
			}
		}
		
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical(boxStyle);
		createNewStateFoldout = GUILayout.Toggle(createNewStateFoldout, "Create State", EditorStyles.foldout);
		if (createNewStateFoldout) {
			EditorGUILayout.Separator();
			meshOrImage = GUILayout.Toolbar (meshOrImage, new string[]{"Image","Mesh"}, EditorStyles.toolbarButton);
			EditorGUILayout.Separator();
			// Add image or mesh
			if (meshOrImage==1)
				addStateMesh = EditorGUILayout.ObjectField("Mesh", addStateMesh, typeof(Mesh), true);
			addStateTexture = EditorGUILayout.ObjectField("Texture", addStateTexture, typeof(Texture2D), true);
			if (meshOrImage==0) {
				addStateDepthmap = EditorGUILayout.ObjectField("Depthmap", addStateDepthmap, typeof(Texture2D), true);
			if (addStateDepthmap!=null)
				addStateDepthmapStrength = EditorGUILayout.FloatField("Depthmap Strength", addStateDepthmapStrength);
			}
			addStateTransform = EditorGUILayout.ObjectField("Transform", addStateTransform, typeof(Transform), true);
			addStateName = EditorGUILayout.TextField("Name", addStateName);
			addStateScale = EditorGUILayout.FloatField("Scale", addStateScale);
			addStateOffset = EditorGUILayout.Vector3Field("Offset", addStateOffset);
			
			EditorGUILayout.Separator();
			
			if (meshOrImage==0)
				GUI.enabled = (addStateTexture!=null);
			else
				GUI.enabled = (addStateMesh!=null);
			
			if(GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50))){
				
				// Check read/write
				if (addStateTexture!=null) {
					TextureImporter tAssetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(addStateTexture as UnityEngine.Object)) as TextureImporter;
					
					// If no Import Settings are found
					if (!tAssetImporter) {
						Debug.Log("Could not read the Import Settings of the selected texture.");
						return; 
					}
					
					// If the texture isn't readable
					if (!tAssetImporter.isReadable) {
						Debug.Log(tAssetImporter.assetPath+" is not readable. Please change Read/Write Enabled on its Import Settings.");
						return; 
					}
				}
				if (addStateMesh!=null) {
					ModelImporter mAssetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(addStateMesh as UnityEngine.Object)) as ModelImporter;
					if (mAssetImporter==null) {
						Debug.Log("Could not read the Import Settings of the selected mesh.");
						return; 
					}
					if (!mAssetImporter.isReadable) {
						Debug.Log(mAssetImporter.assetPath+" is not readable. Please change Read/Write Enabled on its Import Settings.");
						return; 
					}
				}
				
				if (addStateName=="" || addStateName==null) addStateName = "State "+(states.arraySize).ToString();
				if (meshOrImage==0) {
					if (addStateDepthmap==null)
						PlaygroundC.Add(playgroundParticlesScriptReference, addStateTexture as Texture2D, addStateScale, addStateOffset, addStateName, addStateTransform as Transform);
					else
						PlaygroundC.Add(playgroundParticlesScriptReference, addStateTexture as Texture2D, addStateDepthmap as Texture2D, addStateDepthmapStrength, addStateScale, addStateOffset, addStateName, addStateTransform as Transform);
				} else {
					if (addStateTexture==null)
						PlaygroundC.Add(playgroundParticlesScriptReference, addStateMesh as Mesh, addStateScale, addStateOffset, addStateName, addStateTransform as Transform);
					else
						PlaygroundC.Add(playgroundParticlesScriptReference, addStateMesh as Mesh, addStateTexture as Texture2D, addStateScale, addStateOffset, addStateName, addStateTransform as Transform);
				}
				playgroundParticlesScriptReference.Start();
				
				statesFoldout = true;
				statesListFoldout.Add(true);
				
				addStateName = "";
				addStateMesh = null;
				addStateTexture = null;
				addStateTransform = null;
				addStateDepthmap = null;
				addStateDepthmapStrength = 1f;
				addStateScale = 1f;
				addStateOffset = Vector3.zero;
			}
			GUI.enabled = true;
		}
		EditorGUILayout.EndVertical();
	}
	
	public void RenderProjectionSettings () {
		
		if (playgroundParticlesScriptReference.projection==null) {
			playgroundParticlesScriptReference.projection = new ParticleProjectionC();
			playgroundParticlesScriptReference.projection.projectionTransform = playgroundParticlesScriptReference.particleSystemTransform;
		}
		
		// Projection texture
		Texture2D prevTexture = playgroundParticlesScriptReference.projection.projectionTexture;
		playgroundParticlesScriptReference.projection.projectionTexture = EditorGUILayout.ObjectField("Projection Texture", playgroundParticlesScriptReference.projection.projectionTexture, typeof(Texture2D), true) as Texture2D;
		
		// Texture changed
		if (prevTexture!=playgroundParticlesScriptReference.projection.projectionTexture) {
			TextureImporter tAssetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(playgroundParticlesScriptReference.projection.projectionTexture as UnityEngine.Object)) as TextureImporter;
			
			// If no Import Settings are found
			if (!tAssetImporter) {
				Debug.Log("Could not read the Import Settings of the selected texture.");
				playgroundParticlesScriptReference.projection.projectionTexture = null;
				return; 
			}
			
			// If the texture isn't readable
			if (!tAssetImporter.isReadable) {
				Debug.Log(tAssetImporter.assetPath+" is not readable. Please change Read/Write Enabled on its Import Settings.");
				playgroundParticlesScriptReference.projection.projectionTexture = null;
				return; 
			}
			
			playgroundParticlesScriptReference.projection.Construct(playgroundParticlesScriptReference.projection.projectionTexture, playgroundParticlesScriptReference.projection.projectionTransform);
		}
		
		playgroundParticlesScriptReference.projection.projectionTransform = EditorGUILayout.ObjectField("Transform", playgroundParticlesScriptReference.projection.projectionTransform, typeof(Transform), true) as Transform;
		playgroundParticlesScriptReference.projection.liveUpdate = EditorGUILayout.Toggle("Live Update", playgroundParticlesScriptReference.projection.liveUpdate);
		playgroundParticlesScriptReference.projection.projectionOrigin = EditorGUILayout.Vector2Field("Origin Offset", playgroundParticlesScriptReference.projection.projectionOrigin);
		playgroundParticlesScriptReference.projection.projectionDistance = EditorGUILayout.FloatField("Projection Distance", playgroundParticlesScriptReference.projection.projectionDistance);
		playgroundParticlesScriptReference.projection.projectionScale = EditorGUILayout.FloatField("Projection Scale", playgroundParticlesScriptReference.projection.projectionScale);
		playgroundParticlesScriptReference.projection.surfaceOffset = EditorGUILayout.FloatField("Surface Offset", playgroundParticlesScriptReference.projection.surfaceOffset);
		EditorGUILayout.PropertyField(projectionMask, new GUIContent("Projection Mask"));
		EditorGUILayout.PropertyField(projectionCollisionType, new GUIContent("Projection Collision Type"));
		if (projectionCollisionType.enumValueIndex==1) {
			GUILayout.BeginHorizontal();
			GUILayout.Space (16);
			GUILayout.Label("Depth");
			EditorGUILayout.Separator();
			float minDepth = playgroundParticlesScriptReference.projection.minDepth;
			float maxDepth = playgroundParticlesScriptReference.projection.maxDepth;
			EditorGUILayout.MinMaxSlider(ref minDepth, ref maxDepth, -playgroundScriptReference.maximumAllowedDepth, playgroundScriptReference.maximumAllowedDepth, GUILayout.Width(Mathf.FloorToInt(Screen.width/1.8f)-105));
			playgroundParticlesScriptReference.projection.minDepth = minDepth;
			playgroundParticlesScriptReference.projection.maxDepth = maxDepth;
			playgroundParticlesScriptReference.projection.minDepth = EditorGUILayout.FloatField(playgroundParticlesScriptReference.projection.minDepth, GUILayout.Width(50));
			playgroundParticlesScriptReference.projection.maxDepth = EditorGUILayout.FloatField(playgroundParticlesScriptReference.projection.maxDepth, GUILayout.Width(50));
			GUILayout.EndHorizontal();
		}
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Points:");
		EditorGUILayout.SelectableLabel(playgroundParticlesScriptReference.projection.positionLength.ToString(), GUILayout.MaxWidth(80));
		EditorGUILayout.Separator();
		if(GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(50))){
			playgroundParticlesScriptReference.projection.Initialize();
		}
		if(GUILayout.Button("Set Particle Count", EditorStyles.toolbarButton)){
			PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.projection.positionLength);
			playgroundParticlesScriptReference.Start();
		}
		if(GUILayout.Button("++", EditorStyles.toolbarButton, new GUILayoutOption[]{GUILayout.Width(24), GUILayout.Height(16)}))
			particleCount.intValue = particleCount.intValue+playgroundParticlesScriptReference.projection.positionLength;
		GUILayout.EndHorizontal();
	}
	
	public static int selectedSort;
	public static int selectedOrigin;
	
	public void LifetimeSorting () {
		//PlaygroundParticlesC.SetLifetime(playgroundParticlesScriptReference, playgroundParticlesScriptReference.sorting, playgroundParticlesScriptReference.lifetime);
		playgroundParticlesScriptReference.Start();
	}
	
	public void LifetimeSortingAll () {
		foreach (PlaygroundParticlesC p in PlaygroundC.reference.particleSystems)
			p.Start();
	}
	
	public void RemoveState (int i) {
		playgroundParticlesScriptReference.RemoveState(i);
	}
	
	
	public void StartStopPaint () {
		inPaintMode = !inPaintMode;
		playgroundParticlesScriptReference.Start();
		if (inPaintMode) {
			//if (!playgroundParticlesScriptReference.paint.initialized) {
			//	PlaygroundC.PaintObject(playgroundParticlesScriptReference);
			//}
			
			if (selectedPaintMode==1)
				SetBrush(selectedBrushPreset);
			
			Tools.current = Tool.None;
		} else {
			Tools.current = lastActiveTool;
		}
	}
	
	public void ClearPaint () {
		if (EditorUtility.DisplayDialog(
			"Clear Paint?",
			"Are you sure you want to remove all painted source positions?", 
			"Yes", "No")) {
				inPaintMode = false;
				PlaygroundC.ClearPaint(playgroundParticlesScriptReference);
				PlaygroundParticlesC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.particleCount);
			}
	}
	
	public void DrawCollisionPlane (PlaygroundColliderC pc) {
		float scale = playgroundScriptReference.collisionPlaneScale;
		if (scale<=0) return;
		Vector3 p1;
		Vector3 p2;
		Handles.color = pc.enabled?new Color(0f,.8f,.1f,.25f):new Color(0f,.8f,.1f,.05f);
		for (int x = 0; x<11; x++) {
			p1 = pc.transform.TransformPoint(new Vector3((x*10f)-50f, 0f, 50f)*scale)+pc.offset;
			p2 = pc.transform.TransformPoint(new Vector3((x*10f)-50f, 0f, -50f)*scale)+pc.offset;
			Handles.DrawLine(p1, p2);
		}
		for (int y = 0; y<11; y++) {
			p1 = pc.transform.TransformPoint(new Vector3(50f, 0f, (y*10f)-50f)*scale)+pc.offset;
			p2 = pc.transform.TransformPoint(new Vector3(-50f, 0f, (y*10f)-50f)*scale)+pc.offset;
			Handles.DrawLine(p1, p2);
		}
	}
	
	bool keyPressed = false;
	int foldoutHeight = 0;
	Quaternion cameraRotation;
	RaycastHit eraserHit = new RaycastHit();
	RaycastHit2D eraserHit2d = new RaycastHit2D();
	void OnSceneGUI () {

		cameraRotation = Camera.current.transform.rotation;

		// Collision Planes
		if (playgroundScriptReference.drawGizmos && collisionFoldout && playgroundParticlesScriptReference.collision && playgroundParticlesScriptReference.colliders.Count>0) {
			for (int c = 0; c<playgroundParticlesScriptReference.colliders.Count; c++) {
				
				if (playgroundParticlesScriptReference.colliders[c].transform==null) continue;
				
				DrawCollisionPlane(playgroundParticlesScriptReference.colliders[c]);
				
				if (playgroundParticlesScriptReference.colliders[c].enabled) {
					// Position
					if (Tools.current==Tool.Move)
						playgroundParticlesScriptReference.colliders[c].transform.position = Handles.PositionHandle(playgroundParticlesScriptReference.colliders[c].transform.position, Tools.pivotRotation==PivotRotation.Global? Quaternion.identity : playgroundParticlesScriptReference.colliders[c].transform.rotation);
					// Rotation
					else if (Tools.current==Tool.Rotate)
						playgroundParticlesScriptReference.colliders[c].transform.rotation = Handles.RotationHandle(playgroundParticlesScriptReference.colliders[c].transform.rotation, playgroundParticlesScriptReference.colliders[c].transform.position);
					// Scale
					else if (Tools.current==Tool.Scale)
						playgroundParticlesScriptReference.colliders[c].transform.localScale = Handles.ScaleHandle(playgroundParticlesScriptReference.colliders[c].transform.localScale, playgroundParticlesScriptReference.colliders[c].transform.position, playgroundParticlesScriptReference.colliders[c].transform.rotation, HandleUtility.GetHandleSize(playgroundParticlesScriptReference.colliders[c].transform.position));
				}
			}
		}

		// Source position hilight
		if (playgroundScriptReference.drawGizmos && playgroundScriptReference.drawSourcePositions) {
			Handles.color = new Color(1f,1f,.2f,.2f);
			for (int pos = 0; pos<playgroundParticlesScriptReference.playgroundCache.targetPosition.Length; pos++) {
				Handles.DotCap(0, playgroundParticlesScriptReference.playgroundCache.targetPosition[pos], cameraRotation, .025f);
			}
		}

		// Nearest neighbor sorting highlight
		if (playgroundScriptReference.drawGizmos && particleSettingsFoldout && (playgroundParticlesScriptReference.sorting==SORTINGC.NearestNeighbor || playgroundParticlesScriptReference.sorting==SORTINGC.NearestNeighborReversed)) {
			Handles.color = new Color(1f,1f,.2f,.6f);
			Handles.CircleCap(0, playgroundParticlesScriptReference.playgroundCache.targetPosition[playgroundParticlesScriptReference.nearestNeighborOrigin], cameraRotation, HandleUtility.GetHandleSize(playgroundParticlesScriptReference.playgroundCache.targetPosition[playgroundParticlesScriptReference.nearestNeighborOrigin])*.05f);
			Handles.color = new Color(1f,.7f,.2f,.2f);
			Handles.DrawSolidDisc(playgroundParticlesScriptReference.playgroundCache.targetPosition[playgroundParticlesScriptReference.nearestNeighborOrigin], Camera.current.transform.forward, HandleUtility.GetHandleSize(playgroundParticlesScriptReference.playgroundCache.targetPosition[playgroundParticlesScriptReference.nearestNeighborOrigin])*.2f);
		}

		// Projection mode
		if (playgroundParticlesScriptReference.source == SOURCEC.Projection) {
			
			// Projector preview
			if (playgroundScriptReference.drawGizmos && playgroundParticlesScriptReference.projection!=null && playgroundParticlesScriptReference.projection.projectionTexture!=null && playgroundParticlesScriptReference.projection.projectionTransform!=null) {
				//Handles.Label(playgroundParticlesScriptReference.projection.projectionTransform.position, GUIContent(playgroundParticlesScriptReference.projection.projectionTexture));
				RaycastHit projectorHit;
				Vector3 p2 = playgroundParticlesScriptReference.projection.projectionTransform.position+(playgroundParticlesScriptReference.projection.projectionTransform.forward*playgroundParticlesScriptReference.projection.projectionDistance);
				bool projectorHasSurface = false;
				if (Physics.Raycast(playgroundParticlesScriptReference.projection.projectionTransform.position, playgroundParticlesScriptReference.projection.projectionTransform.forward, out projectorHit, playgroundParticlesScriptReference.projection.projectionDistance, playgroundParticlesScriptReference.projection.projectionMask)) {
					p2 = projectorHit.point;
					projectorHasSurface = true;
				}
				Handles.color = projectorHasSurface?new Color(1f,1f,.25f,.6f):new Color(1f,1f,.25f,.2f);
				Handles.DrawLine(playgroundParticlesScriptReference.projection.projectionTransform.position, p2);
			}
		}
		
		// Paint mode
		if (playgroundParticlesScriptReference.source == SOURCEC.Paint) {
			Event e = Event.current;

			// Paint Toolbox in Scene View
			Rect toolboxRect = new Rect(10f,Screen.height-(138f+foldoutHeight),300f,103f+foldoutHeight);
			if (PlaygroundC.reference.paintToolbox) {
				if (!paintToolboxSettingsFoldout) {
					foldoutHeight = 0;
				} else {
					switch (selectedPaintMode) {
						case 0: foldoutHeight = 54; break;
						case 1: foldoutHeight = 144; break;
						case 2: foldoutHeight = 36; break;
					}
				}
				if (!toolboxFoldout) foldoutHeight=-69;
				Handles.BeginGUI();
					GUILayout.BeginArea(toolboxRect);
					if (boxStyle==null)
						boxStyle = GUI.skin.FindStyle("box");
					GUILayout.BeginVertical(boxStyle);
					toolboxFoldout = GUILayout.Toggle(toolboxFoldout, "Playground Paint", EditorStyles.foldout);
					if (toolboxFoldout) {
						selectedPaintMode = GUILayout.Toolbar (selectedPaintMode, new string[]{"Dot","Brush","Eraser"}, EditorStyles.toolbarButton);
						
						// Settings
						GUILayout.BeginVertical(boxStyle);
						paintToolboxSettingsFoldout = GUILayout.Toggle(paintToolboxSettingsFoldout, "Settings", EditorStyles.foldout);
						if (paintToolboxSettingsFoldout) {
							switch (selectedPaintMode) {
								case 0:
									paintColor = EditorGUILayout.ColorField("Color", paintColor);
									playgroundParticlesScriptReference.paint.spacing = EditorGUILayout.Slider("Paint Spacing", playgroundParticlesScriptReference.paint.spacing, .0f, playgroundScriptReference.maximumAllowedPaintSpacing);
									EditorGUILayout.PropertyField(paintLayerMask, new GUIContent("Paint Mask"));
								break;
								case 1:
									GUILayout.BeginHorizontal();
									EditorGUILayout.PrefixLabel("Brush Shape");
									paintTexture = EditorGUILayout.ObjectField(paintTexture, typeof(Texture2D), false) as Texture2D;
									GUILayout.EndHorizontal();
									if (paintTexture!=null && paintTexture!=playgroundParticlesScriptReference.paint.brush.texture) {
										selectedBrushPreset = -1;
										SetBrush(selectedBrushPreset);
									}
									playgroundParticlesScriptReference.paint.brush.detail = (BRUSHDETAILC)EditorGUILayout.EnumPopup("Detail", playgroundParticlesScriptReference.paint.brush.detail);
									playgroundParticlesScriptReference.paint.brush.scale = EditorGUILayout.Slider("Scale", playgroundParticlesScriptReference.paint.brush.scale, playgroundScriptReference.minimumAllowedBrushScale, playgroundScriptReference.maximumAllowedBrushScale);
									playgroundParticlesScriptReference.paint.brush.distance = EditorGUILayout.FloatField("Distance", playgroundParticlesScriptReference.paint.brush.distance);
									useBrushColor = EditorGUILayout.Toggle("Use Brush Color", useBrushColor);
									GUI.enabled = !useBrushColor;
									paintColor = EditorGUILayout.ColorField("Color", paintColor);
									GUI.enabled = true;
									playgroundParticlesScriptReference.paint.spacing = EditorGUILayout.Slider("Paint Spacing", playgroundParticlesScriptReference.paint.spacing, .0f, playgroundScriptReference.maximumAllowedPaintSpacing);
									EditorGUILayout.PropertyField(paintLayerMask, new GUIContent("Paint Mask"));
								break;
								case 2:
									eraserRadius = EditorGUILayout.Slider("Radius", eraserRadius, playgroundScriptReference.minimumEraserRadius, playgroundScriptReference.maximumEraserRadius);
									EditorGUILayout.PropertyField(paintLayerMask, new GUIContent("Paint Mask"));
								break;
							}
						}
						GUILayout.EndVertical();
						GUILayout.BeginHorizontal();
						GUI.enabled = !(selectedPaintMode==1 && paintTexture==null);
						if(GUILayout.Button((inPaintMode?"Stop":"Start")+" Paint", EditorStyles.toolbarButton))
			 				StartStopPaint();
			 			GUI.enabled = (playgroundParticlesScriptReference.paint.positionLength>0);
			 			if(GUILayout.Button("Clear", EditorStyles.toolbarButton))
			 				ClearPaint();
			 			GUI.enabled = true;
			 			ProgressBar((playgroundParticlesScriptReference.paint.positionLength*1f)/PlaygroundC.reference.paintMaxPositions, playgroundParticlesScriptReference.paint.positionLength+"/"+PlaygroundC.reference.paintMaxPositions, 115f);
			 			GUILayout.EndHorizontal();
		 			}
		 			GUILayout.EndVertical();
		 			GUILayout.EndArea();
		 		Handles.EndGUI();
	 		}
	 		
			if (inPaintMode) {
				if (e.type == EventType.Layout) {
					HandleUtility.AddDefaultControl(0);
				}
				
				Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);

				// Brush preview
				if (selectedPaintMode==1 && playgroundParticlesScriptReference.paint.brush.texture!=null && sceneBrushStyle!=null && !toolboxRect.Contains(e.mousePosition)) {
					Handles.Label(mouseRay.origin, new GUIContent(playgroundParticlesScriptReference.paint.brush.texture as Texture2D), sceneBrushStyle);
				}
					
				// Eraser preview
				if (selectedPaintMode==2 && !toolboxRect.Contains(e.mousePosition)) {
					if (playgroundParticlesScriptReference.paint.collisionType==COLLISIONTYPEC.Physics3D) {
						if (Physics.Raycast(mouseRay, out eraserHit, 10000f, playgroundParticlesScriptReference.paint.layerMask)) {
							Handles.color = new Color(0f,0f,0f,.4f);
							Handles.CircleCap(-1, eraserHit.point, Quaternion.LookRotation(mouseRay.direction), eraserRadius);
						}
					} else {
						eraserHit2d = Physics2D.Raycast (mouseRay.origin, mouseRay.direction, 100000f, playgroundParticlesScriptReference.paint.layerMask, playgroundParticlesScriptReference.paint.minDepth, playgroundParticlesScriptReference.paint.maxDepth);
						if (eraserHit2d.collider!=null) {
							Handles.color = new Color(0f,0f,0f,.4f);
							Handles.CircleCap(-1, eraserHit2d.point, Quaternion.LookRotation(mouseRay.direction), eraserRadius);
						}
					}
				}
				
				
				// Spacing preview
				if (selectedPaintMode!=2) {
					Handles.color = new Color(.3f,1f,.3f,.3f);
					Handles.CircleCap(-1, playgroundParticlesScriptReference.paint.lastPaintPosition, Quaternion.LookRotation(Camera.current.transform.forward), playgroundParticlesScriptReference.paint.spacing);
				}
				
				if (e.type  == EventType.KeyDown)
					keyPressed = true;
				else if (e.type == EventType.KeyUp)
					keyPressed = false;
				
				// Paint from the Brush's texture into the Scene View
				if (!keyPressed && e.button == 0 && e.isMouse && !e.alt) {
					if (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) {
						switch (selectedPaintMode) {
							// Dot
							case 0:
							if (playgroundParticlesScriptReference.paint.exceedMaxStopsPaint && playgroundParticlesScriptReference.paint.positionLength>=PlaygroundC.reference.paintMaxPositions) return;
							if (playgroundParticlesScriptReference.paint.collisionType==COLLISIONTYPEC.Physics3D) {
								RaycastHit dotHit;
								if (Physics.Raycast(mouseRay, out dotHit, 10000f, playgroundParticlesScriptReference.paint.layerMask)) {
									if (e.type != EventType.MouseDown)
										if (Vector3.Distance(dotHit.point, playgroundParticlesScriptReference.paint.lastPaintPosition)<=playgroundParticlesScriptReference.paint.spacing) return;
									PlaygroundC.Paint(playgroundParticlesScriptReference, dotHit.point, dotHit.normal, dotHit.transform, paintColor);
									playgroundParticlesScriptReference.paint.lastPaintPosition = dotHit.point;
								}
							} else {
								RaycastHit2D dotHit2d = Physics2D.Raycast (mouseRay.origin, mouseRay.direction, 10000f, playgroundParticlesScriptReference.paint.layerMask, playgroundParticlesScriptReference.paint.minDepth, playgroundParticlesScriptReference.paint.maxDepth);
								if (dotHit2d.collider!=null) {
									if (e.type != EventType.MouseDown)
										if (Vector3.Distance(dotHit2d.point, playgroundParticlesScriptReference.paint.lastPaintPosition)<=playgroundParticlesScriptReference.paint.spacing) return;
									PlaygroundC.Paint(playgroundParticlesScriptReference, dotHit2d.point, dotHit2d.normal, dotHit2d.transform, paintColor);
									playgroundParticlesScriptReference.paint.lastPaintPosition = dotHit2d.point;
								}
							}
							break;
							// Brush
							case 1:
							if (playgroundParticlesScriptReference.paint.exceedMaxStopsPaint && playgroundParticlesScriptReference.paint.positionLength>=PlaygroundC.reference.paintMaxPositions || !playgroundParticlesScriptReference.paint.brush.texture) return;
							if (e.type != EventType.MouseDown) {
								if (playgroundParticlesScriptReference.paint.collisionType==COLLISIONTYPEC.Physics3D) {
									RaycastHit brushHit;
									if (Physics.Raycast(mouseRay, out brushHit, 10000f, playgroundParticlesScriptReference.paint.layerMask))
										if (Vector3.Distance(brushHit.point, playgroundParticlesScriptReference.paint.lastPaintPosition)<=playgroundParticlesScriptReference.paint.spacing) return;
								} else {
									RaycastHit2D brushHit2d = Physics2D.Raycast(mouseRay.origin, mouseRay.direction, 10000f, playgroundParticlesScriptReference.paint.layerMask, playgroundParticlesScriptReference.paint.minDepth, playgroundParticlesScriptReference.paint.maxDepth);
									if (brushHit2d.collider!=null)
										if (Vector3.Distance(brushHit2d.point, playgroundParticlesScriptReference.paint.lastPaintPosition)<=playgroundParticlesScriptReference.paint.spacing) return;
								}
							}
							int detail = 0;
							switch (playgroundParticlesScriptReference.paint.brush.detail) {
								case BRUSHDETAILC.Perfect: detail=0; break;
								case BRUSHDETAILC.High: detail=2; break;
								case BRUSHDETAILC.Medium: detail=4; break;
								case BRUSHDETAILC.Low: detail=6; break;
							}
							Color32 pixelColor;
							for (int x = 0; x<playgroundParticlesScriptReference.paint.brush.texture.width; x++) {
								for (int y = 0; y<playgroundParticlesScriptReference.paint.brush.texture.height; y++) {
									if (detail==0 || ((x+1)*(y+1)-1)%detail==0) {
										pixelColor = playgroundParticlesScriptReference.paint.brush.GetColor((x+1)*(y+1)-1);
										if (!useBrushColor) pixelColor = new Color(paintColor.r, paintColor.g, paintColor.b, pixelColor.a);
										if (pixelColor.a!=0) {
											mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition+new Vector2((-playgroundParticlesScriptReference.paint.brush.texture.width/2f)+x,(-playgroundParticlesScriptReference.paint.brush.texture.height/2)+y)*playgroundParticlesScriptReference.paint.brush.scale);
											playgroundParticlesScriptReference.paint.Paint(mouseRay, pixelColor);
										}
									}
								}
							}
							break;
							// Eraser
							case 2:
							if (playgroundParticlesScriptReference.paint.collisionType==COLLISIONTYPEC.Physics3D && eraserHit.collider!=null || playgroundParticlesScriptReference.paint.collisionType==COLLISIONTYPEC.Physics2D && eraserHit2d.collider!=null) {
								playgroundParticlesScriptReference.paint.Erase((playgroundParticlesScriptReference.paint.collisionType==COLLISIONTYPEC.Physics3D)?eraserHit.point:new Vector3(eraserHit2d.point.x, eraserHit2d.point.y), eraserRadius);
							}
							break;
						}
						
					}
					SceneView.RepaintAll();
				}
				
				
				if (e.type == EventType.MouseUp) {
					playgroundParticlesScriptReference.paint.lastPaintPosition = PlaygroundC.initialTargetPosition;
					
					// No positions to emit from, reset particle system by rebuilding
					if ((eraserHit.collider!=null || eraserHit2d.collider!=null) && playgroundParticlesScriptReference.paint.positionLength==0) {
						PlaygroundC.SetParticleCount(playgroundParticlesScriptReference, playgroundParticlesScriptReference.particleCount);
					}
				}
			}
		}
		
		// Render global manipulators
		int i = 0;
		if (playgroundScriptReference!=null && playgroundScriptReference.drawGizmos && PlaygroundInspectorC.manipulatorsFoldout)
			for (; i<playgroundScriptReference.manipulators.Count; i++)
				PlaygroundInspectorC.RenderManipulatorInScene(playgroundScriptReference.manipulators[i], playgroundScriptReference.manipulators[i].inverseBounds?new Color(1f,.6f,.4f,1f):new Color(.4f,.6f,1f,1f));
		// Render local manipulators
		if (playgroundScriptReference.drawGizmos && manipulatorsFoldout)
			for (i = 0; i<playgroundParticlesScriptReference.manipulators.Count; i++)
				PlaygroundInspectorC.RenderManipulatorInScene(playgroundParticlesScriptReference.manipulators[i], playgroundParticlesScriptReference.manipulators[i].inverseBounds?new Color(1f,1f,.4f,1f):new Color(.4f,1f,1f,1f));
		
		if (GUI.changed)
            EditorUtility.SetDirty (target);
	}
	
}