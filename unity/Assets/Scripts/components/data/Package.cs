using System;

namespace Assets.Sources.components.data
{
    public class Package
    {
        public string Name;
        public int PackageId;
        public Action Action;
//        public JSONObject raw;

        [Obsolete("Not used anymore", false)]
        public bool Verified;
    }
}
