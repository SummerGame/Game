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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameEngine;
using mapDrawer;

namespace SummerGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //FIELDS

        public static HelpWindow HelpWindow;
        public static GameWindow GameWindow;
        public static OptionsWindow OptionsWindow;
        public static ObjectProperties PropertiesWindow;
        public static MainMenu MainMenuWindow;
        public static MainWindow TheMainWindow;
        public static MapEditorWindow Drawer;
        public static ScenarioSelectionWindow ScenarioWindow;

        //

        public MainWindow()
        {
            InitializeComponent();

            TheMainWindow = this;

            if (MainMenuWindow == null)
                MainMenuWindow = new MainMenu();

            if (GameWindow == null)
                GameWindow = new GameWindow();

            if (Drawer == null)
                Drawer = new MapEditorWindow();

            if (ScenarioWindow == null)
                ScenarioWindow = new ScenarioSelectionWindow();

            if (HelpWindow == null)
                HelpWindow = new HelpWindow();
        }

        /// <summary>
        /// Открывает главное окно с главным меню
        /// </summary>
        private void Window_Activated(object sender, EventArgs e)
        {
            MainMenuWindow.Owner = this;
            MainMenuWindow.Show();
        }

        
    }
}
