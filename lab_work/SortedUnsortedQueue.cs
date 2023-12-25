using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queues;
using Structures;
namespace MVCFrame
{
  {
        private QueueItem<T>[] queue;
        public int Count { get; private set; }

        public HPFQueue() 
        {
            queue = new QueueItem<T>[10]; // Initialize with a size of 10
            Count = 0;
        }

        public IQueueable<T> Put(T value, int priority)
        {
            if (Count == queue.Length)
            {
                throw new InvalidOperationException("Queue is full");
            }
            queue[Count++] = new QueueItem<T>(value, priority);
            Sort();
            return this;
        }

        public IQueueable<T> Remove()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            Array.Copy(queue, 1, queue, 0, --Count);
            return this;
        }

        public T Item()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            return queue[0].Item;
        }

        public IQueueable<T> Clear()
        {
            queue = new QueueItem<T>[10];
            Count = 0;
            return this;
        }

        public T[] ToArray()
        {
            return queue.Take(Count).Select(x => x.Item).ToArray();
        }

        private void Sort()
        {
            Array.Sort(queue, 0, Count, new QueueItemComparer<T>());
        }
    }
}
