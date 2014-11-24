using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.network.googleplayservice
{
    public class ConcurrentArrayList
    {
        private readonly object _syncLock = new object();
        private readonly LinkedList<JObject> _list;

        public ConcurrentArrayList()
        {
            _list = new LinkedList<JObject>();
        }

        public void Add(JObject item)
        {
            lock (_syncLock)
            {
                _list.AddLast(item);
            }
        }

        public void Remove(JObject item)
        {
            lock (_syncLock)
            {
                _list.Remove(item);
            }
        }

        public JObject Acknowledge(JObject ackJson)
        {
            lock (_syncLock)
            {
                var receiverPackageId = ackJson["packageId"].ToObject<int>();
                var result = _list.FirstOrDefault(json => json["packageId"].ToObject<int>() == receiverPackageId);
                Remove(result);
                return result;
            }
        }

        public int Count()
        {
            lock (_syncLock)
            {
                return _list.Count();
            }
        }
    }
}