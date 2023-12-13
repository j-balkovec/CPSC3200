//[FILE]: Driver.cs
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Sun 24th Sep
//
//[REVISION HISTORY]
//- 1.0 [09/24/2023]: Initial skeleton
//- 2.0 [09/25/2023]: DisplayValues
//
//[DESC]: -This file contains the shows the
//         functionality and usage of the Formula and Plan class.
//[VALGRIND]: valgrind --leak-check=full --track-origins=yes --show-leak-kinds=all --log-file="p4_valgrind.log" ./main
#include <iostream>
#include <new>
#include <memory>
#include <unordered_map>
#include <functional>

#include "Formula.h"
#include "Plan.h"
#include "ExecutablePlan.h"
#include "Stockpile.h"

namespace Driver {

    //[NAMESPACE]: For testing operators
    namespace TestOperators
    {
        auto PrintTestTag = [](std::string TestCode) -> void {
            const std::string GREEN = "\033[1;32m"; 
            const std::string RESET = "\033[0m";

            std::cout << GREEN << "\n[TESTING OPERATOR]: " << TestCode << RESET;
        };
    }//[NAMESPACE]: TestOperators

    //[NAMESPACE]: Imported namespace {Formula, Plan, ExecutablePlan, Stockpile}
    using namespace ResourceConversion;

    // [NOTE]: Meyers Singleton Pattern
    //         - The constructor of the `Example` class is private to prevent direct instantiation of the
    //           class, ensuring that all access to the single instance is done through the `Instance()`
    //           method.
    // [DESC]: The 'Example' class implements the Meyers Singleton pattern, which ensures that only one
    // instance of the class is created and provides a global point of access to that instance.
    class Example
    {
    private:
        Example() {}
    public:
    

        //[DESC]: Get the instance of the `Example` class. This method returns a reference to the single
        //        instance of the `Example` class. If the instance does not exist, it is created during the
        //        first call to this function.
        //[RETURN]: Reference to the single instance of the `Example` class.
        static Example& Instance()
        {
            static Example Instance_;
            return Instance_;
        }

    //[DESC]: Initialize Formula objects with provided data.
    //
    //[NOTE]: This function initializes four Formula objects with the provided data, which includes
    //        arrays of strings and unsigned integers, as well as other parameters. The Formula objects
    //        are passed by reference and are updated with the initialized data.
    //
    //[PARAM]: FExm1 - Reference to the first Formula object to be initialized.
    //[PARAM]: FExm2 - Reference to the second Formula object to be initialized.
    //[PARAM]: FExm3 - Reference to the third Formula object to be initialized.
    //[PARAM]: FExm4 - Reference to the fourth Formula object to be initialized.
        static void InitFormulas(Formula &FExm1, Formula &FExm2, Formula &FExm3, Formula &FExm4)
        {
            const size_t InRS1 = 2;
            const size_t InQS1 = 2;
            const size_t OutRS1 = 1;
            const size_t OutQS1 = 1;

            std::string* InR1 = new (std::nothrow) std::string[InRS1]{ "A1", "B1" };
            std::string* OutR1 = new (std::nothrow) std::string[OutRS1]{ "C1" };
            unsigned int* InQ1 = new (std::nothrow) unsigned int[InQS1] {1, 2};
            unsigned int* OutQ1 = new (std::nothrow) unsigned int[OutQS1] {3};
            unsigned int* Result1 = new (std::nothrow) unsigned int [OutQS1] {0};

            const size_t InRS2 = 2;
            const size_t InQS2 = 2;
            const size_t OutRS2 = 1;
            const size_t OutQS2 = 1;

            std::string* InR2 = new (std::nothrow) std::string[InRS2]{ "A2", "B2" };
            std::string* OutR2 = new (std::nothrow) std::string[OutRS2]{ "C2" };
            unsigned int* InQ2 = new (std::nothrow) unsigned int[InQS2] {4, 5};
            unsigned int* OutQ2 = new (std::nothrow) unsigned int[OutQS2] {6};
            unsigned int* Result2 = new (std::nothrow) unsigned int [OutQS2] {0};

            const size_t InRS3 = 2;
            const size_t InQS3 = 2;
            const size_t OutRS3 = 1;
            const size_t OutQS3 = 1;

            std::string* InR3 = new (std::nothrow) std::string[InRS3]{ "A3", "B3" };
            std::string* OutR3 = new (std::nothrow) std::string[OutRS3]{ "C3" };
            unsigned int* InQ3 = new (std::nothrow) unsigned int[InQS3] {7, 8};
            unsigned int* OutQ3 = new (std::nothrow) unsigned int[OutQS3] {9};
            unsigned int* Result3 = new (std::nothrow) unsigned int [OutQS3] {0};

            const size_t InRS4 = 2;
            const size_t InQS4 = 2;
            const size_t OutRS4 = 1;
            const size_t OutQS4 = 1;

            std::string* InR4 = new (std::nothrow) std::string[InRS4]{ "A4", "B4" };
            std::string* OutR4 = new (std::nothrow) std::string[OutRS4]{ "C4" };
            unsigned int* InQ4 = new (std::nothrow) unsigned int[InQS4] {8, 2};
            unsigned int* OutQ4 = new (std::nothrow) unsigned int[OutQS4] {3};
            unsigned int* Result4 = new (std::nothrow) unsigned int [OutQS4] {0};

            const unsigned int ProfLvl = 0;

            FExm1 = Formula(InR1, InRS1, InQ1, InQS1, OutR1, OutRS1, OutQ1, OutQS1, Result1, ProfLvl);
            FExm2 = Formula(InR2, InRS2, InQ2, InQS2, OutR2, OutRS2, OutQ2, OutQS2, Result2, ProfLvl);
            FExm3 = Formula(InR3, InRS3, InQ3, InQS3, OutR3, OutRS3, OutQ3, OutQS3, Result3, ProfLvl);
            FExm4 = Formula(InR4, InRS4, InQ4, InQS4, OutR4, OutRS4, OutQ4, OutQS4, Result4, ProfLvl);
        }

        //[DESC]: Runs an example using the Plan class.
        //        - This function demonstrates the use of the Plan class by setting up multiple formulas,
        //          applying the plan, and displaying the results. It also handles exceptions that may occur
        //          during execution.
        //
        //[NOTE]: The function creates formulas, adds and removes them from the plan, and applies the plan.
        //        It also showcases the use of copy constructors and assignment operators.
        //
        //[POST]: The function initializes Formula and Plan objects, applies the plan, and displays the
        //        results.
        //
        //[THROW]: If any exceptions occur during execution, they are caught and handled.
        //
        //[ATTENTION]: Move Constructor and Operator are invoked when returning a local object by value.
        //[ATTENTION]: Example of Copy Constructor and Overloaded Assignment Operator usage are
        static void Run()
        {
            Formula FExm1; 
            Formula FExm2;
            Formula FExm3;
            Formula FExm4;

            InitFormulas(FExm1, FExm2, FExm3, FExm4);

            constexpr size_t ValidIndex = 1;
            constexpr bool PrintResult = true;

            const size_t FormulaSeqSize = 2;
            const std::string Line = "\n____________________________________________________________\n";
            
            Formula FormulaSeq1[FormulaSeqSize] = { FExm1, FExm2};          //?2
            ExecutablePlan XPlanExample (FormulaSeq1, FormulaSeqSize, 0);

            XPlanExample.PlanDisplayValues();
            std::cout << "\n";

            XPlanExample.AddFormula (FExm3);                                //?3
            XPlanExample.PlanDisplayValues();
            std::cout << "\n";

            XPlanExample.ReplaceFormula(FExm4, ValidIndex);                 //?3
            XPlanExample.PlanDisplayValues();
            std::cout << "\n";

            XPlanExample.RemoveLastFormula();                               //?2
            XPlanExample.PlanDisplayValues();
            std::cout << "\n";

            XPlanExample.PlanApply();                                       //?2 Applied
            XPlanExample.PlanDisplayValues(PrintResult);
            std::cout << "\n";
            
            ExecutablePlan XPlanOne = ExecutablePlan(XPlanExample);
            ExecutablePlan XPlanTwo = XPlanOne;

            std::cout << "\n\n\n<-- [Copy/Move Semantics Segment] -->";
            std::cout << Line;
            std::cout << "\n<-- [Original Object] -->\n";
            XPlanExample.PlanDisplayValues();

            std::cout << "\n\n\n<-- [Copy Constructor] -->\n";
            XPlanOne.PlanDisplayValues();
            
            std::cout << "\n\n\n<-- [Copy Assignment Operator] -->\n";
            XPlanTwo.PlanDisplayValues();
            std::cout << Line;
            std::cout << "\n";
        }
};

    // [DESC]: Test the Stockpile class by demonstrating resource management operations.
    // [NOTE]: This function is designed to test the Stockpile class, which represents a collection
    //         of resources and provides methods to increase and decrease the quantity of resources.
    static inline void TestStockpile()
    {
        Formula FExm1; 
        Formula FExm2;
        Formula FExm3;
        Formula FExm4;

        Example::InitFormulas(FExm1, FExm2, FExm3, FExm4);

        const std::size_t FormulaSeqSize = 2;
        constexpr unsigned int Step = 0;
        Formula FormulaSeq[FormulaSeqSize] = { FExm1, FExm2 };

        ExecutablePlan ExPlanMockTest(FormulaSeq, FormulaSeqSize, Step);
        std::unordered_map<std::string, size_t> StockpileMap = 
        {
            {"A1", 1},
            {"B1", 2},
            {"C1", 3},
            {"A2", 4},
            {"B2", 5},
            {"C2", 6},
        };
        const std::shared_ptr<Stockpile>& MockStockpile = std::make_shared<Stockpile>(StockpileMap);

        std::shared_ptr<Stockpile> ResultStockpile = ExPlanMockTest.PlanApply(MockStockpile);
        ExPlanMockTest.PlanDisplayValues(true);
        std::cout << "\n";
    }

    // [DESC]: Test various operators and operations on Formula objects.
    // [NOTE]: This function is designed to test and demonstrate the behavior of operators and
    //         operations defined for Formula objects. It covers comparison operators, post-
    //         and pre-increment/decrement, addition, and subtraction, among others.
    static inline void TestFormulaOperators()
    {
        Formula FExm1; 
        Formula FExm2;
        Formula FExm3;
        Formula FExm4;

        Example::InitFormulas(FExm1, FExm2, FExm3, FExm4);

        FExm1.DisplayFormulaValues();
        FExm2.DisplayFormulaValues();

        bool TestOne = (FExm1 == FExm2);
        TestOperators::PrintTestTag("<==>");
        std::cout << "\t" << std::boolalpha << TestOne;

        bool TestTwo = (FExm1 != FExm3);
        TestOperators::PrintTestTag("<!=>");
        std::cout << "\t" << std::boolalpha << TestTwo << std::endl;

        TestOperators::PrintTestTag("<[ORIGINAL]>");
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[POST++]>");
        FExm3++;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[POST--]>");
        FExm3--;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[++PRE]>");
        ++FExm3;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[--PRE]>");
        --FExm3;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[+=]>");
        FExm3 += 1;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[-=]>");
        FExm3 -= 1;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[+]>");
        FExm3 = FExm3 + 1;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[-]>");
        FExm3 = FExm3 - 1;
        FExm3.DisplayFormulaValues();

        TestOperators::PrintTestTag("<[DEFAULT]>");
        FExm3.DisplayFormulaValues();
        std::cout << std::endl;
        return;
    }

    // [DESC]: Test various operators and operations on Plan objects.
    // [NOTE]: This function is designed to test and demonstrate the behavior of operators and operations
    //         defined for Plan objects. It covers comparison operators (equality and relational),
    //         increment/decrement, addition, subtraction, and display methods.
    static inline void TestPlanOperators()
    {
        Formula FExm1; 
        Formula FExm2;
        Formula FExm3;
        Formula FExm4;

        Example::InitFormulas(FExm1, FExm2, FExm3, FExm4);
        Formula SeqOne[1] = {FExm1};
        Formula SeqTwo[1] = {FExm2};
        Formula SeqThree[2] = {FExm3, FExm4};
        
        Plan ObjectOne(SeqOne, 1);
        Plan ObjectTwo(SeqTwo, 1);
        Plan ObjectThree(SeqThree, 2);
        
        bool TestOne = (ObjectOne == ObjectTwo);
        TestOperators::PrintTestTag("<==>");
        std::cout << "\t" << std::boolalpha << TestOne;

        bool TestTwo = (ObjectOne != ObjectThree);
        TestOperators::PrintTestTag("<!=>");
        std::cout << "\t" << std::boolalpha << TestTwo << std::endl;

        bool TestThree = (ObjectThree > ObjectOne);
        TestOperators::PrintTestTag("< > >");
        std::cout << "\t" << std::boolalpha << TestThree;

        bool TestFour = (ObjectOne < ObjectThree);
        TestOperators::PrintTestTag("< < >");
        std::cout << "\t" << std::boolalpha << TestFour << std::endl;

        bool TestFive = (ObjectOne <= ObjectOne);
        TestOperators::PrintTestTag("< <= >");
        std::cout << "\t" << std::boolalpha << TestFive;

        bool TestSix = (ObjectOne >= ObjectOne);
        TestOperators::PrintTestTag("< >= >");
        std::cout << "\t" << std::boolalpha << TestSix << std::endl;

        TestOperators::PrintTestTag("<[ORIGINAL]>");
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[POST++]>");
        ObjectOne++;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[POST--]>");
        ObjectOne--;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[++PRE]>");
        ++ObjectOne;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[--PRE]>");
        --ObjectOne;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[+=]>");
        ObjectOne += 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[-=]>");
        ObjectOne -= 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[+]>");
        ObjectOne = ObjectOne + 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[-]>");
        ObjectOne = ObjectOne - 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[DEFAULT]>");
        ObjectOne.PlanDisplayValues();
        std::cout << std::endl;
    }

    // [DESC]: Test various operators and operations on ExecutablePlan objects.
    // [NOTE]: This function is designed to test and demonstrate the behavior of operators and operations
    //         defined for ExecutablePlan objects. It covers comparison operators (equality and relational),
    //         increment/decrement, addition, subtraction, and display methods specific to ExecutablePlan objects.
    static inline void TestExecutablePlanOperators()
    {
        Formula FExm1; 
        Formula FExm2;
        Formula FExm3;
        Formula FExm4;

        Example::InitFormulas(FExm1, FExm2, FExm3, FExm4);
        Formula SeqOne[1] = {FExm1};
        Formula SeqTwo[1] = {FExm2};
        Formula SeqThree[2] = {FExm3, FExm4};

        ExecutablePlan ObjectOne(SeqOne, 1, 0);
        ExecutablePlan ObjectTwo(SeqTwo, 1, 0);
        ExecutablePlan ObjectThree(SeqThree, 2, 1);

        bool TestOne = (ObjectOne == ObjectTwo);
        TestOperators::PrintTestTag("<==>");
        std::cout << "\t" << std::boolalpha << TestOne;

        bool TestTwo = (ObjectOne != ObjectThree);
        TestOperators::PrintTestTag("<!=>");
        std::cout << "\t" << std::boolalpha << TestTwo << std::endl;

        bool TestThree = (ObjectThree > ObjectOne);
        TestOperators::PrintTestTag("< > >");
        std::cout << "\t" << std::boolalpha << TestThree;

        bool TestFour = (ObjectOne < ObjectThree);
        TestOperators::PrintTestTag("< < >");
        std::cout << "\t" << std::boolalpha << TestFour << std::endl;

        bool TestFive = (ObjectOne <= ObjectOne);
        TestOperators::PrintTestTag("< <= >");
        std::cout << "\t" << std::boolalpha << TestFive;

        bool TestSix = (ObjectOne >= ObjectOne);
        TestOperators::PrintTestTag("< >= >");
        std::cout << "\t" << std::boolalpha << TestSix << std::endl;

        TestOperators::PrintTestTag("<[ORIGINAL]>");
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[POST++]>");
        ObjectOne++;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[POST--]>");
        ObjectOne--;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[++PRE]>");
        ++ObjectOne;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[--PRE]>");
        --ObjectOne;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[+=]>");
        ObjectOne += 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[-=]>");
        ObjectOne -= 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[+]>");
        ObjectOne = ObjectOne + 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[-]>");
        ObjectOne = ObjectOne - 1;
        ObjectOne.PlanDisplayValues();

        TestOperators::PrintTestTag("<[DEFAULT]>");
        ObjectOne.PlanDisplayValues();
        std::cout << std::endl;
    }

//[DESC]: Entry point of the program.
//    - This function serves as the starting point of the program and controls the program's
//      execution flow.
//[NOTE]: Exceptions 'e' is re-throw to propagate it
inline void InitAndRun()
{
    try{ Example::Instance().Run(); } catch (std::exception& Error)
    {
        std::cout << "{Error}: " << Error.what () << std::endl;
        throw Error;
    }
}
} //[NAMESPACE]: Driver

int main ([[maybe_unused]]int argc, [[maybe_unused]]const char* argv[]) {
    Driver::InitAndRun();
    return 0;
}
