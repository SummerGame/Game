using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawer
{

    public interface IDrawable
    {
        void PopulatePort(Port port, string prefix);
    }

}
