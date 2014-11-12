namespace Geometry.Figures
{
    /// <summary>
    /// Класс Круг
    /// </summary>
    public class Circle : Figure
    {
        #region Свойства

        public Point Center { get; set; }

        public double Radius { get; set; }

        #endregion

        #region Конструкторы

        public Circle(Point cent,double r)
        {
            Center = cent;
            Radius = r;
        }

        public Circle(double x,double y,double r)
        {
            Center = new Point(x, y);
            Radius = r;
        }

        public Circle(int x,int y,int r)
        {
            Center = new Point(x, y);
            Radius = r;
        }

        #endregion

        /// <summary>
        /// Преобразовывает координаты центра и радиус из типа double в int
        /// </summary>
        public Circle toInt()
        {
            return new Circle(Center.toInt(), (int) Radius);
        }

    }
}
