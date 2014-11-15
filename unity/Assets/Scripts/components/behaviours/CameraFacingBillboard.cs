using UnityEngine;

/**
 * http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
 */
namespace Assets.Sources.components.behaviours
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        public void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}