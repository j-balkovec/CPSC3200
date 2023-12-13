//[DESC]: This file contains the implementation of the 'ExecutablePlan' class. 
//        It is a derived class from the Plan base class and extends its
///       functionality to provide a framework for managing and executing a sequence of formulas.
//
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Fri 29th Oct
//
//[VERSION]: Revision History
//           [1.0] Class design, Methods
//           [2.0] Deep, Shallow copying
//           [3.0] Debugging
//
//[INVARIANT]: Formulas added to the ExecutablePlan must not have already been applied or completed.
//[INVARIANT]: The client is restricted from replacing formulas that have already been applied or 
//             removing the last formula if it has been marked as completed.
//[INVARIANT]: The client may query the current step using the Step property.
//[INVARIANT]: The client can apply the current formula using the PlanApply method, 
//             which advances the plan to the next step.
//[INVARIANT]: The ExecutablePlan class supports deep copying through the EPlanDeepCopy method, 
//             which includes a deep copy of its variables as well as those of the base class.

#include <stdexcept>
#include <algorithm>
#include <new>
#include <memory>
#include <utility>
#include <optional>

#include <iostream>

#include "Stockpile.h"
#include "ExecutablePlan.h"
#include "Formula.h"
#include "Plan.h"

namespace ResourceConversion {

//[DESC]: 'ExecutablePlan' Default constructor that sets the initial private/protected members to it's
//        default values.
//[NOTE]: Defining a no-argument construcotr allows for heap allocations/arrays of the ExecutablePlan
//        obejcts
//[PRE]: Should only be invoked upon using heap allocations or array allocations
//[POST]: A default object is created
//[THROW]: None
ExecutablePlan::ExecutablePlan() : Plan()
{
    Step = 0;
    CompletedArray = new (std::nothrow) bool[Size];
    for(size_t i = 0; i < Size; i++)
    {
        CompletedArray[i] = false;
    }
}

//[DESC]: This parametrized constructor for 'ExecutablePlan' initializes its private/protected members
//        to the specified parameters.
//[PRE]: Should be invoked upon constructing an 'ExecutablePlan' object on the stack or in static
//       memory
//[POST]: ExecutablePlan object is constructed via passed parameters;
//[THROW]: 'std::invalid_argument' if the Step is invalid
//[LAMBDA]: 'SetCompletedArrayValues' Captures the 'this' pointer and 'Size' to set the CompletedArray
//          values to FALSE;
ExecutablePlan::ExecutablePlan(Formula* FormulaArray_, size_t Size_, unsigned int CurrentStep) : Plan(FormulaArray_, Size_)
    {
        if(CurrentStep >= Size_)
        {
            throw std::invalid_argument("[EP]ExecutablePlan(...) [Current step is invalid]");
        }
        
        Step = CurrentStep;
        this -> CompletedArraySize = Size_;
        
        auto SetCompletedArrayValues = [this]() {
            bool const InitialUncompletedValue = false;
            for(size_t i = 0; i < CompletedArraySize; i++)
            {
                CompletedArray[i] = InitialUncompletedValue;
            }
        };
        
        CompletedArray = new (std::nothrow) bool[CompletedArraySize];
        SetCompletedArrayValues();
    }

//[DESC]: Destructor for the 'ExecutablePlan' class, handles heap memory for the 'CompletedArray'.
//        The destructor is tagged as virtual in the base class to clean up the memory
//[PRE]: Object has to go out of scope
//[POST]: Memory is deallocated and the parameters are zero-ed out
ExecutablePlan::~ExecutablePlan() {ClearXPlan();}

//[DESC]: Private utility method that handles memory deallocation
//[PRE]: Object has to go out of scope
//[POST]: Deallocates memory that was allocated internally
//[INVOKE]: In Destructor and Copy Assignment operator
inline void ExecutablePlan::ClearXPlan ()
{
    if(CompletedArray != nullptr) {delete [] CompletedArray;}
    CompletedArray = nullptr;
    Step = 0;
}

//[DESC]: Private utility method that handles copying of the data
//[PRE]: 'Other' has to be a valid object
//[POST]: Data is copied
//[INVOKE]: In Copy construcotr and Copy Assignment operator
inline void ExecutablePlan::CopyXPlanData (const ExecutablePlan& other)
{
    Step = other.Step;
    CompletedArraySize = other.CompletedArraySize;
    CompletedArray = new (std::nothrow) bool[CompletedArraySize];
    
    for(size_t i = 0; i < CompletedArraySize; i++)
    {
        CompletedArray[i] = other.CompletedArray[i];
    }
}

// [DESC]: Resets the ExecutablePlan by setting its CompletedArray to nullptr,
//         resetting the CompletedArraySize to 0, and setting the Step to 0.
// [PRE]: None.
// [POST]: The ExecutablePlan is reset to its initial state with CompletedArray set to nullptr,
//         CompletedArraySize set to 0, and Step set to 0.
inline void ExecutablePlan::ResetXPlan ()
{
    CompletedArray = nullptr;
    CompletedArraySize = 0;
    Step = 0;
}

// [DESC]: Reassigns data from another ExecutablePlan by moving its contents to this object.
//         The other object's CompletedArray, CompletedArraySize, and Step values are transferred to
//         this object.
// [PRE]: The 'other' ExecutablePlan object is in a valid state, but it may be left in an unspecified
//        state after this operation.
// [POST]: This ExecutablePlan object receives the data (CompletedArray, CompletedArraySize, and Step)
//         from 'other,' and 'other' may be in an unspecified state after this operation.
inline void ExecutablePlan::ReassignDataXPlan(ExecutablePlan&& other) const
{
    other.CompletedArray = CompletedArray;
    other.CompletedArraySize = CompletedArraySize;
    other.Step = Step;
}

// [DESC]: Swaps the data of this ExecutablePlan with another ExecutablePlan using move semantics.
//         The CompletedArray, CompletedArraySize, and Step values of 'other' are swapped with those
//         of this object.
// [PRE]: The 'other' ExecutablePlan object should be in a valid state.
// [POST]: The data (CompletedArray, CompletedArraySize, and Step) of this ExecutablePlan is swapped
//         with 'other'. Both this object and 'other' may have different data after the operation.
inline void ExecutablePlan::SwapDataXPlan(ExecutablePlan&& other)
{
    std::swap(other.CompletedArray, CompletedArray);
    std::swap(other.CompletedArraySize, CompletedArraySize);
    std::swap(other.Step, Step);
}

//[DESC]: Copy constructor for the 'Exeutable Plan' class, allows for Deep Copying of the objects
//[PRE]: 'this' and 'other' are in valid state
//[POST]: Object is copied
ExecutablePlan::ExecutablePlan(const ExecutablePlan& other) : Plan(other)
{
    CopyXPlanData(other);
}

//[DESC]: Copy Assignment operator for the ExecutablePlan class, marked with the [[nodiscard]]
//        attribute. The operator is responsible for copying the contents of one ExecutablePlan object
//        to another and ensures that resources are correctly managed during this process.
//
//[PRE]: 'this' and 'other' are in valid state
//[POST]: Object is copied and capable of handling chain assignment
//[NOTE]: Invoke the base class Copy Assignment operator
//[ATTRIBUTE]: [[nodiscard]] -> Returned reference should not be ignored
[[nodiscard]]ExecutablePlan& ExecutablePlan::operator=(const ExecutablePlan& other)
{
    if(this == &other) { return *this; }
    ClearXPlan();
    CopyXPlanData(other);
    
    Plan::operator=(other);
    
    return *this;
}

//[DESC]: Move Constructor for the 'ExecutablePlan'. Allows for efficient deep copying
//[PRE]: Object MUST be in a valid state
//[POST]: The data, and poniters are re-assigned.
//
//[NOTE]: Very efficient copying, to invoke it use std::move(T) to pass in a rvalue reference
//        Pre conditions are important, if not met the program will crash
ExecutablePlan::ExecutablePlan(ExecutablePlan &&other) noexcept : Plan(std::move(other))
{
    ReassignDataXPlan(std::move(other));
    ClearXPlan();
}

//[DESC]: Move assignment operator for the 'ExecutablePlan' class. Allows for efficient deep copying
//[PRE]: Object MUST be in a valid state
//[POST]: The data, and poniters are swapped around.
//
//[NOTE]: Invoke the base class Move Assignment operator. To invoke it use std::move(T) to pass in a
//        rvalue reference. Pre conditions are important, if not met the program will crash
[[nodiscard]]ExecutablePlan& ExecutablePlan::operator=(ExecutablePlan &&other) noexcept
{
    if(this == &other) { return *this; }
    SwapDataXPlan(std::move(other));
    
    Plan::operator=(std::move(other));
    
    return *this;
}

//[DESC]: Adds a value to the CompletedArray.
//[PRE]: The 'CompletedArray' must be a valid pointer to an existing C-style array of booleans.
//[POST]: The CompletedArray is resized to include the new value, and `CompletedArraySize` is updated
//        accordingly.
inline void ExecutablePlan::PushArrayValue(const bool &Value)
{
    size_t NewSize = CompletedArraySize + 1;
    bool* NewArray = new (std::nothrow) bool[NewSize];
    
    for(size_t i = 0; i < CompletedArraySize; i++)
    {
        NewArray[i] = CompletedArray[i];
    }
    NewArray[CompletedArraySize] = Value;
    delete [] CompletedArray;
    CompletedArray = NewArray;
    CompletedArraySize = NewSize;
}

//[DESC]: Add a new Formula to the ExecutablePlan.
//[PARAM]: NewFormula The Formula to be added.
//[PRE]: None.
//[POST]: {BASE}
//        - If adding the Formula exceeds the Capacity, the Plan is resized to accommodate it.
//          The new Formula is added to the end of the Plan, and the Size is updated accordingly.
//        {CHILD}
//        - Pushes a 'false' value to the completed array
//[THROW]: None, client is unable to pass in an invalid object due to the Constructor
void ExecutablePlan::AddFormula(const Formula &NewFormula)
{
    bool const InitialCompletedArrayValue = false;
    Plan::AddFormula(NewFormula);
    PushArrayValue(InitialCompletedArrayValue);
}



//[DESC]: Remove the last Formula from the Plan.
//[PRE]: The Size of the Plan should be greater than 0.
//[POST]: The last Formula is removed from the Plan, and the Size is decremented.
//
//[THROW]: std::logic_error if the Size is less than or equal to 0.
void ExecutablePlan::RemoveLastFormula()
{
    unsigned int LastElementIndex = static_cast<unsigned int>(Size);
    if(Step-1 == LastElementIndex && CompletedArray[CompletedArraySize-1] == true)
    {
        throw std::logic_error("[EP]RemoveLastFormula(): [Cannot remove last 'Formula' object]");
    }
    Plan::RemoveLastFormula();
}

//[DESC]: Replace the Formula at the specified index with a new Formula.
//[PRE]: Index should be within the valid range [0, Size - 1].
//
//[POST]: The Formula at the specified index is replaced with the new Formula.
//
//[THROW]: std::invalid_argument if Index < Step, and std::logic_error if the formula was already
//         completed
void ExecutablePlan::ReplaceFormula(const Formula& NewFormula, const size_t &Index)
{
    if(Index < Step)
    {
        throw std::invalid_argument("[EP]ReplaceFormula(...): [Index cannot be less than _Step]");
    }
    
    for(size_t i = 0; i < CompletedArraySize; i++)
    {
        if(CompletedArray[Index] == true)
        {
            throw std::logic_error("[EP]ReplaceFormula(...): [Cannot Replace, Formula was already applied]");
        }
        Plan::ReplaceFormula(NewFormula, Index);
    }
}

//[DESC]: Apply all Formulas in the Plan.ß
//
//[PRE]: The Size of the Plan should be greater than 0.
//
//[POST]: All Formulas in the Plan are applied sequentially.
//
//[THROW]: std::invalid_argument if the Formula was already completed
void ExecutablePlan::PlanApply() {
    if (Step >= CompletedArraySize) {
        throw std::out_of_range("[EP]PlanApply(): Step is out of range");
    }

    if (CompletedArray[Step]) {
        throw std::invalid_argument("[EP]PlanApply(): Formula was already applied");
    }

    if (Step >= Size) {
        throw std::out_of_range("[EP]PlanApply(): Step is out of range for FormulaArray");
    }

    FormulaArray[Step].Apply();

    CompletedArray[Step] = true;
    Step++;
}

//[DESC]: Applies the plan to the given Stockpile, modifying the Stockpile based on the plan's formulas.
//[PRE]: The StockpilePtr parameter must not be a null shared_ptr.
//[POST]: The Stockpile is updated according to the plan's formulas, and the resulting Stockpile is returned.
//
//[PARAM]: Reference to a a 'shared_ptr' of Type Stockpile
//[RETURN]: A shared_ptr to the updated Stockpile after applying the plan.
//[THROW]: Throws std::invalid_argument if StockpilePtr is a null shared_ptr.
//[NOTE]: This function iterates through the formulas in the plan, checks if the required resources are available 
//        in the Stockpile, applies the formulas, and updates the Stockpile accordingly.
std::shared_ptr<Stockpile> ExecutablePlan::PlanApply(const std::shared_ptr<Stockpile>& StockpilePtr)
{
    if (StockpilePtr == nullptr)
    {
        throw std::invalid_argument("[EP]PlanApply{Overload}(...) [StockpilePtr must not be NULL]");
    }

    std::shared_ptr<Stockpile> ResultStockpile = StockpilePtr;

    for (size_t i = 0; i < Size; i++)
    {
        Formula::StockpileDataLoader s_Data(FormulaArray[i]);
        bool QuantitiesAreSufficient = std::all_of(s_Data.s_InputResources.begin(),
            s_Data.s_InputResources.end(),
            [ResultStockpile, &s_Data](const std::string& resource) {
                return ResultStockpile -> HasResource(resource) &&
                       ResultStockpile -> GetResourceQuantity(resource) >=
                           s_Data.s_InputQuantities[&resource - &s_Data.s_InputResources[0]];
            });

        if (QuantitiesAreSufficient)
        {
            FormulaArray[i].Apply();
            for (size_t j = 0; j < s_Data.s_InputResources.size(); j++)
            {
                ResultStockpile -> DecreaseQuantity(s_Data.s_InputResources[j], s_Data.s_InputQuantities[j]);
            }

            for (size_t j = 0; j < FormulaArray[i].GetOutputResourcesSize(); j++)
            {
                ResultStockpile -> IncreaseQuantity(FormulaArray[i].GetOutputResources()[j], this -> FormulaArray[i].GetOutputQuantities()[j]);
            }
        }
    }
    return ResultStockpile;
}

// [DESC]: Overloads the inequality operator (!=) for comparing two ExecutablePlan objects.
//        Determines whether this ExecutablePlan is not equal to another ExecutablePlan
//        by comparing their 'Step' member and invoking the inequality operator of the base class.
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator!=' implementation for correct comparison.
//
// [POST]:
// - Returns true if this ExecutablePlan is not equal to 'other'; otherwise, returns false.
// - The comparison result is based on the inequality of 'Step' members and the base class comparison.
// - The state of both 'this' and 'other' ExecutablePlan objects remains unchanged.
//
// [NOTE]:
// - This operator!= function allows you to compare ExecutablePlan objects for inequality
//   based on their 'Step' and the equality operator of the base class (Plan).
// - The base class comparison is used to compare other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator!=' for meaningful results.
bool ExecutablePlan::operator!=(const ExecutablePlan& other) const { return (this -> Step != other.Step) && Plan::operator!=(other); }

// [DESC]: Overloads the equality operator (==) for comparing two ExecutablePlan objects.
//        Determines whether this ExecutablePlan is equal to another ExecutablePlan
//        by comparing their 'Step' member and invoking the equality operator of the base class.
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator==' implementation for correct comparison.
//
// [POST]:
// - Returns true if this ExecutablePlan is equal to 'other'; otherwise, returns false.
// - The comparison result is based on the equality of 'Step' members and the base class comparison.
// - The state of both 'this' and 'other' ExecutablePlan objects remains unchanged.
//
// [NOTE]:
// - This operator== function allows you to compare ExecutablePlan objects for equality
//   based on their 'Step' and the equality operator of the base class (Plan).
// - The base class comparison is used to compare other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator==' for meaningful results.
bool ExecutablePlan::operator==(const ExecutablePlan& other) const { return !ExecutablePlan::operator!=(other); }

// [DESC]: Overloads the less-than operator (<) for comparing two ExecutablePlan objects.
//        Determines whether this ExecutablePlan is less than another ExecutablePlan
//        by comparing their 'Step' member and invoking the less-than operator of the base class.
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator<' implementation for correct comparison.
//
// [POST]:
// - Returns true if this ExecutablePlan is less than 'other'; otherwise, returns false.
// - The comparison result is based on the 'Step' members and the base class comparison.
// - The state of both 'this' and 'other' ExecutablePlan objects remains unchanged.
//
// [NOTE]:
// - This operator< function allows you to compare ExecutablePlan objects for less-than relationship
//   based on their 'Step' and the less-than operator of the base class (Plan).
// - The base class comparison is used to compare other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator<' for meaningful results.
bool ExecutablePlan::operator<(const ExecutablePlan& other) const { return (this -> Step < other.Step) && Plan::operator<(other); }

// [DESC]: Overloads the greater-than operator (>) for comparing two ExecutablePlan objects.
//        Determines whether this ExecutablePlan is greater than another ExecutablePlan
//        by comparing their 'Step' member and invoking the greater-than operator of the base class.
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator>' implementation for correct comparison.
//
// [POST]:
// - Returns true if this ExecutablePlan is greater than 'other'; otherwise, returns false.
// - The comparison result is based on the 'Step' members and the base class comparison.
// - The state of both 'this' and 'other' ExecutablePlan objects remains unchanged.
//
// [NOTE]:
// - This operator> function allows you to compare ExecutablePlan objects for a greater-than relationship
//   based on their 'Step' and the greater-than operator of the base class (Plan).
// - The base class comparison is used to compare other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator>' for meaningful results.
bool ExecutablePlan::operator>(const ExecutablePlan& other) const { return (this -> Step > other.Step) && Plan::operator>(other); }

// [DESC]: Overloads the less-than or equal to operator (<=) for comparing two ExecutablePlan objects.
//        Determines whether this ExecutablePlan is less than or equal to another ExecutablePlan
//        by comparing their 'Step' member and invoking the less-than or equal to operator of the base class.
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator<=' implementation for correct comparison.
//
// [POST]:
// - Returns true if this ExecutablePlan is less than or equal to 'other'; otherwise, returns false.
// - The comparison result is based on the 'Step' members and the base class comparison.
// - The state of both 'this' and 'other' ExecutablePlan objects remains unchanged.
//
// [NOTE]:
// - This operator<= function allows you to compare ExecutablePlan objects for a less-than or equal to relationship
//   based on their 'Step' and the less-than or equal to operator of the base class (Plan).
// - The base class comparison is used to compare other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator<=' for meaningful results.
bool ExecutablePlan::operator<=(const ExecutablePlan& other) const { return (this -> Step <= other.Step) && Plan::operator<=(other); }

// [DESC]: Overloads the greater-than or equal to operator (>=) for comparing two ExecutablePlan objects.
//        Determines whether this ExecutablePlan is greater than or equal to another ExecutablePlan
//        by comparing their 'Step' member and invoking the greater-than or equal to operator of the base class.
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator>=' implementation for correct comparison.
//
// [POST]:
// - Returns true if this ExecutablePlan is greater than or equal to 'other'; otherwise, returns false.
// - The comparison result is based on the 'Step' members and the base class comparison.
// - The state of both 'this' and 'other' ExecutablePlan objects remains unchanged.
//
// [NOTE]:
// - This operator>= function allows you to compare ExecutablePlan objects for a greater-than or equal to relationship
//   based on their 'Step' and the greater-than or equal to operator of the base class (Plan).
// - The base class comparison is used to compare other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator>=' for meaningful results.
bool ExecutablePlan::operator>=(const ExecutablePlan& other) const { return (this -> Step >= other.Step) && Plan::operator>=(other); }

// [DESC]: Overloads the addition operator (+) for combining two ExecutablePlan objects.
//        Combines this ExecutablePlan with another ExecutablePlan by adding their 'Step' members
//        and invoking the addition operator of the base class (Plan).
//
// [PRE]:
// - Both this ExecutablePlan object and 'other' ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator+' implementation for correct combination.
//
// [POST]:
// - Returns a new ExecutablePlan that is the result of combining 'this' and 'other' ExecutablePlan objects.
// - The 'Step' member of 'this' ExecutablePlan is updated by adding 'other.Step'.
// - The base class (Plan) is combined by invoking the 'operator+' of the base class.
// - 'OldState' represents the state of 'this' ExecutablePlan before the addition and is returned.
//
// [NOTE]: Concatenation
// - This operator+ function allows you to concatenate ExecutablePlan objects by adding their 'Step' members
//   and invoking the addition operator of the base class (Plan).
// - The base class combination is used to concatenate other aspects of the ExecutablePlan objects.
// - Ensure that the base class (Plan) correctly implements 'operator+' for meaningful results.
ExecutablePlan ExecutablePlan::operator+(const ExecutablePlan& other) 
{
  ExecutablePlan OldState = *this;
  Step += other.Step;
  Plan::operator+(other);
  return OldState;
}

// [DESC]: Overloads the addition operator (+) for extending the size of the ExecutablePlan object.
//        Extends the size of this ExecutablePlan by invoking the addition operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator+' implementation for size extension.
// - 'NewSize' represents the number of additional elements to be added to the Plan.
//
// [POST]:
// - Returns the modified ExecutablePlan object with its size extended by 'NewSize' elements.
// - The base class (Plan) is modified to include 'NewSize' additional elements.
// - The state of 'this' ExecutablePlan object is updated to include the new elements.
//
// [NOTE]:
// - This operator+ function allows you to extend the size of the ExecutablePlan object by invoking
//   the addition operator of the base class (Plan) to add 'NewSize' additional elements.
// - Ensure that the base class (Plan) correctly implements 'operator+' for size extension.
ExecutablePlan ExecutablePlan::operator+(size_t NewSize)
{
    Plan::operator+(NewSize);
    return *this;
}

// [DESC]: Overloads the subtraction operator (-) for reducing the size of the ExecutablePlan object.
//        Reduces the size of this ExecutablePlan by invoking the subtraction operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator-' implementation for size reduction.
// - 'NewSize' represents the number of elements to be removed from the Plan.
//
// [POST]:
// - Returns the modified ExecutablePlan object with its size reduced by 'NewSize' elements.
// - The base class (Plan) is modified to remove 'NewSize' elements from the end of the Plan.
// - The state of 'this' ExecutablePlan object is updated to reflect the reduced size.
//
// [NOTE]:
// - This operator- function allows you to reduce the size of the ExecutablePlan object by invoking
//   the subtraction operator of the base class (Plan) to remove 'NewSize' elements from the end.
// - Ensure that the base class (Plan) correctly implements 'operator-' for size reduction.
ExecutablePlan ExecutablePlan::operator-(int NewSize)
{
    Plan::operator-(NewSize);
    return *this;
}

// [DESC]: Overloads the post-increment operator (++) for the ExecutablePlan object.
//        Performs a post-increment operation by invoking the post-increment operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator++' implementation for post-increment.
// - 'DummyParameter' is a placeholder parameter, typically unused, required for overloading the operator.
//
// [POST]:
// - Returns a copy of the ExecutablePlan object before the post-increment operation.
// - The base class (Plan) is post-incremented by invoking the 'operator++' of the base class.
// - The state of 'this' ExecutablePlan object is updated to reflect the post-increment operation.
//
// [NOTE]:
// - This operator++ function allows you to perform a post-increment operation on the ExecutablePlan object
//   by invoking the post-increment operator of the base class (Plan).
// - The 'DummyParameter' is a typical requirement for overloading the post-increment operator.
// - Ensure that the base class (Plan) correctly implements 'operator++' for post-increment.
ExecutablePlan ExecutablePlan::operator++(int DummyParameter)
{
    ExecutablePlan OldState = *this;
    Plan::operator++(DummyParameter);
    return OldState;
}

// [DESC]: Overloads the post-decrement operator (--) for the ExecutablePlan object.
//        Performs a post-decrement operation by invoking the post-decrement operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator--' implementation for post-decrement.
// - 'DummyParameter' is a placeholder parameter, typically unused, required for overloading the operator.
//
// [POST]:
// - Returns a copy of the ExecutablePlan object before the post-decrement operation.
// - The base class (Plan) is post-decremented by invoking the 'operator--' of the base class.
// - The state of 'this' ExecutablePlan object is updated to reflect the post-decrement operation.
//
// [NOTE]:
// - This operator-- function allows you to perform a post-decrement operation on the ExecutablePlan object
//   by invoking the post-decrement operator of the base class (Plan).
// - The 'DummyParameter' is a typical requirement for overloading the post-decrement operator.
// - Ensure that the base class (Plan) correctly implements 'operator--' for post-decrement.
ExecutablePlan ExecutablePlan::operator--(int DummyParameter)
{
    ExecutablePlan OldState = *this;
    Plan::operator--(DummyParameter);
    return OldState;
}

// [DESC]: Overloads the pre-increment operator (++) for the ExecutablePlan object.
//        Performs a pre-increment operation by invoking the pre-increment operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator++' implementation for pre-increment.
//
// [POST]:
// - Returns a reference to the modified ExecutablePlan object after the pre-increment operation.
// - The base class (Plan) is pre-incremented by invoking the 'operator++' of the base class.
// - The state of 'this' ExecutablePlan object is updated to reflect the pre-increment operation.
//
// [NOTE]:
// - This operator++ function allows you to perform a pre-increment operation on the ExecutablePlan object
//   by invoking the pre-increment operator of the base class (Plan).
// - Ensure that the base class (Plan) correctly implements 'operator++' for pre-increment.
ExecutablePlan& ExecutablePlan::operator++()
{
    Plan::operator++();
    return *this;
}

// [DESC]: Overloads the pre-decrement operator (--) for the ExecutablePlan object.
//        Performs a pre-decrement operation by invoking the pre-decrement operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator--' implementation for pre-decrement.
//
// [POST]:
// - Returns a reference to the modified ExecutablePlan object after the pre-decrement operation.
// - The base class (Plan) is pre-decremented by invoking the 'operator--' of the base class.
// - The state of 'this' ExecutablePlan object is updated to reflect the pre-decrement operation.
//
// [NOTE]:
// - This operator-- function allows you to perform a pre-decrement operation on the ExecutablePlan object
//   by invoking the pre-decrement operator of the base class (Plan).
// - Ensure that the base class (Plan) correctly implements 'operator--' for pre-decrement.
ExecutablePlan& ExecutablePlan::operator--()
{
    Plan::operator--();
    return *this;
}

// [DESC]: Overloads the addition assignment operator (+=) for the ExecutablePlan object.
//        Performs an addition assignment operation by invoking the addition assignment operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator+=' implementation for addition assignment.
// - 'IncrementValue' represents the value to be added to the ExecutablePlan and its base class.
//
// [POST]:
// - Returns a reference to the modified ExecutablePlan object after the addition assignment operation.
// - The base class (Plan) is modified to include 'IncrementValue'.
// - The state of 'this' ExecutablePlan object is updated to reflect the addition assignment operation.
//
// [NOTE]:
// - This operator+= function allows you to perform an addition assignment operation on the ExecutablePlan object
//   by invoking the addition assignment operator of the base class (Plan).
// - 'IncrementValue' represents the value to be added to the ExecutablePlan and its base class.
// - Ensure that the base class (Plan) correctly implements 'operator+=' for addition assignment.
ExecutablePlan& ExecutablePlan::operator+=(unsigned int IncrementValue)
{
    Plan::operator+=(IncrementValue);
    return *this;
}

// [DESC]: Overloads the subtraction assignment operator (-=) for the ExecutablePlan object.
//        Performs a subtraction assignment operation by invoking the subtraction assignment operator of the base class (Plan).
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - The base class (Plan) must have a valid 'operator-=' implementation for subtraction assignment.
// - 'DecrementValue' represents the value to be subtracted from the ExecutablePlan and its base class.
//
// [POST]:
// - Returns a reference to the modified ExecutablePlan object after the subtraction assignment operation.
// - The base class (Plan) is modified to subtract 'DecrementValue'.
// - The state of 'this' ExecutablePlan object is updated to reflect the subtraction assignment operation.
//
// [NOTE]:
// - This operator-= function allows you to perform a subtraction assignment operation on the ExecutablePlan object
//   by invoking the subtraction assignment operator of the base class (Plan).
// - 'DecrementValue' represents the value to be subtracted from the ExecutablePlan and its base class.
// - Ensure that the base class (Plan) correctly implements 'operator-=' for subtraction assignment.
ExecutablePlan& ExecutablePlan::operator-=(unsigned int DecrementValue)
{
    Plan::operator-=(DecrementValue);
    return *this;
}

// [DESC]: Overloads the subscript operator ([]) for the ExecutablePlan object.
//        Allows users to access the element at the specified index in the Plan.
//
// [PRE]:
// - The ExecutablePlan object must be properly initialized.
// - 'Index' is the index of the element to be accessed in the Plan.
//
// [POST]:
// - Returns a reference to the element at the specified 'Index' in the Plan.
//
// [NOTE]:
// - This operator[] function is used to directly access elements in the Plan using 'Index'.
Formula& ExecutablePlan::operator[](size_t Index) const { return Plan::operator[](Index); }
}//[NAMESPACE]: ResourceConversion