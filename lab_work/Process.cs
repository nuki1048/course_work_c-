using System;

namespace MVCFrame
{
    public enum ProcessStatus
    {
        Ready,
        Running,
        Waiting,
        Terminated
    }
    public class Process: IComparable<Process>
    {
        public Process(long pid, long addrSpace)
        {
            id = pid;
            AddrSpace = addrSpace;           
            name = "P" + pid.ToString();

            Status = ProcessStatus.Ready;
        }

        public void IncreaseWorkTime()
        {
            workTime++;
            if (workTime != BurstTime) return;
            if (Status == ProcessStatus.Running)
            {
                Status = random.Next(0, 2) == 0 ? ProcessStatus.Terminated : ProcessStatus.Waiting;
                if (Status == ProcessStatus.Waiting)
                {                    
                    smth.DeviceNumber = (int)random.Next(1, 3);
                    OnFreeingAResource(smth);
                    return;
                }
                  
            }
            else
            {
                Status = ProcessStatus.Ready;
            }
            OnFreeingAResource(smth);
        }
        public void ResetWorkTime()
        {
            workTime = 0;
        }

        public override string ToString()
        {
            var result = "Id: " + id.ToString() + " Name: " + name + " Status: " + Status + " BurstTime: " + BurstTime + " AddrSpace: " + AddrSpace;
            return result;
        }

        public int CompareTo(Process other)
        {
            return other == null ? 1 : other.BurstTime.CompareTo(this.BurstTime);
        }
        public void OnFreeingAResource(EventArgs e = null)
        {
            if (FreeingAResource != null)
            {
                FreeingAResource(this, e);
            }
        }

        private readonly long id;
        private readonly string name;
        private long workTime;
        public long BurstTime { get; set; }
        public ProcessStatus Status { get; set; }    
        public long ReadyQueueArrivalTime { get; set; }
        public long AddrSpace { get; private set; }
        public long ArrivalTime { get; set; }
        public long CommonWaitingTime { get; set; }
        readonly NewEventArgs smth = new NewEventArgs();
        public event EventHandler FreeingAResource;
        private readonly Random random = new Random();
    }
}
