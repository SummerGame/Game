using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using GameEngine;
using GameEngine.Characters;
using GameEngine.Lands;
using GameEngine.StoryTelling;

namespace UserInterface
{
    public static class UISelection
    {
        #region Fields

        private static List<List<FrameworkElement>> all_figures = new List<List<FrameworkElement>>();
        // список добавленных фигур, которые мы должны отрисовать

        private static List<FrameworkElement> adds_list = new List<FrameworkElement>();
        // дополнительный контейнер. Служит для создания нового списка фигур

        private static List<Point> clicked_points = new List<Point>();
        // храним информацию о том, куда должны идти фигуры

        private static List<FrameworkElement> selected_items = new List<FrameworkElement>();
        // отсылаем этот контейнер Алмазу чтобы он выделил фигуры из этого контейнера(увеличил ширину);

        #endregion

        static UISelection()
        {
        }

        #region Properties

        public static List<List<FrameworkElement>> AllFigures
        {
            get { return UISelection.all_figures; }
            //set { UISelection.all_figures = value; }
        }


        public static List<FrameworkElement> SelectedItems
        {
            get { return UISelection.selected_items; }
            //set { UISelection.selected_items = value; }
        }

        public static List<FrameworkElement> AddsList
        {
            get { return UISelection.adds_list; }
            set { UISelection.adds_list = value; }
        }

        public static List<Point> ClickedPoints
        {
            get { return UISelection.clicked_points; }
            //set { UISelection.all_figures = value; }
        }

        #endregion

        #region Methods

        // выделение фигуры и отмена выделений
        public static void ToggleSelection(object sender, MouseButtonEventArgs e)
        {
            bool flag = true;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                FrameworkElement fig = sender as FrameworkElement;
                if ((adds_list.Count == 0) && (selected_items.Count == 0))
                // если у нас нет выделенных фигур и фигур в контейнере,то: 
                {
                    selected_items.Add(fig);
                    adds_list.Add(fig);
                    flag = false;
                }
                else // если у нас есть выделенные фигуры,то:
                {
                    if (adds_list.Count == 0)
                    {
                        int len = all_figures.Count - 1;
                        if (all_figures[len].Contains(fig)) // && (fig.StrokeThickness == 3)
                        {
                            if (all_figures[len].Count == 1)
                            {
                                int x = all_figures[len].IndexOf(fig);
                            }
                            else
                            {
                                foreach (List<FrameworkElement> s in all_figures)
                                {
                                    if (s.Contains(fig)) s.Remove(fig);
                                }
                                List<FrameworkElement> sh = new List<FrameworkElement>();
                                // удалил фигуру уже "посланную" куда-то? Добавляем её в начало списка!
                                sh.Add(fig);
                                all_figures.Insert(0, sh);
                                Point p = clicked_points[len];
                                clicked_points.Insert(0, p);
                            }
                            flag = false;
                            selected_items.Remove(fig);
                        }
                    }
                    else //+
                    {
                        if ((adds_list.Contains(fig)))
                        // без fig.StrokeThickness == 3 попробовать && (fig.StrokeThickness == 3)
                        {
                            if (adds_list.Count == 1) // если элемент один, то очищаем весь список
                            {
                                adds_list = new List<FrameworkElement>();

                            }
                            else // если элемент из группы, то попросту удаляем его оттуда
                            {
                                adds_list.Remove(fig);
                                // правильно, поскольку мы его никуда не послали и у него нет координат куда идти

                            }
                            selected_items.Remove(fig);
                            flag = false;
                        }
                    }

                    if (flag == true)
                    // если мы ничего не удалили, значит мы хотим добавить новую фигуру в наш контейнер
                    {
                        if (adds_list.Count != 0)
                        {
                            flag = false;
                            adds_list.Add(fig);
                        }
                        else
                        {
                            List<FrameworkElement> x = all_figures[all_figures.Count - 1];
                            for (int j = 0; j < x.Count; j++) adds_list.Add(x[j]); //Figures.adds_list = x; -- ?
                            adds_list.Add(fig);
                            flag = false;
                        }
                        selected_items.Add(fig);
                    }
                }
            }

        }

        
       

        #endregion

    }
}
