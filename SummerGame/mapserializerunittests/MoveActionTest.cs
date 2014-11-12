using GameEngine;
using GameEngine.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Geometry.Figures;
using System.Collections.Generic;

namespace MapSerializerUnitTests
{
    
    
    /// <summary>
    ///This is a test class for MoveActionTest and is intended
    ///to contain all MoveActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MoveActionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Serializer.Deserialize(@"D:\Test Map.xml");
        }
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
        ///A test for getMaxLen
        ///</summary>
        [TestMethod()]
        public void getMaxLenTest()
        {
            List<Point> points = new List<Point>(){new Point(0,0),new Point(5,0),new Point(10,0),new Point(15,0),new Point(20,0)}; // TODO: Initialize to an appropriate value
            List<Point> expected = new List<Point>() { new Point(0, 0), new Point(20, 0) }; // TODO: Initialize to an appropriate value
            // 
            List<Point> actual;
            actual = MoveAction.getMaxLen(points);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WayPolyline
        ///</summary>
        [TestMethod()]
        [DeploymentItem("GameEngine.dll")]
        public void WayPolylineTest()
        {
            MoveAction_Accessor target = new MoveAction_Accessor(); // TODO: Initialize to an appropriate value
            Unit unit = null; // TODO: Initialize to an appropriate value
            Polyline polyline = null; // TODO: Initialize to an appropriate value
            List<Way> expected = null; // TODO: Initialize to an appropriate value
            List<Way> actual;
            actual = target.WayPolyline(unit, polyline);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Now
        ///</summary>
        [TestMethod()]
        public void NowTest()
        {
            MoveAction target = new MoveAction(); // TODO: Initialize to an appropriate value
            Unit unit = null; // TODO: Initialize to an appropriate value
            double time = 0F; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.Now(unit, time);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Collision
        ///</summary>
        [TestMethod()]
        [DeploymentItem("GameEngine.dll")]
        public void CollisionTest()
        {
            MoveAction_Accessor target = new MoveAction_Accessor(); // TODO: Initialize to an appropriate value
            Unit unit = null; // TODO: Initialize to an appropriate value
            List<Way> curUnitWays = null; // TODO: Initialize to an appropriate value
            List<Way> expected = null; // TODO: Initialize to an appropriate value
            List<Way> actual;
            actual = target.Collision(unit, curUnitWays);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CheckWaysCollision
        ///</summary>
        [TestMethod()]
        [DeploymentItem("GameEngine.dll")]
        public void CheckWaysCollisionTest()
        {
            MoveAction_Accessor target = new MoveAction_Accessor(); // TODO: Initialize to an appropriate value
            Unit curUnit = null; // TODO: Initialize to an appropriate value
            Unit movingUnit = null; // TODO: Initialize to an appropriate value
            List<Way> curUnitWays = null; // TODO: Initialize to an appropriate value
            List<Way> movingUnitWays = null; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.CheckWaysCollision(curUnit, movingUnit, curUnitWays, movingUnitWays);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
