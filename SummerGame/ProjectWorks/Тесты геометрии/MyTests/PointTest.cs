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
    public class PointTest : ITestable, IAutoGenerator
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
                throw new Exception("PointTest. Input is not Points.");
            }

            var inp = input as PointSet;

            Point a = inp[0];
            Point b = inp[1];

            bool x = Intersect.IsIntersected(a,b);
            if (x) return new PointSet(a);
            else return new PointSet(new Point(0,0));
            
        }

         IDrawable generate(VariablesSet pars)
        {

            //double r1 = pars["Radius1"].Value;
            //double r2 = pars["Radius2"].Value;

            //Segment a = new Segment(l, 0, 0, l);
            //Point p = new Point(2 * l * Math.Cos(angle), 2 * l * Math.Sin(angle));
            //Segment b = new Segment(0, 0, p.X, p.Y);

             Point a = new Point(20, 20);
             Point b = new Point(20.0000001, 20);

            return new PointSet(a,b);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public PointTest()
        {
            md = new AlgoMetaData("Point test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Radius1",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("Radius2",GenerationStrategy.Random,10,30)
                                                     });
        }
    }
   
}
