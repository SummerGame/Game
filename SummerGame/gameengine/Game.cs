using System;
using System.Collections.Generic;
using GameEngine.Characters;
using GameEngine.StoryTelling;
using Geometry.Figures;
using Action = AbstractGameEngine.Action;
using AbstractGameEngine;
using System.Linq;


namespace GameEngine
{
    /// <summary>
    /// Статический Класс "Игра"
    /// Содержит основные настройки игры, время игры и прочие.
    /// Включает методы вызывающие расчеты игровой модели.
    /// </summary>
    public static class Game
    {
        #region Fields

        internal static GameMap mainMap;
        private static double _gameTime = 0;
        private static int _gameTimeInterval = 1; // todo make it double?
        private static int _playerTeam = 0;
        internal static bool gameLoaded = false;
        private static bool _pvp = true;

        #endregion

        #region Properties

        public static GameMap MainMap
        {
            get
            {
                return mainMap;
            }
            set
            { 
                mainMap = value;
            }
        }

        /// <summary>
        /// Продолжительность (в часах) одной единицы игрового времени
        /// </summary>
        public static int GameTimeInterval
        {
            get { return _gameTimeInterval; }
        }

        /// <summary>
        /// Текущее игровое время
        /// </summary>
        public static double GameTime
        {
            get { return _gameTime; }
        }

        public static bool GameLoaded
        {
            get { return gameLoaded; }
        }

        public static bool Pvp
        {
            get { return _pvp; }
            set { _pvp = value; }
        }

        public static int PlayerTeam
        {
            get { return _playerTeam; }
        }

        public static string PlayerStep
        {
            get { return "Player" + (_playerTeam + 1); }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор игры по умолчанию
        /// </summary>
        static Game()
        {
            mainMap = new GameMap();
        }

        #endregion

        #region Methods


        /// <summary>
        ///  Метод, возвращающий текущее время
        /// </summary>
        /// <returns>Время модели</returns>
        public static double Time()
        {
            return _gameTime;
        }

        /// <summary>
        /// Основной расчетный игровой метод
        /// 1) Вызывает Отрисовку WPF элементов карты и юнитов
        /// 2) Производит расчет игровой модели
        /// 3) Обновляет текущий момент времени
        /// После чего возвращает истории для исполнения интерфейсом
        /// </summary>
        /// <returns>Истории</returns>
        public static Stories UpdateTime()
        {

            Stories stories = new Stories();

            for (int i = 0; i < mainMap.Units.Count; i++)
            {
                var unit = mainMap.Units[i];
                var story = new UnitStory(unit);

                ActionCombination(AttackActions());

                if (unit.CurrentAction != null)
                {
                    
                    switch (unit.CurrentAction.GetType().Name)
                    {
                        case "MoveAction":
                            story.States.AddRange((List<UnitState>)((MoveAction)unit.CurrentAction).Now(unit, _gameTime));
                            stories.GetStories.Add(story);
                            break;
                        case "AttackAction":
                            story.States.AddRange((List<UnitState>)((AttackAction)unit.CurrentAction).Now(unit, _gameTime));
                            stories.GetStories.Add(story);
                            break;
                        case "BuildAction":
                            story.States.AddRange((List<UnitState>)((BuildAction)unit.CurrentAction).Now(unit, _gameTime));
                            stories.GetStories.Add(story);
                            break;
                        default:		

                            // Nothing to do
                            break;
                    }

                    if (unit.CurrentAction.Completed)
                    {
                        unit.CurrentAction = null;
                        break;
                    }
                }
            }

                 _gameTime += _gameTimeInterval;
            return stories;
        }

        /// <summary>
        /// Удаляет текущие действия из списка действий юнитов, если они завершены
        /// </summary>
        /// <returns>Список текущих незавершённых действий</returns>
        public static List<Action> AllActions()
        {
            List<Action> actions = new List<Action>();
            for (int i = 0; i < mainMap.Units.Count; i++)
            {
                var unit = mainMap.Units[i];
                if (unit.CurrentAction != null)
                {
                    if (unit.CurrentAction.Completed)
                    {
                        unit.CurrentAction = null;
                    }
                    else
                    {
                        actions.Add(unit.CurrentAction);
                    }
                }
            }
            return actions;
        }

        /// <summary>
        /// Удаляет атаку из списка действий каждого юнита, если она завершена
        /// </summary>
        /// <returns> Список незавершённых атак для всех юнитов</returns>
        public static List<AttackAction> AttackActions()
        {
            List<AttackAction> actions = new List<AttackAction>();
            for (int i = 0; i < mainMap.Units.Count; i++)
            {
                var unit = mainMap.Units[i];
                if (unit.CurrentAction != null)
                {
                    if (unit.CurrentAction.Completed)
                    {
                        unit.CurrentAction = null;
                    }
                    else if (unit.CurrentAction.GetType() == typeof(AttackAction))
                    {
                        actions.Add(unit.CurrentAction as AttackAction);
                    }
                }
            }
            return actions;
        }

        /// <summary>
        /// Группирует дествия "Атака" по нападающим и атакуемым юнитам
        /// </summary>
        /// <param name="actions"></param>
        /// <returns>список атак, в которых несколько юнитов-союзников нападают на один вражеский</returns>
        public static List<CombinedAttackAction> ActionCombination(List<AttackAction> actions)
        {
           // actions.GroupBy(;
            //var battleGroups = new List<Tuple<Unit, List<Unit>>>();
            var list = new List<CombinedAttackAction>();
            //foreach (var action in actions)
            //{
            //    if (action.GetType().Name == "AttackAction")
            //    {
            //        //(action as AttackAction).Defender 
            //        var newGroup = new Tuple<Unit,List<Unit>>((action as AttackAction).Defender,(action as AttackAction).Attaker));
            //    }
            //}
            IEnumerable<IGrouping<Unit, Unit>> query = actions.GroupBy(action => action.Defender, action => action.Attaker);
            foreach (IGrouping<Unit, Unit> group in query)
            {
                list.Add(new CombinedAttackAction(group.ToList(),group.Key));
            }
            return list;
        }

        /// <summary>
        /// Находит все юниты, нападающие на defender
        /// </summary>
        /// <param name="defender">Обороняющийся юнит</param>
        /// <returns>Список атакующих юнитов, нападающих на defender</returns>
        public static List<Unit> Attackers(Unit defender)
        {
            //for (int i = 0; i <  
            List<Unit> attackers = new List<Unit>();
            for (int i = 0; i < mainMap.Units.Count; i++)
            {
                var unit = mainMap.Units[i];
                if (unit.CurrentAction != null)
                {
                    if (unit.CurrentAction.GetType().Name == "AttackAction")
                    {
                        if ((unit.CurrentAction as AttackAction).Defender == defender) attackers.Add(unit);
                    }
                }
            }
            return attackers;
        }
        
        /// <summary>
        /// Метод посылки приказа Юниту
        /// </summary>
        /// <param name="unit">Кому приказ</param>
        /// <param name="task">Приказ</param>
        /// <returns>Успешно ли получен приказ</returns>
        public static bool SendOrder(Unit unit, Action task)
        {
            if (unit.CurrentAction == null)
            {
                unit.CurrentAction = task;
                return true;
            }
            else
            {
                if (unit.CurrentAction.Completed)
                {
                    unit.CurrentAction = task;
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Метод запускающий игру с начальной картой и настройками
        /// </summary>
        public static void StartNewGame()
        {
            try
            {
                Game.mainMap = Serializer.LoadGame("NewGame.xml", null); // Вызываем инициализацию карты
            }
            catch (Exception)
            {

                throw new Exception("Starting New Game Failed.");
            }
        }

        /// <summary>
        /// Добавление предмета на игровую карту
        /// </summary>
        /// <param name="item"></param>
        public static void AddItem(Item item)
        {
            mainMap.Items.Add(item);   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void StartNewGame(string path)
        {
            try
            {
                Game.mainMap = Serializer.LoadGame(path, null); // Вызываем инициализацию карты
            }
            catch (Exception)
            {

                throw new Exception("Starting New Game Failed.");
            }
        }

        public static void StartNewGame(GameMap map)
        {
            try
            {
                Game.mainMap = map; // Вызываем инициализацию карты
            }
            catch (Exception)
            {

                throw new Exception("Starting New Game Failed.");
            }
        }

        /// <summary>
        /// Функция для изменения игрока
        /// </summary>
        /// <param name="newSide">Новый владелец</param>
        public static void ChangeSide(int newSide)
        {
            _playerTeam = newSide;
        }

        #endregion
    }
}
