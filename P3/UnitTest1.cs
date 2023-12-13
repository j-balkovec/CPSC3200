using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Logger;
using ResourceConversion;

/// <attention>
/// - The Formula class has undergone comprehensive testing and is bug-free for the P1 submission.
/// - To access the test details, please consult the P1UnitTest.csproj.
/// </attention>

namespace P3UnitTest
{
    [TestClass]
    public class PlanUnitTest1
    {
        //! Formula Object to be replace or inserted
        private readonly Formula MockFormulaObjOne = new Formula(new string[] { "X-Mock1" },
                                                                 new uint[] { 1 },
                                                                 new string[] { "Y-Mock1" },
                                                                 new uint[] { 2 },
                                                                 new uint[] { },
                                                                 3);

        //! Formula Object to be replace or inserted
        private readonly Formula MockFormulaObjTwo = new Formula(new string[] { "X-Mock2" },
                                                                 new uint[] { 1 },
                                                                 new string[] { "Y-Mock2" },
                                                                 new uint[] { 2 },
                                                                 new uint[] { },
                                                                 3);

        //! Formula Object to be replace or inserted
        private readonly Formula MockFormulaObjThree = new Formula(new string[] { "X-Mock3" },
                                                                   new uint[] { 1 },
                                                                   new string[] { "Y-Mock3" },
                                                                   new uint[] { 2 },
                                                                   new uint[] { },
                                                                   3);

        //! Default null object --> aliasing for clarity and readability
        private readonly Formula? DefaultNullObj = null;

        static Formula[] _InitSequence_()
        {
            const uint ProficiencyLevel = 3;

            (string[] InputResources,
             uint[] InputQuantities,
             string[] OutputResources,
             uint[] OutputQuantities,
             uint[] ResultArray)[] examples = {

                (new[] { "X1" }, new uint[] { 2 },
                new [] { "Y1" }, new uint[] { 1 }, new uint[0]),

                (new[] { "X2-1", "X2-1" }, new uint[] { 3, 1 },
                new[] { "Y2" }, new uint[] { 1 }, new uint[0]),

                (new[] { "X3-1", "X3-2" }, new uint[] { 8, 4 },
                new[] { "Y3" }, new uint[] { 16 }, new uint[0]),

                (new[] { "X4-1", "X4-2", "X4-3"}, new uint[] { 2, 3, 1 },
                new[] { "Y4" }, new uint[] { 36 }, new uint[0])
                                              };

            Formula[] formulaArray = new Formula[examples.Length];

            for (int i = 0; i < examples.Length; i++)
            {
                formulaArray[i] = new Formula(examples[i].InputResources, examples[i].InputQuantities,
                    examples[i].OutputResources, examples[i].OutputQuantities, examples[i].ResultArray, ProficiencyLevel);
            }

            return formulaArray;
        }

        [TestMethod]
        public void TestPlanConstructorWithValidData()
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Assert.IsTrue(MockPlan is not null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPlanConstructorWithInvalidData_ExpectException()
        {
            Formula[] Sequence = _InitSequence_();
            Sequence = Enumerable.Repeat<Formula>(DefaultNullObj, Sequence.Length).ToArray();

            Plan MockPlan = new Plan(Sequence);
        }

        [TestMethod]
        public void TestPlanDeepCopy() //! private Clone() tested here
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Plan DeepCopy = MockPlan.PlanDeepCopy();

            Assert.IsTrue(MockPlan.Equals(DeepCopy));
        }

        [TestMethod]
        public void TestPlanShallowCopy()
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Plan ShallowCopy = MockPlan.PlanShallowCopy();

            Assert.IsTrue(MockPlan.Equals(ShallowCopy));
        }

        [DataTestMethod]
        [DataRow("X-Mock1", "Y-Mock1")]
        [DataRow("X-Mock2", "Y-Mock2")]
        [DataRow("X-Mock3", "Y-Mock3")]
        public void TestPlanAddWithValidDataRows(string InputResource, string OutputResources)
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Formula? FormulaToAdd = null;

            if (InputResource == "X-Mock1") { FormulaToAdd = MockFormulaObjOne; }
            else if (InputResource == "X-Mock2") { FormulaToAdd = MockFormulaObjTwo; }
            else if (InputResource == "X-Mock3") { FormulaToAdd = MockFormulaObjThree; }

            uint MockPlanSizeBefore = (uint)MockPlan.GetFormulaArray().Length;
            MockPlan.AddFormula(FormulaToAdd);
            uint MockPlanSizeAfter = (uint)MockPlan.GetFormulaArray().Length;

            Assert.IsTrue(MockPlanSizeBefore != MockPlanSizeAfter);

            Formula[] MockArray = MockPlan.GetFormulaArray();
            CollectionAssert.Contains(MockArray, FormulaToAdd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPlanAddWithNull_ExpectException()
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Formula? FormulaToAdd = null;

            MockPlan.AddFormula(FormulaToAdd);
        }

        [TestMethod]
        public void TestPlanRemoveLastWithValidData()
        {
            Plan MockPlan = new Plan(_InitSequence_());

            Formula FormulaToBeRemoved = MockPlan.GetFormulaArray()[^1];

            uint MockPlanSizeBefore = (uint)MockPlan.GetFormulaArray().Length;
            MockPlan.RemoveLastFormula();
            uint MockPlanSizeAfter = (uint)MockPlan.GetFormulaArray().Length;

            Assert.IsTrue(MockPlanSizeBefore != MockPlanSizeAfter);

            Formula[] MockArray = MockPlan.GetFormulaArray();
            CollectionAssert.DoesNotContain(MockArray, FormulaToBeRemoved);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestPlanRemoveLast_ExpectException()
        {
            ///! If passed null as parameter to Plan() => ctor would throw a different error
            
            Formula[] Seq = _InitSequence_();
            Plan MockPlan = new Plan(Seq);
            ushort InvalidSize = (ushort)(Seq.Length + 1);

            for (ushort i = 0; i < InvalidSize; i++)
            {
                MockPlan.RemoveLastFormula();
            }
        }

        [DataTestMethod]
        [DataRow("X-Mock1", "Y-Mock1")]
        [DataRow("X-Mock2", "Y-Mock2")]
        [DataRow("X-Mock3", "Y-Mock3")]
        public void TestPlanReplaceWithValidDataRows(string InputResource, string OutputResources)
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Formula? FormulaToReplace = null;

            if (InputResource == "X-Mock1") { FormulaToReplace = MockFormulaObjOne; }
            else if (InputResource == "X-Mock2") { FormulaToReplace = MockFormulaObjTwo; }
            else if (InputResource == "X-Mock3") { FormulaToReplace = MockFormulaObjThree; }

            uint MockPlanSizeBefore = (uint)MockPlan.GetFormulaArray().Length;
            MockPlan.ReplaceFormula(FormulaToReplace, 0);
            uint MockPlanSizeAfter = (uint)MockPlan.GetFormulaArray().Length;

            //! Size shouldn't change
            Assert.IsFalse(MockPlanSizeBefore != MockPlanSizeAfter);

            Formula[] MockArray = MockPlan.GetFormulaArray();
            CollectionAssert.Contains(MockArray, FormulaToReplace);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestPlanReplaceWithInvalidIndex_ExpectException()
        {
            Formula[] Seq = _InitSequence_();
            Plan MockPlan = new Plan(Seq);
            Formula FormulaToReplace = MockFormulaObjOne;
            ushort InvalidIndex = (ushort)(Seq.Length);

            MockPlan.ReplaceFormula(FormulaToReplace, InvalidIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPlanReplaceWithNull_ExpectException()
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Formula? FormulaToReplace = null;
            const ushort ValidIndex = 0;

            MockPlan.ReplaceFormula(FormulaToReplace, ValidIndex);
        }

        [TestMethod]
        public void TestPlanApplyWihtValidData()
        {
            Plan MockPlan = new Plan(_InitSequence_());
            Formula[] FormulaArray = MockPlan.GetFormulaArray();

            //! 1 -> 0,1,2
            uint[] ResultArrayOne = FormulaArray[0].GetResultArray;

            //! 1 -> 0,1,2
            uint[] ResultArrayTwo = FormulaArray[1].GetResultArray;

            //! 16 -> 0, 12, 16, 18
            uint[] ResultArrayThree = FormulaArray[2].GetResultArray;

            //! 36 -> 0, 27, 36, 40
            uint[] ResultArrayFour = FormulaArray[3].GetResultArray;

            foreach (uint value in ResultArrayOne)
            {
                Assert.IsTrue(value == 0 || value == 1 || value == 2);
            }

            foreach (uint value in ResultArrayTwo)
            {
                Assert.IsTrue(value == 0 || value == 1 || value == 2);
            }

            foreach (uint value in ResultArrayThree)
            {
                Assert.IsTrue(value == 0 || value == 12 || value == 16 || value == 18);
            }

            foreach (uint value in ResultArrayFour)
            {
                Assert.IsTrue(value == 0 || value == 27 || value == 36 || value == 40);
            }
        }
    }
}