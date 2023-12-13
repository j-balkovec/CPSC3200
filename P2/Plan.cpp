/**
 @file Plan.cpp
 @author Jakob Balkovec (CPSC 3200)
 @instructor A. Dingle (CPSC 3200)

 @date Wed 20th Sep

 @version
 Revision History
 - 1.0 [07/10/2023]: Class Methods, Move semantics
 - 2.0 [08/10/2023]: Debugging, Refine documentatin
 - 3.0 [09/10/2023]: Debugging

 @brief
 * This file contains the implementation of the Plan class, which represents
 * handling an array of Formula Objects. The plan class
 * encapsulates information about the array and abstratcs memory managment.

 @invariant
 * Resource Arrays should contain valid Strings and should not be empty
 * Quantity Arrays should contain Non-Negative Quantities and should not be empty
 * Proficiency Level should be Non-Negative and within valid range
*/
#include <stdexcept>

#include "Formula.h"
#include "Plan.h"

/**
 * @namespace ResourceConversion
 * @brief Defines the Formula and Plan class, avoiding global namespace polution
 */
namespace ResourceConversion
{
  /**
   * @brief Resize the Plan to accommodate a new capacity.
   *
   * @param NewCapacity The new capacity for the Plan.
   *
   * @pre NewCapacity should be greater than or equal to the current Size.
   *
   * @post The Plan's capacity will be updated to NewCapacity.
   *       The Plan's Size remains unchanged, and the existing data is preserved.
   *       FormulaArray points to a new array of size NewCapacity containing the same data.
   */
  void Plan::ResizePlan (size_t NewCapacity)
  {
    Formula* NewFormulaArray = new Formula[NewCapacity];
    for (size_t i = 0; i < Size; ++i)
    {
      NewFormulaArray[i] = FormulaArray[i];
    }
    delete[] FormulaArray;
    FormulaArray = NewFormulaArray;
    Size = Capacity;
    Capacity = NewCapacity;
  }

  /**
   * @brief Clears the Plan by releasing allocated resources and resetting its size and capacity to 0.
   *
   * @pre None.
   *
   * @post All allocated memory for FormulaArray is freed, and FormulaArray is set to nullptr.
   *       The Size and Capacity of the Plan are both set to 0.
   */
  inline void Plan::ClearPlan ()
  {
    if (FormulaArray != nullptr)
    {
      delete[] FormulaArray;
    }
    FormulaArray = nullptr;
    Size = Capacity = 0;
  }

  /**
   * @brief Copies data from another Plan into this Plan.
   *
   * @param other The Plan object from which data will be copied.
   *
   * @pre The 'other' Plan object should be properly initialized.
   *
   * @post This Plan will contain a deep copy of the data from the 'other' Plan,
   *       including the same Size and Capacity.
   */
  inline void Plan::CopyPlanData (const Plan& other)
  {
    Size = other.Size;
    Capacity = other.Capacity;
    FormulaArray = new Formula[Capacity];
    for (size_t i = 0; i < Size; ++i)
    {
      FormulaArray[i] = other.FormulaArray[i];
    }
  }

  /**
   * @brief Resets this Plan by transferring ownership of data from another Plan.
   *
   * @param other The Plan object from which ownership of data will be transferred.
   *
   * @pre The 'other' Plan object should be properly initialized.
   *
   * @post This Plan takes ownership of the data (including Size and Capacity) from 'other.'
   *       'other' becomes an empty Plan with no data, and its Capacity is set to 0.
   */
  inline void Plan::ResetPlan (Plan& other)
  {
    Capacity = other.Capacity;
    Size = other.Size;
    FormulaArray = other.FormulaArray;
    other.Capacity = 0;
    other.Size = 0;
    other.FormulaArray = nullptr;
  }

  /**
   * @brief Constructs an empty Plan with an initial Capacity of 2.
   *
   * @pre None.
   *
   * @post An empty Plan object is created with Capacity set to 2.
   *       The Size is initialized to 0, and an array of Capacity elements is allocated.
   */
  Plan::Plan ()
  {
    Capacity = 2;
    Size = 0;
    FormulaArray = new Formula[Capacity];
  }

  /**
   * @brief Constructs a Plan object with an initial sequence of Formulas and a specified initial size.
   *
   * @param InitialSequence An array of Formula objects representing the initial sequence.
   * @param InitialSize The initial size of the Plan, which must be equal to or greater than the size of InitialSequence.
   *
   * @pre InitialSize should be greater than or equal to the size of InitialSequence.
   *
   * @post A Plan object is constructed with Capacity set to InitialSize.
   *       The Size is initialized to InitialSize, and the data is copied from InitialSequence.
   */
  Plan::Plan (Formula* InitialSequence, size_t InitialSize)
  {
    Capacity = InitialSize;
    Size = InitialSize;
    FormulaArray = new Formula[Capacity];
    for (size_t i = 0; i < Size; ++i)
    {
      FormulaArray[i] = InitialSequence[i];
    }
  }

  /**
   * @brief Constructs a Plan object with an initial sequence of Formulas using an initializer list.
   *
   * @param InitialSequence An initializer list of Formula objects representing the initial sequence.
   *
   * @pre None.
   *
   * @post A Plan object is constructed with Capacity set to the size of InitialSequence.
   *       The Size is initialized to the same size as Capacity, and the data is copied from InitialSequence.
   */
  Plan::Plan (const std::initializer_list<Formula>& InitialSequence)
  {
    Capacity = InitialSequence.size ();
    Size = Capacity;
    FormulaArray = new Formula[Capacity];

    size_t i = 0;
    for (const Formula& FormulaObj : InitialSequence) {
      FormulaArray[i++] = FormulaObj;
    }
  }

  /**
   * @brief Destructor for the Plan class.
   *
   * @pre None.
   *
   * @post The Plan object is destroyed, and any allocated resources are released.
   */
  Plan::~Plan ()
  {
    ClearPlan ();
  }

  /**
   * @brief Copy constructor for the Plan class.
   *
   * @param other The Plan object to be copied.
   *
   * @pre None.
   *
   * @post A new Plan object is created as a deep copy of 'other,' including its data (Size and Capacity).
   */
  Plan::Plan (const Plan& other)
  {
    CopyPlanData (other);
  }

  /**
   * @brief Copy assignment operator for the Plan class.
   *
   * @param other The Plan object to be assigned from.
   *
   * @pre None.
   *
   * @post The current Plan object is assigned the data (including Size and Capacity) from 'other.'
   *       Any existing data in the current object is cleared.
   *
   * @return A reference to the current Plan object after the assignment.
   */
  Plan& Plan::operator=(const Plan& other)
  {
    if (this == &other) { return *this; }

    ClearPlan ();
    CopyPlanData (other);

    return *this;
  }

  /**
   * @brief Move constructor for the Plan class.
   *
   * @param other The Plan object to be moved from.
   *
   * @pre None.
   *
   * @post A new Plan object is created by taking ownership of the data (including Size and Capacity) from 'other.'
   *       'other' becomes an empty Plan with no data, and its Capacity is set to 0.
   */
  Plan::Plan (Plan&& other) noexcept
  {
    std::swap(FormulaArray, other.FormulaArray);
    std::swap(Size, other.Size);
    std::swap(Capacity, other.Capacity);
    
    ResetPlan (other);
  }

  /**
   * @brief Move assignment operator for the Plan class.
   *
   * @param other The Plan object to be assigned from.
   *
   * @pre None.
   *
   * @post The current Plan object is assigned the data (including Size and Capacity) from 'other' via a move operation.
   *       'other' becomes an empty Plan with no data, and its Capacity is set to 0.
   *
   * @return A reference to the current Plan object after the move assignment.
   */
  Plan& Plan::operator=(Plan&& other)
  {
    if (this == &other) { return *this; }

    ClearPlan ();
    CopyPlanData (other);
    ResetPlan (other);

    return *this;
  }

  /**
   * @brief Access the Formula at the specified index.
   *
   * @param Index The index of the Formula to access.
   *
   * @pre Index should be within the valid range [0, Size - 1].
   *
   * @post None.
   *
   * @throw std::out_of_range if Index is out of range.
   *
   * @return A reference to the Formula at the specified index.
   */
  Formula& Plan::operator[](size_t Index)
  {
    if (Index >= Size) { throw std::out_of_range ("[Index out of range]"); }
    return FormulaArray[Index];
  }

  /**
   * @brief Add a new Formula to the Plan.
   *
   * @param NewFormula The Formula to be added.
   *
   * @pre None.
   *
   * @post If adding the Formula exceeds the Capacity, the Plan is resized to accommodate it.
   *       The new Formula is added to the end of the Plan, and the Size is updated accordingly.
   */
  void Plan::AddFormula (const Formula NewFormula) {
    if (Size >= Capacity) {
      ResizePlan (Capacity * 2);
    }
    FormulaArray[Size++] = NewFormula;
  }

  /**
   * @brief Remove the last Formula from the Plan.
   *
   * @pre The Size of the Plan should be greater than 0.
   *
   * @post The last Formula is removed from the Plan, and the Size is decremented.
   *
   * @throw std::invalid_argument if the Size is less than or equal to 0.
   */
  void Plan::RemoveLastFormula ()
  {
    if (Size <= 0) { throw std::invalid_argument ("[Plan size is less than or equal to 0]"); }
    --Size;
  }

  /**
   * @brief Replace the Formula at the specified index with a new Formula.
   *
   * @param Index The index of the Formula to replace.
   * @param NewFormula The new Formula to be assigned at the specified index.
   *
   * @pre Index should be within the valid range [0, Size - 1].
   *
   * @post The Formula at the specified index is replaced with the new Formula.
   *
   * @throw std::out_of_range if Index is out of range.
   */
  void Plan::ReplaceFormula (size_t Index, const Formula& NewFormula)
  {
    if (Index > Size) { throw std::out_of_range ("[Index is Out of Bounds"); }
    FormulaArray[Index] = NewFormula;
  }

  /**
   * @brief Apply all Formulas in the Plan.
   *
   * @pre The Size of the Plan should be greater than 0.
   *
   * @post All Formulas in the Plan are applied sequentially.
   *
   * @throw std::logic_error if the Size is less than or equal to 0.
   */
  void Plan::PlanApply ()
  {
    if (Size <= 0) { throw std::logic_error ("[Size is =< 0]"); }

    for (size_t i = 0; i < Size; ++i)
    {
      FormulaArray[i].Apply ();
    }
  }
}
