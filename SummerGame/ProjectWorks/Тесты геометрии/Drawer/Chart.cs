using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Drawer.Transformers;

namespace Drawer
{
    public class ChartSettings : VisualSettings, INotifyPropertyChanged
    {
        private bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChartSettings(string name, string color = "Undefined", ITransformer transformer = null) :
            base(name, color, transformer)
        {

        }
    }

    public class Chart
    {

        #region Properties

        public ChartSettings Settings { get; set; }

        public List<DrawableElement> Elements { get; set; }

        #endregion

        #region Constructors

        public Chart(string name, string color)
        {
            Settings = new ChartSettings(name, color);
        }

        public Chart(string name, string color, ITransformer transform)
        {
            Settings = new ChartSettings(name, color, transform);
        }

        #endregion

        public void Populate(params DrawableElement[] elements)
        {
            if (Elements == null) Elements = elements.ToList();
            else Elements.AddRange(elements.ToList());

            foreach (var element in Elements)
            {
                element.Settings.ParentSettings = Settings;
            }
        }

        public void Populate(List<DrawableElement> elements)
        {
            if (Elements == null) Elements = elements;
            else Elements.AddRange(elements);

            foreach (var element in Elements)
            {
                element.Settings.ParentSettings = Settings;
            }
        }

        public void Update()
        {
            if (Elements != null)
            {
                foreach (var element in Elements)
                {
                    element.Update();
                }
            }
        }

    }
}
