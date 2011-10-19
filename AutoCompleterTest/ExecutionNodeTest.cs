using console2.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoCompleterTest
{
    
    
    /// <summary>
    ///This is a test class for ExecutionNodeTest and is intended
    ///to contain all ExecutionNodeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExecutionNodeTest
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
        ///A test for seperate
        ///</summary>
        [TestMethod()]
        public void seperateTestPart1()
        {
            string text = "A.B.get(C,d, E) F(g(h));"; // TODO: Initialize to an appropriate value
            ExecutionNode actual;
            actual = ExecutionNode.seperate(text);

            Assert.IsTrue(actual.executionText.Equals("A"));
            Assert.IsTrue(actual.getPostDotNode().executionText.Equals("B"));
            Assert.IsTrue(actual.getPostDotNode().getPostDotNode().getPostParallelNode().executionText.Equals("F"));
            Assert.IsTrue(actual.getPostDotNode().getPostDotNode().getPostParallelNode().getParameterNodes()[0].getParameterNodes()[0].executionText.Equals("h"));
            Assert.IsTrue(actual.getPostDotNode().getPostDotNode().getParameterNodes()[1].executionText.Equals("d"));
            Assert.IsTrue(actual.getPostDotNode().getPostDotNode().getParameterNodes()[2].executionText.Equals("E"));
        }
        




    }
}
