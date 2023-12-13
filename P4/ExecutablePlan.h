//[FILE]: ExecutablePlan.h
//[DESC]: This file defines the derived class 'ExecutablePlan.' 'ExecutablePlan' extends the 
//        functionality of the base class 'Plan' through an "is a" relationship. It introduces the
//        concept of 'Step,' enhancing the capabilities of the 'Plan' class. The 'ExecutablePlan'
//        class maintains an array of 'Formula' objects and concurrently manages a 'Step' variable
//        along with a parallel boolean array indicating whether each formula has been completed. The
//        class efficiently supports both shallow and deep copying through move semantics. {[SEE]:
//        [MOVE SEMANTICS]} The class also support some simple mathemtical 
//        operatoions through overloaded operators. {[SEE]: [OPERATORS]}
//
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Fri 27th Oct
//[VERSION]: Revision History
//          - 1.0 [27/10/23] Initial Migration from C# => C++
//          - 2.0 [28/10/23] Optimisation, Improved Copy,Move Semantics
//          - 3.0 [28/10/23] Debugging, Removing unnecesary allocations
//          - 4.0 [29/10/23] Refine documentation, fix error formatting
//          - 5.0 [29/10/23] More Debugging (std::move())
//
//[INVARIANT]: Step cannot be negative (unsigned int)
//[INVARIANT]: 'CompletedArray' matches the 'FormulaArray' size
//[INVARIANT]: FormulaArray is never NULL
//[INVARIANT]: 'Step' should always be in bounds
//[INVARIANT]: CompletedArray is never null
//
//[MOVE SEMANTICS]
//{
// ExecutablePlan Obj1(...); -> Initial Construction
//
// ExecutablePlan Obj2 = ExecutablePlan(Obbj1) -> Copy Ctor
// ExecutablePlan Obj3 = Obj2 -> Copy Assignment Operator
//
// ExecutablePlan Obj4 = ExecutablePlan(std::move(Obj1)) -> Move Ctor
// ExecutablePlan Obj5 = std::move(Obj2) -> Move Assignment Operator
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
//
// Formula FObject = Object1[0]
//}
//
//[DEPENDENCIES]:
//       [INTERNAL]:
//          - 'Formula' class {[SEE]: Formula.h}
//          - Member, Helper, Utility methods
//          - Private Data Members
//
//        [EXTERNAL]:
//          - 'Plan' class {[SEE]: Plan.h}
//
//[NAMESPACE]: Might be deemed unnecessary, but despite the distinctive function names, global
//             namespace pollution is still in the picture.
//
//            - {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan' class.
#ifndef ExecutablePlan_h
#define ExecutablePlan_h

#include <memory>
#include <utility>
#include <optional>

#include "Plan.h"
#include "Formula.h"
#include "Stockpile.h"

namespace ResourceConversion
{
  class ExecutablePlan : public Plan
  {
  private:
    unsigned int Step = 0;
    bool* CompletedArray = nullptr;
    size_t CompletedArraySize = 0;
    
    inline void ClearXPlan ();
    inline void CopyXPlanData (const ExecutablePlan& other);
    inline void ResetXPlan ();
    inline void ReassignDataXPlan(ExecutablePlan&& other) const;
    inline void SwapDataXPlan(ExecutablePlan&& other);
    
    inline void PushArrayValue(const bool& Value);
    
  public:
    explicit ExecutablePlan();
    ExecutablePlan(Formula* FormulaArray_, size_t Size, unsigned int CurrentStep = 0);
    
    ~ExecutablePlan();
    ExecutablePlan(const ExecutablePlan& other);
    ExecutablePlan& operator=(const ExecutablePlan& other);
    
    ExecutablePlan(ExecutablePlan&& other) noexcept;
    ExecutablePlan& operator=(ExecutablePlan&& other) noexcept;
    
    void AddFormula(const Formula &NewFormula) override;
    void RemoveLastFormula() override;
    void ReplaceFormula(const Formula& NewFormula, const size_t &Index) override;
    void PlanApply() override;
    std::shared_ptr<Stockpile> PlanApply(const std::shared_ptr<Stockpile>& StockpilePtr);

    //[OPERATORS]:
    bool operator!=(const ExecutablePlan& other) const;
    bool operator==(const ExecutablePlan& other) const;

    bool operator<(const ExecutablePlan& other) const;
    bool operator>(const ExecutablePlan& other) const;

    bool operator<=(const ExecutablePlan& other) const;
    bool operator>=(const ExecutablePlan& other) const;
    
    ExecutablePlan operator+(const ExecutablePlan& other);

    ExecutablePlan operator+(size_t NewSize);
    ExecutablePlan operator-(int NewSize);

    ExecutablePlan operator++(int DummyParameter);
    ExecutablePlan operator--(int DummyParameter);

    ExecutablePlan& operator++();
    ExecutablePlan& operator--();

    ExecutablePlan& operator+=(unsigned int IncrementValue);
    ExecutablePlan& operator-=(unsigned int DecrementValue);

    Formula& operator[](size_t Index) const;
  };
}//[NAMESPACE]: ResourceConversion
#endif /* ExecutablePlan_h */
