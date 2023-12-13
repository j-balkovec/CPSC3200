//[FILE]: Formula.h
//[DESC]: This file contains the definition of the Formula class, which represents
//        a way of converting resources into other resources. The Formula class
//        encapsulates information about input resources, output resources,
//        chances of outcomes, proficiency levels, and the ability to apply the formula
//        to simulate outcomes. The class supports shallow and efficeint deep copying through move
//        semantics. {[SEE]: [MOVE SEMANTICS]} The class also support some simple mathemtical 
//        operatoions through overloaded operators. {[SEE]: [OPERATORS]}
//
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Fri 27th Oct
//
//[VERSION]: Revision History
//           - 1.0 [27/10/2023]: Optimisation and Impored Move Semantics
//           - 2.0 [28/10/2023]: Debugging
//           - 3.0 [28/10/2023]: Documentation
//
//[INVARIANT]: Resource Arrays should contain valid Strings and should not be empty
//[INVARIANT]: Quantity Arrays should contain Non-Negative Quantities and should not be empty
//[INVARIANT]: Proficiency Level should be Non-Negative and within the valid range
//
//[MOVE SEMANTICS]
//{
// Plan Obj1(...); -> Initial Construction
//
// Plan Obj2 = Plan(Obbj1) -> Copy Ctor
// Plan Obj3 = Obj2 -> Copy Assignment Operator
//
// Plan Obj4 = Plan(std::move(Obj1)) -> Move Ctor
// Plan Obj5 = std::move(Obj2) -> Move Assignment Operator
//}
//
//[OPERATORS]
//{
// Example use case of Overloaded operatrors -> Assume: Objects proprerly initialised
//
// Formula Object1(...);
// Formula Object2(...);
//
// Object1 == Object2;
// Object1 != Object2;
//
// Object1++;
// Object1--;
//
// ++Object1;
// --Object1;
//
// Object1 += 1;
// Object1 -= 1;
//
// Object1 = Object1 + 1;
// Object1 = Object1 - 1;
//}
//
//[DEPENDENCIES]:
//       [INTERNAL]:
//          - Member, Helper, Utility methods
//          - Private Data Members
//
//        [EXTERNAL]:
//          - This class doesn't have any extrenal depencdencies
//
//[NAMESPACE]: Might be deemed unnecessary, but despite the distinctive function names, global
//             namespace pollution is still in the picture.
//
//            - {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan' class.s

#ifndef Formula_h
#define Formula_h

#include <string>
#include <random>

namespace ResourceConversion
{
    class Formula
    {
    private:
        std::string* InputResources = nullptr;
        size_t InputResourcesSize = 0;

        unsigned int* InputQuantities = nullptr;
        size_t InputQuantitiesSize = 0;

        std::string* OutputResources = nullptr;
        size_t OutputResourcesSize = 0;

        unsigned int* OutputQuantities = nullptr;
        size_t OutputQuantitiesSize = 0;

        unsigned int* ResultArray = nullptr;

        unsigned int ProficiencyLevel = 0;
            
        static const bool ShouldPrintValues = true;


        struct OutcomeModifiers
        {
            float Failure;
            float Partial;
            float Bonus;
            float Normal;

            // [DESC]: Constructor for OutcomeModifiers.
            //
            // [PARAM LIST]
            // Failure_ The failure modifier.
            // Partial_ The partial modifier.
            // Bonus_ The bonus modifier.
            // Normal_ The normal modifier.
            OutcomeModifiers (float Failure_, float Partial_, float Bonus_, float Normal_)
                : Failure (Failure_),
                  Partial (Partial_),
                  Bonus (Bonus_),
                  Normal (Normal_) {}

        };

        inline bool ContainsNullOrWhiteSpace (const std::string* Array, const size_t& ArraySize) const;
        inline OutcomeModifiers GetOutcomeChances (const unsigned int &Level);
        
        inline void CopyData (const Formula& other);
        inline void ClearContainer ();
        inline void ResetContainer ();
        inline void SwapData(Formula &&other);
        inline void ReassignData(Formula &&other) const;

        template<typename T>
        typename std::enable_if<std::is_same<T, unsigned int>::value || std::is_same<T, std::string>::value, bool>::type 
        inline ArraysAreEqual(T* First, T* Second, size_t FirstArraySize, size_t SecondArraySize) const;

        //[NOTE]: Decrements the Quantities
        inline void Decrement(unsigned int* &Array, size_t ArraySize, unsigned int DefaultValue = 1);
        inline void Increment(unsigned int* &Array, size_t ArraySize, unsigned int DefaultValue = 1);

    public:

        explicit Formula ();
        Formula (std::string* InputResources_, size_t InputResourcesSize_,
                 unsigned int* InputQuantities_, size_t InputQuantitiesSize_,
                 std::string* OutputResources_, size_t OutputResourcesSize_,
                 unsigned int* OutputQuantities_, size_t OutputQuantitiesSize_,
                 unsigned int* ResultArray_, unsigned int ProficiencyLevel_ = 0);

        ~Formula ();

        Formula (const Formula& other);
        Formula& operator=(const Formula& other);

        Formula (Formula&& other) noexcept;
        Formula& operator=(Formula&& other) noexcept;

        void Apply ();
        inline unsigned int* GetResultArray () const { return ResultArray; }
        void DisplayFormulaValues(const bool PrintResultArray = false) const;

        inline std::string* GetInputResources() { return InputResources; }
        inline std::string* GetOutputResources() { return OutputResources; }

        inline std::size_t GetInputResourcesSize() { return InputResourcesSize; }
        inline std::size_t GetOutputResourcesSize() { return OutputResourcesSize; }

        inline unsigned int* GetInputQuantities() { return InputQuantities; }
        inline unsigned int* GetOutputQuantities() { return OutputQuantities; }
        

        //[DESC]: Struct that abstracts the acess of internal data [Formula].
        //        Does not modify any of the data but simply retrieves them
        //[NOTE]: using the "s_prefix" naming convention, to not confuse the
        //        internals of the struct with the Formula members
        //[NOTE #2]: Using 'std::vector' and iterators to build the Resources.
        //           Dsicussed with professor Dingle.
        struct StockpileDataLoader
        { 
            std::vector<std::string> s_InputResources;
            std::vector<std::string> s_OutputResources;
            std::vector<unsigned int> s_InputQuantities;
            std::vector<unsigned int> s_OutputQuantities;

            std::size_t s_InputResourcesSize = 0;
            std::size_t s_OutputResourcesSize = 0;

            StockpileDataLoader(Formula s_Formula)
          : s_InputResources(s_Formula.GetInputResources(), s_Formula.GetInputResources() + s_Formula.GetInputResourcesSize()),
            s_OutputResources(s_Formula.GetOutputResources(), s_Formula.GetOutputResources() + s_Formula.GetOutputResourcesSize()),
            s_InputQuantities(s_Formula.GetInputQuantities(), s_Formula.GetInputQuantities() + s_Formula.GetInputResourcesSize()),
            s_OutputQuantities(s_Formula.GetOutputQuantities(), s_Formula.GetOutputQuantities() + s_Formula.GetOutputResourcesSize()),
            s_InputResourcesSize(s_Formula.GetInputResourcesSize()),
            s_OutputResourcesSize(s_Formula.GetOutputResourcesSize()) {}
        };
        
        //[OPERATORS]
        bool operator!=(const Formula& other) const;
        bool operator==(const Formula& other) const;
        
        Formula operator+(unsigned int IncrementValue);
        Formula operator-(unsigned int DecrementValue);

        Formula operator++(int DummyParameter);
        Formula operator--(int DummyParameter);

        Formula& operator++();
        Formula& operator--();

        Formula& operator+=(unsigned int IncrementValue);
        Formula& operator-=(unsigned int DecrementValue);
    };
}//[NAMESPACE]: ResourceConversion
#endif /* Formula_h */

