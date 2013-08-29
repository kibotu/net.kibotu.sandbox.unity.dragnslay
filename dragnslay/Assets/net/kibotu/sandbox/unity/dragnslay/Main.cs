using UnityEngine;
using System;
using System.Collections;

public class Main : MonoBehaviour {
	
	public Texture btnTexture;
	private bool buttonIsVisible = true;
	private ClientSocket socket; 
	
    void OnGUI() {
        if (!btnTexture) {
            Debug.LogError("Please assign a texture on the inspector");
            return;
        }
        if (buttonIsVisible && GUI.Button(new Rect(10, 10, 400, 220), btnTexture)) {
			buttonIsVisible = false;
			startGame();
		}
    }
	
	void startGame() {
		
		Orb a = createOrb(new Vector3(0,2,0));
		//Orb b = createOrb(new Vector3(5,0,0));
		
		
		
		//Planet [] p = new Planet[10] { n	ew Planet() };
		// add planets to stage
		
		// spawn ships
		
		// touch events
		
		// server conection + transmitting touch events
		//socket = new ClientSocket();
		//socket.Execute();
	}
	
	public bool isDragging = false;
	
	public void OnMouseDrag () {
	
		if (!isDragging) {
    		//Do something here
    		isDragging = true;
		}
    }
	
	public void OnMouseUp () {
    	isDragging = false;

	}
	
	/*void connectToServer() {
		socket = new Client("http://127.0.0.1:3000");
		socket.Connect();
		
		
		socket.C((data) => { 
			Debug.Log("received message : " + data);
		});
		
		socket.On ("message", (data) => { 
			Debug.Log("received message : " + data);
		});
		
	 	socket.Emit("send", new JSONMessage("{ message : \"hallo world\", username : \"unity\" }")); //, "", (data) => { Debug.Log("successfully send event: " + data); } );	
	}*/
	
	void Start () {
		Particle[] particles = particleEmitter.particles;
	    int i = 0;
	    while (i < particles.Length) {
	        float yPosition = Mathf.Sin(Time.time) * Time.deltaTime;
	        particles[i].position += new Vector3(0, yPosition, 0);
	        particles[i].color = Color.red;
	        particles[i].size = Mathf.Sin(Time.time) * 0.2F;
	        i++;
	    }
	    particleEmitter.particles = particles;
	}
	
	private GameObject particle;
	
	void Update () {
		
		if(isDragging) Debug.Log("is dragging");
		
		
	  	foreach (Touch touch in Input.touches) {
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
		}
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

		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		orb.go = go;
        go.renderer.material.mainTexture = Resources.Load("glass") as Texture;
		go.transform.position = orb.physicalProperty.position;
		
		//go.AddComponent<Orbitting>();
		
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = orb.type.physicalProperty.position;
		cube.transform.parent = go.transform;
		
		return orb;
	}
}
