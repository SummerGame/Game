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
using Geometry;
using GameEngine.Characters;
using GameEngine.Lands;
using GameEngine.Object;
using GameEngine.StoryTelling;
using UserInterface.Presenters;
using System.ComponentModel;



namespace UserInterface
{

    public delegate void GameReloadedEvent(object src, EventArgs e);


    public static class UI
    {

        #region Fields

        private static bool pause; // переменная отвечает за паузу.

        private static Storyboard aStoryboard = new Storyboard();

        private static ContentControl theMapControl = null;

        private static GamePresenter gamePresenter;

        #endregion

        static UI()
        {
        }

        #region Properties

        public static bool Pause
        {
            get { return UI.pause; }
            set { UI.pause = value; }
        }

        public static Storyboard AStoryboard
        {
            get { return UI.aStoryboard; }
            // set { UserInterface.aStoryboard = value; }
        }

        public static ContentControl TheMapControl
        {
            get { return UI.theMapControl; }
            set { UI.theMapControl = value; }
        }

        public static GamePresenter GamePresenter
        {
            get { return UI.gamePresenter; }
            //set { UI.gamePresenter = value; }
        }

        #endregion

        #region Public Methods
        // движение фигур по клику

        public static void Movement(Point pos)
         {
            if (UISelection.AddsList.Count != 0) // хотим добавить новый список в контейнер
            {
                UISelection.ClickedPoints.Add(pos);
                UISelection.AllFigures.Add(UISelection.AddsList);
                foreach (FrameworkElement f in UISelection.AddsList)
                {
                    MouseOrderMove(pos, f);
                }
                UISelection.AddsList = new List<FrameworkElement>();
            }
            else if ((UISelection.AllFigures.Count != 0) && (UISelection.SelectedItems.Count != 0))
            {
                int ind = UISelection.AllFigures.Count;
                foreach (FrameworkElement f in UISelection.AllFigures[ind - 1])
                {
                    MouseOrderMove(pos, f);
                }
                UISelection.ClickedPoints.Insert(ind - 1, pos); //[ind - 1] = pos;
            }
        }

        public static void Attack(FrameworkElement attackedshape)
        {
            foreach (var shape in UISelection.SelectedItems)
            {
                MouseOrderAttack(shape, attackedshape);
            }
        }

        public static void Build(Unit unit)
        {
            MouseOrderBuild(unit);
        }


        /// <summary>
        /// Функция смены хода
        /// </summary>
        public static void TogglePause()
        {
            if (GameEngine.Game.Pvp)
            {
                if (GameEngine.Game.PlayerTeam == 0) ChangeSide(1);
                else

                    if (aStoryboard.Children.Count != 0)
                    {
                        ClockState clockState = aStoryboard.GetCurrentState();
                        if (clockState != ClockState.Active)
                        {
                            Start();
                        }

                    }
                    else
                    {
                        Start();
                    }
            }
            else
            {
                if (pause)
                {
                    Start();
                    pause = false;
                }
                else
                {
                    pause = true;
                }
            }

        }

        public static List<String> GetInfo(Shape fig)
        {
            var tag = fig.Tag;
            if (tag != null)
            {
                if (tag is Landscape)
                {
                    if (tag is City)
                    {
                        return new List<string> { "Городок" };
                    }
                    else if (tag is Road)
                    {
                        return new List<string> { "Дорога в ад" };
                    }
                    else if (tag is Field)
                    {
                        return new List<string> { "Поляна" };
                    }
                    else if (tag is Forest)
                    {
                        return new List<string> { "Лес, чудесный лес" };
                    }
                    else if (tag is Lowland)
                    {
                        return new List<string> { "Низина" };
                    }
                    else if (tag is Mountains)
                    {
                        return new List<string> { "Горы" };
                    }
                    else if (tag is Water)
                    {
                        return new List<string> { "Водица" };
                    }
                }
                else if (tag is Unit)
                {
                    return new List<String>{ "Отряд" };//((Unit)tag).Description();
                }
            }
            return new List<string> { "Карта" };
        }

        #region New/Load/Save Game
        public static void NewGame()
        {
            try
            { // ЗАГВОЗДКА ЗДЕСЯ
                GameEngine.Game.StartNewGame();   // загрузка карты из файла
                gamePresenter = new GamePresenter(startTime: new DateTime(1939, 5, 1), side: 0);  // ???
                gamePresenter.UpdateMap(Game.MainMap);
                // НАПИСАЛ МИША, НАДО БЫЛО
                
                foreach (Unit unit in Game.MainMap.Units)
                {
                    unit.UnitCommander = new AutomaticCommander("Vasya", CommanderType.Common, unit);
                }
                RaiseGameReloaded();
            }
            catch (Exception e)
            {
                throw e; // todo remove this
            }
        }

        public static void NewGame(GameMap map)
        {
            try
            {
                GameEngine.Game.StartNewGame(map);
                gamePresenter = new GamePresenter(startTime: new DateTime(1939, 5, 1), side: 0);                
                gamePresenter.UpdateMap(map);
                RaiseGameReloaded();
            }
            catch (Exception e)
            {
                throw e; // todo remove this
            }
        }

        public static void NewGame(string path)
        {
            try
            {
                GameEngine.Game.StartNewGame("..\\..\\Maps\\"+path);   // загрузка карты из файла
                gamePresenter = new GamePresenter(startTime: new DateTime(1939, 5, 1), side: 0, bg: Game.MainMap.Background);  // ???
                gamePresenter.UpdateMap(Game.MainMap);
                // НАПИСАЛ МИША, НАДО БЫЛО
                foreach (Unit unit in Game.MainMap.Units)
                {
                    unit.UnitCommander = new AutomaticCommander("Vasya", CommanderType.Common, unit);
                }
                RaiseGameReloaded();

            }
            catch (Exception e)
            {
                throw e; // todo remove this
            }
        }

        public static void SaveGame()
        {
            Serializer.SaveGame("SaveGame.xml");
        }

        public static void LoadGame()
        {

            Serializer.LoadGame("SaveGame.xml", Game.MainMap);
            gamePresenter.UpdateMap(Game.MainMap);
            

        }
        #endregion

        public static void ChangeSide(int side)
        {
			UISelection.AddsList.Clear(); 
            UISelection.AllFigures.Clear();
            UISelection.SelectedItems.Clear();
			GameEngine.Game.ChangeSide(side);
            gamePresenter.UpdateSide(side);
        }

        public static void ZoomIn()//увеличить масштаб игровой карты
        {
            if (Transformer.Ratio <= 200)
            {
                Transformer.Ratio *= 2;
                Transformer.TranslationX = 2 * Transformer.TranslationX - Mouse.GetPosition(null).X;
                Transformer.TranslationY = 2 * Transformer.TranslationY - Mouse.GetPosition(null).Y;
                gamePresenter.UpdateMap();
            }
        }

        public static void ZoomOut()//уменьшить масштаб игровой карты
        {
            if (Transformer.Ratio >= 12.5)
            {
                Transformer.Ratio /= 2;
                Transformer.TranslationX = (Transformer.TranslationX + Mouse.GetPosition(null).X) / 2;
                Transformer.TranslationY = (Transformer.TranslationY + Mouse.GetPosition(null).Y) / 2;
                gamePresenter.UpdateMap();
            }
        }

        public static void Up()//сдвинуть карту вверх
        {
            Transformer.TranslationY += 15;
            gamePresenter.UpdateMap();
        }

        public static void Down()//сдвинуть карту вниз
        {
            Transformer.TranslationY -= 15;
            gamePresenter.UpdateMap();
        }

        public static void Left()//сдвинуть карту влево
        {
            Transformer.TranslationX += 15;
            gamePresenter.UpdateMap();
        }

        public static void Right()//сдвинуть карту вправо
        {
            Transformer.TranslationX -= 15;
            gamePresenter.UpdateMap();
        }

        #endregion
        
        #region Private Methods

        private static void MouseOrderMove(Point p1, FrameworkElement fig)
        {
            var unitPresenter = fig.Tag as UnitPresenter;
            var unit = unitPresenter.Unit;
            var pos = unit.Position;
            Geometry.Figures.Point newPo = new Geometry.Figures.Point(p1.X, p1.Y);
            newPo = Transformer.ConvertToModel(newPo);

            Geometry.Figures.Polyline poly = new Geometry.Figures.Polyline(pos.X, pos.Y, newPo.X, newPo.Y);
            
            GameEngine.Game.SendOrder(unit, new MoveAction(unit, poly));
            unitPresenter.Update();
        }

        private static void MouseOrderBuild(Unit unit)
        {
            GameEngine.Game.SendOrder(unit, new BuildAction(unit, new Goods(ObjectType.Structures, 1, new Geometry.Figures.Circle(unit.Position, 1))));
        }

        private static void MouseOrderAttack(FrameworkElement attacker, FrameworkElement defender)
        {
            var attacker_unit = ((UnitPresenter)attacker.Tag).Unit;
            var defender_unit = ((UnitPresenter)defender.Tag).Unit;
            if (attacker_unit.CanAttack(defender_unit))
            {
                //var attack_action = new AttackAction(attacker_unit, defender_unit);
                //GameEngine.Game.SendOrder(attacker_unit, attack_action);
                attacker_unit.UnitCommander.Attack(defender_unit);
                //attacker_unit.UnitCommander.MassAttack(new List<Unit>(), defender_unit);
                // может тут проверять бьют ли уже дефендера
            }
        }

        private static void Start()
        {
            aStoryboard = StoryConverter.GetAnimation(GameEngine.Game.UpdateTime());
            aStoryboard.Completed += Completed;
            gamePresenter.UpdateTime(Game.GameTime);
            aStoryboard.Begin();
        }

        private static void Completed(object sender, EventArgs e)
        {
            if (GameEngine.Game.Pvp)
            {
                pause = true;
                ChangeSide(0);
                gamePresenter.UpdateMap();
            }
            else
            {
                if (!pause)
                {
                    Start();
                    gamePresenter.UpdateMap();
                }
            }
        }

        #endregion

        #region GameReloaded event

        public static event GameReloadedEvent GameReloaded;

        public static void RaiseGameReloaded()
        {
            if (GameReloaded != null)
                GameReloaded(gamePresenter, new EventArgs());
        }

        #endregion



    }
}
