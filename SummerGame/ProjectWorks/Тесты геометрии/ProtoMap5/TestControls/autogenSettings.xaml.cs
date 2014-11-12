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
using MyTestEnvironment;

namespace ProtoMap5.TestControls
{
    /// <summary>
    /// Interaction logic for autogenSettings.xaml
    /// </summary>
    public partial class autogenSettings : Page
    {
        public autogenSettings()
        {
            InitializeComponent();

            SettingsList.ItemsSource = MainTest.SelectedTestAutoGeneratorSettings;
        }
    }
}
