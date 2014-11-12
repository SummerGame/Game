using System.Collections.Generic;
using System.ComponentModel;
using AbstractGameEngine;
using GameEngine.Lands;
using GameEngine.Characters;
using Geometry;

namespace GameEngine
{
    /// <summary>
    /// Класс Карта Игры
    /// Предназначен для хранения всех объектов игры
    /// </summary>
    public class GameMap : INotifyPropertyChanged
    {
        #region Fields

        private string name; 
        private int width;
        private int height;
        private List<Landscape> lands;
        private List<Item> items;
        private List<Unit> units;
        private string background;//фон карты
        private List<MoveAction> moveActions;//на будущее - список для хранения всех действий движения на карте

        #endregion

        #region Properties

        public List<Landscape> Lands
        {
            get { return lands; }
        }

        public List<Unit> Units
        {
            get { return units; }
            set { throw new System.NotImplementedException(); }
        }

        public List<Item> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Картинка фона карты
        /// </summary>
        public string Background
        {
            get { return background; }
            set { background = value; }
        }

        /// <summary>
        /// Список всех действий движения юнитов на карте (на будущее)
        /// </summary>
        public List<MoveAction> MoveActions
        {
            get { return moveActions; }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Ширина игровой карты в километрах
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Высота игровой карты в километрах
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Constructors

        public GameMap()
        {
            lands = new List<Landscape>();
            units = new List<Unit>();
            items = new List<Item>();
            moveActions = new List<MoveAction>();//на будущее - список для хранения всех действий на карте
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void MapChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
