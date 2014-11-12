//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UserInterface.Presenters
{
    public class OrderPresenter : DependencyObject
    {

        #region Backing fields

        private AbstractGameEngine.Action action;//на будущее - нужен для Update(), которую вызывает MapPresenter.cs (и, может, не только для этого)

        Geometry.Figures.Point origin;//точка текущего положения отряда

        Geometry.Figures.Point target;//точка цели

        #endregion

        #region Constructors
        
        /// <summary>
        /// Конструктор текущего действия (движения)
        /// </summary>
        /// <param name="a">Действие</param>
        /// <param name="curPoint">Текущее положение отряда</param>
        public OrderPresenter(AbstractGameEngine.Action a, Geometry.Figures.Point curPoint)
        {
            double ratio = Transformer.Ratio;
            this.origin = curPoint;
            this.target = (a as GameEngine.Characters.MoveAction).Destination;
            OrderOrigin = new Point(curPoint.X * ratio, curPoint.Y * ratio);
            OrderTarget = new Point(target.X * ratio, target.Y * ratio);
            this.OrderType = 0;
            
            VisibleWayAim = Visibility.Visible;
            this.TurnsToComplete = (a as GameEngine.Characters.MoveAction).TurnsToComplete;

            double dX=0;//смещение значка цели перемещения относительно положения отряда по оси X
            double dY=0;//смещение значка цели перемещения относительно положения отряда по оси Y

            if ((target.X - origin.X) < 0) dX = 2 * (target.X - origin.X) * ratio;//если отряд двигается влево
            else dX = (target.X - origin.X) * ratio - 12.5;//если отряд двигается вправо
            if ((origin.Y - target.Y) < 0) dY = 2 * (origin.Y - target.Y) * ratio;//если отряд двигается вверх
            else dY = (origin.Y - target.Y) * ratio - 12.5;//если отряд двигается вниз            
            this.Margin = new Thickness(dX, dY, 0, 0);

            DeltaHorisontal = (target.X - origin.X) * ratio;
            DeltaVertical = (origin.Y - target.Y) * ratio;

            this.WayLine = new System.Windows.Media.PointCollection();
            Point aimPos = new Point(dX, dY);//получение точки цели перемещения отряда
            this.WayLine.Add(new Point(0, 0));//точка нахождения отряда относительно отряда
            this.WayLine.Add(new Point((target.X - origin.X) * ratio, (origin.Y - target.Y) * ratio));
        }

        #endregion


        #region Dependency properties

        #region OrderOrigin

        /// <summary>
        /// Order origin -- where the arrow of an order will have its tail
        /// </summary>
        public System.Windows.Point OrderOrigin
        {
            get { return (System.Windows.Point)GetValue(OrderOriginProperty); }
            set { SetValue(OrderOriginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OderOrigin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderOriginProperty =
            DependencyProperty.Register("OrderOrigin", typeof(System.Windows.Point), typeof(OrderPresenter), new UIPropertyMetadata(null));

        #endregion

        #region OrderTarget

        /// <summary>
        /// Order target --  where the arrow of an order will have its point
        /// </summary>
        public System.Windows.Point OrderTarget
        {
            get { return (System.Windows.Point)GetValue(OrderTargetProperty); }
            set { SetValue(OrderTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderTargetProperty =
            DependencyProperty.Register("OrderTarget", typeof(System.Windows.Point), typeof(OrderPresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Order type is a property to show which kind of arrow to draw
        /// </summary>

        #region Order type

        public int OrderType
        {
            get { return (int)GetValue(OrderTypeProperty); }
            set { SetValue(OrderTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderTypeProperty =
            DependencyProperty.Register("OrderType", typeof(int), typeof(OrderPresenter), new UIPropertyMetadata(0));

        #endregion

        /// <summary>
        /// Показывать ли значок цели перемещения или нет
        /// </summary>
        #region VisibleWayAim

        public Visibility VisibleWayAim//возвращает bool-значение, показывать ли значок цели
        {
            get { return (Visibility)GetValue(VisibleWayAimProperty); }
            set { SetValue(VisibleWayAimProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleWayAim.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleWayAimProperty =
            DependencyProperty.Register("VisibleWayAim", typeof(Visibility), typeof(OrderPresenter), new UIPropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Margin

        /// <summary>
        /// отступ значка цели (действия) от значка отряда
        /// </summary>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Margin. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.Register("Margin", typeof(Thickness), typeof(OrderPresenter), new UIPropertyMetadata(null)); // why not New Zealand, eh?

        #endregion

        #region DeltaLeft

        /// <summary>
        /// Положение значка цели (действия) относительно значка отряда по горизонтали
        /// </summary>
        public double DeltaHorisontal
        {
            get { return (double)GetValue(DeltaHorisontalProperty); }
            set { SetValue(DeltaHorisontalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeltaHorisontal. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeltaHorisontalProperty =
            DependencyProperty.Register("DeltaHorisontal", typeof(double), typeof(OrderPresenter), new UIPropertyMetadata(null)); // why not New Zealand, eh?

        #endregion

        #region DeltaVertical

        /// <summary>
        /// Положение значка цели (действия) относительно значка отряда по вертикали
        /// </summary>
        public double DeltaVertical
        {
            get { return (double)GetValue(DeltaVerticalProperty); }
            set { SetValue(DeltaVerticalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeltaLeft. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeltaVerticalProperty =
            DependencyProperty.Register("DeltaVertical", typeof(double), typeof(OrderPresenter), new UIPropertyMetadata(null)); // why not New Zealand, eh?

        #endregion

        #region WayLine

        /// <summary>
        /// Координаты линии, соединяющей отряд и его цель перемещения
        /// </summary>
        public System.Windows.Media.PointCollection WayLine
        {
            get { return (System.Windows.Media.PointCollection)GetValue(WayLineProperty); }
            set { SetValue(WayLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WayLine. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WayLineProperty =
            DependencyProperty.Register("WayLine", typeof(System.Windows.Media.PointCollection), typeof(OrderPresenter), new UIPropertyMetadata(null)); // why not New Zealand, eh?

        #endregion

        /// <summary>
        /// Показывает, за сколько ходов отряд достигнет цели перемещения
        /// </summary>
        #region TurnsToComplete

        public Int32 TurnsToComplete//возвращает количество ходов до конечной цели движения
        {
            get { return (Int32)GetValue(TurnsToCompleteProperty); }
            set { SetValue(TurnsToCompleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TurnsToComplete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TurnsToCompleteProperty =
            DependencyProperty.Register("TurnsToComplete", typeof(Int32), typeof(OrderPresenter), new UIPropertyMetadata(null));

        #endregion

        #endregion
        

        internal void Update()//на будущее - для MapPresenter.cs
        {
            Geometry.Figures.Point p0;
            if (origin != null)
            {
                p0 = Transformer.ConvertToScreen(origin);
                OrderOrigin = new System.Windows.Point(p0.X, p0.Y);
            }

            Geometry.Figures.Point p1;
            if (origin != null)
            {
                p1 = Transformer.ConvertToScreen(target);
                OrderTarget = new System.Windows.Point(target.X, target.Y);
            }

            if (action is GameEngine.Characters.MoveAction)
            {
                if (action.Completed == true)
                {
                    VisibleWayAim = Visibility.Collapsed;
                    TurnsToComplete = 0;
                }
                else
                {
                    Margin=new Thickness(((action as GameEngine.Characters.MoveAction).Destination.X-(action as GameEngine.Characters.MoveAction).Origin.X)*100,
                        ((action as GameEngine.Characters.MoveAction).Destination.Y-(action as GameEngine.Characters.MoveAction).Origin.Y)*100,
                        0, 0);

                    VisibleWayAim = Visibility.Visible;
                    TurnsToComplete = (action as GameEngine.Characters.MoveAction).TurnsToComplete;
                }
            }
        }
    }
}
