using System.Collections;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.behaviours.combat
{
    public class Assault : MonoBehaviour
    {
        private float _startTime;
        public ShipData AssaultShipData;
        public GameObject Target;

        public void Start()
        {
            // _startTime = ShipData.AttackSpeed; // shoot without waiting at arrival
            _startTime = 0;
            AssaultShipData = GetComponent<ShipData>();
        }

        public void FixedUpdate()
        {
            if (!Game.IsRunning()) return;

            _startTime += Time.deltaTime;
            if (_startTime < AssaultShipData.AttackSpeed) return;
            _startTime -= AssaultShipData.AttackSpeed;

            // current island
            var islandData = transform.parent.gameObject.GetComponent<IslandData>();

            // current enemy ships
            var enemyShips = IslandData.GetEnemyShips(islandData, AssaultShipData.PlayerData.uid);

            // 1) if is enemy island and has no there are no enemy ships => convert
            if (IsOnEnemyIsland() && enemyShips.Count == 0)
            {
//                Debug.Log(ShipData.uid + " invades " + transform.parent.gameObject.GetComponent<IslandData>().uid);
                islandData.Convert(AssaultShipData.PlayerData);
            }

            // 2) attack every enemy ship
            if (enemyShips.Count <= 0) return;

            var enemyShip = (GameObject)enemyShips[Random.Range(0, enemyShips.Count)]; // Important! actual range 0 to list size - 1
//            Debug.Log(ShipData.playerUid +"[" + ShipData.uid + "] attacks " + enemyShip.GetComponent<ShipData>().playerUid + "[" + enemyShip.GetComponent<ShipData>().uid + "]");

            var rocket = Prefabs.Instance.GetNewRocket();
            var behaviour = rocket.GetComponent<RocketMove>();
            behaviour.Attacker = gameObject.transform.position;
            behaviour.AttackDamage = AssaultShipData.AttackDamage;
            behaviour.Defender = enemyShip;
            behaviour.Velocity = 0.6f;
        }

        private bool IsOnEnemyIsland()
        {
            //Debug.Log(transform.parent.gameObject.GetComponent<IslandData>().PlayerData.uid + " " + ShipData.PlayerData.uid);
            return transform.parent.gameObject.GetComponent<IslandData>().PlayerData.uid != AssaultShipData.PlayerData.uid;
        }

        public void AttackTarget(int targetUid)
        {
            _startTime = 0;
            Target = Registry.Ships[targetUid];
        }
    }
}
