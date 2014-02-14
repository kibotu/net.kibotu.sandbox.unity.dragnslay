using Assets.Sources.components.behaviours.combat;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    class Rocket : MonoBehaviour
    {
        public Vector3 Attacker;
        public int AttackDamage;
        public GameObject Defender;
        public float Distance;
        public float Velocity;
        public float Acceleration;
        private float _startTime;
        private float _startVelocity;

        public void Start()
        {
            transform.position = Attacker;
            _startVelocity = Velocity;
        }

        public void Update()
        {
            _startTime += Time.deltaTime;
            if (_startTime >= 0.25f)
            {
                Velocity = GetVelocity(Acceleration, _startVelocity);
                _startTime = 0;
            }

            if (Defender == null) Destroy(gameObject);
            if (!(Vector3.Distance(transform.position, Defender.transform.position) > Distance))
            {
                Defender.GetComponent<Defence>().Defend(AttackDamage);
                Destroy(gameObject);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, Defender.transform.position, Time.deltaTime * Velocity);
            transform.up = -Direction(transform.position, Defender.transform.position); // rotate round vec3(0,-1,0);
            transform.GetChild(0).up = -Camera.main.transform.forward; // inside rocket billboard to vec3(0,0,-1) 
        }

        public static Vector3 Direction(Vector3 source, Vector3 target)
        {
            return (source - target).normalized;
        }

        public static float GetVelocity(float acceleration, float startVelocity = 0f)
        {
            return startVelocity + acceleration*Time.time;
        }
    }
}