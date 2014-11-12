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
    public class SegmentPolygonTest : ITestable, IAutoGenerator
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
                throw new Exception("SegmentPolygonTest. Input is not SegmentPolygonTest.");
            }

            var inp = input as PointSet;

            Point a = inp[0]; Point c = inp[1]; Point d = inp[2]; Point e = inp[3];
            
            //a = new Point(10, 10); e = new Point(40, 10); Point ee = new Point(60, 40);
            //Segment b = new Segment(a, e); //Segment bbb = new Segment(e, ee);
            Segment b = new Segment(10, (c.Y + d.Y) / 2.5, 100, (c.Y + d.Y) / 2.5);
            var f = new ConvexPolygon(new Point((c.X + d.X) / 2, c.Y), new Point(d.X, (c.Y + d.Y) / 3), new Point(d.X, 2 * ((d.Y + c.Y) / 3)), new Point((c.X + d.X) / 2, d.Y), new Point(c.X, 2*((d.Y + c.Y) / 3)), new Point(c.X, ((d.Y + c.Y) / 3)));
            var fa = new ConvexPolygon(new Point((c.X + d.X) / 2 + 40, c.Y), new Point(d.X + 40, (c.Y + d.Y) / 3), new Point(d.X + 40, 2 * ((d.Y + c.Y) / 3)), new Point((c.X + d.X) / 2 + 40, d.Y), new Point(c.X + 40, 2 * ((d.Y + c.Y) / 3)), new Point(c.X + 40, ((d.Y + c.Y) / 3)));
            //var f = new ConvexPolygon(new Point(c.X, c.Y), new Point(d.X, c.Y), new Point(d.X, d.Y), new Point(c.X, d.Y));

            //var f = new ConvexPolygon(new Point(40,40), new Point(70,40), new Point(70,70), new Point(40,70));
            //b = new Segment(30,50,40,50);

            var x = Intersect.GetIntersection(b, f);// var y = Intersect.GetIntersection(f);
            var xx = Intersect.GetIntersection(b, fa);
            if (x.Count + xx.Count > 0) return new DrawableSet(new List<IDrawable>() 
            { new PointSet(x.Concat(xx).ToArray()), 
                new SegmentSet(b),
                new PolygonSet(f,fa), 
                DrawableElement.Text(0, 0, "Vse horosho") 
            });
            else return new DrawableSet(new List<IDrawable>() 
            { new PointSet(x.Concat(xx).ToArray()),
                new SegmentSet(b),
                new PolygonSet(f, fa), 
                DrawableElement.Text(0, 0, "Vse ploho") 
            });
            //return new PointSet(x.ToArray());
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
            double minX = pars["minX"].Value;
            double maxX = pars["maxX"].Value;
            double minY = pars["minY"].Value;
            double maxY = pars["maxY"].Value;

            double poi = pars["pointfirst"].Value;
            double angle = Math.PI * pars["Angle"].Value / 180;

            Point a = new Point(l * Math.Cos(angle), l * Math.Sin(angle));
            Point b = new Point(minX, minY);
            Point c = new Point(maxX, maxY);
            Point d = new Point(poi, poi);
            return new PointSet(a, b, c, d);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public SegmentPolygonTest()
        {
            md = new AlgoMetaData("SegmentPolygon test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Angle",GenerationStrategy.Random,45),
                                                         new VariableGeneratorMetadata("Length",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("pointfirst",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("minX",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("maxX",GenerationStrategy.Random,40,60),
                                                         new VariableGeneratorMetadata("minY",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("maxY",GenerationStrategy.Random,40,60)
                                                     });
        }

    }
}
