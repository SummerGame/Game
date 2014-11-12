using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using GameEngine.Characters;
using GameEngine.Lands;
using AbstractGameEngine;

namespace GameEngine
{
    /// <summary>
    /// Статический Класс "Сериализатор" 
    /// Дает возможность серилизации из xml/в xml для:
    /// Карты,
    /// Юнитов
    /// </summary>
    public static class Serializer
    {
        #region Methods
        /// <summary>
        /// Метод серилизации
        /// Сохраняет игру в формате xml по указанному пути.
        /// </summary>
        /// <param name="map">Карта</param>
        /// <param name="path">Путь</param>
        public static void Serialize(GameMap map, string path, string name)
        {
            var mapPath = path + "\\Maps\\" + name + " map.xaml";
            var xdocument = new XDocument(map.Serialize());
            xdocument.Save(mapPath);
            
            path += "\\" + name + ".xml";

            var xMap = new XElement("Map");
            xMap.SetAttributeValue("Path", mapPath);

            //сохранение фонового изображения
            var xBackground = new XElement("Background");
            if (map.Background != null && map.Background !="")
            {
                FileStream bgOrigin = new FileStream(map.Background, FileMode.Open, FileAccess.Read);//источник фона карты

                var directs = map.Background.Split('.');
                string bgMapAddress = "bg" + name + "." + directs[directs.Length - 1];
                FileStream bgMap = new FileStream(bgMapAddress, FileMode.Create);//куда скопировать фоновое изображение
                bgOrigin.CopyTo(bgMap);
                xBackground.SetAttributeValue("Path", bgMapAddress);
            }

            // сохраняем юнитов
            var xDoc = new XDocument(Serialize(map.Units));
            // сохраняем итемы 
            XElement xItems = Serialize(map.Items);

            xDoc.Root.Add(xMap);
            xDoc.Root.Add(xBackground);
            xDoc.Root.Add(xItems);
            xDoc.Save(path);
        }

        /// <summary>
        /// Метод серилизации
        /// Сохраняет юнитов в XDocument
        /// </summary>
        /// <param name="units">Юниты</param>
        public static XDocument Serialize(List<Unit> units)
        {
            var xdocument = new XDocument(units.Serialize());
            return xdocument;
        }

        /// <summary>
        /// Метод серилизации
        /// Сохраняет итемы в XDocument
        /// </summary>
        /// <param name="units">Итемы</param>
        public static XElement Serialize(List<Item> items)
        {
            var xdocument = new XElement(items.Serialize());
            return xdocument;
        }

        /// <summary>
        /// Метод десерилизации
        /// Создает новую карту в Статическом Классе Игры и загружает ее из xml файла по указанному пути.
        /// </summary>
        /// <param name="path">Путь</param>
        public static GameMap Deserialize(string path, GameMap map)
        {
            if (File.Exists(path))
            {
                var xdocument = XDocument.Load(path);
                map = new GameMap();

                if (xdocument.Root != null)
                {
                    foreach (var node in xdocument.Root.Attributes())
                    {
                        if(node.Name.ToString().Equals("Width"))
                            map.Width = (int)xdocument.Root.Attribute("Width");
                        else if (node.Name.ToString().Equals("Height"))
                            map.Height = (int)xdocument.Root.Attribute("Height");
                    }

                    foreach (var node in xdocument.Root.Elements())
                        map.Lands.Add((Landscape)node.Deserialize());
                }
                return map;
            }
            else
            {
                throw new Exception("File to deserialize doesn't exists");
            }
        }

        /// <summary>
        /// Получение адреса фонового изображения карты
        /// </summary>
        /// <param name="path">Проверяемое значение адреса фонового изображения карты</param>
        /// <returns>Aдрес фонового изображения карты</returns>
        public static string DeserializeBGImage(string path)
        {
            string bgImage = "";
            if (File.Exists(path))
            {
                bgImage = path;
            }
            return bgImage;
        }

        /// <summary>
        /// Метод десерилизации
        /// Создает список Юнитов с файла.
        /// </summary>
        /// <param name="path">Путь</param>
        public static List<Unit> DeserializeUnits(string path)
        {
            if (File.Exists(path))
            {
                var xdocument = XDocument.Load(path);
                var units = new List<Unit>();
                if (xdocument.Root != null)
                    foreach (var node in xdocument.Root.Elements())
                    {
                        if (node.Name.ToString().Equals("Unit"))
                            units.Add((Unit)node.Deserialize());
                    }
                return units;
            }
            else
            {
                throw new Exception("File to deserialize doesn't exists");
            }
        }

        /// <summary>
        /// Метод загрузки игры
        /// Загружает карту игры и состояние юнитов
        /// </summary>
        /// <param name="path">Путь к файлу загрузки</param>
        public static GameMap LoadGame(string path, GameMap map)
        {
            if (File.Exists(path))
            {
                var xdocument = XDocument.Load(path);
                var units = new List<Unit>();
                if (xdocument.Root != null)
                {
                    units = DeserializeUnits(path);
                    var xElement = xdocument.Root.Element("Map");
                    if (xElement != null)
                        map = Deserialize("..\\..\\"+(string)xElement.Attribute("Path"), map);
                    // map.Name = (string)xElement.Attribute("Path");

                    //нахождение фона карты
                    var xBackground = xdocument.Root.Element("Background");
                    if (xBackground != null)
                        map.Background = DeserializeBGImage((string)xBackground.Attribute("Path"));
                }

                map.Units.Clear();
                map.Units.AddRange(units);
                Game.gameLoaded = true; // what?
                return map;
            }
            else
            {
                throw new Exception("File to deserialize doesn't exists");
            }
        }

        /// <summary>
        /// Метод сохранения игры
        /// Сохраняет игру
        /// </summary>
        /// <param name="path">Путь</param>
        public static void SaveGame(string path)
        {
            if (File.Exists(path))
            {
                var xdocument = Serialize(Game.mainMap.Units);
                var map = new XElement("Map");
                xdocument.Root.Add(map);
                xdocument.Save(path);
            }
            else
            {
                throw new Exception("File to deserialize doesn't exists");
            }
        }

        #endregion
    }
}

