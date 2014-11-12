using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GameEngine.Characters;
using GameEngine.Object;
using GameEngine;
using Geometry;

namespace UserInterface.Presenters
{
    public class UnitPresenter : DependencyObject
    {
        #region Backing fields

        private Unit original;

        #endregion

        #region Constructors

        public UnitPresenter(Unit u)
        {
            this.original = u;
            Unit = u;
            Country = (u.Side as Team).Country;
            Position = new FigurePresenter(u.Polygon, u);
            Name = u.Properties.Name;
            Abbreviation = Name; // todo correct this
            
            Update();
        }

        #endregion
        
        #region Dependency properties

        /// <summary>
        /// The original unit presented. It is a GameEngine entity.
        /// </summary>

        #region Unit

        public Unit Unit
        {
            get { return (Unit)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Unit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(Unit), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// The country of unit origin, i.e. the flag it uses.
        /// </summary>

        #region Country

        public Countries Country
        {
            get { return (Countries)GetValue(CountryProperty); }
            set { SetValue(CountryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Country.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountryProperty =
            DependencyProperty.Register("Country", typeof(Countries), typeof(UnitPresenter), new UIPropertyMetadata(Countries.NewZealand)); // why not New Zealand, eh?

        #endregion

        /// <summary>
        /// A boolean value showing whether the unit presented is on a current player's side
        /// </summary>

        #region IsMyUnit

        public bool IsMyUnit
        {
            get { return (bool)GetValue(IsMyUnitProperty); }
            set { SetValue(IsMyUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMyUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMyUnitProperty =
            DependencyProperty.Register("IsMyUnit", typeof(bool), typeof(UnitPresenter), new UIPropertyMetadata(false));

        #endregion

        /// <summary>
        /// Position property is a polygon showing unit placement on a map.
        /// </summary>
        /// <remarks>
        /// For now it is implemented in an extremely warped way
        /// </remarks>

        #region Position

        public FigurePresenter Position
        {
            get { return (FigurePresenter)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(FigurePresenter), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Unit name, long
        /// </summary>

        #region Name

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(UnitPresenter), new UIPropertyMetadata("Unnamed unit"));

        #endregion

        /// <summary>
        /// Unit name, short
        /// </summary>
        #region Abbreviation

        public string Abbreviation
        {
            get { return (string)GetValue(AbbreviationProperty); }
            set { SetValue(AbbreviationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Abbreviation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AbbreviationProperty =
            DependencyProperty.Register("Abbreviation", typeof(string), typeof(UnitPresenter), new UIPropertyMetadata("Unnamed"));

        #endregion

        #region UnitType

        /// <summary>
        /// Тип войск, к которым принадлежит отряд
        /// </summary>
        public string UnitType
        {
            get { return (string)GetValue(UnitTypeProperty); }
            set { SetValue(UnitTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnitType. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitTypeProperty =
            DependencyProperty.Register("UnitType", typeof(string), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        #region AllPersonnel

        /// <summary>
        /// Общая численность отряда
        /// </summary>
        public int AllPersonnel
        {
            get { return (int)GetValue(AllPersonnelProperty); }
            set { SetValue(AllPersonnelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Personnel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllPersonnelProperty =
            DependencyProperty.Register("AllPersonnel", typeof(int), typeof(UnitPresenter), new UIPropertyMetadata(0));

        #endregion

        /// <summary>
        /// A number of active men in a unit
        /// </summary>

        #region ActivePersonnel

        public int ActivePersonnel
        {
            get { return (int)GetValue(ActivePersonnelProperty); }
            set { SetValue(ActivePersonnelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Personnel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivePersonnelProperty =
            DependencyProperty.Register("ActivePersonnel", typeof(int), typeof(UnitPresenter), new UIPropertyMetadata(0));

        #endregion

        #region PercentageAliveAndAmmo

        //Доля живых членов отряда от его исходной численности
        public string PercentageAliveAndAmmo
        {
            get { return (string)GetValue(PercentageAliveAndAmmoProperty); }
            set { SetValue(PercentageAliveAndAmmoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Personnel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageAliveAndAmmoProperty =
            DependencyProperty.Register("PercentageAliveAndAmmo", typeof(string), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        #region Speed

        /// <summary>
        /// Текущая скорость отряда
        /// </summary>
        public string Speed
        {
            get { return (string)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Speed. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(string), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        #region WeaponsCount

        /// <summary>
        /// Number of heavy weapons items
        /// </summary>
        public int WeaponsCount
        {
            get { return (int)GetValue(WeaponsCountProperty); }
            set { SetValue(WeaponsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WeaponsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WeaponsCountProperty =
            DependencyProperty.Register("WeaponsCount", typeof(int), typeof(UnitPresenter), new UIPropertyMetadata(0));

        #endregion

        #region CaliberCount

        /// <summary>
        /// Список количества орудий по калибрам
        /// </summary>
        public List<int> CaliberCount
        {
            get { return (List<int>)GetValue(CaliberCountProperty); }
            set { SetValue(CaliberCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaliberCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaliberCountProperty =
            DependencyProperty.Register("CaliberCount", typeof(List<int>), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        #region DefendCaliber

        /// <summary>
        /// Защита по калибрам
        /// </summary>
        public List<int> DefendCaliber
        {
            get { return (List<int>)GetValue(DefendCaliberProperty); }
            set { SetValue(DefendCaliberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaliberCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefendCaliberProperty =
            DependencyProperty.Register("DefendCaliber", typeof(List<int>), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Total ammo count
        /// </summary>

        #region Ammo

        public int Ammo
        {
            get { return (int)GetValue(AmmoProperty); }
            set { SetValue(AmmoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ammo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AmmoProperty =
            DependencyProperty.Register("Ammo", typeof(int), typeof(UnitPresenter), new UIPropertyMetadata(0));

        #endregion

        /// <summary>
        /// Текущеее действие отряда
        /// </summary>

        #region CurrentAction

        public OrderPresenter CurrentAction
        {
            //get {return new OrderPresenter((Unit.CurrentAction as MoveAction).Origin, (Unit.CurrentAction as MoveAction).Destination, 0);
            get { return (OrderPresenter)GetValue(CurrentActionProperty); }
            set { SetValue(CurrentActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentAction. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentActionProperty =
            DependencyProperty.Register("CurrentAction", typeof(OrderPresenter), typeof(UnitPresenter), new UIPropertyMetadata(null));

        #endregion


        #endregion
        
        /// <summary>
        /// Функция обновления содержимого UnitPresenter,
        /// использует содержимое Features.cs, tm.cs
        /// </summary>
        internal void Update()
        {
            Position.Update();

            if (original.UnitType == GameEngine.Characters.UnitType.Infantry)
                UnitType = "Пехота";
            else if (original.UnitType == GameEngine.Characters.UnitType.Armor)
                UnitType = "Танковые войска";
            else if (original.UnitType == GameEngine.Characters.UnitType.Engineers)
                UnitType = "Инженерные войска";
            else if (original.UnitType == GameEngine.Characters.UnitType.Artillery)
                UnitType = "Артиллерия";
            else if (original.UnitType == GameEngine.Characters.UnitType.AirForce)
                UnitType = "Авиация";
            else if (original.UnitType == GameEngine.Characters.UnitType.Cavalry)
                UnitType = "Кавалерия";
            else if (original.UnitType == GameEngine.Characters.UnitType.Navy)
                UnitType = "Флот";

            if (original.AuthorizedStrength != 0)
                PercentageAliveAndAmmo = ((int)((double)original.Properties.AllCount() / original.AuthorizedStrength * 100)).ToString() + "%/" + ((int)((double)original.Properties.AmunitionCount() / original.AuthorizedAmmo * 100)).ToString() + "%";
            else PercentageAliveAndAmmo = "";
            //original
            AllPersonnel=original.Properties.AllCount();
            ActivePersonnel = original.Properties.ActiveCount();
            WeaponsCount = original.Properties.HeavyWeaponCount();
            Speed = (original.Properties.CurSpeed * 10).ToString() + " км/ч";
            CaliberCount = GetCaliberCount();
            DefendCaliber = GetCaliberDefend();
            Ammo = original.Properties.AmunitionCount();

            IsMyUnit = original.Side.TeamNumber == Game.PlayerTeam;

            //Передача данных для отображения действия
            if (original.CurrentAction != null && original.CurrentAction is MoveAction)
                CurrentAction = new OrderPresenter(original.CurrentAction, original.Position);
        }

        /// <summary>
        /// Считает количество орудий по калибрам
        /// </summary>
        /// <returns>Список количества орудий по калибрам</returns>
        internal List<int> GetCaliberCount()
        {
            int type0 = 0, type1 = 0, type2 = 0, type3 = 0, type4 = 0, type5 = 0;
            var Items = original.Properties.Items;

           //статистика типов оружия
          foreach (AbstractGameEngine.Item item in original.Properties.Items)
          {
              if (item is EquipmentMark)
              {
                  switch (((EquipmentMark)item).Weapon)
                  {
                      case Caliber.smallArms:
                          type0 += item.Count;
                          break;
                      case Caliber.largeSmallArms:
                          type1 += item.Count;
                          break;
                      case Caliber.small:
                          type2 += item.Count;
                          break;
                      case Caliber.medium:
                          type3 += item.Count;
                          break;
                      case Caliber.large:
                          type4 += item.Count;
                          break;
                      case Caliber.extraLarge:
                          type5 += item.Count;
                          break;
                  }
              }
          }
          return new List<int> { type0, type1, type2, type3, type4, type5 };
        }

        internal List<int> GetCaliberDefend()
        {
            int type0 = 0, type1 = 0, type2 = 0, type3 = 0, type4 = 0, type5 = 0;
            var Items = original.Properties.Items;

           //статистика типов оружия
          foreach (AbstractGameEngine.Item item in original.Properties.Items)
          {
              if (item is EquipmentMark)
              {
                  switch (((EquipmentMark)item).Armor)
                  {
                      case Caliber.smallArms:
                          type0 += item.Count;
                          break;
                      case Caliber.largeSmallArms:
                          type1 += item.Count;
                          break;
                      case Caliber.small:
                          type2 += item.Count;
                          break;
                      case Caliber.medium:
                          type3 += item.Count;
                          break;
                      case Caliber.large:
                          type4 += item.Count;
                          break;
                      case Caliber.extraLarge:
                          type5 += item.Count;
                          break;
                  }
              }
          }
          return new List<int> { type0, type1, type2, type3, type4, type5 };
        }
    }
}
