/**
 @file Plan.h
 @author Jakob Balkovec (CPSC 3200)
 @instructor A. Dingle (CPSC 3200)

 @date Wed 20th Sep

 @version
 Revision History
 - 1.0 [07/10/2023]: Inital class definition
 - 2.0 [08/10/2023]: Debugging, Refine documentatin
 - 3.0 [09/10/2023]: Debugging

 @brief
 * This file contains the definition of the Plan class, which represents
 * handling an array of Formula Objects. The plan class
 * encapsulates information about the array and abstratcs memory managment.

 @invariant
 * Capacity should be greater than or equal to Size.
 * Size should be greater than or equal to 0.
 * If Size is 0, FormulaArray should be nullptr.
 * If FormulaArray is not nullptr, it should point to an array of Formula objects of
   size Capacity.
*/

#ifndef Plan_h
#define Plan_h

#include "Formula.h"

namespace ResourceConversion
{
  /**
 * @brief Class invariants for the Plan class:
 *
 * - Capacity is the capacity for FormulaArray and should always be greater than or equal to 2.
 * - Size represents the current number of Formula objects in the Plan and should be greater than or equal to 1.
 * - FormulaArray is a dynamic array to store Formula objects.
 *
 * @invariant Capacity >= 2
 * @invariant Size >= 1
 * @invariant FormulaArray != nullptr
 */
  class Plan
  {
  private:
    size_t Capacity = 2;
    size_t Size = 1;
    Formula* FormulaArray;

    void ResizePlan (size_t NewCapacity);
    inline void ClearPlan ();
    inline void CopyPlanData (const Plan& other);
    inline void ResetPlan (Plan& other);

  public:
    Plan ();
    Plan (Formula* InitialSequence, size_t InitialSize);
    Plan (const std::initializer_list<Formula>& InitialSequence);

    ~Plan ();

    Plan (const Plan& other);
    Plan& operator=(const Plan& other);

    Plan (Plan&& other) noexcept;
    Plan& operator=(Plan&& other);

    Formula& operator[](size_t Index);
    void AddFormula (const Formula NewFormula);
    void RemoveLastFormula ();
    void ReplaceFormula (size_t Index, const Formula& NewFormula);

    void PlanApply ();
  };
}

#endif /* Plan_h */
