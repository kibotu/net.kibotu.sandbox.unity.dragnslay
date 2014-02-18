using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class MoveToTarget : MonoBehaviour
    {
        public GameObject target;
        public float Velocity = 7f;
        public float RotationVelocity = 50f;

        private Orbiting orbiting;
        private Vector3 finalDestination;

        private PlayMakerFSM fsm;

        public void Start()
        {
            // transform.parent = target.transform; instantly sending back possible however if called too fast, nullpointer, also problematique with attacking while flying
            orbiting = gameObject.GetComponent<Orbiting>();
            orbiting.Center = target.transform;
            finalDestination = orbiting.GetFinalDestination();
            orbiting.enabled = false;

            fsm = GetComponent<PlayMakerFSM>();
            fsm.SendEvent("Move");
        }

        public void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, finalDestination, Time.deltaTime * Velocity);

            var targetDir = transform.position.Direction(finalDestination);
            var step = RotationVelocity * Time.deltaTime;
            var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            //Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);

            if (Vector3.Distance(transform.position, finalDestination) < 0.01f)
            {
                transform.parent = target.transform;
                orbiting.enabled = true;
                fsm.SendEvent("Arrive");
                Destroy(this);
            }
        }

        public void Reset()
        {
            if (GetComponents<MoveToTarget>().Length > 1)
            {
                Invoke("DestroyThis", 0);
            }
        }

        public void DestroyThis()
        {
            DestroyImmediate(this);
        }
    }
}
