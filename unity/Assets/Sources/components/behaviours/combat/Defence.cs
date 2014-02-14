using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.behaviours.combat
{
    public class Defence : MonoBehaviour
    {
        private LifeData _lifeData;
        private float _startTime;
        private bool _isExploding = false;

        public void Start()
        {
            _lifeData = GetComponent<LifeData>();
        }

        public void Defend(int damage)
        {
            _lifeData.CurrentHp -= damage;
            if (_lifeData.CurrentHp > 0 || _isExploding) return;

            Destroy(gameObject);
            Prefabs.Instance.GetNewExplosion().transform.position = transform.position;
            
            Registry.Instance.Ships.Remove(GetComponent<ShipData>().uid);

            Debug.Log(GetComponent<ShipData>().uid + " has been destroyed!");
            _isExploding = true;
        }
    }
}
