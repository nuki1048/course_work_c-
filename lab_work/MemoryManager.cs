namespace MVCFrame
{
    public class MemoryManager
    {       
        public void Save(Memory ramMemory)
        {
            this.memory = ramMemory;
        }
        public Memory Allocate(long size)
        {
            if (size > memory.FreeSize) return null;
            memory.OccupiedSize += size;
            return memory;
        }
        public Memory Free(long size)
        {
            memory.OccupiedSize -= size;
            return memory;
        }
        private Memory memory;
    }
}
