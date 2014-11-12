using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawer.Transformers
{
    public class ChainTransformer : ITransformer
    {
        private List<ITransformer> chain;

        public void ToModelCoords(double x, double y, out double a, out double b)
        {
            if (chain != null && chain.Count > 0)
            {
                double c = x, d = y;
                foreach (var transformer in chain)
                {
                    transformer.ToModelCoords(c, d, out a, out b);
                    c = a; d = b;
                }
                a = c; b = d;
            }
            else
            {
                EmptyTransformer.ToModelCoords(x, y, out a, out b);
            }
        }

        public void ToScreenCoords(double a, double b, out double x, out double y)
        {
            if (chain != null && chain.Count > 0)
            {
                double c = a, d = b;
                foreach (var transformer in chain)
                {
                    transformer.ToScreenCoords(c, d, out x, out y);
                    c = x; d = y;
                }
                x = c; y = d;
            }
            else
            {
                EmptyTransformer.ToScreenCoords(a, b, out x, out y);
            }

        }

        public ChainTransformer(params ITransformer[] tr)
        {
            this.chain = new List<ITransformer>();
            foreach (var transformer in tr)
            {
                if (transformer != null) this.chain.Add(transformer);
            }
        }
    }
}
