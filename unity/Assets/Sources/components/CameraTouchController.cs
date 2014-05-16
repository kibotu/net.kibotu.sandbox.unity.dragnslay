using System;
using UnityEngine;

namespace Assets.Sources.components
{
    internal class CameraTouchController : MonoBehaviour
    {
        public GameObject Camera;
        private Vector2 vecP0;
        private Vector2 vecP0toP1;
        private Vector2 vecP1;

        public void Update()
        {
            TouchDown();
            TouchDragged();
        }

        public bool TouchDown()
        {
            Debug.Log("fingersDown after \"touchDown\": " + Input.touchCount);
            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                vecP0 = Input.GetTouch(0).position;
                vecP1 = Input.GetTouch(1).position;
                vecP0toP1 = vecP1 - vecP0;
            }
            return true;
        }

        public bool TouchUp()
        {
            Debug.Log("fingersDown after \"touchUp\": " + Input.touchCount);
            if (Input.touchCount == 2)
            {
                vecP0 = Vector2.zero;
                vecP1 = Vector2.zero;
                vecP0toP1 = Vector2.zero;
            }
            return false;
        }

        public bool TouchDragged()
        {
            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var newVecP0 = Input.GetTouch(0).position;
                var newVecP1 = Input.GetTouch(1).position;

                var vecP0ToNewVecP0 = vecP0 - newVecP0;
                var vecP1ToNewVecP1 = vecP1 - newVecP1;

                var newVecP0ToP1 = newVecP1 - newVecP0;

                if (Math.Abs(vecP0ToNewVecP0.magnitude) > Double.Epsilon && vecP1ToNewVecP1.magnitude > Double.Epsilon)
                {
                    if (
                        Mathf.Abs(Vector2.Angle(vecP0ToNewVecP0, Vector2.zero) -
                                  Vector2.Angle(vecP1ToNewVecP1, Vector2.zero)) < 30)
                    {
                        var translationVector = new Vector3(-Mathf.Min(Input.GetTouch(0).deltaPosition.x, Input.GetTouch(1).deltaPosition.x)/100f,
                            Mathf.Min(Input.GetTouch(0).deltaPosition.y, Input.GetTouch(1).deltaPosition.y)/100f, 0f);
                        translationVector = Camera.transform.InverseTransformPoint(translationVector);
                        Camera.transform.position = new Vector3(translationVector.x, translationVector.y,translationVector.z);
                    }
                }
                if (Mathf.Abs(Vector2.Angle(vecP0ToNewVecP0, Vector2.zero) - Vector2.Angle(vecP1ToNewVecP1, Vector2.zero) - 180) < 20)
                {
                    var diff = vecP0toP1.magnitude - newVecP0ToP1.magnitude;
                    var zoomVector = Camera.transform.forward * -diff/50f;
                    Camera.transform.Translate(zoomVector.x, zoomVector.y, zoomVector.z);
                }
                var angle = Vector2.Angle(vecP0toP1, Vector2.zero) - Vector2.Angle(newVecP0ToP1, Vector2.zero);
                if (Mathf.Abs(angle) > 1.5)
                {
                    var rotationVector = Camera.transform.forward;
                    Camera.transform.Rotate(rotationVector, angle);
                }

                vecP0 = newVecP0;
                vecP1 = newVecP1;
                vecP0toP1 = newVecP0ToP1;
            }
            else
            {
                Camera.transform.Rotate(Camera.transform.up, Input.GetTouch(0).deltaPosition.x*Mathf.PI/180f);
                Camera.transform.Rotate(Vector3.Cross(Camera.transform.up, Camera.transform.forward), - Input.GetTouch(0).deltaPosition.y * Mathf.PI / 180f);
            }

            return true;
        }
    }
}