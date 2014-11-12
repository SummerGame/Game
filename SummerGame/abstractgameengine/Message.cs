namespace AbstractGameEngine
{
    /// <summary>
    /// Абстрактный Класс "Сообщение"
    /// Предполагает, что в игре все Administrant могут передавать друг другу сообщения.
    /// </summary>
    abstract class Message
    {
        #region Fields

        /// <summary>
        /// Задание, которое необходимо выполнить.
        /// </summary>
        private Action task;

        #endregion  

        #region Properties
        /// <summary>
        /// Тот, кто отдает поручение.
        /// </summary>
        public Administrant from { get; set; } 

        /// <summary>
        /// Тот, кто принимает поручение.
        /// </summary>
        public Administrant to { get; set; } // Кому

        #endregion

    }
}
