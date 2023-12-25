using System.Collections.Generic;
using System.Diagnostics;

namespace MVCFrame
{
    internal class QueueItemComparer<T> : IComparer<QueueItem<T>>
    {
        public int Compare(QueueItem<T> x, QueueItem<T> y)
        {
            Debug.Assert(y != null, nameof(y) + " != null");
            Debug.Assert(x != null, nameof(x) + " != null");
            return y.Priority.CompareTo(x.Priority);
        }
    }
}