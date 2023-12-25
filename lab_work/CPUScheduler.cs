namespace MVCFrame
{
    class CpuScheduler
    {
        private readonly Resource resource;
        private readonly HpfQueue<Process> hpfQueue;

        public CpuScheduler(Resource resource, HpfQueue<Process> hpfQueue)
        {
            this.resource = resource;
            this.hpfQueue = hpfQueue;
        }

        public HpfQueue<Process> Session()
        {
            var tmpProc = hpfQueue.Item();
            tmpProc.Status = ProcessStatus.Running;
            hpfQueue.Remove();
            resource.ActiveProcess = tmpProc;
            return hpfQueue;
        }
    }
}
