using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Newtonsoft.Json.Linq;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    class PackageFactory
    {
        // static 
        private PackageFactory()
        {
        }

        private static long _packageId;

        public static JObject CreateHelloWorldMessage()
        {
            return new JObject
            {
                {"message",     "hallo welt"},
                {"username",    "android"},
                {"name",        "message"}
            };
        }

        /// <summary>Sending Units from all sources to target destination.</summary>
        /// 
        /// <param name="ships">All send ships.</param>
        /// <param name="target">Target destination.</param>
        /// <param name="packetId">package id - must be executed in correct order</param>
        /// <param name="scheduleId">Target destination.</param>
        /// 
        /// <returns>Generated JsonObject.</returns>
        public static JObject CreateMoveUnitMessage(int target, int[] ships)
        {
            return new JObject{
                {"message",     "move-unit"},
                {"ships",       new JArray(ships)},
                {"target",      target},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateSpawnMessage(SpawnData[] spawns)
        {
            return new JObject{
                {"message",     "spawn-unit"},
                {"spawns",      new JObject(spawns)},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateJoinQueueMessage(string uid)
        {
            return new JObject{
                {"message",     uid},
                {"uid",         uid},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateGameTypeGameMessage(string name, string gameType)
        {
            return new JObject{
                {"message",     name},
                {"game-type",   gameType},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateRequestGameData()
        {
            return new JObject{
                {"message",     "game-data"},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateMessage(string name, string[] keyValuePairs)
        {
            var json = new JObject{
                {"message",     name},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };

            for (var i = 0; i < keyValuePairs.Length; i += 2)
                json.Add(keyValuePairs[i], keyValuePairs[i + 1]);

            return json;
        }
    }
}
