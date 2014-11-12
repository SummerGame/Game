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
using UserInterface;
using mapDrawer;


namespace SummerGame
{
    /// <summary>
    /// Класс, содержащий методы для событий, происходящих в главном меню
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            MainWindow.MainMenuWindow = this;
            DataContext = GameEngine.Game.GameLoaded;
        }

        /// <summary>
        /// Обработка нажатия кнопки "Новая игра". Скрывает главное меню и открывает окно со списками сценариев
        /// </summary>
        public void StartNewGame(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.ScenarioWindow = new ScenarioSelectionWindow();
                MainWindow.ScenarioWindow.Show();
                MainWindow.ScenarioWindow.Owner = MainWindow.TheMainWindow;

                MainWindow.MainMenuWindow.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка! Невозможно начать новую игру.");
            }
        }

        /// <summary>
        /// Открывает окно с настройками
        /// </summary>
        private void ShowOptionsWindow(object sender, RoutedEventArgs e)
        {
            if (MainWindow.OptionsWindow == null) MainWindow.OptionsWindow = new OptionsWindow();
            if (MainWindow.GameWindow.IsActive ) MainWindow.OptionsWindow.Owner = MainWindow.GameWindow;
            MainWindow.OptionsWindow.Show();
        }

        /// <summary>
        /// Закрывает приложение
        /// </summary>

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Продолжить". 
        /// Скрывает главное меню и возвращает игрока к игровому окну
        /// </summary>

        private void ContinueGame(object sender, RoutedEventArgs e)
        {
            MainWindow.MainMenuWindow.Hide();
        }

        /// <summary>
        /// Сейчас не используется
        /// Открывает окно для загрузки сохранённой игры
        /// </summary>
        private void LoadGame(object sender, RoutedEventArgs e)
        {
            try
            {
                UI.LoadGame();  
            }
            catch(Exception exception)
            {
                MessageBox.Show("Невозможно загрузить игру. Нет файлов сохраненной игры!");
            }

            
        }

        /// <summary>
        /// Сохранение текущей игры
        /// Сейчас не используется
        /// </summary>
        private void SaveGame(object sender, RoutedEventArgs e)
        {
            try
            {
                UI.SaveGame();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка! Невозможно сохранить игру.");
            }
        }

        /// <summary>
        /// Открывает окно редактора карт
        /// </summary>
        private void Tools(object sender, RoutedEventArgs e)
        {
            if (MainWindow.MainMenuWindow.Owner == MainWindow.GameWindow)
            {
                MainWindow.GameWindow.Hide();
                MainWindow.TheMainWindow.Show();
            }
            
            MainWindow.Drawer.Owner = this;
            MainWindow.Drawer.Show();
            MainWindow.MainMenuWindow.Hide();
            
        }

        /// <summary>
        /// Открывает окно со справкой об игре
        /// </summary>
        private void OpenHelpWindow(object sender, RoutedEventArgs e)
        {
            if (MainWindow.HelpWindow == null) MainWindow.HelpWindow = new HelpWindow();
            if (MainWindow.GameWindow.IsActive) MainWindow.HelpWindow.Owner = MainWindow.GameWindow;
            MainWindow.HelpWindow.Show();
        }
    }
}
