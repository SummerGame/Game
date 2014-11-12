using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawer.Transformers;
using System.ComponentModel;

namespace Drawer
{

    public class Port : INotifyPropertyChanged
    {

        #region Fields

        private List<Chart> charts;

        private VisualSettings settings;

        #endregion

        #region Properties

        // An iterator returns charts by name for their population
        // If a name doesn't exist, creates a new chart
        public Chart this[string name]
        {
            get
            {
                var chartsfound = charts.Where(x => x.Settings.Name == name).ToList();
                Chart chartfound;
                if (chartsfound.Count == 0)
                {
                    chartfound = new Chart(name,"Undefined");
                    chartfound.Settings.ParentSettings = Settings;
                    charts.Add(chartfound);
                }
                else
                {
                    chartfound = chartsfound[0];
                }
                return chartfound;
            }
        }


        // Don't use this property to set charts of a port - that will be useless
        // Use iterator instead
        public List<Chart> Charts
        {
            get { return new List<Chart>(charts); }
        }

        public VisualSettings Settings { get { return settings; } }

        public List<ChartSettings> ChartSettings
        {
            get
            {
                List<ChartSettings> result = new List<ChartSettings>(); 
                foreach (var chart in charts)
                    result.Add(chart.Settings);
                return result;
            }
        }

        #endregion

        #region Constructors

        public Port(VisualSettings settings)
        {
            charts = new List<Chart>();
            this.settings = settings;
        }

        #endregion

        public void Update()
        {
            foreach (var chart in charts)
            {
                chart.Update();
            }

            if (PropertyChanged != null)
            { PropertyChanged(this, new PropertyChangedEventArgs("Charts")); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
