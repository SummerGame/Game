using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawer.Transformers
{
    public static class EmptyTransformer
    {
        public static void ToModelCoords(double x, double y, out double a, out double b)
        {
            a = x; b = y;
        }

        public static void ToScreenCoords(double a, double b, out double x, out double y)
        {
            x = a; y = b;
        }
    }
}
