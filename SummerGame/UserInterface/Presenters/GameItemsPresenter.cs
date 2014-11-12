using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AbstractGameEngine;

namespace UserInterface.Presenters
{
    public class GameItemsPresenter : DependencyObject
    {
        private Item original;

        public GameItemsPresenter(AbstractGameEngine.Item x)
        {
            this.original = x;
        }

        #region Dependency properties

        #region Something

        #endregion

        #endregion

        internal void Update()
        {
            throw new NotImplementedException();
        }
    }
}
