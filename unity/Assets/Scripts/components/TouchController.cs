using Assets.Sources.components.behaviours;
using Assets.Sources.components.behaviours.camera;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components
{
    public class TouchController : MonoBehaviour
    {
        public enum Mode
        {
            Idle,
            Selecting,
            CameraMovement
        }

        public MoveCamera CamCtrl;
        public Mode ControlMode = Mode.Idle;

        public SelectionController SelCtrl;
        private GameObject _currentSelected;

        private void Update()
        {
            UpdateTouchMode();

            switch (ControlMode)
            {
                case Mode.CameraMovement:
                    CamCtrl.UpdateCamera();
                    break;
                case Mode.Selecting:
                    // 1) on hit => selecting mode
                    // 2) else camera movement
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //  | Input.GetTouch(0).position
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (_currentSelected == null)
                        {
                            _currentSelected = hit.transform.gameObject;
                            SelCtrl.ToggleAddToList(_currentSelected);
                        }
                    }
                    else
                    {
                        _currentSelected = null;

                        // move camera if nothing has been selected at first touch
                        if (SelCtrl.Selected.IsEmpty())
                        {
                            ControlMode = Mode.CameraMovement;
                            CamCtrl.UpdateCamera();
                        }
                    }

                    break;
                case Mode.Idle:
                default:
                    if (_currentSelected != null)
                    {
                        SelCtrl.FinalTarget(_currentSelected);
                        _currentSelected = null;
                    }
                    else
                        SelCtrl.DeselectAll();

                    break;
            }
        }

        public Vector3 TouchInputToWorld()
        {
            Vector3 inputPosScreen = Input.mousePosition; // Input.GetTouch(0).position;
            inputPosScreen.z = -CamCtrl.Cam.transform.position.z;
            return CamCtrl.Cam.ScreenToWorldPoint(inputPosScreen);
        }

        private void UpdateTouchMode()
        {
            if(Input.GetMouseButtonUp(0))
                ControlMode = Mode.Idle;

            if (ControlMode != Mode.Idle) return;

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

            ControlMode = Input.GetMouseButton(0)
                ? Input.GetMouseButton(1) ? Mode.CameraMovement : Mode.Selecting
                : Mode.Idle;
        }
    }
}