using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawer.Transformers
{
    public class ScaleTransformer : ITransformer
    {
        private double xRatio = 1, yRatio = 1; // how many model coordinates units in a screen pixel


        public void ToModelCoords(double x, double y, out double a, out double b)
        {
            a = x * xRatio; b = y * yRatio;
        }


        public void ToScreenCoords(double a, double b, out double x, out double y)
        {
            x = a / xRatio; y = b / yRatio;
        }


        public ScaleTransformer(double xRatio, double yRatio)
        {
            this.xRatio = xRatio; this.yRatio = yRatio;
        }
    }
}
