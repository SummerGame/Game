using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer;
using Geometry.Figures;
using MyTestEnvironment;
using Geometry;
using MyTests.Data;

namespace MyTests
{
    public class PolylinePoligonTest : ITestable, IAutoGenerator
    {
        private AlgoMetaData md;

        public AlgoMetaData MetaData
        {
            get { return md; }
        }


        IDrawable run(IDrawable input)
        {
            if (!(input is PointSet))
            {
                throw new Exception("PolylinePoligonTest. Input is not PolylinePoligonTest.");
            }

            var inp = input as PointSet;

            Point a = inp[0]; Point c = inp[1]; Point d = inp[2]; Point e = inp[4];
            Point polyPoint1 = inp[4];

            Point aa = new Point(20, 20); e = new Point(30, 20); Point ee = new Point(50,30);
            Segment b = new Segment(aa, e); Segment eee = new Segment(e, ee); Segment eae = new Segment(new Point(50, 30), a);
            Polyline polyline = new Polyline(20, 20, 30, 20, 50, 30, a.X, a.Y);
            
            //Segment b = new Segment(a,e);
            //Segment eee = new Segment(e,new Point(3*(a.X+e.X)/2,4*(a.Y+e.Y)/2));
            //var polyline = new Polyline(new List<Point>() { a, e, new Point((a.X + e.X) / 2, (a.Y + e.Y) / 2) });

            //var f = new ConvexPolygon(new Point(c.X, c.Y), new Point(d.X, c.Y), new Point(d.X, d.Y), new Point(c.X, d.Y));

            var f = new ConvexPolygon(new Point((c.X + d.X) / 2, c.Y), new Point(d.X, (c.Y + d.Y) / 3), new Point(d.X, 2 * ((d.Y + c.Y) / 3)), new Point((c.X + d.X) / 2, d.Y), new Point(c.X, 2 * ((d.Y + c.Y) / 3)), new Point(c.X, ((d.Y + c.Y) / 3)));


            var x = Intersect.GetIntersection(polyline,f);
            if (x.Count > 0) return new DrawableSet(new List<IDrawable>() { new PointSet(x.ToArray()), new SegmentSet(b,eee,eae),new PolygonSet(f), DrawableElement.Text(0, 0, "Vse horosho") });
            else return new DrawableSet(new List<IDrawable>() { new PointSet(x.ToArray()),new SegmentSet(b,eee,eae), new PolygonSet(f), DrawableElement.Text(0, 0, "Vse ploho") });
        }

        public Runner RunMethod
        {
            get { return run; }
        }

        private AutoGeneratorSettings settings;

        public AutoGeneratorSettings Settings
        {
            get { return settings; }
        }

        IDrawable generate(VariablesSet pars)
        {
            double l = pars["Length"].Value;
            double l2 = pars["Length2"].Value;
            double minX = pars["minX"].Value;
            double maxX = pars["maxX"].Value;
            double minY = pars["minY"].Value;
            double maxY = pars["maxY"].Value;
            double angle = Math.PI * pars["Angle"].Value / 180;
            double angle2 = Math.PI * pars["Angle"].Value / 180;

            Point a = new Point(l * Math.Cos(angle), l * Math.Sin(angle));
            Point e = new Point(l2 * Math.Cos(angle2), l2 * Math.Sin(angle2));
            Point b = new Point(minX, minY);
            Point c = new Point(maxX, maxY);
            Point d = new Point(0,0);
            return new PointSet(a, b, c, d,e);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public PolylinePoligonTest()
        {
            md = new AlgoMetaData("PolylinePoligonTest test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Angle",GenerationStrategy.Random,0,45),
                                                         new VariableGeneratorMetadata("Length",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("pointfirst",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("minX",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("maxX",GenerationStrategy.Random,40,60),
                                                         new VariableGeneratorMetadata("minY",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("maxY",GenerationStrategy.Random,40,60),
                                                         new VariableGeneratorMetadata("Angle2",GenerationStrategy.Random,0,45),
                                                         new VariableGeneratorMetadata("Length2",GenerationStrategy.Random,10,30)
                                                     });
        }

    }
}
