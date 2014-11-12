using System.Collections.Generic;
using AbstractGameEngine;
using GameEngine.Object;

namespace GameEngine.Characters
{
    /// <summary>
    /// Класс "Особенности" - Свойства "Юнита"
    /// Конкретный Класс, который реализует основные свойства групп Юнита и самого Юнита.
    /// </summary>
    public class UnitFeatures : UnitProperties
    {
        #region Properties
        
        public List<Group> Groups { get; set; }
        public List<Item> Items { get; set; }
        public double Visible { get; set; }
        public double CurVisible { get; set; }
        public double Speed { get; set; }
        public double CurSpeed { get; set; }
        public string Name { get; set; }

        #endregion

        #region Constructors

        public UnitFeatures(List<Group> group, List<Item> items)
        {
            Groups = group;
            Items = items;
            Speed = ActiveInfantryCount() > 0 ? 0.35 : 5;
            Visible = 0.5;
            CurSpeed = Speed;
            CurVisible = Visible;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Считает количество активной техники в Unit'e.
        /// </summary>
        /// <returns>Количество активной тяжелой техники</returns>
        public int HeavyWeaponCount()
        {
            int tanksmans = 0;
            int needtanksmans = 0;
            int tanks = 0;
            foreach (var gro in Groups)
            {
                tanksmans += gro.GetTanksMan();
            }
            foreach (var item in Items)
            {
                if (item is EquipmentMark)
                {
                    if (((EquipmentMark)item).Crew > 1)
                    {
                        tanks += item.Count;
                        needtanksmans += ((EquipmentMark)item).Crew;
                    }
                }
            }
            if (needtanksmans > tanksmans) return tanksmans / 3;
            else return tanks;
            //var tanks = Items.Where(x => x is EquipmentMark).Count();
            //return tanks / tanksmans;

        }

        /// <summary>
        /// Считает количество активных пехотинцев.
        /// </summary>
        /// <returns>Количество пехотинцев</returns>
        public int ActiveInfantryCount()
        {
            int infanery = 0;
            foreach (var gro in Groups)
            {
                infanery += gro.GetWalking();
            }
            return infanery;
        }

        /// <summary>
        /// Считает количество активных инженеров в Unit'e
        /// </summary>
        /// <returns>Количество инжинеров</returns>
        public int ActiveEngineerCount()
        {
            int engineers = 0;
            foreach (var gro in Groups)
            {
                engineers += gro.GetEngineers();
            }
            return engineers;
        }


        /// <summary>
        /// Считает количество всех людей в отряде.
        /// </summary>
        /// <returns>Общая численность отряда</returns>
        public int AllCount()
        {
            int allMan = 0;
            foreach (var gro in Groups)
            {
                allMan+=gro.Count;
            }
            return allMan;
        }

        /// <summary>
        /// Считает количество дееспособных людей.
        /// </summary>
        /// <returns>Количество дееспособных людей</returns>
        public int ActiveCount()
        {
            int activeMan = 0;
            foreach (var gro in Groups)
            {
                activeMan += gro.GetWalking();
                activeMan += gro.GetTanksMan();
                activeMan += gro.GetEngineers();
            }
            return activeMan;
        }

        /// <summary>
        /// Возвращает количество танкистов, способных принимать участие в боевых действиях
        /// </summary>
        /// <returns></returns>
        public int ActiveTanksManCount()
        {
            int tankMan = 0;
            foreach (var gro in Groups)
            {
                tankMan += gro.GetTanksMan();
            }
            return tankMan;
        }
        #region Old

        //public List<double> UnitVitality()
        //{
        //    double type0 = 0, type1 = 0, type2 = 0, type3 = 0, type4 = 0, type5 = 0;
        //    int infantry = ActiveInfantryCount();
        //    int curInfantry = 0;
        //    int heavyWeap = HeavyWeaponCount();
        //    int curHeavyWeap = 0;

        //    foreach (var item in Items)
        //    {
        //        if (item is EquipmentMark)
        //        {
        //            if (((EquipmentMark)item).Armor == Caliber.smallArms)
        //            {
        //                if (curInfantry < infantry)
        //                {
        //                    type0 += item.Count;
        //                    curInfantry += item.Count;
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.largeSmallArms)
        //            {
        //                if (curInfantry < infantry)
        //                {
        //                    type1 += item.Count;
        //                    curInfantry += item.Count;
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.small)
        //            {
        //                if (curHeavyWeap < heavyWeap)
        //                {
        //                    type2 += item.Count;
        //                    curHeavyWeap += item.Count;
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.medium)
        //            {
        //                if (curHeavyWeap < heavyWeap)
        //                {
        //                    type3 += item.Count;
        //                    curHeavyWeap += item.Count;
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.large)
        //            {
        //                if (curHeavyWeap < heavyWeap)
        //                {
        //                    type4 += item.Count;
        //                    curHeavyWeap += item.Count;
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.extraLarge)
        //            {
        //                if (curHeavyWeap < heavyWeap)
        //                {
        //                    type5 += item.Count;
        //                    curHeavyWeap += item.Count;
        //                }
        //            }
        //        }
        //    }
        //    return new List<double> { type0, type1, type2, type3, type4, type5 };
        //}

        //private void DamageTaken(List<double> damage)
        //{
        //    //var damage = DamageDealt();
        //    var vitality = UnitVitality();
        //    var newCountVit = vitality;
        //    double overDamage = 0;
        //    for (int i = 5; i > -1; i--)
        //    {
        //        if (damage[i] + overDamage > 0)
        //        {
        //            if (damage[i] + overDamage > vitality[i])
        //            {
        //                newCountVit[i] = 0;
        //                overDamage += damage[i] + overDamage - newCountVit[i];
        //            }
        //            else
        //            {
        //                newCountVit[i] -= damage[i] + overDamage;
        //                overDamage = 0;
        //            }
        //        }
        //    }
        //    var newGroups = new List<Group>();
        //    var newInfantry = newCountVit[0] + newCountVit[1];
        //    newGroups.Add(new Group((int)newInfantry, Specialization.infantry));
        //    foreach (var item in Items)
        //    {
        //        if (item is EquipmentMark)
        //        {
        //            if (((EquipmentMark)item).Armor == Caliber.smallArms)
        //            {
        //                if (item.Count > newCountVit[0])
        //                {
        //                    item.Count -= (int)newCountVit[0];
        //                    newCountVit[0] = 0;
        //                }
        //                else
        //                {
        //                    item.Count = 0;
        //                    newCountVit[0] = item.Count - newCountVit[0];
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.largeSmallArms)
        //            {
        //                if (item.Count > newCountVit[1])
        //                {
        //                    item.Count -= (int)newCountVit[1];
        //                    newCountVit[1] = 0;
        //                }
        //                else
        //                {
        //                    item.Count = 0;
        //                    newCountVit[1] = item.Count - newCountVit[1];
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.small)
        //            {
        //                if (item.Count > newCountVit[2])
        //                {
        //                    item.Count -= (int)newCountVit[2];
        //                    newCountVit[2] = 0;
        //                }
        //                else
        //                {
        //                    item.Count = 0;
        //                    newCountVit[2] = item.Count - newCountVit[2];
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.medium)
        //            {
        //                if (item.Count > newCountVit[3])
        //                {
        //                    item.Count -= (int)newCountVit[3];
        //                    newCountVit[3] = 0;
        //                }
        //                else
        //                {
        //                    item.Count = 0;
        //                    newCountVit[3] = item.Count - newCountVit[3];
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.large)
        //            {
        //                if (item.Count > newCountVit[4])
        //                {
        //                    item.Count -= (int)newCountVit[4];
        //                    newCountVit[4] = 0;
        //                }
        //                else
        //                {
        //                    item.Count = 0;
        //                    newCountVit[4] = item.Count - newCountVit[4];
        //                }
        //            }
        //            if (((EquipmentMark)item).Armor == Caliber.extraLarge)
        //            {
        //                if (item.Count > newCountVit[5])
        //                {
        //                    item.Count -= (int)newCountVit[5];
        //                    newCountVit[5] = 0;
        //                }
        //                else
        //                {
        //                    item.Count = 0;
        //                    newCountVit[5] = item.Count - newCountVit[5];
        //                }
        //            }
        //        }
        //    }

        //}

        #endregion

        /// <summary>
        /// Возвращает количество боезопаса.
        /// </summary>
        /// <returns></returns>
        public int AmunitionCount()
        {
            var curAmmunition = 0;
            foreach (var item in Items)
            {
                if (item is Goods)
                {
                    if ((item as Goods).ItemType == ObjectType.Ammunition) curAmmunition += item.Count;
                }
            }
            //if (curAmmunition < 0) return 0;
            return curAmmunition;
        }

        /// <summary>
        /// Считает количество произведенных выстрелов, каждого типа урона.
        /// </summary>
        /// <returns>Считает список количества выстрелов по типу</returns>
        internal List<double> ShotsCount()
        {
            double type0 = 0, type1 = 0, type2 = 0, type3 = 0, type4 = 0, type5 = 0, amountShots = 0;
            int infantry = ActiveInfantryCount();
            int curInfantry = 0;
            int heavyWeap = HeavyWeaponCount();
            int curHeavyWeap = 0;
            int curAmmunition = 0;

            foreach (var item in Items)
            {
                if (item is Goods)
                {
                    if ((item as Goods).ItemType == ObjectType.Ammunition) curAmmunition += item.Count;
                }
            }
            amountShots += curAmmunition;
            foreach (Item item in Items)
            {
                if (item is EquipmentMark)
                {
                    if (((EquipmentMark)item).Weapon == Caliber.smallArms)
                    {
                        if ((curInfantry < infantry) && (curAmmunition > 0))
                        {
                            type0 += item.Count * ((EquipmentMark)item).FireRate;
                            curInfantry += item.Count;
                            curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                        }
                    }
                    if (((EquipmentMark)item).Weapon == Caliber.largeSmallArms)
                    {
                        if ((((EquipmentMark) item).Crew > 1)&& (curAmmunition > 0))
                        {
                            if (curHeavyWeap < heavyWeap)
                            {
                                type2 += item.Count * ((EquipmentMark)item).FireRate;
                                curHeavyWeap += item.Count;
                                curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                            }
                        }
                        else if (curInfantry < infantry)
                        {
                            type1 += item.Count * ((EquipmentMark)item).FireRate;
                            curInfantry += item.Count;
                            curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                        }
                    }
                    if (((EquipmentMark)item).Weapon == Caliber.small)
                    {
                        if ((curHeavyWeap < heavyWeap)&& (curAmmunition > 0))
                        {
                            type2 += item.Count * ((EquipmentMark)item).FireRate;
                            curHeavyWeap += item.Count;
                            curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                        }
                    }
                    if (((EquipmentMark)item).Weapon == Caliber.medium)
                    {
                        if ((curHeavyWeap < heavyWeap)&& (curAmmunition > 0))
                        {
                            type3 += item.Count * ((EquipmentMark)item).FireRate;
                            curHeavyWeap += item.Count;
                            curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                        }
                    }
                    if (((EquipmentMark)item).Weapon == Caliber.large)
                    {
                        if ((curHeavyWeap < heavyWeap)&& (curAmmunition > 0))
                        {
                            type4 += item.Count * ((EquipmentMark)item).FireRate;
                            curHeavyWeap += item.Count;
                            curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                        }
                    }
                    if (((EquipmentMark)item).Weapon == Caliber.extraLarge)
                    {
                        if ((curHeavyWeap < heavyWeap)&& (curAmmunition > 0))
                        {
                            type5 += item.Count * ((EquipmentMark)item).FireRate;
                            curHeavyWeap += item.Count;
                            curAmmunition -= (int)(item.Count * ((EquipmentMark)item).FireRate);
                        }
                    }
                }
            }
            if (curAmmunition < 0) curAmmunition = 0;
            foreach (var item in Items)
            {
                if (item is Goods)
                {
                    if ((item as Goods).ItemType == ObjectType.Ammunition)
                    {
                        // TODO переделать при использовании разной амуниции и хранении её в разных итемах
                        item.Count = curAmmunition;
                    }
                }
            }
            amountShots -= curAmmunition;
            return new List<double> { type0, type1, type2, type3, type4, type5, amountShots };
        }



        /// <summary>
        /// Формирование структуры, для формул получения урона
        /// </summary>
        /// <returns>Список классов SameType</returns>
        internal List<SameType> TheSames()
        {
            //var accuracy = 0.05;
            var same0 = new SameType(); var same1 = new SameType(); var same2 = new SameType();
            var same3 = new SameType(); var same4 = new SameType(); var same5 = new SameType();
            int infantry = ActiveInfantryCount();
            int curInfantry = 0;
            int heavyWeap = HeavyWeaponCount();
            int curHeavyWeap = 0;

            foreach (var item in Items)
            {
                if (item is EquipmentMark)
                {
                    if (((EquipmentMark)item).Armor == Caliber.smallArms)
                    {
                        if (curInfantry < infantry)
                        {
                            same0.Count += item.Count;
                            //same0.Damage += item.Count  * ((EquipmentMark)item).FireRate * accuracy;
                            same0.UnitType.Add(item);
                            curInfantry += item.Count;
                        }
                    }
                    if (((EquipmentMark)item).Armor == Caliber.largeSmallArms)
                    {
                        //if (curInfantry < infantry) 
                        //{
                        //    same1.Count += item.Count;
                        //    //same1.Damage += item.Count * ((EquipmentMark)item).FireRate * accuracy;
                        //    same1.UnitType.Add(item);
                        //    curInfantry += item.Count;
                        //}
                        //TODO тяжелые пулеметы не учитываются, хотя вроде они и не сюда.
                        if ((curHeavyWeap < heavyWeap) && (((EquipmentMark)item).Crew > 1))
                        {
                            same1.Count += item.Count;
                            //same2.Damage += ((EquipmentMark)item).FireRate * accuracy;
                            same1.UnitType.Add(item);
                            curHeavyWeap += item.Count;
                        }
                    }
                    if (((EquipmentMark)item).Armor == Caliber.small)
                    {
                        if (curHeavyWeap < heavyWeap)
                        {
                            same2.Count += item.Count;
                            //same2.Damage += ((EquipmentMark)item).FireRate * accuracy;
                            same2.UnitType.Add(item);
                            curHeavyWeap += item.Count;
                        }
                    }
                    if (((EquipmentMark)item).Armor == Caliber.medium)
                    {
                        if (curHeavyWeap < heavyWeap)
                        {
                            same3.Count += item.Count;
                            //same3.Damage += ((EquipmentMark)item).FireRate * accuracy;
                            same3.UnitType.Add(item);
                            curHeavyWeap += item.Count;
                        }
                    }
                    if (((EquipmentMark)item).Armor == Caliber.large)
                    {
                        if (curHeavyWeap < heavyWeap)
                        {
                            same4.Count += item.Count;
                            //same4.Damage += ((EquipmentMark)item).FireRate * accuracy;
                            same4.UnitType.Add(item);
                            curHeavyWeap += item.Count;
                        }
                    }
                    if (((EquipmentMark)item).Armor == Caliber.extraLarge)
                    {
                        if (curHeavyWeap < heavyWeap)
                        {
                            same5.Count += item.Count;
                            //same5.Damage += ((EquipmentMark)item).FireRate * accuracy;
                            same5.UnitType.Add(item);
                            curHeavyWeap += item.Count;
                        }
                    }
                }
            }
            var sames = new List<SameType>() { same0, same1, same2, same3, same4, same5 };
            foreach (var same in sames)
            {
                var temp = new List<double>();
                foreach (var probType in same0.UnitType)
                {
                    if (same.Count == 0) temp.Add(0);
                    else
                    {
                        temp.Add(probType.Count / same.Count);
                    }
                }
                same.Probabilities = temp;
            }


            return sames;
        }

        /// <summary>
        /// Распределяет полученный урон по Unit'у.
        /// </summary>
        /// <param name="damage">Cписок полученного урона</param>
        public int TakeDamage(List<double> damage)
        {
            var before = ActiveCount();
            var sameTypeDif = TheSames();
            var infanties = ActiveInfantryCount();
            double overDamage = 0;
            var alive = new List<double>();
            var dead = new List<double>() { 0, 0, 0, 0, 0, 0 };
            foreach (var sameType in sameTypeDif)
            {
                alive.Add(sameType.Count);
            }

            for (int i = 5; i > -1; i--) // Считаем количество выживших и погибших
            {
                if (damage[i] + overDamage > 0)
                {
                    if (damage[i] + overDamage > sameTypeDif[i].Count)
                    {
                        dead[i] += sameTypeDif[i].Count;
                        overDamage += damage[i] + overDamage - alive[i];
                        alive[i] = 0;
                        sameTypeDif[i].Count = 0;
                    }
                    else
                    {
                        alive[i] -= damage[i] + overDamage;
                        dead[i] += damage[i] + overDamage;
                        overDamage = 0;
                    }
                }
            }
            var newListItems = new List<Item>();
            var newGroups = new List<Group>();

            foreach (var item in Items) // добавляем небоевые предметы (медикаменты, аммуницию etc)
            {
                if (!(item is EquipmentMark))
                {
                    newListItems.Add(item);
                }
            }
            var tanksMansCount = 0;

            if (infanties - dead[0] > 0) newGroups.Add(new Group((int)(infanties - dead[0]), Specialization.InfantryMan)); // Добавляем выжившие число пехотинцев


            for (int i = 1; i < 6; i++) // Пересичтываем список TheSame, дабы сформировать ровное распределение
            // TODO сделать корректно
            {
                if (alive[i] != 0)
                {
                    while (sameTypeDif[i].Count > alive[i])
                    {
                        foreach (var sameType in sameTypeDif[i].UnitType)
                        {
                            if ((sameType.Count > 0) && (sameTypeDif[i].Count > alive[i]))
                            {
                                sameType.Count -= 1;
                                sameTypeDif[i].Count -= 1;
                            }
                        }
                    }
                }
            }

            foreach (SameType sameType in sameTypeDif) // Добавляем в новое состояние технику, и считаем количество выживших танкистов
            {
                if (sameType.Count != 0)
                {
                    foreach (var items in sameType.UnitType)
                    {
                        if (items.Count != 0)
                        {
                            newListItems.Add(items);
                            if ((items as EquipmentMark).Crew > 1) tanksMansCount += items.Count * (items as EquipmentMark).Crew;
                        }
                    }
                }
            }
            newGroups.Add(new Group(tanksMansCount, Specialization.TankMan));
            Groups = newGroups;
            Items = newListItems;
            //var itog = 0;
            //foreach (var deadType in dead)
            //{
            //    itog += (int)deadType;
            //}
            return  before - ActiveCount();
        }

        #endregion
 
    }

    /// <summary>
    /// Вспомогательный класс, хранит необходимые параметры для расчетов.
    /// </summary>
    internal class SameType
    {
        #region Properties

        public int Count { get; set; }
        public List<Item> UnitType { get; set; }
        public List<double> Probabilities { get; set; }

        #endregion

        #region Contructors

        public SameType(int count, List<Item> unittypes)
        {
            Count = count;
            UnitType = unittypes;
            var prorab = new List<double>();
            foreach (var probType in UnitType)
            {
                prorab.Add(probType.Count / Count);
            }
            Probabilities = prorab;
        }

        public SameType()
        {
            Count = 0;
            UnitType = new List<Item>();
            Probabilities = new List<double>();
        }

        #endregion
    }
}
