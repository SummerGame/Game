using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Characters;
using GameEngine.Characters.Groups;
using GameEngine.Object;
using GameEngine;
using AbstractGameEngine;


namespace mapDrawer
{
    public static class UnitCreater
    {
        private static List<Group> groups = new List<Group>();
        private static List<Item> items = new List<Item>();
        private static List<Item> equip = new List<Item>();
        
        //для группы
        private static Rank rank;
        private static Vitality vitality;
        private static Specialization specialization;
        private static Qualification qualification;
        private static Qualification experience;
        private static Countries country;

        //для юнита
        private static UnitType type;
        private static int side;

        //для оружия
        private static Caliber weapon;
        private static Caliber armor;
        private static double speed;
        private static double fireRate;
        private static int crew;
        private static int count;

        //для небоевых предметов
        private static int objectCount;
        private static ObjectType objType;
        #region 
        public static ObjectType ObjType
        {
            get { return UnitCreater.objType; }
            set { UnitCreater.objType = value; }
        }

        public static Caliber Weapon
        {
            get { return UnitCreater.weapon; }
            set { UnitCreater.weapon = value; }
        }

        public static Caliber Armor
        {
            get { return UnitCreater.armor; }
            set { UnitCreater.armor = value; }
        }

        public static int Count
        {
            get { return UnitCreater.count; }
            set { UnitCreater.count = value; }
        }

        public static int Crew
        {
            get { return UnitCreater.crew; }
            set { UnitCreater.crew = value; }
        }

        public static double Speed
        {
            get { return UnitCreater.speed; }
            set { UnitCreater.speed = value; }
        }

        public static double FireRate
        {
            get { return UnitCreater.fireRate; }
            set { UnitCreater.fireRate = value; }
        }

        public static int Side
        {
            get { return UnitCreater.side; }
            set { UnitCreater.side = value; }
        }

        public static Countries Country
        {
            get { return UnitCreater.country; }
            set { UnitCreater.country = value; }
        }

        public static Rank Rank
        {
            get { return UnitCreater.rank; }
            set { UnitCreater.rank = value; }
        }

        public static Specialization Specialization
        {
            get { return UnitCreater.specialization; }
            set { UnitCreater.specialization = value; }
        }

        public static Qualification Qualification
        {
            get { return UnitCreater.qualification; }
            set { UnitCreater.qualification = value; }
        }

        public static Qualification Experience
        {
            get { return UnitCreater.experience; }
            set { UnitCreater.experience = value; }
        }
        
        public static Vitality Vitality
        {
            get { return UnitCreater.vitality; }
            set { UnitCreater.vitality = value; }
        }

        public static List<Item> Items
        {
            get { return UnitCreater.items; }
            set { UnitCreater.items = value; }
        }

        public static List<Group> Groups
        {
            get { return groups; }
            set { groups = value; }
        }

        public static UnitType Type
        {
            get { return UnitCreater.type; }
            set { UnitCreater.type = value; }
        }

        public static List<Item> Equip
        {
            get { return UnitCreater.equip; }
            set { UnitCreater.equip = value; }
        }

        public static int ObjCount
        {
            get { return UnitCreater.objectCount; }
            set { UnitCreater.objectCount = value; }
        }
        #endregion
    }
}
