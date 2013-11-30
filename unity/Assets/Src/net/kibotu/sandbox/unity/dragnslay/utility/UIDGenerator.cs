using System;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    public class UidGenerator
    {
        // static
        private UidGenerator()
        {
        }
        
        public static readonly int StartUid = 0;
        public static readonly int InvalidUid = StartUid - 1;
        private static int _nextUid = 0;

        public static int GetNewUid()
        {
            if (!IsValid(_nextUid))
            {
                throw new FormatException("UID pool depleted.");
            }
            return _nextUid++;
        }

        public static bool IsValid ( int uid ) {
            return uid >= StartUid;
        }
    }
}