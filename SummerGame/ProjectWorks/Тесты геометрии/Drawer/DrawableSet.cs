using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer.Transformers;

namespace Drawer
{

    /// <summary>
    /// An convenience class handy to quickly batch lists of drawable entities into groups.
    /// Each group is assigned a name and a color (the latter not reqiuired).
    /// Best used when a list of entities is as long as a list of names.
    /// If a transformer is given, it is set as a transformer for the port itself.
    /// </summary>
    public class DrawableSet : IDrawable
    {

        #region Fields

        private List<IDrawable> list;

        private List<string> colors;

        private List<string> names;

        private ITransformer transformer;

        #endregion

        #region CircularIndex

        public class CircularIndex
        {
            int count, index;
            void check(int i)
            {
                if (i < 0) index = i % count == 0 ? 0 : count - (i % count);
                else if (i >= count) index = i % count;
                else index = i;
            }
            public int Current
            {
                get { return index; }
                set { check(value); }
            }
            public CircularIndex(int count)
            {
                if (count <= 0)
                    throw new Exception("CircularIndex. Cannot initialise, count given is negative or zero.");
                this.count = count; index = 0;
            }
            public int Next() { check(index + 1); return index; }
        }

        #endregion

        #region Constructors

        public DrawableSet(List<IDrawable> list, List<string> names = null, List<string> colors = null, ITransformer tr = null)
        {
            this.list = list; this.colors = colors; this.names = names; this.transformer = tr;
        }

        public DrawableSet(ITransformer tr = null, List<string> names = null, List<string> colors = null, params IDrawable[] list)
        {
            this.list = list.ToList(); this.colors = colors; this.names = names; this.transformer = tr;
        }

        #endregion

        public void PopulatePort(Port port, string prefix)
        {
            bool gotColors = colors != null && colors.Count > 0;
            CircularIndex i = null;
            if (gotColors) i = new CircularIndex(colors.Count);

            int k = (names == null) ? 0 : names.Count;
            int l = (list == null) ? 0 : list.Count;
            int m = Math.Min(k, l);

            int j = 0;
            while (j < m) // named ones
            {
                list[j].PopulatePort(port, prefix + names[j]);
                if (gotColors)
                {
                    port[prefix + names[j]].Settings.Color = colors[i.Current];
                    i.Next();
                }
                j++;
            }

            string elsename = prefix + "All else";
            while (j < l) // and all others
            {
                list[j].PopulatePort(port, elsename);
                if (gotColors) port[elsename].Settings.Color = colors[i.Current];
                j++;
            }

            if (transformer != null)
            {
                port.Settings.Transformer = transformer;
            }
        }

    }
}
