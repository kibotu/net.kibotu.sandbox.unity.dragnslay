using UnityEngine;

/**
 * http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
 */
namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        public Camera mCamera = Camera.main;

        public void Start()
        {
            //if (mCamera == null) mCamera = Camera.main;
        }

        public void Update()
        {
            transform.LookAt(transform.position + mCamera.transform.rotation * Vector3.back, mCamera.transform.rotation * Vector3.up);
        }
    }
}
