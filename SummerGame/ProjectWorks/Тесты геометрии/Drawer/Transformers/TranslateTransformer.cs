using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawer.Transformers
{
    public class TranslateTransformer : ITransformer
    {
        private double dx = 0, dy = 0; // move screen coords from their model original

        public void ToModelCoords(double x, double y, out double a, out double b)
        {
            a = x - dx; b = y - dy;
        }

        public void ToScreenCoords(double a, double b, out double x, out double y)
        {
            x = a + dx; y = b + dy;
        }

        public TranslateTransformer(double dx, double dy)
        { 
            this.dx = dx; this.dy = dy; 
        }
    }
}
