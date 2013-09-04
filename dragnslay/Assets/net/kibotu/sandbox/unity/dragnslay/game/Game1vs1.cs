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

        void Awake()
        {
            isRunning = false;
        }

        static void createWorld()
        {

            ClientSocket.Instance.Connect("http://127.0.0.1:3000/");

            ClientSocket.Instance.On("message", (data) =>
            {
                if (data != null)
                {
                    Debug.Log("received message : " + data); 
                    OrbFactory.createIsland();   
                }
            });

            Orb island1 = OrbFactory.createIsland();
            island1.go.transform.position = new Vector3(1, 1, 0);

                
            Orb island2 = OrbFactory.createIsland();
            island2.go.transform.position = new Vector3(1, 30, 0);


            Orb island3 = OrbFactory.createIsland();
            island3.go.transform.position = new Vector3(30, 1, 0);


            Orb island4 = OrbFactory.createIsland();
            island4.go.transform.position = new Vector3(30, 30, 0);


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


            if (!isRunning)
            {
                isRunning = true;
                //Debug.Log("start game1vs1");
               // createWorld();
            }

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
	
        /*public static Orb createOrb(Vector3 position) {

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //Mesh mesh = (Mesh) Resources.Load("resources/meshes/island");
            //Instantiate(Transform, new Vector3(x, y, 0), Quaternion.identity);

            GameObject go = new GameObject("island");
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load("meshes/island", typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load("meshes/island", typeof(Texture)) as Texture;

            // orb.go = go;
            //go.renderer.material.mainTexture = Resources.Load("glass") as Texture;
            //go.transform.position = orb.physicalProperty.position;
		
            go.AddComponent<Orbitting>();
            go.AddComponent<SendUnits>();
		
            //GameObject cube = new GameObject("spaseship");
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
            cube.transform.position = orb.type.physicalProperty.position;
            cube.transform.parent = go.transform;
		
            return orb;
        }*/
    }
}
