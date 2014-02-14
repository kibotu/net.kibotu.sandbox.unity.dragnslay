using System.Collections.Generic;

namespace Assets.Sources.utility
{
   public static class UidPool
    {
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
