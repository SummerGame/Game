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
    public class PolygonTest : ITestable, IAutoGenerator
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
                throw new Exception("PolygonTest. Input is not PolygonTest.");
            }
            var inp = input as PointSet;
            Point c = inp[0]; Point d = inp[1]; Point e = inp[2]; Point fe = inp[3];

            ConvexPolygon f1 = new ConvexPolygon(new Point((c.X + d.X) / 2, c.Y), new Point(d.X, (c.Y + d.Y) / 3), new Point(d.X, 2 * ((d.Y + c.Y) / 3)), new Point((c.X + d.X) / 2, d.Y), new Point(c.X, 2 * ((d.Y + c.Y) / 3)), new Point(c.X, ((d.Y + c.Y) / 3)));
            ConvexPolygon f2 = new ConvexPolygon(new Point((e.X + fe.X) / 2, e.Y), new Point(fe.X, (e.Y + fe.Y) / 3), new Point(fe.X, 2 * ((fe.Y + e.Y) / 3)), new Point((e.X + fe.X) / 2, fe.Y), new Point(e.X, 2 * ((fe.Y + e.Y) / 3)), new Point(e.X, ((fe.Y + e.Y) / 3)));
            var x = Intersect.GetIntersection(f1,f2);
            if (x.Count > 0) return new DrawableSet(new List<IDrawable>() { new PointSet(x.ToArray()),new PolygonSet(f1,f2), DrawableElement.Text(0, 0, "Vse horosho") });
            else return new DrawableSet(new List<IDrawable>() { new PointSet(x.ToArray()), new PolygonSet(f1, f2), DrawableElement.Text(0, 0, "Vse ploho") });
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
            double minX1 = pars["minX1"].Value;
            double maxX1 = pars["maxX1"].Value;
            double minY1 = pars["minY1"].Value;
            double maxY1 = pars["maxY1"].Value;
            double minX2 = pars["minX2"].Value;
            double maxX2 = pars["maxX2"].Value;
            double minY2 = pars["minY2"].Value;
            double maxY2 = pars["maxY2"].Value;
            double angle = Math.PI * pars["Angle"].Value / 180;

            //Point a = new Point(l * Math.Cos(angle), l * Math.Sin(angle));
            Point b = new Point(minX1, minY1);
            Point c = new Point(maxX1, maxY1);

            Point d = new Point(minX2 * Math.Cos(angle), minY2 * Math.Cos(angle));
            Point e = new Point(maxX2 * Math.Cos(angle), maxY2 * Math.Cos(angle));
            return new PointSet(b, c, d,e);
        }

        public InputGenerator InputGenerator
        {
            get { return generate; }
        }


        public PolygonTest()
        {
            md = new AlgoMetaData("PolygonTest", this);
            settings = new AutoGeneratorSettings(new List<VariableGeneratorMetadata>()
                                                     {
                                                         new VariableGeneratorMetadata("Angle",GenerationStrategy.Random,45),

                                                         new VariableGeneratorMetadata("minX1",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("maxX1",GenerationStrategy.Random,40,60),
                                                         new VariableGeneratorMetadata("minY1",GenerationStrategy.Random,10,20),
                                                         new VariableGeneratorMetadata("maxY1",GenerationStrategy.Random,40,60),


                                                         new VariableGeneratorMetadata("minX2",GenerationStrategy.Random,0,10),
                                                         new VariableGeneratorMetadata("maxX2",GenerationStrategy.Random,30,50),
                                                         new VariableGeneratorMetadata("minY2",GenerationStrategy.Random,0,10),
                                                         new VariableGeneratorMetadata("maxY2",GenerationStrategy.Random,30,50),
                                                     });
        }

    }
}
