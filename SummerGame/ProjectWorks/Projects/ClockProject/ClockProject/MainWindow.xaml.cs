﻿using System;
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

namespace ClockProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MyClock.Inctance.Hour = 3;
        }

        private void Hours_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MyClock.Inctance.Hour = Convert.ToInt32(Hours.Value);
        }
    }
}