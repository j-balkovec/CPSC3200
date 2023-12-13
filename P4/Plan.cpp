//[DESC]: This file contains the implementation of the Plan class, which represents
//       handling an array of Formula Objects. The Plan class
//       encapsulates information about the array and abstracts memory management.
//
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Wed 20th Sep
//
//[VERSION]: Revision History
//           - 1.0 [07/10/2023]: Class Methods, Move semantics
//           - 2.0 [08/10/2023]: Debugging, Refine documentation
//           - 3.0 [09/10/2023]: Debugging
//
//[INVARIANT]: Resource Arrays should contain valid Strings and should not be empty
//[INVARIANT]: Quantity Arrays should contain Non-Negative Quantities and should not be empty
//[INVARIANT]: Proficiency Level should be Non-Negative and within the valid range

#include <stdexcept>
#include <algorithm>
#include <new>

#include <memory>
#include <iostream>

#include "Formula.h"
#include "Plan.h"

namespace ResourceConversion
{
//[DESC]: Resize the Plan to accommodate a new capacity.
//
//[PARAM]: NewCapacity The new capacity for the Plan.
//
//[PRE]: NewCapacity should be greater than or equal to the current Size.
//
//[POST]: The Plan's capacity will be updated to NewCapacity.
//        The Plan's Size remains unchanged, and the existing data is preserved.
//        FormulaArray points to a new array of size NewCapacity containing the same data.
inline void Plan::ResizePlan (size_t NewCapacity)
{
  Formula* NewFormulaArray = new (std::nothrow) Formula[NewCapacity];
  for (size_t i = 0; i < Size; ++i)
  {
    NewFormulaArray[i] = FormulaArray[i];
  }
  delete[] FormulaArray;
  FormulaArray = NewFormulaArray;
  Size = Capacity;
  Capacity = NewCapacity;
}

//[DESC]: Clears the Plan by releasing allocated resources and resetting its size and capacity to 0.
//
//[PRE]: None.
//
//[POST]: All allocated memory for FormulaArray is freed, and FormulaArray is set to nullptr.
//        The Size and Capacity of the Plan are both set to 0.
inline void Plan::ClearPlan ()
{
  if (FormulaArray != nullptr)
  {
    delete[] FormulaArray;
  }
  FormulaArray = nullptr;
  Size = Capacity = 0;
}

//[DESC]: Copies data from another Plan into this Plan.
//
//[PARAM]: other The Plan object from which data will be copied.
//
//[PRE]: The 'other' Plan object should be properly initialized.
//
//[POST]: This Plan will contain a deep copy of the data from the 'other' Plan,
//       including the same Size and Capacity.
inline void Plan::CopyPlanData (const Plan& other)
{
  Size = other.Size;
  Capacity = other.Capacity;
  FormulaArray = new (std::nothrow) Formula[Capacity];
  for (size_t i = 0; i < Size; ++i)
  {
    FormulaArray[i] = other.FormulaArray[i];
  }
}

//[DESC]: Resets this Plan by transferring ownership of data from another Plan.
//
//[PARAM]: other The Plan object from which ownership of data will be transferred.
//
//[PRE]: The 'other' Plan object should be properly initialized.
//
//[POST]: This Plan takes ownership of the data (including Size and Capacity) from 'other.'
//       'other' becomes an empty Plan with no data, and its Capacity is set to 0.
inline void Plan::ResetPlan ()
{
    Capacity = 0;
    Size = 0;
    FormulaArray = nullptr;
}

//[DESC]: Reassigns data from another Plan object by swapping data members.
//
//[PARAM LIST]:
//      other - The Plan object from which data will be reassigned.
//
//[PRE]: The 'other' Plan object should be properly initialized.
//
//[POST]: The data, including FormulaArray, Size, and Capacity, is swapped between 'this' Plan object
//        and 'other'.
inline void Plan::ReassignData(Plan&& other) const
{
  other.FormulaArray = FormulaArray;
  other.Size = Size;
  other.Capacity = Capacity;
}

//[DESC]: Swaps data with another Plan object.
//
//[PARAM LIST]:
//      other - The Plan object to swap data with.
//
//[PRE]: The 'other' Plan object should be properly initialized.
//
//[POST]: Data, including FormulaArray, Size, and Capacity, is swapped between 'this' Plan object and
//        'other'.
inline void Plan::SwapData(Plan&& other)
{
  std::swap(other.FormulaArray, FormulaArray);
  std::swap(other.Size, other.Size);
  std::swap(other.Capacity, Capacity);
}

//[DESC]: Constructs an empty Plan with an initial Capacity of 2.
//
//[PRE]: None.
//
//[POST]: An empty Plan object is created with Capacity set to 2.
//       The Size is initialized to 0, and an array of Capacity elements is allocated.
Plan::Plan ()
{
  Capacity = 2;
  Size = 0;
  FormulaArray = new (std::nothrow) Formula[Capacity];
}

//[DESC]: Constructs a Plan object with an initial sequence of Formulas and a specified initial size.
//
//[PARAM]: InitialSequence An array of Formula objects representing the initial sequence.
//[PARAM]: InitialSize The initial size of the Plan, which must be equal to or greater than the size
//         of InitialSequence.
//
//[PRE]: InitialSize should be greater than or equal to the size of InitialSequence.
//
//[POST]: A Plan object is constructed with Capacity set to InitialSize.
//        The Size is initialized to InitialSize, and the data is copied from InitialSequence.
Plan::Plan (Formula* InitialSequence, size_t InitialSize)
{
  if(InitialSize <= 0)
  {
    throw std::invalid_argument("[P]Plan(...): [Size cannot be non-positive]");
  }
  Capacity = InitialSize;
  Size = InitialSize;
  FormulaArray = new (std::nothrow) Formula[Capacity];
  for (size_t i = 0; i < Size; ++i)
  {
    FormulaArray[i] = InitialSequence[i];
  }
}

//[DESC]: Destructor for the Plan class.
//
//[PRE]: None.
//
//[POST]: The Plan object is destroyed, and any allocated resources are released.
Plan::~Plan ()
{
  ClearPlan ();
}

//[DESC]: Copy constructor for the Plan class.
//
//[PARAM]: other The Plan object to be copied.
//
//[PRE]: None.
//
//[POST]: A new Plan object is created as a deep copy of 'other,' including its data
//        (Size and Capacity).
Plan::Plan (const Plan& other)
{
  CopyPlanData (other);
}

//[DESC]: Copy assignment operator for the Plan class.
//
//[PARAM]: other The Plan object to be assigned from.
//
//[PRE]: None.
//
//[POST]: The current Plan object is assigned the data (including Size and Capacity) from 'other.'
//        Any existing data in the current object is cleared.
[[nodiscard]]Plan& Plan::operator=(const Plan& other)
{
  if (this == &other) { return *this; }

  ClearPlan ();
  CopyPlanData (other);

  return *this;
}

//[DESC]: Move constructor for the Plan class.
//
//[PARAM]: other The Plan object to be moved from.
//
//[PRE]: None.
//
//[POST]: A new Plan object is created by taking ownership of the data (including Size and Capacity)
//        from 'other.'
//        'other' becomes an empty Plan with no data, and its Capacity is set to 0.
//
//[NOTE]: Using 'std::move()' to actually pass in an rvalue reference
//        - 'std::move() converts the param <T> to an rvalue reference to indicate intent of moving
//           the object
Plan::Plan (Plan&& other) noexcept
{
  ReassignData(std::move(other));
  ResetPlan ();
}

//[DESC]: Move assignment operator for the Plan class.
//
//[PARAM]: other The Plan object to be assigned from.
//
//[PRE]: None.
//
//[POST]: The current Plan object is assigned the data (including Size and Capacity) from 'other' via
//        a move operation.
//        'other' becomes an empty Plan with no data, and its Capacity is set to 0.
//
//[NOTE]: Using 'std::move()' to actually pass in an rvalue reference
//        - 'std::move() converts the param <T> to an rvalue reference to indicate intent of moving
//           the object
[[nodiscard]]Plan& Plan::operator=(Plan&& other)
{
  if (this == &other) { return *this; }
  SwapData(std::move(other));
  return *this;
}

//[DESC]: Add a new Formula to the Plan.
//
//[PARAM]: NewFormula The Formula to be added.
//
//[PRE]: None.
//
//[POST]: If adding the Formula exceeds the Capacity, the Plan is resized to accommodate it.
//        The new Formula is added to the end of the Plan, and the Size is updated accordingly.
void Plan::AddFormula (const Formula &NewFormula) {
  if (Size >= Capacity) {
    ResizePlan (Capacity * 2);
  }
  FormulaArray[Size++] = NewFormula;
}

//[DESC]: Remove the last Formula from the Plan.
//
//[PRE]: The Size of the Plan should be greater than 0.
//
//[POST]: The last Formula is removed from the Plan, and the Size is decremented.
//
//[THROW]: std::invalid_argument if the Size is less than or equal to 0.
void Plan::RemoveLastFormula ()
{
  if (Size <= 0) { throw std::invalid_argument ("[P]RemoveLastFomrula(): [Plan size is less than or equal to 0]"); }
  --Size;
}

//[DESC]: Replace the Formula at the specified index with a new Formula.
//
//[PARAM]: Index The index of the Formula to replace.
//[PARAM]: NewFormula The new Formula to be assigned at the specified index.
//
//[PRE]: Index should be within the valid range [0, Size - 1].
//
//[POST]: The Formula at the specified index is replaced with the new Formula.
//
//[THROW]: std::out_of_range if Index is out of range.
void Plan::ReplaceFormula (const Formula& NewFormula, const size_t &Index)
{
  if (Index > Size) { throw std::out_of_range ("[P]ReplaceFomrula(...): [Index is Out of Bounds"); }
  FormulaArray[Index] = NewFormula;
}

//[DESC]: Apply all Formulas in the Plan.
//
//[PRE]: The Size of the Plan should be greater than 0.
//
//[POST]: All Formulas in the Plan are applied sequentially.
//
//[THROW]: std::logic_error if the Size is less than or equal to 0.
void Plan::PlanApply ()
{
  if (Size <= 0) { throw std::logic_error ("[P]PlanApply(): [Size is =< 0]"); }

  for (size_t i = 0; i < Size; ++i)
  {
    FormulaArray[i].Apply ();
  }
}

//[DESC]: Displays the values of formulas in the Plan, including the input and output resources and
//        either the result array or output quantities for each formula.
//
//[PARAM LIST]:
//      PrintResultArray - A boolean flag to determine whether to print the result array (if true) or
//                         the output quantities (if false) for each formula in the Plan.
//
//[PRE]: None.
//
//[POST]: The values of input resources, input quantities, output resources, and either the result
//        array or output quantities are displayed for each formula in the Plan.
//
//[THROW]: None.
//
//[NOTES]:
//        - This function iterates through each formula in the Plan and calls the DisplayFormulaValues
//          function with the provided PrintResultArray flag to control whether to print the result
//          array or output quantities. If PrintResultArray is true, it displays the result array;
//          otherwise, it displays the output quantities for each formula.
//
//        - To enable value printing for individual formulas, ensure that the 'ShouldPrintValues'
//          member variable of each formula is set to true. If 'ShouldPrintValues' is false for a
//          formula, no values will be displayed for that formula.
void Plan::PlanDisplayValues(const bool PrintResultArray) const
{
    for(size_t i = 0; i < Size; i++)
    {
        FormulaArray[i].DisplayFormulaValues(PrintResultArray);
    }
}

//[DESC]: Checks if two Plan objects have equal FormulaArray contents.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be 
//        properly initialized.
//[POST]: Returns true if the FormulaArray contents of the current Plan and 
//        'other' Plan objects are equal.
//[PARAM]: other - A reference to another Plan object that you want to compare 
//         with the current Plan object.
//[RETURN]: Returns true if the FormulaArray contents of the current Plan and 
//          'other' Plan objects are equal, otherwise returns false.
inline bool Plan::PlanArraysAreEqual(const Plan& other) const
{
  for(size_t i = 0; i < Size; i++)
  {
    if(other.FormulaArray[i] != FormulaArray[i]) { return false; }
  }
  return false;
}

// [DESC]: This function pushes default values into an array of Formula objects.
// [PRE]: The array FormulaArray has been initialized and has a size of at least 'Size'.
// [POST]: Default values have been added to the FormulaArray starting from index 'OldSize' 
//         up to 'Size - 1'.
// [NOTE]: This is used to fill uninitialized or newly extended parts of the array with 
//         default values.
inline void Plan::PushDefaultValueInArray(size_t OldSize) 
{
  Formula DefaultObject = Formula();
  for(size_t i = OldSize; i < Size; i++)
  {
    FormulaArray[i] = DefaultObject;
  }
}
//[DESC]: Concatenates the FormulaArray of the current Plan object with the FormulaArray of 
//        another Plan object.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be properly initialized.
//        The current Plan object's FormulaArray must have enough capacity to accommodate 
//        the concatenation.
//[POST]: The FormulaArray of the current Plan object is extended with the contents of the 
//        'other' Plan object's FormulaArray.
//[PARAM]: other - A reference to another Plan object whose FormulaArray you want to concatenate 
//         with the current Plan object's FormulaArray.
inline void Plan::ConcatinateArrays(const Plan& other)
{
  size_t OldSize = Size;
  size_t SizeOfNewArray = Size + other.Size;
  ResizePlan(SizeOfNewArray);

  for(size_t i = OldSize - 1; i < Size - 1; i++)
  {
    FormulaArray[i] = other.FormulaArray[i];
  }
}

//[DESC]: Access the Formula at the specified index.
//[PARAM]: Index The index of the Formula to access.
//[PRE]: Index should be within the valid range [0, Size - 1].
//[POST]: Returns a reference of the 'Formula' at 'Index'
//
//[THROW]: std::out_of_range if Index is out of range.
Formula& Plan::operator[](size_t Index) const
{
  if (Index >= Size) { throw std::out_of_range ("[P]operator[](...): [Index out of range]"); }
  return FormulaArray[Index];
}

//[DESC]: Checks if two Plan objects are not equal by comparing their FormulaArrays.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be properly initialized.
//[POST]: Returns true if the arrays of the current and 'other' objects are not equal. 
//        Otherwise, returns false.
//[PARAM]: other - A reference to another Plan object that you want to compare with
//         the current Plan object.
//[RETURN]: Returns true if the FormulaArrays of the current Plan and 'other' Plan objects 
//          are not equal. Otherwise, returns false.
//[NOTE]: This overloaded inequality operator (!=) compares two Plan objects by checking 
//        if their FormulaArrays are not equal. If they are not equal, the function 
//        returns true; otherwise, it returns false.
bool Plan::operator!=(const Plan& other) const
{
  if(PlanArraysAreEqual(other)) { return false; }
  return true;
}

//[DESC]: Checks if two Plan objects are equal by comparing their FormulaArrays.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be properly initialized.
//[POST]: Returns true if the FormulaArrays of the current Plan and 'other' Plan objects are equal. 
//        Otherwise, returns false.
//[PARAM]: other - A reference to another Plan object that you want to compare with the 
//         current Plan object.
//[THROW]: None
//[RETURN]: Returns true if the FormulaArrays of the current Plan and 'other' Plan
//          objects are equal. Otherwise, returns false.
//[NOTE]: This overloaded equality operator (==) compares two Plan objects by checking
//        if their FormulaArrays are equal. If they are equal, the function returns true;
//        otherwise, it returns false. It is implemented by utilizing the inequality operator 
//        (!=) to determine equality.
bool Plan::operator==(const Plan& other) const
{
  bool PlaceHolderValue = operator!=(other);
  return !PlaceHolderValue;
}

//[DESC]: Compares two Plan objects based on their sizes.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be properly initialized.
//[POST]: Returns true if the size of the current Plan object is less than the size of the 
//        'other' Plan object. Otherwise, returns false.
//[PARAM]: other - A reference to another Plan object that you want to compare with the 
//         current Plan object.
//[THROW]: None
//[RETURN]: Returns true if the size of the current Plan object is less than the size 
//          of the 'other' Plan object. Otherwise, returns false.
//[NOTE]: This overloaded less-than operator (<) compares two Plan objects based on 
//        their sizes. It returns true if the size of the current Plan object is less 
//        than the size of the 'other' Plan object. This comparison is solely based on 
//        the 'Size' attribute of the Plan objects.
bool Plan::operator<(const Plan& other) const { return this -> Size < other.Size; }

//[DESC]: Compares two Plan objects based on their sizes.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be properly initialized.
//[POST]: Returns true if the size of the current Plan object is greater than the size of the 
//        'other' Plan object. Otherwise, returns false.
//[PARAM]: other - A reference to another Plan object that you want to compare with the 
//         current Plan object.
//[THROW]: None
//[RETURN]: Returns true if the size of the current Plan object is greater than the size
//          of the 'other' Plan object. Otherwise, returns false.
//[NOTE]: This overloaded greater-than operator (>) compares two Plan objects based on 
//        their sizes. It returns true if the size of the current Plan object is greater 
//        than the size of the 'other' Plan object. This comparison is solely based on 
//        the 'Size' attribute of the Plan objects.
bool Plan::operator>(const Plan& other) const { return this -> Size > other.Size; }

//[DESC]: Compares two Plan objects based on their sizes and checks if the current 
//        Plan's size is less than or equal to the size of the 'other' Plan.
//[PRE]: Both the current Plan object and the 'other' Plan object must be properly initialized.
//[POST]: Returns true if the size of the current Plan object is less than or equal to 
//        the size of the 'other' Plan object. Otherwise, returns false.
//[PARAM]: other - A reference to another Plan object that you want to compare with 
//         the current Plan object.
//[THROW]: None
//[RETURN]: Returns true if the size of the current Plan object is less than or equal 
//          to the size of the 'other' Plan object. Otherwise, returns false.
//[NOTE]: This operator (<=) compares two Plan objects based on their sizes. 
//        It returns true if the size of the current Plan object is less than or 
//        equal to the size of the 'other' Plan object.
bool Plan::operator<=(const Plan& other) const {return this -> Size <= other.Size; }

//[DESC]: Compares two Plan objects based on their sizes and checks if the current Plan's size is greater than or equal to the size of the 'other' Plan.
//[PRE]: Both the current Plan object and the 'other' Plan object must be properly initialized.
//[POST]: Returns true if the size of the current Plan object is greater than or equal to the size of the 'other' Plan object. Otherwise, returns false.
//[PARAM]: other - A reference to another Plan object that you want to compare with the current Plan object.
//[THROW]: None
//[RETURN]: Returns true if the size of the current Plan object is greater than or equal to the size of the 'other' Plan object. Otherwise, returns false.
//[NOTE]: This operator (>=) compares two Plan objects based on their sizes. It returns true if the size of the current Plan object is greater than or equal to the size of the 'other' Plan object.
bool Plan::operator>=(const Plan& other) const {return this -> Size >= other.Size; }

//[DESC]: Concatenates the FormulaArrays of two Plan objects, preserving the state of 
//        the current Plan.
//[PRE]:  Both the current Plan object and the 'other' Plan object must be properly 
//        initialized. The current Plan object's FormulaArray must have enough capacity
//        to accommodate the concatenation.
//[POST]: The FormulaArray of the current Plan object is extended with the contents of
//        the 'other' Plan object's FormulaArray, while preserving the state of the 
//        current Plan. The resulting Plan object represents the state of the current 
//        Plan before concatenation.
//[PARAM]: other - A reference to another Plan object whose FormulaArray you want 
//         to concatenate with the current Plan object's FormulaArray.
//[THROW]: None
//[RETURN]: Returns a new Plan object that represents the state of the current Plan 
//          before concatenation.
//[NOTE]: Simulates std::string concatination
Plan Plan::operator+(const Plan& other)
{
  Plan OldState = *this;
  ConcatinateArrays(other);
  return OldState;
}

//[DESC]: Resizes the FormulaArray of the current Plan object to the specified size, 
//        ensuring that the size is not negative.
//[PRE]:  The current Plan object must be properly initialized. 'NewSize' is the 
//        desired new size for the FormulaArray and should be a non-negative integer.
//[POST]: Resizes the FormulaArray of the current Plan object to the specified 'NewSize'
//        ensuring it is not negative. If 'NewSize' is negative, the function throws an 
//        'std::invalid_argument' exception.
//
//[PARAM]: NewSize - An integer representing the desired new size for the FormulaArray.
//[THROW]: std::invalid_argument - If 'NewSize' is negative, this exception is thrown.
//[RETURN]: Returns a reference to the modified current Plan object after resizing its FormulaArray.
//
//[NOTE]: If the client passes in a negative size (allowed with unsigned int and size_t)
//        the consequences are catastrophic as it results in the program raising a SIGABRT,
//        and implementing a signal handler for that is pointless
Plan Plan::operator+(size_t NewSize)
{
  
  if(NewSize < Size) {throw std::invalid_argument("[P]operator+(...) [Invalid NewSize]");}  
  size_t OldSize = this -> Size;
  ResizePlan(NewSize);
  PushDefaultValueInArray(OldSize);

  return *this;
}

//[DESC]: Resizes the FormulaArray of the current Plan object by subtracting 
//        the specified 'NewSize' from its current size, ensuring 'NewSize' 
//        is not negative.
//[PRE]:  The current Plan object must be properly initialized. 'NewSize' is 
//        the integer value to subtract from the current size, and it should 
//        be a non-negative integer.
//[POST]: Resizes the FormulaArray of the current Plan object by subtracting 
//        'NewSize' from its current size, ensuring that the new size is not negative. 
//        If 'NewSize' is negative, the function throws an 'std::invalid_argument' exception.
//
//[PARAM]: NewSize - An integer representing the value to subtract from the current 
//         size to resize the FormulaArray.
//[THROW]: std::invalid_argument - If 'NewSize' is negative, this exception is thrown.
//[RETURN]: Returns a new Plan object with the modified size after the subtraction operation.
Plan Plan::operator-(int NewSize)
{ 
  constexpr int MinimumAllowedSize = 0;
  if (NewSize < MinimumAllowedSize) {throw std::invalid_argument("[P]operator-(...) [Invalid Size]");}
  ResizePlan((size_t)NewSize);
  return *this;
}

//[DESC]: Increments the elements in the FormulaArray of the current Plan object and returns
//        the previous state.
//[PRE]:  The current Plan object must be properly initialized. 'DummyParameter' is not
//        used for the increment operation.
//[POST]: Increments the elements in the FormulaArray of the current Plan object and 
//        returns the previous state of the Plan object.
//[PARAM]: DummyParameter - For the compiler
//[THROW]: None
//[RETURN]: Returns a new Plan object representing the state of the current Plan before 
//          the increment operation.
Plan Plan::operator++(int DummyParameter)
{
  Plan OldState = *this;
  for(size_t i = 0; i < Size; i++)
  {
    FormulaArray[i].operator++(DummyParameter);
  }
  return OldState;
}

//[DESC]: Decrements the elements in the FormulaArray of the current Plan object 
//        and returns the previous state.
//[PRE]:  The current Plan object must be properly initialized. 'DummyParameter'
//        is not used for the decrement operation.
//[POST]: Decrements the elements in the FormulaArray of the current Plan object 
//        and returns the previous state of the Plan object.
//[PARAM]: DummyParameter - For the compiler
//[RETURN]: Returns a new Plan object representing the state of the current 
//          Plan before the decrement operation.
Plan Plan::operator--(int DummyParameter)
{
  Plan OldState = *this;
  for(size_t i = 0; i < Size; i++)
  {
    FormulaArray[i].operator--(DummyParameter);
  }
  return OldState;
}

//[DESC]: Increments the elements in the FormulaArray of the current Plan object 
//        and returns the updated state.
//[PRE]:  The current Plan object must be properly initialized.
//[POST]: Increments the elements in the FormulaArray of the current Plan object 
//        and returns the updated state of the Plan object.
//[RETURN]: Returns a reference to the modified current Plan object after the increment operation.
Plan& Plan::operator++()
{
  for(size_t i = 0; i < Size; i++)
  {
    FormulaArray[i].operator++();
  }    
  return *this;
}

//[DESC]: Decrements the elements in the FormulaArray of the current Plan 
//        object and returns the updated state.
//[PRE]:  The current Plan object must be properly initialized.
//[POST]: Decrements the elements in the FormulaArray of the current Plan
//        object and returns the updated state of the Plan object.
//[RETURN]: Returns a reference to the modified current Plan object after the decrement operation.
Plan& Plan::operator--()
{
  for(size_t i = 0; i < Size; i++)
  {
    FormulaArray[i].operator--();
  }    
  return *this;
}

//[DESC]: Increments the elements in the FormulaArray of the current Plan object by 
//        the specified 'IncrementValue' and returns the updated state.
//[PRE]:  The current Plan object must be properly initialized.
//[POST]: Increments the elements in the FormulaArray of the current Plan object by 
//        the specified 'IncrementValue' and returns the updated state of the Plan object.
//[PARAM]: IncrementValue - An unsigned integer representing the value by which the elements 
//         in the FormulaArray should be incremented.
//[RETURN]: Returns a reference to the modified current Plan object after the increment operation.
Plan& Plan::operator+=(unsigned int IncrementValue)
{
  for(size_t i = 0; i < Size; i++)
  {
    FormulaArray[i].operator+=(IncrementValue);
  }
  return *this;
}

//[DESC]: Decrements the elements in the FormulaArray of the current Plan object by 
//        the specified 'DecrementValue' and returns the updated state.
//[PRE]:  The current Plan object must be properly initialized.
//[POST]: Decrements the elements in the FormulaArray of the current Plan object 
//        by the specified 'DecrementValue' and returns the updated state of the Plan object.
//[PARAM]: DecrementValue - An unsigned integer representing the value by which the elements 
//         in the FormulaArray should be decremented.
//[RETURN]: Returns a reference to the modified current Plan object after the decrement operation.
Plan& Plan::operator-=(unsigned int DecrementValue)
{
  for(size_t i = 0; i < Size; i++)
  {
    FormulaArray[i].operator-=(DecrementValue);
  }
  return *this;
}

}//[NAMESPACE]: ResourceConversion