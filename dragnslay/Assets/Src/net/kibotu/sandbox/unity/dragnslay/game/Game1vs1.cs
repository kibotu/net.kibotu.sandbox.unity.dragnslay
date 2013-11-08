using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : MonoBehaviour {

        static void CreateWorld()
        {
            const float scale = 50;

            var island1 = GameObjectFactory.CreateIsland();
            island1.transform.position = new Vector3(50, 50, 0);
            island1.transform.localScale = new Vector3(scale, scale, scale);

            var island2 = GameObjectFactory.CreateIsland();
            island2.transform.Translate(50, 350  , 0);
            island2.transform.localScale = new Vector3(scale, scale, scale);

            var island3 = GameObjectFactory.CreateIsland();
            island3.transform.position = new Vector3(400, 50, 0);
            island3.transform.localScale = new Vector3(scale, scale, scale);

            var island4 = GameObjectFactory.CreateIsland();
            island4.transform.position = new Vector3(400, 350, 0);
            island4.transform.localScale = new Vector3(scale, scale, scale);

            //Planet [] p = new Planet[10] { n	ew Planet() };
            // add planets to stage

            // spawn ships

            // touch events
        }

        public void Start () {
            CreateWorld();
        }

        public void Update()
        {
        }
    }
}
