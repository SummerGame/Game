using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace ClockProject
{
    public class MyClock:INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int hour;
        public int Hour
        {
            get
            {
                switch (hour)
                {
                    case 0: return 0;
                    case 1: return -30;
                    case 2: return -60;
                    case 3: return -90;
                    case 4: return -120;
                    case 5: return -150;
                    case 6: return -180;
                    case 7: return -210;
                    case 8: return -240;
                    case 9: return -270;
                    case 10: return -300;
                    case 11: return -330;
                    default: return 0;
                }
            }
            internal set
            {
                if (hour != value)
                {
                    hour = value;
                    
                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Hour"));
                    }
                }
            }
        }
        private MyClock() { }

        public static MyClock Inctance { get; private set; }
        static MyClock()
        {
            Inctance = new MyClock();
        }
    }
}
