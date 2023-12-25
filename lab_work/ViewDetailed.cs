using System.ComponentModel;
using System.Windows.Forms;

namespace MVCFrame
{
    class ViewDetailed : View
    {
        public ViewDetailed(Model model, Controller controller, FrmDetailed frm) : base(model, controller)
        {
            this.frm = frm;
        }
        public override void DataBind()
        {
            frm.LblTime.DataBindings.Add(new Binding("Text", Model.Clock, "Clock"));
            frm.TbCPU.DataBindings.Add(new Binding("Text", Model.Cpu, "ActiveProcess"));
            frm.TbDevice.DataBindings.Add(new Binding("Text", Model.Device, "ActiveProcess"));
            frm.TbDevice_2.DataBindings.Add(new Binding("Text", Model.Device_2, "ActiveProcess"));
            frm.LblOccupateMemSize.DataBindings.Add(new Binding("Text", Model.Ram, "OccupiedSize"));
            frm.LblFreeMemSize.DataBindings.Add(new Binding("Text", Model.Ram, "FreeSize"));

            var intestineBinding = new Binding("Value", Model.ModelSettings, "Intensity")
                {
                    ControlUpdateMode = ControlUpdateMode.Never
                };
            frm.NudIntesity.DataBindings.Add(intestineBinding);

            var minValueOfBurstTimeBinding = new Binding("Value", Model.ModelSettings, "MinValueOfBurstTime")
                {
                    ControlUpdateMode = ControlUpdateMode.Never
                };
            frm.NudMinValueOfBurstTime.DataBindings.Add(minValueOfBurstTimeBinding);

            var maxValueOfBurstTimeBinding = new Binding("Value", Model.ModelSettings, "MaxValueOfBurstTime")
                {
                    ControlUpdateMode = ControlUpdateMode.Never
                };
            frm.NudMaxValueOfBurstTime.DataBindings.Add(maxValueOfBurstTimeBinding);

            var minValueOfAddrSpaceBinding = new Binding("Value", Model.ModelSettings, "MinValueOfAddrSpace")
                {
                    ControlUpdateMode = ControlUpdateMode.Never
                };
            frm.NudMinValueOfAddrSpace.DataBindings.Add(minValueOfAddrSpaceBinding);

            var maxValueOfAddrSpaceBinding = new Binding("Value", Model.ModelSettings, "MaxValueOfAddrSpace")
                {
                    ControlUpdateMode = ControlUpdateMode.Never
                };
            frm.NudMaxValueOfAddrSpace.DataBindings.Add(maxValueOfAddrSpaceBinding);

            var valueOfRamSizeBindings = new Binding("SelectedItem", Model.ModelSettings, "ValueOfRAMSize", true)
                {
                    ControlUpdateMode = ControlUpdateMode.Never
                };
            frm.CbRamSize.DataBindings.Add(valueOfRamSizeBindings);

            var cpuThroughputBinding = new Binding("Text", Model.statistics, "Throughput", true, DataSourceUpdateMode.Never, null, "#0.#%");
            frm.LblThroughput.DataBindings.Add(cpuThroughputBinding);

            var cpuUtilizationBinding = new Binding("Text", Model.statistics, "CpuUtilization", true, DataSourceUpdateMode.Never, null, "#0.#%");
            frm.LblUtilization.DataBindings.Add(cpuUtilizationBinding);

            Subscribe();
        }

        public override void DataUnbind()
        {
            frm.NudIntesity.DataBindings.RemoveAt(0);
            frm.NudMinValueOfBurstTime.DataBindings.RemoveAt(0);
            frm.NudMaxValueOfBurstTime.DataBindings.RemoveAt(0);
            frm.NudMinValueOfAddrSpace.DataBindings.RemoveAt(0);
            frm.NudMaxValueOfAddrSpace.DataBindings.RemoveAt(0);
            frm.CbRamSize.DataBindings.RemoveAt(0);

            Unsubscribe();
        }
        private void Subscribe()
        {
            Model.PropertyChanged += PropertyChangedHandler;
        }

        private void Unsubscribe()
        {
            Model.PropertyChanged -= PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ReadyQueue")
            {
                var temp = Model.ReadyQueue;
                UpdateListBox(temp.ToArray(), frm.LblSortedCPUQueue);
            }             
            else
            {
                UpdateListBox(Model.DeviceQueue.ToArray(), frm.LblDeviceQueue);
                UpdateListBox(Model.DeviceQueue_2.ToArray(), frm.LblDeviceQueue_2);
            }                       
        }
        private void UpdateListBox(object[] processes, ListBox lb)
        {
            lb.Items.Clear();
            if(processes.Length != 0)
                lb.Items.AddRange(processes);
        }
        private FrmDetailed frm;
    }
}
