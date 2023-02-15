using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mill5C.Settings;

namespace Mill5C.Tests
{
    public class TestClass1
    {
        public TestClass1()
        {
        }

        public TestClass1(float arg1, string arg2)
        {
        }
    }

    /// <summary>
    /// Summary description for SettingsTest
    /// </summary>
    [TestClass]
    public class SettingsTest
    {
        public SettingsTest()
        {
            //
            // TODO: Add constructor logic here
            //
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

        [TestMethod]
        public void SaveTest()
        {
            SettingsNode vn1 = new SettingsNode { ValueObject = 0.1f };
            SettingsNode vn2 = new SettingsNode { ValueObject = "blablizer" };
            SettingsNode tn1 = new SettingsNode(vn1, vn2) { TypeObject = typeof(TestClass1).AssemblyQualifiedName };
            tn1.Save("test1.xml");

            SettingsNode node = SettingsNode.FromFile("test1.xml");
            Assert.IsNotNull(node);

            var factory = new GenericObjectFactory<TestClass1>();
            TestClass1 tc = factory.Create(node);
            Assert.IsNotNull(tc);
        }
    }
}
