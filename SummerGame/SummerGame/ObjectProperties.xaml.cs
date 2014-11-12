using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameEngine.Characters;
using UserInterface.Presenters;
using GameEngine.Characters.Groups;
using UserInterface;

namespace SummerGame
{
    /// <summary>
    /// Логика взаимодействия для ObjectProperties.xaml (для окна свойств юнита)
    /// </summary>
    public partial class ObjectProperties : Window, INotifyPropertyChanged
    {
        public UnitPresenter TheUnit
        {
            get { return theUnit; }
            set
            {
                theUnit = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TheUnit"));
            }
        }

        private static UnitPresenter theUnit;

        /// <summary>
        /// Открывает окно свойств юнита
        /// </summary>
        public ObjectProperties()
        {
            InitializeComponent();
            MainWindow.PropertiesWindow = this;
            AGrid.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Обработка нажатия кнопок в окне свойств юнита
        /// </summary>
        private void AGrid_Click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            switch (button.Name)
            {
                case "ClosingButton":
                    {
                        this.Hide();
                        break;
                    }
                case "BuildButton":
                    {
                        ContentControl unitControl = e.Source as ContentControl;
                        UnitPresenter unitPresenter = unitControl.Content as UnitPresenter;
                        Unit unit = unitPresenter.Unit;
                        UI.Build(unit);
                        break;
                    }
            }
        }

    }
}
