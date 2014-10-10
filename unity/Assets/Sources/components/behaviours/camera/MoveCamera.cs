using UnityEngine;

namespace Assets.Sources.components.behaviours.camera
{
    // Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
    // for using the mouse displacement for calculating the amount of camera movement and panning code.
    // @see https://gist.github.com/JISyed/5017805
    public class MoveCamera : MonoBehaviour
    {
        public float TurnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
        public float PanSpeed = 4.0f;		// Speed of the camera when being panned
        public float ZoomSpeed = 4.0f;		// Speed of the camera going back and forth

        private Vector3 _mouseOrigin;	    // Position of cursor when mouse dragging starts
        private bool _isPanning;		    // Is the camera being panned?
        private bool _isRotating;	        // Is the camera being rotated?
        private bool _isZooming;		    // Is the camera zooming?
        private bool _isResetting;          // Is the camera resetting to initial position?

        private float Velocity = 0.75f;
        public bool InvertMouse = true;

        public Camera Cam;
        public Transform Bounds;
        public float BoxRatio = Mathf.PI / 2;

        public void UpdateCamera()
        {
            UpdateInputState();

            // Rotate camera along X and Y axis
            if (_isRotating)
                Rotate();

            // Move the camera on it's XY plane
            if (_isPanning && !_isZooming)
                Pan();

            // Move the camera linearly along Z axis
            if (_isZooming)
                Zoom();

            Cam.transform.position = SetBounds();
        }

        private void UpdateInputState()
        {
// Get the left mouse button
//            if (Input.GetMouseButtonDown(0))
//            {
//                // Get mouse origin
//                mouseOrigin = Input.mousePosition;
//                isRotating = true;
//            }

            // Get the right mouse button
            if (Input.GetMouseButtonDown(0))
            {
                // Get mouse origin
                _mouseOrigin = Input.mousePosition;
                _isPanning = true;
            }

            // Get the middle mouse button
            if (Input.GetMouseButtonDown(1))
            {
                // Get mouse origin
                _mouseOrigin = Input.mousePosition;
                _isZooming = true;
            }

            // Disable movements on button release
//            if (!Input.GetMouseButton(0)) _isRotating = false;
            if (!Input.GetMouseButton(0)) _isPanning = false;
            if (!Input.GetMouseButton(1)) _isZooming = false;
        }

        private void Rotate()
        {
            var pos = Cam.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);
            Cam.transform.RotateAround(Cam.transform.position, Cam.transform.right, -pos.y*TurnSpeed);
            Cam.transform.RotateAround(Cam.transform.position, Vector3.up, pos.x*TurnSpeed);
        }

        private void Pan()
        {
            var pos = Cam.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);
            var move = new Vector3(pos.x*PanSpeed*(InvertMouse ? -1 : 1), pos.y*PanSpeed*(InvertMouse ? -1 : 1), 0);
            Cam.transform.Translate(move, Space.Self);
        }

        private void Zoom()
        {
            var pos = Cam.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);
            var move = pos.y*ZoomSpeed*(InvertMouse ? 1 : -1)*Cam.transform.forward;
            Cam.transform.Translate(move, Space.World);
        }

        private Vector3 SetBounds()
        {
            var cP = Cam.transform.position;
            cP.x = Mathf.Clamp(cP.x, Bounds.position.x - Bounds.localScale.x / BoxRatio, Bounds.position.x + Bounds.localScale.x / BoxRatio);
            cP.y = Mathf.Clamp(cP.y, Bounds.position.y - Bounds.localScale.y / BoxRatio, Bounds.position.y + Bounds.localScale.y / BoxRatio);
            cP.z = Mathf.Clamp(cP.z, Bounds.position.z - Bounds.localScale.z / BoxRatio, Bounds.position.z + Bounds.localScale.z / BoxRatio);
            return cP;
        }
    }
}

