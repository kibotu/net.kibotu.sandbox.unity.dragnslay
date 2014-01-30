using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    public class RotateIsland : MonoBehaviour
    {
        public float xRot;
        public float yRot;
        public float zRot;
        public float speed;
        public float xSign;
        public float ySign;
        public float zSign;

        public void Awake()
        {
            xRot = Random.Range(0.8f, 1.6f);
            yRot = Random.Range(1.4f, 2f);
            zRot = Random.Range(1.9f, 2f);
            speed = Random.Range(1.2f, 1.8f);
            xSign = Mathf.Sign(Random.Range(-1, 1));
            ySign = Mathf.Sign(Random.Range(-1, 1));
            zSign = Mathf.Sign(Random.Range(-1, 1));
        }

        public void Start ()
        {
            if (transform.childCount > 0)
                transform.localRotation = Quaternion.Euler(270, 0, 0);
        }

        public void Update ()
        {
            if (!Game.IsRunning()) return;

            transform.Rotate(new Vector3(GetSwingingRotAngel(xRot) * xSign, GetSwingingRotAngel(yRot) * ySign, GetSwingingRotAngel(zRot) * zSign) * Time.deltaTime * speed, Space.World);
        }

        public static float GetSwingingRotAngel(float x)
        {
            return x * x * (1 + Mathf.Sin(Time.time)) - x * x;
        }
    }
}
