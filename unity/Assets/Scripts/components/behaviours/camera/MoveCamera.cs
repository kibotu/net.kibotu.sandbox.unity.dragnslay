using System;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace Assets.Sources.components.behaviours.camera
{
    // Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
    // for using the mouse displacement for calculating the amount of camera movement and panning code.
    // @see https://gist.github.com/JISyed/5017805
    public class MoveCamera : MonoBehaviour
    {
        public float TurnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
        public float PanSpeed = 2.0f;		// Speed of the camera when being panned
        public float ZoomSpeed = 4.0f;		// Speed of the camera going back and forth

        private Vector3 _mouseOrigin;	    // Position of cursor when mouse dragging starts
        private bool _isPanning;		    // Is the camera being panned?
        private bool _isRotating;	        // Is the camera being rotated?
        private bool _isZooming;		    // Is the camera zooming?
        private bool _isResetting;          // Is the camera resetting to initial position?

        public bool InvertMouse = true;

        public Camera Cam;
        public Transform Bounds;
        public float BoxRatio = Mathf.PI / 2;
        public Vector3 Velocity = new Vector3(0,0,0);
        public Vector3 Friction = new Vector3(0.1f,0.1f,0.1f);

        public void Update()
        {
            ApplyFriction();

            Camera.main.transform.position = SetBounds();
        }

        private void ApplyFriction()
        {
            if (Velocity.sqrMagnitude < 0.01f)
                return;
            
            Velocity.x *= Mathf.Pow(Friction.x, Time.deltaTime);
            Velocity.y *= Mathf.Pow(Friction.y, Time.deltaTime);
            Velocity.z *= Mathf.Pow(Friction.z, Time.deltaTime);

            if (Mathf.Abs(Velocity.x) < 0.01f) Velocity.x = 0;
            if (Mathf.Abs(Velocity.y) < 0.01f) Velocity.y = 0;
            if (Mathf.Abs(Velocity.z) < 0.01f) Velocity.z = 0;

            Cam.transform.Translate(Velocity, Space.Self);
        }

        public void UpdateCamera()
        {
            UpdateInputState();

            // Rotate camera along X and Y axis
            if (_isRotating)
                Rotate();

            // Move the camera on it's XY plane
            if (_isPanning)
                Fling();

            // Move the camera linearly along Z axis
            if (_isZooming)
                Zoom();
        }

        private void UpdateInputState()
        {
            _isPanning = Input.GetMouseButton(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;

            if (Input.GetMouseButtonDown(0))
                _mouseOrigin = Input.mousePosition;

            // Get mouse origin
            _isZooming = Input.GetMouseButtonDown(1);
        }

        public float GetZoom()
        {
            return Mathf.Abs(Cam.transform.position.z / (Bounds.position.z + Bounds.transform.localScale.z));
        }

        private Vector2 prevMousePosition;

        private void Fling()
        {
            if (Vector2.Distance(prevMousePosition, Input.mousePosition) < 0.1f)
                return;

            prevMousePosition = Input.mousePosition;
            
            var pos = Cam.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin); // Input.GetTouch(0).deltaPosition);
            Velocity = new Vector3((InvertMouse ? -1 : 1) * pos.x * GetZoom() * PanSpeed, (InvertMouse ? -1 : 1) * pos.y * GetZoom() * PanSpeed);
        }

        private void Rotate()
        {
            var pos = Cam.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);
            Cam.transform.RotateAround(Cam.transform.position, Cam.transform.right, -pos.y*TurnSpeed);
            Cam.transform.RotateAround(Cam.transform.position, Vector3.up, pos.x*TurnSpeed);
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

