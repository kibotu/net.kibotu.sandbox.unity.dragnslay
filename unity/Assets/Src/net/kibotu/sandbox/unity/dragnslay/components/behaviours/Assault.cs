using System.Collections;
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
        private ShipData _shipData;
        public GameObject Target;

        public void Start()
        {
            AttackSpeed = 3f;
            AttackDamage = 3;
            _startTime = 0;
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
            if (IsOnEnemyIsland() && enemyShips.Count == 0)
            {
                Debug.Log(_shipData.uid + " invades " + transform.parent.gameObject.GetComponent<IslandData>().uid);
                transform.parent.gameObject.GetComponent<IslandData>().playerUid = _shipData.playerUid;
                transform.parent.gameObject.renderer.material.color = GetComponentInChildren<Renderer>().material.color;
            }

            // 2) attack every enemy ship
            if (enemyShips.Count <= 0) return;

            var enemyShip = (GameObject)enemyShips[Random.Range(0, enemyShips.Count)]; // Important! actual range 0 to list size - 1
            Debug.Log(_shipData.uid + " attacks " + enemyShip.GetComponent<ShipData>().uid);

            var rocket = Prefabs.Instance.GetNewRocket();
            var behaviour = rocket.AddComponent<Rocket>();
            behaviour.Attacker = gameObject.transform.position;
            behaviour.AttackDamage = AttackDamage;
            behaviour.Defender = enemyShip;
            behaviour.Distance = 2f;
            behaviour.Speed = 20f;
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
