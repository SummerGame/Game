namespace GameEngine.Characters
{
    #region Enums

    /// <summary>
    /// Перечислимый тип - Ранг (Звание)
    /// </summary>
    public enum Rank
    {
        Ordinary,
        Sergeant,
        Officer
    }

    /// <summary>
    /// Перечислимый тип - Специализация (Профессия)
    /// </summary>
    public enum Specialization
    {
        InfantryMan,
        TankMan,
        EngineerMan
    }

    /// <summary>
    /// Перечислимый тип - Квалификация (Умение)
    /// </summary>
    public enum Qualification
    {
        low = 10,
        medium = 12,
        high = 15,
    }

    /// <summary>
    /// Перечислимый тип - Состояние здоровья
    /// </summary>
    public enum Vitality
    {
        healthy = 10,
        wounded = 7,
        heavilyWounded = 4,
        dead = 0
    }

    #endregion

    /// <summary>
    /// Группа - неделимый элемент Unit'a, объединенный по общиму признаку.
    /// </summary>
    public class Group
    {
        #region Properties

        /// <summary>
        /// Изначальный размер группы
        /// </summary>
        public int OriginalCount { get; set; }
        public int Count { get; set; }
        public Rank Rank { get; set; }
        public Specialization Specialization { get; set; }
        public Qualification Qualification { get; set; }
        public Qualification Experience { get; set; }
        public Vitality Vitality { get; set; }

        #endregion

        #region Contructors

        public Group(int count, Rank rank, Specialization specialization, Qualification qualification, Qualification experience, Vitality vitality)
        {
            OriginalCount = count;
            Count = count;
            Rank = rank;
            Specialization = specialization;
            Qualification = qualification;
            Experience = experience;
            Vitality = vitality;
        }

        public Group(int count, Specialization specialization)
        {
            Count = count;
            Rank = Rank.Ordinary;
            Specialization = specialization;
            Qualification = Qualification.low;
            Experience = Qualification.low;
            Vitality = Vitality.healthy;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод возвращающий эффективность Группы
        /// </summary>
        /// <returns>Коэффицент эффективности</returns>
        public double GetEfficiency()
        {
            return ((double)Qualification * 0.6 + (double)Experience * 0.4) / 10;
        }

        /// <summary>
        /// Считает количество танкистов
        /// </summary>
        /// <returns>Количество танкистов</returns>
        public int GetTanksMan()
        {
            if (Specialization == Specialization.TankMan) return Count;
            else return 0;
        }

        /// <summary>
        /// Считает количество пехотинцев.
        /// </summary>
        /// <returns>Количество пехотинцев</returns>
        public int GetWalking()
        {
            if ((Specialization == Specialization.InfantryMan) && (Vitality == Vitality.healthy))
                return Count;
            else return 0;
        }

        /// <summary>
        /// Считает колчичество работоспособных инженеров в группе.
        /// </summary>
        /// <returns>Количество инженеров</returns>
        public int GetEngineers()
        {
            if ((Specialization == Specialization.EngineerMan) && (Vitality == Vitality.healthy))
                return Count;
            else return 0;
        }

        #endregion
    }
}
