using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTestEnvironment
{
    /// <summary>
    /// One variable generation description
    /// </summary>
    public class VariableGeneratorMetadata
    {
        #region Fields

        private String variableName;

        private GenerationStrategy strategy;

        private List<double> parameters;

        static Random gen = new Random( Convert.ToInt32(DateTime.Now.Millisecond) );

        #endregion

        #region Properties

        public List<double> Parameters
        {
            get { return parameters; }
        }

        public GenerationStrategy Strategy
        {
            get { return strategy; }
            set { strategy = value; parameters = VerifyParameters(parameters.ToArray()); }
        }

        public static List<GenerationStrategy> Strategies
        {
            get
            {
                return new List<GenerationStrategy>() 
                    { GenerationStrategy.Const
                    , GenerationStrategy.Iterate
                    , GenerationStrategy.Random };
            }
        }

        public string VariableName
        {
            get { return variableName; }
        }

        public int Cycles
        {
            get
            {
                var result = 1;
                switch (strategy)
                {
                    case GenerationStrategy.Iterate:
                        double a = parameters[0], b = parameters[1], c = parameters[2];
                        result = (int)Math.Floor((b - a) / c) + 1;
                        break;

                }
                return result;
            }
        }

        #endregion

        public Variable GenerateVariable(int i)
        {
            double result = parameters[0];
            switch (strategy)
            {
                case GenerationStrategy.Const:
                    break;
                case GenerationStrategy.Iterate:
                    result += i * parameters[2];
                    break;
                case GenerationStrategy.Random:

                    result += gen.NextDouble() * (parameters[1] - parameters[0]);
                    break;
                default:
                    break;
            }
            return new Variable(variableName, result);
        }

        public VariableGeneratorMetadata(String name, GenerationStrategy strategy, params double[] parameters)
        {
            this.variableName = name;
            this.strategy = strategy;
            this.parameters = VerifyParameters(parameters);
        }

        #region Private methods

        /// <summary>
        /// Counts a number of arguments needed for correct value generation
        /// </summary>
        /// <param name="strategy">Generation strategy</param>
        /// <returns>Integral number of arguments</returns>
        private static int ParamsLength(GenerationStrategy strategy)
        {
            var result = 1; // Constant is defined by 1 parameter
            switch (strategy)
            {
                case GenerationStrategy.Iterate:
                    result = 3; // From ... To ... Step ...
                    break;
                case GenerationStrategy.Random:
                    result = 2; // From ... To ...
                    break;
            }
            return result;
        }

        private List<double> VerifyParameters(params double[] @params)
        {
            var list = @params.ToList();
            var length = ParamsLength(strategy);

            //
            // Simple verification - ensures we get right the 
            // required number of params
            //

            // If we get not enough params we just add zeroes
            if (list.Count < length)
                for (var i = list.Count; i < length; i++)
                {
                    list.Add(0);
                }
            // If we get too many params we just discard the excess
            else if (list.Count > length)
                for (var i = list.Count - 1; i > length - 1; i--)
                {
                    list.RemoveAt(i);
                }

            //
            // Sophisticated verification
            //

            switch (strategy)
            {
                case GenerationStrategy.Iterate:
                    double a = list[0], b = list[1], c = list[2];

                    // c is loop step. It shoud be non-zero
                    if (c == 0) c = 1;

                    // c shoud be positive if a < b
                    // and negative otherwise
                    if ((a < b && c < 0) || (a > b && c > 0))
                        c *= -1;

                    list[0] = a;
                    list[1] = b;
                    list[2] = c;
                    break;
                case GenerationStrategy.Random:
                    double f = list[0], g = list[1];
                    if (f > g)
                    {
                        var h = g;
                        g = f;
                        f = h;
                    }
                    list[0] = f;
                    list[1] = g;
                    break;
            }

            return list;
        }

        #endregion
    }
}