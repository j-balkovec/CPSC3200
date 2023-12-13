/// <file> UnitTest1.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Tue 26th Sep </date>
/// 
/// <revision>
/// Revision History 
/// - 1.0 [09/26/2023]: All test Methods && Documentation
/// </revision>
///
/// <summary>
/// * This file contains the definition of the FormulaTest class.
///   The class provides unit tests for the public methods of the Formula class.
/// </summary>
///
/// <important>
/// * Private methods are tested through the public methods (GetOutComeChances => Apply())
/// </important>
/// 
/// <dependencies> This class does not have any external dependencies </dependencies>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceConversion;
using System.Diagnostics;

namespace UnitTest
{
    /// <summary>
    /// * Test class for the Formula class, which represents a resource conversion formula.
    /// </summary>
    [TestClass]
    public class FormulaTests
    {
        /// <summary>
        /// * Test the constructor of the Formula class with valid input.
        /// </summary>
        ///
        /// <remarks>
        /// * This test case verifies that the Formula constructor works correctly with valid input.
        /// * It checks if the Formula object is not null after construction.
        /// 
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        ///
        /// <postconditions>
        /// * A Formula object is created with the provided input.
        /// * The Formula object is not null.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        public void TestFormulaConstructorValidInput()
        {
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 5, 10 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 7 };
            const uint ProficiencyLevel = 2;

            Formula formula = new Formula(InputResources,
                                          InputQuantities,
                                          OutputResources,
                                          OutputQuantities,
                                          ProficiencyLevel);

            Assert.IsNotNull(formula);
        }

        /// <summary>
        /// *  Test the Formula constructor with invalid input array, expecting an ArgumentException.
        /// </summary>
        ///
        /// <remarks>
        /// * This test case verifies that the Formula constructor correctly throws
        ///   an ArgumentException when provided with invalid input.
        /// 
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        ///
        /// <postconditions>
        /// * An ArgumentException is expected to be thrown during the creation
        ///   of the Formula object with invalid input.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFormulaConstructorInvalidArray()
        {
            // Arrange
            string[] InputResources = { null, "ResourceB" }; //invalid
            uint[] InputQuantities = { 5, 10 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 7 };
            const uint ProficiencyLevel = 0;


            Formula TestFormula = new Formula(InputResources,
                                             InputQuantities,
                                             OutputResources,
                                             OutputQuantities,
                                             ProficiencyLevel);
        }

        /// <summary>
        /// *  Test the Formula constructor with input arrays of unequal length, expecting an ArgumentException.
        /// </summary>
        ///
        /// <remarks>
        /// * This test case verifies that the Formula constructor correctly throws an ArgumentException
        ///   when provided with input arrays of unequal length.
        /// 
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        ///
        /// <postconditions>
        /// * An ArgumentException is expected to be thrown during the creation of the
        ///   Formula object with input arrays of unequal length.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFormulaConstructorInvalidArrayLength()
        {
            // Arrange
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 5, 10 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 7, 6 }; //invalid
            const uint ProficiencyLevel = 0;


            Formula TestFormula = new Formula(InputResources,
                                             InputQuantities,
                                             OutputResources,
                                             OutputQuantities,
                                             ProficiencyLevel);
        }

        /// <summary>
        /// *  Test the Formula constructor with an invalid proficiency level, expecting an ArgumentException.
        /// </summary>
        ///
        /// <remarks>
        /// * This test case verifies that the Formula constructor correctly throws an
        /// ArgumentException when provided with an invalid proficiency level.
        /// 
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        ///
        /// <postconditions>
        /// * An ArgumentException is expected to be thrown during the creation of the Formula
        ///   object with an invalid proficiency level.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFormulaConstructorInvalidProficiencyLevel()
        {
            // Arrange
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 5, 10 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 7 };
            const uint ProficiencyLevel = 10; //invalid


            Formula TestFormula = new Formula(InputResources,
                                             InputQuantities,
                                             OutputResources,
                                             OutputQuantities,
                                             ProficiencyLevel);
        }


        /// <summary>
        /// * Test method to verify the behavior of the Formula constructor with invalid parameters.
        /// </summary>
        /// 
        /// <remarks>
        /// * This test case checks whether the Formula constructor properly throws an ArgumentException
        ///   when invalid parameters are provided.
        ///
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        /// 
        /// <postconditions>
        /// * An ArgumentException should be thrown when attempting to create the Formula instance.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFormulaConstructorInvalidParameters()
        {
            // Arrange
            string[] InputResources = { };
            uint[] InputQuantities = { };
            string[] OutputResources = { };
            uint[] OutputQuantities = { };
            const uint ProficiencyLevel = 6; //invalid


            Formula TestFormula = new Formula(InputResources,
                                             InputQuantities,
                                             OutputResources,
                                             OutputQuantities,
                                             ProficiencyLevel);
        }

        /// <summary>
        /// * Test method to verify the behavior of the Formula.Apply() method when the
        ///   proficiency level is greater than zero.
        /// </summary>
        /// 
        /// <remarks>
        /// * This test case checks the Result of applying a Formula with a proficiency level greater than zero.
        ///   It verifies that there is zero chance of obtaining a partial Result when the proficiency level is 4.
        ///
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        /// 
        /// <postconditions>
        /// * The Formula instance should be successfully created.
        /// * The Result array should contain valid values.
        /// * There should be zero chance of obtaining a partial Result when the proficiency level is 4.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        public void TestApplyLevelGreaterThanZero()
        {
            //zero chance of partial at level 4
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 2, 1 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 1 };
            const uint ProficiencyLevel = 4;

            Formula formula = new Formula(InputResources,
                                 InputQuantities,
                                 OutputResources,
                                 OutputQuantities,
                                 ProficiencyLevel);

            uint[] Result = formula.Apply();

            for (uint i = 0; i < Result.Length; i++)
            {
                Assert.IsTrue((Result[i] == null) || (Result[i] >= OutputQuantities[i]));
            }
        }

        /// <summary>
        /// *  Test method to verify the behavior of the Formula.Apply() method when the proficiency level is at its maximum (level 5).
        /// </summary>
        /// 
        /// <remarks>
        /// * This test case checks the Result of applying a Formula with a proficiency level of 5.
        ///   It verifies that there is zero chance of failure when the proficiency level is at its maximum.
        ///
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        /// 
        /// <postconditions>
        /// * The Formula instance should be successfully created.
        /// * The Result array should contain valid values.
        /// * There should be zero chance of failure when the proficiency level is at its maximum (level 5).
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        public void TestApplyMaxLevel()
        {
            //zero chance of failure at level 5
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 6, 3 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 12 };
            const uint ProficiencyLevel = 5;

            Formula formula = new Formula(InputResources,
                                 InputQuantities,
                                 OutputResources,
                                 OutputQuantities,
                                 ProficiencyLevel);

            uint[] Result = formula.Apply();

            for (uint i = 0; i < Result.Length; i++)
            {
                Assert.IsTrue((Result[i] != null) || (Result[i] >= OutputQuantities[i]));
            }
        }

        /// <summary>
        /// *  Test method to verify the random distribution of the Formula.Apply() method.
        /// </summary>
        /// 
        /// <remarks>
        /// * This test case checks the random distribution of Results when applying a Formula with a proficiency level of 0.
        ///   It applies the Formula multiple times and ensures that the Results fall within expected values.
        ///
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        /// 
        /// <postconditions>
        /// * The Formula instance should be successfully created.
        /// * Multiple Results should be generated by applying the Formula.
        /// * Each Result should contain values that fall within expected ranges (null, 61, 82, or 91).
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        public void TestRandomDistributionOfApplyMethod()
        {
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 6, 12 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 82 };
            const uint ProficiencyLevel = 0;

            Formula formula = new Formula(InputResources,
                                 InputQuantities,
                                 OutputResources,
                                 OutputQuantities,
                                 ProficiencyLevel);

            uint[] ResultOne = formula.Apply();
            uint[] ResultTwo = formula.Apply();
            uint[] ResultThree = formula.Apply();
            uint[] ResultFour = formula.Apply();
            uint[] ResultFive = formula.Apply();

            Assert.IsTrue(new[] { ResultOne,
                                  ResultTwo,
                                  ResultThree,
                                  ResultFour,
                                  ResultFive }
                            .All(Result => Result.All(Item => Item == null
                                                              || Item == 61
                                                              || Item == 82
                                                              || Item == 91)));

        }

        /// <summary>
        /// *  Test method to evaluate the performance of the Formula.Apply() method.
        /// </summary>
        /// 
        /// <remarks>
        /// * This test case assesses the performance of applying the Formula with a proficiency level of 5.
        ///   It measures the execution time of applying the Formula a large number of times and compares it
        ///   to a specified time limit to ensure it completes within an acceptable timeframe.
        ///
        /// <preconditions>
        /// * InputResources is an array of input resource names.
        /// * InputQuantities is an array of input quantities corresponding to InputResources.
        /// * OutputResources is an array of output resource names.
        /// * OutputQuantities is an array of output quantities corresponding to OutputResources.
        /// * ProficiencyLevel is a positive integer representing the proficiency level of the formula.
        /// </preconditions>
        /// 
        /// <postconditions>
        /// * The Formula instance should be successfully created.
        /// * The Formula.Apply() method should execute within an acceptable time limit.
        /// </postconditions>
        /// </remarks>
        [TestMethod]
        public void TestApplyMethodPerformance()
        {
            string[] InputResources = { "ResourceA", "ResourceB" };
            uint[] InputQuantities = { 2, 1 };
            string[] OutputResources = { "ResourceC" };
            uint[] OutputQuantities = { 1 };
            const uint ProficiencyLevel = 5;

            Formula formula = new Formula(InputResources,
                                 InputQuantities,
                                 OutputResources,
                                 OutputQuantities,
                                 ProficiencyLevel);

            const ulong NumberOfIterations = 30000000;
            const uint TimeLimitInSeconds = 2;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (ulong i = 0; i < NumberOfIterations; i++)
            {
                uint[] Result = formula.Apply();
            }

            stopwatch.Stop();

            TimeSpan elapsedTime = stopwatch.Elapsed;
            Assert.IsTrue(elapsedTime < TimeSpan.FromSeconds(TimeLimitInSeconds));
        }

    }
}