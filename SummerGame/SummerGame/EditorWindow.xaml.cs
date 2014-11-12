using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using GameEngine;
using SummerGame;
using GameEngine.Characters;
using GameEngine.Object;
using UserInterface;
using AbstractGameEngine;
using Geometry;
using Geometry.Figures;
using SummerGame.Resources;
using System.Threading;

namespace mapDrawer
{
    /// <summary>
    /// Класс, содержащий методы для событий, происходящих в окне редактора карт
    /// </summary>
    public partial class MapEditorWindow : Window
    {
        private static int max_length = 1000;
        private static int min_length = 10;
        private static int scale = 8;


        public MapEditorWindow()
        {
            InitializeComponent();
            
            updateAllowedMaps();
            reDrawMap();
        }

        /// <summary>
        /// Обновление списка доступных карт
        /// </summary>
        public void updateAllowedMaps()
        {
            var allowedMaps = getAllowedMaps();
            Maps.Items.Clear();
            foreach (string mapName in allowedMaps)
            {
                Maps.Items.Add(mapName);
            }
        }


        private GameMap map = new GameMap();
        private bool flag = false;

        /// <summary>
        /// Обрабатывает клик мыши по редактируемой карте
        /// </summary>
        private void canvasClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point pos = Mouse.GetPosition(sender as ContentControl);
            Geometry.Figures.Point point = UserInterface.Transformer.ConvertToModel(new Geometry.Figures.Point(pos.X, pos.Y));
            if (flag)
            {
                addModel(point);
            }
            else
            {
                if (selectShapes.IsChecked == true && !(e.OriginalSource is Rectangle))// выбран режим выделения
                {
                    int i = 0;
                    for (i = 0; i < map.Lands.Count; i++)      // находим индекс нашей фигуры в списке map.Lands
                        if (Geometry.Figures.Intersect.IsIntersected(point, map.Lands[i].Polygon)) break;

                    Shape shape = e.OriginalSource as Shape;
                    if (Drawer.SelectedShapes.Contains(shape)) // если фигура уже выделена, то снимаем выделение
                    {
                        shape.StrokeThickness = 0;
                        Drawer.SelectedShapes.Remove(shape);
                        Drawer.SelectedLands.Remove(map.Lands[i]);
                    }
                    else                                       // иначе выделяем её
                    {
                        Drawer.SelectedShapes.Add(shape);
                        shape.Stroke = new SolidColorBrush(Colors.Red);
                        shape.StrokeThickness = 3;

                        Drawer.SelectedLands.Add(map.Lands[i]); //!!
                    }
                }
                else
                {
                    Drawer.ClickedPoints.Add(point);
                    var radius = UserInterface.Transformer.ConvertToModelLength(6);
                    Geometry.Figures.Circle circle = new Geometry.Figures.Circle(point, radius);

                    UserInterface.Presenters.FigurePresenter FP = new UserInterface.Presenters.FigurePresenter(circle, null);
                    UI.GamePresenter.Map.Figures.Add(FP);

                    // после этих строк точки можно ставить не везде
                    Geometry.Figures.Polyline polyline = new Geometry.Figures.Polyline(Drawer.ClickedPoints);
                    Drawer.Line = new UserInterface.Presenters.FigurePresenter(polyline, null);
                    UI.GamePresenter.Map.Figures.Add(Drawer.Line);

                    reDrawMap();
                }
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Удалить все точки"
        /// Удаляет маленькие круги (обозначающие точки) с карты
        /// </summary>
        private void resetPoints(object sender, RoutedEventArgs e) 
        {
            Drawer.ClickedPoints = new List<Geometry.Figures.Point>();
            UI.GamePresenter.Map.Figures.Clear();
            reDrawMap();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Нарисовать", отрисовывает нужную местность
        /// </summary>
        private void drawArea(object sender, RoutedEventArgs e)
        {
            if (Drawer.ClickedPoints.Count == 0)
            {
                MessageBox.Show(Messages.EmptyClickedPoints);
            }
            else
                try
                {
                    List<Geometry.Figure> po = new List<Geometry.Figure>();

                    if (Drawer.LandName == "Road" || Drawer.LandName == "Water")
                    {
                        #region Check Self Intersection

                        var listSeg = Geometry.Figures.Polyline.PointsToSegments(Drawer.ClickedPoints);
                        listSeg.Remove(listSeg.Last());

                        if (Geometry.Figures.Polyline.CheckSelfIntersection(listSeg))
                        { throw new Exception(Messages.SelfIntersectionPolyline); }

                        #endregion

                        Geometry.Figures.Polyline poly = new Geometry.Figures.Polyline(Drawer.ClickedPoints);
                        po.Add(poly);
                    }
                    else
                    { 
                        ConvexPolygon CV = new ConvexPolygon();

                        //??
                        List<Geometry.Figures.Point> mypoints = new List<Geometry.Figures.Point>();
                        List<List<Geometry.Figures.Point>> mypolpoints = new List<List<Geometry.Figures.Point>>();

                        foreach (Geometry.Figures.Point p in Drawer.ClickedPoints)
                        {
                            mypoints.Add(new Geometry.Figures.Point(p.X, p.Y));
                        }

                        #region Check Self Intersection

                        var listSeg = Geometry.Figures.Polyline.PointsToSegments(mypoints);

                        if (Geometry.Figures.ConvexPolygon.CheckSelfIntersection(listSeg))
                        { throw new Exception(Messages.SelfIntersection); }

                        #endregion

                        mypolpoints = CV.ConvertToConvexList(mypoints);

                        Drawer.Line.Points.Clear();
                        List<Geometry.Figures.Point> polPoints = new List<Geometry.Figures.Point>();

                        Geometry.Figures.ConvexPolygon poly = new Geometry.Figures.ConvexPolygon(Drawer.ClickedPoints);

                        foreach (List<Geometry.Figures.Point> pointList in mypolpoints) //points
                        {
                            po.Add(new Geometry.Figures.ConvexPolygon(pointList));
                        }
                    }
                    switch (Drawer.LandName)
                    {
                        case "Sand":
                            {
                                GameEngine.Lands.Sand sand = new GameEngine.Lands.Sand(po);
                                map.Lands.Add(sand);
                                break;
                            }
                        case "Forest":
                            {
                                GameEngine.Lands.Forest forest = new GameEngine.Lands.Forest(po);
                                map.Lands.Add(forest);
                                break;
                            }
                        case "Water":
                            {
                                GameEngine.Lands.Water water = new GameEngine.Lands.Water(po);
                                map.Lands.Add(water);
                                break;
                            }
                        case "Mountain":
                            {
                                GameEngine.Lands.Mountains mountain = new GameEngine.Lands.Mountains(po);
                                map.Lands.Add(mountain);
                                break;
                            }
                        case "City":
                            {
                                GameEngine.Lands.City city = new GameEngine.Lands.City(po);
                                map.Lands.Add(city);
                                break;
                            }
                        case "LowLand":
                            {
                                GameEngine.Lands.Lowland lowLand = new GameEngine.Lands.Lowland(po);
                                map.Lands.Add(lowLand);
                                break;
                            }
                        case "Field":
                            {
                                GameEngine.Lands.Field field = new GameEngine.Lands.Field(po);
                                map.Lands.Add(field);
                                break;
                            }
                        case "Road":
                            {
                                GameEngine.Lands.Road road = new GameEngine.Lands.Road(po);
                                map.Lands.Add(road);
                                break;
                            }
                    }
                    resetPoints(null, null);
                    reDrawMap();
                    Drawer.ClickedPoints = new List<Geometry.Figures.Point>();
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    resetPoints(null, null);
                    reDrawMap();
                    Drawer.ClickedPoints = new List<Geometry.Figures.Point>();
                }
        } 

        /// <summary>
        /// Обработка нажатия кнопки "Изменить ширину и высоту". Измененяет размеры карты
        /// </summary>
        private void changeCanvas(object sender, RoutedEventArgs e)
        {
            int width = 0;
            int height = 0;
            var childrens = UserInterface.StoryConverter.FindVisualChild<ContentPresenter>(mapToDraw);
            Rectangle rectangle = (Rectangle)UI.TheMapControl.ContentTemplate.FindName("FakeFieldRectangle", childrens);

            if (canvasWidthTB != null)
            {
                try
                {
                    if (canvasWidthTB.Text == "")
                    {
                        MessageBox.Show(Messages.EmptyWidthField);

                    }
                    else if (Convert.ToInt32(canvasWidthTB.Text) > max_length)
                    {
                        rectangle.Width = scale*max_length;
                        mapToDraw.Width = scale*max_length;
                        map.Width = max_length;
                        canvasWidthTB.Text = Convert.ToString(max_length);
                    }
                    else if (Convert.ToInt32(canvasWidthTB.Text) < min_length)
                    {
                        rectangle.Width = scale*min_length;
                        mapToDraw.Width = scale*min_length;
                        map.Width = min_length;
                        canvasWidthTB.Text = Convert.ToString(min_length);
                    }
                    else
                    {
                        width = Convert.ToInt32(canvasWidthTB.Text);
                        rectangle.Width = width*scale;
                        mapToDraw.Width = width * scale;
                        map.Width = width;
                    }

                }
                catch { MessageBox.Show(Messages.IncorrectWidth); }
            }

            if (canvasHeightTB != null)
            {
                try
                {
                    if (canvasHeightTB.Text == "")
                    { MessageBox.Show(Messages.EmptyHeightField); }
                    else if (Convert.ToInt32(canvasHeightTB.Text) > max_length)
                    {
                        rectangle.Height = scale * max_length;
                        mapToDraw.Height = scale * max_length;
                        map.Height = max_length;
                        canvasHeightTB.Text = Convert.ToString(max_length);
                    }
                    else if (Convert.ToInt32(canvasHeightTB.Text) < min_length)
                    {
                        rectangle.Height = scale * min_length;
                        mapToDraw.Height = scale * min_length;
                        map.Height = min_length;
                        canvasHeightTB.Text = Convert.ToString(min_length);
                    }
                    else
                    {
                        height = Convert.ToInt32(canvasHeightTB.Text);
                        rectangle.Height = height*scale;
                        mapToDraw.Height = height * scale;
                        map.Height = height;
                    }
                }
                catch { MessageBox.Show(Messages.IncorrectHeight); }
            }
        } 

        /// <summary>
        /// Функция удаления выделенных местностей
        /// </summary>
        private void deleteSelectedShapes(object sender, RoutedEventArgs e) 
        {
            for (int i = 0; i < Drawer.SelectedLands.Count; i++)
            {
                map.Lands.Remove(Drawer.SelectedLands[i]);
            }
            reDrawMap();
        }

        /// <summary>
        /// Функция отмены выделения
        /// </summary>
        private void deselect(object sender, RoutedEventArgs e)
        {
            foreach (var shape in Drawer.SelectedShapes)
            {
                shape.StrokeThickness = 0;
            }
            Drawer.SelectedShapes.Clear();
            Drawer.SelectedLands.Clear();
        } 

        /// <summary>
        /// При выборе одной из местностей (нажатии соответствующей Radio Button), Drawer.Name меняется на название этой местности
        /// </summary>
        
        #region checkRadioButton

        private void roadChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Road";
        }

        private void fieldChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Field";
        }

        private void sandChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Sand";
        }

        private void forestChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Forest";
        }

        private void swampChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Swamp";
        }

        private void lowlandChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "LowLand";
        }

        private void cityChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "City";
        }

        private void mountainChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Mountain";
        }

        private void waterChecked(object sender, RoutedEventArgs e)
        {
            Drawer.LandName = "Water";
        }
        #endregion

        /// <summary>
        /// Обработка нажатия кнопки "Загрузить карту"
        /// Функция загрузки карты из файла 
        /// </summary>
        private void loadMap(object sender, RoutedEventArgs e) 
        {
            if (Maps.SelectedItem == null)
                MessageBox.Show(Messages.MapIsNotChosen);
            else
            {
                mapToDraw.Content = null;
                map = Serializer.LoadGame(Maps.SelectedItem.ToString(), map);
                reDrawMap();
                mapToDraw.Height = 2000;
                mapToDraw.Width = 2000;
            }

        }

        /// <summary>
        /// Функция перерисовки карты 
        /// </summary>
        private void reDrawMap() 
        {
            if (UI.GamePresenter == null)
            {
                UI.NewGame(map);
            }
            UI.GamePresenter.UpdateMap(map);
            mapToDraw.Content = null;
            mapToDraw.Content = UI.GamePresenter.Map;
        }

        /// <summary>
        /// Функция получает доступные карты для загрузки
        /// </summary>
        /// <returns>список карт</returns>
        private List<string> getAllowedMaps() 
        {
            List<string> allowedMaps = new List<string>();
            var files = Directory.GetFiles(".");
            foreach (var fileName in files) // предположим, что в названии файла нет точек
            {
                string[] temp = fileName.Split('.');
                if (temp[1] == "xml")
                {
                    allowedMaps.Add(fileName);
                }
            }
            return allowedMaps;
        }

        /// <summary>
        /// Функция сохранения игры в файл
        /// </summary>
        private void saveMap(object sender, RoutedEventArgs e) 
        {
            string name = mapName.Text;
            if (name == "")
            {
                MessageBox.Show(Messages.EmptyMapName);
            }
            else
            {
                Serializer.Serialize(map, Environment.CurrentDirectory, name);
                updateAllowedMaps();
            }
        }

        /// <summary>
        /// Обработка нажатия "Загрузить фоновую картинку"
        /// Загрузка фонового изображения редактируемой карты
        /// </summary>
        private void loadImage(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                openFileDialog1.ShowDialog();

                var children = UserInterface.StoryConverter.FindVisualChild<ContentPresenter>(mapToDraw);
                Rectangle rectangle = (Rectangle)UI.TheMapControl.ContentTemplate.FindName("FakeFieldRectangle", children);
                if (openFileDialog1.FileName != "")
                    rectangle.Fill = new ImageBrush(new BitmapImage(new Uri(openFileDialog1.FileName, UriKind.Relative)));
                
                map.Background = openFileDialog1.FileName;//получение адреса фонового изображения
            }
            catch
            {
                MessageBox.Show(Messages.LoadFileError);
            }
        }

        private void addModel(Geometry.Figures.Point point)
        {
            //if (comboBox.SelectedItem != null)
            //{
            //    string unitType = comboBox.SelectedItem.ToString();
            //    switch (unitType)
            //    {
            //        case "System.Windows.Controls.ComboBoxItem: Бочка":
            //            {
            //                var item = new GameEngine.Item(GameEngine.ItemType.Barrel, point);
            //                UserInterface.Presenters.GameItemsPresenter itemPres = new UserInterface.Presenters.GameItemsPresenter(item);
            //                map.Items.Add(item);
            //                UI.GamePresenter.Map.VisibleItems.Add(itemPres);
            //                break;
            //            }
            //        case "System.Windows.Controls.ComboBoxItem: БА-6":
            //            {
            //                var item = new GameEngine.Item(GameEngine.ItemType.BA_6, point);
            //                UserInterface.Presenters.GameItemsPresenter itemPres = new UserInterface.Presenters.GameItemsPresenter(item);
            //                map.Items.Add(item);
            //                UI.GamePresenter.Map.VisibleItems.Add(itemPres);
            //                break;
            //            }
            //        case "System.Windows.Controls.ComboBoxItem: ГАЗ-АА":
            //            {
            //                var item = new GameEngine.Item(GameEngine.ItemType.GAZ_AA, point);
            //                UserInterface.Presenters.GameItemsPresenter itemPres = new UserInterface.Presenters.GameItemsPresenter(item);
            //                map.Items.Add(item);
            //                UI.GamePresenter.Map.VisibleItems.Add(itemPres);
            //                break;
            //            }
            //        case "System.Windows.Controls.ComboBoxItem: СУ-12-1":
            //            {
            //                var item = new GameEngine.Item(GameEngine.ItemType.SU_12_1, point);
            //                UserInterface.Presenters.GameItemsPresenter itemPres = new UserInterface.Presenters.GameItemsPresenter(item);
            //                map.Items.Add(item);
            //                UI.GamePresenter.Map.VisibleItems.Add(itemPres);
            //                break;
            //            }
            //        case "System.Windows.Controls.ComboBoxItem: Т-37":
            //            {
            //                var item = new GameEngine.Item(GameEngine.ItemType.T_37, point);
            //                UserInterface.Presenters.GameItemsPresenter itemPres = new UserInterface.Presenters.GameItemsPresenter(item);
            //                map.Items.Add(item);
            //                UI.GamePresenter.Map.VisibleItems.Add(itemPres);
            //                break;
            //            }
            //        case "System.Windows.Controls.ComboBoxItem: 122мм":
            //            {
            //                var item = new GameEngine.Item(GameEngine.ItemType.GUN122MM, point);
            //                UserInterface.Presenters.GameItemsPresenter itemPres = new UserInterface.Presenters.GameItemsPresenter(item);
            //                map.Items.Add(item);
            //                UI.GamePresenter.Map.VisibleItems.Add(itemPres);
            //                break;
            //            }
            //    }

            //    reDrawMap();
            //}
        }

        /// <summary>
        /// Не используется
        /// </summary>
        //private void CheckBox_Click(object sender, RoutedEventArgs e)
        //{
        //    if (flag) flag = false;
        //    else flag = true;
        //}


        /// <summary>
        /// Создание отряда, добавляет созданный отряд в UnitCreater.Groups 
        /// </summary>
        private void CreateGroup(object sender, RoutedEventArgs e)
        {
            int count = 0;
            messageGroup.Text = "";
            try
            {
                count = Convert.ToInt32(unitCount.Text);
            }
            catch
            {
                if (unitCount.Text == "")
                    MessageBox.Show(Messages.EmptyStrength,Messages.EmptyFields);
                else
                    MessageBox.Show(Messages.FieldContainsNumbers, Messages.IncorrectUnitCountValue);
                return;
            }

            try
            {
                switch ((GameEngine.Characters.Rank)Rank.SelectionBoxItem)
                {
                    case GameEngine.Characters.Rank.Sergeant:
                        {
                            UnitCreater.Rank = GameEngine.Characters.Rank.Sergeant;
                            break;
                        }
                    case GameEngine.Characters.Rank.Ordinary:
                        {
                            UnitCreater.Rank = GameEngine.Characters.Rank.Ordinary;
                            break;
                        }
                    case GameEngine.Characters.Rank.Officer:
                        {
                            UnitCreater.Rank = GameEngine.Characters.Rank.Officer;
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show(Messages.EmptyRank,Messages.EmptyFields);
                return;
            }

            try
            {

                switch ((GameEngine.Characters.Specialization)Specialization.SelectionBoxItem)
                {
                    case GameEngine.Characters.Specialization.InfantryMan:
                        {
                            UnitCreater.Specialization = GameEngine.Characters.Specialization.InfantryMan;
                            break;
                        }
                    case GameEngine.Characters.Specialization.TankMan:
                        {
                            UnitCreater.Specialization = GameEngine.Characters.Specialization.TankMan;
                            break;
                        }
                    case GameEngine.Characters.Specialization.EngineerMan:
                        {
                            UnitCreater.Specialization = GameEngine.Characters.Specialization.EngineerMan;
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show(Messages.EmptySpecialization, Messages.EmptyFields);
                return;
            }

            try
            {
                switch ((GameEngine.Characters.Qualification)Qualification.SelectionBoxItem)
                {
                    case GameEngine.Characters.Qualification.low:
                        {
                            UnitCreater.Qualification = GameEngine.Characters.Qualification.low;
                            break;
                        }
                    case GameEngine.Characters.Qualification.medium:
                        {
                            UnitCreater.Qualification = GameEngine.Characters.Qualification.medium;
                            break;
                        }
                    case GameEngine.Characters.Qualification.high:
                        {
                            UnitCreater.Qualification = GameEngine.Characters.Qualification.high;
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show(Messages.EmptyQualification, Messages.EmptyFields);
                return;
            }

            try
            {
                switch ((GameEngine.Characters.Vitality)Vitality.SelectionBoxItem)
                {
                    case GameEngine.Characters.Vitality.healthy:
                        {
                            UnitCreater.Vitality = GameEngine.Characters.Vitality.healthy;
                            break;
                        }
                    case GameEngine.Characters.Vitality.wounded:
                        {
                            UnitCreater.Vitality = GameEngine.Characters.Vitality.wounded;
                            break;
                        }
                    case GameEngine.Characters.Vitality.heavilyWounded:
                        {
                            UnitCreater.Vitality = GameEngine.Characters.Vitality.heavilyWounded;
                            break;
                        }
                    case GameEngine.Characters.Vitality.dead:
                        {
                            UnitCreater.Vitality = GameEngine.Characters.Vitality.dead;
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show(Messages.EmptyVitality, Messages.EmptyFields);
                return;
            }

            try
            {

                switch ((GameEngine.Characters.Qualification)Experience.SelectionBoxItem)
                {
                    case GameEngine.Characters.Qualification.low:
                        {
                            UnitCreater.Experience = GameEngine.Characters.Qualification.low;
                            break;
                        }
                    case GameEngine.Characters.Qualification.medium:
                        {
                            UnitCreater.Experience = GameEngine.Characters.Qualification.medium;
                            break;
                        }
                    case GameEngine.Characters.Qualification.high:
                        {
                            UnitCreater.Experience = GameEngine.Characters.Qualification.high;
                            break;
                        }
                }

                Group group = new Group(count, UnitCreater.Rank, UnitCreater.Specialization, UnitCreater.Qualification, UnitCreater.Experience, UnitCreater.Vitality);
                UnitCreater.Groups.Add(group);
                
                messageGroup.Text = Messages.GroupIsCreated;
            }


            
            catch 
            {
                MessageBox.Show(Messages.EmptyExperience,Messages.EmptyFields);
            }
        }

        /// <summary>
        /// Создание предмета. Добавляет предмет в UnitCreater.Items
        /// </summary>
        private void CreateItem(object sender, RoutedEventArgs e)
        {
            messageItem.Text = "";

            try
            {
                switch ((Caliber)weaponCaliber.SelectionBoxItem)
                {
                    case Caliber.smallArms:
                        {
                            UnitCreater.Weapon = Caliber.smallArms;
                            break;
                        }
                    case Caliber.largeSmallArms:
                        {
                            UnitCreater.Weapon = Caliber.largeSmallArms;
                            break;
                        }
                    case Caliber.small:
                        {
                            UnitCreater.Weapon = Caliber.small;
                            break;
                        }
                    case Caliber.medium:
                        {
                            UnitCreater.Weapon = Caliber.medium;
                            break;
                        }
                    case Caliber.large:
                        {
                            UnitCreater.Weapon = Caliber.large;
                            break;
                        }
                    case Caliber.extraLarge:
                        {
                            UnitCreater.Weapon = Caliber.extraLarge;
                            break;
                        }
                }
            }

            catch
            {
                MessageBox.Show(Messages.EmptyCaliber, Messages.EmptyFields);
                return;
            }

            try
            {
                switch ((Caliber)armorCaliber.SelectionBoxItem)
                {
                    case Caliber.smallArms:
                        {
                            UnitCreater.Armor = Caliber.smallArms;
                            break;
                        }
                    case Caliber.largeSmallArms:
                        {
                            UnitCreater.Armor = Caliber.largeSmallArms;
                            break;
                        }
                    case Caliber.small:
                        {
                            UnitCreater.Armor = Caliber.small;
                            break;
                        }
                    case Caliber.medium:
                        {
                            UnitCreater.Armor = Caliber.medium;
                            break;
                        }
                    case Caliber.large:
                        {
                            UnitCreater.Armor = Caliber.large;
                            break;
                        }
                    case Caliber.extraLarge:
                        {
                            UnitCreater.Armor = Caliber.extraLarge;
                            break;
                        }
                }

            }

            catch
            {
                MessageBox.Show(Messages.EmptyArmor, Messages.EmptyFields);
                return;
            }

            try 
            {
                UnitCreater.Speed = Convert.ToInt32(movementSpeed.Text);
                
            }
            catch 
            {
                if (movementSpeed.Text == "")
                    MessageBox.Show(Messages.EmptyMovementSpeed, Messages.EmptyFields);
                else
                    MessageBox.Show(Messages.IncorrectTextBoxValue, Messages.IncorrectInput);
                return;
            }
            try
            {
                UnitCreater.FireRate = Convert.ToInt32(fireRate.Text);
            }
            catch
            {
                if (fireRate.Text == "")
                    MessageBox.Show(Messages.EmptyFireRate, Messages.EmptyFields);
                else
                    MessageBox.Show(Messages.IncorrectTextBoxValue, Messages.IncorrectInput);
                return;
            }
            try
            {
                UnitCreater.Crew = Convert.ToInt32(crew.Text);
            }
            catch
            {
                if (crew.Text == "")
                    MessageBox.Show(Messages.EmptyCrew, Messages.EmptyFields);
                else
                    MessageBox.Show(Messages.IncorrectTextBoxValue, Messages.IncorrectInput);
                return;
            }

            try
            {
                UnitCreater.Count = Convert.ToInt32(count.Text);
            }
            catch
            {
                if (count.Text == "")
                    MessageBox.Show(Messages.EmptyCount, Messages.EmptyFields);
                else
                    MessageBox.Show(Messages.IncorrectTextBoxValue, Messages.IncorrectInput);
                return;
            }

            
            try
            {
                //определяем тип юнита
                switch ((UnitType)UnitTypes.SelectionBoxItem)
                {
                    case UnitType.Infantry:
                        {
                            UnitCreater.Type = UnitType.Infantry;
                            break;
                        }
                    case UnitType.Cavalry:
                        {
                            UnitCreater.Type = UnitType.Cavalry;
                            break;
                        }
                    case UnitType.Artillery:
                        {
                            UnitCreater.Type = UnitType.Artillery;
                            break;
                        }
                    case UnitType.Armor:
                        {
                            UnitCreater.Type = UnitType.Armor;
                            break;
                        }
                    case UnitType.AirForce:
                        {
                            UnitCreater.Type = UnitType.AirForce;
                            break;
                        }
                    case UnitType.Navy:
                        {
                            UnitCreater.Type = UnitType.Navy;
                            break;
                        }
                    case UnitType.Engineers:
                        {
                            UnitCreater.Type = UnitType.Engineers;
                            break;
                        }
                }

            }
            // сообщение, которое выводится в случае некорректного ввода данных 
            catch
            {
            }
            
            EquipmentMark equip = new EquipmentMark(UnitCreater.Weapon, UnitCreater.Armor, UnitCreater.Speed, UnitCreater.FireRate, UnitCreater.Crew, UnitCreater.Count);
            UnitCreater.Items.Add(equip);
            messageItem.Text = Messages.ItemIsCreated;
        }

        /// <summary>
        /// Создание войска. Войско создаётся при условии, что списки UnitCreater.Groups, UnitCreaters.Items не пусты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateUnit(object sender, RoutedEventArgs e)
        {
            messageUnit.Text = "";
           
            if (Drawer.ClickedPoints.Count == 0)
            {
                MessageBox.Show(Messages.PositionIsNotChosen);
                return;    
            }
            Geometry.Figures.ConvexPolygon polygon = new Geometry.Figures.ConvexPolygon(Drawer.ClickedPoints);

            try
            {
                //определяем тип юнита
                switch ((UnitType)UnitTypes.SelectionBoxItem)
                {
                    case UnitType.Infantry:
                        {
                            UnitCreater.Type = UnitType.Infantry;
                            break;
                        }
                    case UnitType.Cavalry:
                        {
                            UnitCreater.Type = UnitType.Cavalry;
                            break;
                        }
                    case UnitType.Artillery:
                        {
                            UnitCreater.Type = UnitType.Artillery;
                            break;
                        }
                    case UnitType.Armor:
                        {
                            UnitCreater.Type = UnitType.Armor;
                            break;
                        }
                    case UnitType.AirForce:
                        {
                            UnitCreater.Type = UnitType.AirForce;
                            break;
                        }
                    case UnitType.Navy:
                        {
                            UnitCreater.Type = UnitType.Navy;
                            break;
                        }
                    case UnitType.Engineers:
                        {
                            UnitCreater.Type = UnitType.Engineers;
                            break;
                        }
                }

            }
            catch
            {
                MessageBox.Show(Messages.EmptyUnit, Messages.EmptyFields);
                return;
            }

            try
            {
                // определяем страну
                switch ((Countries)Country.SelectionBoxItem)
                {
                    case Countries.USSR:
                        {
                            UnitCreater.Country = Countries.USSR;
                            break;
                        }
                    case Countries.Japan:
                        {
                            UnitCreater.Country = Countries.Japan;
                            break;
                        }
                    case Countries.Mongolia:
                        {
                            UnitCreater.Country = Countries.Mongolia;
                            break;
                        }
                    case Countries.Manchukuo:
                        {
                            UnitCreater.Country = Countries.Manchukuo;
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show(Messages.EmptyCountry,Messages.EmptyFields);
                return;
            }

            try
            {
                // определяем сторону
                switch (Side.Text)
                {
                        
                    case "1":
                        {
                            UnitCreater.Side = 1;
                            break;
                        }
                    case "0":
                        {
                            UnitCreater.Side = 0;
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show(Messages.EmptySide, Messages.EmptyFields);
                return;
            }

            
            try
            {
                if (UnitCreater.Groups.Count == 0 || UnitCreater.Items.Count == 0)
                    throw new Exception();

                UnitFeatures features = new UnitFeatures(UnitCreater.Groups, UnitCreater.Items);
                Unit unit = new Unit(polygon, features, UnitCreater.Type);
                unit.SetSide(UnitCreater.Side, UnitCreater.Country, null);

                UnitCreater.Groups = new List<Group>();
                UnitCreater.Items = new List<Item>();

                map.Units.Add(unit);
                resetPoints(null, null);
                reDrawMap();
                messageUnit.Text = Messages.UnitIsCreated;

            }
            catch
            {
                MessageBox.Show(Messages.ItemOrGroupIsNotCreated);
            }

            
        }

        /// <summary>
        /// Создание небоевого предмета. Созданный предмет добавляется в список UnitCreater.Items
        /// </summary>
        private void CreateGood(object sender, RoutedEventArgs e)
        {
            messageGood.Text = "";
            try
            {
                UnitCreater.ObjCount = Convert.ToInt32(objCount.Text);

                switch ((ObjectType)goodsType.SelectionBoxItem)
                {
                    case ObjectType.Ammunition:
                        {
                            UnitCreater.ObjType = ObjectType.Ammunition;
                            break;
                        }
                    case ObjectType.Fuel:
                        {
                            UnitCreater.ObjType = ObjectType.Fuel;
                            break;
                        }
                    case ObjectType.Provision:
                        {
                            UnitCreater.ObjType = ObjectType.Provision;
                            break;
                        }
                    case ObjectType.Medicines:
                        {
                            UnitCreater.ObjType = ObjectType.Medicines;
                            break;
                        }
                    case ObjectType.Spares:
                        {
                            UnitCreater.ObjType = ObjectType.Spares;
                            break;
                        }
                    case ObjectType.BuildingMaterials:
                        {
                            UnitCreater.ObjType = ObjectType.BuildingMaterials;
                            break;
                        }
                    case ObjectType.Structures:
                        {
                            UnitCreater.ObjType = ObjectType.Structures;
                            break;
                        }
                }
                               
                Goods good = new Goods(UnitCreater.ObjType, UnitCreater.ObjCount, new Circle (new Geometry.Figures.Point(0.0,0.0), 0.5)); // ?
                UnitCreater.Items.Add(good);
                messageGood.Text = Messages.GoodIsCreated;
            }
            catch (FormatException exception)
            {
                MessageBox.Show(Messages.IncorrectGoodCount);
            }
            catch { MessageBox.Show(Messages.EmptyFields); }

        }

        /// <summary>
        /// Обработчик события нажатия на кнопку, закрывающую окно
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //e.Cancel = true;
            MainWindow.MainMenuWindow.Show();
            Hide();
        }
    }
}
