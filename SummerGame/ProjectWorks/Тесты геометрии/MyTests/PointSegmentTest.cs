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
    public class PointSegmentTest : ITestable, IAutoGenerator
    {
        private AlgoMetaData md;

        public AlgoMetaData MetaData
        {
            get { return md; }
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

        IDrawable run(IDrawable input)
        {
            if (!(input is PointSet))
            {
                throw new Exception("PointSegmentTest. Input is not PointSegmentTest.");
            }

            var inp = input as PointSet;

            Point a = inp[0];
            Point b = inp[1];
            Point c = inp[2];
            Segment d = new Segment(b, c);

            bool x = Intersect.IsIntersected(a,d);
            if (x)
                return
                    new DrawableSet(new List<IDrawable>() {new SegmentSet(d), DrawableElement.Text(0, 0, "Vse horosho")});
            else return new DrawableSet(new List<IDrawable>() { new SegmentSet(d), DrawableElement.Text(0, 0, "Vse ploho") });
                
        }

         IDrawable generate(VariablesSet pars)
        {

            double po = pars["Point"].Value;
            double le = pars["Length"].Value;

            //Segment a = new Segment(l, 0, 0, l);
            //Point p = new Point(2 * l * Math.Cos(angle), 2 * l * Math.Sin(angle));
            //Segment b = new Segment(0, 0, p.X, p.Y);

             Point a = new Point(po,po);
             Point b = new Point(0, 0);
             Point c = new Point(le, le);
             

            return new PointSet(a,b,c);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public PointSegmentTest()
        {
            md = new AlgoMetaData("PointSegment test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Point",GenerationStrategy.Random,20,50),
                                                         new VariableGeneratorMetadata("Length",GenerationStrategy.Random,0,40)
                                                     });
        }
    }
   
}
