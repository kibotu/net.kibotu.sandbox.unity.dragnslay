using Assets.net.kibotu.sandbox.unity.dragnslay.menu;
using Assets.net.kibotu.sandbox.unity.dragnslay.model;
using Assets.net.kibotu.sandbox.unity.dragnslay.network;
using Assets.net.kibotu.sandbox.unity.dragnslay.scripts;
using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : MonoBehaviour {
	
        public Texture btnTexture;
        private bool isRunning;
	
        void OnGUI() {
         
        }
	
        void startGame() {

            ClientSocket.Instance.Connect("http://127.0.0.1:3000/");

            Orb a = createOrb(new Vector3(0,2,0));
            //Orb b = createOrb(new Vector3(5,0,0));

            ClientSocket.Instance.On("message", (data) =>
                { if (data != null) Debug.Log("received message : " + data); });

		
            //Planet [] p = new Planet[10] { n	ew Planet() };
            // add planets to stage
		
            // spawn ships
		
            // touch events
        }
	
        public bool isDragging = false;
	
        public void OnMouseDown () {
            Debug.Log("OnMouseDown");
        }
	
        public void OnMouseDrag () {
	
            Debug.Log("OnMouseDrag");
		
            if (!isDragging) {
                //Do something here
                isDragging = true;
            }
        }
	
        public void OnMouseUp () {
            Debug.Log("OnMouseUp");
            isDragging = false;

        }
	
        void Start () {
            isRunning = false;
            /*Particle[] particles = particleEmitter.particles;
	    int i = 0;
	    while (i < particles.Length) {
	        float yPosition = Mathf.Sin(Time.time) * Time.deltaTime;
	        particles[i].position += new Vector3(0, yPosition, 0);
	        particles[i].color = Color.red;
	        particles[i].size = Mathf.Sin(Time.time) * 0.2F;
	        i++;
	    }
	    particleEmitter.particles = particles;
		
	  	originalColor = renderer.sharedMaterial.color;*/
        }
	
        void Update () {

            if (!isRunning)
            {
                Orb a = createOrb(new Vector3(0, 0, 0));
                isRunning = true;
            }

		
		
            /*foreach (Touch touch in Input.touches) {
			Debug.Log(touch.position);
			if (touch.phase == TouchPhase.Began) {
					// Construct a ray from the current touch coordinates
				var ray = Camera.main.ScreenPointToRay (touch.position);
				if (Physics.Raycast (ray)) {
					// Create a particle if hit
					Instantiate (particle, transform.position, transform.rotation);
					Debug.Log(touch.position);
				}
				Debug.Log(touch.position);
			}
		}*/
        }
	
        public static Orb createOrb(Vector3 position) {
            Orb orb = new Orb();
            orb.currentPopulation = 0;
            orb.maxPopulation = 10;
            orb.textureId = 0;
            orb.spawnPerSec = 0.5f;
            orb.life = new Life();
            orb.life.current_hp = orb.life.max_hp = 100;
            orb.life.armor = orb.life.current_shield = orb.life.max_shield = 0;
            orb.life.shield_regen = orb.life.hp_regen = 0f;
            orb.physicalProperty = new PhysicalProperty();
            orb.physicalProperty.acceleration = 0f;
		
            orb.physicalProperty.position = position;
            orb.physicalProperty.scalling = new Vector3(1,1,1);
            orb.physicalProperty.rotation = new Quaternion(0,0,0,0);
		
            orb.physicalProperty.mass = 0f;
            orb.physicalProperty.rotationSpeed = 23f;
            orb.physicalProperty.rotationDistance = 0f;
		
            orb.type = new TrabantPrototype();
            orb.type.physicalProperty = new PhysicalProperty();
            orb.type.physicalProperty.rotationDistance = 0f;
            orb.type.physicalProperty.position = new Vector3(1.5f,0,0);

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //Mesh mesh = (Mesh) Resources.Load("resources/meshes/island");
            //Instantiate(Transform, new Vector3(x, y, 0), Quaternion.identity);

            GameObject go = new GameObject("island");
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load("meshes/island", typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load("meshes/island", typeof(Texture)) as Texture;

            orb.go = go;
            //go.renderer.material.mainTexture = Resources.Load("glass") as Texture;
            go.transform.position = orb.physicalProperty.position;
		
            go.AddComponent<Orbitting>();
            go.AddComponent<SendUnits>();
		
            //GameObject cube = new GameObject("spaseship");
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
            cube.transform.position = orb.type.physicalProperty.position;
            cube.transform.parent = go.transform;
		
            return orb;
        }
    }
}
