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

        private static long packageId = 0;

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
        public static JObject CreateSendUnitsMessage(int target, int[] ships)
        {
            return new JObject{
                {"message",     "move-units"},
                {"ships",       new JArray(ships)},
                {"target",      target},
                {"packageId",   ++PackageFactory.packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateSpawnMessage(Spawn[] spawns)
        {
            return new JObject{
                {"message",     "spawn-units"},
                {"spawns",      new JObject(spawns)},
                {"packageId",   ++PackageFactory.packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateJoinQueueMessage(string uid)
        {
            return new JObject{
                {"message",     uid},
                {"uid",         uid},
                {"packageId",   ++PackageFactory.packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateGameTypeGameMessage(string name, string gameType)
        {
            return new JObject{
                {"message",     name},
                {"game-type",   gameType},
                {"packageId",   ++PackageFactory.packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }

        public static JObject CreateRequestGameData()
        {
            return new JObject{
                {"message",     "game-data"},
                {"packageId",   ++PackageFactory.packageId},
                {"scheduleId",  Game.ScheduleId()}
            };
        }
    }
}
