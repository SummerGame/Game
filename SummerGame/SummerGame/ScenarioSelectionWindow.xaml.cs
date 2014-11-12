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
using System.Text.RegularExpressions;
using System.Windows.Shapes;
using System.IO;
using UserInterface;

namespace SummerGame
{
    /// <summary>
    /// Логика для окна с выбором сценариев
    /// </summary>
    public partial class ScenarioSelectionWindow : Window
    {
        public ScenarioSelectionWindow()
        {
            InitializeComponent();
            MainWindow.ScenarioWindow = this;
            GetAllowedMaps();

        }

        /// <summary>
        /// Обработчик события нажатия на кнопку, закрывающую окно
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// функция получает доступные карты для загрузки
        /// </summary>
        private void GetAllowedMaps() 
        {
            List<string> allowedMaps = new List<string>();
            //var files = Directory.GetFiles(Environment.CurrentDirectory);

            //DirectoryInfo a = new DirectoryInfo("Maps");
            var files = Directory.GetFiles("..\\..\\Maps");
            //TODO сделать через готовую библиотечку
            foreach (var fileName in files)
            {
                var isXml = fileName.Split(new string[] { "\\" }, StringSplitOptions.None);
                string[] temp = isXml.Last().Split('.');
                if (temp.Last() == "xml")
                {
                    allowedMaps.Add(isXml[3]);
                }
            }
            foreach (var map in allowedMaps)
            {
                AllowedScenarios.Items.Add(map);
            }
        }

        /// <summary>
        /// Загрузка карты для игры
        /// </summary>
        private void LoadMap(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainMenuWindow.Hide();
            MainWindow.TheMainWindow.Hide();
            MainWindow.ScenarioWindow.Hide();

            UI.NewGame(AllowedScenarios.SelectedItem.ToString());
            if (MainWindow.GameWindow == null) MainWindow.GameWindow = new GameWindow();
                        
            MainWindow.GameWindow.Show();

            //установка фонового изображения            
            var childrens = UserInterface.StoryConverter.FindVisualChild<ContentPresenter>(MainWindow.GameWindow.TheMap);
            Rectangle rectangle;
            if (childrens != null)
            {
                rectangle = (Rectangle)UI.TheMapControl.ContentTemplate.FindName("BackgroundImage", childrens);
                if (UI.GamePresenter.Map.Background != null && UI.GamePresenter.Map.Background !="")
                    rectangle.Fill = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + UI.GamePresenter.Map.Background, UriKind.Relative)));
            }
        }

        /// <summary>
        /// Вызывается после выполнения функции Close()
        /// </summary>
        private void ClosingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow.MainMenuWindow.Show();
        }
    }
}
