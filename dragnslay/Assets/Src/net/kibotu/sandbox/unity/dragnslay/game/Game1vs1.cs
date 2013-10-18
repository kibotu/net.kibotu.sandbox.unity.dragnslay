using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : MonoBehaviour {

        static void createWorld()
        {
            float scale = 15;

            Orb island1 = OrbFactory.createIsland();
            island1.go.transform.position = new Vector3(50, 50, 0);
            island1.go.transform.localScale = new Vector3(scale, scale, scale / 2);

            Orb island2 = OrbFactory.createIsland();
            island2.go.transform.Translate(50, 350  , 0);
            island2.go.transform.localScale = new Vector3(scale, scale, scale / 2);

            Orb island3 = OrbFactory.createIsland();
            island3.go.transform.position = new Vector3(400, 50, 0);
            island3.go.transform.localScale = new Vector3(scale, scale, scale / 2);

            Orb island4 = OrbFactory.createIsland();
            island4.go.transform.position = new Vector3(400, 350, 0);
            island4.go.transform.localScale = new Vector3(scale, scale, scale / 2);

            //Planet [] p = new Planet[10] { n	ew Planet() };
            // add planets to stage

            // spawn ships

            // touch events
        }

        void Start () {
            createWorld();
        }
	
        void Update () {

            /*foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began) {
                    // Construct a ray from the current touch coordinates
                    var ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray))
                    {
                        // Create a particle if hit
                        //Instantiate (particle, transform.position, transform.rotation);
                        Debug.Log(touch.position);
                    }
                }
            }*/
        }
    }
}
