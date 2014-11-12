using System;
using System.Windows.Input;
using Microsoft.Win32;
using MyTestEnvironment;
using MyTests;
using MyTests.Data;
using ProtoMap5.TestControls;
using System.Windows;
using System.Collections.Generic;

namespace ProtoMap5
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage
    {

        static MainTest MainTest = new MainTest(new List<ITestable> 
        { 
            // All runnable tests
            new SegmentTest(), 
            new CircleTest(),
            new PointTest(),
            new PointSegmentTest(),
            new PointCircleTest(),
            new SegmentCircleTest(),
            new SegmentPolygonTest(),
            new PointPolygonTest(),
            new PolylinePoligonTest(),
            new PolygonTest()
        });

        public TestPage()
        {
            InitializeComponent();

            // Set MainTest properties in all auxiluary pages to the same instance
            // Now all data changes will be visible to all pages 
            // since MainTest implements INotifyPropertyChanged
            TestRunControls.MainTest = MainTest;
            TestSelector.MainTest = MainTest;
            DrawingPage.MainTest = MainTest;
        }


        #region Commands

        #region NextPageCommand

        public void NextPageCommandCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            var type = ControlFrame.Content.GetType();
            if (type == typeof(TestSelector))
            {
                var t = (TestSelector)ControlFrame.Content;
                if (t.AlgoSelector.Text != "" && t.DataSourceSelector.Text != "")
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
            else if (type == typeof(autogenSettings))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public void NextPageCommandExecute(object src, ExecutedRoutedEventArgs e)
        {
            var type = ControlFrame.Content.GetType();
            if (type == typeof(TestSelector))
            {
                var page = (TestSelector)ControlFrame.Content;
                var value = (InputCreationMode)page.DataSourceSelector.SelectedValue;
                if (value == InputCreationMode.AutoGeneration)
                {
                    ControlFrame.Source = new Uri("TestControls\\autogenSettings.xaml", UriKind.Relative);
                }
                else if (value == InputCreationMode.FromFile)
                {
                    var dlg = new OpenFileDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        MainTest.FileNames = new List<string>(dlg.FileNames);
                        MainTest.CurrentTestNumber = 1;
                        ControlFrame.Source = new Uri("TestControls\\testRunControls.xaml", UriKind.Relative);
                    }
                }
                else
                {

                }
                //TODO insert input settings analysis
            }
            else if (type == typeof(autogenSettings))
            {
                ControlFrame.Source = new Uri("TestControls\\testRunControls.xaml", UriKind.Relative);
                MainTest.CurrentTestNumber = 1;
            }
        }

        #endregion

        #region PrevPageCommand

        public void PrevPageCommanCanExecute(object src, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void PrevPageCommandExecute(object src, ExecutedRoutedEventArgs e)
        {
            var type = ControlFrame.Content.GetType();

            if (type == typeof(autogenSettings))
            {
                ControlFrame.Source = new Uri("TestControls\\testSelector.xaml", UriKind.Relative);
            }
            else if (type == typeof(TestRunControls))
            {
                if (MainTest.SelectedTest.MetaData.AutoGenerator != null)
                    ControlFrame.Source = new Uri("TestControls\\autogenSettings.xaml", UriKind.Relative);
                else
                {
                    MainTest.SelectedTest = null;
                    ControlFrame.Source = new Uri("TestControls\\testSelector.xaml", UriKind.Relative);
                }
            }
        }

        #endregion
        
        #endregion

    }

}
