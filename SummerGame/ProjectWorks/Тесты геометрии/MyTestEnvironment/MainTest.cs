using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Drawer;
using Drawer.Transformers;

namespace MyTestEnvironment
{
    public class MainTest : INotifyPropertyChanged
    {

        #region Fields

        private static ITestable _selectedTest = null;

        private static int _currentTestNumber;

        private static int _maxTestNumber;

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private static List<ITestable> Tests;

        public static List<String> Names
        {
            get
            {
                return Tests.Select(metaData => metaData.MetaData.Name).ToList();
            }
        }

        public static List<InputCreationMode> SelectedTestInputMethods()
        {
            return SelectedTest.MetaData.InputModes;
        }

        public static ITestable SelectedTest
        {
            get { return _selectedTest; }
            set { _selectedTest = value; }
        }

        public InputCreationMode SelectedInputMode
        {
            get
            {
                if (SelectedTest == null)
                {
                    return InputCreationMode.None;
                }
                else
                {
                    return SelectedTest.MetaData.InputMode;
                }
            }
            set
            {
                if (SelectedTest != null)
                {
                    SelectedTest.MetaData.InputMode = value;
                }
            }
        }

        // 1-based counter
        public int CurrentTestNumber
        {
            get { return _currentTestNumber; }
            set
            {
                if ((value > MaxTestNumber) || (value < 1))
                {
                    _currentTestNumber = 1;
                }
                else
                {
                    _currentTestNumber = value;
                }

                CurrentInput = GetTestInput();//todo redo this

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentTestNumber"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentInput"));
                }

            }
        }

        public int MaxTestNumber
        {
            get
            {
                if (SelectedTest != null && SelectedTest.MetaData.AutoGenerator != null)
                { _maxTestNumber = SelectedTest.MetaData.AutoGenerator.Settings.TotalCycles; }; //todo correct this
                return _maxTestNumber;
            }
            private set
            {
                if (value >= 0)
                {
                    _maxTestNumber = value;
                }
                else
                {
                    _maxTestNumber = 0;
                }
                PropertyChanged(this, new PropertyChangedEventArgs("MaxTestNumber"));
            }
        }

        public List<string> FileNames { get; set; }

        public /*IDrawable*/ Port CurrentInput { get; private set; }

        public static void SelectTest(String name)
        {
            List<ITestable> items = Tests.Select(x => x).Where(metaData => metaData.MetaData.Name == name).ToList();
            SelectedTest = null;
            if (items.Count == 1)
            {
                SelectedTest = items[0];
                if (SelectedTest.MetaData != null)
                {
                    if (SelectedTest.MetaData.AutoGenerator != null)
                    {
                        _maxTestNumber = SelectedTest.MetaData.AutoGenerator.Settings.TotalCycles - 1;
                    }
                    else
                    {
                        _maxTestNumber = 1;
                    }
                }//todo make it better

            }// TODO Decide what to do in case many items are found
        }

        public static List<VariableGeneratorMetadata> SelectedTestAutoGeneratorSettings
        {
            get { return SelectedTest.MetaData.AutoGenerator.Settings.Data; }
        }

        private /*IDrawable*/ Port GetTestInput()
        {
            var p = new Port(new VisualSettings("A", color: "Green", transformer: new TranslateTransformer(10, 20)));

            IDrawable input, output;
            try
            {

                if (SelectedInputMode == InputCreationMode.AutoGeneration)
                {
                    input = SelectedTest.MetaData.MakeInput(null, _currentTestNumber);
                }
                else if (SelectedInputMode == InputCreationMode.FromFile)
                {
                    string filename = FileNames[_currentTestNumber - 1];
                    input = SelectedTest.MetaData.FileReader(filename);
                }
                else return null;
            }
            catch (Exception e)
            {
                throw new Exception("There was an error during input creation", e);
            }

            input.PopulatePort(p, "Input."); p.Update();
            try
            {
                output = SelectedTest.RunMethod(input);
                output.PopulatePort(p, "Output."); p.Update();
            }
            catch (Exception e)
            {
                //throw new Exception("There was an error during alogorithm invocation", e);
                p["Error"].Populate(
                    DrawableElement.Text(20, 20, "There was an error during alogorithm invocation\n",
                    "Error messaage was:", "\t" + e.Message,
                    "\nStackTrace was:", "\t" + e.StackTrace)
                    );
            }

            return p;

        }

        #region Constructors

        static MainTest()
        {
            SelectedTest = null;
        }

        public MainTest(List<ITestable> tests = null)
        {
            SelectedTest = null;
            Tests = tests;
        }

        #endregion

    }

}
