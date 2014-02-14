using UnityEngine;

// http://itween.pixelplacement.com/documentation.php
namespace Assets.Sources.components.behaviours
{
    public class PlaneMovement : MonoBehaviour
    {

        public Vector3 up;
        public Vector3 forward;
        public string pathName;
        public float time;

        void Start ()
        {

            //transform.rotation = transform.rotation * Quaternion.Euler(-90f, 0, 0) * Quaternion.Euler(0, 180, 0) ;

            //iTween.RotateBy(gameObject, iTween.Hash("y", .25, "time", 2, "easeType", "easeInOutQuad", "loopType", "pingPong", "delay", .2));

            //   iTween.MoveTo(gameObject, iTween.Hash("x", 6, "time", 4, "loopType", "pingPong", "delay", .4, "easeType", "easeInOutQuad"));

            //  iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("wurst"), "time", 100));

            // iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("wurst"), "orienttopath", true,  "islocal",true, "lookahead", .1f, "time", 100));

            iTween.MoveTo(gameObject,
                iTween.Hash(
                    "path", iTweenPath.GetPath(pathName), 
                    "time", time, 
                    "orientToPath", true, 
                    "lookTime", 0.1f, 
                    "lookahead", 0.1f));/*
        transform.forward = Vector3.left;
        transform.up = Vector3.up;
        up = transform.up;
        forward = transform.forward;*/
	
        }   
	
        void Update ()
        {
            // transform.forward = forward;
        }
    }
}
