using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MyTestEnvironment;
using ProtoMap5.Commands;

namespace ProtoMap5.TestControls
{
    /// <summary>
    /// Interaction logic for testControls1.xaml
    /// </summary>
    public partial class TestSelector : Page
    {
        public static MainTest MainTest { get; set; }

        public TestSelector()
        {
            InitializeComponent();

            DataContext = MainTest;
        }

        private void AlgoSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainTest.SelectTest((string)AlgoSelector.SelectedValue);
            DataSourceSelector.ItemsSource = MainTest.SelectedTestInputMethods();
        }

    }
}
