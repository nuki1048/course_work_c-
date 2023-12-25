namespace MVCFrame
{
    class DeviceScheduler
    {
        private readonly Resource resource;
        private readonly HpfQueue<Process> queue;

        public DeviceScheduler(Resource resource, HpfQueue<Process> queue)
        {
            this.resource = resource;
            this.queue = queue;
        }

        public HpfQueue<Process> Session()
        {
            var tmpProc = queue.Item();
            queue.Remove();
            resource.ActiveProcess = tmpProc;
            return queue;
        }
    }
}
