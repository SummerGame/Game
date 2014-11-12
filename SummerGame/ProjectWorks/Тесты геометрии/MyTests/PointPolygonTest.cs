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
    public class PointPolygonTest : ITestable, IAutoGenerator
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
                throw new Exception("PointPolygonTest. Input is not PointPolygonTest.");
            }

            var inp = input as PointSet;

            Point a = inp[0]; Point c = inp[1]; Point d = inp[2];
            var b = new ConvexPolygon(new Point(c.X,c.Y), new Point(d.X,c.Y), new Point(d.X,d.Y), new Point(c.X, d.Y));

            var x = Intersect.IsIntersected(a, b);
            //var x = Intersect.GetIntersection(b, a);
            if (x) return new DrawableSet(new List<IDrawable>() { new PointSet(), new PolygonSet(b), DrawableElement.Text(0, 0, "Vse horosho") });
            else return new DrawableSet(new List<IDrawable>() { new PointSet(), new PolygonSet(b), DrawableElement.Text(0, 0, "Vse ploho") });
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
            double angle = Math.PI * pars["Angle"].Value / 180;

            Point a = new Point(l * Math.Cos(angle), l * Math.Sin(angle));
            Point b = new Point(minX,minY);
            Point c = new Point(maxX, maxY);
            return new PointSet(a,b,c);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public PointPolygonTest()
        {
            md = new AlgoMetaData("PointPolygon test", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Angle",GenerationStrategy.Random,45),
                                                         new VariableGeneratorMetadata("Length",GenerationStrategy.Random,10,30),
                                                         new VariableGeneratorMetadata("minX",GenerationStrategy.Random,10,29),
                                                         new VariableGeneratorMetadata("maxX",GenerationStrategy.Random,30,60),
                                                         new VariableGeneratorMetadata("minY",GenerationStrategy.Random,10,29),
                                                         new VariableGeneratorMetadata("maxY",GenerationStrategy.Random,30,60)
                                                     });
        }

    }
}
