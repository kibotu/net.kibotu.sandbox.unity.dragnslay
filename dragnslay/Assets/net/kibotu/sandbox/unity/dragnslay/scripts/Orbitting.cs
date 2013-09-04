using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.scripts
{
    public class Orbitting : MonoBehaviour {

        void Start () {
        }
	
        void Update () {
            transform.Rotate(new Vector3(0,1,0) * Time.deltaTime * 100, Space.World);
        }
    }
}
