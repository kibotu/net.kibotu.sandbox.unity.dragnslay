using Assets.Sources.components.behaviours.combat;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class RocketMove : MonoBehaviour
    {
        public Vector3 Attacker;
        public float AttackDamage;
        public GameObject Defender;
        public float Velocity;
        public float Acceleration;
        private float _startTime;

        public void Start()
        {
            transform.position = Attacker;
        }

        public void Update()
        {
            // another ship has destroyed the target; Note: potential re-targeting
            if (Defender == null)
            {
                Destroy(gameObject);
                return;
            }

            // transform.position = Vector3.MoveTowards(transform.position, Defender.transform.position, Time.delta * Velocity); // not good enough interpolation
            _startTime += Time.deltaTime;
            // http://whydoidoit.com/2012/04/06/unity-curved-path-following-with-easing/
            transform.position = new Vector3
            {
                x = Mathf.Lerp(Attacker.x, Defender.transform.position.x, Easing.Sinusoidal.easeInOut(_startTime * Velocity)),
                y = Mathf.Lerp(Attacker.y, Defender.transform.position.y, Easing.Sinusoidal.easeInOut(_startTime * Velocity)),
                z = Mathf.Lerp(Attacker.z, Defender.transform.position.z, Easing.Sinusoidal.easeInOut(_startTime * Velocity))
            }; 

            // rotate along forward axe of camera towards target
            var dir = transform.position.Direction(Defender.transform.position);
         

            // allign to camera (billboard)
            // transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            // however we want the rocket pointing its up vector towards the target position
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * dir);

            // if reached, apply damage and destroy rocket
            if (Vector3.Distance(transform.position, Defender.transform.position) < transform.lossyScale.y)
            {
                //var hit = Prefabs.Instance.GetNewSmallExplosion();
                //hit.transform.position = gameObject.transform.position;
                //hit.transform.parent = Defender.transform;
                Destroy(gameObject);
                Defender.GetComponent<Defence>().Defend(AttackDamage);
            }
        }
    }
}