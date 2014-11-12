using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CalendarProject
{
    public class MyCalendar 
    {
        private static DateTime date = new DateTime();

        public static MyCalendar Inctance { get; private set; }

        public  string Month
        {
            get 
            {
                switch (date.Month.ToString())
                {
                    case "1": return "Января";
                    case "2": return "Февраля";
                    case "3": return "Марта";
                    case "4": return "Апреля";
                    case "5": return "Мая";
                    case "6": return "Июня";
                    case "7": return "Июля";
                    case "8": return "Августа";
                    case "9": return "Сентября";
                    case "10": return "Октярбря";
                    case "11": return "Ноября";
                    case "12": return "Декабря";
                    default: return "";
                }
            }
        }

        public string Day
        {
            get { return MyCalendar.date.Day.ToString(); }
        }

        public string Year
        {
            get { return MyCalendar.date.Year.ToString(); }
        }


        static MyCalendar()
        {
            Inctance = new MyCalendar();
            date = DateTime.Now;
        }
    }
}
