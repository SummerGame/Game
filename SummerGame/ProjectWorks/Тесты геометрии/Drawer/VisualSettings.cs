using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer.Transformers;

namespace Drawer
{

    public class VisualSettings
    {

        #region Fields

        string color;

        ITransformer transformer;

        #endregion

        #region Properties

        public String Name { get; set; }

        public String Color 
        {
            get
            {
                if (color == "Undefined")
                {
                    if (ParentSettings == null)
                        return "Black";
                    else return ParentSettings.Color;
                }
                else return color;
            }
            /*private*/ set
            {
                color = value;
            }
        }

        public ITransformer Transformer 
        { 
            get
            {
                if (ParentSettings == null)
                {
                    return transformer;
                }
                else
                {
                    return new ChainTransformer(ParentSettings.Transformer, transformer);
                }
            }
            /*private*/ set
            {
                transformer = value;
            }
        }

        public VisualSettings ParentSettings { get; set; }

        #endregion

        public VisualSettings(String name, String color = "Undefined", ITransformer transformer = null)
        {
            this.Name = name;
            this.Color = color;
            this.Transformer = transformer;
        }

    }

}
