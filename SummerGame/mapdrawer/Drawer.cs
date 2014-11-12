using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Geometry;
using UserInterface;

namespace mapDrawer
{
    public static class Drawer
    {
        private static string landName;

        // +
        private static List<Geometry.Figures.Point> clickedPoints = new List<Geometry.Figures.Point>();
        private static List<Shape> selectedShapes = new List<Shape>();
        private static List<GameEngine.Lands.Landscape> selectedLands = new List<GameEngine.Lands.Landscape>();

        public static List<GameEngine.Lands.Landscape> SelectedLands
        {
            get { return Drawer.selectedLands; }
            set { Drawer.selectedLands = value; }
        }

        private static UserInterface.Presenters.FigurePresenter line;


        public static UserInterface.Presenters.FigurePresenter Line
        {
            get { return Drawer.line; }
            set { Drawer.line = value; }
        }

        public static List<Shape> SelectedShapes
        {
            get { return Drawer.selectedShapes; }
            set { Drawer.selectedShapes = value; }
        }

        public static List<Geometry.Figures.Point> ClickedPoints
        {
            get { return Drawer.clickedPoints; }
            set { Drawer.clickedPoints = value; }
        }

        public static string LandName
        {
            get { return Drawer.landName; }
            set { Drawer.landName = value; }
        }

    }
}
