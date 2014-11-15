using UnityEngine;

namespace Assets.Sources
{
    public class Sensor : MonoBehaviour
    {

        public float Speed = 0.1f;

        void Update () {
            transform.Translate(Input.acceleration.x*Speed, 0, -Input.acceleration.z*Speed);
//            transform.rotation = Quaternion.Slerp(transform.rotation, cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);
        }

        private Quaternion GetRotFix()
        {
            if (Screen.orientation == ScreenOrientation.Portrait)
                return Quaternion.identity;
            if (Screen.orientation == ScreenOrientation.LandscapeLeft
            || Screen.orientation == ScreenOrientation.Landscape)
                return Quaternion.Euler(0, 0, -90);
            if (Screen.orientation == ScreenOrientation.LandscapeRight)
                return Quaternion.Euler(0, 0, 90);
            if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
                return Quaternion.Euler(0, 0, 180);
            return Quaternion.identity;
        }

        private static Quaternion ConvertRotation(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }
    }
}
