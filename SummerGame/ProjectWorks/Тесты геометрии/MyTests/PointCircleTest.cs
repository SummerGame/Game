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
    public class PointCircleTest : ITestable, IAutoGenerator
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
                throw new Exception("PointCircleTest. Input is not PointCircleTest.");
            }

            var inp = input as PointSet;
            
            Point a = inp[0];
            Point b = inp[1];
            Point c = inp[2];

            Circle d = new Circle(b.X, b.Y, c.X);

            bool x = Intersect.IsIntersected(a,d);
            if (x) return new DrawableSet(new List<IDrawable>(){new PointSet(a),new CircleSet(d),DrawableElement.Text(0,0,"Vse horosho")});
            else return new DrawableSet(new List<IDrawable>() { new PointSet(a), new CircleSet(d), DrawableElement.Text(0, 0, "Vse Ploho") });
            
        }

         IDrawable generate(VariablesSet pars)
        {

            double po = pars["Point"].Value;
            double r = pars["Radius"].Value;

            //Segment a = new Segment(l, 0, 0, l);
            //Point p = new Point(2 * l * Math.Cos(angle), 2 * l * Math.Sin(angle));
            //Segment b = new Segment(0, 0, p.X, p.Y);

             Point a = new Point(po,po);
             Point b = new Point(40, 40);
             Point c = new Point(r, r);
             

            return new PointSet(a,b,c);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public PointCircleTest()
        {
            md = new AlgoMetaData("PointCircle test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Point",GenerationStrategy.Random,0,30),
                                                         new VariableGeneratorMetadata("Radius",GenerationStrategy.Random,0,20)
                                                     });
        }
    }
   
}
