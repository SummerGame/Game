using System;
namespace AbstractGameEngine
{
  
    /// <summary>
    /// Абстрактный класс "Действие"
    /// Применяется для хранения параметризованных экземпляров Абстрактного Класса "Действие"
    /// </summary>
    public abstract class Action
    {
        private bool completed;
        private double time;

        // Добавлено А.Кругликовым для нужд отображения действий в интерфейсе
        public Type Type
        {
            get { return GetType(); }
        }

        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        public double Time
        {
            get { return time; }
            set { time = value; }
        }
    }

    /// <summary>
    /// Абстрактный параметрический класс "Действие"
    /// Применяется, как исполняемое действие в модели игры.
    /// Шаблон для реализации конкретных классов действий.
    /// </summary>
    /// <typeparam name="A">Параметр для задания любого воздействующего объекта</typeparam>
    public abstract class Action<A> : Action
    {

        #region Methods

        //TODO: comment
        public abstract object Do(A adm);

        /// <summary>
        /// Изменяет состояние войска через каждый интервал игрового времени
        /// </summary>
        /// <param name="adm">Войско</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public abstract object Now(A adm, double time);

        #endregion

    }
}
