using Assets.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : MonoBehaviour {
	
        public Texture btnTexture;
	
        void OnGUI() {
         
        }

        void Awake()
        {
        }

        static void createWorld()
        {

            //ClientSocket.Instance.Connect("http://127.0.0.1:3000/");

            /*ClientSocket.Instance.On("message", (data) =>
            {
                if (data != null)
                {
                    Debug.Log("received message : " + data); 
                }
            });*/



            Orb island1 = OrbFactory.createIsland();
            island1.go.transform.position = new Vector3(1, 1, 0);

            Orb island2 = OrbFactory.createIsland();
            island2.go.transform.Translate(1, 20, 0);


            Orb island3 = OrbFactory.createIsland();
            island3.go.transform.position = new Vector3(20, 1, 0);

            Orb island4 = OrbFactory.createIsland();
            island4.go.transform.position = new Vector3(20, 20, 0);
            

            //Planet [] p = new Planet[10] { n	ew Planet() };
            // add planets to stage

            // spawn ships

            // touch events
        }

        void Start () {
            createWorld();

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
    }
}
