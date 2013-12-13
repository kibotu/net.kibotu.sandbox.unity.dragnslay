using System.Collections;
using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    class Assault : MonoBehaviour
    {
        private float _startTime;
        public float AttackSpeed;
        public int AttackDamage;
        private LifeData _lifeData;
        private ShipData _shipData;
        public GameObject Target;

        public void Start()
        {
            AttackSpeed = 1000;
            AttackDamage = 3;
            _startTime = 0;
            _lifeData = GetComponent<LifeData>();
            _shipData = GetComponent<ShipData>();
        }

        public void Update()
        {
            if (!Game.IsRunning()) return;

            _startTime += Time.deltaTime;
            if (_startTime < AttackSpeed) return;
            _startTime -= AttackSpeed;

            var enemyShips = GetEnemyShips();

            // 1) if is enemy island and has no there are no enemy ships => convert
            if (IsOnEnemyIsland())
            {
                Debug.Log("converting");
            }

            // 2) attack every enemy ship
            if (enemyShips.Count > 0)
            {
                var enemyShip = (GameObject)enemyShips[Random.Range(0, enemyShips.Count)]; // Important! actual range 0 to list size - 1
                
                //enemyShip.GetComponent<Defence>().Defence(AttackDamage);
            }
        }

        private bool IsOnEnemyIsland()
        {
            return transform.parent.gameObject.GetComponent<IslandData>().playerUid != _shipData.playerUid;
        }

        public void AttackTarget(int targetUid)
        {
            _startTime = 0;
            Target = Registry.Instance.Ships[targetUid];
        }

        public ArrayList GetEnemyShips()
        {
            var enemyShips = new ArrayList(transform.parent.childCount-1); 
            for (var i = 0; i < transform.parent.childCount; ++i)
            {
                var ship = transform.parent.GetChild(i).gameObject;
                if(ship.name.Equals("Sphere")) continue;
                if(ship.GetComponent<ShipData>().playerUid != _shipData.playerUid)
                    enemyShips.Add(ship);
            }
            return enemyShips;
        }
    }
}
