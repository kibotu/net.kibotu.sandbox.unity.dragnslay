using UnityEngine;

namespace Assets.Sources.components.behaviours.camera
{
    // Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
    // for using the mouse displacement for calculating the amount of camera movement and panning code.
    // @see https://gist.github.com/JISyed/5017805
    public class MoveCamera : MonoBehaviour
    {
        //
        // VARIABLES
        //

        public float TurnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
        public float PanSpeed = 4.0f;		// Speed of the camera when being panned
        public float ZoomSpeed = 4.0f;		// Speed of the camera going back and forth

        private Vector3 _mouseOrigin;	    // Position of cursor when mouse dragging starts
        private bool _isPanning;		    // Is the camera being panned?
        private bool _isRotating;	        // Is the camera being rotated?
        private bool _isZooming;		    // Is the camera zooming?
        private bool _isResetting;          // Is the camera resetting to initial position?

        private Vector3 origCamera;
        private Vector3 startCameraPosition;
        private float _startTime;
        private float Velocity = 0.75f;
        public bool InvertMouse = true;

        void Start()
        {
            origCamera = Camera.main.transform.position;
        }

        void Update()
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

            if (Input.GetMouseButtonDown(2) && !_isResetting)
            {
                startCameraPosition = Camera.main.transform.position;
                _isResetting = true;
                _startTime = 0;
            }
            
            // Disable movements on button release
//            if (!Input.GetMouseButton(0)) _isRotating = false;
            if (!Input.GetMouseButton(0)) _isPanning = false;
            if (!Input.GetMouseButton(1)) _isZooming = false;

            // Rotate camera along X and Y axis
            if (_isRotating)
            {
                var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);

                transform.RotateAround(transform.position, transform.right, -pos.y * TurnSpeed);
                transform.RotateAround(transform.position, Vector3.up, pos.x * TurnSpeed);
            }

            // Move the camera on it's XY plane
            if (_isPanning)
            {
                var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);

                var move = new Vector3(pos.x * PanSpeed * (InvertMouse ? -1 : 1), pos.y * PanSpeed * (InvertMouse ? -1 : 1), 0);
                transform.Translate(move, Space.Self);
            }

            // Move the camera linearly along Z axis
            if (_isZooming)
            {
                var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);

                var move = pos.y * ZoomSpeed * (InvertMouse ? 1 : -1) * transform.forward;
                transform.Translate(move, Space.World);
            }

            if (_isResetting)
            {
                _startTime += Time.deltaTime;
                Camera.main.transform.position = new Vector3
                {
                    x = Mathf.Lerp(startCameraPosition.x, origCamera.x, Easing.Sinusoidal.easeOut(_startTime * Velocity)),
                    y = Mathf.Lerp(startCameraPosition.y, origCamera.y, Easing.Sinusoidal.easeOut(_startTime * Velocity)),
                    z = Mathf.Lerp(startCameraPosition.z, origCamera.z, Easing.Sinusoidal.easeOut(_startTime * Velocity))
                };

                if (Vector3.Distance(Camera.main.transform.position, origCamera) <= 0.01f)
                {
                    _isResetting = false;
                }
            }
        }
    }
}

