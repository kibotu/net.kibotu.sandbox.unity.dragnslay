using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.scripts
{
    public class Orbitting : MonoBehaviour {

        void Start () {
        }
	
        void Update () {
            transform.Rotate(Vector3.forward * Time.deltaTime * 100, Space.World);
        }
    }
}
