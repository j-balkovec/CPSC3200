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
using System;

class Driver
{
    /// <summary>
    /// * Displays the input and output values for a resource conversion.
    /// </summary>
    /// 
    /// <param name="InputResources">An array of input resource names.</param>
    /// <param name="InputQuantities">An array of input resource quantities.</param>
    /// <param name="OutputResources">An array of output resource names.</param>
    /// <param name="OutputQuantities">An array of output resource quantities.</param>
    /// 
    /// <exception cref="ArgumentException">
    /// * Thrown if the lengths of InputResources and InputQuantities arrays do not match.
    /// </exception>
    /// 
    /// <remarks>
    /// <precondition>
    /// * The InputResources and InputQuantities arrays must not be null.
    /// * The lengths of InputResources and InputQuantities arrays must match.
    /// </precondition>
    /// 
    /// <postcondition>
    /// * If the precondition is met, the method displays the input and output values.
    /// </postcondition>
    /// </remarks>
    static void DisplayValues(string[] InputResources,
                              uint[] InputQuantities,
                              string[] OutputResources,
                              uint[] OutputQuantities)
    {
        if (InputResources.Length != InputQuantities.Length)
        {
            throw new ArgumentException(
                                        $"[lengths of [IN] -> Resources array doesn't match " +
                                        $"the [IN] -> Quantity array]"
                                        );
        }

        Console.Write("{");
        for (ushort i = 0; i < InputResources.Length; i++)
        {
            Console.Write($"{InputResources[i]}: {InputQuantities[i]} ");
        }
        Console.Write("} >>> {");


        if (OutputQuantities.Length == 0)
        {
            Console.Write("}\n"); // {0: cookies} || {}
        }
        else
        {
            for (ushort i = 0; i < OutputResources.Length; i++)
            {
                Console.Write($"{OutputResources[i]}: {OutputQuantities[i]} ");
            }
            Console.Write("}\n");
        }
    }

    /// <summary>
    /// * Prints an error message with a custom format and waits for user input
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
    static void PrintErrorMessage(Exception ErrorThrown)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"\n[error]: {ErrorThrown.Message}");
        Console.ResetColor();
        Console.Write(" -> [press <Enter> to continue]");
        Console.ReadLine();
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
        const ushort ExitFailure = 1;

            try
            {
            const uint ProficiencyLevel = 3;

            string[] InputResourcesOne = { "Iron Ore" };
            uint[] InputQuantitiesOne = { 2 };
            string[] OutputResourcesOne = { "Iron Bar" };
            uint[] OutputQuantitiesOne = { 1 };

            string[] InputResourcesTwo = { "Iron Ore", "Coal" };
            uint[] InputQuantitiesTwo = { 3, 1 };
            string[] OutputResourcesTwo = { "Steel Bar" };
            uint[] OutputQuantitiesTwo = { 1};

            string[] InputResourcesThree = { "Butter", "Eggs", "Sugar", "Flour", "Baking Soda" };
            uint[] InputQuantitiesThree = { 2, 3, 1, 2, 2 };
            string[] OutputResourcesThree = { "Cookies" };
            uint[] OutputQuantitiesThree = { 36 };

            Formula ExampleOne = new Formula(InputResourcesOne,
                                             InputQuantitiesOne,
                                             OutputResourcesOne,
                                             OutputQuantitiesOne,
                                             ProficiencyLevel);

            Formula ExampleTwo = new Formula(InputResourcesTwo,
                                             InputQuantitiesTwo,
                                             OutputResourcesTwo,
                                             OutputQuantitiesTwo,
                                             ProficiencyLevel);

            Formula ExampleThree = new Formula(InputResourcesThree,
                                               InputQuantitiesThree,
                                               OutputResourcesThree,
                                               OutputQuantitiesThree,
                                               ProficiencyLevel);


            Formula[] ExamplesArray = new Formula[3] { ExampleOne, ExampleTwo, ExampleThree };

            uint[] ExampleResultOne = ExamplesArray[0].Apply();
            DisplayValues(InputResourcesOne,
                          InputQuantitiesOne,
                          OutputResourcesOne,
                          ExampleResultOne);

            uint[] ExampleResultTwo = ExamplesArray[1].Apply();
            DisplayValues(InputResourcesTwo,
                          InputQuantitiesTwo,
                          OutputResourcesTwo,
                          ExampleResultTwo);

            uint[] ExampleResultThree = ExamplesArray[2].Apply();
            DisplayValues(InputResourcesThree,
                          InputQuantitiesThree,
                          OutputResourcesThree,
                          ExampleResultThree);

        }
        catch (Exception ErrorThrown)
            {
                PrintErrorMessage(ErrorThrown);
                Environment.Exit(ExitFailure);

            }
        Console.ReadLine();
    }
}