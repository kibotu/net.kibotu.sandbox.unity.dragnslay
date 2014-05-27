using UnityEngine;
using Assets.Sources.utility;

namespace Assets.Sources.components.behaviours
{
    public class MoveToTarget : MonoBehaviour
    {
        public GameObject Target;
        public float Velocity = 20f;
        public float RotationVelocity = 50f;

        private Orbiting _orbiting;
        private Vector3 _finalDestination;

        private PlayMakerFSM fsm;

        public void Start()
        {
            // transform.parent = target.transform; instantly sending back possible however if called too fast, nullpointer, also problematique with attacking while flying
            _orbiting = gameObject.GetComponent<Orbiting>();
            _orbiting.Center = Target.transform;
            _finalDestination = _orbiting.GetFinalDestination();
            _orbiting.enabled = false;

            fsm = GetComponent<PlayMakerFSM>();
            fsm.SendEvent("Move");
        }

        public void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _finalDestination, Time.deltaTime * Velocity);

            var targetDir = transform.position.Direction(_finalDestination);
            var step = RotationVelocity * Time.deltaTime;
            var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            //Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);

            if (Vector3.Distance(transform.position, _finalDestination) < 0.01f)
            {
                transform.parent = Target.transform;
                _orbiting.enabled = true;
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
