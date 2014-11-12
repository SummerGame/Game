using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel; //

namespace MyTestEnvironment
{
    public class AutoGeneratorSettings : INotifyPropertyChanged //todo remove interface?
    {
        private List<VariableGeneratorMetadata> data;

        public List<VariableGeneratorMetadata> Data { get { return data; } }

        public VariableGeneratorMetadata this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public int TotalCycles
        {
            get
            {
                var i = 1;
                foreach (var item in data)
                {
                    i *= item.Cycles;
                }
                if (PropertyChanged != null) // todo what the fuck?!?
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalCycles"));
                return i;
            }
        }

        public VariablesSet GenerateVariables(int i)
        {
            var result = new List<Variable>();

            for (int k = data.Count - 1; k >= 0; k--)
            {
                int n = i % data[k].Cycles;
                result.Add(data[k].GenerateVariable(n));
                i = i / data[k].Cycles;
            }

            return new VariablesSet(result);
        }

        public AutoGeneratorSettings(List<VariableGeneratorMetadata> data)
        {
            this.data = data;
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
