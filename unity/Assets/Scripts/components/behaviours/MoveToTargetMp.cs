using Assets.Scripts.network;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.network;

namespace Assets.Sources.components.behaviours
{
    public class MoveToTargetMp : MoveToTarget
    {
        public override void Arrive()
        {
            // 1) send event
            var json = PackageFactory.CreateArrivalMessage(GetComponent<ShipData>().uid);
            SocketHandler.EmitNow("unit-arrival", json);

            // 2) schedule event
            ((GameMp) Game.Shared).ScheduleAt("unit-arrival", json["scheduleId"].ToObject<long>(), json["packageId"].ToObject<int>(), () =>
            {
                var uid = json["uid"].ToObject<int>();
                Registry.Ships[uid].GetComponent<PlayMakerFSM>().SendEvent("Arrive");
            });
        }
    }
}
