using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_WINRT && !UNITY_EDITOR
using Windows.System.Threading;
#else
using System.Threading;
#endif

namespace ParticlePlayground {
	[ExecuteInEditMode()]
	public class PlaygroundC : MonoBehaviour {

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground settings
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		// Time variables
		public static float globalTime;														// Time used by particles
		public static float lastTimeUpdated;												// Time when globalTime last updated
		public static float globalDeltaTime;												// Delta time for globalTime (globalTime-lastTimeUpdated)
		public static float globalTimescale = 1.0f;											// Scaling of globalTime
		
		// Misc settings
		public static Vector3 initialTargetPosition = new Vector3(0,100000,0); 				// Initial spawn position (dirty trick to not make them appear in Vector3.zero...)
		public static int skinnedUpdateRate = 1;											// Update rate for finding vertices in skinned meshes (1 = Every frame, 2 = Every second frame...)
		public static bool triggerSceneRepaint = true;										// Let a PlaygroundParticleWindow repaint the scene
		public static float collisionSleepVelocity = .01f;									// Minimum velocity of a particle before it goes to rest


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground cache
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		// Static references to this object
		public static PlaygroundC reference;
		public static Transform referenceTransform;
		public static GameObject referenceGameObject;


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground public variables
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		[SerializeField]
		public List<PlaygroundParticlesC> particleSystems = new List<PlaygroundParticlesC>();// List of particle systems handled by Playground Manager
		[SerializeField]
		public List<ManipulatorObjectC> manipulators = new List<ManipulatorObjectC>();		// List of manipulator objects handled by Playground Manager
		[HideInInspector] public int paintMaxPositions = 10000;								// Maximum amount of emission positions in a PaintObject
		[HideInInspector] public bool calculate = true;										// Calculate forces on PlaygroundParticlesC objects
		[HideInInspector] public PIXELMODEC pixelFilterMode = PIXELMODEC.Pixel32;			// Color filtering mode
		[HideInInspector] public bool autoGroup = true;										// Automatically parent a PlaygroundParticlesC object to Playground if it has no parent
		[HideInInspector] public  bool buildZeroAlphaPixels = false;						// Turn this on if you want to build particles from 0 alpha pixels into states
		[HideInInspector] public bool drawGizmos = true;									// Draw gizmos for manipulators and other particle system helpers in Scene View
		[HideInInspector] public bool drawSourcePositions = false;							// Draw gizmos for source positions in Scene View
		[HideInInspector] public bool showShuriken = false;									// Should the Shuriken particle system component be visible? (This is just a precaution as editing Shuriken can lead to unexpected behavior)
		[HideInInspector] public bool paintToolbox = true;									// Show toolbox in Scene View when Source is set to Paint on PlaygroundParticlesC objects
		[HideInInspector] public float collisionPlaneScale = .1f;							// Scale of collision planes
		[HideInInspector] public bool showSnapshotsInHierarchy = false;						// Should snapshots be visible in the Hieararchy? (Enable this if you want to edit snapshots)
		[HideInInspector] public bool drawWireframe = false;								// Should wireframes be rendered around particles in Scene View?

		// Limitations (shown in Playground Manager > Advanced)
		[HideInInspector] public float maximumAllowedLifetime = 100f;						// The maximum value for Particle Lifetime in Editor
		[HideInInspector] public int maximumAllowedParticles = 100000;						// The maximum value for Particle Count in Editor
		[HideInInspector] public float maximumAllowedRotation = 360f;						// The maximum value for Minimum- and Maximum Particle Rotation Speed in Editor
		[HideInInspector] public float maximumAllowedSize = 10f;							// The maximum value for Minimum- and Maximum Particle Size in Editor
		[HideInInspector] public float maximumAllowedDeltaMovementStrength = 100f;			// The maximum value for Particle Delta Movement Strength in Editor
		[HideInInspector] public float maximumAllowedScale = 10f;							// The maximum value for Particle Scale in Editor
		[HideInInspector] public float maximumAllowedDamping = 10f;							// The maximum value for Particle Damping in Editor
		[HideInInspector] public float maximumAllowedVelocity = 100f;						// The maximum value for Particle Max Velocity in Editor
		[HideInInspector] public float maximumAllowedDepth = 100f;							// The maximum value for Particle Collision Depth in Editor (collision type Physics2D)
		[HideInInspector] public float maximumAllowedMass = 100f;							// The maximum value for Particle Mass in Editor
		[HideInInspector] public float maximumAllowedCollisionRadius = 10f;					// The maximum value for Particle Collision Radius in Editor
		[HideInInspector] public float maximumAllowedBounciness = 2f;						// The maximum value for Particle Bounciness in Editor
		[HideInInspector] public int minimumAllowedUpdateRate = 10;							// The minimum value for Particle Update Rate in Editor
		[HideInInspector] public float maximumAllowedTransitionTime = 10f;					// The maximum value for Particle Transition Time in Editor
		[HideInInspector] public float minimumAllowedTimescale = .01f;						// The minimum value for Particle Timescale
		[HideInInspector] public float maximumAllowedTimescale = 2f;						// The maximum value for Particle Timescale
		[HideInInspector] public int maximumAllowedPaintPositions = 100000;					// The maximum value for Paint Positions
		[HideInInspector] public float minimumAllowedBrushScale = .001f;					// The minimum scale of a Brush
		[HideInInspector] public float maximumAllowedBrushScale = 1f;						// The maximum scale of a Brush
		[HideInInspector] public float maximumAllowedPaintSpacing = 10f;					// The maximum spacing when painting
		[HideInInspector] public float maximumAllowedInitialVelocity = 100f;				// The maximum value for Minimum- and Maximum Initial (+Local) Velocity
		[HideInInspector] public float minimumEraserRadius = .001f;							// The minimum value for Eraser radius
		[HideInInspector] public float maximumEraserRadius = 100f;							// The maximum value for Eraser radius
		[HideInInspector] public float maximumRenderSliders = 10f;							// The minimum- and maximum value for sliders in Rendering
		[HideInInspector] public float maximumAllowedManipulatorSize = 100f;				// The maximum value for Manipulator Size
		[HideInInspector] public float maximumAllowedManipulatorStrength = 100f;			// The maximum value for Manipulator Strength
		[HideInInspector] public float maximumAllowedManipulatorStrengthEffectors = 2f;		// The maximum value for Manipulator Strength Effect Scale
		[HideInInspector] public float maximumAllowedManipulatorZeroVelocity = 10f;			// The maximum value for Manipulator Property Zero Velocity Strength
		[HideInInspector] public float maximumAllowedSourceScatter = 10f;					// The maximum value for scattering source positions
		[HideInInspector] public float maximumAllowedTurbulenceTimeScale = 10f;				// The maximum value for turbulence time scale
		[HideInInspector] public float maximumAllowedTurbulenceStrength = 100f;				// The maximum value for turbulence strength
		[HideInInspector] public float maximumAllowedTurbulenceScale = 10f;					// The maximum value for turbulence scale
		[HideInInspector] public float maximumAllowedStretchSpeed = 10f;					// THe maximum value for stretch speed


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground private variables
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		static System.Random random = new System.Random();

		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground counters
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		public static int meshQuantity;
		public static int particlesQuantity;
		public static string version = "2.03";
		public static string specialVersion = "";
		

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground wrapper
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		// Create a PlaygroundParticlesC object by standard prefab
		public static PlaygroundParticlesC Particle () {
			PlaygroundParticlesC playgroundParticles = ResourceInstantiate("Particle Playground System").GetComponent<PlaygroundParticlesC>();
			return playgroundParticles;
		}

		// Create an empty PlaygroundParticlesC object by script
		public static PlaygroundParticlesC ParticleNew () {
			PlaygroundParticlesC playgroundParticles = PlaygroundParticlesC.CreateParticleObject("Particle Playground System "+particlesQuantity,Vector3.zero,Quaternion.identity,1f,new Material(Shader.Find("Playground/Vertex Color")));
			playgroundParticles.particleCache = new ParticleSystem.Particle[playgroundParticles.particleCount];
			PlaygroundParticlesC.OnCreatePlaygroundParticles(playgroundParticles);
			return playgroundParticles;
		}
		
		// Single image
		public static PlaygroundParticlesC Particle (Texture2D image, string name, Vector3 position, Quaternion rotation, Vector3 offset, float particleSize, float scale, Material material) {
			return PlaygroundParticlesC.CreatePlaygroundParticles(new Texture2D[]{image},name,position,rotation,offset,particleSize,scale,material);
		}
		
		// Single image simple
		public static PlaygroundParticlesC Particle (Texture2D image) {
			return PlaygroundParticlesC.CreatePlaygroundParticles(new Texture2D[]{image},"Particle Playground System "+particlesQuantity,Vector3.zero,Quaternion.identity,Vector3.zero,1f,1f,new Material(Shader.Find("Playground/Vertex Color")));
		}
		
		// Multiple images
		public static PlaygroundParticlesC Particle (Texture2D[] images, string name, Vector3 position, Quaternion rotation, Vector3 offset, float particleSize, float scale, Material material) {
			return PlaygroundParticlesC.CreatePlaygroundParticles(images,name,position,rotation,offset,particleSize,scale,material);
		}
		
		// Multiple images simple
		public static PlaygroundParticlesC Particle (Texture2D[] images) {
			return PlaygroundParticlesC.CreatePlaygroundParticles(images,"Particle Playground System "+particlesQuantity,Vector3.zero,Quaternion.identity,Vector3.zero,1f,1f,new Material(Shader.Find("Playground/Vertex Color")));
		}
		
		// Single mesh
		public static PlaygroundParticlesC Particle (Mesh mesh, Texture2D texture, string name, Vector3 position, Quaternion rotation, float particleScale, Vector3 offset, Material material) {
			return MeshParticles.CreateMeshParticles(new Mesh[]{mesh},new Texture2D[]{texture},null,name,position,rotation,particleScale,new Vector3[]{offset},material);
		}
		
		// Single mesh simple
		public static PlaygroundParticlesC Particle (Mesh mesh, Texture2D texture) {
			return MeshParticles.CreateMeshParticles(new Mesh[]{mesh},new Texture2D[]{texture},null,"Particle Playground System "+particlesQuantity,Vector3.zero,Quaternion.identity,1f,new Vector3[]{Vector3.zero},new Material(Shader.Find("Playground/Vertex Color")));
		}
		
		// Multiple meshes
		public static PlaygroundParticlesC Particle (Mesh[] meshes, Texture2D[] textures, string name, Vector3 position, Quaternion rotation, float particleScale, Vector3[] offsets, Material material) {
			return MeshParticles.CreateMeshParticles(meshes,textures,null,name,position,rotation,particleScale,offsets,material);
		}
		
		// Multiple meshes simple
		public static PlaygroundParticlesC Particle (Mesh[] meshes, Texture2D[] textures) {
			return MeshParticles.CreateMeshParticles(meshes,textures,null,"Particle Playground System "+particlesQuantity,Vector3.zero,Quaternion.identity,1f,new Vector3[meshes.Length],new Material(Shader.Find("Playground/Vertex Color")));
		}
		
		// Emit next particle - using the particle system as a pool (note that you need to set scriptedEmission-variables on beforehand). Returns emitted particle number.
		public static int Emit (PlaygroundParticlesC playgroundParticles) {
			return playgroundParticles.Emit(playgroundParticles.scriptedEmissionPosition,playgroundParticles.scriptedEmissionVelocity,playgroundParticles.scriptedEmissionColor);
		}
		
		// Emit next particle while setting scriptedEmission data - using the particle system as a pool. Returns emitted particle number.
		public static int Emit (PlaygroundParticlesC playgroundParticles, Vector3 position, Vector3 normal, Color color) {
			return playgroundParticles.Emit(position,normal,color);
		}
		
		// Set emission on/off
		public static void Emit (PlaygroundParticlesC playgroundParticles, bool setEmission) {
			playgroundParticles.Emit(setEmission);
		}
		
		// Random position
		public static void Random (PlaygroundParticlesC playgroundParticles, Vector3 min, Vector3 max) {
			PlaygroundParticlesC.Random(playgroundParticles,min,max);
		}
		
		// Linear interpolation position and color
		public static void Lerp (PlaygroundParticlesC playgroundParticles, int to, float time) {
			PlaygroundParticlesC.Lerp(playgroundParticles,to,time,LERPTYPEC.PositionColor);
		}
		
		// Linear interpolation to state
		public static void Lerp (PlaygroundParticlesC playgroundParticles, ParticleStateC state, float time) {
			PlaygroundParticlesC.Lerp(playgroundParticles,state,time,LERPTYPEC.PositionColor);
		}
		
		// Linear interpolation color
		public static void ColorLerp (PlaygroundParticlesC playgroundParticles, int to, float time) {
			PlaygroundParticlesC.Lerp(playgroundParticles,to,time,LERPTYPEC.Color);
		}
		
		// Linear interpolation position
		public static void PositionLerp (PlaygroundParticlesC playgroundParticles, int to, float time) {
			PlaygroundParticlesC.Lerp(playgroundParticles,to,time,LERPTYPEC.Position);
		}
		
		// Linear interpolation to object in world space
		public static void Lerp (PlaygroundParticlesC playgroundParticles, SkinnedWorldObject particleStateWorldObject, float time) {
			PlaygroundParticlesC.Lerp(playgroundParticles,particleStateWorldObject,time);
		} 

		// Get vertices and normals from a skinned world object in Vector3[] format (notice that the array is modified by reference)
		public static void GetPosition (SkinnedWorldObject particleStateWorldObject, bool updateNormals) {
			PlaygroundParticlesC.GetPosition(particleStateWorldObject, updateNormals);
		}
		
		// Get vertices from a world object in Vector3[] format (notice that the array is modified by reference)
		public static void GetPosition (Vector3[] vertices, WorldObject particleStateWorldObject) {
			PlaygroundParticlesC.GetPosition(vertices,particleStateWorldObject);
		}
		
		// Get normals from a world object in Vector3[] format (notice that the array is modified by reference)
		public static void GetNormals (Vector3[] normals, WorldObject particleStateWorldObject) {
			PlaygroundParticlesC.GetNormals(normals,particleStateWorldObject);
		}
		
		// Set new color to State instantly
		public static void SetColor (PlaygroundParticlesC playgroundParticles, int to) {
			PlaygroundParticlesC.SetColor(playgroundParticles,to);
		}
		
		// Set new color to Color instantly 
		public static void SetColor (PlaygroundParticlesC playgroundParticles, Color color) {
			PlaygroundParticlesC.SetColor(playgroundParticles,color);
		}
		
		// Set alpha of particles instantly
		public static void SetAlpha (PlaygroundParticlesC playgroundParticles, float alpha) {
			PlaygroundParticlesC.SetAlpha(playgroundParticles,alpha);
		}
		
		// Set particle size
		public static void SetSize (PlaygroundParticlesC playgroundParticles, float size) {
			PlaygroundParticlesC.SetSize(playgroundParticles,size);
		}
		
		// Translate all particles in Particle System
		public static void Translate (PlaygroundParticlesC playgroundParticles, Vector3 direction) {
			PlaygroundParticlesC.Translate(playgroundParticles,direction);
		}
		
		// Refresh and calculate particles
		public static void Update (PlaygroundParticlesC playgroundParticles) {
			PlaygroundParticlesC.Update(playgroundParticles);
		}
		
		// Add single state
		public static void Add (PlaygroundParticlesC playgroundParticles, ParticleStateC state) {
			PlaygroundParticlesC.Add(playgroundParticles,state);
		}
		
		// Add single state image
		public static void Add (PlaygroundParticlesC playgroundParticles, Texture2D image, float scale, Vector3 offset, string stateName) {
			PlaygroundParticlesC.Add(playgroundParticles,image,scale,offset,stateName,null);
		}
		
		// Add single state image with transform
		public static void Add (PlaygroundParticlesC playgroundParticles, Texture2D image, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			PlaygroundParticlesC.Add(playgroundParticles,image,scale,offset,stateName,stateTransform);
		}
		
		// Add single state image with depthmap
		public static void Add (PlaygroundParticlesC playgroundParticles, Texture2D image, Texture2D depthmap, float depthmapStrength, float scale, Vector3 offset, string stateName) {
			PlaygroundParticlesC.Add(playgroundParticles,image,depthmap,depthmapStrength,scale,offset,stateName,null);
		}
		
		// Add single state image with depthmap and transform
		public static void Add (PlaygroundParticlesC playgroundParticles, Texture2D image, Texture2D depthmap, float depthmapStrength, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			PlaygroundParticlesC.Add(playgroundParticles,image,depthmap,depthmapStrength,scale,offset,stateName,stateTransform);
		}
		
		// Add single state mesh
		public static void Add (PlaygroundParticlesC playgroundParticles, Mesh mesh, float scale, Vector3 offset, string stateName) {
			MeshParticles.Add(playgroundParticles,mesh,scale,offset,stateName,null);
		}
		
		// Add single state mesh with transform
		public static void Add (PlaygroundParticlesC playgroundParticles, Mesh mesh, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			MeshParticles.Add(playgroundParticles,mesh,scale,offset,stateName,stateTransform);
		}
		
		// Add single state mesh with texture
		public static void Add (PlaygroundParticlesC playgroundParticles, Mesh mesh, Texture2D texture, float scale, Vector3 offset, string stateName) {
			MeshParticles.Add(playgroundParticles,mesh,texture,scale,offset,stateName,null);
		}
		
		// Add single state mesh with texture and transform
		public static void Add (PlaygroundParticlesC playgroundParticles, Mesh mesh, Texture2D texture, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			MeshParticles.Add(playgroundParticles,mesh,texture,scale,offset,stateName,stateTransform);
		}
		
		// Add a plane collider
		public static PlaygroundColliderC AddCollider (PlaygroundParticlesC playgroundParticles) {
			PlaygroundColliderC pCollider = new PlaygroundColliderC();
			playgroundParticles.colliders.Add(pCollider);
			return pCollider;
		}
		
		// Add a plane collider and assign a transform
		public static PlaygroundColliderC AddCollider (PlaygroundParticlesC playgroundParticles, Transform transform) {
			PlaygroundColliderC pCollider = new PlaygroundColliderC();
			pCollider.transform = transform;
			playgroundParticles.colliders.Add(pCollider);
			return pCollider;
		}
		
		// Set amount of particles for this Particle System
		public static void SetParticleCount (PlaygroundParticlesC playgroundParticles, int amount) {
			PlaygroundParticlesC.SetParticleCount(playgroundParticles,amount);
		}
		
		// Set lifetime for this Particle System
		public static void SetLifetime (PlaygroundParticlesC playgroundParticles, float time) {
			PlaygroundParticlesC.SetLifetime(playgroundParticles,playgroundParticles.sorting,time);
		}
		
		// Set material for this Particle System
		public static void SetMaterial (PlaygroundParticlesC playgroundParticles, Material particleMaterial) {
			PlaygroundParticlesC.SetMaterial(playgroundParticles, particleMaterial);
		}
		
		// Destroy this Particle System
		public static void Destroy (PlaygroundParticlesC playgroundParticles) {
			PlaygroundParticlesC.Destroy(playgroundParticles);
		}
		
		// Create a world object reference (used for live world positioning of particles towards a mesh)
		public static WorldObject WorldObject (Transform meshTransform) {
			return PlaygroundParticlesC.NewWorldObject(meshTransform);
		}
		
		// Create a skinned world object reference (used for live world positioning of particles towards a mesh)
		public static SkinnedWorldObject SkinnedWorldObject (Transform meshTransform) {
			return PlaygroundParticlesC.NewSkinnedWorldObject(meshTransform);
		}
		
		// Create a manipulator object
		public static ManipulatorObjectC ManipulatorObject (MANIPULATORTYPEC type, LayerMask affects, Transform manipulatorTransform, float size, float strength) {
			return PlaygroundParticlesC.NewManipulatorObject(type,affects,manipulatorTransform,size,strength,null);
		}
		
		// Create a manipulator object by transform
		public static ManipulatorObjectC ManipulatorObject (Transform manipulatorTransform) {
			LayerMask layerMask = -1;
			return PlaygroundParticlesC.NewManipulatorObject(MANIPULATORTYPEC.Attractor,layerMask,manipulatorTransform,1f,1f,null);
		}
		
		// Create a manipulator in a PlaygroundParticlesC object
		public static ManipulatorObjectC ManipulatorObject (MANIPULATORTYPEC type, LayerMask affects, Transform manipulatorTransform, float size, float strength, PlaygroundParticlesC playgroundParticles) {
			return PlaygroundParticlesC.NewManipulatorObject(type,affects,manipulatorTransform,size,strength,playgroundParticles);
		}
		
		// Create a manipulator in a PlaygroundParticlesC object by transform
		public static ManipulatorObjectC ManipulatorObject (Transform manipulatorTransform, PlaygroundParticlesC playgroundParticles) {
			LayerMask layerMask = -1;
			return PlaygroundParticlesC.NewManipulatorObject(MANIPULATORTYPEC.Attractor,layerMask,manipulatorTransform,1f,1f,playgroundParticles);
		}
		
		// Return a manipulator in array position
		public static ManipulatorObjectC GetManipulator (int i) {
			if (reference.manipulators.Count>0 && reference.manipulators[i%reference.manipulators.Count]!=null)
				return reference.manipulators[i%reference.manipulators.Count];
			else return null;
		}
		
		// Return a manipulator in a PlaygroundParticlesC object in array position
		public static ManipulatorObjectC GetManipulator (int i, PlaygroundParticlesC playgroundParticles) {
			if (playgroundParticles.manipulators.Count>0 && playgroundParticles.manipulators[i%playgroundParticles.manipulators.Count]!=null)
				return playgroundParticles.manipulators[i%playgroundParticles.manipulators.Count];
			else return null;
		}

		// Create an empty event
		public static PlaygroundEventC CreateEvent () {
			return new PlaygroundEventC();
		}

		// Create an event into particle system
		public static PlaygroundEventC CreateEvent (PlaygroundParticlesC playgroundParticles) {
			PlaygroundEventC playgroundEvent = new PlaygroundEventC();
			playgroundParticles.events.Add (playgroundEvent);
			return playgroundEvent;
		}

		// Return an event from PlaygroundParticlesC object in array position
		public static PlaygroundEventC GetEvent (int i, PlaygroundParticlesC playgroundParticles) {
			if (playgroundParticles.events.Count>0 && playgroundParticles.events[i%playgroundParticles.events.Count]!=null)
				return playgroundParticles.events[i%playgroundParticles.events.Count];
			else return null;
		}

		public static void RemoveEvent (int i, PlaygroundParticlesC playgroundParticles) {
			i = i%playgroundParticles.events.Count;
			if (playgroundParticles.events.Count>0 && playgroundParticles.events[i]!=null) {
				if (playgroundParticles.events[i].target!=null) {
					for (int x = 0; x<playgroundParticles.events[i].target.eventControlledBy.Count; x++)
						if (playgroundParticles.events[i].target.eventControlledBy[x]==playgroundParticles)
							playgroundParticles.events[i].target.eventControlledBy.RemoveAt(x);
				}
				playgroundParticles.events.RemoveAt (i);
			}
		}
		
		// Return a Particle Playground System in array position
		public static PlaygroundParticlesC GetParticles (int i) {
			if (reference.particleSystems.Count>0 && reference.particleSystems[i%reference.particleSystems.Count]!=null)
				return reference.particleSystems[i%reference.particleSystems.Count];
			else return null;
		}
		
		// Create a projection object reference
		public static ParticleProjectionC ParticleProjection (PlaygroundParticlesC playgroundParticles)  {
			return PlaygroundParticlesC.NewProjectionObject(playgroundParticles);
		}
		
		// Create a paint object reference
		public static PaintObjectC PaintObject (PlaygroundParticlesC playgroundParticles) {
			return PlaygroundParticlesC.NewPaintObject(playgroundParticles);
		}
		
		// Live paint into a PlaygroundParticlesC PaintObject's positions
		public static int Paint (PlaygroundParticlesC playgroundParticles, Vector3 position, Vector3 normal, Transform parent, Color32 color) {
			return playgroundParticles.paint.Paint(position,normal,parent,color);
		}
		
		// Live paint into a PaintObject's positions directly
		public static void Paint (PaintObjectC paintObject, Vector3 position, Vector3 normal, Transform parent, Color32 color) {
			paintObject.Paint(position,normal,parent,color);
		}
		
		// Live erase into a PlaygroundParticlesC PaintObject's positions, returns true if position was erased
		public static bool Erase (PlaygroundParticlesC playgroundParticles, Vector3 position, float radius) {
			return playgroundParticles.paint.Erase(position,radius);
		}
		
		// Live erase into a PaintObject's positions directly, returns true if position was erased
		public static bool Erase (PaintObjectC paintObject, Vector3 position, float radius) {
			return paintObject.Erase(position,radius);
		}
		
		// Live erase into a PlaygroundParticlesC PaintObject's using a specified index, returns true if position was erased
		public static bool Erase (PlaygroundParticlesC playgroundParticles, int index) {
			return playgroundParticles.paint.Erase(index);
		}
		
		// Clear out paint in a PlaygroundParticlesC object
		public static void ClearPaint (PlaygroundParticlesC playgroundParticles) {
			if (playgroundParticles.paint!=null)
				playgroundParticles.paint.ClearPaint();
		}
		
		// Get the amount of paint positions in this PlaygroundParticlesC PaintObject
		public static int GetPaintPositionLength (PlaygroundParticlesC playgroundParticles) {
			return playgroundParticles.paint.positionLength;
		}
		
		// Set initial target position for this Particle System
		public static void SetInitialTargetPosition (PlaygroundParticlesC playgroundParticles, Vector3 position) {
			PlaygroundParticlesC.SetInitialTargetPosition(playgroundParticles,position, true);
		}
		
		// Set emission for this Particle System
		public static void Emission (PlaygroundParticlesC playgroundParticles, bool emit) {
			PlaygroundParticlesC.Emission(playgroundParticles,emit,false);
		}

		// Set emission for this Particle System controlling to run rest emission
		public static void Emission (PlaygroundParticlesC playgroundParticles, bool emit, bool restEmission) {
			PlaygroundParticlesC.Emission(playgroundParticles,emit,restEmission);
		}
		
		// Clear out this Particle System
		public static void Clear (PlaygroundParticlesC playgroundParticles) {
			PlaygroundParticlesC.Clear(playgroundParticles);
		}
		
		// Refresh source scatter for this Particle System
		public static void RefreshScatter (PlaygroundParticlesC playgroundParticles) {
			playgroundParticles.RefreshScatter();
		}
		
		// Instantiate a preset by name reference
		public static PlaygroundParticlesC InstantiatePreset (string presetName) {
			GameObject presetGo = ResourceInstantiate("Presets/"+presetName);
			PlaygroundParticlesC presetParticles = presetGo.GetComponent<PlaygroundParticlesC>();
			if (presetParticles!=null) {
				if (reference==null)
					PlaygroundC.ResourceInstantiate("Playground Manager");
				if (reference) {
					if (reference.autoGroup && presetParticles.particleSystemTransform.parent==null)
						presetParticles.particleSystemTransform.parent = referenceTransform;
					particlesQuantity++;
					//reference.particleSystems.Add(presetParticles);
					presetParticles.particleSystemId = particlesQuantity;
				}
				presetGo.name = presetName;
				return presetParticles;
			} else {
				if (presetGo.name.Contains("Presets/"))
				    presetGo.name = presetGo.name.Remove(0, 8);
				return null;
			}
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Shared functions
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		// Return pixels from a texture
		public static Color32[] GetPixels (Texture2D image) {
			if (reference && reference.pixelFilterMode==PIXELMODEC.Bilinear) {
				Color32[] pixels = new Color32[image.width*image.height];
				for (int y = 0; y<image.height; y++) {
					for (int x = 0; x<image.width; x++) {
						pixels[(y*image.width)+x] = image.GetPixelBilinear((x*1f)/image.width, (y*1f)/image.height); 
					}
				}
				return pixels;
			} else return image.GetPixels32();
		}
		
		// Return offset based on image size
		public static Vector3 Offset (PLAYGROUNDORIGINC origin, int imageWidth, int imageHeight, float meshScale) {
			Vector3 offset = Vector3.zero;
			switch (origin) {
			case PLAYGROUNDORIGINC.TopLeft: 		offset.y = -imageHeight*meshScale; break;
			case PLAYGROUNDORIGINC.TopCenter: 		offset.y = -imageHeight*meshScale; offset.x = (-imageWidth*meshScale)/2; break;
			case PLAYGROUNDORIGINC.TopRight: 		offset.y = -imageHeight*meshScale; offset.x = (-imageWidth*meshScale); break;
			case PLAYGROUNDORIGINC.MiddleLeft: 		offset.y = -imageHeight*meshScale/2; break;
			case PLAYGROUNDORIGINC.MiddleCenter:	offset.y = -imageHeight*meshScale/2; offset.x = (-imageWidth*meshScale)/2; break;
			case PLAYGROUNDORIGINC.MiddleRight:		offset.y = -imageHeight*meshScale/2; offset.x = (-imageWidth*meshScale); break;
			case PLAYGROUNDORIGINC.BottomCenter:	offset.x = (-imageWidth*meshScale)/2; break;
			case PLAYGROUNDORIGINC.BottomRight:		offset.x = (-imageWidth*meshScale); break;
			}
			return offset;
		}
		
		// Return random vector3-array
		public static Vector3[] RandomVector3 (int length, Vector3 min, Vector3 max) {
			Vector3[] v3 = new Vector3[length];
			for (int i = 0; i<length; i++) {
				v3[i] = new Vector3(
					PlaygroundParticlesC.RandomRange(random, min.x, max.x),
					PlaygroundParticlesC.RandomRange(random, min.y, max.y),
					PlaygroundParticlesC.RandomRange(random, min.z, max.z)
					);
			}
			return v3;
		}
		
		// Returns a float array by random values
		public static float[] RandomFloat (int length, float min, float max) {
			float[] f = new float[length];
			for (int i = 0; i<length; i++) {
				f[i] = PlaygroundParticlesC.RandomRange (random, min, max);
			}
			return f;
		}

		// Shuffle an existing built-in float array
		public static void ShuffleArray (float[] arr) {
			int r;
			float tmp;
			for (int i = arr.Length - 1; i > 0; i--) {
				r = random.Next(0,i);
				tmp = arr[i];
				arr[i] = arr[r];
				arr[r] = tmp;
			}
		}

		// Shuffle an existing built-in int array
		public static void ShuffleArray (int[] arr) {
			int r;
			int tmp;
			for (int i = arr.Length - 1; i > 0; i--) {
				r = random.Next(0,i);
				tmp = arr[i];
				arr[i] = arr[r];
				arr[r] = tmp;
			}
		}
		
		// Compare and return largest array
		public static int Largest (int[] compare) {
			int largest = 0;
			for (int i = 0; i<compare.Length; i++)
				if (compare[i]>largest) largest = i;
			return largest;
		}
		
		// Count the completely transparent pixels in a Texture2D
		public static int CountZeroAlphasInTexture (Texture2D image) {
			int alphaCount = 0;
			Color32[] pixels = image.GetPixels32();
			for (int x = 0; x<pixels.Length; x++)
				if (pixels[x].a==0)
					alphaCount++;
			return alphaCount;
		}
		
		// Instantiate from Resources
		public static GameObject ResourceInstantiate (string n) {
			GameObject res = UnityEngine.Resources.Load(n) as GameObject;
			if (!res)
				return null;
			GameObject go = Instantiate(res) as GameObject;
			go.name = n;
			return go;
		}

		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Playground Manager
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		public static void TimeReset () {
			globalDeltaTime = .0f;
			#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying) {
				globalTime = 0f;
			} else
				globalTime = Time.timeSinceLevelLoad;
			#else
				globalTime = Time.timeSinceLevelLoad;
			#endif
			lastTimeUpdated = globalTime;
		}
		
		// Initializes the manager and particle systems
		public IEnumerator InitializePlayground () {

			// Check for duplicates of the Playground Manager
			if (reference!=null && reference!=this) {
				yield return null;
				Debug.Log("There can only be one instance of the Playground Manager in the scene.");
				DestroyImmediate(gameObject);
				yield break;
			}

			// New random
			random = new System.Random();

			// Reset time
			TimeReset();
			
			// Remove any null particle systems
			for (int i = 0; i<particleSystems.Count; i++) {
				if (particleSystems[i]!=null) {
					particleSystems[i].particleSystemId = i;
				} else {
					particleSystems.RemoveAt(i);
					i--;
				}
			}
			
			// Set quantity counter
			particlesQuantity = particleSystems.Count;
		}

		public static void SetTime () {
			
			// Set time
			#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying)
				globalTime += (Time.realtimeSinceStartup-lastTimeUpdated)*globalTimescale;
			else
				globalTime += (Time.timeSinceLevelLoad-lastTimeUpdated)*globalTimescale;
			#else
				globalTime += (Time.timeSinceLevelLoad-lastTimeUpdated)*globalTimescale;
			#endif

			// Set delta time
			globalDeltaTime = globalTime-lastTimeUpdated;
			
			// Set interval stamp
			lastTimeUpdated = globalTime;
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// MonoBehaviours
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		// OnEnable is called in both Edit- and Play Mode
		// Initializes all particle systems
		void OnEnable () {

			// Cache the Playground reference
			reference = this;
			referenceTransform = transform;
			referenceGameObject = gameObject;

			// Initialize
			StartCoroutine(InitializePlayground());
		}

		// Set initial time
		void Start () {SetTime();}
		
		// Update is called both in Edit- and Play Mode once per frame
		void Update () {

			// Prepare all global manipulators
			for (int m = 0; m<manipulators.Count; m++)
				manipulators[m].Update();

			// Update all particle systems
			for (int i = 0; i<particleSystems.Count; i++) {
				if (particleSystems[i]!=null &&
				    !particleSystems[i].isSnapshot &&
				    particleSystems[i].shurikenParticleSystem!=null &&
				    particleSystems[i].particleSystemGameObject.activeInHierarchy && 
				    Time.frameCount%particleSystems[i].updateRate==0) {
					PlaygroundParticlesC.Update(particleSystems[i]);
				}
			}

			SetTime();
		}

		// Threading
		#if UNITY_WINRT && !UNITY_EDITOR
		public static Thread RunAsync (IAsyncAction a) {
			lock (a) {
				ThreadPool.RunAsync(RunAction, a);
			}
			return null;
		}
		#else
		public static Thread RunAsync (Action a) {
			lock (a) {
				ThreadPool.QueueUserWorkItem(RunAction, a);
			}
			return null;
		}
		#endif
		private static void RunAction (object a) {
			try {
				((Action)a)();
			} catch {}
		}
		
		// Reset time when turning back to the editor
		#if UNITY_EDITOR
		public IEnumerator OnApplicationPause (bool pauseStatus) {
			if (!pauseStatus) {
				TimeReset();
				yield return null;
				foreach(PlaygroundParticlesC p in particleSystems)
					if (!p.isSnapshot)
						PlaygroundParticlesC.SetParticleTimeNow(p);
			}
		}
		#endif
	}

	// Holds information about a Paint source
	[Serializable]
	public class PaintObjectC {
		[HideInInspector] public List<PaintPositionC> paintPositions; 	// Position data for each origin point
		[HideInInspector] public int positionLength;					// The current length of position array
		[HideInInspector] public Vector3 lastPaintPosition;				// The last painted position
		[HideInInspector] public COLLISIONTYPEC collisionType;			// The collider type this PaintObject sees when painting
		public float minDepth = -1f;									// Minimum depth for 2D collisions
		public float maxDepth = 1f;										// Maximum depth for 2D collisions
		public float spacing;											// The required space between the last and current paint position 
		public LayerMask layerMask = -1;								// The layers this PaintObject sees when painting
		public PlaygroundBrushC brush;									// The brush data for this PaintObject
		public bool exceedMaxStopsPaint;								// Should painting stop when paintPositions is equal to maxPositions (if false paint positions will be removed from list when painting new ones)
		public bool initialized = false;								// Is this PaintObject initialized yet?
		
		// Initializes a PaintObject for painting
		public void Initialize () {
			paintPositions = new List<PaintPositionC>();
			brush = new PlaygroundBrushC();
			lastPaintPosition = PlaygroundC.initialTargetPosition;
			initialized = true;
		}
		
		// Live paint into this PaintObject using a Ray and color information
		public bool Paint (Ray ray, Color32 color) {
			if (collisionType==COLLISIONTYPEC.Physics3D) {
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, brush.distance, layerMask)) {
					Paint(hit.point, hit.normal, hit.transform, color);
					lastPaintPosition = hit.point;
					return true;
				}
			} else {
				RaycastHit2D hitInfo2D = Physics2D.Raycast(ray.origin, ray.direction, brush.distance, layerMask, minDepth, maxDepth);
				if (hitInfo2D.collider!=null) {
					Paint(hitInfo2D.point, hitInfo2D.normal, hitInfo2D.transform, color);
					lastPaintPosition = hitInfo2D.point;
					return true;
				}
			}
			return false;
		}
		
		// Live paint into this PaintObject (single point with information about position, normal, parent and color), returns current painted index
		public int Paint (Vector3 pos, Vector3 norm, Transform parent, Color32 color) {		
			PaintPositionC pPos = new PaintPositionC();
			
			if (parent) {
				pPos.parent = parent;
				pPos.initialPosition = parent.InverseTransformPoint(pos);
				pPos.initialNormal = parent.InverseTransformDirection(norm);
			} else {
				pPos.initialPosition = pos;
				pPos.initialNormal = norm;
			}
			
			pPos.color = color;
			pPos.position = pPos.initialPosition;
			
			paintPositions.Add(pPos);
			
			positionLength = paintPositions.Count;
			if (!exceedMaxStopsPaint && positionLength>PlaygroundC.reference.paintMaxPositions) {
				paintPositions.RemoveAt(0);
				positionLength = paintPositions.Count;
			}
			
			return positionLength-1;
		}
		
		// Erase in this PaintObject using a position and radius, returns true if position was erased
		public bool Erase (Vector3 pos, float radius) {
			bool erased = false;
			if (paintPositions.Count==0) return false;
			for (int i = 0; i<paintPositions.Count; i++) {
				if (Vector3.Distance(paintPositions[i].position, pos)<radius) {
					paintPositions.RemoveAt(i);
					erased = true;
				}
			}
			positionLength = paintPositions.Count;
			return erased;
		}
		
		// Erase in this PaintObject using a specified index, returns true if position at index was found and removed
		public bool Erase (int index) {
			if (index>=0 && index<positionLength) {
				paintPositions.RemoveAt(index);
				positionLength = paintPositions.Count;
				return true;
			} else return false;
		}
		
		// Return position at index of PaintObject's PaintPosition
		public Vector3 GetPosition (int index) {
			index=index%positionLength;
			return paintPositions[index].position;
		}
		
		// Return color at index of PaintObject's PaintPosition
		public Color32 GetColor (int index) {
			index=index%positionLength;
			return paintPositions[index].color;
		}
		
		// Return normal at index of PaintObject's PaintPosition
		public Vector3 GetNormal (int index) {
			index=index%positionLength;
			return paintPositions[index].initialNormal;
		}
		
		// Return parent at index of PaintObject's PaintPosition
		public Transform GetParent (int index) {
			index=index%positionLength;
			return paintPositions[index].parent;
		}

		// Return rotation of parent at index of PaintObject's PaintPosition
		public Quaternion GetRotation (int index) {
			index=index%positionLength;
			return paintPositions[index].parentRotation;
		}
		
		// Live positioning of paintPositions regarding their parent
		// This happens automatically in PlaygroundParticlesC.Update() - only use this if you need to set all positions at once
		public void Update () {
			for (int i = 0; i<paintPositions.Count; i++) {
				if (paintPositions[i].parent==null)
					continue;
				else
					Update(i);
			}
		}
		
		// Update specific position
		public void Update (int thisPosition) {
			thisPosition = thisPosition%positionLength;
			//if (!paintPositions[thisPosition].parent) {
			//	RemoveNonParented(thisPosition);
			//	return;
			//}
			paintPositions[thisPosition].position = paintPositions[thisPosition].parent.TransformPoint(paintPositions[thisPosition].initialPosition);
			paintPositions[thisPosition].parentRotation = paintPositions[thisPosition].parent.rotation;
		}
		
		// Clear out all emission positions where the parent transform has been removed
		public void RemoveNonParented () {
			for (int i = 0; i<paintPositions.Count; i++)
			if (paintPositions[i].parent==null) {
				paintPositions.RemoveAt(i);
				i--;
			}
		}
		
		// Clear out emission position where the parent transform has been removed
		// Returns true if this position didn't have a parent
		public bool RemoveNonParented (int thisPosition) {
			thisPosition = thisPosition%positionLength;
			if (paintPositions[thisPosition].parent==null) {
				paintPositions.RemoveAt(thisPosition);
				return true;
			}
			return false;
		}
		
		// Clear out the painted positions
		public void ClearPaint () {
			paintPositions = new List<PaintPositionC>();
			positionLength = 0;
		}
		
		// Clone this PaintObject
		public PaintObjectC Clone () {
			PaintObjectC paintObject = new PaintObjectC();
			if (paintPositions!=null && paintPositions.Count>0) {
				paintObject.paintPositions = new List<PaintPositionC>();
				paintObject.paintPositions.AddRange(paintPositions);
			}
			paintObject.positionLength = positionLength;
			paintObject.lastPaintPosition = lastPaintPosition;
			paintObject.spacing = spacing;
			paintObject.layerMask = layerMask;
			paintObject.collisionType = collisionType;
			if (brush!=null)
				paintObject.brush = brush.Clone();
			else
				paintObject.brush = new PlaygroundBrushC();
			paintObject.exceedMaxStopsPaint = exceedMaxStopsPaint;
			paintObject.initialized = initialized;
			return paintObject;
		}
	}

	// Constructor for a painted position
	[Serializable]
	public class PaintPositionC {
		public Vector3 position;								// Emission spot in local position of parent
		public Color32 color;									// Color of emission spot
		public Transform parent;								// The parent transform
		public Vector3 initialPosition;							// The first position where this originally were painted
		public Vector3 initialNormal;							// The first normal direction when painted
		public Quaternion parentRotation;						// The rotation of the parent
	}

	// Holds information about a brush used for source painting
	[Serializable]
	public class PlaygroundBrushC {
		public Texture2D texture;								// The texture to construct this Brush from
		public float scale = 1f;								// The scale of this Brush (measured in Units)
		public BRUSHDETAILC detail = BRUSHDETAILC.High;			// The detail textures of this brush
		public float distance = 10000;							// The distance the brush reaches
		[HideInInspector] public Color32[] color;				// Color data of this brush
		[HideInInspector] public int colorLength;				// The length of color array
		
		// Set the texture of this brush
		public void SetTexture (Texture2D newTexture) {
			texture = newTexture;
			Construct();
		}
		
		// Cache the color information from this brush
		public void Construct () {
			color = PlaygroundC.GetPixels(texture);
			colorLength = color.Length;
		}
		
		// Return color at index of Brush
		public Color32 GetColor (int index) {
			index=index%colorLength;
			return color[index];
		}
		
		// Set color at index of Brush
		public void SetColor (Color32 c, int index) {
			color[index] = c;
		}
		
		// Clone this PlaygroundBrush
		public PlaygroundBrushC Clone () {
			PlaygroundBrushC playgroundBrush = new PlaygroundBrushC();
			playgroundBrush.texture = texture;
			playgroundBrush.scale = scale;
			playgroundBrush.detail = detail;
			playgroundBrush.distance = distance;
			if (playgroundBrush.color!=null)
				playgroundBrush.color = color.Clone() as Color32[];
			playgroundBrush.colorLength = colorLength;
			return playgroundBrush;
		}
	}

	// Holds information about a State source
	[Serializable]
	public class ParticleStateC {
		[HideInInspector] Color32[] color; 						// Color data
		[HideInInspector] Vector3[] position;					// Position data
		[HideInInspector] Vector3[] normals;					// Normal data
		public Texture2D stateTexture;							// The texture to construct this state from (used to color each vertex if mesh is used)
		public Texture2D stateDepthmap;							// The texture to use as depthmap for this state. A grayscale image of the same size as stateTexture is required
		public float stateDepthmapStrength = 1f;				// How much the grayscale from stateDepthmap will affect z-value
		public Mesh stateMesh;									// The mesh used to set this state's positions. Positions will be calculated per vertex.
		public string stateName;								// The name of this state
		public float stateScale;								// The scale of this state (measured in units)
		public Vector3 stateOffset;								// The offset of this state in world position (measured in units)
		public Transform stateTransform;						// The transform that act as parent to this state
		[HideInInspector] public bool initialized = false; 		// Is this ParticleState intialized?
		[HideInInspector] public int colorLength;				// The length of color array
		[HideInInspector] public int positionLength;			// The length of position array
		
		// Initializes a ParticleState for construction
		public void Initialize () {
			if (stateMesh==null && stateTexture!=null) {
				ConstructParticles(stateTexture, stateScale, stateOffset, stateName, stateTransform);
				if (stateDepthmap!=null)
					SetDepthmap(); 
			} else if (stateMesh!=null && stateTexture!=null) {
				ConstructParticles(stateMesh, stateTexture, stateScale, stateOffset, stateName, stateTransform);
			} else if (stateMesh!=null && stateTexture==null) {
				ConstructParticles(stateMesh, stateScale, stateOffset, stateName, stateTransform);
			} else Debug.Log("State Texture or State Mesh returned null. Please assign an object to State: "+stateName+".");
			initialized = true;
		}
		
		// Construct Image data
		public void ConstructParticles (Texture2D image, float scale, Vector3 offset, string newStateName, Transform newStateTransform) {
			List<Color32> tmpColor = new List<Color32>();
			List<Vector3> tmpPosition = new List<Vector3>();
			color = PlaygroundC.GetPixels(image);
			bool readAlpha = false;
			if (PlaygroundC.reference)
				readAlpha = PlaygroundC.reference.buildZeroAlphaPixels;
			int x = 0;
			int y = 0;
			for (int i = 0; i<color.Length; i++) {
				if (readAlpha || color[i].a!=0) {
					tmpColor.Add(color[i]);
					tmpPosition.Add(new Vector3(x,y,0));			
				}
				x++; x=x%image.width;
				if (x==0 && i!=0) y++;
			}
			color = tmpColor.ToArray() as Color32[];
			position = tmpPosition.ToArray() as Vector3[];
			tmpColor = null;
			tmpPosition = null;
			normals = new Vector3[position.Length];
			for (int n = 0; n<normals.Length; n++) normals[n] = Vector3.forward;
			stateTransform = newStateTransform;
			stateTexture = image;
			colorLength = color.Length;
			positionLength = position.Length;
			stateScale = scale;
			stateOffset = offset;
			stateName = newStateName;
			initialized = true;
		}
		
		// Construct Mesh data with texture
		public void ConstructParticles (Mesh mesh, Texture2D texture, float scale, Vector3 offset, string newStateName, Transform newStateTransform) {
			position = mesh.vertices;
			normals = mesh.normals;
			Vector2[] uvs = mesh.uv;
			color = new Color32[uvs.Length];
			for (int i = 0; i<position.Length; i++)
				color[i] = texture.GetPixelBilinear(uvs[i].x, uvs[i].y);
			stateMesh = mesh;
			stateTransform = newStateTransform;
			colorLength = color.Length;
			positionLength = position.Length;
			stateScale = scale;
			stateOffset = offset;
			stateName = newStateName;
			initialized = true;
		}
		
		// Construct Mesh data
		public void ConstructParticles (Mesh mesh, float scale, Vector3 offset, string newStateName, Transform newStateTransform) {
			position = mesh.vertices;
			normals = mesh.normals;
			Vector2[] uvs = mesh.uv;
			color = new Color32[uvs.Length];
			stateMesh = mesh;
			stateTransform = newStateTransform;
			colorLength = color.Length;
			positionLength = position.Length;
			stateScale = scale;
			stateOffset = offset;
			stateName = newStateName;
			initialized = true;
		}
		
		// Set depth map to ParticleState
		public void SetDepthmap () {
			Color32[] depthMapPixels = PlaygroundC.GetPixels(stateDepthmap);
			float z;
			for (int x = 0; x<depthMapPixels.Length; x++) {
				z = ((depthMapPixels[x].r+depthMapPixels[x].g+depthMapPixels[x].b)/3);
				position[x%position.Length].z -= (z*stateDepthmapStrength/255);
			}
		}
		
		// Return color at index of ParticleState
		public Color32 GetColor (int index) {
			index=index%colorLength;
			return color[index];
		}
		
		// Return position at index of ParticleState
		public Vector3 GetPosition (int index) {
			index=index%positionLength;
			return (position[index]+stateOffset)*stateScale;
		}
		
		// Return normal at index of ParticleState
		public Vector3 GetNormal (int index) {
			index=index%positionLength;
			return normals[index];
		}
		
		// Return colors in ParticleState
		public Color32[] GetColors () {
			return color.Clone() as Color32[];
		}
		
		// Return positions in ParticleState
		public Vector3[] GetPositions () {
			return position.Clone() as Vector3[];
		}
		
		// Return normals in ParticleState
		public Vector3[] GetNormals () {
			return normals.Clone() as Vector3[];
		}
		
		// Set color at index of ParticleState
		public void SetColor (int index, Color32 c) {
			color[index] = c;
		}
		
		// Set position at index of ParticleState
		public void SetPosition (int index, Vector3 v) {
			position[index] = v;
		}
		
		// Set normal at index of ParticleState
		public void SetNormal (int index, Vector3 v) {
			normals[index] = v;
		}
		
		// Return position from parent's TransformPoint (can only be called from main-thread)
		public Vector3 GetParentedPosition (int thisPosition) {
			thisPosition = thisPosition%positionLength;
			return stateTransform.TransformPoint((position[thisPosition]+stateOffset)*stateScale);
		}
		
		// Return a copy of this ParticleState
		public ParticleStateC Clone () {
			ParticleStateC particleState = new ParticleStateC();
			particleState.stateName = stateName;
			particleState.stateScale = stateScale;
			particleState.stateTexture = stateTexture;
			particleState.stateDepthmap = stateDepthmap;
			particleState.stateDepthmapStrength = stateDepthmapStrength;
			particleState.stateMesh = stateMesh;
			particleState.stateOffset = stateOffset;
			particleState.colorLength = colorLength;
			particleState.positionLength = positionLength;
			particleState.stateTransform = stateTransform;
			return particleState;
		}
	}

	[Serializable]
	public class ParticleProjectionC {
		[HideInInspector] private Color32[] sourceColors;					// Color data
		[HideInInspector] private Vector3[] sourcePositions;				// Position data
		[HideInInspector] private Vector3[] targetPositions;				// Projected position data
		[HideInInspector] private Vector3[] targetNormals;					// Projected normal data
		[HideInInspector] private bool[] hasProjected;						// Projection has hit surface at point
		[HideInInspector] private Transform[] targetParents;				// Projected transforms 
		public Texture2D projectionTexture;									// The texture to project
		public Vector2 projectionOrigin; 									// The origin offset in Units
		public Transform projectionTransform;								// Transform to project from
		public Matrix4x4 projectionMatrix;									// Matrix to project from
		public Vector3 projectionPosition;									// Position of projection source
		public Vector3 projectionDirection;									// Direction of projection source
		public Quaternion projectionRotation;								// Rotation of projection source
		public float projectionDistance = 1000f;							// The distance in Units the projection travels
		public float projectionScale = .1f;									// The scale of projection in Units
		public LayerMask projectionMask = -1;								// Layers seen by projection
		public COLLISIONTYPEC collisionType;								// Determines if 3d- or 2d colliders are seen by projection
		public float minDepth = -1f;										// Minimum depth of 2d colliders seen by projection 
		public float maxDepth = 1f;											// Maximum depth of 2d colliders seen by projection 
		public float surfaceOffset = 0f;									// The offset from projected surface
		public bool liveUpdate = true;										// Update this projector each frame
		public bool hasRefreshed = false;									// Is this projector finished refreshing?
		[HideInInspector] public bool initialized = false;					// Is this projector ready?
		[HideInInspector] public int colorLength;							// The length of color array
		[HideInInspector] public int positionLength;						// The length of position array

		// Initialize this ParticleProjection object
		public void Initialize () {
			if (!projectionTexture) return;
			Construct(projectionTexture, projectionTransform);
			if (!liveUpdate) {
				UpdateSource();
				Update();
			}
			initialized = true;
			hasRefreshed = false;
		}
		
		// Build source data
		public void Construct (Texture2D image, Transform transform) {
			List<Color32> tmpColor = new List<Color32>();
			List<Vector3> tmpPosition = new List<Vector3>();
			sourceColors = PlaygroundC.GetPixels(image);
			bool readAlpha = false;
			if (PlaygroundC.reference)
				readAlpha = PlaygroundC.reference.buildZeroAlphaPixels;
			
			int x = 0;
			int y = 0;
			for (int i = 0; i<sourceColors.Length; i++) {
				if (readAlpha || sourceColors[i].a!=0) {
					tmpColor.Add(sourceColors[i]);
					tmpPosition.Add(new Vector3(x*projectionScale,y*projectionScale,0));			
				}
				x++; x=x%image.width;
				if (x==0 && i!=0) y++;
			}
			sourceColors = tmpColor.ToArray();
			sourcePositions = tmpPosition.ToArray();
			tmpColor = null;
			tmpPosition = null;
			targetPositions = new Vector3[sourcePositions.Length];
			targetNormals = new Vector3[sourcePositions.Length];
			targetParents = new Transform[sourcePositions.Length];
			hasProjected = new bool[sourcePositions.Length];
			positionLength = sourcePositions.Length;
			colorLength = sourceColors.Length;
			projectionTexture = image;
			projectionTransform = transform;
			projectionMatrix = new Matrix4x4();
		}

		// Update source matrix
		public void UpdateSource () {
			projectionPosition = projectionTransform.position;
			projectionDirection = projectionTransform.forward;
			projectionRotation = projectionTransform.rotation;
			projectionMatrix.SetTRS(projectionPosition, projectionRotation, projectionTransform.localScale);
		}

		// Project all particle sources (only call this if you need to set all particles at once)
		public void Update () {
			for (int i = 0; i<positionLength; i++)
				Update(i);
		}
		
		// Project a single particle source
		public void Update (int index) {
			index=index%positionLength;
			Vector3 sourcePosition = projectionMatrix.MultiplyPoint3x4(sourcePositions[index]+new Vector3(projectionOrigin.x, projectionOrigin.y, 0));
			if (collisionType==COLLISIONTYPEC.Physics3D) {
				RaycastHit hit;
				if (Physics.Raycast(sourcePosition, projectionDirection, out hit, projectionDistance, projectionMask)) {
					targetPositions[index] = hit.point+(hit.normal*surfaceOffset);
					targetNormals[index] = hit.normal;
					targetParents[index] = hit.transform;
					hasProjected[index] = true;
				} else {
					targetPositions[index] = PlaygroundC.initialTargetPosition;
					targetNormals[index] = Vector3.forward;
					hasProjected[index] = false;
					targetParents[index] = null;
				}
			} else {
				RaycastHit2D hit2d = Physics2D.Raycast (sourcePosition, projectionDirection, projectionDistance, projectionMask, minDepth, maxDepth);
				if (hit2d.collider!=null) {
					targetPositions[index] = hit2d.point+(hit2d.normal*surfaceOffset);
					targetNormals[index] = hit2d.normal;
					targetParents[index] = hit2d.transform;
					hasProjected[index] = true;
				} else {
					targetPositions[index] = PlaygroundC.initialTargetPosition;
					targetNormals[index] = Vector3.forward;
					hasProjected[index] = false;
					targetParents[index] = null;
				}
			}
		}
		
		// Return color at index of ParticleProjection
		public Color32 GetColor (int index) {
			index=index%colorLength;
			return sourceColors[index];
		}
		
		// Return position at index of ParticleProjection
		public Vector3 GetPosition (int index) {
			index=index%positionLength;
			return (targetPositions[index]);
		}
		
		// Return normal at index of ParticleProjection
		public Vector3 GetNormal (int index) {
			index=index%positionLength;
			return targetNormals[index];
		}

		// Return parent at index of ParticleProjection
		public Transform GetParent (int index) {
			index=index%positionLength;
			return targetParents[index];
		}

		// Return projection status at index of ParticleProjection
		public bool HasProjection (int index) {
			index=index%positionLength;
			return hasProjected[index];
		}

		// Return a copy of this ParticleProjectionC object
		public ParticleProjectionC Clone () {
			ParticleProjectionC particleProjection 	= new ParticleProjectionC();
			if (sourceColors!=null)
				particleProjection.sourceColors		= (Color32[])sourceColors.Clone ();
			if (sourcePositions!=null)
				particleProjection.sourcePositions	= (Vector3[])sourcePositions.Clone ();
			if (targetPositions!=null)
				particleProjection.targetPositions	= (Vector3[])targetPositions.Clone ();
			if (targetNormals!=null)
				particleProjection.targetNormals	= (Vector3[])targetNormals.Clone ();
			if (hasProjected!=null)
				particleProjection.hasProjected		= (bool[])hasProjected.Clone ();
			if (targetParents!=null)
				particleProjection.targetParents	= (Transform[])targetParents.Clone ();
			particleProjection.projectionTexture	= projectionTexture;
			particleProjection.projectionOrigin		= projectionOrigin;
			particleProjection.projectionTransform	= projectionTransform;
			particleProjection.projectionMatrix		= projectionMatrix;
			particleProjection.projectionPosition	= projectionPosition;
			particleProjection.projectionDirection	= projectionDirection;
			particleProjection.projectionRotation	= projectionRotation;
			particleProjection.projectionDistance	= projectionDistance;
			particleProjection.projectionScale		= projectionScale;
			particleProjection.projectionMask		= projectionMask;
			particleProjection.collisionType		= collisionType;
			particleProjection.minDepth				= minDepth;
			particleProjection.maxDepth				= maxDepth;
			particleProjection.surfaceOffset		= surfaceOffset;
			particleProjection.liveUpdate			= liveUpdate;
			particleProjection.hasRefreshed			= hasRefreshed;
			particleProjection.initialized			= initialized;
			particleProjection.colorLength			= colorLength;
			particleProjection.positionLength		= positionLength;
			return particleProjection;
		}
	}

	// Holds AnimationCurves in X, Y and Z variables
	[Serializable]
	public class Vector3AnimationCurveC {
		public AnimationCurve x;							// AnimationCurve for X-axis
		public AnimationCurve y;							// AnimationCurve for Y-axis
		public AnimationCurve z;							// AnimationCurve for Z-axis
		
		// Return a vector3 at time
		public Vector3 Evaluate (float time) {
			return new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
		}
		
		// Return a copy of this Vector3AnimationCurve
		public Vector3AnimationCurveC Clone () {
			Vector3AnimationCurveC vector3AnimationCurveClone = new Vector3AnimationCurveC();
			vector3AnimationCurveClone.x = new AnimationCurve(x.keys);
			vector3AnimationCurveClone.y = new AnimationCurve(y.keys);
			vector3AnimationCurveClone.z = new AnimationCurve(z.keys);
			return vector3AnimationCurveClone;
		}
	}

	// Holds information about a World Object
	[Serializable]
	public class WorldObjectBaseC {
		public GameObject gameObject;							// The GameObject of this World Object
		[HideInInspector] public Transform transform;			// The Transform of this World Object
		[HideInInspector] public Rigidbody rigidbody;			// The Rigidbody of this World Object
		[HideInInspector] public MeshFilter meshFilter;			// The mesh filter of this World Object (will be null for skinned meshes)
		[HideInInspector] public Mesh mesh;						// The mesh of this World Object
		[HideInInspector] public Vector3[] vertexPositions;		// The vertices of this World Object
		[HideInInspector] public Vector3[] normals;				// The normals of this World Object
		[HideInInspector] public bool updateNormals = true;		// Should normals update?
		[NonSerialized] public int cachedId;					// The id of this World Object (used to keep track when this object changes)
		[NonSerialized] public bool initialized = false;		// Is this World Object initialized?
	}

	[Serializable]
	public class WorldObject : WorldObjectBaseC {
		[HideInInspector] public Renderer renderer;

		// Initialize this WorldObject
		public void Initialize () {
			gameObject = transform.gameObject;
			rigidbody = gameObject.rigidbody;
			renderer = transform.GetComponentInChildren<Renderer>();
			meshFilter = transform.GetComponentInChildren<MeshFilter>();
			if (meshFilter!=null) {
				mesh = meshFilter.sharedMesh;
				vertexPositions = mesh.vertices;
				normals = mesh.normals;
			}
			cachedId = gameObject.GetInstanceID();
			initialized = true;
		}

		// Update this WorldObject
		public void Update () {
			if (mesh!=null) {
				vertexPositions = mesh.vertices;
				if (updateNormals)
					normals = mesh.normals;
			}
		}

		// Return a copy of this WorldObject
		public WorldObject Clone () {
			WorldObject worldObject = new WorldObject();
			worldObject.gameObject = gameObject;
			worldObject.transform = transform;
			worldObject.rigidbody = rigidbody;
			worldObject.renderer = renderer;
			worldObject.meshFilter = meshFilter;
			worldObject.mesh = mesh;
			worldObject.updateNormals = updateNormals;
			return worldObject;
		}
	}

	[Serializable]
	public class SkinnedWorldObject : WorldObjectBaseC {
		public int downResolution = 1;							// Downresolution will skip vertices to set fewer target positions in a SkinnedWorldObject
		[HideInInspector] public SkinnedMeshRenderer renderer;	// The renderer of this SkinnedWorldObject
		private Transform[] boneTransforms;						// The bones of this SkinnedWorldObject
		private BoneWeight[] weights;							// The weights of this SkinnedWorldObject
		private Matrix4x4[] bindPoses;							// The binding poses of this SkinnedWorldObject
		private Matrix4x4[] boneMatrices;						// The bone matrices of this SkinnedWorldObject
		private Vector3[] vertices;								// The calculated world vertices of this SkinnedWorldObject
		private Vector3[] localVertices;						// The local vertices to calculate upon of this SkinnedWorldObject

		public void Initialize () {
			gameObject = transform.gameObject;
			cachedId = gameObject.GetInstanceID();
			rigidbody = gameObject.rigidbody;
			renderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
			mesh = renderer.sharedMesh;
			normals = mesh.normals;
			vertices = new Vector3[mesh.vertices.Length];
			localVertices = mesh.vertices;
			weights = mesh.boneWeights;
			boneTransforms = renderer.bones;
			bindPoses = mesh.bindposes;
			boneMatrices = new Matrix4x4[boneTransforms.Length];
			vertexPositions = vertices;
			initialized = true;
			Update();
		}

		public void MeshUpdate () {
			if (initialized) {
				localVertices = mesh.vertices;
				if (updateNormals)
					normals = mesh.normals;
			}
		}

		public void BoneUpdate () {
			if (initialized) {
				for (int i = 0; i<boneMatrices.Length; i++) 
					boneMatrices[i] = boneTransforms[i].localToWorldMatrix * bindPoses[i];
			}
		}

		public void Update () {
			if (initialized) {
				Matrix4x4 vertexMatrix = new Matrix4x4();
				for (int i = 0; i<vertices.Length; i++) {
					Matrix4x4 m0 = boneMatrices[weights[i].boneIndex0];
					Matrix4x4 m1 = boneMatrices[weights[i].boneIndex1];
					Matrix4x4 m2 = boneMatrices[weights[i].boneIndex2];
					Matrix4x4 m3 = boneMatrices[weights[i].boneIndex3];
					
					for (int n=0; n<16; n++) {
						vertexMatrix[n] =
							m0[n] * weights[i].weight0 +
							m1[n] * weights[i].weight1 +
							m2[n] * weights[i].weight2 +
							m3[n] * weights[i].weight3;
					}
					vertices[i] = vertexMatrix.MultiplyPoint3x4(localVertices[i]);
				}
				vertexPositions = vertices;
			}
		}

		// Return a copy of this SkinnedWorldObject
		public SkinnedWorldObject Clone () {
			SkinnedWorldObject skinnedWorldObject = new SkinnedWorldObject();
			skinnedWorldObject.downResolution = downResolution;
			skinnedWorldObject.gameObject = gameObject;
			skinnedWorldObject.transform = transform;
			skinnedWorldObject.rigidbody = rigidbody;
			skinnedWorldObject.renderer = renderer;
			skinnedWorldObject.meshFilter = meshFilter;
			skinnedWorldObject.mesh = mesh;
			skinnedWorldObject.updateNormals = updateNormals;
			return skinnedWorldObject;
		}
	}

	// Holds information about a Manipulator Object
	[Serializable]
	public class ManipulatorObjectC {
		public MANIPULATORTYPEC type;							// The type of this manipulator
		public ManipulatorPropertyC property = new ManipulatorPropertyC(); // The property settings (if type is property)
		public List<ManipulatorPropertyC> properties = new List<ManipulatorPropertyC>(); // The combined properties (if type is combined)
		public LayerMask affects;								// The layers this manipulator will affect
		public PlaygroundTransformC transform = new PlaygroundTransformC(); // The transform of this manipulator (wrapped class for threading)
		public MANIPULATORSHAPEC shape;							// The shape of this manipulator
		public float size;										// The size of this manipulator (if shape is sphere)
		public Bounds bounds;									// The bounds of this manipulator (if shape is box)
		public float strength;									// The strength of this manipulator
		public float strengthSmoothing = 1f;					// The scale of strength smoothing effector
		public float strengthDistanceEffect = 1f;				// The scale of distance strength effector
		public bool enabled = true;								// Is this manipulator enabled?
		public bool inverseBounds = false;						// Should this manipulator be checking for particles inside or outside the shape's bounds?
		public int manipulatorId = 0;							// The id of this manipulator
		public bool willAffect = true;							// Predetermination if a global manipulator will have any effect on current particle system (local will always by default)
		public bool applyLifetimeFilter = false;				// Should lifetime filter determine which particles are affected?
		public float lifetimeFilterMinimum = 0f;				// The minimum lifetime of a particle that is affected by this manipulator
		public float lifetimeFilterMaximum = 0f;				// The maximum lifetime of a particle that is affected by this manipulator

		// Check if manipulator contains position
		public bool Contains (Vector3 position, Vector3 mPosition) {
			if (shape==MANIPULATORSHAPEC.Box) {
				if (!inverseBounds)
					return bounds.Contains(position-mPosition);
				else
					return !bounds.Contains(position-mPosition);
			} else {
				if (!inverseBounds)
					return (Vector3.Distance(position, mPosition)<=size);
				else
					return (Vector3.Distance(position, mPosition)>=size);
			}
		}

		// Check if lifetime filter will apply
		public bool LifetimeFilter (float life) {
			if (!applyLifetimeFilter) return true;
			return (life>=lifetimeFilterMinimum && life<=lifetimeFilterMaximum);
		}

		// Update this Manipulator Object
		public void Update () {
			if (transform.Update() && enabled) {
				manipulatorId = transform.instanceID;
				if (type==MANIPULATORTYPEC.Property || type==MANIPULATORTYPEC.Combined) {
					property.Update();
					property.SetLocalVelocity(transform.rotation, property.velocity);
					for (int i = 0; i<properties.Count; i++) {
						properties[i].Update();
						properties[i].SetLocalVelocity(transform.rotation, properties[i].velocity);
					}
				}
			}
		}

		public void SetLocalTargetsPosition (Transform otherTransform) {
			property.SetLocalTargetsPosition(otherTransform);
			for (int i = 0; i<properties.Count; i++)
				properties[i].SetLocalTargetsPosition (otherTransform);
		}
		
		// Return a copy of this ManipulatorObjectC
		public ManipulatorObjectC Clone () {
			ManipulatorObjectC manipulatorObject = new ManipulatorObjectC();
			manipulatorObject.type = type;
			manipulatorObject.property = property.Clone();
			manipulatorObject.affects = affects;
			manipulatorObject.transform = transform.Clone();
			manipulatorObject.size = size;
			manipulatorObject.shape = shape;
			manipulatorObject.bounds = bounds;
			manipulatorObject.strength = strength;
			manipulatorObject.strengthSmoothing = strengthSmoothing;
			manipulatorObject.strengthDistanceEffect = strengthDistanceEffect;
			manipulatorObject.enabled = enabled;
			manipulatorObject.applyLifetimeFilter = applyLifetimeFilter;
			manipulatorObject.lifetimeFilterMinimum = lifetimeFilterMinimum;
			manipulatorObject.lifetimeFilterMaximum = lifetimeFilterMaximum;
			manipulatorObject.inverseBounds = inverseBounds;
			manipulatorObject.properties = new List<ManipulatorPropertyC>();
			for (int i = 0; i<properties.Count; i++)
				manipulatorObject.properties.Add(properties[i].Clone());
			return manipulatorObject;
		}
	}

	[Serializable]
	public class ManipulatorPropertyC {
		public MANIPULATORPROPERTYTYPEC type;					// The type of this manipulator property
		public MANIPULATORPROPERTYTRANSITIONC transition;		// The transition of this manipulator property
		public Vector3 velocity;								// The velocity of this manipulator property
		public Vector3 localVelocity;							// The local velocity of this manipulator property (set by SetLocalVelocity)
		public Color color = new Color(1,1,1,1);				// The color of this manipulator property
		public Gradient lifetimeColor;							// The lifetime color of this manipulator property
		public float size = 1f;									// The size of this manipulator property
		public List<PlaygroundTransformC> targets = new List<PlaygroundTransformC>(); // The targets to position towards of this manipulator property
		public int targetPointer;								// The pointer of targets (used for calculation)
		public WorldObject meshTarget = new WorldObject();		// The Mesh Target (World Object) to target particles to
		public SkinnedWorldObject skinnedMeshTarget = new SkinnedWorldObject(); // The Skinned Mesh Target (Skinned World Object) to target particles to
		public Matrix4x4 meshTargetMatrix = new Matrix4x4();	// Transform matrix for mesh targets
		public bool useLocalRotation = false;					// Should the manipulator's transform direction be used to apply velocity?
		public bool onlyColorInRange = true;					// Should the particles go back to original color when out of range?
		public bool keepColorAlphas = true;						// Should the particles keep their original color alpha?
		public bool onlyPositionInRange = true;					// Should the particles stop positioning towards target when out of range?
		public float zeroVelocityStrength = 1f;					// The strength to zero velocity on target positioning when using transitions
		public float strength = 1f;								// Individual property strength
		public bool unfolded = true;							// Is unfolded in list
		public TARGETSORTINGC targetSorting;					// Target sorting type
		[NonSerialized] public int[] targetSortingList;			// The sorted list for target positions
		TARGETSORTINGC previousTargetSorting;					// The previous sorting type

		// Update this ManipulatorPropertyC
		public void Update () {
			if (type==MANIPULATORPROPERTYTYPEC.Target) {
				for (int i = 0; i<targets.Count; i++)
					targets[i].Update();
			} else
			if (type==MANIPULATORPROPERTYTYPEC.MeshTarget) {
				if (meshTarget.gameObject!=null) {
					if (meshTarget.cachedId!=meshTarget.gameObject.GetInstanceID())
						meshTarget = PlaygroundParticlesC.NewWorldObject (meshTarget.gameObject.transform);
					if (meshTarget.mesh!=null)
						meshTargetMatrix.SetTRS(meshTarget.transform.position, meshTarget.transform.rotation, meshTarget.transform.localScale);
					if (meshTarget.initialized && (targetSortingList==null || targetSortingList.Length!=meshTarget.vertexPositions.Length || previousTargetSorting!=targetSorting))
						TargetSorting();
				}
			} else
			if (type==MANIPULATORPROPERTYTYPEC.SkinnedMeshTarget) {
				if (skinnedMeshTarget.gameObject!=null) {
					if (skinnedMeshTarget.cachedId!=skinnedMeshTarget.gameObject.GetInstanceID() || !skinnedMeshTarget.initialized) {
						skinnedMeshTarget = PlaygroundParticlesC.NewSkinnedWorldObject (skinnedMeshTarget.gameObject.transform);
					}
					if (skinnedMeshTarget.initialized) {
						skinnedMeshTarget.BoneUpdate();
						PlaygroundC.RunAsync(()=>{
							skinnedMeshTarget.Update();
						});
						if (targetSortingList==null || targetSortingList.Length!=skinnedMeshTarget.vertexPositions.Length || previousTargetSorting!=targetSorting)
							TargetSorting();
					}
				}
			}
		}

		void TargetSorting () {
			if (type==MANIPULATORPROPERTYTYPEC.MeshTarget && !meshTarget.initialized || type==MANIPULATORPROPERTYTYPEC.SkinnedMeshTarget && !skinnedMeshTarget.initialized) return;
			targetPointer = 0;
			targetSortingList = new int[type==MANIPULATORPROPERTYTYPEC.MeshTarget?meshTarget.vertexPositions.Length:skinnedMeshTarget.vertexPositions.Length];
			switch (targetSorting) {
				case TARGETSORTINGC.Scrambled:
				for (int i = 0; i<targetSortingList.Length; i++)
					targetSortingList[i]=i;
					PlaygroundC.ShuffleArray(targetSortingList);
				break;
				case TARGETSORTINGC.Linear:
				for (int i = 0; i<targetSortingList.Length; i++)
					targetSortingList[i]=i;
				break;
				case TARGETSORTINGC.Reversed:
				int x = 0;
				for (int i = targetSortingList.Length-1; i>0; i--) {
					targetSortingList[x]=i;
					x++;
				}
				break;
			}
			previousTargetSorting = targetSorting;
		}

		public void SetLocalVelocity (Quaternion rotation, Vector3 newVelocity) {
			localVelocity = rotation*newVelocity;
		}

		public void SetLocalTargetsPosition (Transform otherTransform) {
			for (int i = 0; i<targets.Count; i++)
				targets[i].SetLocalPosition (otherTransform);
		}

		// Update a target, returns availability
		public bool UpdateTarget (int index) {
			if (targets.Count>0) {
				index=index%targets.Count;
				targets[index].Update();
			}
			return targets[index].available;
		}

		// Return a copy of this ManipulatorPropertyC
		public ManipulatorPropertyC Clone () {
			ManipulatorPropertyC manipulatorProperty = new ManipulatorPropertyC();
			manipulatorProperty.type = type;
			manipulatorProperty.transition = transition;
			manipulatorProperty.velocity = velocity;
			manipulatorProperty.color = color;
			manipulatorProperty.lifetimeColor = lifetimeColor;
			manipulatorProperty.size = size;
			manipulatorProperty.useLocalRotation = useLocalRotation;
			manipulatorProperty.onlyColorInRange = onlyColorInRange;
			manipulatorProperty.keepColorAlphas = keepColorAlphas;
			manipulatorProperty.onlyPositionInRange = onlyPositionInRange;
			manipulatorProperty.zeroVelocityStrength = zeroVelocityStrength;
			manipulatorProperty.strength = strength;
			manipulatorProperty.targetPointer = targetPointer;
			manipulatorProperty.targets = new List<PlaygroundTransformC>();
			for (int i = 0; i<targets.Count; i++)
				manipulatorProperty.targets.Add(targets[i].Clone());
			manipulatorProperty.meshTarget = meshTarget.Clone();
			manipulatorProperty.skinnedMeshTarget = skinnedMeshTarget.Clone();
			return manipulatorProperty;
		}
	}

	[Serializable]
	public class PlaygroundTransformC {
		public Transform transform;
		public int instanceID;
		public bool available;
		public Vector3 position;
		public Vector3 localPosition;
		public Vector3 forward;
		public Vector3 up;
		public Vector3 right;
		public Quaternion rotation;
		public Vector3 localScale;

		// Update this PlaygroundTransformC, returns availability
		public bool Update () {
			if (transform!=null) {
				instanceID = transform.GetInstanceID();
				available = true;
				position = transform.position;
				forward = transform.forward;
				up = transform.up;
				right = transform.right;
				rotation = transform.rotation;
				localScale = transform.localScale;
			} else {
				available = false;
			}
			return available;
		}

		public void SetFromTransform (Transform otherTransform) {
			transform = otherTransform;
			Update();
		}

		public void GetFromTransform (Transform otherTransform) {
			otherTransform.position = position;
			otherTransform.rotation = rotation;
			otherTransform.localScale = localScale;
		}

		// Set local position from another transform (not thread-safe)
		public void SetLocalPosition (Transform otherTransform) {
			if (transform!=null)
				localPosition = otherTransform.InverseTransformPoint(transform.position);
		}

		// Return the instance id of this PlaygroundTransformC
		public int GetInstanceID () {
			return instanceID;
		}

		// Return a copy of this PlaygroundTransformC
		public PlaygroundTransformC Clone () {
			PlaygroundTransformC manipulatorTransform = new PlaygroundTransformC();
			manipulatorTransform.transform = transform;
			manipulatorTransform.available = available;
			manipulatorTransform.position = position;
			manipulatorTransform.forward = forward;
			manipulatorTransform.up = up;
			manipulatorTransform.right = right;
			manipulatorTransform.rotation = rotation;
			return manipulatorTransform;
		}
	}

	[Serializable]
	public class PlaygroundColliderC {
		public bool enabled = true;								// Is this PlaygroundCollider enabled?
		public Transform transform;								// The transform that makes this PlaygroundCollider
		public Plane plane = new Plane();						// The plane of this PlaygroundCollider
		public Vector3 offset;									// The offset in world space to this plane

		// Update this PlaygroundCollider's plane
		public void UpdatePlane () {
			if (!transform) return;
			plane.SetNormalAndPosition(transform.up, transform.position+offset);
		}
		
		public PlaygroundColliderC Clone () {
			PlaygroundColliderC playgroundCollider = new PlaygroundColliderC();
			playgroundCollider.enabled = enabled;
			playgroundCollider.transform = transform;
			playgroundCollider.plane = new Plane(plane.normal, plane.distance);
			playgroundCollider.offset = offset;
			return playgroundCollider;
		}
	}

	[Serializable]
	public class PlaygroundAxisConstraintsC {
		public bool x = false;
		public bool y = false;
		public bool z = false;
	}

	[Serializable]
	public class PlaygroundGradientC {
		public Gradient gradient = new Gradient();

		public void CopyTo (PlaygroundGradientC copy) {
			copy.gradient.SetKeys (gradient.colorKeys, gradient.alphaKeys);
		}
	}
	
	[Serializable]
	public class PlaygroundEventC {
		public bool enabled = true;									// Is this PlaygroundEvent enabled?
		[NonSerialized] public bool initializedTarget = false;		// Has this PlaygroundEvent initialized with its target?
		[NonSerialized] public bool initializedEvent = false;		// Does this PlaygroundEvent have any subscribers to the particleEvent?
		public PlaygroundParticlesC target;							// The target particle system to send events to
		public EVENTBROADCASTC broadcastType;						// The broadcast type of this event (A PlaygroundParticlesC target and/or Event Listeners)
		public EVENTTYPEC eventType;								// The type of event
		public event OnPlaygroundParticle particleEvent;			// The event of a particle (when using Event Listeners)
		public EVENTINHERITANCEC eventInheritancePosition;			// The inheritance method of position
		public EVENTINHERITANCEC eventInheritanceVelocity;			// The inheritance method of velocity
		public EVENTINHERITANCEC eventInheritanceColor;				// The inheritance method of color
		public Vector3 eventPosition;								// The position to send (if inheritance is User)
		public Vector3 eventVelocity;								// The velocity to send (if inheritance is User)
		public Color32 eventColor = Color.white;					// The color to send (if inheritance is User)
		public float eventTime = 1f;								// The time between events (if type is set to Time)
		public float collisionThreshold = 10f;						// The magnitude threshold to trigger collision events
		public float velocityMultiplier = 1f;						// The multiplier of velocity
		float timer = 0f;											// Calculated timer to trigger timed events

		public void Initialize () {
			initializedTarget = (target!=null);
			initializedEvent = (particleEvent!=null);
 		}

		public bool UpdateTime () {
			bool readyToSend = false;
			timer+=PlaygroundC.globalDeltaTime;
			if (timer>=eventTime) {
				readyToSend = true;
				timer = 0f;
			}
			return readyToSend;
		}

		public void SetTimer (float newTime) {
			timer = newTime;
		}

		public void SendParticleEvent (PlaygroundEventParticle eventParticle) {
			particleEvent(eventParticle);
		}

		// Return a copy of this PlaygroundEventC
		public PlaygroundEventC Clone () {
			PlaygroundEventC playgroundEvent = new PlaygroundEventC();
			playgroundEvent.enabled = enabled;
			playgroundEvent.target = target;
			playgroundEvent.eventType = eventType;
			playgroundEvent.eventInheritancePosition = eventInheritancePosition;
			playgroundEvent.eventInheritanceVelocity = eventInheritanceVelocity;
			playgroundEvent.eventInheritanceColor = eventInheritanceColor;
			playgroundEvent.eventPosition = eventPosition;
			playgroundEvent.eventVelocity = eventVelocity;
			playgroundEvent.eventColor = eventColor;
			playgroundEvent.eventTime = eventTime;
			playgroundEvent.collisionThreshold = collisionThreshold;
			playgroundEvent.velocityMultiplier = velocityMultiplier;
			playgroundEvent.particleEvent = particleEvent;
			playgroundEvent.broadcastType = broadcastType;
			return playgroundEvent;
		}
	}

	public enum MANIPULATORTYPEC {
		None,
		Attractor,
		AttractorGravitational,
		Repellent,
		Property,
		Combined,
		Vortex
	}

	public enum MANIPULATORPROPERTYTYPEC {
		None,
		Color,
		Velocity,
		AdditiveVelocity,
		Size,
		Target,
		Death,
		Attractor,
		Gravitational,
		Repellent,
		LifetimeColor,
		Vortex,
		MeshTarget,
		SkinnedMeshTarget
	}

	public enum MANIPULATORPROPERTYTRANSITIONC {
		None,
		Lerp,
		Linear
	}

	public enum MANIPULATORSHAPEC {
		Sphere,
		Box
	}

	public enum PLAYGROUNDORIGINC {
		TopLeft, TopCenter, TopRight,
		MiddleLeft, MiddleCenter, MiddleRight,
		BottomLeft, BottomCenter, BottomRight
	}

	public enum PIXELMODEC {
		Bilinear,
		Pixel32
	}

	public enum COLORSOURCEC {
		Source,
		LifetimeColor,
		LifetimeColors
	}

	public enum OVERFLOWMODEC {
		SourceTransform,
		World,
		SourcePoint
	}

	public enum SOURCESCATTERSPACEC {
		Local=0,
		Global=1
	}

	public enum EMISSIONMODEC {
		Time,
		Scripted
	}

	public enum TRANSITIONTYPEC {
		Linear,
		EaseIn,
		EaseOut,
	}

	public enum INDIVIDUALTRANSITIONTYPEC {
		Inherit,
		Linear,
		EaseIn,
		EaseOut,
	}

	public enum LERPTYPEC {
		PositionColor,
		Position,
		Color,
	}

	public enum MESHMODEC {
		Uv,
		Vertex
	}

	public enum SOURCEC {
		State,
		Transform,
		WorldObject,
		SkinnedWorldObject,
		Script,
		Paint,
		Projection
	}

	public enum SORTINGC {
		Scrambled,
		ScrambledLinear,
		Burst,
		Linear,
		Reversed,
		NearestNeighbor,
		NearestNeighborReversed,
		Custom
	}

	public enum BRUSHDETAILC {
		Perfect,
		High,
		Medium,
		Low
	}

	public enum COLLISIONTYPEC {
		Physics3D,
		Physics2D
	}

	public enum VELOCITYBENDINGTYPEC {
		SourcePosition,
		ParticleDeltaPosition
	}

	public enum TARGETSORTINGC {
		Scrambled,
		Linear,
		Reversed	
	}

	public enum EVENTTYPEC {
		Birth,
		Death,
		Collision,
		Time
	}

	public enum EVENTBROADCASTC {
		Target,
		EventListeners,
		Both
	}

	public enum EVENTINHERITANCEC {
		User,
		Particle,
		Source
	}

	public enum TURBULENCETYPE {
		None,
		Simplex,
		Perlin
	}

	public delegate void OnPlaygroundParticle(PlaygroundEventParticle particle);
}
