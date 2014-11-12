using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows;
using MyTestEnvironment;
using System;

namespace ProtoMap5.TestControls
{
    /// <summary>
    /// Interaction logic for testControls.xaml
    /// </summary>
    public partial class TestRunControls : Page
    {

        public static MainTest MainTest { get; set; }

        #region Fields & Properties

        // Has DependencyProperty
        public int Frequency
        {
            get { return (int)GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }

        // regime of tests sequence - false if test are to be timer-invoked
        // has dependency property
        public bool ManualRegime
        {
            get { return (bool)GetValue(ManualRegimeProperty); }
            set { SetValue(ManualRegimeProperty, value); }
        }

        private readonly Timer _timer = new Timer();

        #endregion

        #region Dependency Properties

        #region ManualRegime

        public static DependencyProperty ManualRegimeProperty =
            DependencyProperty.Register("ManualRegime", typeof(bool), typeof(TestRunControls),
                                           new PropertyMetadata(true, OnRegimeChange, OnRegimeCoerce));

        private static void OnRegimeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object OnRegimeCoerce(DependencyObject d, object value)
        {
            var the = (TestRunControls)d;
            var val = (bool)value;
            the._timer.Enabled = !val;
            return value;
        }

        #endregion

        #region Frequency

        public static DependencyProperty FrequencyProperty =
            DependencyProperty.Register("Frequency", typeof(int), typeof(TestRunControls),
                                       new PropertyMetadata(10, OnCurrentFreqChange, OnCurrentFreqCoerce));

        private static void OnCurrentFreqChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object OnCurrentFreqCoerce(DependencyObject d, object value)
        {
            var newvalue = (int)value;
            var the = (TestRunControls)d;
            var oldvalue = the.Frequency;
            if (newvalue > 0) { the._timer.Interval = 10000 / newvalue; }
            else newvalue = oldvalue;
            return newvalue;
        }

        #endregion

        #endregion

        #region Commands

        #region Run All Tests

        public void RunTestsCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ManualRegime && (MainTest.MaxTestNumber > 0)
                && (MainTest.CurrentTestNumber != MainTest.MaxTestNumber);
        }

        public void RunTestsExecute(object src, ExecutedRoutedEventArgs e)
        {
            ManualRegime = false;
            //todo execute command code
        }

        #endregion

        #region Stop Tests Run

        public void StopRunCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ManualRegime;
        }

        public void StopRunExecute(object src, ExecutedRoutedEventArgs e)
        {
            ManualRegime = true;
            MainTest.CurrentTestNumber = 0;
            //todo execute command code
        }

        #endregion

        #region Pause Tests Run

        public void PauseRunCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ManualRegime;
        }

        public void PauseRunExecute(object src, ExecutedRoutedEventArgs e)
        {
            ManualRegime = true;
            //todo execute command code
        }

        #endregion

        #region Restart

        public void RestartCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ManualRegime && (MainTest.MaxTestNumber > 0);
        }

        public void RestartExecuted(object src, ExecutedRoutedEventArgs e)
        {
            MainTest.CurrentTestNumber = 1;
            if ( MainTest.MaxTestNumber > 1 ) ManualRegime = false;
        }

        #endregion

        #region Run Next Test

        public void RunNextTestCanExecute(object src, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = ManualRegime && (MainTest.CurrentTestNumber < MainTest.MaxTestNumber);
        }

        public void RunNextTestExecute(object src, ExecutedRoutedEventArgs e)
        {
            MainTest.CurrentTestNumber += 1;
            //todo execute command code
        }

        #endregion

        #region Run Prev Test

        public void RunPrevTestCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ManualRegime && (MainTest.CurrentTestNumber > 1);
        }

        public void RunPrevTestExecute(object src, ExecutedRoutedEventArgs e)
        {
            MainTest.CurrentTestNumber -= 1;
            //todo execute command code
        }

        #endregion

        #endregion

        private void TimerTick(object src, object e)
        {
            if (MainTest.CurrentTestNumber < MainTest.MaxTestNumber)
                MainTest.CurrentTestNumber += 1;
            
            if (MainTest.CurrentTestNumber == MainTest.MaxTestNumber)
            {
                this.Focus();
                ManualRegime = true;
            }
        }

        public TestRunControls()
        {
            _timer.Enabled = false;
            _timer.Tick += TimerTick;
            Frequency = 10;

            InitializeComponent();
            FrequencyPanel.DataContext = this;
            MainBox.DataContext = MainTest;
        }

        private void ButtonClick(object sender, RoutedEventArgs e) //todo look how to evade ButtonClick event
        {
            var button = (System.Windows.Controls.Button)sender;
            button.Command.Execute(null);
        }

    }
}
