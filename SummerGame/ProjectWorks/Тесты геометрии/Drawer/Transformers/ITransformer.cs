using System;

namespace Drawer.Transformers
{

    public interface ITransformer
    {
        void ToModelCoords(double x, double y, out double a, out double b);
        void ToScreenCoords(double a, double b, out double x, out double y);
    }

}
