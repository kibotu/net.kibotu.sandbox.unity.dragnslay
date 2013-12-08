using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using SimpleJson;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    class PackageFactory
    {
        // static 
        private PackageFactory()
        {
        }

        private static long _packageId;

        public static JsonObject CreateHelloWorldMessage()
        {
            return new JsonObject
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
        public static JsonObject CreateMoveUnitMessage(int target, int[] ships)
        {
            return new JsonObject{
                {"message",     "move-unit"},
                {"ships",       ships},
                {"target",      target},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JsonObject CreateSpawnMessage(SpawnData[] spawns)
        {
            return new JsonObject{
                {"message",     "spawn-unit"},
                {"spawns",      spawns},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JsonObject CreateJoinQueueMessage(string uid)
        {
            return new JsonObject{
                {"message",     uid},
                {"uid",         uid},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JsonObject CreateGameTypeGameMessage(string name, string gameType)
        {
            return new JsonObject{
                {"message",     name},
                {"game-type",   gameType},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JsonObject CreateRequestGameData()
        {
            return new JsonObject{
                {"message",     "game-data"},
                {"packageId",   ++_packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JsonObject CreateMessage(string name, string [] keyValuePairs)
        {
            var json = new JsonObject{
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
