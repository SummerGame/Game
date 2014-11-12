using System.Collections.Generic;
using System.Linq;
using Drawer;
using Drawer.Transformers;
using Geometry.Figures;

namespace MyTests.Data
{
    public class PointSet : IDrawable
    {

        private List<Point> pts;

        public Point this[int i]
        {
            get { return pts[i]; }
        }

        public void PopulatePort(Port port, string prefix)
        {
            // Form a list of DrawableElement's
            List<DrawableElement> list = new List<DrawableElement>();
            List<string> strings = new List<string>();
            foreach (var s in pts)
            {
                list.Add(DrawableElement.Point(s.X, s.Y, "A", "x:" + s.X + " y:" + s.Y));
                strings.Add("x:" + s.X + " y:" + s.Y);
            }

            // To a port's chart named "prefix+..." add a list of DrawableElement's
            port[prefix + "Point"].Populate(list);
            port[prefix + "Text"].Populate( DrawableElement.Text(40,40,strings.ToArray()));
            port[prefix + "Point"].Settings.Color = "Black";
            //port.Settings.Transformer = new ChainTransformer(new ScaleTransformer(0.3, -0.3), new TranslateTransformer(50, 200));
        }

        public PointSet(params Point[] points)
        {
            pts = points.ToList();
        }
    }
}
