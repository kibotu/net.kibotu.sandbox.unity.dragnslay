using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof (SetCameraFOV))]
    public class SetCameraFOVActionEditor : CustomActionEditor
    {
        public override bool OnGUI()
        {
            return DrawDefaultInspector();
        }

        public override void OnSceneGUI()
        {
            var setCameraFOVAction = (SetCameraFOV) target;

            if (setCameraFOVAction.fieldOfView.IsNone)
            {
                return;
            }

            var go = setCameraFOVAction.Fsm.GetOwnerDefaultTarget(setCameraFOVAction.gameObject);
            var fov = setCameraFOVAction.fieldOfView.Value;

            if (go != null && fov > 0)
            {
                var cam = go.camera;
                if (cam != null)
                {
                    var originalFOV = cam.fieldOfView;
                    cam.fieldOfView = setCameraFOVAction.fieldOfView.Value;

                    Handles.color = new Color(1, 1, 0, .5f);
                    SceneGUI.DrawCameraFrustrum(cam);

                    cam.fieldOfView = originalFOV;
                }
            }
        }
    }
}
