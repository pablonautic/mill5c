using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mill5C.Core.Cutters;
using Mill5C.Core.Geometry;

namespace Mill5C.Tests
{
    /// <summary>
    /// Summary description for CollisionTest
    /// </summary>
    [TestClass]
    public class CollisionTest
    {
        FlatCutter fc;

        public CollisionTest()
        {
            fc = new FlatCutter
            {
                R = 1,
                H = 10
            };
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void CalcCTest()
        //{
        //    var c1 = fc.CalcC(new Point3D { X = 2, Y = 2, Z = -1 }, 1);
        //    Assert.IsTrue(c1.IsNear(0, 0, -1));

        //    var c2 = fc.CalcC(new Point3D { X = 2, Y = 2, Z = 2.5f }, 3);
        //    Assert.IsTrue(c2.IsNear(0, 0, 2.5f));
        //}

        //[TestMethod]
        //public void CalcDTest()
        //{
        //    var c1 = fc.CalcD(new Point3D { X = 2, Y = 2, Z = -1 });
        //    Assert.IsTrue(c1.IsNear(1 + fc.H / 2));

        //    var c2 = fc.CalcD(new Point3D { X = 2, Y = -6, Z = 7 });
        //    Assert.IsTrue(c2.IsNear(2));
        //}

        //[TestMethod]
        //public void IntersectTest()
        //{
        //    Assert.IsTrue(fc.CylinderSphere(new Point3D { X = 0, Y = 0, Z = -1 }, 2) == CollisionType.Partial);

        //    Assert.IsFalse(fc.CylinderSphere(new Point3D { X = 2, Y = 2, Z = -2 }, 1.5f) == CollisionType.Partial);
        //}
    }
}
