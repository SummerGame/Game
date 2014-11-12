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
using ProtoMap5.Commands;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace ProtoMap5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void XMLConvertClick(object sender, RoutedEventArgs e)
        {
        }

        #region MoveScreenCommand

        private void MoveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is MainWindow)
            {
                var window = sender as MainWindow;
                var page = window.MainFrame.Content as TestPage;
                var drp = page.ImageFrame.Content as DrawingPage;
                var drawing = drp.TheDrawer;
                var tr = drawing.RenderTransform.Value; //RenderTransform.Value;

                if ((e.Parameter as string) == "Right")
                {
                    tr.Translate(30, 0);
                }
                else if ((e.Parameter as string) == "Left")
                {
                    tr.Translate(-30, 0);
                }
                else if ((e.Parameter as string) == "Top")
                {
                    tr.Translate(0, 30);
                }
                else
                {
                    tr.Translate(0, -30);
                }

                drawing.RenderTransform = new MatrixTransform(tr);
            }
        }

        #endregion

        #region ZoomCommand

        private void ZoomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is MainWindow)
            {
                var window = sender as MainWindow;
                var page = window.MainFrame.Content as TestPage;
                var drp = page.ImageFrame.Content as DrawingPage;
                var drawing = drp.TheDrawer;
                var tr = drawing.RenderTransform.Value;

                if ((e.Parameter as string) == "In")
                {
                    tr.Scale(1.5, 1.5);
                }
                else
                {
                    tr.Scale(0.75, 0.75);
                }

                drawing.RenderTransform = new MatrixTransform(tr);
            }

        }

        #endregion
    }
}
