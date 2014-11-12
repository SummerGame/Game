using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using ProtoMap5.TestControls;

namespace ProtoMap5.Commands
{
    class InterfaceCommands
    {

        //static readonly InputGestureCollection gests = new InputGestureCollection() { new MouseGesture() { MouseAction = MouseAction.LeftClick } };

        public static readonly RoutedCommand RunCommand = new RoutedCommand("RunTestsCommand",typeof(TestRunControls));
        public static readonly RoutedCommand StopCommand = new RoutedCommand("StopCommand", typeof(TestRunControls));
        public static readonly RoutedCommand PauseCommand = new RoutedCommand("PauseCommand", typeof(Button));
        public static readonly RoutedCommand ResumeCommand = new RoutedCommand("ResumeCommand", typeof(Button));
        public static readonly RoutedCommand RestartCommand = new RoutedCommand("RestartCommand", typeof(Button));
        public static readonly RoutedCommand PrevOneCommand = new RoutedCommand("PrevOneCommand", typeof(Button));
        public static readonly RoutedCommand NextOneCommand = new RoutedCommand("NextOneCommand", typeof(Button));
        public static readonly RoutedCommand PrevManyCommand = new RoutedCommand("PrevManyCommand", typeof(Button));
        public static readonly RoutedCommand NextManyCommand = new RoutedCommand("NextManyCommand", typeof(Button));
        //public readonly RoutedCommand UpdateDrawing

        #region Constructor

        static InterfaceCommands()
        {

        }

        #endregion

    }
}
