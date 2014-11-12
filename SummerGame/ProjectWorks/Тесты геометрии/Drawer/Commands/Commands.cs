using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Drawer.Commands
{
    public class Commands
    {
        public static RoutedCommand DrawerMoveCommand = new RoutedCommand();
        public static RoutedCommand DrawerZoomCommand = new RoutedCommand();
    }
}
