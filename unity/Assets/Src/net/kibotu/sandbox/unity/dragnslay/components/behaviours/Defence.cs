using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data
{
    class Defence : MonoBehaviour
    {
        private LifeData _lifeData;
        private float _startTime;
        private bool isBlinking;
        private float blinkTime;
        private Color oldColor;

        private bool isExploding = false;

        public void Start()
        {
            isBlinking = false;
            blinkTime = 0.15f;
            _lifeData = GetComponent<LifeData>();
        }

        public void Defend(int damage)
        {
            _lifeData.CurrentHp -= damage;
            BlinkOnce();
            if (_lifeData.CurrentHp > 0) return;

            if (isExploding) return;

            Destroy(gameObject);
            Prefabs.Instance.GetNewExplosion().transform.position = transform.position;
            
            Registry.Instance.Ships.Remove(GetComponent<ShipData>().uid);

            Debug.Log(GetComponent<ShipData>().uid + " has been destroyed!");
            isExploding = true;
        }

        public void Update()
        {
            /*if (isBlinking)
            {
                _startTime += Time.deltaTime;
                if (_startTime < blinkTime) return;

                oldColor = GetComponentInChildren<Renderer>().material.color;

                GetComponentInChildren<Renderer>().material.color = new Color(oldColor.r + 0.5f, oldColor.g + 0.5f, oldColor.b + 0.5f, oldColor.a + 0.50f);
                
                isBlinking = false;
            }
            else
            {
                GetComponentInChildren<Renderer>().material.color = oldColor;
            }*/
        }

        private void BlinkOnce()
        {
            isBlinking = true;
            _startTime = 0;
        }
    }
}
