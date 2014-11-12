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
using System.Windows.Media.Animation;
using GameEngine;
using UserInterface;
using GameEngine.Characters;
using UserInterface.Presenters;
using System.IO;
using System.Windows.Markup;

namespace SummerGame
{
    /// <summary>
    /// Класс, содержащий методы для событий, происходящих в окне помощи
    /// </summary>
    public partial class HelpWindow : Window
    {
        /// <summary>
        /// Загрузка текста в окно помощи из файла
        /// </summary>
        public HelpWindow()
        {
            InitializeComponent();
            MainWindow.HelpWindow = this;
            TextRange helpDoc = new TextRange(Helper.Document.ContentStart, Helper.Document.ContentEnd);
            using (FileStream fs = new FileStream("..\\..\\..\\Help.txt", FileMode.Open))
                helpDoc.Load(fs, "Text");
        }

        /// <summary>
        /// Скрывает окно
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Не используется
        /// </summary>
        private void Load_Help()
        {
            //XamlReader.Load          
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку, закрывающую окно
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
