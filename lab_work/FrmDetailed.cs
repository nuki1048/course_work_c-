using System;
using System.Windows.Forms;

namespace MVCFrame
{
    public partial class FrmDetailed : Form
    {
        private ViewDetailed viewDetailed;
        public FrmDetailed()
        {
            InitializeComponent();
            cbRamSize.SelectedItem = cbRamSize.Items[0];
            viewDetailed = new ViewDetailed(new Model(), new Controller(), this);
            viewDetailed.DataBind();
        }

        public Label LblTime
        {
            get
            {
                return lblTime;
            }
        }
        public TextBox TbCPU
        {
            get
            {
                return tbCPU;
            }
        }
        public TextBox TbDevice
        {
            get
            {
                return tbDevice;
            }
        }

        public TextBox TbDevice_2
        {
            get
            {
                return tbDevice_2;
            }
        }

        public ListBox LblSortedCPUQueue
        {
            get
            {
                return lblSortedCPUQueue;
            }
        }
        public ListBox LblDeviceQueue
        {
            get
            {
                return lblDeviceQueue;
            }
        }
        public ListBox LblDeviceQueue_2
        {
            get
            {
                return lblDeviceQueue_2;
            }
        }
        public Label LblOccupateMemSize
        {
            get
            {
                return lblOccupateMemValue;
            }
        }
        public Label LblFreeMemSize
        {
            get
            {
                return lblFreeMemSize;
            }
        }
        public NumericUpDown NudIntesity
        {
            get
            {
                return nudIntesity;
            }
        }
        public NumericUpDown NudMinValueOfBurstTime
        {
            get
            {
                return nudMinValueOfBurstTime;
            }
        }
        public NumericUpDown NudMaxValueOfBurstTime
        {
            get
            {
                return nudMaxValueOfBurstTime;
            }
        }
        public NumericUpDown NudMinValueOfAddrSpace
        {
            get
            {
                return nudMinValueOfAddrSpace;
            }
        }
        public NumericUpDown NudMaxValueOfAddrSpace
        {
            get
            {
                return nudMaxValueOfAddrSpace;
            }
        }
        public ComboBox CbRamSize
        {
            get
            {
                return cbRamSize;
            }
        }

        public Label LblThroughput
        {
            get
            {
                return lblThroughput;
            }
        }

        public Label LblUtilization
        {
            get
            {
                return lblUtilization;
            }
        }


        private void bWorkingCycle_Click(object sender, EventArgs e)
        {
            viewDetailed.ReactToUserActions(ModelOperations.WorkingCycle);
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            if (rbAuto.Checked)
                timerAutoMode.Enabled = false;
                
            viewDetailed.ReactToUserActions(ModelOperations.Clear);
            endOfSession();
            UpdateSettings();
        }
        private void sessionPreparation()
        {
            if (rbAuto.Checked)
            {
                bPause.Enabled = true;
                rbManual.Enabled = false;
            }
            else
            {
                rbAuto.Enabled = false;
            }
            bSaveSettings.Enabled = false;
            bClear.Enabled = true;
            bWorkingCycle.Enabled = rbManual.Checked;
            nudIntesity.Enabled = false;
            nudMaxValueOfAddrSpace.Enabled = false;
            nudMaxValueOfBurstTime.Enabled = false;
            nudMinValueOfAddrSpace.Enabled = false;
            nudMinValueOfBurstTime.Enabled = false;
            cbRamSize.Enabled = false;

        }
        private void UpdateSettings()
        {
            nudIntesity.Value = (decimal)0.5;
            nudMinValueOfBurstTime.Value = 1;
            nudMaxValueOfBurstTime.Value = 4;
            nudMinValueOfAddrSpace.Value = 25;
            nudMaxValueOfAddrSpace.Value = 125;
        }
        private void endOfSession()
        {
            rbAuto.Enabled = true;
            rbManual.Enabled = true;
            bClear.Enabled = false;
            bSaveSettings.Enabled = true;
            bWorkingCycle.Enabled = false;
            nudIntesity.Enabled = true;
            nudMaxValueOfAddrSpace.Enabled = true;
            nudMaxValueOfBurstTime.Enabled = true;
            nudMinValueOfAddrSpace.Enabled = true;
            nudMinValueOfBurstTime.Enabled = true;
            cbRamSize.Enabled = true;
            bPause.Enabled = false;
        }

        private void bSaveSettings_Click(object sender, EventArgs e)
        {
            sessionPreparation();
            if (rbManual.Checked)
                viewDetailed.ReactToUserActions(ModelOperations.SaveSettings);
            else
            {
                viewDetailed.ReactToUserActions(ModelOperations.SaveSettings);                
                timerAutoMode = new Timer();
                timerAutoMode.Enabled = true;
                timerAutoMode.Interval = 1000;
                timerAutoMode.Tick += bWorkingCycle_Click;
            }
        }

        private void bPause_Click(object sender, EventArgs e)
        {
            timerAutoMode.Enabled = !timerAutoMode.Enabled;
        }

        private void Не_Enter(object sender, EventArgs e)
        {
        }
    }
}
