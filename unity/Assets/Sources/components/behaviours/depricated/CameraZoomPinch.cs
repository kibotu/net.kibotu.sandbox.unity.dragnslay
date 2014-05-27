using System;
using UnityEngine;

namespace Assets.Sources.components
{
    [Obsolete("Not used anymore", false)]
    internal class CameraZoomPinch : MonoBehaviour
    {
        public float MAXSCALE = 5.0F;
        public float MINSCALE = 2.0F;
        private Vector2 curDist = new Vector2(0, 0);
        public float minPinchSpeed = 5.0F;
        private Vector2 prevDist = new Vector2(0, 0);
        public Camera selectedCamera;
        public int speed = 4;
        private float speedTouch0;
        private float speedTouch1;
        private float touchDelta;
        public float varianceInDistances = 5.0F;
        public float fieldOfView;

        public void Update()
        {
            fieldOfView = selectedCamera.fieldOfView;

            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                //current distance between finger touches
                curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
                //difference in previous locations using delta positions
                prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
                    
                touchDelta = curDist.magnitude - prevDist.magnitude;
                speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude/Input.GetTouch(0).deltaTime;
                speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude/Input.GetTouch(1).deltaTime;


                Debug.Log("speedTouch0" + speedTouch0 + " speedTouch1 " + speedTouch1 + "magnitude1" + Input.GetTouch(1).deltaPosition.magnitude + "deltaTime1 " + Input.GetTouch(1).deltaTime);
                if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {
                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + speed, 15, 90);
                }

                if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {
                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - speed, 15, 90);
                }
            }
        }
    }
}