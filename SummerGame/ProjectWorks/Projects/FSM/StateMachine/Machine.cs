using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;

namespace StateMachine
{


    /// <summary>
    /// Автомат
    /// Состоит из списка состояний, списка функий перехода и начального состояния автомата.
    /// </summary>
    public class Machine
    {


        #region Fields
        /// <summary>
        /// Список состояний автомата.
        /// </summary>
        private ObservableCollection<State> states;

        /// <summary>
        /// Список функций перехода
        /// </summary>
        private ObservableCollection<Function> functions;

        /// <summary>
        /// Начальное состояние автомата.
        /// </summary>
        private State firstState;


        /// <summary>
        /// Распознаваемый автоматом алфавит.
        /// </summary>
        private Alphabet alphabet;



        #endregion

        #region Properties

        /// <summary>
        /// Список состояний автомата.
        /// </summary>
        public ObservableCollection<State> States
        {
            get { return states; }
            set { states = value; }
        }

        /// <summary>
        /// Список функций перехода
        /// </summary>
        public ObservableCollection<Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        /// <summary>
        /// Начальное состояние автомата.
        /// </summary>
        public State FirstState
        {
            get { return firstState; }
            set { firstState = value; }
        }

        /// <summary>
        /// Распознаваемый автоматом алфавит.
        /// </summary>
        public Alphabet Alphabet
        {
            get { return alphabet; }
            set { alphabet = value; }
        }

        #endregion

        #region Methods

        //TODO прикрутить к проверкам функций алфавит.


        /// <summary>
        /// Добавление нового состояния.
        /// Добавление просиходит только тогда, когда такого состояния в автомате нет.
        /// </summary>
        /// <param name="newState">Состояние, которое мы хотим добавить</param>
        public void AddNewState(State newState)
        {
            if (newState.Name.Length > 0)
            {
                foreach (State state in States)
                {
                    if (state == newState) throw new ArgumentException("Такое состояние уже есть!");
                }
                States.Add(newState);
            }
            else
            {
                throw new ArgumentException("Таконе название не допустимо!");
            }
        }


        /// <summary>
        /// Удаляет недостижимые состояни и неисользуемые буквы алфавита
        /// </summary>
        public void ClearMachine()
        {
            List<State> removeStates = new List<State>(); //Список недостижимых состояних
            List<Letter> removeLetters = new List<Letter>(); // список недостижимых букв

            //Ищем такие состояния, что они не являются ни начальной вершиной какой либо функции, ни конечной
            foreach (State state in States)
            {
                bool exist = false;
                foreach (Function function in Functions)
                {
                    if ((function.From == state) || (function.To == state)) exist = true;
                }
                if (!exist) removeStates.Add(state); // Все такие вершины добавляем в список
            }
            // Проверяет все буквы алфавита, и если буква не используется ниодной функцией, то она идет под удоление
            foreach (Letter letter in Alphabet.Letters)
            {
                bool exist = false;
                foreach (Function function in Functions)
                {
                    if (function.Letter == letter) exist = true;
                }
                if (!exist) removeLetters.Add(letter);
            }
            //Удоляем
            foreach (State state in removeStates)
            {
                States.Remove(state);
            }
            foreach (Letter letter in removeLetters)
            {
                Alphabet.Letters.Remove(letter);
            }
        }

        //Проверяем слово на распознаваемость
        public bool CheckWord(List<Letter> word)
        {
            State currentState = new State();
            currentState = FirstState; // Начинаем поиск с 1ой вершины
            try
            {
                
                // Проверяем, есть ли буквы слова в алфавите автомата
                if (CanRead(word)) 
                {
                    
                    foreach (Letter letter in word) //Идем по всем буквам слова
                    {
                        bool temp = true;
                        foreach (Function function in Functions) // Прверяем, куда мы можем уйти
                        {
                            // Если из текущего состояния, по букве слова мы можем перейти
                            if ((function.From == currentState) && (function.Letter == letter) && (temp))
                            {
                                // то перехоим
                                temp = false;
                                currentState = function.To; // меняем состояние
                            }

                        }
                        if (temp) return false; // Нету перехода из состояния по букве слова
                    }
                }
                else return false; // нет нужных букв в алфавите
            }
            catch (Exception)
            {
                return false;
            }
            return currentState.IsLast; // Если не вытели раньше, то сово распознано
        }

        /// <summary>
        /// Добавление новой буквы.
        /// Добавление просиходит только тогда, когда такой буквы в алфавите нет.
        /// </summary>
        /// <param name="newState">Состояние, которое мы хотим добавить</param>
        public void AddNewLetter(Letter letter)
        {
            if (letter.Name.Length > 0)
            {
                foreach (Letter lett in Alphabet.Letters)
                {
                    if (lett == letter) throw new ArgumentException("Такая буква уже есть!");
                }
                Alphabet.Letters.Add(letter);
            }
            else
            {
                throw new ArgumentException("Таконе название не допустимо!");
            }
        }

        /// <summary>
        /// Добавление новой функции перехода.
        /// Добавление происходит только тогда, когда такая функиця перехода ещё не добавлена.
        /// </summary>
        /// <param name="newFunction">Функция перехода, которую мы хотим добавить</param>
        public void AddNewFunction(Function newFunction)
        {
            if (newFunction.Name.Length > 0)
            {
                foreach (Function function in functions)
                {
                    if (function == newFunction) throw new ArgumentException("Такая функция уже есть!");
                }
                Functions.Add(newFunction);
            }
            else
            {
                throw new ArgumentException("Такое название не допустимо!");
            }
        }

        /// <summary>
        /// Провереяет, являются ли все буквы слова допустимыми.
        /// </summary>
        /// <param name="word">Слово</param>
        /// <returns></returns>
        public bool CanRead(List<Letter> word)
        {
            return alphabet.CanRead(word);
        }

        ///// <summary>
        ///// Проверка автомата на детерминированность.
        ///// </summary>
        ///// <returns></returns>
        //public bool IsDet()
        //{
        //    foreach (State state in states)
        //    {
        //        List<State> tempStates = new List<State>();
        //        foreach (Function function in functions)
        //        {
        //            if (function.From == state)
        //            {
        //                if (tempStates.Contains(function.To)) return false;
        //                else
        //                {
        //                    tempStates.Add(function.To);
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}

        public Machine GetMachine()
        {
            return this;
        }
        /// <summary>
        /// Проверка автомата на детерминированность.
        /// </summary>
        /// <returns></returns>
        public bool IsDet()
        {
            if (alphabet.Letters.Count != 0) // если алфавит не пустой
            {
                foreach (State state in states)
                {
                    // для каждой вершины составляем список букв, по которым возможен переход в ругое состояние
                    Alphabet tempAlphabet = new Alphabet();
                    foreach (Function function in functions)
                    {
                        if (function.From == state)
                        {
                            if (tempAlphabet.Contains(function.Letter)) return false; // Если из состояния уже есть переход по данной букве
                            else
                            {
                                tempAlphabet.AddNewLetter(function.Letter);
                            }
                        }
                    }
                    //if (tempAlphabet.Letters.Count != alphabet.Letters.Count) return false;
                }
                return true;
            }
            else return false;
        }



        public void removeState(State state)
        {
            for (int i = 0; i < functions.Count; i++)
            {
                if ((functions[i].From == state) || (functions[i].To == state))
                {
                    removeFunction(functions[i]);
                    i--;
                }

            }
            states.Remove(state);
        }

        public void removeFunction(Function function)
        {
            functions.Remove(function);
        }

        public void removeLetter(Letter letter)
        {
            for (int i = 0; i < functions.Count; i++)
            {
                if (functions[i].Letter == letter)
                {
                    removeFunction(functions[i]);
                    i--;
                }

            }
            alphabet.Letters.Remove(letter);
        }

        /// <summary>
        /// Составление языка автомата
        /// </summary>
        /// <param name="state"></param>
        /// <param name="word"></param>
        /// <param name="way"></param>
        /// <returns></returns>
 
        private List<List<Letter>> Language(State state, List<Letter> word, List<State> way)
        {
            way.Add(state); // Составляем путь
            //State currentState = new State();
            //currentState = FirstState;
            List<List<Letter>> itog = new List<List<Letter>>();
            if (MyMath.getCount(way, state) > 1) // если мы гдето зациклелись
            {
                // Делаем бесконечный цикл (внешний вид)
                var index = MyMath.getPos(word, way); 
                //word.RemoveAt(word.Count-1);
                word.Insert(index, new Letter("(", ""));
                word.Add(new Letter(")*    как база + ещё", ""));
                itog.Add(word);
                way.RemoveAt(way.Count-1);
            }
            else
            {


                if (state.IsLast) // если мы уже получили распознаваемое слово
                {
                    if (word.Count == 0) itog.Add(new List<Letter>() { new Letter("epsilon", "") });
                    else itog.Add(word);
                }
                foreach (Function function in functions)
                {
                    // Смотрим, куда можно пойти
                    if (function.From == state)
                    {


                        var newWord = new List<Letter>();
                        foreach (Letter letter in word)
                        {
                            newWord.Add(letter);
                        }
                        newWord.Add(function.Letter); // Создаем копию слова
                        //
                        var d = Language(function.To, newWord, way); // рекурсивно вызываем
                        //itog.AddRange(d);
                        foreach (List<Letter> a in d)
                        {
                            // добавляем все слова, полученные рекурсивно
                            if (!itog.Contains(a)) itog.Add(a);
                        }
                    }
                }
                way.RemoveAt(way.Count - 1); // поднимаемся на уровень вверх
            }
            
            return itog;
        }


        public List<String> GetLanguage(State state, List<Letter> word, List<State> way)
        {
            List<string> list = new List<string>();
            var a = Language(state, word, way);

            foreach (List<Letter> someWord in a)
            {
                var wordic = "";
                foreach (Letter letter in someWord)
                {
                    wordic += letter.Name + " ";
                }
                if (!list.Contains(wordic)) list.Add(wordic);
            }
            return list;
        }
        #endregion

        #region Constructors

        public Machine()
        {
            this.states = new ObservableCollection<State>();
            this.functions = new ObservableCollection<Function>();
            this.firstState = new State();
            this.alphabet = new Alphabet();
        }

        public Machine(ObservableCollection<State> states, ObservableCollection<Function> functions, State state, Alphabet alphabet)
        {
            this.firstState = state;
            this.states = states;
            this.functions = functions;
            this.Alphabet = alphabet;
        }

        #endregion
    }

    /// <summary>
    /// Класс вспомогательных функций
    /// </summary>
    public static class MyMath
    {
        /// <summary>
        /// Возвращает позицию первого вхождения конечной вершины пути
        /// </summary>
        /// <param name="word"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        public static int getPos(List<Letter> word, List<State> way)
        {
            //var a = way.IndexOf(way.Last());
            var index = MyMath.find(way, way.Last());
            var b = word[index];
            return index;
            return word.IndexOf(b);
        }

        /// <summary>
        /// Считает количество вхождений вершины в пути
        /// </summary>
        /// <param name="states"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int getCount(List<State> states, State state)
        {
            int count = 0;
            foreach (State fstate in states)
            {
                if (state == fstate) count++;
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="way"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static int find(List<State> way, State state)
        {
            for (int i = 0; i < way.Count; i++)
            {
                if (way[i] == state) return i;
            }
            return -1;
        }
    }

    /// <summary>
    /// Состояние в автомате.
    /// Состоит из названия и метки, является ли данная вершина конечной для автомата.
    /// </summary>
    public class State
    {
        #region Fields

        /// <summary>
        /// Название состояния
        /// </summary>
        private string name;

        /// <summary>
        /// Является ли это состояние конечным для автомата.
        /// </summary>
        private bool isLast;

        #endregion

        #region Constructors

        public State(string name, bool isLast)
        {
            this.Name = name;
            this.IsLast = isLast;
        }

        public State()
        {
            this.Name = "";
            this.IsLast = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Название состояния
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Является ли это состояние конечным для автомата.
        /// </summary>
        public bool IsLast
        {
            get { return isLast; }
            set { isLast = value; }
        }

        #endregion

        #region Methods

        public static bool operator ==(State a, State b)
        {
            return (a.name == b.name);
        }

        public static bool operator !=(State a, State b)
        {
            return (!(a == b));
        }

        #endregion
    }


    /// <summary>
    /// Функция перехода
    /// Состоит из названия, состояния откуда будет проиходить переход и нового состояния автомата после перехода.
    /// </summary>
    public class Function
    {

        #region Fields

        /// <summary>
        /// Название данной функции перехода 
        /// </summary>
        private string name;

        /// <summary>
        /// От какого состояния идет переход.
        /// </summary>
        private State from;

        /// <summary>
        /// К какому состоянию идет переход.
        /// </summary>
        private State to;

        /// <summary>
        /// Буква, по которой происходит переход.
        /// </summary>
        private Letter letter;

        #endregion

        #region Properties

        /// <summary>
        /// Название данной функции перехода 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// От какого состояния идет переход.
        /// </summary>
        public State From
        {
            get { return @from; }
            set { @from = value; }
        }

        /// <summary>
        /// К какому состоянию идет переход.
        /// </summary>
        public State To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// Буква, по которой происходит переход.
        /// </summary>
        public Letter Letter
        {
            get { return letter; }
            set { letter = value; }
        }

        #endregion

        #region Constructors

        public Function()
        {
            this.name = "";
            this.from = new State();
            this.to = new State();
            this.letter = new Letter();
        }

        public Function(string name, State from, State to, Letter letter)
        {
            this.name = name;
            this.from = from;
            this.to = to;
            this.letter = letter;
        }

        #endregion

        #region Methods

        public static bool operator ==(Function a, Function b)
        {
            return ((a.name == b.name) && (a.from == b.from) && (a.to == b.to));
        }

        public static bool operator !=(Function a, Function b)
        {
            return (!(a == b));
        }

        #endregion
    }


    /// <summary>
    /// Алфавит автомата
    /// Состоит из списка букв.
    /// </summary>
    public class Alphabet
    {
        #region Fields

        private ObservableCollection<Letter> letters;

        #endregion

        #region Properties

        public ObservableCollection<Letter> Letters
        {
            get { return letters; }
            set { letters = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет, является ли свово распознаваемы данным алфавитом.
        /// </summary>
        /// <param name="word">Слово</param>
        /// <returns></returns>
        public bool CanRead(List<Letter> word)
        {
            foreach (Letter letter in word)
            {
                if (!Contains(letter)) return false;
            }
            return true;
        }


        /// <summary>
        /// Проверяет, влючена ли буква в данный алфавит.
        /// </summary>
        /// <param name="findLatter">буква</param>
        /// <returns></returns>
        public bool Contains(Letter findLatter)
        {
            foreach (Letter letter in Letters)
            {
                if (letter == findLatter) return true;
            }
            return false;
        }

        /// <summary>
        /// Добавляет новую букву в алфавит.
        /// </summary>
        /// <param name="letter">буква</param>
        public void AddNewLetter(Letter newLetter)
        {
            if (newLetter.Name.Length > 0)
            {
                foreach (Letter letter in letters)
                {
                    if (letter == newLetter) throw new ArgumentException("Такая буква уже есть!");
                }
                letters.Add(newLetter);
            }
            else
            {
                throw new ArgumentException("Таконе название не допустимо!");
            }
        }

        #endregion

        #region Constructors

        public Alphabet()
        {
            letters = new ObservableCollection<Letter>();
        }

        public Alphabet(ObservableCollection<Letter> letters)
        {
            this.letters = letters;
        }

        #endregion
    }


    /// <summary>
    /// Буква алфавита.
    /// Букву определяет название и описание.
    /// </summary>
    public class Letter
    {

        #region Fields

        private string name;

        private string description;

        #endregion

        #region Properties
        //TODO переделать под словарь???

        /// <summary>
        /// Имя буква
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Её описание
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region Constuctors

        public Letter()
        {
            this.name = "";
            this.description = "";
        }

        public Letter(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        #endregion

        #region Methods

        public static bool operator ==(Letter a, Letter b)
        {
            return (a.name == b.name);
        }

        public static bool operator !=(Letter a, Letter b)
        {
            return (!(a == b));
        }


        #endregion
    }

}
