using System.IO;
using GameEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapSerializerUnitTests
{


    /// <summary>
    ///This is a test class for MapSerializerTest and is intended
    ///to contain all MapSerializerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MapSerializerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion


        /// <summary>
        ///A test for Serialize
        ///</summary>


        //public void SerializeTest()
        //{
        //    Game.DummyDebugMap();
        //    string path = "unitTest.xml";
        //    Serializer.Serialize(Game.MainMap, path);
        //    Assert.IsTrue(Game.MainMap.Lands.Count > 0 && File.Exists(path));
        //}

        /// <summary>
        ///A test for Deserialize
        ///</summary>
        //[TestMethod()]
        //public void DeserializeTest()
        //{
        //    string path = @"C:\Users\Betin_Ye_A\Dropbox\SummerGame\SummerGame\TestResults\Betin_Ye_A_T506-I7-2 2013-07-15 18_00_51\Out\unitTest.xml";
        //    Serializer.Deserialize(path);
        //    Assert.AreNotEqual(Game.MainMap,null);
        //}
        //}
    }
}
