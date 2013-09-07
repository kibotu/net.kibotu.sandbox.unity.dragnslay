using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.scripts
{
    public class ParticleScript : MonoBehaviour {

        GameObject goEmitter;
        ParticleRenderer particleRender;
        Color mouseOverColor = Color.blue;
        private Color originalColor;
     
        void Start () {
            originalColor = renderer.sharedMaterial.color;
        }
        void OnMouseEnter () {
            renderer.material.color = mouseOverColor;
       
        }
     
        void OnMouseExit () {
            renderer.material.color = originalColor;
        }
     
        void OnMouseDown () {
            Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            while (Input.GetMouseButton(0))
            {
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
                transform.position = curPosition;
            }
        }
     
        void Update () {
            goEmitter = GameObject.Find ("Lightning1");
       
            if (Input.GetMouseButtonDown (0)) {
                particleRender.enabled = true;
                Debug.Log("draggin");
            }
     
            else if (Input.GetMouseButtonUp (0))
                particleRender.enabled = false;
     
     
        }
    }
}