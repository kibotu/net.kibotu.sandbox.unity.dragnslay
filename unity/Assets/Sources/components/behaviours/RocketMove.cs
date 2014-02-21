using Assets.Sources.components.behaviours.combat;
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
            // transform.position = Vector3.MoveTowards(transform.position, Defender.transform.position, Time.delta * Velocity);

            _startTime += Time.deltaTime;

            // http://whydoidoit.com/2012/04/06/unity-curved-path-following-with-easing/
            transform.position = new Vector3
            {
                x = Mathf.Lerp(Attacker.x, Defender.transform.position.x, Easing.Quintic.easeIn(_startTime * Velocity)),
                y = Mathf.Lerp(Attacker.y, Defender.transform.position.y, Easing.Quintic.easeIn(_startTime * Velocity)),
                z = Mathf.Lerp(Attacker.z, Defender.transform.position.z, Easing.Quintic.easeIn(_startTime * Velocity))
            }; 

            // rotate along forward axe of camera towards target
            var dir = transform.position.Direction(Defender.transform.position);
            //Debug.DrawRay(transform.position, dir,Color.magenta); // target direction
            //Debug.DrawRay(transform.position, transform.up, Color.black); // up vector
            //Debug.DrawRay(transform.position, -transform.forward, Color.cyan); // back vector
            //Debug.DrawRay(transform.position, transform.position + Camera.main.transform.rotation * Vector3.forward, Color.blue); // direction towards camera
            //Debug.DrawRay(transform.position, Camera.main.transform.rotation * Vector3.up, Color.red); // camera up direction

            // allign to camera (billboard)
            // transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            // however we want the rocket pointing its up vector towards the target position
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * dir);

            // another ship has destroyed the target; Note: potential re-targeting
           
            if (Defender == null) 
            { 
                Destroy(gameObject);
                return;
            }

            // if reached, apply damage and destroy rocket
            if (Vector3.Distance(transform.position, Defender.transform.position) < transform.lossyScale.y)
            {
                Defender.GetComponent<Defence>().Defend(AttackDamage);
                Destroy(gameObject);
            }
        }
    }
}