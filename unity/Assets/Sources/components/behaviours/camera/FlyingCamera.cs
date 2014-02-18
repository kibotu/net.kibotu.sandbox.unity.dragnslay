using System;
using UnityEngine;

namespace Assets.Sources.components.behaviours.camera
{
    /**
     * http://forum.unity3d.com/threads/167604-Simplle-Free-Fly-Camera-script
     */
    class FlyingCamera : MonoBehaviour
    {
        public float FlySpeed = 0.5f;
        public GameObject DefaultCam;
        public GameObject PlayerObject;
        public bool IsEnabled;
        public bool Shift;
        public bool Ctrl;
        public float AccelerationAmount = 3f;
        public float AccelerationRatio = 1f;
        public float SlowDownRatio = 0.5f;

        public void Start()
        {
            if (DefaultCam == null)
                DefaultCam = Camera.main.gameObject;

            if (!DefaultCam.GetComponent<MouseLook>())
                DefaultCam.AddComponent<MouseLook>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                Shift = true;
                FlySpeed *= AccelerationRatio;
            }
       
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                Shift = false;
                FlySpeed /= AccelerationRatio;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                Ctrl = true;
                FlySpeed *= SlowDownRatio;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                Ctrl = false;
                FlySpeed /= SlowDownRatio;
            }
            if (Math.Abs(Input.GetAxis("Vertical")) > 0.0001f)
            {
                transform.Translate(-DefaultCam.transform.forward * FlySpeed * Input.GetAxis("Vertical"));
            }
            if (Math.Abs(Input.GetAxis("Horizontal")) > 0.0001f)
            {
                transform.Translate(-DefaultCam.transform.right * FlySpeed * Input.GetAxis("Horizontal"));
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(DefaultCam.transform.up * FlySpeed*0.5f);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(-DefaultCam.transform.up * FlySpeed*0.5f);
            }
            if (Input.GetKeyDown(KeyCode.F12))
                switchCamera();
            if (Input.GetKeyDown(KeyCode.M))
                PlayerObject.transform.position = transform.position; //Moves the player to the flycam's position. Make sure not to just move the player's camera.
        }
    
        public void switchCamera()
        {
            if (!IsEnabled) //means it is currently disabled. code will enable the flycam. you can NOT use 'enabled' as boolean's name.
            {
                transform.position = DefaultCam.transform.position; //moves the flycam to the defaultcam's position
                DefaultCam.camera.enabled = false;
                camera.enabled = true;
                IsEnabled = true;
            }
            else if (IsEnabled) //if it is not disabled, it must be enabled. the function will disable the freefly camera this time.
            {
                camera.enabled = false;
                DefaultCam.camera.enabled = true;
                IsEnabled = false;
            }
        }
    }
}
