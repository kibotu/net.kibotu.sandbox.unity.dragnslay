using Assets.Sources.game;
using Assets.Sources.network;
using Newtonsoft.Json.Linq;

namespace Assets.Sources.components.behaviours
{
    public class SpawnUnitsMp : SpawnUnits
    {
        private PlayMakerFSM _fsm;

        public override void Start()
        {
            base.Start();
            _fsm = GetComponent<PlayMakerFSM>();
        }

        public override void Spawn()
        {
            // check if spawning event already triggered
            if (_fsm.ActiveStateName.Equals("Spawning Ship"))
                return;

            // 1) block spawnings until network event did actually spawn the ship
            _fsm.SendEvent("Spawn");

            // 2) broadcast spawn event
            SocketHandler.EmitNow("spawn-unit", PackageFactory.CreateSpawnMessage(
                    new[] { new JObject
                {
                    {"island_uid", Island.Uid},
                    {"uid" , -1}
                            }}));
        }

        public override void ResetSpawnTimer()
        {
            if (!GameMp.IsHost())
                return;

            base.ResetSpawnTimer();
            _fsm.SendEvent("Spawned");
        }
    }
}