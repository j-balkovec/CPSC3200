/**
 * @file Driver.cs
 * @author Jakob Balkovec (CPSC 3200)
 * @instructor A. Dingle (CPSC 3200)
 *
 * @date Sun 24th Sep
 *
 * @version
 * Revision History
 * - 1.0 [09/24/2023]: Initial skeleton
 * - 2.0 [09/25/2023]: DisplayValues
 *
 * @brief
 * - This file contains the definition of the Driver class, which shows the
 * - functionality and usage of the Formula class
 *
 * @depends
 * - This class does not have any external dependencies
*/
#include <iostream>

#include "Formula.h"
#include "Plan.h"

using namespace ResourceConversion;

/**
 * @brief Displays the values of input and output resources and quantities.
 *
 * @param InputResources An array of input resource names.
 * @param InputResourcesSize The size of the input resources array.
 * @param InputQuantities An array of input quantities.
 * @param InputQuantitiesSize The size of the input quantities array.
 * @param OutputResources An array of output resource names.
 * @param OutputResourcesSize The size of the output resources array.
 * @param ResultArray An array of resulting quantities.
 * @param ResultArraySize The size of the result array.
 *
 * @post Displays the values of input and output resources and quantities.
 *       The function does not modify the provided arrays.
 */
void DisplayResultValues (std::string InputResources[], size_t InputResourcesSize,
    unsigned int InputQuantities[], size_t InputQuantitiesSize,
    std::string  OutputResources[], size_t OutputResourcesSize,
    unsigned int* ResultArray, size_t ResultArraySize)
{
    std::cout << "\n";
    if (InputResourcesSize != InputQuantitiesSize)
    {
        throw std::invalid_argument ("[lengths of [IN] -> Resources array doesn't match the [IN] -> Quantity array]");
    }

    std::cout << "{";
    for (size_t i = 0; i < InputResourcesSize; ++i)
    {
        std::cout << "[" << InputResources[i] << "]: [" << InputQuantities[i] << "]";
    }
    std::cout << "} <===> {";

    for (size_t i = 0; i < OutputResourcesSize; ++i)
    {
        std::cout << "[" << OutputResources[i] << "]: [" << ResultArray[i] << "]";
    }
    std::cout << "}";

}

/**
 * @brief Displays the values of input resources, input quantities, output resources, and output quantities.
 *
 * @param InputResources An array of input resource names.
 * @param InputResourcesSize The size of the input resources array.
 * @param InputQuantities An array of input quantities.
 * @param InputQuantitiesSize The size of the input quantities array.
 * @param OutputResources An array of output resource names.
 * @param OutputResourcesSize The size of the output resources array.
 * @param OutputQuantities An array of output quantities.
 * @param OutputQuantitiesSize The size of the output quantities array.
 *
 * @post Displays the values of input resources, input quantities, output resources, and output quantities.
 *       The function does not modify the provided arrays.
 */
void DisplayValues (std::string  InputResources[], size_t InputResourcesSize,
    unsigned int InputQuantities[], size_t InputQuantitiesSize,
    std::string  OutputResources[], size_t OutputResourcesSize,
    unsigned int OutputQuantites[], size_t OutputQuantitesSize)
{
    std::cout << "\n";
    if (InputResourcesSize != InputQuantitiesSize)
    {
        throw std::invalid_argument ("[lengths of [IN] -> Resources array doesn't match the [IN] -> Quantity array]");
    }

    std::cout << "{";
    for (size_t i = 0; i < InputResourcesSize; ++i)
    {
        std::cout << "[" << InputResources[i] << "]: [" << InputQuantities[i] << "]";
    }
    std::cout << "} <---> {";

    for (size_t i = 0; i < OutputQuantitesSize; ++i)
    {
        std::cout << "[" << OutputResources[i] << "]: [" << OutputQuantites[i] << "]";
    }
    std::cout << "}";
}

/**
 * @brief Runs an example using the Plan class.
 * - This function demonstrates the use of the Plan class by setting up multiple formulas, applying the plan,
 *   and displaying the results. It also handles exceptions that may occur during execution.
 *
 * @note The function creates formulas, adds and removes them from the plan, and applies the plan.
 *       It also showcases the use of copy constructors and assignment operators.
 *
 * @post The function initializes Formula and Plan objects, applies the plan, and displays the results.
 *
 * @throw std::exception If any exceptions occur during execution, they are caught and handled.
 *
 * @attention Move Constructor and Operator are invoked when returning a local object by value.
 * @attention Example of Copy Constructor and Overloaded Assignment Operator usage are demonstrated below.
 */
void RunExample ()
{
    const size_t InRS1 = 2;
    const size_t InQS1 = 2;
    const size_t OutRS1 = 1;
    const size_t OutQS1 = 1;

    std::string* InR1 = new std::string[InRS1]{ "A1", "B1" };
    std::string* OutR1 = new std::string[OutRS1]{ "C1" };
    unsigned int* InQ1 = new unsigned int[InQS1] {1, 2};
    unsigned int* OutQ1 = new unsigned int[OutQS1] {3};
    unsigned int* Result1 = new unsigned int [OutQS1] {0};

    const size_t InRS2 = 2;
    const size_t InQS2 = 2;
    const size_t OutRS2 = 1;
    const size_t OutQS2 = 1;

    std::string* InR2 = new std::string[InRS2]{ "A2", "B2" };
    std::string* OutR2 = new std::string[OutRS2]{ "C2" };
    unsigned int* InQ2 = new unsigned int[InQS2] {4, 5};
    unsigned int* OutQ2 = new unsigned int[OutQS2] {6};
    unsigned int* Result2 = new unsigned int [OutQS2] {0};

    const size_t InRS3 = 2;
    const size_t InQS3 = 2;
    const size_t OutRS3 = 1;
    const size_t OutQS3 = 1;

    std::string* InR3 = new std::string[InRS3]{ "A3", "B3" };
    std::string* OutR3 = new std::string[OutRS3]{ "C3" };
    unsigned int* InQ3 = new unsigned int[InQS3] {7, 8};
    unsigned int* OutQ3 = new unsigned int[OutQS3] {9};
    unsigned int* Result3 = new unsigned int [OutQS3] {0};

    const unsigned int ProfLvl = 0;

    Formula FExm1 (InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, OutQ1, OutQS1, Result1, ProfLvl);
    Formula FExm2 (InR2, InRS2, InQ2, InQS2, OutR2, OutRS2, OutQ2, OutQS2, Result2, ProfLvl);
    Formula FExm3 (InR3, InRS3, InQ3, InQS3, OutR3, OutRS3, OutQ3, OutQS3, Result3, ProfLvl);


    const size_t FormulaSeqSize = 3;
    Formula FormulaSeq1[FormulaSeqSize] = { FExm1, FExm2, FExm3 };
    Plan PlanExample (FormulaSeq1, FormulaSeqSize);

    std::cout << "\n[Current Plan]: ";
    DisplayValues (InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, OutQ1, OutQS1);
    DisplayValues (InR2, InRS2, InQ2, InQS2, OutR2, OutRS2, OutQ2, OutQS2);
    DisplayValues (InR3, InRS3, InQ3, InQS3, OutR3, OutRS3, OutQ3, OutQS3);

    PlanExample.RemoveLastFormula ();
    PlanExample.RemoveLastFormula ();

    std::cout << "\n\n[Plan after removing 2 Formulas {FExm2, FExm3}]: ";
    DisplayValues (InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, OutQ1, OutQS1);

    PlanExample.AddFormula (FExm3);
    std::cout << "\n\n[Plan after adding a Formula {FExm3}]: ";
    DisplayValues (InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, OutQ1, OutQS1);
    DisplayValues (InR3, InRS3, InQ3, InQS3, OutR3, OutRS3, OutQ3, OutQS3);

    PlanExample.ReplaceFormula (1, FExm2);
    std::cout << "\n\n[Plan after replacing a Fomrula at the index 1 {FExm2 with FExm3}]:";
    DisplayValues (InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, OutQ1, OutQS1);
    DisplayValues (InR2, InRS2, InQ2, InQS2, OutR2, OutRS2, OutQ2, OutQS2);

    PlanExample.PlanApply ();
    std::cout << "\n\n[Plan after Apply() {FExm1, FExm2}]: ";
    DisplayResultValues (InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, PlanExample[0].GetResultArray (), OutQS1);
    DisplayResultValues (InR2, InRS2, InQ2, InQS2, OutR2, OutRS2, PlanExample[1].GetResultArray (), OutQS2);

    Plan PlanExample2 = PlanExample; //
    Plan PlanExample3 = Plan (PlanExample2);
}

/**
 * @brief Entry point of the program.
 *
 * @param argc Number of command-line arguments.
 * @param argv Array of command-line argument strings.
 * @return Exit code, 0 for success.
 */
int main (int argc, const char* argv[]) {
    try
    {
        RunExample ();
    }
    catch (std::exception& e)
    {
        std::cerr << "{Error}: " << e.what () << std::endl;
    }
    std::cout << "\n";
    return 0;
}
