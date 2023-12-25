namespace MVCFrame
{
    public class IdGenerator
    {
        private long id;
        public long Id => id == long.MaxValue ? 0 : ++id;

        public IdGenerator Clear()
        {
            id = 0;
            return this;
        }
    }
}
