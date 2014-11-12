using System.Collections.Generic;
using Drawer;

namespace MyTestEnvironment
{

    public interface IMetaData
    {
        AlgoMetaData MetaData { get; }
    }

    public interface IRunnable
    {
        Runner RunMethod { get; }
    }

    public interface ITestable : IMetaData, IRunnable
    {
    }

    public interface IAutoGenerator
    {
        AutoGeneratorSettings Settings { get; }

        InputGenerator InputGenerator { get; }
    }

    #region Delegates

    /// <summary>
    /// IDrawable InputGenerator(VariablesSet parameters)
    /// </summary>
    /// <param name="parameters">A collection of variables and their values</param>
    /// <returns>Input for the tested algorithm</returns>
    public delegate IDrawable InputGenerator(VariablesSet parameters);

    public delegate IDrawable FileReader(string filename);

    public delegate IDrawable ManualInput();

    /// <summary>
    /// IDrawable Runner(IDrawable input)
    /// Runs a method for testing its results
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public delegate IDrawable Runner(IDrawable input);

    #endregion

    public enum GenerationStrategy { Const, Iterate, Random }


    public enum InputCreationMode { None, Manual, FromFile, AutoGeneration }
}
