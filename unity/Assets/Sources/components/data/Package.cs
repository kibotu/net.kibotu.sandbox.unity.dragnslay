using System;
using SimpleJson;

namespace Assets.Sources.components.data
{
    public class Package
    {
        public string Name;
        public int PackageId;
        public Action Action;
        public JsonObject raw;

        [Obsolete("Not used anymore", false)]
        public bool Verified;
    }
}
