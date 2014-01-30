using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    class Rocket : MonoBehaviour
    {
        public Vector3 Attacker;
        public int AttackDamage;
        public GameObject Defender;
        public float Distance;
        public float Speed;

        public void Start()
        {
            transform.position = Attacker;
            transform.localScale = new Vector3(3,3,3);
        }

        public void Update()
        {
            if (Defender == null) Destroy(gameObject);
            if (!(Vector3.Distance(transform.position, Defender.transform.position) > Distance))
            {
                Debug.Log("hit");
                Defender.GetComponent<Defence>().Defend(AttackDamage);
                Destroy(gameObject);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, Defender.transform.position, Time.deltaTime * Speed);
        }
    }
}
