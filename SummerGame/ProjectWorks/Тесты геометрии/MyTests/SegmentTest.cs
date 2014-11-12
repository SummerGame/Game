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
    public class SegmentTest : ITestable, IAutoGenerator
    {
        private AlgoMetaData md;

        public AlgoMetaData MetaData
        {
            get { return md; }
        }


        IDrawable run(IDrawable input)
        {
            if (!(input is SegmentSet))
            {
                throw new Exception("SegmentTest. Input is not Segments.");
            }

            var inp = input as SegmentSet;

            Segment a = inp[0];
            Segment b = inp[1];

            //Circle circle = new Circle(a.End,1);

            //Segment a = new Segment(20,0,20,80);
            //Segment b = new Segment(0, 20, 0, 80);

            var x = Intersect.GetIntersection(a, b);
            //var x = Intersect.GetArrowBase(a.Begin, a.End);
            if (x.Count > 0) return new PointSet(x[0]);
            else return new PointSet();
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

            double angle1 = Math.PI * pars["Angle1"].Value / 180;
            double angle2 = Math.PI * pars["Angle2"].Value / 180;
            double l = pars["Length"].Value;

            Segment a = new Segment(l * Math.Cos(angle2), 40, 40, l * Math.Sin(angle2));
            Point p = new Point(2 * l * Math.Cos(angle1), 2 * l * Math.Sin(angle1));
            Segment b = new Segment(40, 40, p.X, p.Y);

            //Segment b = new Segment(0, -20, 50, -20);
            //Segment b = new Segment(25, -25, 25, 25);
            //Segment a = new Segment(new Point(0,0),p );

            return new SegmentSet(a,b);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public SegmentTest()
        {
            md = new AlgoMetaData("Segments test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Angle1",GenerationStrategy.Random,45),
                                                         new VariableGeneratorMetadata("Angle2",GenerationStrategy.Random,45),
                                                         new VariableGeneratorMetadata("Length",GenerationStrategy.Random,10,30)
                                                     });
        }

    }
}
