using System;
using Assets.Sources.game;
using UnityEngine;

namespace Assets.Sources.components.behaviours.legacy
{
    [Obsolete("Not used anymore", false)]
    class Move : MonoBehaviour {

        public float startTime;
        public float duration;
        public float speed;
        public Transform destination;
        public Vector3 oldPosition;

        public void Start()
        {
            duration = 1000;
            startTime = 0;
            oldPosition = gameObject.transform.localPosition;
        }

        public void Update()
        {
            if (!Game.IsRunning()) return;

            startTime += Time.deltaTime;
            //Debug.Log("move from: " + gameObject.transform.position + " to " + destination.position);

            gameObject.transform.position = move(destination.position, gameObject.transform.position, startTime, duration);
        }

        public void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("hit " + collision.gameObject.name + " contacts  " + collision.contacts.Length + " magnitute " + collision.relativeVelocity.magnitude);
            foreach (ContactPoint contact in collision.contacts)
            {
                if (destination.gameObject.GetInstanceID() != contact.otherCollider.gameObject.GetInstanceID())
                    return;

                gameObject.transform.parent = destination.gameObject.transform.parent;
                Debug.Log("this " + contact.thisCollider.gameObject.name + " hits " + contact.otherCollider.gameObject.transform.parent.gameObject.name);
                Destroy(this);
            }
        }

        public static float getY(float x)
        {
            return -(x * x) + 2 * x;
        }

        public static Vector3 move(Vector3 end, Vector3 start, float dt, float maxTime)
        {
            // (b - a) * t + a
            return Vector3.Lerp(start, end, Vector3.Distance(end, start) * dt / maxTime);
        }
    }
}
