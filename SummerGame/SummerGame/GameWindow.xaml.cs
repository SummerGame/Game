using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using GameEngine;
using UserInterface;
using GameEngine.Characters;
using UserInterface.Presenters;
using SummerGame.Resources;


namespace SummerGame
{
    /// <summary>
    /// Класс, содержащий методы для событий, происходящих в игровом окне
    /// </summary>
    /// 

    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            MainWindow.GameWindow = this;

            UI.GameReloaded += ((snd, e) => { this.DataContext = snd as UserInterface.Presenters.GamePresenter; });
            UI.TheMapControl = TheMap; // WHAT

            Scale();
        }

        /// <summary>
        /// Открывает окно главного меню
        /// </summary>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.GameWindow.IsActive) return;
            if (MainWindow.MainMenuWindow.Owner == MainWindow.GameWindow)
            {
                MainWindow.MainMenuWindow.Owner = null;
                MainWindow.MainMenuWindow.Hide();
            }
            else
            {
                MainWindow.MainMenuWindow.Owner = MainWindow.GameWindow;
                MainWindow.MainMenuWindow.Show();
            }
        }

        /// <summary>
        /// Открывает окно настроек
        /// </summary>
        private void Options_Click(object sender, RoutedEventArgs e)
        {
            
            if (!MainWindow.GameWindow.IsActive) return; //?
            if (MainWindow.OptionsWindow == null) MainWindow.OptionsWindow = new OptionsWindow();
            MainWindow.OptionsWindow.Show();
        }

        /// <summary>
        /// Обработка нажатия клавиш 
        /// </summary>
        private void KeyPressedHandler(object sender, KeyEventArgs e) 
        {
            UnselectAll();
            if (e.Key == Key.Space)
                UI.TogglePause();
            else if (e.Key == Key.Up)//нажата клавиша "стрелка вверх"
            {
                UI.Up();
            }
            else if (e.Key == Key.Down)//нажата клавиша "стрелка вниз"
            {
                UI.Down();
            }
            else if (e.Key == Key.Left)//нажата клавиша "стрелка влево"
            {
                UI.Left();
            }
            else if (e.Key == Key.Right)//нажата клавиша "стрелка вправо"
            {
                UI.Right();
            }
            else if (e.Key == Key.Subtract)//нажата клавиша "минус"
            {
                UI.ZoomOut();
                Scale();
            }
            else if (e.Key == Key.Add)//нажата клавиша "плюс"
            {
                UI.ZoomIn();
                Scale();
            }
        }

        /// <summary>
        /// Обработка нажатия мыши
        /// </summary>
        public void MouseClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (!MainWindow.GameWindow.IsActive) return;
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed) //левый клик по полю - "посыл" в эту точку фигуры
                {
                    if (e.OriginalSource is FrameworkElement)// (!(e.OriginalSource is Viewport3D))
                    {
                        var tag = (e.OriginalSource as FrameworkElement).Tag;
                        if (tag != null && tag is UnitPresenter)
                        {
                            if ((tag as UnitPresenter).IsMyUnit) //если юнит свой, то
                            {
                                SelectAndDeselect(e);
                            }
                            else
                            {
                                UI.Attack(e.OriginalSource as FrameworkElement);
                            }
                        }
                        else
                        {
                            Point pos = Mouse.GetPosition(sender as ContentControl);
                            UI.Movement(pos);
                        }
                    }
                }

                else if (e.RightButton == MouseButtonState.Pressed) 
                {
                    // правый клик по юниту - окно свойств юнита
                    if ((e.OriginalSource as FrameworkElement).Tag is UnitPresenter)
                    {
                        if (MainWindow.PropertiesWindow == null) MainWindow.PropertiesWindow = new ObjectProperties();
                        MainWindow.PropertiesWindow.TheUnit = (e.OriginalSource as FrameworkElement).Tag as UnitPresenter;
                        MainWindow.PropertiesWindow.Owner = this;
                        MainWindow.PropertiesWindow.Show();
                    }
                    // правый клик по полю - отмена выделения всех фигур
                    else if ((e.OriginalSource as FrameworkElement).Name == "FakeFieldRectangle")
                    {
                        UnselectAll();
                        UISelection.SelectedItems.Clear();
                        UISelection.AddsList = new List<FrameworkElement>();
                    }
                }
            }
        }

        /// <summary>
        ///  отмена выделений всех фигур
        /// </summary>
        private void UnselectAll() 
        {
            for (int i = 0; i < UISelection.AllFigures.Count; i++)
            {
                foreach (FrameworkElement s in UISelection.AllFigures[i])
                {
                    if (s is Shape)
                    {
                        (s as Shape).StrokeThickness = 0;
                    }
                }
            }

            foreach (FrameworkElement s in UISelection.SelectedItems)
            {
                if (s is Shape)
                {
                    (s as Shape).StrokeThickness = 0;
                }
            }
            UISelection.AllFigures.Clear(); // TODO: remove??
            UISelection.SelectedItems.Clear();

        }

        /// <summary>
        /// выделение фигуры и отмена выделений
        /// </summary>
        /// <param name="e"></param>
        private void SelectAndDeselect(MouseButtonEventArgs e) 
        {
            if (!MainWindow.GameWindow.IsActive) return;
            UISelection.ToggleSelection(e.OriginalSource, e);
            SolidColorBrush stroke_color = new SolidColorBrush();
            stroke_color.Color = Colors.Green;
            // выделение для контейнера временных фигур;
            int len = UISelection.AllFigures.Count;
            if (UISelection.SelectedItems.Count != 0)
            {
                foreach (FrameworkElement s in UISelection.SelectedItems)
                {
                    if (s is Shape)
                    {
                        (s as Shape).Stroke = stroke_color;
                        (s as Shape).StrokeThickness = 3;
                    }
                }
            }
            else if (len > 1)
            {
                foreach (FrameworkElement s in UISelection.AllFigures[len - 1])
                {
                    if (s is Shape)
                    {
                        (s as Shape).Stroke = stroke_color;
                        (s as Shape).StrokeThickness = 3;
                    }
                }
            }
        }

        /// <summary>
        /// Пересчёт координат при движении мыши
        /// </summary>
        private void MouseEnterHandler(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            if (!MainWindow.GameWindow.IsActive) return;
            if (e.OriginalSource is Shape)
            {
                var info = UI.GetInfo((Shape)e.OriginalSource);
                Geometry.Figures.Point MapPoint = Transformer.ConvertToModel(point.X, point.Y);
                if (info.Count > 1)
                {
                    MousePosition.Content = String.Format("X: {0}, Y: {1}", MapPoint.X, MapPoint.Y) + " " + info[0];
                }
                else
                    MousePosition.Content = String.Format("X: {0}, Y: {1}", MapPoint.X, MapPoint.Y) + " " + info[0];
            }
        }

        /// <summary>
        /// Изменяет масштаб карты при прокрутке колёсика мыши
        /// </summary>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta >0) UI.ZoomIn();
            else if (e.Delta <0) UI.ZoomOut();

            Scale();
        }

        /// <summary>
        /// Пересчёт масштаба
        /// </summary>
        private void Scale()
        {
            CenterScale.Content = (1250 / Transformer.Ratio).ToString() + "км";//получение масштаба
            RightScale.Content = (2500 / Transformer.Ratio).ToString() + "км";
        }

        /// <summary>
        /// Вызывается, когда игровое окно становится главным окном
        /// </summary>
        private void ActivatedHandler(object sender, EventArgs e)
        {
            Game.MainMap.MapChanged("");
        }

        /// <summary>
        /// Обработка нажатия паузы. Меняет игрока
        /// </summary>
        private void Side(object sender, RoutedEventArgs e)
        {
            UnselectAll();
            UI.TogglePause();
        }

        /// <summary>
        /// Обработка нажатия кнопки увеличения масштаба
        /// </summary>
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            UI.ZoomIn();
            Scale();
        }

        /// <summary>
        /// Обработка нажатия кнопки уменьшения масштаба
        /// </summary>
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            UI.ZoomOut();
            Scale();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку, закрывающую окно
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
