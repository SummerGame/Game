using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GameEngine.Lands;
using Geometry;

namespace UserInterface.Presenters
{
    public class LandscapePresenter : DependencyObject
    {
        #region Backing fields

        private Landscape original;

        #endregion

        #region Dependecy properties

        #region Figures

        public List<FigurePresenter> Figures
        {
            get { return (List<FigurePresenter>)GetValue(FiguresProperty); }
            set { SetValue(FiguresProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Figures.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiguresProperty =
            DependencyProperty.Register("Figures", typeof(List<FigurePresenter>), typeof(LandscapePresenter), new UIPropertyMetadata(null));


        #endregion

        #endregion

        public LandscapePresenter(Landscape landscape)
        {
            this.original = landscape;
            if (landscape != null)
                Figures = landscape.Figures.Select(x => new FigurePresenter(x, landscape)).ToList();
            Update();
        }

        internal void Update()
        {
            if (Figures != null) foreach (var f in Figures) f.Update();
        }
    }
}
