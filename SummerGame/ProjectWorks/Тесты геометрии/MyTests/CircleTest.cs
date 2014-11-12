using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer;
using Geometry.Figures;
using MyTestEnvironment;
using Geometry;
using MyTests.Data;


namespace MyTests.Data
{
    public class CircleTest : ITestable, IAutoGenerator
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
            if (!(input is CircleSet))
            {
                throw new Exception("CircleTest. Input is not Circles.");
            }

            var inp = input as CircleSet;

            Circle a = inp[0];
            Circle b = inp[1];

            bool x = Intersect.IsIntersected(a,b);
            if (x) return DrawableElement.Text(0, 0, "Vse horosho");
            else return DrawableElement.Text(0, 0, "Vse ploho");
            
        }

         IDrawable generate(VariablesSet pars)
        {

            double r1 = pars["Radius1"].Value;
            double r2 = pars["Radius2"].Value;
            double c1 = pars["Center1"].Value;
            double c2 = pars["Center2"].Value;

            //Segment a = new Segment(l, 0, 0, l);
            //Point p = new Point(2 * l * Math.Cos(angle), 2 * l * Math.Sin(angle));
            //Segment b = new Segment(0, 0, p.X, p.Y);

             Circle a = new Circle(c1, c1, r1);
             Circle b = new Circle(c2, c2, r2);

            return new CircleSet(a,b);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public CircleTest()
        {
            md = new AlgoMetaData("Circle test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Radius1",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("Radius2",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("Center1",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("Center2",GenerationStrategy.Random,20,30)
                                                     });
        }
    }
}
