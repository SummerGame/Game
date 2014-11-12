using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace StateMachine
{
    public static class MachineDrawer
    {
        #region Fields
        public static List<FrameworkElement> shapeList = new List<FrameworkElement>();
        private static List<int> eachSide = new List<int>();
        private static List<Vertex> vertices = new List<Vertex>();
        private static List<Edge> edges = new List<Edge>();
        private static double canvasWidth;
        private static double canvasHeight;
        private static Machine myMachine = new Machine();
        #endregion

        #region Functions
        public static void GenerateVertices(Machine machine, double x, double y)
        {
            eachSide = new List<int>();
            vertices = new List<Vertex>();
            shapeList = new List<FrameworkElement>();
            edges = new List<Edge>();

            canvasHeight = y;
            canvasWidth = x;

            myMachine = machine;
            int i;
            int count = myMachine.States.Count;
            int rest = count - (count / 4) * 4; // остаток
            int onEachSide = count / 4;  // количество на каждой стороне

            for (i = 0; i < 4; i++)
            {
                eachSide.Add(onEachSide);
            }

            for (i = 0; i < rest; i++)
            {
                eachSide[i] += 1;
            }

            GenerateVertices();
            GenerateEdges();
            DrawVertices();
            DrawEdges();
        }

        static void HorizontalLine(double end, int count, double indention, int currentVertex)
        {
            double interval = end / (count + 1);
            double start = interval;
            for (int i = 0; i < count; i++)
            {
                string name = myMachine.States[currentVertex].Name;
                Vertex vertex = new Vertex(name, start, indention);
                start += interval;
                vertices.Add(vertex);
                currentVertex++;
            }
        }

        static void VerticalLine(double end, int count, double indention, int currentVertex)
        {
            double interval = end / (count + 1);
            double start = interval;
            for (int i = 0; i < count; i++)
            {
                string name = myMachine.States[currentVertex].Name;
                Vertex vertex = new Vertex(name, indention, start);
                start += interval;
                vertices.Add(vertex);
                currentVertex++;
            }
        }

        private static void GenerateVertices()
        {
            double topInterval = (canvasHeight) / ((eachSide[2] + 1) * 2);
            double leftInterval = (canvasWidth) / ((eachSide[0] + 1) * 2);
            // горизонтальная верхняя прямая
            HorizontalLine(canvasWidth, eachSide[0], topInterval, 0);
            // вертикальная правая прямая
            VerticalLine(canvasHeight, eachSide[1], canvasWidth - leftInterval, eachSide[0]);
            // горизонтальная нижняя прямая
            HorizontalLine(canvasWidth, eachSide[2], canvasHeight - topInterval, eachSide[0] + eachSide[1]);
            // вертикальная левая прямая
            VerticalLine(canvasHeight, eachSide[3], leftInterval, eachSide[0] + eachSide[1] + eachSide[2]);

        }

        private static void GenerateEdges()
        {
            foreach (var function in myMachine.Functions)
            {

                string from = function.From.Name;
                string to = function.To.Name;
                Vertex toEdge = vertices.Where(n => n.Name == to).ToList()[0] as Vertex;
                Vertex fromEdge = vertices.Where(n => n.Name == from).ToList()[0] as Vertex;

                Edge edge = new Edge(from, to, function.Letter.Name);
                edges.Add(edge);

                if (from != to)
                {
                    Arrow arrow = new Arrow();
                    arrow.X2 = toEdge.W;
                    arrow.Y2 = toEdge.H;

                    arrow.X1 = fromEdge.W;
                    arrow.Y1 = fromEdge.H;

                    arrow.HeadWidth = 15;
                    arrow.HeadHeight = 5;

                    arrow.Stroke = new SolidColorBrush(Colors.Black);
                    arrow.StrokeThickness = 2;

                    shapeList.Add(arrow);
                }
                else
                {
                    var cirlesCoord = GetCircles(toEdge);
                    foreach (var x in cirlesCoord)
                    {
                        Ellipse ell = new Ellipse();
                        ell.Stroke = new SolidColorBrush(Colors.Green);
                        ell.StrokeThickness = 2;
                        ell.Height = x.Item2;
                        ell.Width = x.Item2;
                        Canvas.SetLeft(ell, toEdge.W); //x.Item1.X
                        Canvas.SetTop(ell, toEdge.H - x.Item2 / 2); // x.Item1.Y
                        shapeList.Add(ell);
                    }
                }
            } edges = ClearEdges();
        }

        private static void DrawVertices()
        {
            foreach (Vertex vertex in vertices)
            {
                Ellipse ell = new Ellipse();
                ell.Height = 5;
                ell.Width = 5;
                Canvas.SetLeft(ell, vertex.W);
                Canvas.SetTop(ell, vertex.H);
                ell.Name = "State_" + vertex.Name;
                ell.Fill = new SolidColorBrush(Colors.Red);
                shapeList.Add(ell);

                TextBlock tb = new TextBlock();
                tb.Text = vertex.Name;
                Canvas.SetLeft(tb, vertex.W);
                Canvas.SetTop(tb, vertex.H);
                shapeList.Add(tb);
            }

            try
            {
                string firstStateName = myMachine.FirstState.Name;
                var firstEll = shapeList.Where(n => n.Name == ("State_" + firstStateName)).ToList()[0] as Ellipse;
                firstEll.Height = 10;
                firstEll.Width = 10;
                firstEll.Fill = new SolidColorBrush(Colors.Yellow);
            }
            catch { }

        }

        private static List<Edge> ClearEdges()
        {
            List<Edge> unique = new List<Edge>();
            for (int i = 0; i < edges.Count; i++)
            {
                var all = edges.Where(n => (n.From == edges[i].From && n.To == edges[i].To)).ToList();
                if (all.Count == 1)
                {
                    unique.Add(all[0]);
                }
                else
                {
                    for (int j = 1; j < all.Count; j++)
                    {
                        all[0].Names.Add(all[j].Names[0]);
                    }
                    unique.Add(all[0]);
                }
                foreach (var edge in all)
                {
                    edges.Remove(edge);
                }
                i--;
            }
            return unique;
        }

        private static Size MeasureString(string candidate,TextBlock tb)
        {
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch), tb.FontSize,Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private static void DrawEdges()
        {

            foreach (var edge in edges)
            {
                double width = 0;
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                var x1 = (vertices.Where(n => n.Name == edge.From).ToList()[0].W + vertices.Where(n => n.Name == edge.To).ToList()[0].W) / 2;
                var y1 = (vertices.Where(n => n.Name == edge.From).ToList()[0].H + vertices.Where(n => n.Name == edge.To).ToList()[0].H) / 2;
                foreach (string name in edge.Names)
                {
                    TextBlock tb1 = new TextBlock();
                    tb1.Text = name;
                    tb1.FontSize = 14;
                    tb1.FontWeight = FontWeights.UltraBold;
                    tb1.TextWrapping = TextWrapping.Wrap;
                    sp.Children.Add(tb1);
                    var size = MeasureString(name, tb1);
                    width += size.Width;
                    tb1.Foreground = new SolidColorBrush(Colors.Green);

                    TextBlock tb2 = new TextBlock();
                    tb2.Text = ",";
                    tb2.FontSize = 14;
                    tb2.FontWeight = FontWeights.UltraBold;
                    tb2.MinWidth = 5;
                    sp.Children.Add(tb2);
                    size = MeasureString(name, tb2);
                    width += size.Width;
                    tb2.Foreground = new SolidColorBrush(Colors.Green);
                }
                Canvas.SetLeft(sp, x1-width/2);
                Canvas.SetTop(sp, y1);
                sp.Children.RemoveAt(sp.Children.Count - 1);
                shapeList.Add(sp);
            }
        }

        private static int GetCirclesCount(Vertex vertex)
        {
            State state = myMachine.States.Where(s => s.Name == vertex.Name).ToList()[0];
            int count = 0;
            foreach (var function in myMachine.Functions)
            {
                if ((function.From == state) && (function.To == state)) count++;
            }
            return count;
        }

        private static List<Tuple<Point, int>> GetCircles(Vertex vertex)
        {
            Point PolygonCenter = new Point(canvasWidth / 2, canvasHeight / 2);
            Point statePoint = new Point(vertex.W, vertex.H);

            Vector a = new Vector(statePoint.X - PolygonCenter.X, statePoint.Y - PolygonCenter.Y);
            if (a.Length == 0) a = new Vector(100, 100);
            double rad = a.Length;
            Vector itog = new Vector(a.X / rad, a.Y / rad);

            var list = new List<Tuple<Point, int>>();
            var count = GetCirclesCount(vertex);
            if (count == 0) throw new Exception("Невозможно построить окружность");
            else
            {
                for (int i = 1; i <= count; i++)
                {
                    rad = 30 * i;
                    Point newPoint = new Point(statePoint.X + rad * itog.X / Math.Sqrt(2) - rad, statePoint.Y + rad * itog.Y / Math.Sqrt(2) - rad);
                    list.Add(new Tuple<Point, int>(newPoint, (int)rad));
                }
                return list;
            }

        }

        #endregion
    }

    public class Vertex
    {
        #region Fields
        private string name;
        private double w;
        private double h;
        #endregion

        #region Properties

        public double W
        {
            get { return w; }
            set { w = value; }
        }

        public double H
        {
            get { return h; }
            set { h = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        public Vertex(string name, double w, double h)
        {
            this.w = w;
            this.h = h;
            this.name = name;
        }
    }

    public class Edge
    {
        private string from;

        public string From
        {
            get { return from; }
            set { from = value; }
        }
        private string to;

        public string To
        {
            get { return to; }
            set { to = value; }
        }
        private List<string> names;

        public List<string> Names
        {
            get { return names; }
            set { names = value; }
        }

        public Edge(string from, string to, string name)
        {
            names = new List<string>();
            this.from = from;
            this.to = to;
            this.names.Add(name);
        }
    }
}
