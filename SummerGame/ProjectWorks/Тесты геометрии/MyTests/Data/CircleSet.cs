using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer;
using Drawer.Transformers;
using Geometry.Figures;

namespace MyTests.Data
{
    class CircleSet : IDrawable
    {
        private List<Circle> sts;

        public Circle this[int i]
        {
            get { return sts[i]; }
        }

        public void PopulatePort(Port port, string prefix)
        {
            // Form a list of DrawableElement's
            List<DrawableElement> list = new List<DrawableElement>();
            foreach (var s in sts )
            {
                list.Add(DrawableElement.Circle(s.Center.X,s.Center.Y,s.Radius));
            }

            // To a port's chart named "prefix+..." add a list of DrawableElement's
            port[prefix + "Circle"].Populate(list);
            port[prefix + "Circle"].Settings.Color = "Black";
            port.Settings.Transformer = new ChainTransformer( new ScaleTransformer(0.3,-0.3), new TranslateTransformer(50,200));
        }

        public CircleSet(params Circle[] circles)
        {
            sts = circles.ToList();
        }
    }
}
