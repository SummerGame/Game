using System.Windows;
using System.Collections.Generic;
using GameEngine;
using System.Linq;
using Geometry;

namespace UserInterface.Presenters
{
    public class MapPresenter : DependencyObject
    {

        #region Backing fields

        private GameMap map;
        private List<FigurePresenter> figures;

        #endregion

        #region Dependecy properties

        #region Width

        /// <summary>
        /// Ширина карты
        /// </summary>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Width. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region Height

        /// <summary>
        /// Высота карты
        /// </summary>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Width. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region MarginLeft

        /// <summary>
        /// Ширина карты
        /// </summary>
        public double MarginLeft
        {
            get { return (double)GetValue(MarginLeftProperty); }
            set { SetValue(MarginLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarginLeft. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginLeftProperty =
            DependencyProperty.Register("MarginLeft", typeof(double), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region MarginTop

        /// <summary>
        /// Ширина карты
        /// </summary>
        public double MarginTop
        {
            get { return (double)GetValue(MarginTopProperty); }
            set { SetValue(MarginTopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarginTop. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginTopProperty =
            DependencyProperty.Register("MarginTop", typeof(double), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region Background

        public string Background
        {
            get { return (string)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(string), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion//??

        #region Visible units

        public List<UnitPresenter> VisibleUnits
        {
            get { return (List<UnitPresenter>)GetValue(VisibleUnitsProperty); }
            set { SetValue(VisibleUnitsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleUnits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleUnitsProperty =
            DependencyProperty.Register("VisibleUnits", typeof(List<UnitPresenter>), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region Landscape

        public List<LandscapePresenter> Landscape
        {
            get { return (List<LandscapePresenter>)GetValue(LandscapeProperty); }
            set { SetValue(LandscapeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Landscape.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LandscapeProperty =
            DependencyProperty.Register("Landscape", typeof(List<LandscapePresenter>), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region Visible items

        public List<GameItemsPresenter> VisibleItems
        {
            get { return (List<GameItemsPresenter>)GetValue(VisibleItemsProperty); }
            set { SetValue(VisibleItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleItemsProperty =
            DependencyProperty.Register("VisibleItems", typeof(List<GameItemsPresenter>), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #region Visible orders

        public List<OrderPresenter> VisibleOrders
        {
            get { return (List<OrderPresenter>)GetValue(VisibleOrdersProperty); }
            set { SetValue(VisibleOrdersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleOrders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleOrdersProperty =
            DependencyProperty.Register("VisibleOrders", typeof(List<OrderPresenter>), typeof(MapPresenter), new UIPropertyMetadata(null));

        #endregion

        #endregion

        #region Constructors

        public MapPresenter(GameMap map)
        {
            this.map = map;
            //Background = map.Background;
            figures = new List<FigurePresenter>();
        }

        public MapPresenter()
        {
            figures = new List<FigurePresenter>();
            map = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// reload all lands, units and items from the game map, used when game map itself was reloaded
        /// </summary>
        /// <param name="newMap"></param>
        internal void Update(GameMap newMap)
        {
            map = newMap;
            Width = map.Width;
            Height = map.Height;
            Background = map.Background;
            Landscape = map.Lands.Select(x => new LandscapePresenter(x)).ToList();
            UpdateVisibility();
        }

        /// <summary>
        /// Перезагрузка видимых отрядов и предметов из игровой карты - обычно используется в конце хода
        /// </summary>
        internal void UpdateVisibility()
        {
            if (map != null)
            {
                VisibleUnits = map.Units.Select(x => new UnitPresenter(x)).ToList(); // todo переделать на получение списка ВИДИМЫХ юнитов
                VisibleItems = map.Items.Select(x => new GameItemsPresenter(x)).ToList(); // todo переделать на получение списка ВИДИМЫХ предметов
                VisibleOrders = null; // todo переделать на список известных игре приказов
                //на будущее -//VisibleOrders=map.MoveActions.Select(x => new OrderPresenter(x)).ToList();// отбразить список всех действий движения на карте
            }
            Update();
        }


        /// <summary>
        // just update the view of all visible entities - usually because of zoom, changing size et al
        /// </summary>
        internal void Update()
        {
            if (map != null)
            {
                Width = map.Width * Transformer.Ratio/12.5;
                Height = map.Height * Transformer.Ratio / 12.5;

                MarginLeft = Transformer.TranslationX;
                MarginTop = Transformer.TranslationY;

                if (Landscape != null) foreach (var land in Landscape) land.Update();
                if (VisibleUnits != null) foreach (var unit in VisibleUnits) unit.Update();
                if (VisibleItems != null) foreach (var item in VisibleItems) item.Update();
                if (VisibleOrders != null) foreach (var order in VisibleOrders) order.Update();//на будущее - обновить список всех приказов
            }
        }

        public List<FigurePresenter> Figures
        {
            get { return figures; }
            set { figures = value; }
        }


        #endregion

    }
}
