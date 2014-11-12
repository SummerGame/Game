using System;
using System.Collections.Generic;
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

namespace SummerGame
{
    /// <summary>
    /// Логика взаимодействия для Wокна свойств
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
            MainWindow.OptionsWindow = this;
        }

        /// <summary>
        /// Скрывает окно. Происходит после функции Close 
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку, закрывающую окно
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Меняет свойство, отвечающее за смену цветов юнитов при паузе в игре
        /// </summary>
        private void ChangeColours_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
