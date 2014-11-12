using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Geometry;
using Geometry.Figures;
using System.Windows.Media;

namespace UserInterface.Presenters
{
    public class FigurePresenter : DependencyObject
    {

        #region Backing fields

        /// <summary>
        /// A figure <see cref="Geometry"/> being presented
        /// </summary>
        private Figure original;



        /// <summary>
        /// A constant used to draw all single points in WPF
        /// </summary>
        private const int pointRadius = 3;

        #endregion

        #region Dependency properties

        /// <summary>
        /// Type property of type System.Type
        /// Used by data templates
        /// </summary>

        #region Type

        public Type Type
        {
            get { return (Type)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(Type), typeof(FigurePresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Center property is used by every kind of figure and is vital for correct positioning.
        /// </summary>
        /// <remarks>
        /// For points and circles center if displaced from the geometrical center to enable WPF-correct drawing
        /// For polylines and polygons is counted as a mass center.
        /// </remarks>
        /// <seealso cref="UpdateCenter"/>

        #region Center

        public System.Windows.Point Center
        {
            get { return (System.Windows.Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Center.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(System.Windows.Point), typeof(FigurePresenter), new UIPropertyMetadata(new System.Windows.Point(0, 0)));

        #endregion

        /// <summary>
        /// Radius property used as of now for circles and points only. Defaults to zero.
        /// </summary>

        #region Radius

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(FigurePresenter), new UIPropertyMetadata(0d));

        #endregion

        /// <summary>
        /// AParent property is used so that each figure knew an area it is bound to
        /// </summary>

        #region AParent

        public AbstractGameEngine.Area AParent
        {
            get { return (AbstractGameEngine.Area)GetValue(AParentProperty); }
            set { SetValue(AParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AParentProperty =
            DependencyProperty.Register("AParent", typeof(AbstractGameEngine.Area), typeof(FigurePresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Points property is used to draw polygons, segments and polylines. 
        /// Points are calculated in relative coordinates to the figure center <seealso cref="Center"/>
        /// </summary>

        #region Points

        public System.Windows.Media.PointCollection Points
        {
            get { return (System.Windows.Media.PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(System.Windows.Media.PointCollection), typeof(FigurePresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// A smoothing ratio given as a property. Effective for convex polygons only (NB! could change later)
        /// Valid values are doubles in [0,1] range.
        /// </summary>

        #region Smoothing

        public double Smoothing
        {
            get { return (double)GetValue(SmoothingProperty); }
            set { SetValue(SmoothingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Smoothing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmoothingProperty =
            DependencyProperty.Register("Smoothing", typeof(double), typeof(FigurePresenter), new UIPropertyMetadata(0d, SmoothingChanged, SmoothingChanging));


        // that's for formal requirements
        private static void SmoothingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// A callback ensures a valid Smoothing parameter will be written. Valid parameters are in range [0,1]
        /// </summary>
        /// <param name="d">an object with changing Smoothing property</param>
        /// <param name="newValue">a new value attempted to be written</param>
        /// <returns>valid property value</returns>
        private static object SmoothingChanging(DependencyObject d, object newValue)
        {
            double newval = (double)newValue;
            double validval = newval;
            if (newval < 0) validval = 0;
            if (newval > 1) validval = 1;
            return validval;
        }

        #endregion

        /// <summary>
        /// Smoothed Path.Data string to be used for smoothed polygons and polylines rendering
        /// </summary>

        #region Smoothed path

        public StreamGeometry SmoothedPath
        {
            get { return (StreamGeometry)GetValue(SmoothedPathDataProperty); }
            set { SetValue(SmoothedPathDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmoothedPathData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmoothedPathDataProperty =
            DependencyProperty.Register("SmoothedPath", typeof(StreamGeometry), typeof(FigurePresenter), new UIPropertyMetadata(null));

        #endregion

        #endregion

        public FigurePresenter(Figure f, AbstractGameEngine.Area parent)
        {
            this.original = f;
            Type = f.GetType();
            AParent = parent;

            // todo find another way for that?
            if (Type == typeof(ConvexPolygon))
            {
                if (parent.GetType() == typeof(GameEngine.Lands.Mountains))
                    Smoothing = 1;
                if (parent.GetType() == typeof(GameEngine.Lands.Forest))
                    Smoothing = 0.5;
                if (parent.GetType() == typeof(GameEngine.Lands.Lowland))
                    Smoothing = 0.8;
            }

            Update();
        }

        

        #region Private and internal methods

        /// <summary>
        /// Calculates a displaced center of a figure according to current Transformer state.
        /// </summary>
        /// <remarks>
        /// A center is displaced equally by X and Y coordinates for a value of Radius property.
        /// Before that Radius property is recalculated according to figure type and current Transformer state.
        /// </remarks>
        /// <returns>System.Windows.Point of a displaced center</returns>
        private void UpdateCenter()
        {
            Geometry.Figures.Point p = new Geometry.Figures.Point(0, 0);

            if (original is Circle) p = (original as Circle).Center;
            else if (original is Geometry.Figures.Point) p = original as Geometry.Figures.Point;
            else if (original is ConvexPolygon) p = (original as ConvexPolygon).Center();
            else if (original is Polyline) p = (original as Polyline).Center();
            else if (original is Segment)
            {
                var segment = original as Segment;
                var v = Geometry.Vector.RadiusVector(segment.Begin) + Geometry.Vector.RadiusVector(segment.End);
                p = new Geometry.Figures.Point(v.X / 2, v.Y / 2);
            }

            Geometry.Figures.Point q = Transformer.ConvertToScreen(p);

            UpdateRadius();
            q.X -= Radius; q.Y -= Radius;

            Center = new System.Windows.Point(q.X, q.Y);
        }

        /// <summary>
        /// Updates a radius value for a given figure.
        /// </summary>
        /// <remarks>
        /// A constant <see cref="pointRadius"/>pointRadius is used for points
        /// A transformed radius is used for points
        /// All other figures get radius of 0
        /// </remarks>
        private void UpdateRadius()
        {
            if (original is Geometry.Figures.Point) Radius = pointRadius;
            else if (original is Circle) Radius = Transformer.ConvertToScreenLength((original as Circle).Radius);
            else Radius = 0;
        }

        /// <summary>
        /// Calculates relative (to the Center) screen points of a polygon, segment or polyline.
        /// For a convex polygon also counts the smoothed path to be used in rendering
        /// </summary>
        private void UpdatePoints()
        {
            List<Geometry.Figures.Point> originalPoints = new List<Geometry.Figures.Point>();
            List<System.Windows.Point> result = null;

            if (original is Polyline)
                originalPoints.AddRange((original as Polyline).Points);
            else if (original is ConvexPolygon)
                originalPoints.AddRange((original as ConvexPolygon).Points);
            else if (original is Segment)
            {
                var segment = original as Segment;
                originalPoints.Add(segment.Begin);
                originalPoints.Add(segment.End);
            }

            if (originalPoints.Count != 0)
            {
                result = new List<System.Windows.Point>();
                foreach (var p in originalPoints)
                {
                    var screenPoint = Transformer.ConvertToScreen(p);
                    var relativeScreenPoint = new System.Windows.Point(screenPoint.X - Center.X, screenPoint.Y - Center.Y);
                    result.Add(relativeScreenPoint);
                }
            }

            if (result != null)
            {
                Points = new PointCollection(result);

                // smoothing insertion -- for now it is for polygons only
                if (original is ConvexPolygon)
                {
                    SmoothedPath = BezierSmoothing(Points, Smoothing);
                }
            }

        }

        /// <summary>
        /// A total points update method
        /// </summary>
        internal void Update()
        {
            UpdateCenter();
            UpdatePoints(); // points are relative to center, so update them after center updating
        }

        /// <summary>
        /// A method used to calculate Smoothed path geometry for a given points collection with a given smoothing ratio
        /// </summary>
        /// <param name="points">points collection (presumably, polygon points in screen coordinates)</param>
        /// <param name="k">smoothing ratio</param>
        /// <returns>a StreamGeometry to be used in drawing</returns>
        private StreamGeometry BezierSmoothing(PointCollection points, double k)
        {
            StreamGeometry result = new StreamGeometry();

            if (k > 0 && k <= 1)
            {
                using (var geometry = result.Open())
                {
                    int count = points.Count;

                    var p = new System.Windows.Point((points[count - 1].X + points[0].X) / 2, (points[count - 1].Y + points[0].Y) / 2);

                    geometry.BeginFigure(p, true, true);

                    for (int i = 0; i < count; i++)
                    {
                        int m = i == 0 ? count - 1 : i - 1; // prev point index
                        int n = i == count - 1 ? 0 : i + 1; // next point index

                        var dx = points[i].X - points[m].X; dx *= 1 - (k * 0.5);
                        var dy = points[i].Y - points[m].Y; dy *= 1 - (k * 0.5);
                        System.Windows.Point p1 = new System.Windows.Point(points[m].X + dx, points[m].Y + dy);

                        dx = points[n].X - points[i].X; dx *= k * 0.5;
                        dy = points[n].Y - points[i].Y; dy *= k * 0.5;
                        System.Windows.Point p2 = new System.Windows.Point(points[i].X + dx, points[i].Y + dy);

                        geometry.LineTo(p1, true, false);
                        geometry.QuadraticBezierTo(points[i], p2, true, false);
                    }

                    geometry.Close();

                }


            }

                // no smoothing if k invalid or just 0
            else
            {
                using (var geometry = result.Open())
                {
                    geometry.BeginFigure(points[0], true, true);
                    for (int i = 0; i < points.Count; i++)
                    {
                        geometry.LineTo(points[i], true, false);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
