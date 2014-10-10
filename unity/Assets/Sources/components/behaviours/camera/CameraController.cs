using UnityEngine;

namespace Assets.Sources.components.behaviours.camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform Bounds;
        private Camera _camera;

        public Camera Camera
        {
            get { return _camera ?? (_camera = Camera.main); }
        }

        public void Move(Vector2 deltaPosition)
        {
//            Debug.Log(deltaPosition);
//            Camera.main.transform.Translate(Camera.main.ScreenToWorldPoint(deltaPosition));
        }
    }
}