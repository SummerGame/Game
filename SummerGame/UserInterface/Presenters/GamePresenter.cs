using System.Windows;
using System.Windows.Interop;
using System;
using GameEngine;
using System.ComponentModel;

namespace UserInterface.Presenters
{
    public class GamePresenter : DependencyObject
    {

        #region Dependency Properties

        #region GameTime

        public string CurrentGameTime
        {
            get { return (string)GetValue(CurrentGameTimeProperty); }
            set { SetValue(CurrentGameTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentGameTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentGameTimeProperty =
            DependencyProperty.Register("CurrentGameTime", typeof(string), typeof(GamePresenter), new PropertyMetadata(String.Empty));

        #endregion

        #region Background

        /// <summary>
        /// Фоновое изображение карты
        /// </summary>
        public string Background
        {
            get { return (string)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(string), typeof(GamePresenter), new UIPropertyMetadata(null));

        #endregion

        #region Side

        public string ActiveSide
        {
            get { return (string)GetValue(ActiveSideProperty); }
            set { SetValue(ActiveSideProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveSide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveSideProperty =
            DependencyProperty.Register("ActiveSide", typeof(string), typeof(GamePresenter), new UIPropertyMetadata(String.Empty));


        #endregion

        #region Map

        public MapPresenter Map
        {
            get { return (MapPresenter)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Map.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(MapPresenter), typeof(GamePresenter), new UIPropertyMetadata(null));

        #endregion

        #region Something else

        #endregion

        #endregion

        #region Backing fields

        DateTime startTime; // TODO: remove this?

        #endregion

        #region Constructors

        public GamePresenter(DateTime startTime, int side)
            : base()
        {
            this.startTime = startTime;
            this.Map = new MapPresenter();
            this.UpdateTime(0);
            this.UpdateSide(side);
            MyDate.Inctance.Hour = startTime.Hour;
        }

        public GamePresenter(DateTime startTime, int side, string bg)
            : base()
        {
            Background = bg;
            this.startTime = startTime;
            this.Map = new MapPresenter();
            this.UpdateTime(0);
            this.UpdateSide(side);
            MyDate.Inctance.Hour = startTime.Hour;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обновление времени
        /// </summary>
        /// <param name="currentTime"></param>
        public void UpdateTime(double currentTime)
        {
            DateTime current = startTime.AddHours(currentTime * Game.GameTimeInterval);// todo change to Transformer.TimeConverter
            MyDate.Inctance.AddHour(Game.GameTimeInterval);
            MyDate.Inctance.UpdateDate(current);
        }

        /// <summary>
        /// Смена игрока означает изменение видимости объектов, т. к. у каждого игрока своё видение карты
        /// </summary>
        /// <param name="side"></param>
        public void UpdateSide(int side)
        {
            ActiveSide = "Player" + (side + 1);
            Map.UpdateVisibility();
        }

        /// <summary>
        /// Обновление карты в случае загрузки новой карты
        /// </summary>
        /// <param name="newMap"></param>
        public void UpdateMap(GameMap newMap)
        {
            Map.Update(newMap);

            // This is to redraw the map at once.
            // Crap, I don't know how to do it nicely
            var m = Map;
            Map = null;
            Map = m;
        }

        // map needs to redraw
        public void UpdateMap()
        {
            Map.Update();
        }

        #endregion

    }

    public class MyDate : INotifyPropertyChanged
    {
        private MyDate() { }
        public void AddHour(int hour)
        {
            if (this.hour != 23)
            {
                this.hour += hour;
            }
            else
            {
                this.hour = 0;
            }
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs("Hour"));
            }
        }

		/// <summary>
        /// Обновление даты
        /// </summary>
        /// <param name="time">Принимает новую дату</param>
        public void UpdateDate(DateTime time)
        {
            this.day = time.Day;
            this.month = time.Month;
            this.year = time.Year;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs("Day"));
                handler(this, new PropertyChangedEventArgs("Month"));
                handler(this, new PropertyChangedEventArgs("Year"));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields
        private int hour;
        private int day;
        private int month;
        private int year;
        #endregion

        #region Properties
		//TODO вынести преобразование в конвертер 
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
                    case 12: return 0;
                    case 13: return -30;
                    case 14: return -60;
                    case 15: return -90;
                    case 16: return -120;
                    case 17: return -150;
                    case 18: return -180;
                    case 19: return -210;
                    case 20: return -240;
                    case 21: return -270;
                    case 22: return -300;
                    case 23: return -330;
                    case 24: return -330;
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
        public string Day
        {
            get { return this.day.ToString(); }
            internal set
            {
                if (day.ToString() != value)
                {
                    Day = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Day"));
                    }
                }
            }
        }
        public string Month
        {
            get
            {
                switch (month)
                {
                    case 1: return "Января";
                    case 2: return "Февраля";
                    case 3: return "Марта";
                    case 4: return "Апреля";
                    case 5: return "Мая";
                    case 6: return "Июня";
                    case 7: return "Июля";
                    case 8: return "Августа";
                    case 9: return "Сентября";
                    case 10: return "Октября";
                    case 11: return "Ноября";
                    case 12: return "Декабря";
                    default: return "";
                }
            }
        }
        public string Year
        {
            get { return year.ToString(); }
        }
        #endregion

        #region Static part
        public static MyDate Inctance { get; private set; }
        static MyDate()
        {
            Inctance = new MyDate();
        }
        #endregion
    }
}
