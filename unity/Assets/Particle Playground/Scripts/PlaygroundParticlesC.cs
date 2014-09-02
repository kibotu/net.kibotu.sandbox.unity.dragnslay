using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ParticlePlayground {
	[RequireComponent (typeof(ParticleSystem))]
	[ExecuteInEditMode()]
	public class PlaygroundParticlesC : MonoBehaviour {

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// PlaygroundParticlesC variables
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		// Particle Playground settings
		[HideInInspector] public SOURCEC source;											// The particle source
		[HideInInspector] public int activeState;											// Current active state (when using state as source)
		[HideInInspector] public bool emit = true;											// If emission of particles is active on this PlaygroundParticles
		[HideInInspector] public bool loop = true;											// Should a particle re-emit when reaching the end of its lifetime?
		[HideInInspector] public bool disableOnDone = false;								// Should the GameObject of this PlaygroundParticlesC disable when not looping? 
		[HideInInspector] public int updateRate = 1;										// The rate to update this PlaygroundParticles
		[HideInInspector] public bool calculate = true;										// Calculate forces on this PlaygroundParticlesC (can be overrided by PlaygroundC.calculate)
		[HideInInspector] public bool calculateDeltaMovement = true;						// Calculate the delta movement force of this particle system
		[HideInInspector] public float deltaMovementStrength = 10f;							// The strength to multiply delta movement with
		[HideInInspector] public bool worldObjectUpdateVertices = false;					// The current world object will change its vertices over time
		[HideInInspector] public bool worldObjectUpdateNormals = false;						// The current world object will change its normals over time
		[HideInInspector] public int nearestNeighborOrigin = 0;								// The initial source position when using lifetime sorting of Nearest Neighbor / Nearest Neighbor Reversed
		[HideInInspector] public int particleCount;											// The amount of particles within this PlaygroundParticlesC object
		[HideInInspector] public float emissionRate = 1f;									// The percentage to emit of particleCount in bursts from this PlaygroundParticles
		[HideInInspector] public OVERFLOWMODEC overflowMode = OVERFLOWMODEC.SourceTransform;// The method to calculate overflow with
		[HideInInspector] public Vector3 overflowOffset;									// Offset when particle count exceeds source count
		[HideInInspector] public bool applySourceScatter = false;							// Should source position scattering be applied?
		[HideInInspector] public Vector3 sourceScatterMin;									// The minimum spread of source position scattering
		[HideInInspector] public Vector3 sourceScatterMax;									// The maximum spread of source position scattering
		[HideInInspector] public SORTINGC sorting = SORTINGC.Scrambled;						// Sort mode for particle lifetime
		[HideInInspector] public AnimationCurve lifetimeSorting;							// Custom sorting for particle lifetime (when sorting is set to Custom)
		[HideInInspector] public float sizeMin = 1f;										// Minimum size
		[HideInInspector] public float sizeMax = 1f;										// Maximum size
		[HideInInspector] public float scale = 1f;											// The scale of minimum and maximum size
		[HideInInspector] public float initialRotationMin;									// Minimum initial rotation
		[HideInInspector] public float initialRotationMax;									// Maximum initial rotation
		[HideInInspector] public float rotationSpeedMin;									// Minimum amount to rotate
		[HideInInspector] public float rotationSpeedMax;									// Maximum amount to rotate
		[HideInInspector] public bool rotateTowardsDirection = false;						// Should the particles rotate towards their movement direction
		[HideInInspector] public Vector3 rotationNormal = -Vector3.forward;					// The rotation direction normal when rotating towards direction (always normalized value)
		[HideInInspector] public float lifetime;											// The life of a particle in seconds
		[HideInInspector] public float lifetimeOffset;										// The offset in time of this particle system
		[HideInInspector] public bool applyLifetimeSize = true;								// Should lifetime size affect each particle?
		[HideInInspector] public AnimationCurve lifetimeSize;								// The size over lifetime of each particle
		[HideInInspector] public bool onlySourcePositioning = false;						// Should the particles only position on their source (and not apply any forces)?
		[HideInInspector] public bool applyLifetimeVelocity = false;						// Should lifetime velocity affect particles?
		[HideInInspector] public Vector3AnimationCurveC lifetimeVelocity;					// The velocity over lifetime of each particle
		[HideInInspector] public bool applyInitialVelocity = false;							// Should initial velocity affect particles?
		[HideInInspector] public Vector3 initialVelocityMin;								// The minimum starting velocity of each particle
		[HideInInspector] public Vector3 initialVelocityMax;								// The maximum starting velocity of each particle
		[HideInInspector] public bool applyInitialLocalVelocity = false;					// Should initial local velocity affect particles?
		[HideInInspector] public Vector3 initialLocalVelocityMin;							// The minimum starting velocity of each particle with normal or transform direction
		[HideInInspector] public Vector3 initialLocalVelocityMax;							// The maximum starting velocity of each particle with normal or transform direction
		[HideInInspector] public bool applyInitialVelocityShape = false;					// Should the initial velocity shape be applied on particle re/birth?
		[HideInInspector] public Vector3AnimationCurveC initialVelocityShape;				// The amount of velocity to apply of the spawning particle's initial/local velocity in form of a Vector3AnimationCurve
		[HideInInspector] public bool applyVelocityBending;									// Should bending affect particles velocity?
		[HideInInspector] public Vector3 velocityBending;									// The amount to bend velocity of each particle
		[HideInInspector] public VELOCITYBENDINGTYPEC velocityBendingType;					// The type of velocity bending
		[HideInInspector] public Vector3 gravity;											// The constant force towards gravitational vector
		[HideInInspector] public float maxVelocity = 100f;									// The maximum positive- and negative velocity of each particle
		[HideInInspector] public PlaygroundAxisConstraintsC axisConstraints = new PlaygroundAxisConstraintsC(); // The force axis constraints of each particle
		[HideInInspector] public float damping;												// Particles inertia over time
		[HideInInspector] public Gradient lifetimeColor;									// The color over lifetime
		[HideInInspector] public List<PlaygroundGradientC> lifetimeColors = new List<PlaygroundGradientC>(); // The colors over lifetime (if Color Source is set to LifetimeColors)
		[HideInInspector] public COLORSOURCEC colorSource = COLORSOURCEC.Source;			// The source to read color from (fallback on Lifetime Color if no source color is available)
		[HideInInspector] public bool sourceUsesLifetimeAlpha;								// Should the source color use alpha from Lifetime Color instead of the source's original alpha?
		[HideInInspector] public bool applyLocalSpaceMovementCompensation = true;			// Should the movement of the particle system transform when in local simulation space be compensated for?
		[HideInInspector] public bool applyRandomSizeOnRebirth = true;						// Should particles get a new random size upon rebirth?
		[HideInInspector] public bool applyRandomInitialVelocityOnRebirth = true;			// Should particles get a new random velocity upon rebirth?
		[HideInInspector] public bool applyRandomRotationOnRebirth = true;					// Should particles get a new random rotation upon rebirth?
		[HideInInspector] public bool applyRandomScatterOnRebirth = false;					// Should particles get a new scatter position upon rebirth?
		[HideInInspector] public bool applyInitialColorOnRebirth = false;					// Should particles get their initial calculated color upon rebirth? (Can resolve flickering)
		[HideInInspector] public bool pauseCalculationWhenInvisible = false;				// Should the particle system pause calculation upon becoming invisible?
		[HideInInspector] public bool forceVisibilityWhenOutOfFrustrum = true;				// Should the particle system force Play() when GameObject is outside of camera view? (Fix for Shuriken stop rendering)
		[HideInInspector] public bool syncPositionsOnMainThread = false;					// Should each particle's position be synced with main-threaad? (Use this when dealing with moving source objects)
		[HideInInspector] public bool applyLockPosition = false;							// Should the particle system force itself to remain in lockPosition?
		[HideInInspector] public bool applyLockRotation = false;							// Should the particle system force itself to remain in lockRotation?
		[HideInInspector] public bool applyLockScale = false;								// Should the particle system force itself to remain in lockScale?
		[HideInInspector] public bool lockPositionIsLocal = false;							// The locked position is considered local
		[HideInInspector] public bool lockRotationIsLocal = false;							// The locked rotation is considered local
		[HideInInspector] public Vector3 lockPosition = Vector3.zero;						// The locked position
		[HideInInspector] public Vector3 lockRotation = Vector3.zero;						// The locked rotation
		[HideInInspector] public Vector3 lockScale = new Vector3(1f,1f,1f);					// The locked scale
		[HideInInspector] public bool applyMovementCompensationLifetimeStrength = false;	// Should the movementCompensationLifetimeStrength affect local space movement compensation? 
		[HideInInspector] public AnimationCurve movementCompensationLifetimeStrength;		// The strength of movement compensation over particles lifetime
		[HideInInspector] public int particleMask = 0;										// The masked amount of particles
		[HideInInspector] public float particleMaskTime = 0f;								// The time it takes to mask in/out particles
		[HideInInspector] public float stretchSpeed = 1f;									// The speed of stretching to reach full effect
		[HideInInspector] public Vector3 stretchStartDirection = Vector3.zero;				// The starting direction of stretching if all intial velocity is zero
		[HideInInspector] public bool applyLifetimeStretching = false;						// Should lifetime stretching be applied?
		[HideInInspector] public AnimationCurve stretchLifetime;							// The lifetime stretching of stretched particles

		// Source Script variables
		[HideInInspector] public int scriptedEmissionIndex;									// When using Emit() the index will point to the next particle in pool to emit
		[HideInInspector] public Vector3 scriptedEmissionPosition;							// When using Emit() the passed in position will determine the position for this particle
		[HideInInspector] public Vector3 scriptedEmissionVelocity;							// When using Emit() the passed in velocity will determine the speed and direction for this particle
		[HideInInspector] public Color scriptedEmissionColor = Color.white;					// When using Emit() the passed in color will decide the color for this particle if colorSource is set to COLORSOURCEC.Source

		// Collision detection
		[HideInInspector] public bool collision = false;									// Can particles collide?
		[HideInInspector] public bool affectRigidbodies = true;								// Should particles affect rigidbodies?
		[HideInInspector] public float mass = .01f;											// The mass of a particle (calculated in collision with rigidbodies)
		[HideInInspector] public float collisionRadius = 1f;								// The spherical radius of a particle
		[HideInInspector] public LayerMask collisionMask;									// The layers these particles will collide with
		[HideInInspector] public float lifetimeLoss = 0f;									// The amount a particle will loose of its lifetime on collision
		[HideInInspector] public float bounciness = .5f;									// The amount a particle will bounce on collision
		[HideInInspector] public Vector3 bounceRandomMin;									// The minimum amount of random bounciness (seen as negative offset from the collided surface's normal direction)
		[HideInInspector] public Vector3 bounceRandomMax;									// The maximum amount of random bounciness (seen as positive offset from the collided surface's normal direction)
		[HideInInspector] public List<PlaygroundColliderC> colliders;						// The Playground Colliders of this particle system
		[HideInInspector] public COLLISIONTYPEC collisionType;								// The type of collision
		[HideInInspector] public float minCollisionDepth = 0f;								// Minimum depth of Raycast2D
		[HideInInspector] public float maxCollisionDepth = 0f;								// Maximum depth of Raycast2D

		// States (source)
		public List<ParticleStateC> states = new List<ParticleStateC>();					// The states of this PlaygroundParticles
		
		// Scene objects (source)
		[HideInInspector] public WorldObject worldObject = new WorldObject();				// A mesh calculated within the scene
		[HideInInspector] public SkinnedWorldObject skinnedWorldObject = new SkinnedWorldObject(); // A skinned mesh calculated within the scene
		[HideInInspector] public Transform sourceTransform;									// A transform calculated within the scene
		
		// Paint
		[HideInInspector] public PaintObjectC paint;										// The paint source of this PlaygroundParticles
		
		// Projection
		[HideInInspector] public ParticleProjectionC projection;							// The projection source of this PlaygroundParticles
		
		// Manipulators
		public List<ManipulatorObjectC> manipulators;										// List of manipulator objects handled by this PlaygroundParticlesC object

		// Events
		[HideInInspector] public List<PlaygroundEventC> events;								// List of event objects handled by this PlaygroundParticlesC object

		// Cache
		[NonSerialized] public PlaygroundCache playgroundCache = new PlaygroundCache();		// Data for each particle
		[NonSerialized] public ParticleSystem.Particle[] particleCache; 					// Particle pool

		// Snapshots
		[HideInInspector] public List<PlaygroundSave> snapshots = new List<PlaygroundSave>(); // Saved data of properties (positions, velocities, colors etc.)
		[HideInInspector] public bool loadFromStart = false;								// Should the particle system load stored data from start?
		[HideInInspector] public int loadFrom = 0;											// Which data should be loaded (if loadFromStart is true)
		[HideInInspector] public bool loadTransition = false;								// Should a transition occur whenever a Load is issued?
		[HideInInspector] public TRANSITIONTYPEC loadTransitionType;						// The type of transition to occur whenever a Load is issued
		[HideInInspector] public float loadTransitionTime = 1f;								// The time for load transition in seconds
		[HideInInspector] public PlaygroundCache snapshotData;								// The storage of position data if this is a snapshot
		[HideInInspector] public float timeOfSnapshot = 0;									// The global time the snapshot was made
		public bool isSnapshot = false;														// Is this particle system a snapshot?

		// Components
		[HideInInspector] public ParticleSystem shurikenParticleSystem;						// This ParticleSystem (Shuriken) component
		[HideInInspector] public int particleSystemId;										// The id of this PlaygroundParticlesC object
		[HideInInspector] public GameObject particleSystemGameObject;						// This GameObject
		[HideInInspector] public Transform particleSystemTransform;							// This Transform
		[HideInInspector] public Renderer particleSystemRenderer;							// This Renderer
		[HideInInspector] public ParticleSystemRenderer particleSystemRenderer2;			// This ParticleSystemRenderer
		[HideInInspector] public List<PlaygroundParticlesC> eventControlledBy = new List<PlaygroundParticlesC>(); // The PlaygroundParticlesC that is controlling this particle system

		// Turbulence
		SimplexNoise turbulenceSimplex;
		[HideInInspector] public TURBULENCETYPE turbulenceType = TURBULENCETYPE.None;
		[HideInInspector] public float turbulenceStrength = 10f;
		[HideInInspector] public float turbulenceScale = 1f;
		[HideInInspector] public float turbulenceTimeScale = 1f;
		[HideInInspector] public bool turbulenceApplyLifetimeStrength = false;
		[HideInInspector] public AnimationCurve turbulenceLifetimeStrength;
		
		// Internally used variables
		bool inTransition = false;			
		int previousParticleCount = -1;
		float previousEmissionRate = 1f;
		bool cameFromNonCalculatedFrame = false;
		bool cameFromNonEmissionFrame = true;
		bool renderModeStretch = false;
		float previousSizeMin;
		float previousSizeMax;
		float previousInitialRotationMin;
		float previousInitialRotationMax;
		float previousRotationSpeedMin;
		float previousRotationSpeedMax;
		Vector3 previousVelocityMin;
		Vector3 previousVelocityMax;
		Vector3 previousLocalVelocityMin;
		Vector3 previousLocalVelocityMax;
		float previousLifetime;
		bool previousEmission = true;
		float emissionStopped = 0f;
		bool queueEmissionHalt = false;
		int lifetimeColorId = 0;
		System.Random internalRandom01 = new System.Random();
		System.Random internalRandom02 = new System.Random();
		System.Random internalRandom03 = new System.Random();
		float lastTimeUpdated = 0f;
		PlaygroundEventParticle eventParticle = new PlaygroundEventParticle();
		[HideInInspector] public float localDeltaTime = 0f;
		[HideInInspector] public int previousActiveState;
		[HideInInspector] public float simulationStarted;
		[HideInInspector] public bool loopExceeded = false;
		[HideInInspector] public int loopExceededOnParticle;

		[HideInInspector] float particleTimescale = 1f;

		// Clone settings by passing in a reference
		public void CopyTo (PlaygroundParticlesC playgroundParticles) {
			
			// Playground variables
			playgroundParticles.source 										= source;
			playgroundParticles.activeState 								= activeState;
			playgroundParticles.emit										= emit;
			playgroundParticles.loop										= loop;
			playgroundParticles.disableOnDone								= disableOnDone;
			playgroundParticles.updateRate 									= updateRate;
			playgroundParticles.calculate 									= calculate;					
			playgroundParticles.calculateDeltaMovement						= calculateDeltaMovement;
			playgroundParticles.deltaMovementStrength 						= deltaMovementStrength;
			playgroundParticles.worldObjectUpdateVertices					= worldObjectUpdateVertices;
			playgroundParticles.worldObjectUpdateNormals 					= worldObjectUpdateNormals;
			playgroundParticles.nearestNeighborOrigin 						= nearestNeighborOrigin;							
			playgroundParticles.particleCount 								= particleCount;									
			playgroundParticles.emissionRate 								= emissionRate;									
			playgroundParticles.overflowMode 								= overflowMode;	
			playgroundParticles.overflowOffset 								= overflowOffset;									
			playgroundParticles.applySourceScatter							= applySourceScatter;
			playgroundParticles.sourceScatterMin							= sourceScatterMin;
			playgroundParticles.sourceScatterMax							= sourceScatterMax;
			playgroundParticles.sorting 									= sorting;
			playgroundParticles.lifetimeSorting								= lifetimeSorting;					
			playgroundParticles.sizeMin 									= sizeMin;										
			playgroundParticles.sizeMax 									= sizeMax;
			playgroundParticles.scale										= scale;
			playgroundParticles.initialRotationMin 							= initialRotationMin;									
			playgroundParticles.initialRotationMax 							= initialRotationMax;								
			playgroundParticles.rotationSpeedMin 							= rotationSpeedMin;								
			playgroundParticles.rotationSpeedMax 							= rotationSpeedMax;									
			playgroundParticles.rotateTowardsDirection 						= rotateTowardsDirection; 				
			playgroundParticles.rotationNormal 								= rotationNormal;
			playgroundParticles.lifetime 									= lifetime;											
			playgroundParticles.lifetimeOffset 								= lifetimeOffset;
			playgroundParticles.applyLifetimeSize							= applyLifetimeSize;
			playgroundParticles.lifetimeSize 								= lifetimeSize;							
			playgroundParticles.onlySourcePositioning 						= onlySourcePositioning;					
			playgroundParticles.applyLifetimeVelocity 						= applyLifetimeVelocity;				
			playgroundParticles.lifetimeVelocity 							= lifetimeVelocity;					
			playgroundParticles.applyInitialVelocity 						= applyInitialVelocity;						
			playgroundParticles.initialVelocityMin 							= initialVelocityMin;								
			playgroundParticles.initialVelocityMax 							= initialVelocityMax;								
			playgroundParticles.applyInitialLocalVelocity 					= applyInitialLocalVelocity;		
			playgroundParticles.initialLocalVelocityMin 					= initialLocalVelocityMin;							
			playgroundParticles.initialLocalVelocityMax 					= initialLocalVelocityMax;							
			playgroundParticles.applyVelocityBending 						= applyVelocityBending;								
			playgroundParticles.velocityBending 							= velocityBending;
			playgroundParticles.velocityBendingType							= velocityBendingType;
			playgroundParticles.applyInitialVelocityShape					= applyInitialVelocityShape;
			playgroundParticles.initialVelocityShape						= initialVelocityShape;
			playgroundParticles.gravity 									= gravity;											
			playgroundParticles.damping 									= damping;
			playgroundParticles.maxVelocity									= maxVelocity;								
			playgroundParticles.lifetimeColor.SetKeys (lifetimeColor.colorKeys, lifetimeColor.alphaKeys);				
			playgroundParticles.colorSource 								= colorSource;				
			playgroundParticles.sourceUsesLifetimeAlpha 					= sourceUsesLifetimeAlpha;
			playgroundParticles.applyLocalSpaceMovementCompensation			= applyLocalSpaceMovementCompensation;
			playgroundParticles.applyRandomSizeOnRebirth					= applyRandomSizeOnRebirth;
			playgroundParticles.applyRandomInitialVelocityOnRebirth			= applyRandomInitialVelocityOnRebirth;
			playgroundParticles.applyRandomRotationOnRebirth				= applyRandomRotationOnRebirth;
			playgroundParticles.applyRandomScatterOnRebirth					= applyRandomScatterOnRebirth;
			playgroundParticles.applyInitialColorOnRebirth					= applyInitialColorOnRebirth;
			playgroundParticles.pauseCalculationWhenInvisible				= pauseCalculationWhenInvisible;
			playgroundParticles.forceVisibilityWhenOutOfFrustrum			= forceVisibilityWhenOutOfFrustrum;
			playgroundParticles.syncPositionsOnMainThread					= syncPositionsOnMainThread;
			playgroundParticles.applyLockPosition							= applyLockPosition;
			playgroundParticles.applyLockRotation							= applyLockRotation;
			playgroundParticles.applyLockScale								= applyLockScale;
			playgroundParticles.lockPositionIsLocal							= lockPositionIsLocal;
			playgroundParticles.lockRotationIsLocal							= lockRotationIsLocal;
			playgroundParticles.lockPosition								= lockPosition;
			playgroundParticles.lockRotation								= lockRotation;
			playgroundParticles.lockScale									= lockScale;
			playgroundParticles.applyMovementCompensationLifetimeStrength	= applyMovementCompensationLifetimeStrength;
			playgroundParticles.movementCompensationLifetimeStrength		= movementCompensationLifetimeStrength;
			playgroundParticles.particleMask								= particleMask;
			playgroundParticles.particleMaskTime							= particleMaskTime;
			playgroundParticles.stretchSpeed								= stretchSpeed;
			playgroundParticles.applyLifetimeStretching						= applyLifetimeStretching;
			playgroundParticles.stretchLifetime								= stretchLifetime;

			// Scripted source variables
			playgroundParticles.scriptedEmissionIndex						= scriptedEmissionIndex;
			playgroundParticles.scriptedEmissionPosition					= scriptedEmissionPosition;
			playgroundParticles.scriptedEmissionVelocity					= scriptedEmissionVelocity;
			playgroundParticles.scriptedEmissionColor						= scriptedEmissionColor;

			// Collision detection
			playgroundParticles.collision 									= collision;
			playgroundParticles.affectRigidbodies 							= affectRigidbodies;
			playgroundParticles.mass 										= mass; 
			playgroundParticles.collisionRadius 							= collisionRadius;
			playgroundParticles.collisionMask 								= collisionMask;					
			playgroundParticles.bounciness 									= bounciness;
			playgroundParticles.lifetimeLoss 								= lifetimeLoss;
			playgroundParticles.bounceRandomMin								= bounceRandomMin;
			playgroundParticles.bounceRandomMax								= bounceRandomMax;
			playgroundParticles.collisionType								= collisionType;
			playgroundParticles.minCollisionDepth							= minCollisionDepth;
			playgroundParticles.maxCollisionDepth							= maxCollisionDepth;
			playgroundParticles.colliders									= new List<PlaygroundColliderC>();
			int i;
			for (i = 0; i<colliders.Count; i++)
				playgroundParticles.colliders.Add(colliders[i].Clone());
			
			// States (source)
			playgroundParticles.states 										= new List<ParticleStateC>();
			for (i = 0; i<states.Count; i++) {
				playgroundParticles.states.Add(states[i].Clone());
			}
			// Scene objects (source)
			playgroundParticles.worldObject 								= worldObject.Clone();
			playgroundParticles.skinnedWorldObject 							= skinnedWorldObject.Clone();
			
			playgroundParticles.sourceTransform 							= sourceTransform;
			
			// Paint
			playgroundParticles.paint 										= paint.Clone();

			// Projection
			playgroundParticles.projection 									= projection.Clone();

			// Manipulators
			playgroundParticles.manipulators								= new List<ManipulatorObjectC>();
			for (i = 0; i<manipulators.Count; i++)
				playgroundParticles.manipulators.Add(manipulators[i].Clone());

			// Events
			playgroundParticles.events										= new List<PlaygroundEventC>();
			for (i = 0; i<events.Count; i++)
				playgroundParticles.events.Add(events[i].Clone());

			// Lifetime Colors
			playgroundParticles.lifetimeColors								= new List<PlaygroundGradientC>();
			for (i = 0; i<lifetimeColors.Count; i++) {
				playgroundParticles.lifetimeColors.Add(new PlaygroundGradientC());
				lifetimeColors[i].CopyTo(playgroundParticles.lifetimeColors[i]);
			}

			// Turbulence
			playgroundParticles.turbulenceType								= turbulenceType;
			playgroundParticles.turbulenceApplyLifetimeStrength				= turbulenceApplyLifetimeStrength;
			playgroundParticles.turbulenceLifetimeStrength					= turbulenceLifetimeStrength;
			playgroundParticles.turbulenceScale								= turbulenceScale;
			playgroundParticles.turbulenceStrength							= turbulenceStrength;
			playgroundParticles.turbulenceTimeScale							= turbulenceTimeScale;

			// Other
			playgroundParticles.particleTimescale							= particleTimescale;
		}

		// Copy stored data into a particle system (separated to solve Save/Load paradox)
		public void CopySaveDataTo (PlaygroundParticlesC playgroundParticles) {
			playgroundParticles.snapshots = new List<PlaygroundSave>();
			for (int i = 0; i<snapshots.Count; i++)
				playgroundParticles.snapshots.Add(snapshots[i].Clone());
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// PlaygroundParticlesC functions
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		// Emit a single particle at position with velocity and color (Source Mode SOURCEC.Script required)
		public int Emit (Vector3 givePosition, Vector3 giveVelocity, Color32 giveColor) {
			source = SOURCEC.Script;
			int returnIndex = scriptedEmissionIndex;
			EmitProcedure(givePosition, giveVelocity, giveColor);
			particleSystemGameObject.SetActive(true);
			return returnIndex;
		}
		
		// Set emission on/off
		public void Emit (bool setEmission) {
			emit = setEmission;
			if (emit) {
				simulationStarted = PlaygroundC.globalTime;
				loopExceeded = false;
				loopExceededOnParticle = -1;
				particleSystemGameObject.SetActive(true);
				Emission(this, true, true);
			} else {
				emissionStopped = PlaygroundC.globalTime;
			}
			previousEmission = setEmission;
		}

		// Emit number of particles set by quantity, position and minimum - maximum random velocity
		public void Emit (int quantity, Vector3 givePosition, Vector3 randomVelocityMin, Vector3 randomVelocityMax, Color32 giveColor) {
			source = SOURCEC.Script;
			for (int i = 0; i<quantity; i++)
				EmitProcedure(
					givePosition,
					applyInitialVelocityShape?
					Vector3.Scale (RandomRange(internalRandom01, randomVelocityMin, randomVelocityMax), initialVelocityShape.Evaluate((i*1f)/(quantity*1f)))
					:
					RandomRange(internalRandom01, randomVelocityMin, randomVelocityMax),
					giveColor
				);
			particleSystemGameObject.SetActive(true);
		}

		// Emit number of particles set by quantity, minimum - maximum random position and velocity
		public void Emit (int quantity, Vector3 randomPositionMin, Vector3 randomPositionMax, Vector3 randomVelocityMin, Vector3 randomVelocityMax, Color32 giveColor) {
			source = SOURCEC.Script;
			for (int i = 0; i<quantity; i++)
				EmitProcedure(
					RandomRange(internalRandom01, randomPositionMin, randomPositionMax),
					applyInitialVelocityShape?
						Vector3.Scale (RandomRange(internalRandom01, randomVelocityMin, randomVelocityMax), initialVelocityShape.Evaluate((i*1f)/(quantity*1f)))
					:
						RandomRange(internalRandom01, randomVelocityMin, randomVelocityMax),
					giveColor
				);
			particleSystemGameObject.SetActive(true);
		}

		// Thread-safe version of Emit()
		public void ThreadSafeEmit (Vector3 givePosition, Vector3 giveVelocity, Color32 giveColor) {
			EmitProcedure(givePosition, giveVelocity, giveColor);
		}
		public void ThreadSafeEmit (int quantity, Vector3 givePosition, Vector3 randomVelocityMin, Vector3 randomVelocityMax, Color32 giveColor) {
			for (int i = 0; i<quantity; i++)
				EmitProcedure(
					givePosition,
					applyInitialVelocityShape?
					Vector3.Scale (RandomRange(internalRandom01, randomVelocityMin, randomVelocityMax), initialVelocityShape.Evaluate((i*1f)/(quantity*1f)))
					:
					RandomRange(internalRandom01, randomVelocityMin, randomVelocityMax),
					giveColor
				);
		}

		// Internal emission procedure called upon Emit()
		void EmitProcedure (Vector3 givePosition, Vector3 giveVelocity, Color32 giveColor) {
			scriptedEmissionIndex=Mathf.Clamp(scriptedEmissionIndex, 0, scriptedEmissionIndex%particleCount);

			scriptedEmissionPosition = givePosition;
			scriptedEmissionVelocity = giveVelocity;
			scriptedEmissionColor = giveColor;

			Rebirth(this, scriptedEmissionIndex, internalRandom01);

			if (playgroundCache.lifetimeOffset.Length!=particleCount) return;
			playgroundCache.lifetimeOffset[scriptedEmissionIndex] = 0;
			playgroundCache.life[scriptedEmissionIndex] = 0;
			playgroundCache.birth[scriptedEmissionIndex] = PlaygroundC.globalTime;
			playgroundCache.death[scriptedEmissionIndex] = playgroundCache.birth[scriptedEmissionIndex]+lifetime;
			playgroundCache.emission[scriptedEmissionIndex] = true;
			playgroundCache.rebirth[scriptedEmissionIndex] = true;
			playgroundCache.scriptedColor[scriptedEmissionIndex] = new Color(giveColor.r, giveColor.g, giveColor.b, giveColor.a);

			emit = true;
			simulationStarted = PlaygroundC.globalTime;
			loopExceeded = false;
			loopExceededOnParticle = -1;
			scriptedEmissionIndex++;scriptedEmissionIndex=scriptedEmissionIndex%particleCount;
		}
		
		// Is this particle system still alive?
		public bool IsAlive () {
			return !loopExceeded;
		}

		// Is this particle system in a transition?
		public bool InTransition () {
			return inTransition;
		}

		// Kill a particle
		public void Kill (int p) {
			playgroundCache.changedByPropertyDeath[p] = true;
			playgroundCache.life[p] = lifetime;
		}

		// Create a new PlaygroundParticlesC object
		public static PlaygroundParticlesC CreatePlaygroundParticles (Texture2D[] images, string name, Vector3 position, Quaternion rotation, Vector3 offset, float particleSize, float scale, Material material) {
			PlaygroundParticlesC playgroundParticles = CreateParticleObject(name,position,rotation,particleSize,material);
			
			int[] quantityList = new int[images.Length];
			int i = 0;
			for (; i<images.Length; i++)
				quantityList[i] = images[i].width*images[i].height;
			playgroundParticles.particleCache = new ParticleSystem.Particle[quantityList[PlaygroundC.Largest(quantityList)]];
			OnCreatePlaygroundParticles(playgroundParticles);	
			
			for (i = 0; i<images.Length; i++) {
				playgroundParticles.states.Add(new ParticleStateC());
				playgroundParticles.states[playgroundParticles.states.Count-1].ConstructParticles(images[i],scale,offset,"State 0",null);
			}
			
			return playgroundParticles;
		}
		
		// Set default settings for PlaygroundParticlesC object
		public static void OnCreatePlaygroundParticles (PlaygroundParticlesC playgroundParticles) {
			playgroundParticles.playgroundCache = new PlaygroundCache();
			playgroundParticles.paint = new PaintObjectC();
			playgroundParticles.states = new List<ParticleStateC>();
			playgroundParticles.projection = new ParticleProjectionC();
			playgroundParticles.colliders = new List<PlaygroundColliderC>();
			playgroundParticles.particleSystemId = PlaygroundC.particlesQuantity;
			playgroundParticles.projection.projectionTransform = playgroundParticles.particleSystemTransform;
			
			playgroundParticles.playgroundCache.initialSize = new float[playgroundParticles.particleCount];
			playgroundParticles.playgroundCache.initialSize = RandomFloat(playgroundParticles.playgroundCache.initialSize.Length, playgroundParticles.sizeMin, playgroundParticles.sizeMax);
			
			playgroundParticles.previousParticleCount = playgroundParticles.particleCount;
			playgroundParticles.lifetimeSize = new AnimationCurve(new Keyframe(0,1), new Keyframe(1,1));
			
			playgroundParticles.shurikenParticleSystem.Emit(playgroundParticles.particleCount);
			playgroundParticles.shurikenParticleSystem.GetParticles(playgroundParticles.particleCache);
			for (int p = 0; p<playgroundParticles.particleCache.Length; p++) {
				playgroundParticles.playgroundCache.size[p] = playgroundParticles.playgroundCache.initialSize[p];
			}
			
			PlaygroundParticlesC.SetParticleCount(playgroundParticles, playgroundParticles.particleCount);
			
			if (PlaygroundC.reference!=null) {
				PlaygroundC.particlesQuantity++;
				PlaygroundC.reference.particleSystems.Add(playgroundParticles);
				playgroundParticles.particleSystemId = PlaygroundC.particlesQuantity;
			}
		}
		
		// Create a Shuriken Particle System
		public static PlaygroundParticlesC CreateParticleObject (string name, Vector3 position, Quaternion rotation, float particleSize, Material material) {
			GameObject go = PlaygroundC.ResourceInstantiate("Particle Playground System");
			PlaygroundParticlesC playgroundParticles = go.GetComponent<PlaygroundParticlesC>();
			playgroundParticles.particleSystemGameObject = go;
			playgroundParticles.particleSystemGameObject.name = name;
			playgroundParticles.shurikenParticleSystem = playgroundParticles.particleSystemGameObject.GetComponent<ParticleSystem>();
			playgroundParticles.particleSystemRenderer = playgroundParticles.shurikenParticleSystem.renderer;
			playgroundParticles.particleSystemRenderer2 = playgroundParticles.shurikenParticleSystem.renderer as ParticleSystemRenderer;
			playgroundParticles.particleSystemTransform = playgroundParticles.particleSystemGameObject.transform;
			playgroundParticles.sourceTransform = playgroundParticles.particleSystemTransform;
			playgroundParticles.source = SOURCEC.Transform;
			playgroundParticles.particleSystemTransform.position = position;
			playgroundParticles.particleSystemTransform.rotation = rotation;

			if (PlaygroundC.reference.autoGroup && playgroundParticles.particleSystemTransform.parent==null)
				playgroundParticles.particleSystemTransform.parent = PlaygroundC.referenceTransform;
			
			if (playgroundParticles.particleSystemRenderer.sharedMaterial==null)
				playgroundParticles.particleSystemRenderer.sharedMaterial = material;

			return playgroundParticles;
		}
		
		// Create a new WorldObject
		public static WorldObject NewWorldObject (Transform meshTransform) {
			WorldObject worldObject = new WorldObject();
			if (meshTransform.GetComponentInChildren<MeshFilter>()) {
				worldObject.transform = meshTransform;
				worldObject.Initialize ();
			} else Debug.Log("Could not find a mesh in "+meshTransform.name+".");
			return worldObject;
		}
		
		// Create a new SkinnedWorldObject
		public static SkinnedWorldObject NewSkinnedWorldObject (Transform meshTransform) {
			SkinnedWorldObject skinnedWorldObject = new SkinnedWorldObject();
			if (meshTransform.GetComponentInChildren<SkinnedMeshRenderer>()) {
				skinnedWorldObject.transform = meshTransform;
				skinnedWorldObject.Initialize ();
			} else Debug.Log("Could not find a skinned mesh in "+meshTransform.name+".");
			return skinnedWorldObject;
		}

		// Create a new SkinnedWorldObject with pre-set down resolution
		public static SkinnedWorldObject NewSkinnedWorldObject (Transform meshTransform, int downResolution) {
			SkinnedWorldObject skinnedWorldObject = NewSkinnedWorldObject(meshTransform);
			skinnedWorldObject.downResolution = downResolution;
			return skinnedWorldObject;
		}
		
		// Create a new PaintObject
		public static PaintObjectC NewPaintObject (PlaygroundParticlesC playgroundParticles) {
			PaintObjectC paintObject = new PaintObjectC();
			playgroundParticles.paint = paintObject;
			playgroundParticles.paint.Initialize();
			return paintObject;
		}
		
		// Create a new ParticleProjection object
		public static ParticleProjectionC NewProjectionObject (PlaygroundParticlesC playgroundParticles) {
			ParticleProjectionC projectionObject = new ParticleProjectionC();
			playgroundParticles.projection = projectionObject;
			playgroundParticles.projection.Initialize();
			return projectionObject;
		}
		
		// Create a new ManipulatorObject and attach to the Playground Manager
		public static ManipulatorObjectC NewManipulatorObject (MANIPULATORTYPEC type, LayerMask affects, Transform manipulatorTransform, float size, float strength, PlaygroundParticlesC playgroundParticles) {
			ManipulatorObjectC manipulatorObject = new ManipulatorObjectC();
			manipulatorObject.type = type;
			manipulatorObject.affects = affects;
			manipulatorObject.transform.transform = manipulatorTransform;
			manipulatorObject.size = size;
			manipulatorObject.strength = strength;
			manipulatorObject.bounds = new Bounds(Vector3.zero, new Vector3(size, size, size));
			manipulatorObject.property = new ManipulatorPropertyC();
			manipulatorObject.Update();

			// Add this to Playground Manager or the passed in playgroundParticles
			if (playgroundParticles==null)
				PlaygroundC.reference.manipulators.Add(manipulatorObject);
			else
				playgroundParticles.manipulators.Add(manipulatorObject);
			
			return manipulatorObject;
		}

		// Lerp to specified state in this PlaygroundParticles
		public static void Lerp (PlaygroundParticlesC playgroundParticles, int to, float time, LERPTYPEC lerpType) {
			if (to<0) {to=playgroundParticles.states.Count;} to=to%playgroundParticles.states.Count;
			if(time<0) time = 0f;
			Color color = new Color();
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				if (lerpType==LERPTYPEC.PositionColor||lerpType==LERPTYPEC.Position) 
					playgroundParticles.particleCache[i].position = Vector3.Lerp(playgroundParticles.particleCache[i].position, playgroundParticles.states[to].GetPosition(i%playgroundParticles.states[to].positionLength), time);
				if (lerpType==LERPTYPEC.PositionColor||lerpType==LERPTYPEC.Color) {
					color = playgroundParticles.states[to].GetColor(i%playgroundParticles.states[to].colorLength);
					playgroundParticles.particleCache[i].color = Color.Lerp(playgroundParticles.particleCache[i].color, color, time);
				}
			}
			Update(playgroundParticles);
		}
		
		// Lerp to state object
		public static void Lerp (PlaygroundParticlesC playgroundParticles, ParticleStateC state, float time, LERPTYPEC lerpType) {
			if(time<0) time = 0f;
			Color color = new Color();
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				if (lerpType==LERPTYPEC.PositionColor||lerpType==LERPTYPEC.Position) 
					playgroundParticles.particleCache[i].position = Vector3.Lerp(playgroundParticles.particleCache[i].position, state.GetPosition(i%state.positionLength), time);
				if (lerpType==LERPTYPEC.PositionColor||lerpType==LERPTYPEC.Color) {
					color = state.GetColor(i%state.colorLength);
					playgroundParticles.particleCache[i].color = Color.Lerp(playgroundParticles.particleCache[i].color, color, time);
				}
			}
		}
		
		// Lerp to a Skinned World Object
		public static void Lerp (PlaygroundParticlesC playgroundParticles, SkinnedWorldObject particleStateWorldObject, float time) {
			if(time<0) time = 0f;
			Vector3[] vertices = particleStateWorldObject.mesh.vertices;
			BoneWeight[] weights = particleStateWorldObject.mesh.boneWeights;
			Matrix4x4[] boneMatrices = new Matrix4x4[particleStateWorldObject.renderer.bones.Length];

			int i;
			for (i = 0; i<boneMatrices.Length; i++)
				boneMatrices[i] = particleStateWorldObject.renderer.bones[i].localToWorldMatrix * particleStateWorldObject.mesh.bindposes[i];
			
			Matrix4x4 vertexMatrix = new Matrix4x4();
			for (i = 0; i<playgroundParticles.particleCache.Length; i++) {
				BoneWeight weight = weights[i];
				Matrix4x4 m0 = boneMatrices[weight.boneIndex0];
				Matrix4x4 m1 = boneMatrices[weight.boneIndex1];
				Matrix4x4 m2 = boneMatrices[weight.boneIndex2];
				Matrix4x4 m3 = boneMatrices[weight.boneIndex3];
				
				for(int n=0;n<16;n++){
					vertexMatrix[n] =
						m0[n] * weight.weight0 +
							m1[n] * weight.weight1 +
							m2[n] * weight.weight2 +
							m3[n] * weight.weight3;
				}
				
				playgroundParticles.particleCache[i].position = Vector3.Lerp(playgroundParticles.particleCache[i].position, vertexMatrix.MultiplyPoint3x4(vertices[i%vertices.Length]), time);
			}
		}

		// Set color from PixelParticle object
		public static void SetColor (PlaygroundParticlesC playgroundParticles, int to) {
			Color color = new Color();
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				color = playgroundParticles.states[to].GetColor(i%playgroundParticles.states[to].colorLength);
				playgroundParticles.particleCache[i].color = color;
			}
		}
		
		// Set color from Color
		public static void SetColor (PlaygroundParticlesC playgroundParticles, Color color) {
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				playgroundParticles.particleCache[i].color = color;
			}
		}

		// Get vertices from a skinned world object in a Vector3-array
		public static void GetPosition (SkinnedWorldObject particleStateWorldObject, bool updateNormals) {
			if (updateNormals)
				particleStateWorldObject.normals = particleStateWorldObject.mesh.normals;
			Vector3[] vertices = particleStateWorldObject.mesh.vertices;
			BoneWeight[] weights = particleStateWorldObject.mesh.boneWeights;
			Matrix4x4[] bindPoses = particleStateWorldObject.mesh.bindposes;
			Matrix4x4[] boneMatrices = new Matrix4x4[particleStateWorldObject.renderer.bones.Length];

			int i = 0;
			for (; i<boneMatrices.Length; i++) {
				boneMatrices[i] = particleStateWorldObject.renderer.bones[i].localToWorldMatrix * bindPoses[i];
			}

			PlaygroundC.RunAsync(()=>{
				Matrix4x4 vertexMatrix = new Matrix4x4();
				for (i = 0; i<vertices.Length; i++) {
					BoneWeight weight = weights[i];
					Matrix4x4 m0 = boneMatrices[weight.boneIndex0];
					Matrix4x4 m1 = boneMatrices[weight.boneIndex1];
					Matrix4x4 m2 = boneMatrices[weight.boneIndex2];
					Matrix4x4 m3 = boneMatrices[weight.boneIndex3];
					
					for (int n=0;n<16;n++) {
						vertexMatrix[n] =
							m0[n] * weight.weight0 +
							m1[n] * weight.weight1 +
							m2[n] * weight.weight2 +
							m3[n] * weight.weight3;
					}
					vertices[i] = vertexMatrix.MultiplyPoint3x4(vertices[i]);
				}
				particleStateWorldObject.vertexPositions = vertices;
			});
		}
		
		// Get position from Mesh World Object
		public static void GetPosition (Vector3[] v3, WorldObject particleStateWorldObject) {
			if (particleStateWorldObject.meshFilter.sharedMesh!=particleStateWorldObject.mesh)
				particleStateWorldObject.mesh = particleStateWorldObject.meshFilter.sharedMesh;
			v3 = particleStateWorldObject.mesh.vertices;
		}

		// Get procedural position from Mesh World Object
		public static void GetProceduralPosition (Vector3[] v3, WorldObject particleStateWorldObject) {
			if (particleStateWorldObject.meshFilter.sharedMesh!=particleStateWorldObject.mesh)
				particleStateWorldObject.mesh = particleStateWorldObject.meshFilter.sharedMesh;
			Vector3[] vertices = particleStateWorldObject.mesh.vertices;
			if (v3.Length!=vertices.Length) v3 = new Vector3[vertices.Length];
			for (int i = 0; i<v3.Length; i++) {
				v3[i] = particleStateWorldObject.transform.TransformPoint(vertices[i%vertices.Length]);
			}
		}
		
		// Get normals from Mesh World Object
		public static void GetNormals (Vector3[] v3, WorldObject particleStateWorldObject) {
			v3 = particleStateWorldObject.mesh.normals;
		}
		
		// Set size for particles
		public static void SetSize (PlaygroundParticlesC playgroundParticles, float size) {
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				playgroundParticles.playgroundCache.initialSize[i] = size;
				playgroundParticles.particleCache[i].size = size;
			}
		}
		
		// Set random size for particles within sizeMinimum- and sizeMaximum range 
		public static void SetSizeRandom (PlaygroundParticlesC playgroundParticles, float sizeMinimum, float sizeMaximum) {
			playgroundParticles.playgroundCache.initialSize = RandomFloat(playgroundParticles.particleCache.Length, sizeMinimum, sizeMaximum);
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				playgroundParticles.particleCache[i].size = playgroundParticles.playgroundCache.initialSize[i];
			}
			playgroundParticles.sizeMin = sizeMinimum;
			playgroundParticles.sizeMax = sizeMaximum;
			playgroundParticles.previousSizeMin = playgroundParticles.sizeMin;
			playgroundParticles.previousSizeMax = playgroundParticles.sizeMax;
		}
		
		// Set random rotation for particles within rotationMinimum- and rotationMaximum range
		public static void SetRotationRandom (PlaygroundParticlesC playgroundParticles, float rotationMinimum, float rotationMaximum) {
			playgroundParticles.playgroundCache.rotationSpeed = RandomFloat(playgroundParticles.particleCache.Length, rotationMinimum, rotationMaximum);
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				playgroundParticles.playgroundCache.rotation[i] = playgroundParticles.playgroundCache.initialRotation[i];
			}
			playgroundParticles.rotationSpeedMin = rotationMinimum;
			playgroundParticles.rotationSpeedMax = rotationMaximum;
			playgroundParticles.previousRotationSpeedMin = playgroundParticles.rotationSpeedMin;
			playgroundParticles.previousRotationSpeedMax = playgroundParticles.rotationSpeedMax;
		}
		
		// Set random initial rotation for particles within rotationMinimum- and rotationMaximum range
		public static void SetInitialRotationRandom (PlaygroundParticlesC playgroundParticles, float rotationMinimum, float rotationMaximum) {
			playgroundParticles.playgroundCache.initialRotation = RandomFloat(playgroundParticles.particleCache.Length, rotationMinimum, rotationMaximum);
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				playgroundParticles.playgroundCache.rotation[i] = playgroundParticles.playgroundCache.initialRotation[i];
			}
			playgroundParticles.initialRotationMin = rotationMinimum;
			playgroundParticles.initialRotationMax = rotationMaximum;
			playgroundParticles.previousInitialRotationMin = playgroundParticles.initialRotationMin;
			playgroundParticles.previousInitialRotationMax = playgroundParticles.initialRotationMax;
		}
		
		// Set initial random velocity for particles within velocityMinimum- and velocityMaximum range
		public static void SetVelocityRandom (PlaygroundParticlesC playgroundParticles, Vector3 velocityMinimum, Vector3 velocityMaximum) {
			System.Random random = new System.Random();
			playgroundParticles.playgroundCache.initialVelocity = new Vector3[playgroundParticles.particleCount];
			for (int i = 0; i<playgroundParticles.particleCount; i++) {
				playgroundParticles.playgroundCache.initialVelocity[i] = new Vector3(
					RandomRange(random, velocityMinimum.x, velocityMaximum.x),
					RandomRange(random, velocityMinimum.y, velocityMaximum.y),
					RandomRange(random, velocityMinimum.z, velocityMaximum.z)
					);
			}
			
			playgroundParticles.initialVelocityMin = velocityMinimum;
			playgroundParticles.initialVelocityMax = velocityMaximum;
			playgroundParticles.previousVelocityMin = playgroundParticles.initialVelocityMin;
			playgroundParticles.previousVelocityMax = playgroundParticles.initialVelocityMax;
		}
		
		// Set initial random local velocity for particles within velocityMinimum- and velocityMaximum range
		public static void SetLocalVelocityRandom (PlaygroundParticlesC playgroundParticles, Vector3 velocityMinimum, Vector3 velocityMaximum) {
			System.Random random = new System.Random();
			playgroundParticles.playgroundCache.initialLocalVelocity = new Vector3[playgroundParticles.particleCount];
			for (int i = 0; i<playgroundParticles.particleCount; i++) {
				playgroundParticles.playgroundCache.initialLocalVelocity[i] = new Vector3(
					RandomRange(random, velocityMinimum.x, velocityMaximum.x),
					RandomRange(random, velocityMinimum.y, velocityMaximum.y),
					RandomRange(random, velocityMinimum.z, velocityMaximum.z)
				);
			}

			playgroundParticles.initialLocalVelocityMin = velocityMinimum;
			playgroundParticles.initialLocalVelocityMax = velocityMaximum;
			playgroundParticles.previousLocalVelocityMin = playgroundParticles.initialLocalVelocityMin;
			playgroundParticles.previousLocalVelocityMax = playgroundParticles.initialLocalVelocityMax;
		}

		// Set material for particle system
		public static void SetMaterial (PlaygroundParticlesC playgroundParticles, Material particleMaterial) {
			playgroundParticles.particleSystemRenderer.sharedMaterial = particleMaterial;
		}
		
		// Set alphas for particles
		public static void SetAlpha (PlaygroundParticlesC playgroundParticles, float alpha) {
			Color pColor;
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++) {
				pColor = playgroundParticles.particleCache[i].color;
				pColor.a = alpha;
				playgroundParticles.particleCache[i].color = pColor;
			}
		}
		
		// Set particle random particle positions within min- and max range
		public static void Random (PlaygroundParticlesC playgroundParticles, Vector3 min, Vector3 max) {
			for (int p = 0; p<playgroundParticles.particleCache.Length; p++) {
				playgroundParticles.playgroundCache.position[p] = RandomRange(playgroundParticles.internalRandom03, min, max);
			}
			Update(playgroundParticles);
		}
		
		// Move all particles in direction
		public static void Translate (PlaygroundParticlesC playgroundParticles, Vector3 direction) {
			for (int i = 0; i<playgroundParticles.particleCache.Length; i++)
				playgroundParticles.particleCache[i].position += direction;
		}
		
		// Add new state from state
		public static void Add (PlaygroundParticlesC playgroundParticles, ParticleStateC state) {
			playgroundParticles.states.Add(state);
			state.Initialize();
		}
		
		// Add new state from image
		public static void Add (PlaygroundParticlesC playgroundParticles, Texture2D image, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			playgroundParticles.states.Add(new ParticleStateC());
			playgroundParticles.states[playgroundParticles.states.Count-1].ConstructParticles(image,scale,offset,stateName,stateTransform);
		}
		
		// Add new state from image with depthmap
		public static void Add (PlaygroundParticlesC playgroundParticles, Texture2D image, Texture2D depthmap, float depthmapStrength, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			playgroundParticles.states.Add(new ParticleStateC());
			playgroundParticles.states[playgroundParticles.states.Count-1].ConstructParticles(image,scale,offset,stateName,stateTransform);
			playgroundParticles.states[playgroundParticles.states.Count-1].stateDepthmap = depthmap;
			playgroundParticles.states[playgroundParticles.states.Count-1].stateDepthmapStrength = depthmapStrength;
		}
		
		// Destroy a PlaygroundParticlesC object
		public static void Destroy (PlaygroundParticlesC playgroundParticles) {
			Clear(playgroundParticles);
			MonoBehaviour.DestroyImmediate(playgroundParticles.particleSystemGameObject);
			playgroundParticles = null;
		}

		// Sorts the particles in lifetime
		public static void SetLifetime (PlaygroundParticlesC playgroundParticles, SORTINGC sorting, float time) {
			//PlaygroundC.RunAsync(()=>{
			playgroundParticles.lifetime = time;
			playgroundParticles.playgroundCache.lifetimeOffset = new float[playgroundParticles.particleCount];
			int pCount = playgroundParticles.playgroundCache.lifetimeOffset.Length;
			if (playgroundParticles.source!=SOURCEC.Script) {
				switch (sorting) {
				case SORTINGC.Scrambled:
					for (int r = 0; r<playgroundParticles.particleCount; r++) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						playgroundParticles.playgroundCache.lifetimeOffset[r] = RandomRange(playgroundParticles.internalRandom02, 0f, playgroundParticles.lifetime);
					}
				break;
				case SORTINGC.ScrambledLinear:
					float slPerc;
					for (int sl = 0; sl<playgroundParticles.particleCount; sl++) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						slPerc = (sl*1f)/(playgroundParticles.particleCount*1f);
						playgroundParticles.playgroundCache.lifetimeOffset[sl] = playgroundParticles.lifetime*slPerc;
					}
					for (int i = playgroundParticles.playgroundCache.lifetimeOffset.Length-1; i>0; i--) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						int r = playgroundParticles.internalRandom02.Next(0,i);
						float tmp = playgroundParticles.playgroundCache.lifetimeOffset[i];
						playgroundParticles.playgroundCache.lifetimeOffset[i] = playgroundParticles.playgroundCache.lifetimeOffset[r];
						playgroundParticles.playgroundCache.lifetimeOffset[r] = tmp;
					}
				break;
				case SORTINGC.Burst:
					// No action needed for spawning all particles at once
				break;
				case SORTINGC.Reversed:
					float lPerc;
					for (int l = 0; l<playgroundParticles.particleCount; l++) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						lPerc = (l*1f)/(playgroundParticles.particleCount*1f);
						playgroundParticles.playgroundCache.lifetimeOffset[l] = playgroundParticles.lifetime*lPerc;
					}
				break;
				case SORTINGC.Linear:
					float rPerc;
					int rInc = 0;
					for (int r = playgroundParticles.particleCount-1; r>=0; r--) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						rPerc = (rInc*1f)/(playgroundParticles.particleCount*1f);
						rInc++;
						playgroundParticles.playgroundCache.lifetimeOffset[r] = playgroundParticles.lifetime*rPerc;
					}
				break;
				case SORTINGC.NearestNeighbor:
					playgroundParticles.nearestNeighborOrigin = Mathf.Clamp(playgroundParticles.nearestNeighborOrigin, 0, playgroundParticles.particleCount-1);
					float[] nnDist = new float[playgroundParticles.particleCount];
					float nnHighest = 0;
					int nn = 0;
					for (; nn<playgroundParticles.particleCount; nn++) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						nnDist[nn%playgroundParticles.particleCount] = Vector3.Distance(playgroundParticles.playgroundCache.targetPosition[playgroundParticles.nearestNeighborOrigin%playgroundParticles.particleCount], playgroundParticles.playgroundCache.targetPosition[nn%playgroundParticles.particleCount]);
						if (nnDist[nn%playgroundParticles.particleCount]>nnHighest)
							nnHighest = nnDist[nn%playgroundParticles.particleCount];
					}
					if (nnHighest>0) {
						for (nn = 0; nn<playgroundParticles.particleCount; nn++) {
							if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
							playgroundParticles.playgroundCache.lifetimeOffset[nn%playgroundParticles.particleCount] = Mathf.Lerp(playgroundParticles.lifetime, 0, (nnDist[nn%playgroundParticles.particleCount]/nnHighest));
						}
					} else {
						for (nn = 0; nn<playgroundParticles.particleCount; nn++) {
							if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
							playgroundParticles.playgroundCache.lifetimeOffset[nn%playgroundParticles.particleCount] = 0;
						}
					}
				break;
				case SORTINGC.NearestNeighborReversed:
					playgroundParticles.nearestNeighborOrigin = Mathf.Clamp(playgroundParticles.nearestNeighborOrigin, 0, playgroundParticles.particleCount-1);
					float[] nnrDist = new float[playgroundParticles.particleCount];
					float nnrHighest = 0;
					int nnr = 0;
					for (; nnr<playgroundParticles.particleCount; nnr++) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						nnrDist[nnr%playgroundParticles.particleCount] = Vector3.Distance(playgroundParticles.playgroundCache.targetPosition[playgroundParticles.nearestNeighborOrigin], playgroundParticles.playgroundCache.targetPosition[nnr%playgroundParticles.particleCount]);
						if (nnrDist[nnr%playgroundParticles.particleCount]>nnrHighest)
							nnrHighest = nnrDist[nnr%playgroundParticles.particleCount];
					}
					if (nnrHighest>0) {
						for (nnr = 0; nnr<playgroundParticles.particleCount; nnr++) {
							if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
							playgroundParticles.playgroundCache.lifetimeOffset[nnr%playgroundParticles.particleCount] = Mathf.Lerp(0, playgroundParticles.lifetime, (nnrDist[nnr%playgroundParticles.particleCount]/nnrHighest));
						}
					} else {
						for (nnr = 0; nnr<playgroundParticles.particleCount; nnr++) {
							if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
							playgroundParticles.playgroundCache.lifetimeOffset[nnr%playgroundParticles.particleCount] = 0;
						}
					}
				break;
				case SORTINGC.Custom:
					for (int cs = playgroundParticles.particleCount-1; cs>=0; cs--) {
						if (pCount!=playgroundParticles.playgroundCache.lifetimeOffset.Length) return;
						playgroundParticles.playgroundCache.lifetimeOffset[cs] = playgroundParticles.lifetime*playgroundParticles.lifetimeSorting.Evaluate(cs*1f/playgroundParticles.particleCount*1f);
					}
				break;
				}
			}
			SetEmissionRate(playgroundParticles);
			SetParticleTimeNow(playgroundParticles);
			playgroundParticles.previousLifetime = playgroundParticles.lifetime;
			//playgroundParticles.isDoneThread = true;
			//});
		}
		
		// Set emission rate percentage of particle count
		public static void SetEmissionRate (PlaygroundParticlesC playgroundParticles) {
			float rateCount = playgroundParticles.lifetime*playgroundParticles.emissionRate;
			int currentCount = playgroundParticles.playgroundCache.emission.Length;
			for (int p = 0; p<playgroundParticles.playgroundCache.emission.Length; p++) {
				if (currentCount!=playgroundParticles.playgroundCache.emission.Length || playgroundParticles.playgroundCache.lifetimeOffset.Length!=currentCount) return;
				if (playgroundParticles.emissionRate!=0 && playgroundParticles.source!=SOURCEC.Script) {
					if (playgroundParticles.sorting!=SORTINGC.Burst || playgroundParticles.sorting==SORTINGC.NearestNeighbor && playgroundParticles.overflowOffset!=Vector3.zero || playgroundParticles.sorting==SORTINGC.NearestNeighborReversed && playgroundParticles.overflowOffset!=Vector3.zero) {
						playgroundParticles.playgroundCache.emission[p] = (playgroundParticles.playgroundCache.lifetimeOffset[p]>=playgroundParticles.lifetime-rateCount && playgroundParticles.emit);
					} else {
						playgroundParticles.playgroundCache.emission[p] = (playgroundParticles.emit && playgroundParticles.emissionRate>(p/currentCount));
					}
				} else playgroundParticles.playgroundCache.emission[p] = false;
				if (playgroundParticles.playgroundCache.emission[p])
					playgroundParticles.playgroundCache.rebirth[p] = true;
				else if (playgroundParticles.source==SOURCEC.Script)
					playgroundParticles.playgroundCache.rebirth[p] = false;
			}
			playgroundParticles.previousEmissionRate = playgroundParticles.emissionRate;
		}
		
		// Set life and death of particles
		public static void SetParticleTimeNow (PlaygroundParticlesC playgroundParticles) {
			if (playgroundParticles.playgroundCache.lifetimeOffset==null || playgroundParticles.playgroundCache.lifetimeOffset.Length!=playgroundParticles.particleCount) return;
			if (playgroundParticles.playgroundCache.life==null || playgroundParticles.playgroundCache.life.Length!=playgroundParticles.particleCount) return;
			if (playgroundParticles.source!=SOURCEC.Script) {
				float currentTime = PlaygroundC.globalTime+playgroundParticles.lifetimeOffset;
				int currentCount = playgroundParticles.particleCount;
				playgroundParticles.simulationStarted = currentTime;
				int p;	
				if (playgroundParticles.sorting!=SORTINGC.Burst || playgroundParticles.sorting==SORTINGC.NearestNeighbor && playgroundParticles.overflowOffset!=Vector3.zero || playgroundParticles.sorting==SORTINGC.NearestNeighborReversed && playgroundParticles.overflowOffset!=Vector3.zero) {
					for (p = 0; p<playgroundParticles.particleCount; p++) {
						if (currentCount!=playgroundParticles.particleCount) return;
						playgroundParticles.playgroundCache.life[p] = playgroundParticles.lifetime-(playgroundParticles.lifetime-playgroundParticles.playgroundCache.lifetimeOffset[p]);
						playgroundParticles.playgroundCache.birth[p] = currentTime-playgroundParticles.playgroundCache.life[p];
						playgroundParticles.playgroundCache.death[p] = currentTime+(playgroundParticles.lifetime-playgroundParticles.playgroundCache.lifetimeOffset[p]);
						playgroundParticles.particleCache[p%playgroundParticles.particleCache.Length].startLifetime = playgroundParticles.lifetime;
						playgroundParticles.playgroundCache.targetPosition[p] = PlaygroundC.initialTargetPosition;
						playgroundParticles.playgroundCache.previousTargetPosition[p] = PlaygroundC.initialTargetPosition;
						playgroundParticles.playgroundCache.position[p] = PlaygroundC.initialTargetPosition;
					}
				} else {
					currentTime = PlaygroundC.globalTime;
					// Force recalculation for burst mode to initiate sequence directly
					for (p = 0; p<playgroundParticles.particleCount; p++) {
						if (currentCount!=playgroundParticles.particleCount) return;
						playgroundParticles.playgroundCache.lifetimeOffset[p] = 0;
						playgroundParticles.playgroundCache.life[p] = playgroundParticles.lifetime;
						playgroundParticles.playgroundCache.birth[p] = currentTime-playgroundParticles.lifetime;
						playgroundParticles.playgroundCache.death[p] = currentTime;
						playgroundParticles.particleCache[p%playgroundParticles.particleCache.Length].startLifetime = playgroundParticles.lifetime;
					}

				}
			}
		}
		
		// Set life and death of particles after emit has changed
		public static void SetParticleTimeNowWithRestEmission (PlaygroundParticlesC playgroundParticles) {
			float currentGlobalTime = PlaygroundC.globalTime;
			float currentTime = currentGlobalTime+playgroundParticles.lifetimeOffset;
			float emissionDelta = currentGlobalTime-playgroundParticles.emissionStopped;
			bool applyDelta = false;
			if (emissionDelta<playgroundParticles.lifetime && emissionDelta>0) {
				applyDelta = true;
				playgroundParticles.cameFromNonEmissionFrame = true;
			}
			for (int p = 0; p<playgroundParticles.particleCount; p++) {
				if (!applyDelta) {
					if (playgroundParticles.sorting!=SORTINGC.Burst || playgroundParticles.sorting==SORTINGC.NearestNeighbor && playgroundParticles.overflowOffset!=Vector3.zero || playgroundParticles.sorting==SORTINGC.NearestNeighborReversed && playgroundParticles.overflowOffset!=Vector3.zero) {
						playgroundParticles.playgroundCache.life[p] = playgroundParticles.lifetime-(playgroundParticles.lifetime-playgroundParticles.playgroundCache.lifetimeOffset[p]);
						playgroundParticles.playgroundCache.birth[p] = currentTime-playgroundParticles.playgroundCache.life[p];
						playgroundParticles.playgroundCache.death[p] = currentTime+(playgroundParticles.lifetime-playgroundParticles.playgroundCache.lifetimeOffset[p]);
					} else {
						playgroundParticles.playgroundCache.life[p] = playgroundParticles.lifetime;
						playgroundParticles.playgroundCache.birth[p] = currentTime-playgroundParticles.lifetime;
						playgroundParticles.playgroundCache.death[p] = currentTime;
					}
					playgroundParticles.playgroundCache.birthDelay[p] = 0f;
				} else {
					playgroundParticles.playgroundCache.birthDelay[p] = emissionDelta;
				}
				playgroundParticles.playgroundCache.emission[p] = playgroundParticles.emit;
			}
		}
		
		// Get color from evaluated lifetime color value where time is normalized
		public static Color32 GetColorAtLifetime (PlaygroundParticlesC playgroundParticles, float time) {
			return playgroundParticles.lifetimeColor.Evaluate(time/playgroundParticles.lifetime);
		}
		
		// Color all particles from evaluated lifetime color value where time is normalized
		public static void SetColorAtLifetime (PlaygroundParticlesC playgroundParticles, float time) {
			Color32 c = playgroundParticles.lifetimeColor.Evaluate(time/playgroundParticles.lifetime);
			for (int p = 0; p<playgroundParticles.particleCount; p++)
				playgroundParticles.particleCache[p].color = c;
			playgroundParticles.shurikenParticleSystem.SetParticles(playgroundParticles.particleCache, playgroundParticles.particleCache.Length);
		}
		
		// Color all particles from lifetime span with sorting
		public static void SetColorWithLifetimeSorting (PlaygroundParticlesC playgroundParticles) {
			SetLifetime(playgroundParticles, playgroundParticles.sorting, playgroundParticles.lifetime);
			Color32 c;
			for (int p = 0; p<playgroundParticles.particleCount; p++) {
				c = playgroundParticles.lifetimeColor.Evaluate(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime);
				playgroundParticles.particleCache[p].color = c;
			}
			playgroundParticles.shurikenParticleSystem.SetParticles(playgroundParticles.particleCache, playgroundParticles.particleCache.Length);
		}

		// Sets the amount of particles and initiates the necessary arrays
		public static void SetParticleCount (PlaygroundParticlesC playgroundParticles, int amount) {
			if (amount<0) amount = 0;

			playgroundParticles.particleCount = amount;

			// Create Particles
			playgroundParticles.particleCache = new ParticleSystem.Particle[amount];
			playgroundParticles.shurikenParticleSystem.Emit(amount);
			playgroundParticles.shurikenParticleSystem.GetParticles(playgroundParticles.particleCache);

			PlaygroundC.RunAsync(()=>{
			
			
			if (playgroundParticles.playgroundCache==null)
				playgroundParticles.playgroundCache = new PlaygroundCache();
			
			for (int i = 0; i<amount; i++) {
				playgroundParticles.particleCache[i].position = PlaygroundC.initialTargetPosition;
				playgroundParticles.particleCache[i].size = 0f;
			}
			
			playgroundParticles.inTransition = false;

			// Rebuild Cache
			playgroundParticles.playgroundCache.size = new float[amount];
			playgroundParticles.playgroundCache.birth = new float[amount];
			playgroundParticles.playgroundCache.death = new float[amount];
			playgroundParticles.playgroundCache.rebirth = new bool[amount];
			playgroundParticles.playgroundCache.birthDelay = new float[amount];
			playgroundParticles.playgroundCache.life = new float[amount];
			playgroundParticles.playgroundCache.rotation = new float[amount];
			playgroundParticles.playgroundCache.lifetimeOffset = new float[amount];
			playgroundParticles.playgroundCache.emission = new bool[amount];
			playgroundParticles.playgroundCache.changedByProperty = new bool[amount];
			playgroundParticles.playgroundCache.changedByPropertyColor = new bool[amount];
			playgroundParticles.playgroundCache.changedByPropertyColorLerp = new bool[amount];
			playgroundParticles.playgroundCache.changedByPropertyColorKeepAlpha = new bool[amount];
			playgroundParticles.playgroundCache.changedByPropertySize = new bool[amount];
			playgroundParticles.playgroundCache.changedByPropertyTarget = new bool[amount];
			playgroundParticles.playgroundCache.changedByPropertyDeath = new bool[amount];
			playgroundParticles.playgroundCache.propertyTarget = new int[amount];
			playgroundParticles.playgroundCache.propertyId = new int[amount];
			playgroundParticles.playgroundCache.propertyColorId = new int[amount];
			playgroundParticles.playgroundCache.color = new Color32[amount];
			playgroundParticles.playgroundCache.scriptedColor = new Color32[amount];
			playgroundParticles.playgroundCache.initialColor = new Color32[amount];
			playgroundParticles.playgroundCache.lifetimeColorId = new int[amount];
			playgroundParticles.playgroundCache.position = new Vector3[amount];
			playgroundParticles.playgroundCache.targetPosition = new Vector3[amount];
			playgroundParticles.playgroundCache.targetDirection = new Vector3[amount];
			playgroundParticles.playgroundCache.previousTargetPosition = new Vector3[amount];
			playgroundParticles.playgroundCache.previousParticlePosition = new Vector3[amount];
			playgroundParticles.playgroundCache.collisionParticlePosition = new Vector3[amount];
			playgroundParticles.playgroundCache.localSpaceMovementCompensation = new Vector3[amount];
			playgroundParticles.playgroundCache.scatterPosition = new Vector3[amount];
			playgroundParticles.playgroundCache.velocity = new Vector3[amount];
			playgroundParticles.playgroundCache.isMasked = new bool[amount];
			playgroundParticles.playgroundCache.maskAlpha = new float[amount];
			playgroundParticles.previousParticleCount = playgroundParticles.particleCount;

			// Set sizes
			SetSizeRandom(playgroundParticles, playgroundParticles.sizeMin, playgroundParticles.sizeMax);
			playgroundParticles.previousSizeMin = playgroundParticles.sizeMin;
			playgroundParticles.previousSizeMax = playgroundParticles.sizeMax;
			
			// Set rotations
			playgroundParticles.playgroundCache.initialRotation = RandomFloat(amount, playgroundParticles.initialRotationMin, playgroundParticles.initialRotationMax);
			playgroundParticles.playgroundCache.rotationSpeed = RandomFloat(amount, playgroundParticles.rotationSpeedMin, playgroundParticles.rotationSpeedMax);
			playgroundParticles.previousInitialRotationMin = playgroundParticles.initialRotationMin;
			playgroundParticles.previousInitialRotationMax = playgroundParticles.initialRotationMax;
			playgroundParticles.previousRotationSpeedMin = playgroundParticles.rotationSpeedMin;
			playgroundParticles.previousRotationSpeedMax = playgroundParticles.rotationSpeedMax;
		
			// Set velocities
			SetVelocityRandom(playgroundParticles, playgroundParticles.initialVelocityMin, playgroundParticles.initialVelocityMax);
			SetLocalVelocityRandom(playgroundParticles, playgroundParticles.initialLocalVelocityMin, playgroundParticles.initialLocalVelocityMax);
			
			// Set Emission
			Emission(playgroundParticles, playgroundParticles.emit, false);
			
			// Make sure scatter is available first lifetime cycle
			if (playgroundParticles.applySourceScatter)
				playgroundParticles.RefreshScatter();
			
			playgroundParticles.isDoneThread = true;
			});
		}

		// Updates a PlaygroundParticlesC object (called from Playground)
		public static void Update (PlaygroundParticlesC playgroundParticles) {
			if (playgroundParticles.isYieldRefreshing || playgroundParticles.isLoading || playgroundParticles.playgroundCache==null) return;

			// Emission halt for disabling called from calculation thread
			if (playgroundParticles.queueEmissionHalt)
				playgroundParticles.particleSystemGameObject.SetActive(false);

			// Particle count
			if (playgroundParticles.particleCount!=playgroundParticles.previousParticleCount) {
				SetParticleCount(playgroundParticles, playgroundParticles.particleCount);
				playgroundParticles.Start();
				return;
			}

			// Particle emission
			if (playgroundParticles.emit!=playgroundParticles.previousEmission) {
				playgroundParticles.Emit (playgroundParticles.emit);
			}
			
			// Particle size
			if (playgroundParticles.sizeMin!=playgroundParticles.previousSizeMin || playgroundParticles.sizeMax!=playgroundParticles.previousSizeMax)
				SetSizeRandom(playgroundParticles, playgroundParticles.sizeMin, playgroundParticles.sizeMax);
			
			// Particle rotation
			if (playgroundParticles.initialRotationMin!=playgroundParticles.previousInitialRotationMin || playgroundParticles.initialRotationMax!=playgroundParticles.previousInitialRotationMax)
				SetInitialRotationRandom(playgroundParticles, playgroundParticles.initialRotationMin, playgroundParticles.initialRotationMax);
			if (playgroundParticles.rotationSpeedMin!=playgroundParticles.previousRotationSpeedMin || playgroundParticles.rotationSpeedMax!=playgroundParticles.previousRotationSpeedMax)
				SetRotationRandom(playgroundParticles, playgroundParticles.rotationSpeedMin, playgroundParticles.rotationSpeedMax);
			
			// Particle velocity
			if (playgroundParticles.applyInitialVelocity)
				if (playgroundParticles.initialVelocityMin!=playgroundParticles.previousVelocityMin || playgroundParticles.initialVelocityMax!=playgroundParticles.previousVelocityMax || playgroundParticles.playgroundCache.initialVelocity==null || playgroundParticles.playgroundCache.initialVelocity.Length!=playgroundParticles.particleCount)
					SetVelocityRandom(playgroundParticles, playgroundParticles.initialVelocityMin, playgroundParticles.initialVelocityMax);
			
			// Particle local velocity
			if (playgroundParticles.applyInitialLocalVelocity)
				if (playgroundParticles.initialLocalVelocityMin!=playgroundParticles.previousLocalVelocityMin || playgroundParticles.initialLocalVelocityMax!=playgroundParticles.previousLocalVelocityMax || playgroundParticles.playgroundCache.initialLocalVelocity==null || playgroundParticles.playgroundCache.initialLocalVelocity.Length!=playgroundParticles.particleCount)
					SetLocalVelocityRandom(playgroundParticles, playgroundParticles.initialLocalVelocityMin, playgroundParticles.initialLocalVelocityMax);
			
			// Particle life
			if (playgroundParticles.previousLifetime!=playgroundParticles.lifetime) {
				SetLifetime(playgroundParticles, playgroundParticles.sorting, playgroundParticles.lifetime);
				return;
			}

			// Particle emission rate
			if (playgroundParticles.previousEmissionRate!=playgroundParticles.emissionRate)
				SetEmissionRate(playgroundParticles);
			
			// Particle state change
			if (playgroundParticles.source==SOURCEC.State && playgroundParticles.activeState!=playgroundParticles.previousActiveState) {
				if (playgroundParticles.states[playgroundParticles.activeState].positionLength>playgroundParticles.particleCount)
					SetParticleCount(playgroundParticles, playgroundParticles.states[playgroundParticles.activeState].positionLength);
				playgroundParticles.previousActiveState = playgroundParticles.activeState;
			}
			
			// Particle calculation
			if (PlaygroundC.reference.calculate && playgroundParticles.calculate && !playgroundParticles.inTransition)
				ThreadedCalculations(playgroundParticles);
			else playgroundParticles.cameFromNonCalculatedFrame = true;
			
			// Assign all particles into the particle system
			if (!playgroundParticles.inTransition && playgroundParticles.particleCache.Length>0 && playgroundParticles.calculate)
				playgroundParticles.shurikenParticleSystem.SetParticles(playgroundParticles.particleCache, playgroundParticles.particleCache.Length);

			// Make sure this particle system is playing
			if (playgroundParticles.shurikenParticleSystem.isPaused || playgroundParticles.shurikenParticleSystem.isStopped)
				playgroundParticles.shurikenParticleSystem.Play();
		}
		
		// Initial target position
		public static void SetInitialTargetPosition (PlaygroundParticlesC playgroundParticles, Vector3 position, bool refreshParticleSystem) {
			for (int p = 0; p<playgroundParticles.particleCache.Length; p++) {
				playgroundParticles.playgroundCache.previousTargetPosition[p] = position;
				playgroundParticles.playgroundCache.targetPosition[p] = position;
				playgroundParticles.particleCache[p].size = 0f;
				playgroundParticles.playgroundCache.position[p] = position;
			}
			if (refreshParticleSystem)
				playgroundParticles.particleSystem.SetParticles(playgroundParticles.particleCache, playgroundParticles.particleCache.Length);
		}

		// Set emission of PlaygroundParticlesC object
		public static void Emission (PlaygroundParticlesC playgroundParticles, bool emission, bool callRestEmission) {
			playgroundParticles.previousEmission = emission;
			if (emission) {
				for (int p = 0; p<playgroundParticles.playgroundCache.rebirth.Length; p++)
					playgroundParticles.playgroundCache.rebirth[p] = true;
				if (callRestEmission)
					SetParticleTimeNowWithRestEmission(playgroundParticles);
			}
		}
		
		// Returns the angle between a and b with normal direction
		public static float SignedAngle (Vector3 a, Vector3 b, Vector3 n) {
			return (Vector3.Angle(a, b)*Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b))));
		}
		
		// Returns a random value between negative- and positive Vector3
		public static Vector3 RandomVector3 (System.Random random, Vector3 v1, Vector3 v2) {
			return RandomRange (random, v1, v2);
		}

		// Threaded particle calculations
		object locker = new object();
		bool isDoneThread = true;
		public static void ThreadedCalculations (PlaygroundParticlesC playgroundParticles) {

			// Component disabled?
			if (!playgroundParticles.enabled || !playgroundParticles.isDoneThread) return;

			// Has the calculation been paused previously?
			if (playgroundParticles.cameFromNonCalculatedFrame) {
				playgroundParticles.Start();
				playgroundParticles.cameFromNonCalculatedFrame = false;
				return;
			}
		
			// Apply locked position
			if (playgroundParticles.applyLockPosition) {
				if (playgroundParticles.lockPositionIsLocal)
					playgroundParticles.particleSystemTransform.localPosition = playgroundParticles.lockPosition;
				else
					playgroundParticles.particleSystemTransform.position = playgroundParticles.lockPosition;
			}

			// Apply locked rotation
			if (playgroundParticles.applyLockRotation) {
				if (playgroundParticles.lockRotationIsLocal)
					playgroundParticles.particleSystemTransform.localRotation = Quaternion.Euler(playgroundParticles.lockRotation);
				else
					playgroundParticles.particleSystemTransform.rotation = Quaternion.Euler(playgroundParticles.lockRotation);
			}

			// Apply locked scale
			if (playgroundParticles.applyLockScale)
				playgroundParticles.particleSystemTransform.localScale = playgroundParticles.lockScale;
				

			// Set delta time
			playgroundParticles.localDeltaTime = PlaygroundC.globalTime-playgroundParticles.lastTimeUpdated;
			playgroundParticles.lastTimeUpdated = PlaygroundC.globalTime;
			float t = (playgroundParticles.localDeltaTime*playgroundParticles.particleTimescale);

			// Prepare Source positions
			Vector3 	stPos = Vector3.zero;
			Quaternion 	stRot = Quaternion.identity;
			Vector3 	stSca = new Vector3(1f, 1f, 1f);
			Vector3		stDir = new Vector3();
			bool 		localSpace = (playgroundParticles.shurikenParticleSystem.simulationSpace == ParticleSystemSimulationSpace.Local);
			bool 		overflow = (playgroundParticles.overflowOffset!=Vector3.zero);
			bool 		skinnedWorldObjectReady = false;
			playgroundParticles.renderModeStretch = playgroundParticles.particleSystemRenderer2.renderMode==ParticleSystemRenderMode.Stretch;
			
			if (playgroundParticles.emit) {
				switch (playgroundParticles.source) {
				case SOURCEC.Script:

				break;
				case SOURCEC.State:
					if (playgroundParticles.states.Count>0) {
						if (playgroundParticles.states[playgroundParticles.activeState].stateTransform!=null) {

							if (!playgroundParticles.states[playgroundParticles.activeState].initialized)
								playgroundParticles.states[playgroundParticles.activeState].Initialize ();

							stPos = playgroundParticles.states[playgroundParticles.activeState].stateTransform.position;
							stRot = playgroundParticles.states[playgroundParticles.activeState].stateTransform.rotation;
							stSca = playgroundParticles.states[playgroundParticles.activeState].stateTransform.localScale;
							if (localSpace && (playgroundParticles.states[playgroundParticles.activeState].stateTransform.parent==playgroundParticles.particleSystemTransform || playgroundParticles.states[playgroundParticles.activeState].stateTransform==playgroundParticles.particleSystemTransform)) {
								stPos = Vector3.zero;
								stRot = Quaternion.Euler (Vector3.zero);
							}
						}
					} else return;
					break;
				case SOURCEC.Transform:
					stPos = playgroundParticles.sourceTransform.position;
					stRot = playgroundParticles.sourceTransform.rotation;
					stSca = playgroundParticles.sourceTransform.localScale;
					if (localSpace && playgroundParticles.sourceTransform==playgroundParticles.particleSystemTransform) {
						stPos = Vector3.zero;
						stRot = Quaternion.Euler (Vector3.zero);
					}
					break;
				case SOURCEC.WorldObject:
					
					// Handle vertex data in active World Object
					if (playgroundParticles.worldObject.gameObject!=null) {
						if (playgroundParticles.worldObject.gameObject.GetInstanceID()!=playgroundParticles.worldObject.cachedId) 
							playgroundParticles.worldObject = NewWorldObject(playgroundParticles.worldObject.gameObject.transform);
						if (playgroundParticles.worldObject.mesh!=null) {
							stPos = playgroundParticles.worldObject.transform.position;
							stRot = playgroundParticles.worldObject.transform.rotation;
							stSca = playgroundParticles.worldObject.transform.localScale;
							if (localSpace) {
								stPos = Vector3.zero;
								stRot = Quaternion.Euler (Vector3.zero);
							}
							
							playgroundParticles.worldObject.updateNormals = playgroundParticles.worldObjectUpdateNormals;
							if (playgroundParticles.worldObjectUpdateVertices)
								playgroundParticles.worldObject.Update ();
						} else return;
					} else return;
					break;
				case SOURCEC.SkinnedWorldObject:

					// Handle vertex data in active Skinned World Object
					if (playgroundParticles.skinnedWorldObject.gameObject!=null) {
						if (playgroundParticles.skinnedWorldObject.gameObject.GetInstanceID()!=playgroundParticles.skinnedWorldObject.cachedId)
							playgroundParticles.skinnedWorldObject = NewSkinnedWorldObject(playgroundParticles.skinnedWorldObject.gameObject.transform, playgroundParticles.skinnedWorldObject.downResolution);
					}
					skinnedWorldObjectReady = playgroundParticles.skinnedWorldObject.gameObject!=null && playgroundParticles.skinnedWorldObject.mesh!=null;
					if (skinnedWorldObjectReady) {
						
						stPos = playgroundParticles.skinnedWorldObject.transform.position;
						stRot = playgroundParticles.skinnedWorldObject.transform.rotation;
						stSca = playgroundParticles.skinnedWorldObject.transform.localScale;
						stDir = playgroundParticles.skinnedWorldObject.transform.TransformDirection (playgroundParticles.overflowOffset);
						
						playgroundParticles.skinnedWorldObject.updateNormals = playgroundParticles.worldObjectUpdateNormals;

						if (Time.frameCount%PlaygroundC.skinnedUpdateRate==0) {
							if (playgroundParticles.worldObjectUpdateVertices)
								playgroundParticles.skinnedWorldObject.MeshUpdate();
							playgroundParticles.skinnedWorldObject.BoneUpdate();
						}
					} else return;
				break;
				case SOURCEC.Paint:
					if (playgroundParticles.paint.initialized) {
						stPos = playgroundParticles.particleSystemTransform.position;
						stRot = playgroundParticles.particleSystemTransform.rotation;
						stSca = playgroundParticles.particleSystemTransform.localScale;

						if (playgroundParticles.paint.positionLength>0) {
							for (int p = 0; p<playgroundParticles.particleCache.Length; p++) {
								playgroundParticles.paint.Update(p);
							}
						} else return;
					} else {
						playgroundParticles.paint.Initialize ();
						return;
					}
				break;
				case SOURCEC.Projection:
					if (playgroundParticles.projection.projectionTexture!=null && playgroundParticles.projection.projectionTransform!=null) {
						if (!playgroundParticles.projection.initialized)
							playgroundParticles.projection.Initialize();

						stPos = playgroundParticles.projection.projectionTransform.position;
						stRot = playgroundParticles.projection.projectionTransform.rotation;
						stSca = playgroundParticles.projection.projectionTransform.localScale;
						
						if (localSpace)
							playgroundParticles.shurikenParticleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
						
						if (playgroundParticles.projection.liveUpdate || !playgroundParticles.projection.hasRefreshed) {
							playgroundParticles.projection.UpdateSource();
							playgroundParticles.projection.Update();
							stDir = playgroundParticles.projection.projectionTransform.TransformDirection (playgroundParticles.overflowOffset);

							playgroundParticles.projection.hasRefreshed = true;
						}
						
					} else return;
				break;
				}
			}

			// Collision detection (runs on main-thread)
			if (playgroundParticles.collision)
				Collisions(playgroundParticles);

			// Sync positions to main-thread
			if (playgroundParticles.syncPositionsOnMainThread) {
				for (int p = 0; p<playgroundParticles.particleCount; p++) {
					playgroundParticles.particleCache[p].position = playgroundParticles.playgroundCache.position[p];
				}
			}

			// Main-thread preparations
			// Prepare Particle colors
			bool stateReadyForTextureColor = (playgroundParticles.source==SOURCEC.State && playgroundParticles.states[playgroundParticles.activeState].stateTexture!=null);

			// Prepare local manipulators
			for (int m = 0; m<playgroundParticles.manipulators.Count; m++) {
				playgroundParticles.manipulators[m].Update();
				playgroundParticles.manipulators[m].transform.SetLocalPosition(playgroundParticles.particleSystemTransform);
				playgroundParticles.manipulators[m].SetLocalTargetsPosition(playgroundParticles.particleSystemTransform);
				playgroundParticles.manipulators[m].willAffect = true;
			}
			// Prepare global manipulators from this local space
			for (int m = 0; m<PlaygroundC.reference.manipulators.Count; m++) {
				PlaygroundC.reference.manipulators[m].willAffect = ((PlaygroundC.reference.manipulators[m].affects.value & 1<<playgroundParticles.particleSystemGameObject.layer)!=0);
				PlaygroundC.reference.manipulators[m].transform.SetLocalPosition(playgroundParticles.particleSystemTransform);
			}

			// Prepare events
			bool hasEvent = playgroundParticles.events.Count>0;
			bool hasTimerEvent = false;
			if (hasEvent) {
				for (int i = 0; i<playgroundParticles.events.Count; i++) {
					playgroundParticles.events[i].Initialize();
					if (playgroundParticles.events[i].initializedTarget && playgroundParticles.events[i].broadcastType!=EVENTBROADCASTC.EventListeners)
						if (!playgroundParticles.events[i].target.eventControlledBy.Contains (playgroundParticles))
							playgroundParticles.events[i].target.eventControlledBy.Add (playgroundParticles);
					
					if (playgroundParticles.events[i].eventType==EVENTTYPEC.Time) {
						hasTimerEvent = playgroundParticles.events[i].UpdateTime();
					}
				}
			}

			// Calculate all thread-safe data on a new thread
			if (!playgroundParticles.isDoneThread) return;
			playgroundParticles.isDoneThread = false;
			PlaygroundC.RunAsync(()=>{
				if (playgroundParticles.isDoneThread) return;
				lock (playgroundParticles.locker) {

				// Prepare variables for particle source positions
				Matrix4x4 stMx = new Matrix4x4();
				Matrix4x4 fMx = new Matrix4x4();
				stMx.SetTRS(stPos, stRot, stSca);
				fMx.SetTRS(Vector3.zero, stRot, new Vector3(1f,1f,1f));
				int downResolution = playgroundParticles.skinnedWorldObject.downResolution;
				int downResolutionP;

				// Prepare variables for lifetime, velocity & manipulators
				int m = 0;
				float evaluatedLife;
				Vector3 deltaVelocity;
				Vector3 manipulatorPosition;
				float manipulatorDistance;
				Vector3 zero = Vector3.zero;
				Vector3 up = Vector3.up;

				// Prepare variables for colors
				Color lifetimeColor = new Color();
				bool hasLifetimeColors = (playgroundParticles.lifetimeColors.Count>0);

				// Update skinned mesh vertices
				if (skinnedWorldObjectReady)
					playgroundParticles.skinnedWorldObject.Update();
				
				int pCount = playgroundParticles.particleCache.Length;
				bool applyMask = false;

				// Check that cache is correct
				if (playgroundParticles.playgroundCache.maskAlpha.Length!=pCount) {
					playgroundParticles.playgroundCache.maskAlpha = new float[pCount];
					playgroundParticles.playgroundCache.isMasked = new bool[pCount];
				}

				// Calculation loop
				for (int p = 0; p<playgroundParticles.particleCache.Length; p++) {
					
					// Check that particle count is correct
					if (pCount != playgroundParticles.particleCache.Length) {
						playgroundParticles.cameFromNonEmissionFrame = false;
						playgroundParticles.isDoneThread = true;
						return;
					}

					// Apply particle mask
					if (p<playgroundParticles.particleMask) {
						if (playgroundParticles.playgroundCache.maskAlpha[p]<=0 || playgroundParticles.particleMaskTime<=0) {
							playgroundParticles.playgroundCache.isMasked[p] = true;
							playgroundParticles.playgroundCache.maskAlpha[p] = 0;
							playgroundParticles.particleCache[p].size = 0;
							applyMask = true;
						} else {
							applyMask = false;
							playgroundParticles.playgroundCache.maskAlpha[p] -= (1f/playgroundParticles.particleMaskTime)*playgroundParticles.localDeltaTime;
						}
					} else {
						applyMask = false;
						if (playgroundParticles.playgroundCache.maskAlpha[p]<1f && playgroundParticles.particleMaskTime>0) {
							playgroundParticles.playgroundCache.maskAlpha[p] += (1f/playgroundParticles.particleMaskTime)*playgroundParticles.localDeltaTime;
						} else {
							playgroundParticles.playgroundCache.maskAlpha[p] = 1f;
							playgroundParticles.playgroundCache.isMasked[p] = false;
						}
					}

					// Get this particle's color
					if (!playgroundParticles.playgroundCache.changedByPropertyColor[p] && !playgroundParticles.playgroundCache.changedByPropertyColorLerp[p]) {
						switch (playgroundParticles.colorSource) {
						case COLORSOURCEC.Source:
							switch (playgroundParticles.source) {
							case SOURCEC.Script:
								lifetimeColor = playgroundParticles.playgroundCache.scriptedColor[p];
								break;
							case SOURCEC.State:
								if (stateReadyForTextureColor)
									lifetimeColor = playgroundParticles.states[playgroundParticles.activeState].GetColor(p);
								else
									lifetimeColor = playgroundParticles.lifetimeColor.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
								break;
							case SOURCEC.Projection:
								lifetimeColor = playgroundParticles.projection.GetColor(p);
								break;
							case SOURCEC.Paint:
								lifetimeColor = playgroundParticles.paint.GetColor(p);
								break;
							default:
								lifetimeColor = playgroundParticles.lifetimeColor.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
								break;
							}
							if (playgroundParticles.sourceUsesLifetimeAlpha)
								lifetimeColor.a = Mathf.Clamp(playgroundParticles.lifetimeColor.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime)).a, 0, lifetimeColor.a);
							break;
						case COLORSOURCEC.LifetimeColor:
							lifetimeColor = playgroundParticles.lifetimeColor.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
							break;
						case COLORSOURCEC.LifetimeColors:
							if (hasLifetimeColors)
								lifetimeColor = playgroundParticles.lifetimeColors[playgroundParticles.playgroundCache.lifetimeColorId[p]%playgroundParticles.lifetimeColors.Count].gradient.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
							else
								lifetimeColor = playgroundParticles.lifetimeColor.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
							break;
						}
						
					} else if (playgroundParticles.playgroundCache.changedByPropertyColorKeepAlpha[p]) {
						lifetimeColor = playgroundParticles.particleCache[p].color;
						lifetimeColor.a = Mathf.Clamp(playgroundParticles.lifetimeColor.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime)).a, 0, lifetimeColor.a);
					}

					// Assign color to particle
					lifetimeColor*=playgroundParticles.playgroundCache.maskAlpha[p];
					playgroundParticles.particleCache[p].color = lifetimeColor;
					
					// Give Playground Cache its color value
					playgroundParticles.playgroundCache.color[p] = lifetimeColor;

					// Source positions
					if (playgroundParticles.emit) {
						switch (playgroundParticles.source) {
						case SOURCEC.State:
							if (playgroundParticles.playgroundCache.rebirth[p] || playgroundParticles.onlySourcePositioning) {
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								if (playgroundParticles.applyInitialLocalVelocity)
									playgroundParticles.playgroundCache.targetDirection[p] = fMx.MultiplyPoint3x4(Vector3Scale(playgroundParticles.playgroundCache.initialLocalVelocity[p], playgroundParticles.states[playgroundParticles.activeState].GetNormal(p)));
								if (!overflow) {
									playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(playgroundParticles.states[playgroundParticles.activeState].GetPosition(p))+playgroundParticles.playgroundCache.scatterPosition[p];
								} else {
									switch (playgroundParticles.overflowMode) {
									case OVERFLOWMODEC.SourceTransform:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(playgroundParticles.states[playgroundParticles.activeState].GetPosition(p)+GetOverflow2(playgroundParticles.overflowOffset, p, playgroundParticles.states[playgroundParticles.activeState].positionLength))+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.World:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(playgroundParticles.states[playgroundParticles.activeState].GetPosition(p))+GetOverflow2(playgroundParticles.overflowOffset, p, playgroundParticles.states[playgroundParticles.activeState].positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.SourcePoint:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(playgroundParticles.states[playgroundParticles.activeState].GetPosition(p)+GetOverflow2(playgroundParticles.overflowOffset, playgroundParticles.states[playgroundParticles.activeState].GetNormal(p), p, playgroundParticles.states[playgroundParticles.activeState].positionLength))+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									}
								}
								
								// Local space compensation calculation
								if (localSpace && playgroundParticles.applyLocalSpaceMovementCompensation)
									playgroundParticles.playgroundCache.localSpaceMovementCompensation[p] = playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p];
							}
						break;
						case SOURCEC.Transform:
							if (playgroundParticles.playgroundCache.rebirth[p] || playgroundParticles.onlySourcePositioning) {
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								if (playgroundParticles.applyInitialLocalVelocity)
									playgroundParticles.playgroundCache.targetDirection[p] = stRot*playgroundParticles.playgroundCache.initialLocalVelocity[p];
								if (!overflow) {
									playgroundParticles.playgroundCache.targetPosition[p] = stPos+playgroundParticles.playgroundCache.scatterPosition[p];
								} else {
									switch (playgroundParticles.overflowMode) {
									case OVERFLOWMODEC.SourceTransform:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(GetOverflow2(playgroundParticles.overflowOffset, p, 1))+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.World:
										playgroundParticles.playgroundCache.targetPosition[p] = stPos+GetOverflow2(playgroundParticles.overflowOffset, p, 1)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.SourcePoint:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(GetOverflow2(playgroundParticles.overflowOffset, p, 1))+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									}
								}
								
								// Local space compensation calculation
								if (localSpace && playgroundParticles.applyLocalSpaceMovementCompensation)
									playgroundParticles.playgroundCache.localSpaceMovementCompensation[p] = playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p];
							}
						break;
						case SOURCEC.WorldObject:
							if (playgroundParticles.playgroundCache.rebirth[p] || playgroundParticles.onlySourcePositioning) {
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								if (playgroundParticles.applyInitialLocalVelocity)
									playgroundParticles.playgroundCache.targetDirection[p] = fMx.MultiplyPoint3x4(Vector3Scale(playgroundParticles.playgroundCache.initialLocalVelocity[p], playgroundParticles.worldObject.normals[p%playgroundParticles.worldObject.vertexPositions.Length]));
								if (!overflow) {
									playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(
										playgroundParticles.worldObject.vertexPositions[p%playgroundParticles.worldObject.vertexPositions.Length]
										)+playgroundParticles.playgroundCache.scatterPosition[p];
								} else {
									switch (playgroundParticles.overflowMode) {
									case OVERFLOWMODEC.SourceTransform:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(
											playgroundParticles.worldObject.vertexPositions[p%playgroundParticles.worldObject.vertexPositions.Length]+GetOverflow2(playgroundParticles.overflowOffset, p, playgroundParticles.worldObject.vertexPositions.Length)
											)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.World:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(
											playgroundParticles.worldObject.vertexPositions[p%playgroundParticles.worldObject.vertexPositions.Length]
											)+GetOverflow2(playgroundParticles.overflowOffset, p, playgroundParticles.worldObject.vertexPositions.Length)+
											playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.SourcePoint:
										playgroundParticles.playgroundCache.targetPosition[p] = stMx.MultiplyPoint3x4(
											playgroundParticles.worldObject.vertexPositions[p%playgroundParticles.worldObject.vertexPositions.Length]+GetOverflow2(playgroundParticles.overflowOffset, playgroundParticles.worldObject.normals[p%playgroundParticles.worldObject.normals.Length], p, playgroundParticles.worldObject.vertexPositions.Length)
											)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									}
								}
								
								// Local space compensation calculation
								if (localSpace && playgroundParticles.applyLocalSpaceMovementCompensation)
									playgroundParticles.playgroundCache.localSpaceMovementCompensation[p] = playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p];
							}
						break;
						case SOURCEC.SkinnedWorldObject:
							if (playgroundParticles.playgroundCache.rebirth[p] || playgroundParticles.onlySourcePositioning) {
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								if (playgroundParticles.applyInitialLocalVelocity)
									playgroundParticles.playgroundCache.targetDirection[p] = fMx.MultiplyPoint3x4(Vector3Scale(playgroundParticles.playgroundCache.initialLocalVelocity[p], playgroundParticles.skinnedWorldObject.normals[p%playgroundParticles.skinnedWorldObject.normals.Length]));
								downResolutionP = Mathf.RoundToInt(p*downResolution)%playgroundParticles.skinnedWorldObject.vertexPositions.Length;
								if (!overflow) {
									playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.skinnedWorldObject.vertexPositions[downResolutionP]+playgroundParticles.playgroundCache.scatterPosition[p];
								} else {
									switch (playgroundParticles.overflowMode) {
									case OVERFLOWMODEC.SourceTransform:
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.skinnedWorldObject.vertexPositions[downResolutionP]+
											GetOverflow2(
												stDir,
												p, 
												playgroundParticles.skinnedWorldObject.vertexPositions.Length/downResolution
												)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.World:
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.skinnedWorldObject.vertexPositions[downResolutionP]+
											GetOverflow2(
												playgroundParticles.overflowOffset,
												p, 
												playgroundParticles.skinnedWorldObject.vertexPositions.Length/downResolution
												)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.SourcePoint:
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.skinnedWorldObject.vertexPositions[downResolutionP]+
											GetOverflow2(
												playgroundParticles.overflowOffset,
												playgroundParticles.skinnedWorldObject.normals[downResolutionP],
												p, 
												playgroundParticles.skinnedWorldObject.vertexPositions.Length/downResolution
												)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									}
								}
								
								// Local space compensation calculation
								if (localSpace && playgroundParticles.applyLocalSpaceMovementCompensation)
									playgroundParticles.playgroundCache.localSpaceMovementCompensation[p] = playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p];
							}
						
						break;
						case SOURCEC.Projection:
								if (playgroundParticles.playgroundCache.rebirth[p] || playgroundParticles.onlySourcePositioning) {
									playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];

									if (playgroundParticles.applyInitialLocalVelocity)
										playgroundParticles.playgroundCache.targetDirection[p] = Vector3Scale(playgroundParticles.projection.GetNormal(p), playgroundParticles.playgroundCache.initialLocalVelocity[p]);
									if (!overflow) {
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.projection.GetPosition(p)+playgroundParticles.playgroundCache.scatterPosition[p];
									} else {
										switch (playgroundParticles.overflowMode) {
										case OVERFLOWMODEC.SourceTransform:
											playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.projection.GetPosition(p)+GetOverflow2(stDir, p, playgroundParticles.projection.positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
											break;
										case OVERFLOWMODEC.World:
											playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.projection.GetPosition(p)+GetOverflow2(playgroundParticles.overflowOffset, p, playgroundParticles.projection.positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
											break;
										case OVERFLOWMODEC.SourcePoint:
											playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.projection.GetPosition(p)+GetOverflow2(Vector3Scale(stDir, playgroundParticles.projection.GetNormal(p)), p, playgroundParticles.projection.positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
											break;
										}
									}
								}

						break;
						case SOURCEC.Paint:
							if (playgroundParticles.playgroundCache.rebirth[p] || playgroundParticles.onlySourcePositioning) {
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								if (playgroundParticles.applyInitialLocalVelocity)
									playgroundParticles.playgroundCache.targetDirection[p] = playgroundParticles.paint.GetRotation(p)*Vector3Scale(playgroundParticles.playgroundCache.initialLocalVelocity[p], playgroundParticles.paint.GetNormal(p));
								if (!overflow) {
									playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.paint.GetPosition(p)+playgroundParticles.playgroundCache.scatterPosition[p];
								} else {
									switch (playgroundParticles.overflowMode) {
									case OVERFLOWMODEC.SourceTransform:
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.paint.GetPosition(p)+GetOverflow2(playgroundParticles.paint.GetRotation(p)*playgroundParticles.overflowOffset, p, playgroundParticles.paint.positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.World:
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.paint.GetPosition(p)+GetOverflow2(playgroundParticles.overflowOffset, p, playgroundParticles.paint.positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									case OVERFLOWMODEC.SourcePoint:
										playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.paint.GetPosition(p)+playgroundParticles.paint.GetRotation(p)*GetOverflow2(playgroundParticles.overflowOffset, playgroundParticles.paint.GetNormal(p), p, playgroundParticles.paint.positionLength)+playgroundParticles.playgroundCache.scatterPosition[p];
										break;
									}
								}
								
								// Local space compensation calculation
								if (localSpace && playgroundParticles.applyLocalSpaceMovementCompensation) 
									playgroundParticles.playgroundCache.localSpaceMovementCompensation[p] = playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p];
							}
						break;
						}
					}

					// Set initial particle values if life is 0
					if (playgroundParticles.playgroundCache.life[p]==0) {
						playgroundParticles.playgroundCache.position[p] = playgroundParticles.playgroundCache.targetPosition[p];
						playgroundParticles.playgroundCache.initialColor[p] = lifetimeColor;
					}

					// Particle lifetime, velocity and manipulators
					if (playgroundParticles.playgroundCache.rebirth[p]) {
						
						// Particle is alive
						if (playgroundParticles.playgroundCache.life[p]<=PlaygroundC.globalTime+playgroundParticles.lifetime) {
							
							// Reset particle velocity
							//playgroundParticles.particleCache[p].velocity = Vector3.zero;
							
							// Lifetime size
							if (!playgroundParticles.playgroundCache.changedByPropertySize[p]) {
								if (playgroundParticles.applyLifetimeSize)
									playgroundParticles.playgroundCache.size[p] = playgroundParticles.playgroundCache.initialSize[p]*playgroundParticles.lifetimeSize.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime))*playgroundParticles.scale;
								else
									playgroundParticles.playgroundCache.size[p] = playgroundParticles.playgroundCache.initialSize[p]*playgroundParticles.scale;
							}
							playgroundParticles.particleCache[p].size = !applyMask?playgroundParticles.playgroundCache.size[p]:0;
							

							
							
							// Local Manipulators
							for (m = 0; m<playgroundParticles.manipulators.Count; m++) {
								if (playgroundParticles.manipulators[m].transform!=null) {
									manipulatorPosition = localSpace?playgroundParticles.manipulators[m].transform.localPosition:playgroundParticles.manipulators[m].transform.position;
									manipulatorDistance = playgroundParticles.manipulators[m].strengthDistanceEffect>0?Vector3.Distance (manipulatorPosition, playgroundParticles.playgroundCache.position[p])/playgroundParticles.manipulators[m].strengthDistanceEffect:10f;
									CalculateManipulator(playgroundParticles, playgroundParticles.manipulators[m], p, t, playgroundParticles.playgroundCache.life[p], playgroundParticles.playgroundCache.position[p], manipulatorPosition, manipulatorDistance, localSpace);
								}
							}
							
							// Global Manipulators
							for (m = 0; m<PlaygroundC.reference.manipulators.Count; m++) {
								if (PlaygroundC.reference.manipulators[m].transform!=null) {
									manipulatorPosition = localSpace?PlaygroundC.reference.manipulators[m].transform.localPosition:PlaygroundC.reference.manipulators[m].transform.position;
									manipulatorDistance = PlaygroundC.reference.manipulators[m].strengthDistanceEffect>0?Vector3.Distance (manipulatorPosition, playgroundParticles.playgroundCache.position[p])/PlaygroundC.reference.manipulators[m].strengthDistanceEffect:10f;
									CalculateManipulator(playgroundParticles, PlaygroundC.reference.manipulators[m], p, t, playgroundParticles.playgroundCache.life[p], playgroundParticles.playgroundCache.position[p], manipulatorPosition, manipulatorDistance, localSpace);
								}
							}

							if (!playgroundParticles.onlySourcePositioning) {
								
								// Velocity bending
								if (playgroundParticles.applyVelocityBending) {
									if (playgroundParticles.velocityBendingType==VELOCITYBENDINGTYPEC.SourcePosition) {
										playgroundParticles.playgroundCache.velocity[p] += Vector3.Reflect(
											new Vector3(
											playgroundParticles.playgroundCache.velocity[p].x*playgroundParticles.velocityBending.x,
											playgroundParticles.playgroundCache.velocity[p].y*playgroundParticles.velocityBending.y, 
											playgroundParticles.playgroundCache.velocity[p].z*playgroundParticles.velocityBending.z
											),
											(playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.position[p]).normalized
											)*t;
									} else {
										playgroundParticles.playgroundCache.velocity[p] += Vector3.Reflect(
											new Vector3(
											playgroundParticles.playgroundCache.velocity[p].x*playgroundParticles.velocityBending.x,
											playgroundParticles.playgroundCache.velocity[p].y*playgroundParticles.velocityBending.y, 
											playgroundParticles.playgroundCache.velocity[p].z*playgroundParticles.velocityBending.z
											),
											(playgroundParticles.playgroundCache.previousParticlePosition[p]-playgroundParticles.playgroundCache.position[p]).normalized
											)*t;
									}
								}
								
								// Delta velocity
								if (!playgroundParticles.cameFromNonEmissionFrame && playgroundParticles.calculateDeltaMovement && playgroundParticles.playgroundCache.life[p]==0 && playgroundParticles.source!=SOURCEC.Script) {
									deltaVelocity = playgroundParticles.playgroundCache.targetPosition[p]-(playgroundParticles.playgroundCache.previousTargetPosition[p]);
									playgroundParticles.playgroundCache.velocity[p] += deltaVelocity*playgroundParticles.deltaMovementStrength;
								}

								// Set previous target position (used by delta velocity & local space movement compensation)
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								
								// Gravity
								playgroundParticles.playgroundCache.velocity[p] -= playgroundParticles.gravity*t;
								
								// Lifetime additive velocity
								if (playgroundParticles.applyLifetimeVelocity) 
									playgroundParticles.playgroundCache.velocity[p] += playgroundParticles.lifetimeVelocity.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime))*t;
								
								// Damping, max velocity, constraints and final positioning
								if (playgroundParticles.playgroundCache.velocity[p]!=zero) {

									// Max velocity
									if (playgroundParticles.playgroundCache.velocity[p].sqrMagnitude>playgroundParticles.maxVelocity)
										playgroundParticles.playgroundCache.velocity[p] = Vector3.ClampMagnitude(playgroundParticles.playgroundCache.velocity[p], playgroundParticles.maxVelocity);

									// Damping
									if (playgroundParticles.damping>0)
										playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], zero, playgroundParticles.damping*t);
									
									// Axis constraints
									if (playgroundParticles.axisConstraints.x)
										playgroundParticles.playgroundCache.velocity[p].x = 0;
									if (playgroundParticles.axisConstraints.y)
										playgroundParticles.playgroundCache.velocity[p].y = 0;
									if (playgroundParticles.axisConstraints.z)
										playgroundParticles.playgroundCache.velocity[p].z = 0;
									
									
									// Set calculated collision position 
									playgroundParticles.playgroundCache.collisionParticlePosition[p] = playgroundParticles.playgroundCache.previousParticlePosition[p];

									
									// Relocate
									playgroundParticles.playgroundCache.position[p] += playgroundParticles.playgroundCache.velocity[p]*t;
									if (playgroundParticles.applyLocalSpaceMovementCompensation) {
										if (!playgroundParticles.applyMovementCompensationLifetimeStrength)
											playgroundParticles.playgroundCache.position[p] += playgroundParticles.playgroundCache.localSpaceMovementCompensation[p];
										else
											playgroundParticles.playgroundCache.position[p] += playgroundParticles.playgroundCache.localSpaceMovementCompensation[p]*playgroundParticles.movementCompensationLifetimeStrength.Evaluate(Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
									}

										// Set particle velocity to be able to stretch towards movement
										if (playgroundParticles.renderModeStretch) {
											if (playgroundParticles.applyLifetimeStretching)
												playgroundParticles.particleCache[p].velocity = Vector3.Slerp (playgroundParticles.particleCache[p].velocity, playgroundParticles.playgroundCache.position[p]-playgroundParticles.playgroundCache.previousParticlePosition[p], t*playgroundParticles.stretchSpeed)*playgroundParticles.stretchLifetime.Evaluate(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime);
											else
												playgroundParticles.particleCache[p].velocity = Vector3.Slerp (playgroundParticles.particleCache[p].velocity, playgroundParticles.playgroundCache.position[p]-playgroundParticles.playgroundCache.previousParticlePosition[p], t*playgroundParticles.stretchSpeed);
										}

								}
								
							} else {
								
								// Only Source Positioning
								// Set particle velocity to be able to stretch towards movement
								if (playgroundParticles.renderModeStretch) {
									if (playgroundParticles.applyLifetimeStretching)
										playgroundParticles.particleCache[p].velocity = Vector3.Slerp (playgroundParticles.particleCache[p].velocity, playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p], t*playgroundParticles.stretchSpeed)*playgroundParticles.stretchLifetime.Evaluate(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime);
									else
										playgroundParticles.particleCache[p].velocity = Vector3.Slerp (playgroundParticles.particleCache[p].velocity, playgroundParticles.playgroundCache.targetPosition[p]-playgroundParticles.playgroundCache.previousTargetPosition[p], t*playgroundParticles.stretchSpeed);
								}
								playgroundParticles.playgroundCache.previousTargetPosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
								if (playgroundParticles.source!=SOURCEC.Script)
									playgroundParticles.playgroundCache.position[p] = playgroundParticles.playgroundCache.targetPosition[p];
								
							}

							// Rotation
							if (!playgroundParticles.rotateTowardsDirection)
								playgroundParticles.playgroundCache.rotation[p] += playgroundParticles.playgroundCache.rotationSpeed[p]*t;
							else {
								playgroundParticles.playgroundCache.rotation[p] = playgroundParticles.playgroundCache.initialRotation[p]+SignedAngle(
									up, 
									playgroundParticles.playgroundCache.position[p]-playgroundParticles.playgroundCache.previousParticlePosition[p],
									playgroundParticles.rotationNormal
								);
							}
								
							playgroundParticles.particleCache[p].rotation = playgroundParticles.playgroundCache.rotation[p];

								// Set previous particle position
								playgroundParticles.playgroundCache.previousParticlePosition[p] = playgroundParticles.playgroundCache.position[p];

							// Send timed event
							if (hasTimerEvent)
								playgroundParticles.SendEvent(EVENTTYPEC.Time, p);
						}


						// Calculate lifetime
						evaluatedLife = (PlaygroundC.globalTime-playgroundParticles.playgroundCache.birth[p])/playgroundParticles.lifetime;
						
						// Lifetime
						if (playgroundParticles.playgroundCache.life[p]<playgroundParticles.lifetime) {
							playgroundParticles.playgroundCache.life[p] = playgroundParticles.lifetime*evaluatedLife;
							playgroundParticles.particleCache[p].lifetime = Mathf.Clamp(playgroundParticles.lifetime - playgroundParticles.playgroundCache.life[p], .1f, playgroundParticles.lifetime);
						} else {
							
							// Loop exceeded
							if (!playgroundParticles.loop && PlaygroundC.globalTime>playgroundParticles.simulationStarted+playgroundParticles.lifetime-.01f) {
								playgroundParticles.loopExceeded = true;
								
								if (playgroundParticles.disableOnDone && playgroundParticles.loopExceededOnParticle==p && evaluatedLife>2)
									playgroundParticles.queueEmissionHalt = true;
								if (playgroundParticles.loopExceededOnParticle==-1)
									playgroundParticles.loopExceededOnParticle = p;
							}
							
							// Send death event
							if (hasEvent) {
								playgroundParticles.SendEvent(EVENTTYPEC.Death, p);
							}
							
							// New cycle begins
							if (PlaygroundC.globalTime>=playgroundParticles.playgroundCache.birth[p]+playgroundParticles.playgroundCache.birthDelay[p] && !playgroundParticles.loopExceeded && playgroundParticles.source!=SOURCEC.Script) {
								if (!playgroundParticles.playgroundCache.changedByPropertyDeath[p] || playgroundParticles.playgroundCache.changedByPropertyDeath[p] && PlaygroundC.globalTime>playgroundParticles.playgroundCache.death[p])
									Rebirth(playgroundParticles, p, playgroundParticles.internalRandom02);
								else {
									playgroundParticles.playgroundCache.position[p] = PlaygroundC.initialTargetPosition;
								}
							} else {
								playgroundParticles.playgroundCache.position[p] = PlaygroundC.initialTargetPosition;
							}
							
						}

					} else {
						
						// Particle is set to not rebirth
						playgroundParticles.playgroundCache.targetPosition[p] = PlaygroundC.initialTargetPosition;
						playgroundParticles.playgroundCache.position[p] = PlaygroundC.initialTargetPosition;
					}

					// Set particle position if no sync
					if (!playgroundParticles.syncPositionsOnMainThread)
						playgroundParticles.particleCache[p].position = playgroundParticles.playgroundCache.position[p];
				}

				}
				playgroundParticles.cameFromNonEmissionFrame = false;
				playgroundParticles.isDoneThread = true;
			});

			// Turbulence
			if (!playgroundParticles.onlySourcePositioning && playgroundParticles.turbulenceStrength>0 && playgroundParticles.turbulenceType!=TURBULENCETYPE.None) {

				float strengthMultiplier = 1f;
				float zeroFixX = 1f;
				float zeroFixY = 2f; 
				float zeroFixZ = 3f; 

				// Simplex
				if (playgroundParticles.turbulenceType==TURBULENCETYPE.Simplex) {
					if (playgroundParticles.turbulenceSimplex==null)
						playgroundParticles.turbulenceSimplex = new SimplexNoise();
					PlaygroundC.RunAsync(()=>{

						for (int p = 0; p<playgroundParticles.particleCount; p++) {
							if (playgroundParticles.playgroundCache.rebirth[p]) {
								if (playgroundParticles.turbulenceTimeScale>0) {
									if (playgroundParticles.turbulenceApplyLifetimeStrength)
										strengthMultiplier = playgroundParticles.turbulenceLifetimeStrength.Evaluate (Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
									if (strengthMultiplier>0) {
									if (!playgroundParticles.axisConstraints.x)
										playgroundParticles.playgroundCache.velocity[p].x += (float)playgroundParticles.turbulenceSimplex.noise(
										(playgroundParticles.playgroundCache.position[p].x+zeroFixX)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].y+zeroFixY)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].z+zeroFixZ)*playgroundParticles.turbulenceScale,
										PlaygroundC.globalTime*playgroundParticles.turbulenceTimeScale
										)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									if (!playgroundParticles.axisConstraints.y)
										playgroundParticles.playgroundCache.velocity[p].y += (float)playgroundParticles.turbulenceSimplex.noise(
										(playgroundParticles.playgroundCache.position[p].y+zeroFixY)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].x+zeroFixX)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].z+zeroFixZ)*playgroundParticles.turbulenceScale,
										PlaygroundC.globalTime*playgroundParticles.turbulenceTimeScale
										)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									if (!playgroundParticles.axisConstraints.z)
										playgroundParticles.playgroundCache.velocity[p].z += (float)playgroundParticles.turbulenceSimplex.noise(
										(playgroundParticles.playgroundCache.position[p].z+zeroFixZ)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].x+zeroFixX)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].y+zeroFixY)*playgroundParticles.turbulenceScale,
										PlaygroundC.globalTime*playgroundParticles.turbulenceTimeScale
										)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									}
								} else {
									if (strengthMultiplier>0) {
									if (!playgroundParticles.axisConstraints.x)
										playgroundParticles.playgroundCache.velocity[p].x += (float)playgroundParticles.turbulenceSimplex.noise(
										(playgroundParticles.playgroundCache.position[p].x+zeroFixX)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].y+zeroFixY)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].z+zeroFixZ)*playgroundParticles.turbulenceScale
										)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									if (!playgroundParticles.axisConstraints.y)
										playgroundParticles.playgroundCache.velocity[p].y += (float)playgroundParticles.turbulenceSimplex.noise(
										(playgroundParticles.playgroundCache.position[p].y+zeroFixY)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].x+zeroFixX)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].z+zeroFixZ)*playgroundParticles.turbulenceScale
										)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									if (!playgroundParticles.axisConstraints.z)
										playgroundParticles.playgroundCache.velocity[p].z += (float)playgroundParticles.turbulenceSimplex.noise(
										(playgroundParticles.playgroundCache.position[p].z+zeroFixZ)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].x+zeroFixX)*playgroundParticles.turbulenceScale,
										(playgroundParticles.playgroundCache.position[p].y+zeroFixY)*playgroundParticles.turbulenceScale
										)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									}
								}
							}
						}
					});
				}

				// Perlin
				if (playgroundParticles.turbulenceType==TURBULENCETYPE.Perlin) {
					PlaygroundC.RunAsync(()=>{
						for (int p = 0; p<playgroundParticles.particleCount; p++) {
							if (playgroundParticles.playgroundCache.rebirth[p]) {
								if (playgroundParticles.turbulenceStrength>0) {
									if (playgroundParticles.turbulenceApplyLifetimeStrength)
										strengthMultiplier = playgroundParticles.turbulenceLifetimeStrength.Evaluate (Mathf.Clamp01(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime));
									if (strengthMultiplier>0) {
									if (!playgroundParticles.axisConstraints.x)
										playgroundParticles.playgroundCache.velocity[p].x += (Mathf.PerlinNoise (
											PlaygroundC.globalTime*playgroundParticles.turbulenceTimeScale,
											playgroundParticles.playgroundCache.position[p].z*playgroundParticles.turbulenceScale
										)-.5f)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									if (!playgroundParticles.axisConstraints.y)
										playgroundParticles.playgroundCache.velocity[p].y += (Mathf.PerlinNoise (
											PlaygroundC.globalTime*playgroundParticles.turbulenceTimeScale,
											playgroundParticles.playgroundCache.position[p].x*playgroundParticles.turbulenceScale

										)-.5f)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									if (!playgroundParticles.axisConstraints.z)
										playgroundParticles.playgroundCache.velocity[p].z += (Mathf.PerlinNoise (
											PlaygroundC.globalTime*playgroundParticles.turbulenceTimeScale,
											playgroundParticles.playgroundCache.position[p].y*playgroundParticles.turbulenceScale

										)-.5f)*playgroundParticles.turbulenceStrength*t*strengthMultiplier;
									}
								}
							}
						}
					});
				}
			}

		}

		public static void Collisions (PlaygroundParticlesC playgroundParticles) {

			// Particle collisions (must currently run on main-thread due to the Physics.Raycast dependency)
			if (!playgroundParticles.onlySourcePositioning && playgroundParticles.collisionRadius>0) {
				Ray ray = new Ray();
				float distance;
				bool is3d = playgroundParticles.collisionType==COLLISIONTYPEC.Physics3D;
				RaycastHit hitInfo;
				RaycastHit2D hitInfo2D;
				bool hasEvents = playgroundParticles.events.Count>0;
				Vector3 preCollisionVelocity;

				// Prepare the infinite collision planes
				if (playgroundParticles.collision && playgroundParticles.collisionRadius>0 && playgroundParticles.colliders.Count>0) {
					for (int c = 0; c<playgroundParticles.colliders.Count; c++) {
						playgroundParticles.colliders[c].UpdatePlane();
					}
				}

				for (int p = 0; p<playgroundParticles.particleCount; p++) {

					if (playgroundParticles.playgroundCache.life[p]==0 || playgroundParticles.playgroundCache.life[p]>=playgroundParticles.lifetime) continue;

					// Playground Plane colliders (never exceed these)
					for (int c = 0; c<playgroundParticles.colliders.Count; c++) {
						if (playgroundParticles.colliders[c].enabled && playgroundParticles.colliders[c].transform && !playgroundParticles.colliders[c].plane.GetSide(playgroundParticles.playgroundCache.position[p])) {
							
							// Set particle to location
							ray.origin = playgroundParticles.playgroundCache.position[p];
							ray.direction = playgroundParticles.colliders[c].plane.normal;
							if (playgroundParticles.colliders[c].plane.Raycast(ray, out distance))
								playgroundParticles.playgroundCache.position[p] = ray.GetPoint(distance);

							// Store velocity before collision
							preCollisionVelocity = playgroundParticles.playgroundCache.velocity[p];

							// Reflect particle
							playgroundParticles.playgroundCache.velocity[p] = Vector3.Reflect(playgroundParticles.playgroundCache.velocity[p], playgroundParticles.colliders[c].plane.normal+RandomVector3(playgroundParticles.internalRandom03, playgroundParticles.bounceRandomMin, playgroundParticles.bounceRandomMax))*playgroundParticles.bounciness;
							
							// Apply lifetime loss
							if (playgroundParticles.lifetimeLoss>0) {
								playgroundParticles.playgroundCache.birth[p] -= playgroundParticles.playgroundCache.life[p]/(1f-playgroundParticles.lifetimeLoss);
								playgroundParticles.playgroundCache.changedByPropertyDeath[p] = true;
							}

							// Send event
							if (hasEvents)
								playgroundParticles.SendEvent(EVENTTYPEC.Collision, p, preCollisionVelocity, playgroundParticles.colliders[c].transform);
						}
					}
					
					// Colliders in scene
					if (playgroundParticles.playgroundCache.velocity[p].magnitude>PlaygroundC.collisionSleepVelocity) {

						// Collide by checking for potential passed collider in the direction of this particle's velocity from the previous frame
						if (is3d) {
							if (Physics.Raycast(
								playgroundParticles.playgroundCache.collisionParticlePosition[p], 
								(playgroundParticles.playgroundCache.position[p]-playgroundParticles.playgroundCache.collisionParticlePosition[p]).normalized, 
								out hitInfo, 
								Vector3.Distance(playgroundParticles.playgroundCache.collisionParticlePosition[p], playgroundParticles.playgroundCache.position[p])+playgroundParticles.collisionRadius, 
								playgroundParticles.collisionMask)) 
							{
								
								// Set particle to location
								playgroundParticles.playgroundCache.position[p] = playgroundParticles.playgroundCache.collisionParticlePosition[p];

								// Store velocity before collision
								preCollisionVelocity = playgroundParticles.playgroundCache.velocity[p];

								// Reflect particle
								playgroundParticles.playgroundCache.velocity[p] = Vector3.Reflect(playgroundParticles.playgroundCache.velocity[p], hitInfo.normal+RandomVector3(playgroundParticles.internalRandom03, playgroundParticles.bounceRandomMin, playgroundParticles.bounceRandomMax))*playgroundParticles.bounciness;
								
								// Apply lifetime loss
								if (playgroundParticles.lifetimeLoss>0) {
									playgroundParticles.playgroundCache.birth[p] -= playgroundParticles.playgroundCache.life[p]/(1f-playgroundParticles.lifetimeLoss);
									playgroundParticles.playgroundCache.changedByPropertyDeath[p] = true;
								}
								
								// Add force to rigidbody
								if (playgroundParticles.affectRigidbodies && hitInfo.rigidbody)
									hitInfo.rigidbody.AddForceAtPosition(playgroundParticles.playgroundCache.velocity[p]*playgroundParticles.mass, playgroundParticles.playgroundCache.position[p]);

								// Send event
								if (hasEvents)
									playgroundParticles.SendEvent(EVENTTYPEC.Collision, p, preCollisionVelocity, hitInfo.transform, hitInfo.collider);
							}
						} else {
							hitInfo2D = Physics2D.Raycast(
								playgroundParticles.playgroundCache.collisionParticlePosition[p], 
								(playgroundParticles.playgroundCache.position[p]-playgroundParticles.playgroundCache.collisionParticlePosition[p]).normalized, 
								Vector3.Distance(playgroundParticles.playgroundCache.collisionParticlePosition[p], playgroundParticles.playgroundCache.position[p])+playgroundParticles.collisionRadius, 
								playgroundParticles.collisionMask,
								playgroundParticles.minCollisionDepth,
								playgroundParticles.maxCollisionDepth
							);
							if (hitInfo2D.collider!=null) {
								
								// Set particle to location
								playgroundParticles.playgroundCache.position[p] = playgroundParticles.playgroundCache.collisionParticlePosition[p];

								// Store velocity before collision
								preCollisionVelocity = playgroundParticles.playgroundCache.velocity[p];

								// Reflect particle
								playgroundParticles.playgroundCache.velocity[p] = Vector3.Reflect(playgroundParticles.playgroundCache.velocity[p], (Vector3)hitInfo2D.normal+RandomVector3(playgroundParticles.internalRandom03, playgroundParticles.bounceRandomMin, playgroundParticles.bounceRandomMax))*playgroundParticles.bounciness;
								
								// Apply lifetime loss
								if (playgroundParticles.lifetimeLoss>0) {
									playgroundParticles.playgroundCache.birth[p] -= playgroundParticles.playgroundCache.life[p]/(1f-playgroundParticles.lifetimeLoss);
									playgroundParticles.playgroundCache.changedByPropertyDeath[p] = true;
								}
								
								// Add force to rigidbody
								if (playgroundParticles.affectRigidbodies && hitInfo2D.rigidbody)
									hitInfo2D.rigidbody.AddForceAtPosition(playgroundParticles.playgroundCache.velocity[p]*playgroundParticles.mass, playgroundParticles.playgroundCache.position[p]);

								// Send event
								if (hasEvents)
									playgroundParticles.SendEvent(EVENTTYPEC.Collision, p, preCollisionVelocity, hitInfo2D.transform, hitInfo2D.collider);
							}
						}
					} else {
						playgroundParticles.playgroundCache.velocity[p] = Vector3.zero;
					}
				}
			}
		}

		// Returns the offset as a remainder from a point, constructed for threading
		public static Vector3 GetOverflow2 (Vector3 overflow, int currentVal, int maxVal) {
			float iteration = (currentVal/maxVal);
			return new Vector3(
				overflow.x*iteration,
				overflow.y*iteration,
				overflow.z*iteration
			);
		}

		// Returns the offset with direction as a remainder from a point, constructed for threading
		public static Vector3 GetOverflow2 (Vector3 overflow, Vector3 direction, int currentVal, int maxVal) {
			float iteration = (currentVal/maxVal);
			return new Vector3(
				direction.x*overflow.x*iteration,
				direction.y*overflow.y*iteration,
			 	direction.z*overflow.z*iteration
			);
		}

		public static Vector3 Vector3Scale (Vector3 v1, Vector3 v2) {
			return new Vector3(v1.x*v2.x,v1.y*v2.y,v1.z*v2.z);
		}

		// Calculate the effect from a manipulator
		public static void CalculateManipulator (PlaygroundParticlesC playgroundParticles, ManipulatorObjectC thisManipulator, int p, float t, float life, Vector3 particlePosition, Vector3 manipulatorPosition, float manipulatorDistance, bool localSpace) {
			if (thisManipulator.enabled && thisManipulator.transform.available && thisManipulator.strength!=0 && thisManipulator.willAffect && thisManipulator.LifetimeFilter(life)) {
				if (!playgroundParticles.onlySourcePositioning) {
					// Attractors
					if (thisManipulator.type==MANIPULATORTYPEC.Attractor) {
						if (thisManipulator.Contains(particlePosition, manipulatorPosition)) {
							playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], (manipulatorPosition-particlePosition)*(thisManipulator.strength/manipulatorDistance), t*(thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
						}
					} else
						
					// Attractors Gravitational
					if (thisManipulator.type==MANIPULATORTYPEC.AttractorGravitational) {
						if (thisManipulator.Contains(particlePosition, manipulatorPosition)) {
							playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], (manipulatorPosition-particlePosition)*thisManipulator.strength/manipulatorDistance, t/thisManipulator.strengthSmoothing);
						}
					} else
						
					// Repellents 
					if (thisManipulator.type==MANIPULATORTYPEC.Repellent) {
						if (thisManipulator.Contains(particlePosition, manipulatorPosition)) {
							playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], (particlePosition-manipulatorPosition)*(thisManipulator.strength/manipulatorDistance), t*(thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
						}
					} else

					// Vortex
					if (thisManipulator.type==MANIPULATORTYPEC.Vortex) {
						if (thisManipulator.Contains(particlePosition, manipulatorPosition)) {
							playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], ((manipulatorPosition-particlePosition)*thisManipulator.strength/manipulatorDistance)-Vector3.Cross(thisManipulator.transform.up, (manipulatorPosition-particlePosition))*thisManipulator.strength/manipulatorDistance, (t*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
						}
					}
				}
				
				// Properties
				if (thisManipulator.type==MANIPULATORTYPEC.Property) {
					PropertyManipulator(playgroundParticles, thisManipulator, thisManipulator.property, p, t, particlePosition, manipulatorPosition, manipulatorDistance, localSpace);
				} else
				
				// Combined
				if (thisManipulator.type==MANIPULATORTYPEC.Combined) {
					for (int i = 0; i<thisManipulator.properties.Count; i++)
						PropertyManipulator(playgroundParticles, thisManipulator, thisManipulator.properties[i], p, t, particlePosition, manipulatorPosition, manipulatorDistance, localSpace);
				}
			}
		}
		
		// Calculate the effect from manipulator properties
		public static void PropertyManipulator (PlaygroundParticlesC playgroundParticles, ManipulatorObjectC thisManipulator, ManipulatorPropertyC thisManipulatorProperty, int p, float t, Vector3 particlePosition, Vector3 manipulatorPosition, float manipulatorDistance, bool localSpace) {
			if (thisManipulator.Contains(particlePosition, manipulatorPosition)) {
				switch (thisManipulatorProperty.type) {
					
					// Velocity Property
					case MANIPULATORPROPERTYTYPEC.Velocity:
					if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None)
						playgroundParticles.playgroundCache.velocity[p] = thisManipulatorProperty.useLocalRotation?
							thisManipulatorProperty.localVelocity
							:
							thisManipulatorProperty.velocity;
					else
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], thisManipulatorProperty.useLocalRotation?
							thisManipulatorProperty.localVelocity
							:
						thisManipulatorProperty.velocity, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
					break;
					
					// Additive Velocity Property
					case MANIPULATORPROPERTYTYPEC.AdditiveVelocity:
					if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None)
						playgroundParticles.playgroundCache.velocity[p] += thisManipulatorProperty.useLocalRotation?
							thisManipulatorProperty.localVelocity*(t*thisManipulatorProperty.strength*thisManipulator.strength)
							:
							thisManipulatorProperty.velocity*(t*thisManipulatorProperty.strength*thisManipulator.strength);
					else
						playgroundParticles.playgroundCache.velocity[p] += Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], thisManipulatorProperty.useLocalRotation?
						    thisManipulatorProperty.localVelocity 
						    : 
						    thisManipulatorProperty.velocity,
						(t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
					break;
					
					// Color Property
					case MANIPULATORPROPERTYTYPEC.Color:
					Color staticColor;
					if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None) {
						if (thisManipulatorProperty.keepColorAlphas) {
							staticColor = thisManipulatorProperty.color;
							staticColor.a = Mathf.Clamp(playgroundParticles.lifetimeColor.Evaluate(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime).a, 0, staticColor.a);
							playgroundParticles.particleCache[p].color = staticColor;
						} else playgroundParticles.particleCache[p].color = thisManipulatorProperty.color;
					} else {
						if (thisManipulatorProperty.keepColorAlphas) {
							staticColor = thisManipulatorProperty.color;
							staticColor.a = Mathf.Clamp(playgroundParticles.lifetimeColor.Evaluate(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime).a, 0, staticColor.a);
							playgroundParticles.particleCache[p].color = Color.Lerp(playgroundParticles.particleCache[p].color, staticColor, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
						} else playgroundParticles.particleCache[p].color = Color.Lerp(playgroundParticles.particleCache[p].color, thisManipulatorProperty.color, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
						playgroundParticles.playgroundCache.changedByPropertyColorLerp[p] = true;
					}
					
					// Only color in range of manipulator boundaries
					if (!thisManipulatorProperty.onlyColorInRange)
						playgroundParticles.playgroundCache.changedByPropertyColor[p] = true;

					// Keep alpha of original color
					if (thisManipulatorProperty.keepColorAlphas)
						playgroundParticles.playgroundCache.changedByPropertyColorKeepAlpha[p] = true;
					
					// Set color pairing key
					else if (playgroundParticles.playgroundCache.propertyColorId[p] != thisManipulator.manipulatorId) {
						playgroundParticles.playgroundCache.propertyColorId[p] = thisManipulator.manipulatorId;
					}
					break;
					
					// Lifetime Color Property
					case MANIPULATORPROPERTYTYPEC.LifetimeColor:
					if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None) {
						playgroundParticles.particleCache[p].color = thisManipulatorProperty.lifetimeColor.Evaluate(playgroundParticles.lifetime>0?playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime:0);
					} else {
						playgroundParticles.particleCache[p].color = Color.Lerp(playgroundParticles.particleCache[p].color, thisManipulatorProperty.lifetimeColor.Evaluate(playgroundParticles.lifetime>0?playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime:0), (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
						playgroundParticles.playgroundCache.changedByPropertyColorLerp[p] = true;
					}
					
					// Only color in range of manipulator boundaries
					if (!thisManipulatorProperty.onlyColorInRange)
						playgroundParticles.playgroundCache.changedByPropertyColor[p] = true;
					
					// Set color pairing key
					else if (playgroundParticles.playgroundCache.propertyColorId[p] != thisManipulator.manipulatorId) {
						playgroundParticles.playgroundCache.propertyColorId[p] = thisManipulator.manipulatorId;
					}
					break;
					
					// Size Property
					case MANIPULATORPROPERTYTYPEC.Size:
					if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None)
						playgroundParticles.playgroundCache.size[p] = thisManipulatorProperty.size;
					else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp)
						playgroundParticles.playgroundCache.size[p] = Mathf.Lerp(playgroundParticles.playgroundCache.size[p], thisManipulatorProperty.size, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
					else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Linear)
						playgroundParticles.playgroundCache.size[p] = Mathf.MoveTowards(playgroundParticles.playgroundCache.size[p], thisManipulatorProperty.size, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
					playgroundParticles.playgroundCache.changedByPropertySize[p] = true;
					break;
					
					// Target Property
					case MANIPULATORPROPERTYTYPEC.Target:
					if (thisManipulatorProperty.targets.Count>0 && thisManipulatorProperty.targets[thisManipulatorProperty.targetPointer].available) {
						
						
						// Set target pointer
						if (playgroundParticles.playgroundCache.propertyId[p] != thisManipulator.manipulatorId) {

							playgroundParticles.playgroundCache.propertyTarget[p] = thisManipulatorProperty.targetPointer;
							thisManipulatorProperty.targetPointer++; thisManipulatorProperty.targetPointer=thisManipulatorProperty.targetPointer%thisManipulatorProperty.targets.Count;
							playgroundParticles.playgroundCache.propertyId[p] = thisManipulator.manipulatorId;
						}

						// Teleport or lerp to position based on transition type
						if (playgroundParticles.playgroundCache.propertyId[p] == thisManipulator.manipulatorId && thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].available) {
							if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None)
								playgroundParticles.playgroundCache.position[p] = localSpace? 
									thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].localPosition
								: 
									thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].position;
							else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp) {
								playgroundParticles.playgroundCache.position[p] = localSpace? 
										Vector3.Lerp(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].localPosition, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing)
								:
											Vector3.Lerp(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].position, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Linear) {
								playgroundParticles.playgroundCache.position[p] = localSpace?
										Vector3.MoveTowards(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].localPosition, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing)
								:
											Vector3.MoveTowards(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].position, (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							}
							
							// This particle was changed by a target property
							playgroundParticles.playgroundCache.changedByPropertyTarget[p] = true;
						}
					}
					break;
					
					// Death Property
					case MANIPULATORPROPERTYTYPEC.Death:
					if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None)
						playgroundParticles.playgroundCache.life[p] = playgroundParticles.lifetime;
					else
						playgroundParticles.playgroundCache.birth[p] -= (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing;
					
					// This particle was changed by a death property
					playgroundParticles.playgroundCache.changedByPropertyDeath[p] = true;
					break;
					
					
					// Attractors
					case MANIPULATORPROPERTYTYPEC.Attractor:
					if (!playgroundParticles.onlySourcePositioning) {
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], (manipulatorPosition-particlePosition)*((thisManipulatorProperty.strength*thisManipulator.strength)/manipulatorDistance), t*((thisManipulatorProperty.strength*thisManipulator.strength)/manipulatorDistance)/thisManipulator.strengthSmoothing);
					}
					break;
					
					// Attractors Gravitational
					case MANIPULATORPROPERTYTYPEC.Gravitational:
					if (!playgroundParticles.onlySourcePositioning) {
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], (manipulatorPosition-particlePosition)*(thisManipulatorProperty.strength*thisManipulator.strength)/manipulatorDistance, t/thisManipulator.strengthSmoothing);
					}
					break;
					
					// Repellents 
					case MANIPULATORPROPERTYTYPEC.Repellent:
					if (!playgroundParticles.onlySourcePositioning) {
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], (particlePosition-manipulatorPosition)*((thisManipulatorProperty.strength*thisManipulator.strength)/manipulatorDistance), t*((thisManipulatorProperty.strength*thisManipulator.strength)/manipulatorDistance)/thisManipulator.strengthSmoothing);
					}
					break;

					// Vortex
					case MANIPULATORPROPERTYTYPEC.Vortex:
					if (!playgroundParticles.onlySourcePositioning) {
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], ((manipulatorPosition-particlePosition)*(thisManipulator.strength*thisManipulatorProperty.strength)/manipulatorDistance)-Vector3.Cross(thisManipulator.transform.up, (manipulatorPosition-particlePosition))*(thisManipulator.strength*thisManipulatorProperty.strength)/manipulatorDistance, (t*(thisManipulator.strength*thisManipulatorProperty.strength)/manipulatorDistance)/thisManipulator.strengthSmoothing);
					}
					break;

					// Mesh Target
					case MANIPULATORPROPERTYTYPEC.MeshTarget:
					if (!playgroundParticles.onlySourcePositioning && thisManipulatorProperty.meshTarget.initialized) {

						// Set target pointer
						if (playgroundParticles.playgroundCache.propertyId[p] != thisManipulator.manipulatorId) {
							playgroundParticles.playgroundCache.propertyTarget[p] = thisManipulatorProperty.targetSortingList[thisManipulatorProperty.targetPointer];
							thisManipulatorProperty.targetPointer++; thisManipulatorProperty.targetPointer=thisManipulatorProperty.targetPointer%thisManipulatorProperty.meshTarget.vertexPositions.Length;
							playgroundParticles.playgroundCache.propertyId[p] = thisManipulator.manipulatorId;
						}

						// Teleport or lerp to position based on transition type
						if (playgroundParticles.playgroundCache.propertyId[p] == thisManipulator.manipulatorId) {
							if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None) {
								playgroundParticles.playgroundCache.position[p] = thisManipulatorProperty.meshTargetMatrix.MultiplyPoint3x4(thisManipulatorProperty.meshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.meshTarget.vertexPositions.Length]);
								playgroundParticles.playgroundCache.velocity[p] = Vector3.zero;
							} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp) {
									playgroundParticles.playgroundCache.position[p] = Vector3.Lerp(particlePosition, thisManipulatorProperty.meshTargetMatrix.MultiplyPoint3x4(thisManipulatorProperty.meshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.meshTarget.vertexPositions.Length]), (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Linear) {
									playgroundParticles.playgroundCache.position[p] = Vector3.MoveTowards(particlePosition, thisManipulatorProperty.meshTargetMatrix.MultiplyPoint3x4(thisManipulatorProperty.meshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.meshTarget.vertexPositions.Length]), (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							}
							
							// This particle was changed by a target property
							playgroundParticles.playgroundCache.changedByPropertyTarget[p] = true;
						}

					}
					break;
				
					// Skinned Mesh Target
					case MANIPULATORPROPERTYTYPEC.SkinnedMeshTarget:
					if (!playgroundParticles.onlySourcePositioning && thisManipulatorProperty.skinnedMeshTarget.initialized) {
							
						// Set target pointer
						if (playgroundParticles.playgroundCache.propertyId[p] != thisManipulator.manipulatorId) {
							playgroundParticles.playgroundCache.propertyTarget[p] = thisManipulatorProperty.targetSortingList[thisManipulatorProperty.targetPointer];
							thisManipulatorProperty.targetPointer++; thisManipulatorProperty.targetPointer=thisManipulatorProperty.targetPointer%thisManipulatorProperty.skinnedMeshTarget.vertexPositions.Length;
							playgroundParticles.playgroundCache.propertyId[p] = thisManipulator.manipulatorId;
						}

						// Teleport or lerp to position based on transition type
						if (playgroundParticles.playgroundCache.propertyId[p] == thisManipulator.manipulatorId) {
							if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.None) {
								playgroundParticles.playgroundCache.position[p] = thisManipulatorProperty.skinnedMeshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.skinnedMeshTarget.vertexPositions.Length];
								playgroundParticles.playgroundCache.velocity[p] = Vector3.zero;
							} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp) {
									playgroundParticles.playgroundCache.position[p] = Vector3.Lerp(particlePosition, thisManipulatorProperty.skinnedMeshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.skinnedMeshTarget.vertexPositions.Length], t*(thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Linear) {
									playgroundParticles.playgroundCache.position[p] = Vector3.MoveTowards(particlePosition, thisManipulatorProperty.skinnedMeshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.skinnedMeshTarget.vertexPositions.Length], t*(thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							}
							
							// This particle was changed by a target property
							playgroundParticles.playgroundCache.changedByPropertyTarget[p] = true;
						}
					}
					break;
				}
				
				playgroundParticles.playgroundCache.changedByProperty[p] = true;
				
			} else {
				
				// Handle colors outside of property manipulator range
				if (playgroundParticles.playgroundCache.propertyColorId[p] == thisManipulator.transform.GetInstanceID() && (thisManipulatorProperty.type == MANIPULATORPROPERTYTYPEC.Color || thisManipulatorProperty.type == MANIPULATORPROPERTYTYPEC.LifetimeColor)) {

					// Lerp back color with previous set key
					if (playgroundParticles.playgroundCache.changedByPropertyColorLerp[p] && thisManipulatorProperty.transition != MANIPULATORPROPERTYTRANSITIONC.None && thisManipulatorProperty.onlyColorInRange)
						playgroundParticles.particleCache[p].color = Color.Lerp(playgroundParticles.particleCache[p].color, playgroundParticles.lifetimeColor.Evaluate(playgroundParticles.playgroundCache.life[p]/playgroundParticles.lifetime), t*thisManipulatorProperty.strength*thisManipulator.strength);
				}

				// Position onto targets when outside of range
				if (!playgroundParticles.onlySourcePositioning && !thisManipulatorProperty.onlyPositionInRange && thisManipulatorProperty.transition != MANIPULATORPROPERTYTRANSITIONC.None) {

					// Target positioning outside of range
					if (thisManipulatorProperty.type == MANIPULATORPROPERTYTYPEC.Target) {
						if (thisManipulatorProperty.targets.Count>0 && thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count]!=null) {
							if (playgroundParticles.playgroundCache.changedByPropertyTarget[p] && thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].available && playgroundParticles.playgroundCache.propertyId[p] == thisManipulator.transform.GetInstanceID()) {
								if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp)
									playgroundParticles.playgroundCache.position[p] = localSpace?
										Vector3.Lerp(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].localPosition, t*(thisManipulatorProperty.strength*thisManipulator.strength)/thisManipulator.strengthSmoothing)
										:	
										Vector3.Lerp(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].position, t*(thisManipulatorProperty.strength*thisManipulator.strength)/thisManipulator.strengthSmoothing);
								else
									playgroundParticles.playgroundCache.position[p] = localSpace?
										Vector3.MoveTowards(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].localPosition, t*(thisManipulatorProperty.strength*thisManipulator.strength)/thisManipulator.strengthSmoothing)
										:
										Vector3.MoveTowards(particlePosition, thisManipulatorProperty.targets[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.targets.Count].position, t*(thisManipulatorProperty.strength*thisManipulator.strength)/thisManipulator.strengthSmoothing);
								
								if (thisManipulatorProperty.zeroVelocityStrength>0)
									playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
							}
						}
					}

					// Mesh Target positioning outside of range
					if (thisManipulatorProperty.type == MANIPULATORPROPERTYTYPEC.MeshTarget && thisManipulatorProperty.meshTarget.initialized && playgroundParticles.playgroundCache.propertyId[p] == thisManipulator.manipulatorId) {
						if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp) {
							playgroundParticles.playgroundCache.position[p] = Vector3.Lerp(particlePosition, thisManipulatorProperty.meshTargetMatrix.MultiplyPoint3x4(thisManipulatorProperty.meshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.meshTarget.vertexPositions.Length]), (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
							if (thisManipulatorProperty.zeroVelocityStrength>0)
								playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
						} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Linear) {
							playgroundParticles.playgroundCache.position[p] = Vector3.MoveTowards(particlePosition, thisManipulatorProperty.meshTargetMatrix.MultiplyPoint3x4(thisManipulatorProperty.meshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.meshTarget.vertexPositions.Length]), (t*thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
							if (thisManipulatorProperty.zeroVelocityStrength>0)
								playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
						}
					}

					// Skinned Mesh Target positioning outside of range
					if (thisManipulatorProperty.type == MANIPULATORPROPERTYTYPEC.SkinnedMeshTarget && thisManipulatorProperty.skinnedMeshTarget.initialized && playgroundParticles.playgroundCache.propertyId[p] == thisManipulator.manipulatorId) {
						if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Lerp) {
							playgroundParticles.playgroundCache.position[p] = Vector3.Lerp(particlePosition, thisManipulatorProperty.skinnedMeshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.skinnedMeshTarget.vertexPositions.Length], t*(thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
							if (thisManipulatorProperty.zeroVelocityStrength>0)
								playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
						} else if (thisManipulatorProperty.transition == MANIPULATORPROPERTYTRANSITIONC.Linear) {
							playgroundParticles.playgroundCache.position[p] = Vector3.MoveTowards(particlePosition, thisManipulatorProperty.skinnedMeshTarget.vertexPositions[playgroundParticles.playgroundCache.propertyTarget[p]%thisManipulatorProperty.skinnedMeshTarget.vertexPositions.Length], t*(thisManipulatorProperty.strength*thisManipulator.strength/manipulatorDistance)/thisManipulator.strengthSmoothing);
							if (thisManipulatorProperty.zeroVelocityStrength>0)
								playgroundParticles.playgroundCache.velocity[p] = Vector3.Lerp(playgroundParticles.playgroundCache.velocity[p], Vector3.zero, t*thisManipulatorProperty.zeroVelocityStrength);
						}
					}
				}
			}
		}
		
		// Update the source scatter
		public void RefreshScatter () {
			for (int p = 0; p<particleCount; p++) {
				playgroundCache.scatterPosition[p] = applySourceScatter?RandomRange(internalRandom03, sourceScatterMin, sourceScatterMax) : Vector3.zero;
			}
		}

		// Random range float adapted for threading
		public static float RandomRange (System.Random random, float min, float max) {
			return min+((float)random.NextDouble())*(max-min);
		}

		// Random range Vector3 adapted for threading
		public static Vector3 RandomRange (System.Random random, Vector3 min, Vector3 max) {
			return new Vector3 (
				RandomRange(random, min.x, max.x),
				RandomRange(random, min.y, max.y),
				RandomRange(random, min.z, max.z)
			);
		}

		// Return a random float array
		public static float[] RandomFloat (int length, float min, float max) {
			System.Random random = new System.Random();
			float[] f = new float[length];
			for (int i = 0; i<length; i++) {
				f[i] = RandomRange (random, min, max);
			}
			return f;
		}

		// Rebirth of a specified particle
		public static void Rebirth (PlaygroundParticlesC playgroundParticles, int p, System.Random random) {

			// Set initial values
			playgroundParticles.playgroundCache.birthDelay[p] = 0f;
			playgroundParticles.playgroundCache.life[p] = 0f;
			playgroundParticles.playgroundCache.birth[p] = playgroundParticles.playgroundCache.death[p]; 
			playgroundParticles.playgroundCache.death[p] += playgroundParticles.lifetime;
			playgroundParticles.playgroundCache.rebirth[p] = playgroundParticles.source==SOURCEC.Script?true:(playgroundParticles.emit && playgroundParticles.playgroundCache.emission[p]);
			playgroundParticles.playgroundCache.velocity[p] = Vector3.zero;

			// Set new random size
			if (playgroundParticles.applyRandomSizeOnRebirth)
				playgroundParticles.playgroundCache.initialSize[p] = RandomRange(random, playgroundParticles.sizeMin, playgroundParticles.sizeMax);

			// Initial velocity
			if (!playgroundParticles.onlySourcePositioning) {
						
				// Initial global velocity
				if (playgroundParticles.applyInitialVelocity) {
					if (playgroundParticles.applyRandomInitialVelocityOnRebirth)
						playgroundParticles.playgroundCache.initialVelocity[p] = RandomRange(random, playgroundParticles.initialVelocityMin, playgroundParticles.initialVelocityMax);
					playgroundParticles.playgroundCache.velocity[p] = playgroundParticles.playgroundCache.initialVelocity[p];
					
					// Give this spawning particle its velocity shape
					if (playgroundParticles.applyInitialVelocityShape)
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Scale(playgroundParticles.playgroundCache.velocity[p], playgroundParticles.initialVelocityShape.Evaluate((p*1f)/(playgroundParticles.particleCount*1f)));
				}

				// Initial local velocity
				if (playgroundParticles.applyInitialLocalVelocity && playgroundParticles.source!=SOURCEC.Script) {
					playgroundParticles.playgroundCache.initialLocalVelocity[p] = RandomRange(random, playgroundParticles.initialLocalVelocityMin, playgroundParticles.initialLocalVelocityMax);
					playgroundParticles.playgroundCache.velocity[p] += playgroundParticles.playgroundCache.targetDirection[p];

					// Give this spawning particle its local velocity shape
					if (playgroundParticles.applyInitialVelocityShape)
						playgroundParticles.playgroundCache.velocity[p] = Vector3.Scale(playgroundParticles.playgroundCache.velocity[p], playgroundParticles.initialVelocityShape.Evaluate((p*1f)/(playgroundParticles.particleCount*1f)));
				}

				// Initial stretch
				if (playgroundParticles.renderModeStretch) {
					if (playgroundParticles.playgroundCache.velocity[p]!=Vector3.zero)
						playgroundParticles.particleCache[p].velocity = playgroundParticles.playgroundCache.velocity[p];
					else 
						playgroundParticles.particleCache[p].velocity = playgroundParticles.stretchStartDirection;
				} else
					playgroundParticles.particleCache[p].velocity = Vector3.zero;
			}
			if (playgroundParticles.source==SOURCEC.Script) {
				// Velocity for script mode
				if (!playgroundParticles.onlySourcePositioning)
					playgroundParticles.playgroundCache.velocity[p] += playgroundParticles.scriptedEmissionVelocity;
				playgroundParticles.playgroundCache.targetPosition[p] = playgroundParticles.scriptedEmissionPosition;
			}
			
			if (playgroundParticles.playgroundCache.rebirth[p]) {

				// Set new random rotation
				if (playgroundParticles.playgroundCache.initialRotation.Length!=playgroundParticles.particleCount) return;
				if (playgroundParticles.applyRandomRotationOnRebirth)
					playgroundParticles.playgroundCache.initialRotation[p] = RandomRange(random, playgroundParticles.initialRotationMin, playgroundParticles.initialRotationMax);
				playgroundParticles.playgroundCache.rotation[p] = playgroundParticles.playgroundCache.initialRotation[p];
				playgroundParticles.particleCache[p].rotation = playgroundParticles.playgroundCache.rotation[p];
				
				// Source Scattering
				if (playgroundParticles.applySourceScatter && playgroundParticles.source!=SOURCEC.Script) {
					if (playgroundParticles.playgroundCache.scatterPosition[p]==Vector3.zero || playgroundParticles.applyRandomScatterOnRebirth)
						playgroundParticles.playgroundCache.scatterPosition[p] = RandomRange(random, playgroundParticles.sourceScatterMin, playgroundParticles.sourceScatterMax);
				} else playgroundParticles.playgroundCache.scatterPosition[p] = Vector3.zero;

				playgroundParticles.playgroundCache.position[p] = playgroundParticles.playgroundCache.targetPosition[p];
				playgroundParticles.particleCache[p].position = playgroundParticles.playgroundCache.targetPosition[p];
				playgroundParticles.playgroundCache.previousParticlePosition[p] = playgroundParticles.playgroundCache.targetPosition[p];
				playgroundParticles.playgroundCache.collisionParticlePosition[p] = playgroundParticles.playgroundCache.targetPosition[p];

				if (playgroundParticles.applyInitialColorOnRebirth) {
					playgroundParticles.particleCache[p].color = playgroundParticles.playgroundCache.initialColor[p];
					playgroundParticles.playgroundCache.color[p] = playgroundParticles.playgroundCache.initialColor[p];
				}
			} else playgroundParticles.particleCache[p].position = PlaygroundC.initialTargetPosition;

			// Set color gradient id
			if (playgroundParticles.colorSource==COLORSOURCEC.LifetimeColors && playgroundParticles.lifetimeColors.Count>0) {
				playgroundParticles.lifetimeColorId++;playgroundParticles.lifetimeColorId=playgroundParticles.lifetimeColorId%playgroundParticles.lifetimeColors.Count;
				playgroundParticles.playgroundCache.lifetimeColorId[p] = playgroundParticles.lifetimeColorId;
			}

			// Reset manipulators influence
			playgroundParticles.playgroundCache.changedByProperty[p] = false;
			playgroundParticles.playgroundCache.changedByPropertyColor[p] = false;
			playgroundParticles.playgroundCache.changedByPropertyColorLerp[p] = false;
			playgroundParticles.playgroundCache.changedByPropertyColorKeepAlpha[p] = false;
			playgroundParticles.playgroundCache.changedByPropertySize[p] = false;
			playgroundParticles.playgroundCache.changedByPropertyTarget[p] = false;
			playgroundParticles.playgroundCache.changedByPropertyDeath[p] = false;
			playgroundParticles.playgroundCache.propertyTarget[p] = 0;
			playgroundParticles.playgroundCache.propertyId[p] = 0;
			playgroundParticles.playgroundCache.propertyColorId[p] = 0;

			// Send birth event
			if (playgroundParticles.events.Count>0)
				playgroundParticles.SendEvent(EVENTTYPEC.Birth, p);
		}

		// Sends an event to Event- Targets and Listeners
		void SendEvent (EVENTTYPEC eventType, int p) {
			SendEvent(eventType, p, playgroundCache.velocity[p], null, null, null);
		}
		void SendEvent (EVENTTYPEC eventType, int p, Vector3 preEventVelocity) {
			SendEvent(eventType, p, preEventVelocity, null, null, null);
		}
		void SendEvent (EVENTTYPEC eventType, int p, Vector3 preEventVelocity, Transform collisionTransform) {
			SendEvent(eventType, p, preEventVelocity, collisionTransform, null, null);
		}
		void SendEvent (EVENTTYPEC eventType, int p, Vector3 preEventVelocity, Transform collisionTransform, Collider collisionCollider) {
			SendEvent(eventType, p, preEventVelocity, collisionTransform, collisionCollider, null);
		}
		void SendEvent (EVENTTYPEC eventType, int p, Vector3 preEventVelocity, Transform collisionTransform, Collider2D collisionCollider2D) {
			SendEvent(eventType, p, preEventVelocity, collisionTransform, null, collisionCollider2D);
		}
		void SendEvent (EVENTTYPEC eventType, int p, Vector3 preEventVelocity, Transform collisionTransform, Collider collisionCollider, Collider2D collisionCollider2D) {
			Vector3 eventPosition = Vector3.zero;
			Vector3 eventVelocity = Vector3.zero;
			Color32 eventColor = Color.white;

			// Loop through available events
			for (int i = 0; i<events.Count; i++) {
				if (events[i].enabled && events[i].eventType==eventType) {

					// Check thresholds
					if (events[i].eventType==EVENTTYPEC.Collision)
						if (events[i].collisionThreshold>preEventVelocity.sqrMagnitude)
							return;
					
					// Set event position
					switch (events[i].eventInheritancePosition) {
						case EVENTINHERITANCEC.User:
						eventPosition = events[i].eventPosition;
						break;
						case EVENTINHERITANCEC.Particle:
						eventPosition = playgroundCache.position[p];
						break;
						case EVENTINHERITANCEC.Source:
						eventPosition = playgroundCache.targetPosition[p];
						break;
					}

					// Set event velocity
					switch (events[i].eventInheritanceVelocity) {
						case EVENTINHERITANCEC.User:
						eventVelocity = events[i].eventVelocity;
						break;
						case EVENTINHERITANCEC.Particle:
						eventVelocity = playgroundCache.velocity[p];
						break;
						case EVENTINHERITANCEC.Source:
						if (applyInitialLocalVelocity)
							eventVelocity = playgroundCache.initialLocalVelocity[p];
						if (applyInitialVelocity)
							eventVelocity += playgroundCache.initialVelocity[p];
						if (applyLifetimeVelocity)
							eventVelocity += lifetimeVelocity.Evaluate(Mathf.Clamp01(playgroundCache.life[p]/lifetime));
						if (applyInitialVelocityShape)
							eventVelocity = Vector3.Scale(eventVelocity, initialVelocityShape.Evaluate((p*1f)/(particleCount*1f)));
						break;
					}

					// Apply multiplier
					eventVelocity *= events[i].velocityMultiplier;

					// Set event color
					switch (events[i].eventInheritanceColor) {
						case EVENTINHERITANCEC.User:
						eventColor = events[i].eventColor;
						break;
						case EVENTINHERITANCEC.Particle:
						eventColor = particleCache[p].color;
						break;
						case EVENTINHERITANCEC.Source:
						eventColor = playgroundCache.initialColor[p];
						break;
					}

					// Send the event to any Event Listeners
					if (events[i].initializedEvent && (events[i].broadcastType==EVENTBROADCASTC.EventListeners || events[i].broadcastType==EVENTBROADCASTC.Both)) {

						eventParticle.particleId = p;
						eventParticle.birth = playgroundCache.birth[p];
						eventParticle.birthDelay = playgroundCache.birthDelay[p];
						eventParticle.changedByProperty = playgroundCache.changedByProperty[p];
						eventParticle.changedByPropertyColor = playgroundCache.changedByPropertyColor[p];
						eventParticle.changedByPropertyColorKeepAlpha = playgroundCache.changedByPropertyColorKeepAlpha[p];
						eventParticle.changedByPropertyColorLerp = playgroundCache.changedByPropertyColorLerp[p];
						eventParticle.changedByPropertyDeath = playgroundCache.changedByPropertyDeath[p];
						eventParticle.changedByPropertySize = playgroundCache.changedByPropertySize[p];
						eventParticle.changedByPropertyTarget = playgroundCache.changedByPropertyTarget[p];
						eventParticle.collisionParticlePosition = playgroundCache.collisionParticlePosition[p];
						eventParticle.color = particleCache[p].color;
						eventParticle.scriptedColor = playgroundCache.scriptedColor[p];
						eventParticle.death = playgroundCache.death[p];
						eventParticle.emission = playgroundCache.emission[p];
						eventParticle.initialColor = playgroundCache.initialColor[p];
						eventParticle.initialLocalVelocity = playgroundCache.initialLocalVelocity[p];
						eventParticle.initialRotation = playgroundCache.initialRotation[p];
						eventParticle.initialSize = playgroundCache.initialSize[p];
						eventParticle.initialVelocity = playgroundCache.initialVelocity[p];
						eventParticle.initialLocalVelocity = playgroundCache.initialLocalVelocity[p];
						eventParticle.life = playgroundCache.life[p];
						eventParticle.lifetimeColorId = playgroundCache.lifetimeColorId[p];
						eventParticle.lifetimeOffset = playgroundCache.lifetimeOffset[p];
						eventParticle.localSpaceMovementCompensation = playgroundCache.localSpaceMovementCompensation[p];
						eventParticle.position = playgroundCache.position[p];
						eventParticle.previousParticlePosition = playgroundCache.previousParticlePosition[p];
						eventParticle.previousTargetPosition = playgroundCache.previousTargetPosition[p];
						eventParticle.propertyColorId = playgroundCache.propertyColorId[p];
						eventParticle.propertyId = playgroundCache.propertyId[p];
						eventParticle.propertyTarget = playgroundCache.propertyTarget[p];
						eventParticle.rebirth = playgroundCache.rebirth[p];
						eventParticle.rotation = playgroundCache.rotation[p];
						eventParticle.rotationSpeed = playgroundCache.rotationSpeed[p];
						eventParticle.scatterPosition = playgroundCache.scatterPosition[p];
						eventParticle.size = playgroundCache.size[p];
						eventParticle.targetDirection = playgroundCache.targetDirection[p];
						eventParticle.targetPosition = playgroundCache.targetPosition[p];
						eventParticle.velocity = playgroundCache.velocity[p];
						eventParticle.collisionCollider = collisionCollider;
						eventParticle.collisionCollider2D = collisionCollider2D;
						eventParticle.collisionTransform = collisionTransform;
						eventParticle.isMasked = playgroundCache.isMasked[p];
						eventParticle.maskAlpha = playgroundCache.maskAlpha[p];

						events[i].SendParticleEvent(eventParticle);
					}

					// Send the event to target
					if (events[i].initializedTarget && (events[i].broadcastType==EVENTBROADCASTC.Target || events[i].broadcastType==EVENTBROADCASTC.Both)) {
						events[i].target.ThreadSafeEmit(eventPosition, eventVelocity, eventColor);
					}
				}
			}
		}
		
		// Delete a state from states list
		public void RemoveState (int i) {
			int newState = activeState;
			newState = (newState%states.Count)-1;
			if (newState<0) newState = 0;
			
			states[newState].Initialize();
			activeState = newState;
			states.RemoveAt(i);
		}
		
		// Wipe out particles in current PlaygroundParticlesC object
		public static void Clear (PlaygroundParticlesC playgroundParticles) {
			playgroundParticles.inTransition = false;
			playgroundParticles.particleCache = new ParticleSystem.Particle[0];
			playgroundParticles.playgroundCache = null;
			playgroundParticles.shurikenParticleSystem.SetParticles(playgroundParticles.particleCache,0);
			playgroundParticles.shurikenParticleSystem.Clear();
		}

		// Store the current state a particle system is in
		public void Save () {
			if (isSnapshot) {
				Debug.Log("A snapshot can't store snapshot data within itself.", gameObject);
				return;
			}
			StartCoroutine (SaveRoutine ("New Snapshot "+(snapshots.Count+1).ToString()));
		}

		// Store the current state a particle system is in and name it
		public void Save (string saveName) {
			if (isSnapshot) {
				Debug.Log("A snapshot can't store snapshot data within itself.", gameObject);
				return;
			}
			StartCoroutine (SaveRoutine (saveName));
		}

		IEnumerator SaveRoutine (string saveName) {
			PlaygroundSave data = new PlaygroundSave();
			data.settings = PlaygroundC.Particle();
			data.settings.isSnapshot = true;
			yield return null;
			data.Save(this);
			data.settings.transform.parent = particleSystemTransform;
			data.settings.transform.name = saveName;
			data.settings.timeOfSnapshot = PlaygroundC.globalTime;
			data.name = saveName;
			data.time = PlaygroundC.globalTime;
			data.particleCount = particleCount;
			data.lifetime = lifetime;
			snapshots.Add (data);
			PlaygroundC.reference.particleSystems.Remove (data.settings);
			#if UNITY_EDITOR
			if (!PlaygroundC.reference.showSnapshotsInHierarchy)
				data.settings.gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif
		}

		// Load from a saved data state using an int
		public void Load (int loadPointer) {
			if (snapshots.Count>0) {
				loadPointer = loadPointer%snapshots.Count;
				StartCoroutine(LoadRoutine(loadPointer));
			} else {
				Debug.Log ("No data to load from. Please use PlaygroundParticlesC.Save() to store a particle system's current state.", particleSystemGameObject);
			}
		}

		// Load from a saved data state using a string
		public void Load (string loadName) {
			if (snapshots.Count>0) {
				for (int i = 0; i<snapshots.Count; i++) { 
					if (snapshots[i].name == loadName.Trim()) {
						StartCoroutine(LoadRoutine(i));
						return;
					}
				}
			} else {
				Debug.Log ("No data found with the name "+loadName+".", particleSystemGameObject);
			}
		}

		// Snapshot loading routine
		bool isLoading = false;
		bool transitionAvailable = false;
		IEnumerator LoadRoutine (int loadPointer) {
			if (loadTransition && loadTransitionTime>0 && snapshots[loadPointer].transitionMultiplier>0 && transitionAvailable && !isYieldRefreshing) {
				StartCoroutine(LoadTransition(loadPointer));
				while (inTransition) yield return null;
			}
			if (isLoading || inTransition || abortTransition) {

				yield break;
			}
			isLoading = true;
			int prevParticleCount = particleCount;
			if (prevParticleCount!=snapshots[loadPointer].settings.particleCount) {
				SetParticleCount(this, snapshots[loadPointer].settings.particleCount);
				yield return null;
			}
			snapshots[loadPointer].Load(this);
			lastTimeUpdated = PlaygroundC.globalTime;
			cameFromNonCalculatedFrame = false;
			cameFromNonEmissionFrame = false;
			yield return null;
			if (snapshots[loadPointer].loadMode!=1) {
				yield return null;
				float tos = snapshots[loadPointer].settings.timeOfSnapshot<=0?snapshots[loadPointer].time:snapshots[loadPointer].settings.timeOfSnapshot;
				for (int p = 0; p<particleCount; p++) {
					playgroundCache.birth[p] = PlaygroundC.globalTime+(playgroundCache.birth[p]-tos); 
					playgroundCache.death[p] = PlaygroundC.globalTime+(playgroundCache.death[p]-tos);
				}
				lifetime = snapshots[loadPointer].settings.lifetime;
				previousLifetime = lifetime;
			} else if (prevParticleCount!=particleCount) {
				SetLifetime(this, sorting, lifetime);
			}
			lastTimeUpdated = PlaygroundC.globalTime;
			cameFromNonCalculatedFrame = false;
			cameFromNonEmissionFrame = false;
			isLoading = false;
			isDoneThread = true;
			transitionAvailable = true;
		}

		// Apply transition between snapshot load
		bool abortTransition = false;
		IEnumerator LoadTransition (int loadPointer) {
			if (inTransition) {
				abortTransition = true;
				yield return null;
			}
			abortTransition = false;
			inTransition = true;

			float transitionStartTime = PlaygroundC.globalTime;
			int loadParticleCount = snapshots[loadPointer].settings.particleCount;
			bool liveParticles = snapshots[loadPointer].loadMode!=1;
			bool firstFrameDone = false;
			int currentParticleCount = particleCount;
			int transitionParticleCount = currentParticleCount;
			PlaygroundCache loadSnapshotData = snapshots[loadPointer].settings.snapshotData;

			INDIVIDUALTRANSITIONTYPEC thisSnapshotTransition = snapshots[loadPointer].transitionType;
			if (thisSnapshotTransition==INDIVIDUALTRANSITIONTYPEC.Inherit) {
				thisSnapshotTransition = (INDIVIDUALTRANSITIONTYPEC)((int)loadTransitionType+1);
			}

			// Prepare arrays
			Vector3[] transitionPosition = (Vector3[])playgroundCache.position.Clone();
			Color32[] transitionColor = (Color32[])playgroundCache.color.Clone();
			float[] transitionSize = (float[])playgroundCache.size.Clone();
			float[] transitionRotation = (float[])playgroundCache.rotation.Clone();

			// Resize if more particles are needed
			if (loadParticleCount>particleCount) {
				PlaygroundC.RunAsync (()=>{
					System.Array.Resize(ref transitionPosition, loadParticleCount);
					System.Array.Resize(ref transitionColor, loadParticleCount);
					System.Array.Resize(ref transitionSize, loadParticleCount);
					System.Array.Resize(ref transitionRotation, loadParticleCount);
					System.Array.Resize(ref transitionRotation, loadParticleCount);

					System.Array.Resize(ref playgroundCache.position, loadParticleCount);
					System.Array.Resize(ref playgroundCache.color, loadParticleCount);
					System.Array.Resize(ref playgroundCache.size, loadParticleCount);
					System.Array.Resize(ref playgroundCache.rotation, loadParticleCount);

					for (int p = particleCount; p<loadParticleCount; p++) {
						transitionPosition[p] = transitionPosition[p%particleCount];
						transitionColor[p].a = 0;
					}
				});
				yield return null;
				particleCache = new ParticleSystem.Particle[loadParticleCount];
				shurikenParticleSystem.Emit(loadParticleCount);
				shurikenParticleSystem.GetParticles(particleCache);
				transitionParticleCount = loadParticleCount;
			}

			// Transition
			while (PlaygroundC.globalTime<transitionStartTime+((loadTransitionTime*snapshots[loadPointer].transitionMultiplier)-.001f) && inTransition && !abortTransition && loadTransition && (loadTransitionTime*snapshots[loadPointer].transitionMultiplier)>0 && currentParticleCount==particleCount) {
				float currentTime = PlaygroundC.globalTime;
				PlaygroundC.RunAsync (()=>{
					float t = TransitionType (thisSnapshotTransition, (currentTime-transitionStartTime)/(loadTransitionTime*snapshots[loadPointer].transitionMultiplier));
					for (int p = 0; p<transitionParticleCount; p++) {

						// Position
						playgroundCache.position[p] = Vector3.Lerp (transitionPosition[p], liveParticles?loadSnapshotData.position[p%loadParticleCount]:loadSnapshotData.targetPosition[p%loadParticleCount], t);
						if (!syncPositionsOnMainThread)
							particleCache[p].position = playgroundCache.position[p];

						// Color
						playgroundCache.color[p] = Color32.Lerp (transitionColor[p], loadSnapshotData.color[p%loadParticleCount], t);
						if (loadParticleCount<particleCount && p>=loadParticleCount)
							playgroundCache.color[p].a = (byte)Mathf.Lerp (transitionColor[p].a, 0f, t);
						particleCache[p].color = playgroundCache.color[p];

						// Size
						playgroundCache.size[p] = Mathf.Lerp (transitionSize[p], loadSnapshotData.size[p%loadParticleCount], t);
						particleCache[p].size = playgroundCache.size[p];

						// Rotation
						playgroundCache.rotation[p] = Mathf.Lerp (transitionRotation[p], loadSnapshotData.rotation[p%loadParticleCount], t);
						particleCache[p].rotation = playgroundCache.rotation[p];
					}
				});
				yield return null;
				if (firstFrameDone && currentParticleCount==particleCount) {
					if (syncPositionsOnMainThread)
						for (int p = 0; p<transitionParticleCount; p++) 
							particleCache[p].position = playgroundCache.position[p];
					shurikenParticleSystem.SetParticles(particleCache, particleCache.Length);
				}
				firstFrameDone = true;
			}

			if (loadParticleCount!=particleCount && abortTransition) {
				particleCache = new ParticleSystem.Particle[particleCount];
				shurikenParticleSystem.Emit(particleCount);
				shurikenParticleSystem.GetParticles(particleCache);
				shurikenParticleSystem.SetParticles(particleCache, particleCache.Length);
				System.Array.Resize(ref playgroundCache.position, particleCount);
				System.Array.Resize(ref playgroundCache.color, particleCount);
				System.Array.Resize(ref playgroundCache.size, particleCount);
				System.Array.Resize(ref playgroundCache.rotation, particleCount);
			}

			lastTimeUpdated = PlaygroundC.globalTime;
			cameFromNonCalculatedFrame = false;
			cameFromNonEmissionFrame = false;
			if (!abortTransition)
				inTransition = false;
		}

		float TransitionType (INDIVIDUALTRANSITIONTYPEC thisTransitionType, float t) {
			if (thisTransitionType==INDIVIDUALTRANSITIONTYPEC.Linear)
				return t;
			else if (thisTransitionType==INDIVIDUALTRANSITIONTYPEC.EaseIn)
				return Mathf.Lerp (0f, 1f, 1f-Mathf.Cos(t*Mathf.PI*.5f));
			else if (thisTransitionType==INDIVIDUALTRANSITIONTYPEC.EaseOut)
				return Mathf.Lerp (0f, 1f, Mathf.Sin(t*Mathf.PI*.5f));
			return t;
		}

		// Check all needed references
		void CheckReferences () {
			if (PlaygroundC.reference==null)
				PlaygroundC.reference = FindObjectOfType<PlaygroundC>();
			if (PlaygroundC.reference==null)
				PlaygroundC.ResourceInstantiate("Playground Manager");
			if (playgroundCache==null)
				playgroundCache = new PlaygroundCache();

			if (particleSystemGameObject==null) {
				particleSystemGameObject = gameObject;
				particleSystemTransform = transform;
				particleSystemRenderer = renderer;
				shurikenParticleSystem = particleSystemGameObject.GetComponent<ParticleSystem>();
				particleSystemRenderer2 = gameObject.particleSystem.renderer as ParticleSystemRenderer;
			}
		}

		// YieldedRefresh makes sure that Playground Manager and simulation time is ready before this particle system
		bool isYieldRefreshing = false;
		public IEnumerator YieldedRefresh () {
			if (isSnapshot) yield break;
			if (isYieldRefreshing) yield break;
			isYieldRefreshing = true;
			bool okToLoadFromStart = true;
			#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying)
				okToLoadFromStart = false;
			#endif
			if (okToLoadFromStart && loadFromStart && snapshots.Count>0) {
				SetLifetime (this, sorting, lifetime);
				yield return null;
				Load(loadFrom);
				yield return null;
				isYieldRefreshing = false;
				yield break;
			}
			isDoneThread = true;
			shurikenParticleSystem.Play();
			if (this.sorting==SORTINGC.NearestNeighbor || this.sorting==SORTINGC.NearestNeighborReversed) {
				yield return null;
				SetLifetime(this, SORTINGC.Burst, lifetime);
				ThreadedCalculations(this);
			}
			yield return null;
			SetLifetime(this, sorting, lifetime);
			yield return null;
			isYieldRefreshing = false;
			transitionAvailable = true;
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// MonoBehaviours
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		void Awake () {

			if (isSnapshot) return;

			// Make sure all references exists
			CheckReferences();

			// Make sure that state data is initialized
			for (int x = 0; x<states.Count; x++)
				states[x].Initialize();
			
			// Initialize
			if (worldObject!=null && worldObject.transform!=null)
				worldObject.Initialize();
			if (skinnedWorldObject!=null && skinnedWorldObject.transform!=null)
				skinnedWorldObject.Initialize();
			if (projection!=null && projection.projectionTexture)
				projection.Initialize();
			if (manipulators.Count>0) {
				for (int i = 0; i<manipulators.Count; i++)
					manipulators[i].Update();
			}

			internalRandom01 = new System.Random();
			internalRandom02 = new System.Random();
			internalRandom03 = new System.Random();
			turbulenceSimplex = new SimplexNoise();
			eventParticle = new PlaygroundEventParticle();

			if (PlaygroundC.reference!=null) {
				#if UNITY_EDITOR
				if (isSnapshot && !PlaygroundC.reference.showSnapshotsInHierarchy) {
					gameObject.hideFlags = HideFlags.HideInHierarchy;
					return;
				}
				#endif
				if (isSnapshot) return;
				if (!PlaygroundC.reference.particleSystems.Contains(this))
					PlaygroundC.reference.particleSystems.Add(this);
				if (particleSystemTransform.parent==null && PlaygroundC.reference.autoGroup)
					particleSystemTransform.parent = PlaygroundC.referenceTransform;
			}

			// Reset event controlled by-list, this will be refreshed first calculation
			eventControlledBy = new List<PlaygroundParticlesC>();
		}

		void OnEnable () {
			if (isSnapshot) return;
			isYieldRefreshing = false;
			isDoneThread = true;
			calculate = true;
			lastTimeUpdated = PlaygroundC.globalTime;

			// Set 0 size to avoid one-frame flash
			shurikenParticleSystem.startSize = 0f;
			if (shurikenParticleSystem.isPaused || shurikenParticleSystem.isStopped)
				shurikenParticleSystem.Play();

			// Set initial values
			previousLifetime = lifetime;
			previousEmission = emit;
			loopExceeded = false;
			loopExceededOnParticle = -1;
			queueEmissionHalt = false;
			
			// Initiate all arrays by setting particle count
			if (particleCache==null) {
				SetParticleCount(this, particleCount);
			} else {
			
				// Clean up particle positions
				SetInitialTargetPosition(this, PlaygroundC.initialTargetPosition, true);

				// Refresh
				StartCoroutine(YieldedRefresh());
			}
		}
		
		public void Start () {
			if (particleSystemGameObject.activeSelf)
				StartCoroutine(YieldedRefresh());
		}

		IEnumerator OnBecameInvisible () {
			if (isSnapshot) yield break;
			if (particleSystemGameObject.activeSelf) {
				if (forceVisibilityWhenOutOfFrustrum) {
					yield return null;
					shurikenParticleSystem.Play();
				}
				if (!pauseCalculationWhenInvisible)
					yield break;
				yield return new WaitForSeconds(1f);
				if (!particleSystemRenderer.isVisible && pauseCalculationWhenInvisible) {
					calculate = false;
					while (!particleSystemRenderer.isVisible && particleSystemGameObject.activeSelf) {
						yield return null;
					}
					if (pauseCalculationWhenInvisible)
						calculate = true;
				}
			}
		}

		void OnBecameVisible () {
			if (isSnapshot) return;
			if (pauseCalculationWhenInvisible) {
				calculate = true;
			}
		}

		void OnDestroy () {
			locker = null;
			// Remove any events listed by other particle systems
			for (int i = 0; i<events.Count; i++) {
				if (events[i].target!=null)
					events[i].target.eventControlledBy.Remove (this);
			}

			// Remove this PlaygroundParticlesC object from Particle Systems list
			if (PlaygroundC.reference)
				PlaygroundC.reference.particleSystems.Remove(this);
		}

	#if UNITY_EDITOR
		// Select the particle system in Editor from MonoBehavior as we need one frame to initialize
		public void EditorYieldSelect () {StartCoroutine (YieldSelect ());}
		public IEnumerator YieldSelect () {
			if (isSnapshot) yield break;
			yield return null;
			if (!UnityEditor.EditorApplication.isPlaying)
				UnityEditor.Selection.activeGameObject = particleSystemGameObject;
		}
	#endif
		
	}	

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// MeshParticles - Extension class for PlaygroundParticlesC which creates mesh states. 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public class MeshParticles : PlaygroundParticlesC {
		
		public static PlaygroundParticlesC CreateMeshParticles (Mesh[] meshes, Texture2D[] textures, Texture2D[] heightMap, string name, Vector3 position, Quaternion rotation, float particleScale, Vector3[] offsets, Material material) {
			PlaygroundParticlesC meshParticles = PlaygroundParticlesC.CreateParticleObject(name,position,rotation,particleScale,material);
			meshParticles.states = new List<ParticleStateC>();
			
			int[] quantityList = new int[meshes.Length];
			int i = 0;
			for (; i<textures.Length; i++)
				quantityList[i] = meshes[i].vertexCount;
			meshParticles.particleCache = new ParticleSystem.Particle[quantityList[PlaygroundC.Largest(quantityList)]];
			meshParticles.shurikenParticleSystem.Emit(meshParticles.particleCache.Length);
			meshParticles.shurikenParticleSystem.GetParticles(meshParticles.particleCache);
			for (i = 0; i<meshes.Length; i++) {
				meshParticles.states.Add(new ParticleStateC());
				meshParticles.states[0].ConstructParticles(meshes[i],textures[i],particleScale,offsets[i], "State "+i,null);
			}
			
			PlaygroundC.Update(meshParticles);
			PlaygroundC.particlesQuantity++;
			
			return meshParticles;
		}
		
		public static void Add (PlaygroundParticlesC meshParticles, Mesh mesh, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			meshParticles.states.Add(new ParticleStateC());
			meshParticles.states[meshParticles.states.Count-1].ConstructParticles(mesh,scale,offset,stateName,stateTransform);
		}
		
		public static void Add (PlaygroundParticlesC meshParticles, Mesh mesh, Texture2D texture, float scale, Vector3 offset, string stateName, Transform stateTransform) {
			meshParticles.states.Add(new ParticleStateC());
			meshParticles.states[meshParticles.states.Count-1].ConstructParticles(mesh,texture,scale,offset,stateName,stateTransform);
		}
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Cache
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[Serializable]
	public class PlaygroundSave {
		[HideInInspector] public string name;										// The name of this PlaygroundSave
		[HideInInspector] public float time;										// The global time when this PlaygroundSave was created
		[HideInInspector] public float lifetime;									// The lifetime of the particle system
		[HideInInspector] public int particleCount;									// The particle count of the particle system
		[HideInInspector] public PlaygroundParticlesC settings;						// The cached settings of the particle system (Particles are stored in settings.snapshotData)
		[HideInInspector] public PlaygroundTransformC transform;					// The stored transform information
		[HideInInspector] public int loadMode = 0;									// 0 = Settings+Particles, 1 = Settings only, 2 = Particles only
		[HideInInspector] public bool loadTransform = true;							// Should the stored transform information load?
		[HideInInspector] public float transitionMultiplier = 1f;					// The multiplier of transition time
		[HideInInspector] public INDIVIDUALTRANSITIONTYPEC transitionType;			// The transition type of this PlaygroundSave

		// Load the data from this PlaygroundSave
		public void Load (PlaygroundParticlesC loadTo) {
			PlaygroundC.RunAsync(()=>{
				switch (loadMode) {
				case 0: settings.CopyTo(loadTo); loadTo.playgroundCache = settings.snapshotData.Clone(); break;
				case 1: settings.CopyTo(loadTo); break;
				case 2: loadTo.playgroundCache = settings.snapshotData.Clone(); break;
				default: settings.CopyTo(loadTo); loadTo.playgroundCache = settings.snapshotData.Clone(); break;
				}
			});
			if (loadTransform)
				transform.GetFromTransform (loadTo.particleSystemTransform);
		}

		// Save the data into this PlaygroundSave
		public void Save (PlaygroundParticlesC playgroundParticles) {
			transform = new PlaygroundTransformC();
			transform.SetFromTransform (playgroundParticles.particleSystemTransform);
			PlaygroundC.RunAsync(()=>{
			playgroundParticles.CopyTo(settings);
			settings.snapshotData = playgroundParticles.playgroundCache.Clone();
			});
		}

		// Return a copy of this PlaygroundSave
		public PlaygroundSave Clone () {
			PlaygroundSave playgroundSave = new PlaygroundSave();
			settings.CopyTo(playgroundSave.settings);
			playgroundSave.name = name;
			playgroundSave.time = time;
			playgroundSave.lifetime = lifetime;
			playgroundSave.particleCount = particleCount;
			playgroundSave.loadMode = loadMode;
			playgroundSave.loadTransform = loadTransform;
			playgroundSave.transitionMultiplier = transitionMultiplier;
			playgroundSave.transitionType = transitionType;
			return playgroundSave;
		}
	}

	[Serializable]
	public class PlaygroundEventParticle {
		[HideInInspector] public float initialSize;									// The initial size of each particle
		[HideInInspector] public float size;										// The lifetime size of each particle
		[HideInInspector] public float rotation;									// The rotation of each particle
		[HideInInspector] public float life;										// The lifetime of each particle
		[HideInInspector] public float birth;										// The time of birth for each particle
		[HideInInspector] public float birthDelay;									// The delayed time of birth when emission has changed
		[HideInInspector] public float death;										// The time of death for each particle
		[HideInInspector] public bool emission;										// The emission for each particle (controlled by emission rate)
		[HideInInspector] public bool rebirth;										// The rebirth for each particle
		[HideInInspector] public float lifetimeOffset;								// The offset in birth-death (sorting)
		[HideInInspector] public Vector3 velocity;									// The velocity of each particle
		[HideInInspector] public Vector3 initialVelocity;							// The initial velocity of each particle
		[HideInInspector] public Vector3 initialLocalVelocity;						// The initial local velocity of each particle
		[HideInInspector] public Vector3 position;									// The position of each particle
		[HideInInspector] public Vector3 targetPosition;							// The source position for each particle
		[HideInInspector] public Vector3 targetDirection;							// The source direction for each particle
		[HideInInspector] public Vector3 previousTargetPosition; 					// The previous source position for each particle (used to calculate delta movement)
		[HideInInspector] public Vector3 previousParticlePosition;					// The previous calculated frame's particle position
		[HideInInspector] public Vector3 collisionParticlePosition;					// The calculated particle position for collision (depending on collision stepper)
		[HideInInspector] public Vector3 localSpaceMovementCompensation;			// The delta to compensate for moving particles in local space
		[HideInInspector] public Vector3 scatterPosition;							// The scattered position to apply on each particle birth
		[HideInInspector] public float initialRotation;								// The initial rotation of each particle
		[HideInInspector] public float rotationSpeed;								// The rotation speed of each particle
		[HideInInspector] public Color32 color;										// The current color of each particle
		[HideInInspector] public Color32 scriptedColor;								// The color set from script of each particle
		[HideInInspector] public Color32 initialColor;								// The set source color
		[HideInInspector] public int lifetimeColorId;								// The color gradient for each particle if Color Source is set to LifetimeColors
		[HideInInspector] public int particleId;									// The id of this particle

		[HideInInspector] public bool changedByProperty;							// The interaction with property manipulators of each particle
		[HideInInspector] public bool changedByPropertyColor;						// The interaction with property manipulators that change color of each particle
		[HideInInspector] public bool changedByPropertyColorLerp; 					// The interaction with property manipulators that change color over time of each particle
		[HideInInspector] public bool changedByPropertyColorKeepAlpha;				// The interaction with property manipulators that change color and wants to keep alpha
		[HideInInspector] public bool changedByPropertySize;						// The interaction with property manipulators that change size of each particle
		[HideInInspector] public bool changedByPropertyTarget;						// The interaction with property manipulators that change target of each particle
		[HideInInspector] public bool changedByPropertyDeath;						// The interaction with death manipulators that forces a particle to a sooner end
		[HideInInspector] public int propertyTarget;								// The property target pointer for each particle
		[HideInInspector] public int propertyId;									// The property target id for each particle (pairing a particle's target to a manipulator)
		[HideInInspector] public int propertyColorId;								// The property color id for each particles (pairing a particle's color to a manipulator

		[HideInInspector] public bool isMasked;										// Is this particle masked?
		[HideInInspector] public float maskAlpha;									// The alpha of this masked particle

		[HideInInspector] public Transform collisionTransform;
		[HideInInspector] public Collider collisionCollider;
		[HideInInspector] public Collider2D collisionCollider2D;

		// Copy this PlaygroundEventParticle
		public PlaygroundEventParticle Clone () {
			PlaygroundEventParticle playgroundEventParticle = new PlaygroundEventParticle();
			playgroundEventParticle.initialSize = initialSize;
			playgroundEventParticle.size = size;
			playgroundEventParticle.life = life;
			playgroundEventParticle.rotation = rotation;
			playgroundEventParticle.birth = birth;
			playgroundEventParticle.birthDelay = birthDelay;
			playgroundEventParticle.death = death;
			playgroundEventParticle.emission = emission;
			playgroundEventParticle.rebirth = rebirth;
			playgroundEventParticle.lifetimeOffset = lifetimeOffset;
			playgroundEventParticle.velocity = velocity;
			playgroundEventParticle.initialVelocity = initialVelocity;
			playgroundEventParticle.initialLocalVelocity = initialLocalVelocity;
			playgroundEventParticle.position = position;
			playgroundEventParticle.targetPosition = targetPosition;
			playgroundEventParticle.targetDirection = targetDirection;
			playgroundEventParticle.previousTargetPosition = previousTargetPosition;
			playgroundEventParticle.previousParticlePosition = previousParticlePosition;
			playgroundEventParticle.collisionParticlePosition = collisionParticlePosition;
			playgroundEventParticle.localSpaceMovementCompensation = localSpaceMovementCompensation;
			playgroundEventParticle.scatterPosition = scatterPosition;
			playgroundEventParticle.initialRotation = initialRotation;
			playgroundEventParticle.rotationSpeed = rotationSpeed;
			playgroundEventParticle.color = color;
			playgroundEventParticle.scriptedColor = scriptedColor;
			playgroundEventParticle.initialColor = initialColor;
			playgroundEventParticle.lifetimeColorId = lifetimeColorId;
			playgroundEventParticle.changedByProperty = changedByProperty;
			playgroundEventParticle.changedByPropertyColor = changedByPropertyColor;
			playgroundEventParticle.changedByPropertyColorLerp = changedByPropertyColorLerp;
			playgroundEventParticle.changedByPropertyColorKeepAlpha = changedByPropertyColorKeepAlpha;
			playgroundEventParticle.changedByPropertySize = changedByPropertySize;
			playgroundEventParticle.changedByPropertyTarget = changedByPropertyTarget;
			playgroundEventParticle.changedByPropertyDeath = changedByPropertyDeath;
			playgroundEventParticle.propertyTarget = propertyTarget;
			playgroundEventParticle.propertyId = propertyId;
			playgroundEventParticle.propertyColorId = propertyColorId;
			playgroundEventParticle.particleId = particleId;
			playgroundEventParticle.isMasked = isMasked;
			playgroundEventParticle.maskAlpha = maskAlpha;

			playgroundEventParticle.collisionTransform = collisionTransform;
			playgroundEventParticle.collisionCollider = collisionCollider;
			playgroundEventParticle.collisionCollider2D = collisionCollider2D;
			
			return playgroundEventParticle;
		}
	}

	[Serializable]
	public class PlaygroundCache {
		[HideInInspector] public float[] initialSize;								// The initial size of each particle
		[HideInInspector] public float[] size;										// The lifetime size of each particle
		[HideInInspector] public float[] rotation;									// The rotation of each particle
		[HideInInspector] public float[] life;										// The lifetime of each particle
		[HideInInspector] public float[] birth;										// The time of birth for each particle
		[HideInInspector] public float[] birthDelay;								// The delayed time of birth when emission has changed
		[HideInInspector] public float[] death;										// The time of death for each particle
		[HideInInspector] public bool[] emission;									// The emission for each particle (controlled by emission rate)
		[HideInInspector] public bool[] rebirth;									// The rebirth for each particle
		[HideInInspector] public float[] lifetimeOffset;							// The offset in birth-death (sorting)
		[HideInInspector] public Vector3[] velocity;								// The velocity of each particle in this PlaygroundParticles
		[HideInInspector] public Vector3[] initialVelocity;							// The initial velocity of each particle in this PlaygroundParticles
		[HideInInspector] public Vector3[] initialLocalVelocity;					// The initial local velocity of each particle in this PlaygroundParticles
		[HideInInspector] public Vector3[] position;								// The position of each particle in this PlaygroundParticles
		[HideInInspector] public Vector3[] targetPosition;							// The source position for each particle
		[HideInInspector] public Vector3[] targetDirection;							// The source direction for each particle
		[HideInInspector] public Vector3[] previousTargetPosition; 					// The previous source position for each particle (used to calculate delta movement)
		[HideInInspector] public Vector3[] previousParticlePosition;				// The previous calculated frame's particle position
		[HideInInspector] public Vector3[] collisionParticlePosition;				// The calculated particle position for collision (depending on collision stepper)
		[HideInInspector] public Vector3[] localSpaceMovementCompensation;			// The delta to compensate for moving particles in local space
		[HideInInspector] public Vector3[] scatterPosition;							// The scattered position to apply on each particle birth in this PlaygroundParticles
		[HideInInspector] public float[] initialRotation;							// The initial rotation of each particle in this PlaygroundParticles
		[HideInInspector] public float[] rotationSpeed;								// The rotation speed of each particle in this PlaygroundParticles
		[HideInInspector] public Color32[] color;									// The color of each particle in this PlaygroundParticles
		[HideInInspector] public Color32[] scriptedColor;							// The color set from script of each particle in this PlaygroundParticles
		[HideInInspector] public Color32[] initialColor;							// The set source color
		[HideInInspector] public int[] lifetimeColorId;								// The color gradient for each particle if Color Source is set to LifetimeColors
		
		[HideInInspector] public bool[] changedByProperty;							// The interaction with property manipulators of each particle
		[HideInInspector] public bool[] changedByPropertyColor;						// The interaction with property manipulators that change color of each particle
		[HideInInspector] public bool[] changedByPropertyColorLerp; 				// The interaction with property manipulators that change color over time of each particle
		[HideInInspector] public bool[] changedByPropertyColorKeepAlpha;			// The interaction with property manipulators that change color and wants to keep alpha
		[HideInInspector] public bool[] changedByPropertySize;						// The interaction with property manipulators that change size of each particle
		[HideInInspector] public bool[] changedByPropertyTarget;					// The interaction with property manipulators that change target of each particle
		[HideInInspector] public bool[] changedByPropertyDeath;						// The interaction with death manipulators that forces a particle to a sooner end
		[HideInInspector] public int[] propertyTarget;								// The property target pointer for each particle
		[HideInInspector] public int[] propertyId;									// The property target id for each particle (pairing a particle's target to a manipulator)
		[HideInInspector] public int[] propertyColorId;								// The property color id for each particles (pairing a particle's color to a manipulator

		[HideInInspector] public bool[] isMasked;									// Is this particle masked?
		[HideInInspector] public float[] maskAlpha;									// The alpha of this masked particle
		
		// Copy this PlaygroundCache
		public PlaygroundCache Clone () {
			PlaygroundCache playgroundCache = new PlaygroundCache();
			playgroundCache.initialSize = initialSize.Clone() as float[];
			playgroundCache.size = size.Clone () as float[];
			playgroundCache.life = life.Clone() as float[];
			playgroundCache.rotation = rotation.Clone() as float[];
			playgroundCache.birth = birth.Clone() as float[];
			playgroundCache.birthDelay = birthDelay.Clone() as float[];
			playgroundCache.death = death.Clone() as float[];
			playgroundCache.emission = emission.Clone() as bool[];
			playgroundCache.rebirth = rebirth.Clone() as bool[];
			playgroundCache.lifetimeOffset = lifetimeOffset.Clone() as float[];
			playgroundCache.velocity = velocity.Clone() as Vector3[];
			playgroundCache.initialVelocity = initialVelocity.Clone() as Vector3[];
			playgroundCache.initialLocalVelocity = initialLocalVelocity.Clone() as Vector3[];
			playgroundCache.position = position.Clone () as Vector3[];
			playgroundCache.targetPosition = targetPosition.Clone() as Vector3[];
			playgroundCache.targetDirection = targetDirection.Clone() as Vector3[];
			playgroundCache.previousTargetPosition = previousTargetPosition.Clone() as Vector3[];
			playgroundCache.previousParticlePosition = previousParticlePosition.Clone() as Vector3[];
			playgroundCache.collisionParticlePosition = collisionParticlePosition.Clone () as Vector3[];
			playgroundCache.localSpaceMovementCompensation = localSpaceMovementCompensation.Clone() as Vector3[];
			playgroundCache.scatterPosition = scatterPosition.Clone() as Vector3[];
			playgroundCache.initialRotation = initialRotation.Clone() as float[];
			playgroundCache.rotationSpeed = rotationSpeed.Clone() as float[];
			playgroundCache.color = color.Clone() as Color32[];
			playgroundCache.scriptedColor = scriptedColor.Clone() as Color32[];
			playgroundCache.initialColor = initialColor.Clone () as Color32[];
			playgroundCache.lifetimeColorId = lifetimeColorId.Clone () as int[];
			playgroundCache.changedByProperty = changedByProperty.Clone() as bool[];
			playgroundCache.changedByPropertyColor = changedByPropertyColor.Clone() as bool[];
			playgroundCache.changedByPropertyColorLerp = changedByPropertyColorLerp.Clone() as bool[];
			playgroundCache.changedByPropertyColorKeepAlpha = changedByPropertyColorKeepAlpha.Clone () as bool[];
			playgroundCache.changedByPropertySize = changedByPropertySize.Clone() as bool[];
			playgroundCache.changedByPropertyTarget = changedByPropertyTarget.Clone() as bool[];
			playgroundCache.changedByPropertyDeath = changedByPropertyDeath.Clone() as bool[];
			playgroundCache.propertyTarget = propertyTarget.Clone() as int[];
			playgroundCache.propertyId = propertyId.Clone() as int[];
			playgroundCache.propertyColorId = propertyColorId.Clone() as int[];
			playgroundCache.isMasked = isMasked.Clone() as bool[];
			playgroundCache.maskAlpha = maskAlpha.Clone() as float[];
			return playgroundCache;
		}
	}

	public class SimplexNoise {

		// Simplex noise based on http://staffwww.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf for public domain by courtesy of Stefan Gustavson.

		private static int[][] grad3 = new int[][] {
			new int[] {1,1,0}, new int[] {-1,1,0}, new int[] {1,-1,0}, new int[] {-1,-1,0},
			new int[] {1,0,1}, new int[] {-1,0,1}, new int[] {1,0,-1}, new int[] {-1,0,-1},
			new int[] {0,1,1}, new int[] {0,-1,1}, new int[] {0,1,-1}, new int[] {0,-1,-1}};
		private static int[][] grad4 = new int[][] {
			new int[] {0,1,1,1},  new int[] {0,1,1,-1},  new int[] {0,1,-1,1},  new int[] {0,1,-1,-1},
			new int[] {0,-1,1,1}, new int[] {0,-1,1,-1}, new int[] {0,-1,-1,1}, new int[] {0,-1,-1,-1},
			new int[] {1,0,1,1},  new int[] {1,0,1,-1},  new int[] {1,0,-1,1},  new int[] {1,0,-1,-1},
			new int[] {-1,0,1,1}, new int[] {-1,0,1,-1}, new int[] {-1,0,-1,1}, new int[] {-1,0,-1,-1},
			new int[] {1,1,0,1},  new int[] {1,1,0,-1},  new int[] {1,-1,0,1},  new int[] {1,-1,0,-1},
			new int[] {-1,1,0,1}, new int[] {-1,1,0,-1}, new int[] {-1,-1,0,1}, new int[] {-1,-1,0,-1},
			new int[] {1,1,1,0},  new int[] {1,1,-1,0},  new int[] {1,-1,1,0},  new int[] {1,-1,-1,0},
			new int[] {-1,1,1,0}, new int[] {-1,1,-1,0}, new int[] {-1,-1,1,0}, new int[] {-1,-1,-1,0}};
		private static int[] p = {151,160,137,91,90,15,
			131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
			190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
			88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
			77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
			102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
			135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
			5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
			223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
			129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
			251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
			49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
			138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};
		// To remove the need for index wrapping, double the permutation table length
		private static int[] perm = new int[512];
		static SimplexNoise() { for(int i=0; i<512; i++) perm[i]=p[i & 255]; } // moved to constructor
		// A lookup table to traverse the simplex around a given point in 4D.
		// Details can be found where this table is used, in the 4D noise method.
		private static int[][] simplex = new int[][] {
			new int[] {0,1,2,3}, new int[] {0,1,3,2}, new int[] {0,0,0,0}, new int[] {0,2,3,1}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {1,2,3,0},
			new int[] {0,2,1,3}, new int[] {0,0,0,0}, new int[] {0,3,1,2}, new int[] {0,3,2,1}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {1,3,2,0},
			new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0},
			new int[] {1,2,0,3}, new int[] {0,0,0,0}, new int[] {1,3,0,2}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {2,3,0,1}, new int[] {2,3,1,0},
			new int[] {1,0,2,3}, new int[] {1,0,3,2}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {2,0,3,1}, new int[] {0,0,0,0}, new int[] {2,1,3,0},
			new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0},
			new int[] {2,0,1,3}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {3,0,1,2}, new int[] {3,0,2,1}, new int[] {0,0,0,0}, new int[] {3,1,2,0},
			new int[] {2,1,0,3}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {3,1,0,2}, new int[] {0,0,0,0}, new int[] {3,2,0,1}, new int[] {3,2,1,0}};
		// This method is a *lot* faster than using (int)Mathf.floor(x)
		private static int fastfloor(double x) {
			return x>0 ? (int)x : (int)x-1;
		}
		private static double dot(int[] g, double x, double y) {
			return g[0]*x + g[1]*y;
		}
		private static double dot(int[] g, double x, double y, double z) {
			return g[0]*x + g[1]*y + g[2]*z;
		}
		private static double dot(int[] g, double x, double y, double z, double w) {
			return g[0]*x + g[1]*y + g[2]*z + g[3]*w; 
		}

		// 3D simplex noise
		public double noise(double xin, double yin, double zin) {
			double n0, n1, n2, n3; // Noise contributions from the four corners
			// Skew the input space to determine which simplex cell we're in
			double F3 = 1.0/3.0;
			double s = (xin+yin+zin)*F3; // Very nice and simple skew factor for 3D
			int i = fastfloor(xin+s);
			int j = fastfloor(yin+s);
			int k = fastfloor(zin+s);
			double G3 = 1.0/6.0; // Very nice and simple unskew factor, too
			double t = (i+j+k)*G3; 
			double X0 = i-t; // Unskew the cell origin back to (x,y,z) space
			double Y0 = j-t;
			double Z0 = k-t;
			double x0 = xin-X0; // The x,y,z distances from the cell origin
			double y0 = yin-Y0;
			double z0 = zin-Z0;
			// For the 3D case, the simplex shape is a slightly irregular tetrahedron.
			// Determine which simplex we are in.
			int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
			int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
			if(x0>=y0) {
				if(y0>=z0)
				{ i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
				else if(x0>=z0) { i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
				else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; } // Z X Y order
			}
			else { // x0<y0
				if(y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
				else if(x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
				else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
			}
			// A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
			// a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
			// a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
			// c = 1/6.
			double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
			double y1 = y0 - j1 + G3;
			double z1 = z0 - k1 + G3;
			double x2 = x0 - i2 + 2.0*G3; // Offsets for third corner in (x,y,z) coords
			double y2 = y0 - j2 + 2.0*G3;
			double z2 = z0 - k2 + 2.0*G3;
			double x3 = x0 - 1.0 + 3.0*G3; // Offsets for last corner in (x,y,z) coords
			double y3 = y0 - 1.0 + 3.0*G3;
			double z3 = z0 - 1.0 + 3.0*G3;
			// Work out the hashed gradient indices of the four simplex corners
			int ii = i & 255;
			int jj = j & 255;
			int kk = k & 255;
			int gi0 = perm[ii+perm[jj+perm[kk]]] % 12;
			int gi1 = perm[ii+i1+perm[jj+j1+perm[kk+k1]]] % 12;
			int gi2 = perm[ii+i2+perm[jj+j2+perm[kk+k2]]] % 12;
			int gi3 = perm[ii+1+perm[jj+1+perm[kk+1]]] % 12;
			// Calculate the contribution from the four corners
			double t0 = 0.5 - x0*x0 - y0*y0 - z0*z0;
			if(t0<0) n0 = 0.0;
			else {
				t0 *= t0;
				n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
			}
			double t1 = 0.6 - x1*x1 - y1*y1 - z1*z1;
			if(t1<0) n1 = 0.0;
			else {
				t1 *= t1;
				n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
			}
			double t2 = 0.6 - x2*x2 - y2*y2 - z2*z2;
			if(t2<0) n2 = 0.0;
			else {
				t2 *= t2;
				n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
			}
			double t3 = 0.6 - x3*x3 - y3*y3 - z3*z3;
			if(t3<0) n3 = 0.0;
			else {
				t3 *= t3;
				n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
			}
			// Add contributions from each corner to get the final noise value.
			// The result is scaled to stay just inside [-1,1]
			return 32.0*(n0 + n1 + n2 + n3);
		}  

		// 4D simplex noise
		public double noise(double x, double y, double z, double w) {
			
			// The skewing and unskewing factors are hairy again for the 4D case
			double F4 = (Mathf.Sqrt(5.0f)-1.0)/4.0;
			double G4 = (5.0-Mathf.Sqrt(5.0f))/20.0;
			double n0, n1, n2, n3, n4; // Noise contributions from the five corners
			// Skew the (x,y,z,w) space to determine which cell of 24 simplices we're in
			double s = (x + y + z + w) * F4; // Factor for 4D skewing
			int i = fastfloor(x + s);
			int j = fastfloor(y + s);
			int k = fastfloor(z + s);
			int l = fastfloor(w + s);
			double t = (i + j + k + l) * G4; // Factor for 4D unskewing
			double X0 = i - t; // Unskew the cell origin back to (x,y,z,w) space
			double Y0 = j - t;
			double Z0 = k - t;
			double W0 = l - t;
			double x0 = x - X0;  // The x,y,z,w distances from the cell origin
			double y0 = y - Y0;
			double z0 = z - Z0;
			double w0 = w - W0;
			// For the 4D case, the simplex is a 4D shape I won't even try to describe.
			// To find out which of the 24 possible simplices we're in, we need to
			// determine the magnitude ordering of x0, y0, z0 and w0.
			// The method below is a good way of finding the ordering of x,y,z,w and
			// then find the correct traversal order for the simplex we’re in.
			// First, six pair-wise comparisons are performed between each possible pair
			// of the four coordinates, and the results are used to add up binary bits
			// for an integer index.
			int c1 = (x0 > y0) ? 32 : 0;
			int c2 = (x0 > z0) ? 16 : 0;
			int c3 = (y0 > z0) ? 8 : 0;
			int c4 = (x0 > w0) ? 4 : 0;
			int c5 = (y0 > w0) ? 2 : 0;
			int c6 = (z0 > w0) ? 1 : 0;
			int c = c1 + c2 + c3 + c4 + c5 + c6;
			int i1, j1, k1, l1; // The integer offsets for the second simplex corner
			int i2, j2, k2, l2; // The integer offsets for the third simplex corner
			int i3, j3, k3, l3; // The integer offsets for the fourth simplex corner
			// simplex[c] is a 4-vector with the numbers 0, 1, 2 and 3 in some order.
			// Many values of c will never occur, since e.g. x>y>z>w makes x<z, y<w and x<w
			// impossible. Only the 24 indices which have non-zero entries make any sense.
			// We use a thresholding to set the coordinates in turn from the largest magnitude.
			// The number 3 in the "simplex" array is at the position of the largest coordinate.
			i1 = simplex[c][0]>=3 ? 1 : 0;
			j1 = simplex[c][1]>=3 ? 1 : 0;
			k1 = simplex[c][2]>=3 ? 1 : 0;
			l1 = simplex[c][3]>=3 ? 1 : 0;
			// The number 2 in the "simplex" array is at the second largest coordinate.
			i2 = simplex[c][0]>=2 ? 1 : 0;
			j2 = simplex[c][1]>=2 ? 1 : 0;    k2 = simplex[c][2]>=2 ? 1 : 0;
			l2 = simplex[c][3]>=2 ? 1 : 0;
			// The number 1 in the "simplex" array is at the second smallest coordinate.
			i3 = simplex[c][0]>=1 ? 1 : 0;
			j3 = simplex[c][1]>=1 ? 1 : 0;
			k3 = simplex[c][2]>=1 ? 1 : 0;
			l3 = simplex[c][3]>=1 ? 1 : 0;
			// The fifth corner has all coordinate offsets = 1, so no need to look that up.
			double x1 = x0 - i1 + G4; // Offsets for second corner in (x,y,z,w) coords
			double y1 = y0 - j1 + G4;
			double z1 = z0 - k1 + G4;
			double w1 = w0 - l1 + G4;
			double x2 = x0 - i2 + 2.0*G4; // Offsets for third corner in (x,y,z,w) coords
			double y2 = y0 - j2 + 2.0*G4;
			double z2 = z0 - k2 + 2.0*G4;
			double w2 = w0 - l2 + 2.0*G4;
			double x3 = x0 - i3 + 3.0*G4; // Offsets for fourth corner in (x,y,z,w) coords
			double y3 = y0 - j3 + 3.0*G4;
			double z3 = z0 - k3 + 3.0*G4;
			double w3 = w0 - l3 + 3.0*G4;
			double x4 = x0 - 1.0 + 4.0*G4; // Offsets for last corner in (x,y,z,w) coords
			double y4 = y0 - 1.0 + 4.0*G4;
			double z4 = z0 - 1.0 + 4.0*G4;
			double w4 = w0 - 1.0 + 4.0*G4;
			// Work out the hashed gradient indices of the five simplex corners
			int ii = i & 255;
			int jj = j & 255;
			int kk = k & 255;
			int ll = l & 255;
			int gi0 = perm[ii+perm[jj+perm[kk+perm[ll]]]] % 32;
			int gi1 = perm[ii+i1+perm[jj+j1+perm[kk+k1+perm[ll+l1]]]] % 32;
			int gi2 = perm[ii+i2+perm[jj+j2+perm[kk+k2+perm[ll+l2]]]] % 32;
			int gi3 = perm[ii+i3+perm[jj+j3+perm[kk+k3+perm[ll+l3]]]] % 32;
			int gi4 = perm[ii+1+perm[jj+1+perm[kk+1+perm[ll+1]]]] % 32;
			// Calculate the contribution from the five corners
			double t0 = 0.5 - x0*x0 - y0*y0 - z0*z0 - w0*w0;
			if(t0<0) n0 = 0.0;
			else {
				t0 *= t0;
				n0 = t0 * t0 * dot(grad4[gi0], x0, y0, z0, w0);
			}
			double t1 = 0.6 - x1*x1 - y1*y1 - z1*z1 - w1*w1;
			if(t1<0) n1 = 0.0;
			else {
				t1 *= t1;
				n1 = t1 * t1 * dot(grad4[gi1], x1, y1, z1, w1);
			}
			double t2 = 0.6 - x2*x2 - y2*y2 - z2*z2 - w2*w2;
			if(t2<0) n2 = 0.0;
			else {
				t2 *= t2;
				n2 = t2 * t2 * dot(grad4[gi2], x2, y2, z2, w2);
			}   double t3 = 0.6 - x3*x3 - y3*y3 - z3*z3 - w3*w3;
			if(t3<0) n3 = 0.0;
			else {
				t3 *= t3;
				n3 = t3 * t3 * dot(grad4[gi3], x3, y3, z3, w3);
			}
			double t4 = 0.6 - x4*x4 - y4*y4 - z4*z4 - w4*w4;
			if(t4<0) n4 = 0.0;
			else {
				t4 *= t4;
				n4 = t4 * t4 * dot(grad4[gi4], x4, y4, z4, w4);
			}
			// Sum up and scale the result to cover the range [-1,1]
			return 27.0 * (n0 + n1 + n2 + n3 + n4);
		}
	}
}
