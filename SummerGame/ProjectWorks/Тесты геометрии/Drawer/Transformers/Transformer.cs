using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drawer.Transformers
{

    public enum MoveTo { Left, Rigth, Up, Down };


    public class Transformer : ITransformer
    {

        #region Fields

        private double ratio;
        // коэффициент увеличения / уменьшения для пересчета
        // экранных координат в модельные
        // т.е. сколько единиц модельной размерности в одном пикселе

        private double centerx, centery;
        // центр модельных координат - модельные координаты точки,
        // которая должна отображаться в центре экрана

        private int screencenterx, screencentery;
        // центр экранных координат

        private int screenwidth, screenheight;

        private bool _invertY;
        // признак - требуется ли инверсия координаты Y
        // (поскольку на экране эта координата растет вниз)

        public bool RatioPreferenceSet; //todo remove bullshit
        // флаг для использования программистом.
        // устанавливай его если хочешь сам себе послать сигнал
        // что нужное увеличение уже установлено и менять его не надо



        #endregion

        #region Properties

        // added
        public double Centerx
        {
            get { return centerx; }
            set { centerx = value; }
        }

        public double Centery
        {
            get { return centery; }
            set { centery = value; }
        }
        // added endregion

        public int ScreenHeight
        {
            get { return screenheight; }
            set { screenheight = value; } // added
        }

        public int ScreenWidth
        {
            get { return screenwidth; }
            set { screenwidth = value; } // added
        }
        // ширина и высота экрана

        public double Ratio
        {
            get { return ratio; }
            set { if (value <= 0) ratio = 1; else ratio = value; }
        }

        public int ScreenCenterX
        {
            get { return screencenterx; }
            set { screencenterx = value; }
        }

        public int ScreenCenterY
        {
            get { return screencentery; }
            set { screencentery = value; }
        }

        public bool InvertY
        {
            get { return _invertY; }
            set { _invertY = value; }
        }
        // Внимание! Это свойство устанавливать ДО
        // инициализации координат !!!

        #endregion

        #region Constructors

        public Transformer()
        { ratio = 1; screenwidth = 0; screenheight = 0; _invertY = false; RatioPreferenceSet = false; }

        //Transformer (const TRect& canvas,
        //              double minx, double maxx, double miny, double maxy);
        // TODO: Check this

        // конструкторы
        //---------------------------------------------------------------------------



        //---------------------------------------------------------------------------


        //void     Init (const TRect& canvas,
        //               double minx, double maxx, double miny, double maxy);
        //TODO: Check this

        public void Init(double minx, double maxx, double miny, double maxy)// инициализаторы, могут использоваться после пустого конструктора
        {
            // Запускать только при смене модели!
            int w = ScreenWidth;
            int h = ScreenHeight;
            try
            {
                centerx = (maxx + minx) / 2;
                centery = (maxy + miny) / 2;
                double w1 = (maxx - minx) / w;
                double h1 = (maxy - miny) / h;
                ratio = Math.Max(w1, h1);
            }
            catch (Exception) { ratio = 1; };
        }

        #endregion

        #region Methods

        #region To Screen Coordinates
        // разновидности обращения модельных координат в экранные

        public void ToScreenCoords(double a, double b, out double x, out double y)
        {
            x = (screencenterx + (a - centerx) / ratio);
            if (_invertY) y = (int)(screencentery + (centery - b) / ratio);
            else y = (screencentery + (b - centery) / ratio);
        }

        public void ToScreenCoords(int a, int b, out double x, out double y)
        {
            ToScreenCoords((double)a, (double)b, out x, out y);
        }

        #endregion

        #region To Model Coorinates
        // разновидности обращения экранных координат в модельные

        public void ToModelCoords(double x, double y, out double a, out double b)
        {
            a = ((x - screencenterx) * ratio) + centerx;
            if (_invertY) b = centery - ((y - screencentery) * ratio);
            else b = centery + ((y - screencentery) * ratio);
        }

        #endregion


        // процедура перерасчета увеличения при изменении размеров порта?
        public void UpdateSize(int newwidth, int newheight)
        {
            if ((ScreenWidth != 0) && (ScreenHeight != 0))
            {
                double rx = (double)newwidth / ScreenWidth;
                double ry = (double)newheight / ScreenHeight;
                rx = Math.Min(rx, ry);
                ratio *= rx;
            }
            double screenwidth = newwidth, screenheight = newheight;
        }


        public void Move(MoveTo side, double num = 1)
        {
            switch (side)
            {
                case MoveTo.Left: centerx -= (num * ratio); break;
                case MoveTo.Rigth: centerx += (num * ratio); break;
                case MoveTo.Up: centery += (num * ratio); break;
                case MoveTo.Down: centery -= (num * ratio); break;
                default: break;
            }
        }

        #endregion

    }
}




