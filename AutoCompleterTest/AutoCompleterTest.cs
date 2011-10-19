using console2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace AutoCompleterTest
{
    
    
    /// <summary>
    ///This is a test class for AutoCompleterTest and is intended
    ///to contain all AutoCompleterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AutoCompleterTest
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
        ///A test for getCompletionItemsOfText
        ///</summary>
        [TestMethod()]
        public void getCompletionItemsOfTextTest()
        {
        /*    MyConsole myconsole = null; // TODO: Initialize to an appropriate value
            AutoCompleter target = new AutoCompleter(myconsole); // TODO: Initialize to an appropriate value
            string text = string.Empty; // TODO: Initialize to an appropriate value
            int position = 0; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.getCompletionItemsOfText(text, position);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        */}


        /// <summary>
        ///A test for getListedItemsOfText
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getListedItemsOfTextTest()
        {
            string text = "NietGebruikt deel1(binnenin).deel2(binnen(binnen).nogBinnen()).deel3";
            int position = text.Length; 
            string[] expected = new string[3] { "deel1(binnenin)", "deel2(binnen(binnen).nogBinnen())", "deel3" };
            var actual = AutoCompleter_Accessor.getListedItemsOfText(text, position);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(actual[i].Equals(expected[i]), actual[i] + " isNotTheSameAs: " + expected[i]);
            
            }
        }
    

        /// <summary>
        ///A test for getListOfSubString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getListOfSubStringTest()
        {
             string useFullString = "hallo(junk().junk()).deel2.deel3()"; 
            var expected = new string[3] { "hallo(junk().junk())", "deel2", "deel3()" };
            var actual = AutoCompleter_Accessor.getListOfSubString(useFullString);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(expected[i].Equals(actual[i]));
            }
            useFullString = "hallo.";
            expected = new string[2] {"hallo", ""};
            actual = AutoCompleter_Accessor.getListOfSubString(useFullString);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(expected[i].Equals(actual[i]));
            }
            
        }

        /// <summary>
        ///A test for getStartIndex
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getStartIndexTest()
        {
            string text = "gewoon";
            int i = 0;
            int normalParenThesis = 0; int expected = 0;
            int actual = AutoCompleter_Accessor.getStartIndex(text, i, normalParenThesis);
            Assert.AreEqual(expected, actual);
            text = "deel1(deel2";
            i = 8;
            normalParenThesis = 0;
            expected = 6;
            Assert.AreEqual(expected, AutoCompleter_Accessor.getStartIndex(text, i, normalParenThesis));
            text = "deel1(deel2)(deel3";
            i = 16;
            normalParenThesis = 0;
            expected = 13;
            Assert.AreEqual(expected, AutoCompleter_Accessor.getStartIndex(text, i, normalParenThesis));





        }

    

       

        /// <summary>
        ///A test for getNextObjects
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getNextObjectsTest()
        {
            /*PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            AutoCompleter_Accessor target = new AutoCompleter_Accessor(param0); // TODO: Initialize to an appropriate value
            object previous = null; // TODO: Initialize to an appropriate value
            string nextString = string.Empty; // TODO: Initialize to an appropriate value
            List<object> expected = null; // TODO: Initialize to an appropriate value
            List<object> actual;
            actual = target.getNextObjects(previous, nextString);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
       */ }

        /// <summary>
        ///A test for getNextStrings
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getNextStringsTest()
        {
       /*     PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            AutoCompleter_Accessor target = new AutoCompleter_Accessor(param0); // TODO: Initialize to an appropriate value
            object previous = null; // TODO: Initialize to an appropriate value
            string nextString = string.Empty; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.getNextStrings(previous, nextString);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
      */  }

        /// <summary>
        ///A test for getPossibleItemsOutOfTextItems
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getPossibleItemsOutOfTextItemsTest()
        {/*
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            AutoCompleter_Accessor target = new AutoCompleter_Accessor(param0); // TODO: Initialize to an appropriate value
            string[] listedItems = null; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.getPossibleItemsOutOfTextItems(listedItems);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        */}

        /// <summary>
        ///A test for getAllFirstBasesString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getAllFirstBasesStringTest()
        {/*
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            AutoCompleter_Accessor target = new AutoCompleter_Accessor(param0); // TODO: Initialize to an appropriate value
            string selectionString = string.Empty; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = target.getAllFirstBasesString(selectionString);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        */}

        /// <summary>
        ///A test for getAllFirstBasesObjects
        ///</summary>
        [TestMethod()]
        [DeploymentItem("console2.exe")]
        public void getAllFirstBasesObjectsTest()
        {
        /*    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            AutoCompleter_Accessor target = new AutoCompleter_Accessor(param0); // TODO: Initialize to an appropriate value
            string selectionString = string.Empty; // TODO: Initialize to an appropriate value
            List<object> expected = null; // TODO: Initialize to an appropriate value
            List<object> actual;
            actual = target.getAllFirstBasesObjects(selectionString);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        */}

      }
}
