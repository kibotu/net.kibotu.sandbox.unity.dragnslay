using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    class UidPool
    {
        // static
        private UidPool()
        {
        }

        private static readonly Queue<int> Uids = new Queue<int>();

        public static int GetNewUid()
        {
            return Uids.Dequeue();
        }

        public static void AddNewUids(int[] newUids)
        {
            foreach(var i in newUids)
                Uids.Enqueue(i);
        }
    }
}
