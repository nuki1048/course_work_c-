using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVCFrame
{
    public class Memory: INotifyPropertyChanged
    {
        public long Size
        {
            get;
            private set;
        }
        public long FreeSize 
        { 
            get => Size - occupiedSize;
            private set{}
        }
        private long occupiedSize;
        public long OccupiedSize 
        { 
            get => occupiedSize;
            set 
            {
                occupiedSize = value;
                FreeSize = Size - occupiedSize;
                OnPropertyChanged(); 
            } 
        }     
        public void Save(long size)
        {
            Size = size;
            OccupiedSize = 0;
        }
        public void Clear()
        {
            FreeSize = 0;
            OccupiedSize = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
