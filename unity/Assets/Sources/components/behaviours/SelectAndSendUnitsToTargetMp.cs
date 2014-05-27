using System.Collections.Generic;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.network;
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

            SocketHandler.EmitNow("move-unit", PackageFactory.CreateMoveUnitMessage(target.GetComponent<IslandData>().Uid, toMovePlanes.ToArray()));
        }
    }
}
