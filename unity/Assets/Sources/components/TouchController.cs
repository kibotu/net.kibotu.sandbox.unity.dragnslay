using Assets.Sources.components.behaviours;
using Assets.Sources.components.behaviours.camera;
using UnityEngine;

namespace Assets.Sources.components
{
    public class TouchController : MonoBehaviour {

        public enum Mode { Idle, Selecting, CameraMovement }

        public Mode ControlMode = Mode.Idle;

        public CameraController CamCtrl;
        public SelectionController SelCtrl;

        public GameObject go;

        void Update () {

            // figure out number of fingers
            // 1 finger => 
//            SelCtrl.ToggleAddToList();

            // 2 finger => 
            // CamCtrl.Move();
            // CamCtrl.Zoom();

            UpdateTouchMode();

            switch (ControlMode)
            {
                    case Mode.CameraMovement:
                        // 1) move
                        Debug.Log("moving camera");
                        CamCtrl.Move(Input.GetTouch(0).deltaPosition);
                        // 2) zoom
                        break;
                    case Mode.Selecting:
                        var inputPosWorld = TouchInputToWorld(); 
                        inputPosWorld.z = go.transform.position.z;
                        go.transform.position = inputPosWorld;
                        break;
                    case Mode.Idle:
                        break;
            }

//            if (Input.touchCount == 1)
//            {
//                if (Input.GetTouch(0).tapCount == 1)
//                {
//                    Debug.Log("Single Tap with one finger.");
//                    RaycastHit hit;
//                    var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
//                    if (Physics.Raycast(ray, out hit))
//                    {
//                        Debug.Log(hit.transform.name);
//                    }
//                }
//            }
        }

        public Vector3 TouchInputToWorld()
        {
            Vector3 inputPosScreen = Input.GetTouch(0).position;
            inputPosScreen.z = -CamCtrl.Camera.transform.position.z;
            return CamCtrl.Camera.ScreenToWorldPoint(inputPosScreen);
        }

        private void UpdateTouchMode()
        {
            switch (Input.touchCount)
            {
                case 2:
                    ControlMode = Mode.CameraMovement;
                    break;
                case 1:
                    ControlMode = Mode.Selecting;
                    break;
                case 0:
                default:
                    ControlMode = Mode.Idle;
                    break;
            }
        }
    }
}
