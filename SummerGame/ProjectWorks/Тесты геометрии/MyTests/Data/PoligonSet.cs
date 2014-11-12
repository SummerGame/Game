using System.Collections.Generic;
using System.Linq;
using Drawer;
using Drawer.Transformers;
using Geometry.Figures;

namespace MyTests.Data
{
    class PolygonSet : IDrawable
    {
        private List<ConvexPolygon> polts;

        public ConvexPolygon this[int i]
        {
            get { return polts[i]; }
        }


        public void PopulatePort(Port port, string prefix)
        {
            // Form a list of DrawableElement's
            List<DrawableElement> list = new List<DrawableElement>();
            List<string> strings = new List<string>();

            foreach (var s in polts)
            {
                List<double> newConPol = new List<double>();
                foreach (var d in s.Points)
                {
                    newConPol.Add(d.X);
                    newConPol.Add(d.Y);
                }
                list.Add(DrawableElement.Polygon(newConPol));
                //strings.Add("x:" + s.X + " y:" + s.Y);
            }
            port[prefix + "ConPol"].Populate(list);
            //port[prefix + "Text"].Populate(DrawableElement.Text(40, 40, strings.ToArray()));
            port[prefix + "ConPol"].Settings.Color = "Black";
            port.Settings.Transformer = new ChainTransformer(new ScaleTransformer(0.3, -0.3),
                                                             new TranslateTransformer(50, 200));

            
        }

        public PolygonSet(params ConvexPolygon[] polig)
        {
            polts = polig.ToList();
        }
    }
}
