using UnityEditor;
using UnityEngine;

// http://forum.unity3d.com/threads/31757-Unity3d-Editor-camera
namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.editor
{
    public static class SceneCameraMenuItem
    {
        private static Camera _sceneCamera = null;

        [MenuItem("Scene/Set Camera")]
        public static void SetMainToScene()
        {

            if (Camera.main == null)
            {
                Debug.Log("Main camera not found.");
                return;

            }

            if (_sceneCamera == null)
            {
                if (Camera.current == null || Camera.current.name != "SceneCamera")
                {
                    Debug.Log("Scene camera not selected. First click on Scene tab before calling.");
                    return;
                }

                _sceneCamera = Camera.current;
            }

            Camera.main.transform.position = _sceneCamera.transform.position;
            Camera.main.transform.rotation = _sceneCamera.transform.rotation;
        }
    }
}