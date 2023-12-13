//[FILE]: Plan.h
//[DESC]: This file contains the definition of the Plan class, which encapsulates an array of Formula
//        Objects. The 'Plan' class brings more functionality than a C-style array with methods like
//        'AddFomrmula(...)', 'RemoveLastFormula()', and so on. Memory managment is abstraced and
//        hidden from the client. The class supports shallow and effiecent deep copying with move
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
//[INVARIANT]: Capacity is the capacity for FormulaArray and should be greater than or equal to 2.
//[INVARIANT]: Size of Plan and should be greater than or equal to 1.
//[INVARIANT]: FormulaArray is a dynamic array to store Formula objects.
//[INVARAINT]: FormulaArray cannot be nullptr
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
// Plan Object1(...);
// Plan Object2(...);
//
// Object1 == Object2;
// Object1 != Object2;
//
// Object1 > Object2;
// Object1 < Object2;
// Object1 >= Object2;
// Object1 <= Object2;
//
// Object1 = Object1 + 1;
// Object1 = Object1 - 1;
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
// Obejct2 = Object2 + Object1;
//}
//
//[DEPENDENCIES]:
//       [INTERNAL]:
//          - Member, Helper, Utility methods
//          - Private Data Members
//
//        [EXTERNAL]:
//          - 'Formula' class {[SEE]: Formula.h}
//
//[NAMESPACE]: Might be deemed unnecessary, but despite the distinctive function names, global
//             namespace pollution is still in the picture.
//
//            - {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan' class.

#ifndef Plan_h
#define Plan_h

#include "Formula.h"

namespace ResourceConversion
{
  class Plan
  {
  private:
    bool ShouldPrintValues = true;

    inline void ResizePlan (size_t NewCapacity);
      
    inline void ClearPlan ();
    inline void CopyPlanData (const Plan& other);
    inline void ResetPlan ();
    inline void ReassignData(Plan&& other) const;
    inline void SwapData(Plan&& other);

    inline bool PlanArraysAreEqual(const Plan& other) const;
    inline void PushDefaultValueInArray(size_t OldSize); 
    inline void ConcatinateArrays(const Plan& other);

  protected:
    size_t Capacity = 2;
    size_t Size = 1;
    Formula* FormulaArray = nullptr;
      
  public:
    Plan ();
    explicit Plan (Formula* InitialSequence, size_t InitialSize);

    virtual ~Plan ();

    Plan (const Plan& other);
    Plan& operator=(const Plan& other);

    Plan (Plan&& other) noexcept;
    Plan& operator=(Plan&& other);

    virtual void AddFormula (const Formula &NewFormula);
    virtual void RemoveLastFormula ();
    virtual void ReplaceFormula (const Formula& NewFormula, const size_t &Index);

    virtual void PlanApply ();
    void PlanDisplayValues(const bool PrintResultArray = false) const;

    //[OPERATORS]
    bool operator!=(const Plan& other) const;
    bool operator==(const Plan& other) const;

    bool operator<(const Plan& other) const;
    bool operator>(const Plan& other) const;

    bool operator<=(const Plan& other) const;
    bool operator>=(const Plan& other) const;

    Formula& operator[](size_t Index) const;
    
    Plan operator+(const Plan& other);

    Plan operator+(size_t NewSize);
    Plan operator-(int NewSize);

    Plan operator++(int DummyParameter);
    Plan operator--(int DummyParameter);

    Plan& operator++();
    Plan& operator--();

    Plan& operator+=(unsigned int IncrementValue);
    Plan& operator-=(unsigned int DecrementValue);
  };
}//[NAMESPACE]: ResourceConversion

#endif /* Plan_h */
