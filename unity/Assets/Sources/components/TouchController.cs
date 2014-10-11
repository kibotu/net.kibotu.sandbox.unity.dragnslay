using Assets.Sources.components.behaviours;
using Assets.Sources.components.behaviours.camera;
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

        public GameObject Dragable;
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
                    //  DragDrop();

                    // 1) on hit => selecting mode
                    // 2) else camera movement
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
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
                        ControlMode = Mode.CameraMovement;
                        CamCtrl.UpdateCamera();
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

        private void DragDrop()
        {
            Vector3 inputPosWorld = TouchInputToWorld();
            inputPosWorld.z = Dragable.transform.position.z;
            Dragable.transform.position = inputPosWorld;
        }

        public Vector3 TouchInputToWorld()
        {
            Vector3 inputPosScreen = Input.GetTouch(0).position;
            inputPosScreen.z = -CamCtrl.Cam.transform.position.z;
            return CamCtrl.Cam.ScreenToWorldPoint(inputPosScreen);
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