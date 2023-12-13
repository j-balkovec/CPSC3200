/// <file> Driver.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Sun 24th Sep </date>
/// 
/// <revision>
/// Revision History 
/// - 1.0 [09/24/2023]: Initial skeleton
/// - 2.0 [09/25/2023]: DisplayValues
/// </revision>
///
/// <summary>
/// * This file contains the definition of the Driver class, which shows the
/// * functionality and usage of the Formula class
/// </summary>
/// 
/// <dependencies>
/// * This class does not have any external dependencies
/// </dependencies>

using ResourceConversion;

class Driver
{
    readonly bool Display = true;

    /// <summary>
    /// - Demonstrates the usage of the ExecutablePlan class and showcases its features.
    /// </summary>
    /// 
    /// <remarks>
    /// - This method provides an example of working with the ExecutablePlan class,
    ///   a derived class from the Plan class. It demonstrates the creation of example formulas,
    ///   initializing an ExecutablePlan instance, and performing various operations such as adding,
    ///   replacing, and removing formulas, as well as applying the plan. The example also
    ///   illustrates deep and shallow copying of the ExecutablePlan object and displays
    ///   the results of each operation.
    /// </remarks>
    ///
    /// <attention>
    /// - For a comprehensive understanding of full functionality, error handling,
    ///   proper inheritance implementation, and edge case scenarios, please consult
    ///   the test cases found in "UnitTest2" and "UnitTest1.
    /// </attention>
    static void ExecutablePlanExample()
    {
        const uint ProficiencyLevel = 3;

        string[] InputResourcesOne = { "Iron Ore" };
        uint[] InputQuantitiesOne = { 2 };
        string[] OutputResourcesOne = { "Iron Bar" };
        uint[] OutputQuantitiesOne = { 1 };
        uint[] ResultArrayOne = { };

        string[] InputResourcesTwo = { "Iron Ore", "Coal" };
        uint[] InputQuantitiesTwo = { 3, 1 };
        string[] OutputResourcesTwo = { "Steel Bar" };
        uint[] OutputQuantitiesTwo = { 1 };
        uint[] ResultArrayTwo = { };

        string[] InputResourcesThree = { "Eggs", "Chesee" };
        uint[] InputQuantitiesThree = { 8, 4 };
        string[] OutputResourcesThree = { "Omlet" };
        uint[] OutputQuantitiesThree = { 16 };
        uint[] ResultArrayThree = { };

        string[] InputResourcesFour = { "Butter", "Eggs", "Sugar", "Flour", "Baking Soda" };
        uint[] InputQuantitiesFour = { 2, 3, 1, 2, 2 };
        string[] OutputResourcesFour = { "Cookies" };
        uint[] OutputQuantitiesFour = { 36 };
        uint[] ResultArrayFour = { };

        Formula ExampleOne = new Formula(InputResourcesOne,
                                         InputQuantitiesOne,
                                         OutputResourcesOne,
                                         OutputQuantitiesOne,
                                         ResultArrayOne,
                                         ProficiencyLevel);

        Formula ExampleTwo = new Formula(InputResourcesTwo,
                                         InputQuantitiesTwo,
                                         OutputResourcesTwo,
                                         OutputQuantitiesTwo,
                                         ResultArrayTwo,
                                         ProficiencyLevel);

        Formula ExampleThree = new Formula(InputResourcesThree,
                                           InputQuantitiesThree,
                                           OutputResourcesThree,
                                           OutputQuantitiesThree,
                                           ResultArrayThree,
                                           ProficiencyLevel);

        Formula ExampleFour = new Formula(InputResourcesFour,
                                           InputQuantitiesFour,
                                           OutputResourcesFour,
                                           OutputQuantitiesFour,
                                           ResultArrayFour,
                                           ProficiencyLevel);

        Formula[] FormulaArray = { ExampleOne, ExampleTwo};               //!2

        ExecutablePlan ExecutablePlanExampleOne = new ExecutablePlan(FormulaArray, 0);

        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.AddFormula(ExampleThree);                //!3
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.ReplaceFormula(ExampleFour, 1);          //!3
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.RemoveLastFormula();                     //!2
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.PlanApply();                             //!2 Applied
        ExecutablePlanExampleOne.PlanDisplayFormulaOutput();
        Console.WriteLine();

        ExecutablePlan ExecutablePlanExampleTwo = ExecutablePlanExampleOne.EPlanDeepCopy();

        ExecutablePlan ExecutablePlanExampleThree = ExecutablePlanExampleOne.EPlanShallowCopy();

        Console.WriteLine("\n\n<--* [Example One] *--> \n");
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine("\n<--* [Example Two] *-->\n");
        ExecutablePlanExampleTwo.PlanDisplayResources();
        Console.WriteLine("\n<--* [Example Three] *-->\n");
        ExecutablePlanExampleThree.PlanDisplayResources();
    }

    /// <summary>
    /// Entry point of the application.
    /// </summary>
    /// 
    /// <param name="args">Command-line arguments. [maybe_unused]</param>
    /// 
    /// <remarks>
    /// This method demonstrates the functionality of the Formula class by creating an array of
    /// formula instances and displaying input and output values.
    /// </remarks>
    /// 
    /// <precondition>
    /// * None
    /// </precondition>
    /// 
    /// <postcondition>
    /// * The program runs and showcases the resource conversion using the Formula class.
    /// </postcondition>
    static void Main(string[] args)
    {
        /// <summary>
        /// * [lambda] Prints an error message with a custom format and waits for user input
        ///   to continue.
        /// </summary>
        /// 
        /// <param name="ErrorThrown">The exception that triggered the error message.</param>
        /// 
        /// <remark>
        /// * Waits for user input before continuing.
        /// </remark>
        /// 
        /// <precondition>
        /// * Set console foreground color to red.
        /// </precondition>
        /// 
        /// <postcondition>
        /// * Resets the console foreground color after displaying the error message.
        /// </postcondition>
        Action<Exception> PrintErrorMessage = error =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"\n[error]: {error.Message}");
            Console.ResetColor();
            Console.Write(" -> [press <Enter> to continue]");
            Console.ReadLine();
        };
        Driver _Driver = new Driver();
        try
        {
            ExecutablePlanExample();
        }
        catch (Exception ErrorThrown)
        {
            if(_Driver.Display)
                PrintErrorMessage(ErrorThrown);

            throw ErrorThrown;

        } finally
        {
            Console.ReadLine();
        }
        
    }
}
