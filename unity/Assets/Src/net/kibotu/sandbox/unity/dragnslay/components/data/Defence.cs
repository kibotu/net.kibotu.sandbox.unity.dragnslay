using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data
{
    class Defence : MonoBehaviour
    {
        private LifeData _lifeData;

        public void Start()
        {
            _lifeData = GetComponent<LifeData>();
        }

        public void Defend(int damage)
        {
            _lifeData.CurrentHp -= damage;
            if (_lifeData.CurrentHp <= 0)
                Destroy(gameObject);
        }
    }
}
