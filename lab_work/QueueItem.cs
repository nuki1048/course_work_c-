namespace MVCFrame
{
    internal class QueueItem<T>
    {
        public T Item { get; }
        public int Priority { get; }

        public QueueItem(T item, int priority)
        {
            Item = item;
            Priority = priority;
        }
    }
}