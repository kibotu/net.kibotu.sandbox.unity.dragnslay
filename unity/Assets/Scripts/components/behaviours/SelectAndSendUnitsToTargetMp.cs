using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.network;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SelectAndSendUnitsToTargetMp : SelectAndSendUnitsToTarget
    {
        protected override void Send(GameObject target)
        {
            var toMovePlanes = new List<int>();

            for (var i = 0; i < Island.transform.childCount; ++i)
            {
                var ship = Island.transform.GetChild(i).gameObject;
                var shipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (shipData == null) continue; // skip non ship gameobjects
                if (shipData.PlayerData.uid == Game.Shared.ClientUid)
                    toMovePlanes.Add(shipData.uid);
            };

            Deselect();

            Debug.Log("request move-units: " + PackageFactory.CreateMoveUnitMessage(target.GetComponent<IslandData>().Uid, toMovePlanes.ToArray()));

            var json = PackageFactory.CreateMoveUnitMessage(target.GetComponent<IslandData>().Uid,toMovePlanes.ToArray());
            SocketHandler.EmitNow("move-unit", json);

            Move(json);
        }

        public void Move(JObject json)
        {
            var target = Registry.Islands[json["target"].ToObject<int>()];

            foreach (var shipUid in json["ships"].Select(shipId => shipId.ToObject<int>()))
            {
                var uid = shipUid;
                ((GameMp) Game.Shared).ScheduleAt("move-unit", json["scheduleId"].ToObject<long>(), json["packageId"].ToObject<int>(), () =>
                {
                    // 1) add move component to ship
                    var move = Registry.Ships[uid].AddComponent<MoveToTarget>();

                    // 2) change speed
                    move.Velocity = 150f;

                    // 3) set move destination
                    move.Target = target;

                    Debug.Log("move " + uid + " to " + target.GetComponent<IslandData>().Uid);
                });
            }

        }
    }
}
