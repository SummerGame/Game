using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Drawer.Transformers;

namespace Drawer
{
    public enum DrawableType { Text, Circle, Ellipse, Point, Segment, Vector, Polyline, Polygon }

    public class DrawableElement : IDrawable
    {

        #region Point reper

        private struct ReperPoint
        {
            public double x, y;
            public ReperPoint(double x, double y) { this.x = x; this.y = y; }
        }

        private List<ReperPoint> reper;

        #endregion

        #region Fields

        List<double> parameters;

        #endregion

        #region Properties

        public DrawableType Type { get; set; }

        public int Top { get; set; } // check whether they are used

        public int Left { get; set; } // check whether they are used

        public List<double> Parameters
        {
            get
            {
                return parameters;
            }
        }

        public List<string> Comment { get; set; }

        public VisualSettings Settings { get; set; }

        #endregion

        #region Factory Methods

        public static DrawableElement Text(int x, int y, params string[] strings)
        {
            var settings = new VisualSettings(null, color: "Black");
            return new DrawableElement
            {
                Type = DrawableType.Text,
                Comment = new List<string>(strings),
                Settings = settings,
                reper = new List<ReperPoint> { new ReperPoint(x, y) }
            };
        }

        public static DrawableElement Point(double x, double y, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            return new DrawableElement
            {
                Type = DrawableType.Point,
                Comment = new List<string>() { comment },
                reper = new List<ReperPoint> { new ReperPoint(x, y) },
                Settings = settings
            };
        }

        public static DrawableElement Ellipse(double ax, double ay, double bx, double by, double l, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            return new DrawableElement
            {
                Type = DrawableType.Ellipse,
                Comment = new List<string>() { comment },
                reper = new List<ReperPoint> { new ReperPoint(ax, ay), new ReperPoint(bx, by), new ReperPoint(ax + l, ay) },
                Settings = settings
            };
        }

        public static DrawableElement Circle(double x, double y, double r, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            return new DrawableElement
                {
                    Type = DrawableType.Circle,
                    Comment = new List<string>() { comment },
                    reper = new List<ReperPoint> { new ReperPoint(x, y), new ReperPoint(x + r, y) },
                    Settings = settings
                };
        }

        public static DrawableElement Segment(double x0, double y0, double x1, double y1, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            return new DrawableElement
            {
                Type = DrawableType.Segment,
                Comment = new List<string>() { comment },
                reper = new List<ReperPoint> { new ReperPoint(x0, y0), new ReperPoint(x1, y1) },
                Settings = settings
            };
        }

        public static DrawableElement Vector(double px, double py, double vx, double vy, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            var l = Math.Sqrt(vx * vx + vy * vy);

            return new DrawableElement
            {
                Type = DrawableType.Vector,
                Comment = new List<string>() { comment },
                reper = new List<ReperPoint> { 
                    new ReperPoint ( px, py ), 
                    new ReperPoint ( px + l, py ),
                    // here's new points to form vector head
                    new ReperPoint ( px + 5 * l / 6, py + l / 12),
                    new ReperPoint ( px + 11 * l / 12, py ),
                    new ReperPoint ( px + 5 * l / 6, py - l / 12 ),
                    new ReperPoint ( px + vx, py + vy ) // this point[5] is to remember angle and should be replaced later
                },
                Settings = settings
            };
        }

        public static DrawableElement Polygon(List<double> coords, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            List<ReperPoint> points = new List<ReperPoint>();
            for (int i = 0; i + 1 < coords.Count; i += 2)
            {
                points.Add(new ReperPoint(coords[i], coords[i + 1]));
            }
            return new DrawableElement()
            {
                Type = DrawableType.Polygon,
                Comment = new List<string>() { comment },
                reper = points,
                Settings = settings
            };
        }

        public static DrawableElement Polyline(List<double> coords, string name = null, string comment = null, string color = "Undefined")
        {
            var settings = new VisualSettings(name, color: color);
            List<ReperPoint> points = new List<ReperPoint>();
            for (int i = 0; 2 * i + 1 < coords.Count; i++)
            {
                points.Add(new ReperPoint(coords[2 * i], coords[2 * i + 1]));
            }
            return new DrawableElement()
            {
                Type = DrawableType.Polyline,
                Comment = new List<string>() { comment },
                reper = points,
                Settings = settings
            };
        }

        #endregion

        public void Update()
        {
            parameters = new List<double>();
            double x, y;
            if (reper != null)
            {
                parameters = new List<double>();

                // reper points transformation
                if (Settings != null && Settings.Transformer != null)
                    foreach (var point in reper)
                    {
                        Settings.Transformer.ToScreenCoords(point.x, point.y, out x, out y);
                        parameters.Add(x); parameters.Add(y);
                    }
                else
                    foreach (var point in reper)
                    {
                        EmptyTransformer.ToScreenCoords(point.x, point.y, out x, out y);
                        parameters.Add(x); parameters.Add(y);
                    }

                // fine-tuninig a Vector
                if (Type == DrawableType.Vector)
                {
                    double px = parameters[0]; double py = parameters[1];
                    double qx = parameters[10]; double qy = parameters[11];
                    double vx = qx - px; double vy = qy - py;
                    double l = Math.Sqrt(vx * vx + vy * vy);
                    var hx = l != 0 ? vx / l : vx;
                    var hy = l != 0 ? vy / l : vy;
                    var angle = hy > 0 ? Math.Acos(hx) : -Math.Acos(hx);
                    angle *= 180 / Math.PI;
                    parameters[10] = parameters[2];
                    parameters[11] = parameters[3];
                    parameters.Add(angle); // this is parameters[12], it transfers angle
                }

                // fine-tuninig a Circle
                else if (Type == DrawableType.Circle)
                {
                    double px = parameters[0]; double py = parameters[1];
                    double qx = parameters[2]; double qy = parameters[3];
                    double dx = qx - px; double dy = qy - py;
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    parameters[0] = px - r;
                    parameters[1] = py - r;
                    parameters[2] = 2 * r;
                }

                // fine-tuninig an Ellipse
                else if (Type == DrawableType.Ellipse)
                {
                    double px = parameters[0]; double py = parameters[1];
                    double qx = parameters[2]; double qy = parameters[3];
                    double dx = qx - px; double dy = qy - py;
                    double c = Math.Sqrt(dx * dx + dy * dy) / 2; // half the interfocus distance

                    double rx = parameters[4]; double ry = parameters[5];
                    double Dx = px - rx; double Dy = py - ry;
                    double l = Math.Sqrt(Dx * Dx + Dy * Dy); // thread length

                    double a = l / 2; // bigger half-axis
                    double b = (a > c) ? Math.Sqrt(a * a - c * c) : 0; // smaller half-axis

                    double angle = (dy > 0) ? Math.Acos(dx / (2 * c)) : -Math.Acos(dx / (2 * c));

                    parameters[0] = px - (a - c); // left-top angle of a rectangle
                    parameters[1] = py - b;
                    parameters[2] = 2 * a; // its width and height
                    parameters[3] = 2 * b;
                    parameters[4] = 180 * angle / Math.PI; // angle was in radians and counterclockwise
                    parameters[5] = a - c; // centre for rotation
                    parameters.Add(b); // we had only 6 doubles after reper transformation, now we need a 7-th one
                }

            }
        }

        public void PopulatePort(Port port, string prefix)
        {
            port[prefix].Populate(this);
        }
    }

}
