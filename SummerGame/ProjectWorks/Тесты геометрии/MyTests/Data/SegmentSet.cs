using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer;
using Geometry.Figures;

namespace MyTests.Data
{
    public class SegmentSet : IDrawable
    {
        private List<Segment> sgts;

        public Segment this[int i]
        {
            get { return sgts[i]; }
        }

        public void PopulatePort(Port port, string prefix)
        {
            // Form a list of DrawableElement's
            List<DrawableElement> list = new List<DrawableElement>();
            foreach (var s in sgts)
            {
                list.Add(DrawableElement.Segment(s.Begin.X, s.Begin.Y, s.End.X, s.End.Y));
            }

            // To a port's chart named "prefix+..." add a list of DrawableElement's
            port[prefix + "Segments"].Populate( list );
        }

        public SegmentSet( params Segment[] segs )
        {
            sgts = segs.ToList();
        }
    }
}
