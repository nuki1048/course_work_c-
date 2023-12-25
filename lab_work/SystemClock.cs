using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVCFrame
{
    internal class SystemClock: INotifyPropertyChanged
    {
        private long clock;
        public long Clock 
        { 
            get => clock;
            private set
            {
                clock = value;
                OnPropertyChanged();
            }
        }

        public void WorkingCycle()
        {
            Clock++;
        }

        public void Clear()
        {
            Clock = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
