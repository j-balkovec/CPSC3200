using System;
using ResourceConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace P3UnitTest
{
    [TestClass]
    public class P3ExecutablePlanUnitTest
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
        public void TestConstrucotrWithValidData()
        {


            ///<note>
            /// #1 [Liskov substitution principle]
            ///    - Instance of Executable plan (Derived class) can be used interchangably
            ///      with the Plan class (Super class)
            /// #2 [Instantiation through the child class handle]
            ///
            /// <remark>_Step = 0 as default</remark>
            /// </note>
            Plan MockEPlanParentHandle = new ExecutablePlan(_InitSequence_());
            ExecutablePlan MockEPlanChildHandle = new ExecutablePlan(_InitSequence_());

            ///! Overridden Equals method -> casting 'object' to specific type
            ///<see cref="Plan.Equals(object?)"/>
            Assert.IsTrue(MockEPlanParentHandle.Equals(MockEPlanChildHandle));
            Assert.IsTrue(MockEPlanChildHandle is not null && MockEPlanParentHandle is not null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestConstructorWithInvalidData_ExpectException()
        {
            Formula[] Sequence = _InitSequence_();
            Sequence = Enumerable.Repeat<Formula>(DefaultNullObj, Sequence.Length).ToArray();
            ExecutablePlan MockEPlanParentHandle = new ExecutablePlan(Sequence);
        }

        [TestMethod]
        public void TestEPlanDeepCopy() //! Clone() tested here 
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_());
            ExecutablePlan DeepCopy = EMockPlan.EPlanDeepCopy();

            Assert.IsTrue(EMockPlan.Equals(DeepCopy));
        }

        [TestMethod]
        public void TestEPShallowCopy()
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_());
            ExecutablePlan ShallowCopy = EMockPlan.EPlanShallowCopy();

            Assert.IsTrue(EMockPlan.Equals(ShallowCopy));
        }

        [DataTestMethod]
        [DataRow("X-Mock1", "Y-Mock1")]
        [DataRow("X-Mock2", "Y-Mock2")]
        [DataRow("X-Mock3", "Y-Mock3")]
        public void TestEPlanAddWithValidDataRows(string InputResource, string OutputResources)
        {
            ExecutablePlan ExecutableMockPlan = new ExecutablePlan(_InitSequence_());
            Formula? FormulaToAdd = null;

            if (InputResource == "X-Mock1") { FormulaToAdd = MockFormulaObjOne; }
            else if (InputResource == "X-Mock2") { FormulaToAdd = MockFormulaObjTwo; }
            else if (InputResource == "X-Mock3") { FormulaToAdd = MockFormulaObjThree; }

            uint MockPlanSizeBefore = (uint)ExecutableMockPlan.GetFormulaArray().Length;
            ExecutableMockPlan.AddFormula(FormulaToAdd);
            uint MockPlanSizeAfter = (uint)ExecutableMockPlan.GetFormulaArray().Length;

            Assert.IsTrue(MockPlanSizeBefore != MockPlanSizeAfter);

            Formula[] EMockPlan = ExecutableMockPlan.GetFormulaArray();
            CollectionAssert.Contains(EMockPlan, FormulaToAdd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEPlanAddWithNull_ExpectException()
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_());
            Formula? FormulaToAdd = null;

            EMockPlan.AddFormula(FormulaToAdd);
        }

        [TestMethod]
        public void TestEPlanRemoveLastFormulaWithValidData()
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_());

            Formula FormulaToBeRemoved = EMockPlan.GetFormulaArray()[^1];

            uint MockEPlanSizeBefore = (uint)EMockPlan.GetFormulaArray().Length;
            EMockPlan.RemoveLastFormula();
            uint MockEPlanSizeAfter = (uint)EMockPlan.GetFormulaArray().Length;

            Assert.IsTrue(MockEPlanSizeBefore != MockEPlanSizeAfter);

            Formula[] MockArray = EMockPlan.GetFormulaArray();
            CollectionAssert.DoesNotContain(MockArray, FormulaToBeRemoved);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEPlanRemoveLastFormulaWithInvalidStep_ExpectException()
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_(), 5);
            EMockPlan.RemoveLastFormula();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEPlanRemoveLastFormulaIfFomrmulaWasApplied_ExpectException()
        {
            Formula[] Seq = _InitSequence_();
            ExecutablePlan EMockPlan = new ExecutablePlan(Seq, 0);
            for (ushort i = 0; i < Seq.Length; i++)
            {
                EMockPlan.PlanApply();
            }
            EMockPlan.RemoveLastFormula();
        }

        [DataTestMethod]
        [DataRow("X-Mock1", "Y-Mock1")]
        [DataRow("X-Mock2", "Y-Mock2")]
        [DataRow("X-Mock3", "Y-Mock3")]
        public void TestEPlanReplaceWithValidDataRows(string InputResource, string OutputResources)
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_());
            Formula? FormulaToReplace = null;

            if (InputResource == "X-Mock1") { FormulaToReplace = MockFormulaObjOne; }
            else if (InputResource == "X-Mock2") { FormulaToReplace = MockFormulaObjTwo; }
            else if (InputResource == "X-Mock3") { FormulaToReplace = MockFormulaObjThree; }

            uint MockPlanSizeBefore = (uint)EMockPlan.GetFormulaArray().Length;
            EMockPlan.ReplaceFormula(FormulaToReplace, 0);
            uint MockPlanSizeAfter = (uint)EMockPlan.GetFormulaArray().Length;

            //! Size shouldn't change
            Assert.IsFalse(MockPlanSizeBefore != MockPlanSizeAfter);

            Formula[] MockArray = EMockPlan.GetFormulaArray();
            CollectionAssert.Contains(MockArray, FormulaToReplace);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestEPlanReplaceWithInvalidStep_ExpectException()
        {
            Formula[] Seq = _InitSequence_();
            ExecutablePlan EMockPlan = new ExecutablePlan(Seq, 2);
            Formula? FormulaToReplace = MockFormulaObjOne;

            const ushort ValidIndex = 1;

            EMockPlan.ReplaceFormula(MockFormulaObjOne, ValidIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestEPlanReplaceWithAppliedFormula_ExpectException()
        {
            Formula[] Seq = _InitSequence_();
            ExecutablePlan EMockPlan = new ExecutablePlan(Seq, 0);
            Formula? FormulaToReplace = MockFormulaObjOne;
            const ushort ValidIndex = 1;

            for (ushort i = 0; i < Seq.Length - 2; i++) //Applies 2 only
            {
                EMockPlan.PlanApply();
            }

            EMockPlan.ReplaceFormula(MockFormulaObjOne, ValidIndex);
        }

        [TestMethod]
        public void TestEPlanApplyWihtValidData()
        {
            ExecutablePlan EMockPlan = new ExecutablePlan(_InitSequence_());
            Formula[] FormulaArray = EMockPlan.GetFormulaArray();

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

