using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVCFrame
{
    class Model: INotifyPropertyChanged
    {
        // операции модели
        public Model() // создание модели
        {
            // создание компонентов вычислительной системы:

            // 1.   часы - счётчик рабочих тактов;
            Clock = new SystemClock();
            // 2.   процессор;
            Cpu = new Resource();
            // 3.   оперативная память;
            Ram = new Memory();
            // 4.   внешнее устройство;
            Device = new Resource();
            Device_2 = new Resource();
            // 5.   генератор идентификаторов процессов;
            idGen = new IdGenerator();
            // 6.   очереди готовых процессов (к процессору);
            readyQueue = new HpfQueue<Process>();
            // 7.   очереди к внешнему устройству;
            deviceQueue = new HpfQueue<Process>();
            deviceQueue_2 = new HpfQueue<Process>();
            // 8.   планировщик процессора;
            cpuScheduler = new CpuScheduler(Cpu, readyQueue);
            // 9.   планировщик памяти;
            memoryManager = new MemoryManager();
            // 10.  планировщик внешнего устройства;
            deviceScheduler = new DeviceScheduler(Device, deviceQueue);
            deviceScheduler_2 = new DeviceScheduler(Device_2, deviceQueue_2);
            // 11.  настройки модели предметной области; 
            ModelSettings = new Settings();
            // 12. статистика
            statistics = new Statistics(Clock);
            // 13.  генератор случайных чисел.
            processRand = new Random();
        }

        public void SaveSettings()
        {
            Ram.Save(ModelSettings.ValueOfRamSize);
            memoryManager.Save(Ram);
        }

        public void WorkingCycle() // рабочий такт вычислительной системы
        {
            //рабочий шаг часов
            Clock.WorkingCycle();
            double c = processRand.NextDouble();
            //через каждые 5 тактов сортирует очередь
            if (Clock.Clock % 5 == 0)
            {
                HpfQueue<Process> temp = (HpfQueue<Process>)readyQueue;
            }

            // если на данном шаге процесс поступит в систему
            if (c <= ModelSettings.Intensity)
            {
                // то создаётся процесс
                Process proc = new Process(idGen.Id, processRand.Next(ModelSettings.MinValueOfAddrSpace, ModelSettings.MaxValueOfAddrSpace + 1));
                statistics.ArrivalProcessesCount++;
                if (memoryManager.Allocate(proc.AddrSpace) != null)
                {
                    // время поступления процесса в вычислительную систему
                    proc.ArrivalTime = Clock.Clock;
                    // генерируется предпологаемый промежуток времени работы процесса на процессоре 
                    proc.BurstTime = processRand.Next(ModelSettings.MinValueOfBurstTime, ModelSettings.MaxValueOfBurstTime + 1);
                    // процесс становится в очередь готовых процессов
                    ReadyQueue = (HpfQueue<Process>)readyQueue.Put(proc,1);
                    // время последней постановки в очередь готовых процессов
                    proc.ReadyQueueArrivalTime = Clock.Clock;
                    // если процессор свободен
                    if (Cpu.IsFree())
                    {
                        putProcessOnResource(Cpu);
                        statistics.CpuFreeTime++;
                    }                  
                }
            }
            Cpu.WorkingCycle();
            Device.WorkingCycle();
            Device_2.WorkingCycle();
        }

        public void Clear() // очистка 
        {
            Unsubscribe(Cpu);
            Unsubscribe(Device);
            Unsubscribe(Device_2);
            // удаление процесса с процессора
            Cpu.Clear();
            // удаление процесса с внешнего устройства
            Device.Clear();
            Device_2.Clear();
            // очистка памяти
            Ram.Clear();
            // очистка очереди готовых процессов
            ReadyQueue = (HpfQueue<Process>)readyQueue.Clear();
            // очистка очереди к устройств
            DeviceQueue = (HpfQueue<Process>)deviceQueue.Clear();
            DeviceQueue_2 = (HpfQueue<Process>)deviceQueue_2.Clear();
            // очистка счётчика тактов
            Clock.Clear();
            // сброс настроек
            idGen.Clear();
            // очистка статистики
            statistics.Clear();
        }

        // компоненты модели:

        // 1.   часы;
        public readonly SystemClock Clock;       
        // 2.   центральный процессор;
        public readonly Resource Cpu;
        
        // 3.   внешние устройства;
        public readonly Resource Device;
        public readonly Resource Device_2;
        
        // 4.   ОП;
        public readonly Memory Ram;
        // 5.   генератор идентификаторов процессов;
        private IdGenerator idGen;
        // 6.   очередь готовых процессов;
        private HpfQueue<Process> readyQueue;
        // 7.   очередь к внешнему устройству;
        private HpfQueue<Process> deviceQueue;
        private HpfQueue<Process> deviceQueue_2;
        // 8    планировщик центрального процессора;
        private CpuScheduler cpuScheduler;
        // 9.   планировщик памяти;
        private MemoryManager memoryManager;
        // 10.  планировщик внешнего устройства;
        private DeviceScheduler deviceScheduler;
        private DeviceScheduler deviceScheduler_2;
        // 11.  генератор процессов (генератор случайных чисел);
        private Random processRand;
        // 12.  настройки модели предметной области.
        public readonly Settings ModelSettings;
        // 13. статистика
        public readonly Statistics statistics;

        public HpfQueue<Process> ReadyQueue
        {
            get
            {
                return (HpfQueue<Process>)readyQueue;
            }
            set
            {
                readyQueue = value;
                OnPropertyChanged();
            }
        }
        
        public HpfQueue<Process> DeviceQueue
        {
            get
            {
                return deviceQueue;
            }
            set
            {
                deviceQueue = value;
                OnPropertyChanged();
            }
        }
        public HpfQueue<Process> DeviceQueue_2
        {
            get
            {
                return deviceQueue_2;
            }
            set
            {
                deviceQueue_2 = value;
                OnPropertyChanged();
            }
        }


        // получатель события
        private void Subscribe(Resource resource)
        {
            if (resource.ActiveProcess != null)
            {
                resource.ActiveProcess.FreeingAResource += FreeingAResourceEventHandler;
            }
        }

        private void Unsubscribe(Resource resource)
        {
            if (resource.ActiveProcess != null)
            {
                resource.ActiveProcess.FreeingAResource -= FreeingAResourceEventHandler;
            }
        }
        private void putProcessOnResource(Resource resource)
        {
            if(resource == Cpu)
            {
                ReadyQueue = cpuScheduler.Session();
                resource.ActiveProcess.CommonWaitingTime += (Clock.Clock - resource.ActiveProcess.ReadyQueueArrivalTime);
            }
            else
            {
                if (resource == Device)
                {
                    DeviceQueue = deviceScheduler.Session();
                }
                else
                {
                    DeviceQueue_2 = deviceScheduler_2.Session();
                }
            }
            Subscribe(resource);
        }
        private void FreeingAResourceEventHandler(object sender, EventArgs e)
        {
            Process resourceFreeingProcess = sender as Process;
            NewEventArgs device = (NewEventArgs)e;

            switch (resourceFreeingProcess.Status)
            {
                case ProcessStatus.Terminated:
                    Unsubscribe(Cpu);
                    Cpu.Clear(); 
                    // освободить память занимаемую процессом;
                    memoryManager.Free(resourceFreeingProcess.AddrSpace);
                    if (readyQueue.Count != 0)
                    {
                        putProcessOnResource(Cpu);
                    }                                              
                    break;
                case ProcessStatus.Waiting:
                    Unsubscribe(Cpu);
                    Cpu.Clear();
                    if (readyQueue.Count != 0)
                    {
                        putProcessOnResource(Cpu);
                    }
                    statistics.TerminatedProcessesCount++;
                    resourceFreeingProcess.ResetWorkTime();

                    resourceFreeingProcess.BurstTime = processRand.Next(ModelSettings.MinValueOfBurstTime, ModelSettings.MaxValueOfBurstTime + 1);
                    switch (device.DeviceNumber)
                    {
                        case 1:
                            DeviceQueue = (HpfQueue<Process>)DeviceQueue.Put(resourceFreeingProcess,1);
                            if (Device.IsFree())
                            {
                                putProcessOnResource(Device);
                            }
                            break;
                        case 2:
                            DeviceQueue_2 = (HpfQueue<Process>)DeviceQueue_2.Put(resourceFreeingProcess,2);
                            if (Device_2.IsFree())
                            {
                                putProcessOnResource(Device_2);
                            }
                            break;
                        default:
                            break;
                    }      
                    break;
                case ProcessStatus.Ready:
                    switch (device.DeviceNumber)
                    {
                        case 1:
                            Unsubscribe(Device);
                            Device.Clear();
                            if (deviceQueue.Count != 0)
                            {
                                putProcessOnResource(Device);
                            }
                            break;
                        case 2:
                            Unsubscribe(Device_2);
                            Device_2.Clear();
                            if (deviceQueue_2.Count != 0)
                            {
                                putProcessOnResource(Device_2);
                            }
                            break;
                        default:
                            break;
                    }
                    resourceFreeingProcess.ResetWorkTime();
                    resourceFreeingProcess.BurstTime = processRand.Next(ModelSettings.MinValueOfBurstTime, ModelSettings.MaxValueOfBurstTime + 1);
                    ReadyQueue = (HpfQueue<Process>)readyQueue.Put(resourceFreeingProcess,1);
 
                    // время последней постановки в очередь готовых процессов
                    resourceFreeingProcess.ReadyQueueArrivalTime = Clock.Clock;
                    if (Cpu.IsFree())
                    {
                        putProcessOnResource(Cpu);
                    }
                    break;
                default:
                    throw new Exception("Unknown process status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}