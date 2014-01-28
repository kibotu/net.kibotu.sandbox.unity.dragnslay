using UnityEngine;

/**
 * http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
 */
namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        public Camera referenceCamera;

        public enum Axis { up, down, left, right, forward, back };
        public bool reverseFace = false;
        public Axis axis = Axis.left;

        // return a direction based upon chosen axis
        public Vector3 GetAxis(Axis refAxis)
        {
            switch (refAxis)
            {
                case Axis.down:
                    return Vector3.down;
                case Axis.forward:
                    return Vector3.forward;
                case Axis.back:
                    return Vector3.back;
                case Axis.left:
                    return Vector3.left;
                case Axis.right:
                    return Vector3.right;
            }

            // default is Vector3.up
            return Vector3.up;
        }

        public void Awake()
        {
            // if no camera referenced, grab the main camera
            if (!referenceCamera)
                referenceCamera = Camera.main;
        }

        public void Update()
        {
            // rotates the object relative to the camera
            Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
            Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);
            transform.LookAt(targetPos, targetOrientation);
        }
    }
}
