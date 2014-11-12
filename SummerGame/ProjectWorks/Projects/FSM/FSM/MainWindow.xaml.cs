using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using StateMachine;

//<ComboBox.ItemTemplate>
//    <DataTemplate>
//        <TextBlock Text="{Binding Name}"/>
//    </DataTemplate>
//</ComboBox.ItemTemplate>

namespace FSM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("МИША ОТВЕТЬ ВК!");
            UpdateBindings();
        }


        Machine machine = new Machine();


        private void AddNewState(object sender, EventArgs e)
        {
            try
            {
                var stateStr = State.Text;
                bool isLast = IsLast.IsChecked.Value;
                State state = new State(stateStr, isLast);
                machine.AddNewState(state);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void AddNewFunction(object sender, RoutedEventArgs e)
        {
            try
            {
                string function = Function.Text;
                State from = (State)From.SelectedValue;
                State to = (State)To.SelectedValue;
                Letter letter = (Letter)WhichLetter.SelectedValue;
                machine.AddNewFunction(new Function(function, from, to, letter));

                //UpdateField();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void AddNewLetter(object sender, EventArgs e)
        {
            try
            {
                string letterName = LetterName.Text;
                string letterDescription = LetterDescription.Text;
                Letter letter = new Letter(letterName, letterDescription);
                machine.Alphabet.AddNewLetter(letter);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void UpdateField(object sender, EventArgs ev)
        {
            Field.Children.Clear();
            MachineDrawer.GenerateVertices(machine, Field.ActualWidth, Field.ActualHeight);
            foreach (UIElement elem in MachineDrawer.shapeList)
            {
                Field.Children.Add(elem);
            }
        }

        private void UpdateBindings()
        {
            From.ItemsSource = machine.States;
            To.ItemsSource = machine.States;
            WhichLetter.ItemsSource = machine.Alphabet.Letters;

            WhichStateRemove.ItemsSource = machine.States;
            WhichLetterRemove.ItemsSource = machine.Alphabet.Letters;
            WhichFunctionRemove.ItemsSource = machine.Functions;

            AllFunctions.ItemsSource = machine.Functions;
            AllStates.ItemsSource = machine.States;
            AllLetters.ItemsSource = machine.Alphabet.Letters;

            FirstCondition.ItemsSource = machine.States;

            machine.Functions.CollectionChanged += UpdateField;
            machine.States.CollectionChanged += UpdateField;
        }

        private void RemoveState(object sender, RoutedEventArgs e)
        {
            try
            {
                var stateToRemove = WhichStateRemove.SelectedItem as State;
                machine.removeState(stateToRemove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void RemoveFunction(object sender, RoutedEventArgs e)
        {
            try
            {
                var functionToRemove = WhichFunctionRemove.SelectedItem as Function;
                machine.removeFunction(functionToRemove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void SetFirstCondition(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = FirstCondition.SelectedItem as State;
                machine.FirstState = obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void RemoveButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var letterToRemove = WhichLetterRemove.SelectedItem as Letter;
                machine.removeLetter(letterToRemove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Determ(object sender, RoutedEventArgs e)
        {
            isDet.Text = machine.IsDet().ToString();
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            MachineSerializer.Serialize(machine, MachineName.Text);
        }

        private void Download(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Text documents (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                machine = MachineSerializer.Deserialize(filename);
                UpdateField(null, null);
                UpdateBindings();
            }

        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            machine.ClearMachine();
            //machine.AddNewState(new State("1", true));
            //machine.AddNewState(new State("2", true));
            //machine.AddNewState(new State("3", true));
            //machine.AddNewState(new State("4", true));
            //machine.AddNewLetter(new Letter("1", ""));
            //machine.AddNewLetter(new Letter("2", ""));
            //machine.AddNewLetter(new Letter("3", ""));
            //machine.AddNewFunction(new Function("1", machine.States[0], machine.States[1], machine.Alphabet.Letters[0]));
            //machine.AddNewFunction(new Function("2", machine.States[1], machine.States[2], machine.Alphabet.Letters[1]));
            //machine.AddNewFunction(new Function("3", machine.States[2], machine.States[0], machine.Alphabet.Letters[2]));
            //machine.AddNewFunction(new Function("2", machine.States[2], machine.States[3], machine.Alphabet.Letters[1]));
            //machine.AddNewFunction(new Function("1", machine.States[3], machine.States[0], machine.Alphabet.Letters[0]));
            //machine.FirstState = machine.States[0];
        }

        private void CheckWord(object sender, RoutedEventArgs e)
        {
            try
            {
                string word = Word.Text;

                if (machine.FirstState.Name=="") throw new Exception("Не задана начальная вершина.");
                //CheckWord
                checkResult.Text = "ОТВЕТ МИША НАПИШИ ЧО НАДА CЮДА";
                bool temp = machine.CheckWord(SupportParser(word));
                checkResult.Text = temp.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }

        }

        /// <summary>
        /// Вспомогательные парсер, для создания слова из строки
        /// </summary>
        /// <param name="strin"></param>
        /// <returns></returns>
        private List<Letter> SupportParser(string strin)
        {
            List<Letter> word = new List<Letter>();
            var a = strin.Split(new Char[] { ' ', ',', '\t' }).Where(n => n.Length>0).ToList();
            foreach (var s in a)
            {
                word.Add(new Letter(s, ""));
            }
            return word;
        }

        private void FindWords(object sender, RoutedEventArgs e)
        {

            // так очищаем и добавляем. 
            Words.Items.Clear();
            
            var pp = machine.GetLanguage(machine.FirstState, new List<Letter>(), new List<State>());
            var d = "";

            foreach (string s in pp)
            {
                Words.Items.Add(s);
            }
            //foreach (List<Letter> word in pp)
            //{
            //    var wordic = "";
            //    foreach (Letter letter in word)
            //    {
            //        wordic += letter.Name + " ";
            //    }
            //    Words.Items.Add(wordic);
            //}
        }

    }
}
