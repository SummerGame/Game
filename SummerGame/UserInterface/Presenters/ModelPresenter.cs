using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UserInterface.Presenters
{
    public enum ModelType
    {
        Cube,
        Arc,
        Bullet,
        TankShell1,
        Rocket,
        KI_27,
        GAZ_AA,
        T_37,
        SU_12_1,
        BA6,
        Barrel,
        gun122mm
    }

    /// <summary>
    /// Класс 3Dмодели
    /// </summary>
    public class ModelPresenter : DependencyObject
    {
        public ModelType Type { get; set; }

        #region Angle

        /// <summary>
        /// Угол поворота модели
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Personnel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(ModelPresenter), new UIPropertyMetadata(null));

        #endregion

        public ModelPresenter(ModelType type, double angle)
        {
            Type = type;
            Angle = angle;
        }

        public ModelPresenter()
        {
        }

    }
}
