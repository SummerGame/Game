using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer;

namespace MyTestEnvironment
{
    public class Variable
    {

        string name;

        double value;

        public string Name { get { return name; } }

        public double Value { get { return value; } }

        public Variable(string name, double value)
        {
            this.name = name;
            this.value = value;
        }

    }

    public class VariablesSet : IDrawable
    {
        List<Variable> data;

        public List<Variable> Variables
        {
            get { return data; }
        }

        public Variable this[string nm]
        {
            get
            {
                Variable result = null;
                var itms = data.Select(x => x).Where(x => x.Name == nm).ToList();
                if (itms.Count == 1)
                {
                    result = itms[0];
                }
                else if (itms.Count == 0)
                {
                    throw new Exception("Variable \'" + nm + "\' coud not be found in Variables object");
                }
                else
                {
                    throw new Exception("Variable\'" + nm + "\' found more than once in Variables object");
                }
                return result;
            }
        }

        public VariablesSet(List<Variable> data)
        {
            this.data = data;
        }

        public void PopulatePort(Port port, string prefix)
        {
            List<string> str = new List<String>();
            foreach (var item in data)
            {
                str.Add(item.Name + " = \t" + item.Value);
            }
            var it = DrawableElement.Text(5, 5, str.ToArray());
            port[prefix + ".Variables"].Populate(it);
        }
    }
}
