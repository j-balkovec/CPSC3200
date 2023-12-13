/// <file> Driver.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Sun 24th Sep </date>
/// 
/// <revision>
/// Revision History 
/// - 1.0 [11/23/2023]: Initial Test Methods
/// - 2.0 [11/24/2023]: Added HC
/// </revision>
///
/// <git-log>
/// * i97jd23 Last Commit
/// * b374467 Added Documenation
/// * a3358be Added HC to driver
/// * d8796be Added test methods to Driver.cs
/// * 1439f21 Debuged Assembly
/// * bdc10ab Updated the Assembly class
/// * 15d9932 Added Functionality to Assembly.cs, TODO: Overload the Apply() method
/// * 04a8833 Added Interfaces, Assembly class, added functionality to the Plan class
/// * c2bacc4 Stable version from P3
/// * fc57240 Updated Logger, Added the Stockpile Class
/// * 010b6ed Initial Commit
/// </git-log>
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
using System.Collections.Concurrent;

class Driver
{
    readonly bool Display = true;

    /// <summary>
    /// * Data Members to instantiate formula objects
    /// </summary>
    const uint ProficiencyLevel = 3;

    static string[] InputResourcesThree = { "A", "B" };
    static uint[] InputQuantitiesThree = { 8, 4 };
    static string[] OutputResourcesThree = { "C" };
    static uint[] OutputQuantitiesThree = { 16 };
    static uint[] ResultArrayThree = { };

    static string[] InputResourcesFour = { "D", "E", "F", "G", "H" };
    static uint[] InputQuantitiesFour = { 2, 3, 1, 2, 2 };
    static string[] OutputResourcesFour = { "I" };
    static uint[] OutputQuantitiesFour = { 36 };
    static uint[] ResultArrayFour = { };


    static Formula DummyObjX = new Formula(InputResourcesThree,
                                   InputQuantitiesThree,
                                   OutputResourcesThree,
                                   OutputQuantitiesThree,
                                   ResultArrayThree,
                                   ProficiencyLevel);

    static Formula DummyObjY = new Formula(InputResourcesFour,
                                       InputQuantitiesFour,
                                       OutputResourcesFour,
                                       OutputQuantitiesFour,
                                       ResultArrayFour,
                                       ProficiencyLevel);

    /// <summary>
    /// * Instantiates an array of Formula objects for testing purposes.
    /// </summary>
    /// 
    /// <returns>
    /// * An array of Formula objects.
    /// </returns>
    static Formula[] InstantiateFomrulaArray()
    {
        const uint ProficiencyLevel = 3;

        string[] InputResourcesOne = { "J" };
        uint[] InputQuantitiesOne = { 2 };
        string[] OutputResourcesOne = { "K" };
        uint[] OutputQuantitiesOne = { 1 };
        uint[] ResultArrayOne = { };

        string[] InputResourcesTwo = { "L", "M" };
        uint[] InputQuantitiesTwo = { 3, 1 };
        string[] OutputResourcesTwo = { "N" };
        uint[] OutputQuantitiesTwo = { 1 };
        uint[] ResultArrayTwo = { };

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

        Formula[] FormulaArray = { ExampleOne, ExampleTwo };
        return FormulaArray;
    }

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
    static Plan? TestPlan()
    {               
        Plan PlanExampleOne = new Plan(InstantiateFomrulaArray());
        Plan ?PlanToBeReturned = PlanExampleOne.DeepCopy() as Plan;


        PlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        PlanExampleOne.AddFormula(DummyObjX);                            //!3
        PlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        PlanExampleOne.ReplaceFormula(DummyObjY, 1);                      //!3
        PlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        PlanExampleOne.RemoveLastFormula();                                 //!2
        PlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        PlanExampleOne.Apply();                                             //!2 Applied
        PlanExampleOne.PlanDisplayFormulaOutput();
        Console.WriteLine();

        return PlanToBeReturned;
    }

    /// <summary>
    /// * This method is responsible for testing the functionality of the ExecutablePlan class.
    /// * It creates an ExecutablePlan instance, performs various operations on it,
    ///   and displays the resources and formula output at different stages.
    /// </summary>
    /// 
    /// <returns>
    /// * An instance of the ExecutablePlan class that has undergone various operations.
    /// </returns>
    static ExecutablePlan? TestExecutablePlan()
    {
        ExecutablePlan ExecutablePlanExampleOne = new ExecutablePlan(InstantiateFomrulaArray(), 0);
        ExecutablePlan? ExecutablePlanToBeReturned = ExecutablePlanExampleOne.DeepCopy() as ExecutablePlan;

        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.AddFormula(DummyObjX);                   //!3
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.ReplaceFormula(DummyObjY, 1);            //!3
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.RemoveLastFormula();                     //!2
        ExecutablePlanExampleOne.PlanDisplayResources();
        Console.WriteLine();

        ExecutablePlanExampleOne.Apply();                                 //!2 Applied
        ExecutablePlanExampleOne.PlanDisplayFormulaOutput();
        Console.WriteLine();

        return ExecutablePlanToBeReturned;
    }

    /// <summary>
    /// * This method is responsible for testing the functionality of the Stockpile class.
    /// * It creates a ConcurrentDictionary representing a stockpile of resources,
    ///   initializes a Stockpile instance, and performs various operations on it.
    /// </summary>
    /// 
    /// <returns>
    /// * An instance of the Stockpile class that has undergone various operations.
    /// </returns>
    /// 
    /// <exception cref="ArgumentException">Thrown when the test for resource containment fails.</exception>
    static Stockpile? TestStockpile()
    {
        ConcurrentDictionary<string, uint>? StockpileDictionary = new ConcurrentDictionary<string, uint>();

        StockpileDictionary.TryAdd("A1", 1);
        StockpileDictionary.TryAdd("B1", 1);

        StockpileDictionary.TryAdd("A2", 2);
        StockpileDictionary.TryAdd("B2", 2);

        StockpileDictionary.TryAdd("A3", 3);
        StockpileDictionary.TryAdd("B3", 3);

        StockpileDictionary.TryAdd("A4", 4);
        StockpileDictionary.TryAdd("B4", 4);

        Stockpile StockpileExample = new Stockpile(StockpileDictionary);
        Stockpile? StockpileToBeReturned = StockpileExample.DeepCopy() as Stockpile;

        StockpileExample.IncreaseQuantity("A1", 10);
        StockpileExample.DecreaseQuantity("B3", 1);

        bool ConatainsA = StockpileExample.ContainsResource("A2");

        if (ConatainsA == false) { throw new ArgumentException("[Contains resource failed]");  }
        return StockpileToBeReturned;
    }

    /// <summary>
    /// * This method is responsible for testing the functionality of the Assembly class.
    /// * It creates instances of Plan, ExecutablePlan, and Stockpile, and uses them to
    ///   create two different instances of the Assembly class.
    /// </summary>
    /// 
    /// <returns>
    /// * An instance of the Assembly class that has undergone various operations.
    /// </returns>
    static Assembly? TestAssembly()
    {
        Plan? InternalAssemblyPlan = TestPlan();
        ExecutablePlan? InternalAssemblyExecutablePlan = TestExecutablePlan();

        Stockpile? InternalAssemblyStockpile = TestStockpile();

        Assembly? AssemblyPlanExample = new Assembly(InternalAssemblyPlan, InternalAssemblyStockpile);
        Assembly? AssemblyExecutablePlanExample = new Assembly(InternalAssemblyExecutablePlan, InternalAssemblyStockpile);

        Assembly? AssembyToBeReturned = AssemblyPlanExample.DeepCopy() as Assembly;


        AssemblyPlanExample.AddFormula(DummyObjX);
        AssemblyExecutablePlanExample.AddFormula(DummyObjY);

        AssemblyPlanExample.RemoveLastFormula();
        AssemblyExecutablePlanExample.RemoveLastFormula();

        const ushort ValidIndex = 0;

        AssemblyPlanExample.ReplaceFormula(DummyObjX, ValidIndex);
        AssemblyExecutablePlanExample.ReplaceFormula(DummyObjY, ValidIndex);

        AssemblyPlanExample.Apply();
        AssemblyExecutablePlanExample.Apply();

        AssemblyPlanExample.IncreaseQuantity("A1", 10);
        AssemblyExecutablePlanExample.DecreaseQuantity("B3", 1);

        AssemblyPlanExample.Restock();
        AssemblyExecutablePlanExample.Restock();

        return AssembyToBeReturned;
    }

    /// <summary>
    /// * This method is used to test the handling of heterogeneous collections (HC).
    /// * It populates two arrays, HCObjOne and HCObjTwo, with instances of different classes.
    /// </summary>
    static void TestHC()
    {
        Plan? PlanObj = TestPlan();
        ExecutablePlan? ExPlanObj = TestExecutablePlan();
        Stockpile? StockpileObj = TestStockpile();
        Assembly? AssemblyObj = TestAssembly();

        IPlan[] HCObjOne = new IPlan[10];
        IStockpile[] HCObjTwo = new IStockpile[10];

        ///<note>
        /// * Seed the RNG engine only once
        /// </note>
        Random random = new Random();

        /// <summary>
        /// * Get's a random initger determeniing what type of obj is stored in the HC
        /// </summary>
        Func<int> getRandomNumberFunc = () => { return random.Next(1, 101); };

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("-------\n[Elements in the first HC]\n");
        Console.ResetColor();

        for (ushort i = 0; i < HCObjOne.Length; i++)
        {
            int RandomNumber = getRandomNumberFunc.Invoke();

            if (RandomNumber <= 34) { HCObjOne[i] = PlanObj; }
            else if (RandomNumber > 34 && RandomNumber <= 67) { HCObjOne[i] = ExPlanObj; }
            else { HCObjOne[i] = AssemblyObj; }
            Console.WriteLine($"HCObjOne[{i}]: {HCObjOne[i]}");
        }

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("-------\n[Elements in the second HC]\n");
        Console.ResetColor();
        for (ushort i = 0; i < HCObjTwo.Length; i++)
        {
            int RandomNumber = getRandomNumberFunc.Invoke();

            if (RandomNumber <= 50) { HCObjTwo[i] = StockpileObj; }
            else { HCObjTwo[i] = AssemblyObj; }
            Console.WriteLine($"HCObjTwo[{i}]: {HCObjTwo[i]}");
        }

        for (int i = 0; i < HCObjOne.Length; i++)
        {
            if (HCObjOne[i] != null)
            {
                HCObjOne[i].AddFormula(DummyObjX);
            }
            
            if (HCObjTwo[i] != null)
            {
                HCObjTwo[i].Restock();
            }
        }
        return;
    }

    /// <summary>
    /// * Entry point of the application.
    /// </summary>
    /// 
    /// <param name="args">Command-line arguments. [maybe_unused]</param>
    /// 
    /// <remarks>
    /// * This method demonstrates the functionality of the Formula class by creating an array of
    ///   formula instances and displaying input and output values.
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

            // Print the stack trace
            Console.WriteLine("\n[StackTrace]:");
            Console.WriteLine(error.StackTrace);

            Console.Write(" -> [press <Enter> to continue]");
            Console.ReadLine();
        };

        Driver _Driver = new Driver();
        try
        {
            TestHC();
        }
        catch (Exception ErrorThrown)
        {
            if (_Driver.Display)
                PrintErrorMessage(ErrorThrown);
        }
        finally
        {
            Console.ReadLine(); //remove for submission
        }


    }
}

