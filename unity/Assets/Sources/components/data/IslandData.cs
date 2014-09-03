using System.Collections;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class IslandData : MonoBehaviour
    {
		private int _uid = -1;
        public int ShipType;
        public PlayerData PlayerData;
        public int IslandType;
        public float SpawnRate; // spawn per second
        public int MaxSpawn;
        public float CurrentRespawnRate;

		public int Uid
		{
			get { return _uid == -1 ? _uid = UidGenerator.GetNewUid() : _uid; }
			set { _uid = value; } 
		}

        public void Start() {

            // debug singleplayer
//            if (Uid == 0)
//            {
//                Uid = UidGenerator.GetNewUid();
//                Registry.Islands.Add(Uid, gameObject);
//                SpawnRate = 3;
//            }

            PlayerData = Registry.Player[PlayerData.uid].GetComponent<PlayerData>();

            CurrentRespawnRate = SpawnRate;

			Dye ();
        }

        public void Convert(PlayerData playerData)
        {
            PlayerData = playerData;
            Dye();
            var shockwave = Prefabs.Instance.GetNewShockwave();
            shockwave.transform.position = transform.position;
            shockwave.GetComponent<DetonatorShockwave>().color = PlayerData.color;
        }

        public void Dye()
        {
            renderer.material.color = PlayerData.color;
        }

        /// <summary>
        /// Computes current respawn timer based on dominance.
        /// 
        /// 100% if no enemy ships are present.
        /// 0% if at least one enemy ship but no own ships are present.
        /// 
        /// Adds respawn percentage wise respawn time up to 200% of the actual value.
        /// 
        /// </summary>
        /// <returns>Respawn rate in percent. Value between 0f and 1f.</returns>
        public float ShipBuildTime()
        {
            var dominance = DominancePercentage(this);
            CurrentRespawnRate = Mathf.Abs(dominance - 1f) < Mathf.Epsilon
                ? SpawnRate
                : SpawnRate + (SpawnRate*(1 - dominance));
            return CurrentRespawnRate;
        }

        public static float DominancePercentage(IslandData island)
        {
            var friendly = 0;
            var foes = 0;

            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects
                if (otherShipData.PlayerData.uid == island.PlayerData.uid)
                {
                    ++friendly;
                }
                else
                {
                    ++foes;
                }
            }

            var sum = foes + friendly;

            return sum == 0 ? 1f : friendly/(float)sum;
        }

        public static ArrayList GetFriendlyShips(IslandData island, string thisUid)
        {
            var enemyShips = new ArrayList(island.transform.childCount - 1);
            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects
                if (otherShipData.PlayerData.uid == thisUid)
                    enemyShips.Add(ship);
            }
            return enemyShips;
        }

        public static ArrayList GetEnemyShips(IslandData island, string thisUid)
        {
            var enemyShips = new ArrayList(island.transform.childCount - 1);
            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects
                if (otherShipData.PlayerData.uid != thisUid)
                    enemyShips.Add(ship);
            }
            return enemyShips;
        }

        public static int AmountFriendlyUnits(IslandData island)
        {
            var friendly = 0;

            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects
                if (otherShipData.PlayerData.uid == island.PlayerData.uid)
                {
                    ++friendly;
                }
            }

            return friendly;
        }
    }
}
