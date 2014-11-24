using System.Collections.Generic;

namespace Assets.Scripts.network.googleplayservice
{
    public class ConcurrentQueue<T>
    {
        private readonly object _syncLock = new object();
        private Queue<T> queue;

        public int Count
        {
            get
            {
                lock (_syncLock)
                {
                    return queue.Count;
                }
            }
        }

        public ConcurrentQueue()
        {
            this.queue = new Queue<T>();
        }

        public T Peek()
        {
            lock (_syncLock)
            {
                return queue.Peek();
            }
        }

        public void Enqueue(T obj)
        {
            lock (_syncLock)
            {
                queue.Enqueue(obj);
            }
        }

        public T Dequeue()
        {
            lock (_syncLock)
            {
                return queue.Dequeue();
            }
        }

        public void Clear()
        {
            lock (_syncLock)
            {
                queue.Clear();
            }
        }

        public T[] CopyToArray()
        {
            lock (_syncLock)
            {
                if (queue.Count == 0)
                {
                    return new T[0];
                }

                T[] values = new T[queue.Count];
                queue.CopyTo(values, 0);
                return values;
            }
        }

        public static ConcurrentQueue<T> InitFromArray(IEnumerable<T> initValues)
        {
            var queue = new ConcurrentQueue<T>();

            if (initValues == null)
            {
                return queue;
            }

            foreach (T val in initValues)
            {
                queue.Enqueue(val);
            }

            return queue;
        }
    }
}
