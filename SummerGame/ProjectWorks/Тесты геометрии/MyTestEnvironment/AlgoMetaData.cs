using System;
using System.Collections.Generic;
using Drawer;

namespace MyTestEnvironment
{
    public class AlgoMetaData
    {
        public string Name { get; private set; }

        public IAutoGenerator AutoGenerator { get; private set; }

        public FileReader FileReader { get; private set; }

        public ManualInput InputFunction { get; private set; }

        public InputCreationMode InputMode { get; set;}

        public List<InputCreationMode> InputModes
        {
            get
            {
                var result = new List<InputCreationMode>();
                if (AutoGenerator != null) result.Add(InputCreationMode.AutoGeneration);
                if (FileReader != null) result.Add(InputCreationMode.FromFile);
                if (InputFunction != null) result.Add(InputCreationMode.Manual);
                return result;
            }
        }

        // Generates input
        // for given input method ( FromFile, AutoGen etc. ) and some parameters
        public IDrawable MakeInput(object parameters, int testNumber)
        {
            IDrawable result = null;

            if (InputMode != InputCreationMode.None)
            {
                if ((InputMode == InputCreationMode.AutoGeneration) && (AutoGenerator != null))
                {
                    var variables = AutoGenerator.Settings.GenerateVariables(testNumber);
                    result = AutoGenerator.InputGenerator(variables);
                }
                else if ((InputMode == InputCreationMode.FromFile) && (FileReader != null))
                {
                    //todo implement this
                }
                else if ((InputMode == InputCreationMode.Manual) && (InputFunction != null))
                {
                    //todo implement this
                }
                else
                {
                    throw new Exception("Invalid input generation mode: " + InputMode);
                }
            }
            return result;
        }

        #region Constructros

        public AlgoMetaData(string name, IAutoGenerator autoGenerator = null, FileReader fileReader = null, ManualInput manualInput = null)
        {
            Name = name;
            AutoGenerator = autoGenerator;
            FileReader = fileReader;
            InputFunction = manualInput;
        }

        #endregion

        #region Private Members

        private IAutoGenerator MakeAutoGenerator(AutoGeneratorSettings settings)
        {
            throw new NotImplementedException(); // todo implement this or throw it away
        }

        #endregion
    }
}