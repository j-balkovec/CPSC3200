//[DESC]: This file contains the implementation of the Formula class, which represents
//       a way of converting resources into other resources. The Formula class
//       encapsulates information about input resources, output resources,
//       chances of outcomes, proficiency levels, and the ability to apply the formula
//       to simulate outcomes.
//
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Fri 29th Oct
//
//[VERSION]: Revision History
//           - 1.0 [10/29/2023]: Improved Documentation
//           - 2.0 [10/29/2023]: Improved Move-Semantics and RNG in Apply
//           - 3.0 [10/29/2023]: Fixed ProficiiencyLevel increment process
//
//[INVARIANT]: Proficiency Level should be Non-Negative and within the valid range
//[INVARIANT]: Resource Arrays should contain valid Strings and should not be empty
//[INVARIANT]: Quantity Arrays should contain Non-Negative Quantities and should not be empty

#include <stdexcept>
#include <algorithm>
#include <cstring>
#include <iostream>
#include <cassert>
#include <memory>
#include <new>
#include <limits>

#include "Formula.h"

namespace ResourceConversion
{
//[DESC]: Constructor for the Formula class, initializing member variables.
//
//[PRE]: None.
//
//[POST]: A new Formula object is created with member variables initialized as follows:
//   - InputResourcesSize and InputQuantitiesSize are set to 0.
//   - InputResources, InputQuantities, OutputResources, OutputQuantities, and ResultArray are
//     set to nullptr.
//   - ProficiencyLevel is set to 0.
Formula::Formula ()
{
    InputResourcesSize = 0;
    InputResources = nullptr;
    
    InputQuantitiesSize = 0;
    InputQuantities = nullptr;
    
    OutputResourcesSize = 0;
    InputResources = nullptr;
    
    OutputQuantitiesSize = 0;
    OutputQuantities = nullptr;
    
    ResultArray = nullptr;
    
    ProficiencyLevel = 0;
}


//[DESC]: Constructor for the Formula class, initializing member variables with provided values.
//
// [PARAM LIST]
//      InputResources_ An array of input resource names.
//      InputResourcesSize_ The size of the input resource array.
//      InputQuantities_ An array of input resource quantities.
//      InputQuantitiesSize_ The size of the input quantity array.
//      OutputResources_ An array of output resource names.
//      OutputResourcesSize_ The size of the output resource array.
//      OutputQuantities_ An array of output resource quantities.
//      OutputQuantitiesSize_ The size of the output quantity array.
//      ResultArray_ An array for storing calculation results.
//      ProficencyLevel_ The proficiency level of the formula (0 to 5).
//
//[PRE]: - InputResources_ and OutputResources_ must not contain null, empty, or whitespace-only
//         strings.
//       - InputResourcesSize_ should match InputQuantitiesSize_.
//       - OutputResourcesSize_ should match OutputQuantitiesSize_.
//       - ProficencyLevel_ must not exceed 5.
//
//[POST]: A new Formula object is created with member variables initialized as follows:
//          - InputResources and InputQuantities are initialized based on the provided arrays.
//          - OutputResources and OutputQuantities are initialized based on the provided arrays.
//          - ResultArray is allocated based on the size of OutputQuantities.
//          - ProficiencyLevel is set to the provided value.
//
//[THROW]: std::invalid_argument if any preconditions are violated.
//[FIX]: error throwing
Formula::Formula (std::string* InputResources_, size_t InputResourcesSize_,
                  unsigned int* InputQuantities_, size_t InputQuantitiesSize_,
                  std::string* OutputResources_, size_t OutputResourcesSize_,
                  unsigned int* OutputQuantities_, size_t OutputQuantitiesSize_,
                  unsigned int* ResultArray_, unsigned int ProficencyLevel_)
{
    if (ContainsNullOrWhiteSpace (InputResources_, InputResourcesSize_)) {
        throw std::invalid_argument ("[F]Formula(...): [InputResources must not be null, empty, or contain only whitespace]");
    }
    
    if (ContainsNullOrWhiteSpace (OutputResources_, OutputResourcesSize_)) {
        throw std::invalid_argument ("[F]Formula(...): [OutputResources must not be null, empty, or contain only whitespace]");
    }
    
    if (InputResourcesSize_ != InputQuantitiesSize_) {
        throw std::invalid_argument ("[F]Formula(...): [lengths of [IN] -> Resources array doesn't match the [IN] -> Quantity array]");
    }
    
    if (OutputResourcesSize_ != OutputQuantitiesSize_) {
        throw std::invalid_argument ("[F]Formula(...): [lengths of [OUT] -> Resources array doesn't match the [OUT] -> Quantity array]");
    }
    
    if (ProficencyLevel_ > 5) {
        throw std::invalid_argument ("[F]Formula(...): [ProficiencyLevel must not exceed 5]");
    }
    
    this->InputResourcesSize = InputResourcesSize_;
    this->InputResources = InputResources_;
    InputResources_ = nullptr;
    
    this->InputQuantitiesSize = InputQuantitiesSize_;
    this->InputQuantities = InputQuantities_;
    InputQuantities_ = nullptr;
    
    this->OutputResourcesSize = OutputResourcesSize_;
    this->OutputResources = OutputResources_;
    OutputResources_ = nullptr;
    
    this->OutputQuantitiesSize = OutputQuantitiesSize_;
    this->OutputQuantities = OutputQuantities_;
    OutputQuantities_ = nullptr;
    
    this->ResultArray = ResultArray_;
    ResultArray_ = nullptr;
}

//[DESC]: Destructor for the Formula class, cleaning up resources.
//
//[PRE]: None.
//
//[POST]: The Formula object is destroyed, and any dynamically allocated memory and resources are
//        released.
Formula::~Formula () { ClearContainer (); }

//[DESC]: Copy constructor for the Formula class, creating a copy of another Formula object.
//
//[PARAM]: other The Formula object to be copied.
//
//[PRE]: None.
//
//[POST]: A new Formula object is created as a deep copy of the provided 'other' Formula object.
//        Both objects will have independent copies of data.
Formula::Formula (const Formula& other) { CopyData (other); }

//[DESC]: Copy assignment operator for the Formula class, assigning the data of another Formula object.
//
//[PARAM]: other The Formula object to be assigned from.
//
//[PRE]: None.
//
//[POST]: The current Formula object is assigned the data (deep copy) from the 'other' Formula object.
//        Both objects will have independent copies of data.
[[nodiscard]]Formula& Formula::operator=(const Formula& other)
{
    if (this == &other) { return *this; }
    ClearContainer ();
    CopyData (other);
    return *this;
}

//[DESC]: Move constructor for the Formula class, transferring ownership of resources from another
//        Formula object.
//
//[PARAM]: other The Formula object to be moved.
//
//[PRE]: None.
//
//[POST]: A new Formula object is created by moving the data (shallow copy) from the 'other' Formula
//        object. The 'other' object will be in a valid but unspecified state after the move.
//
//[NOTE]: Using 'std::move()' to actually pass in an rvalue reference
//        - 'std::move() converts the param <T> to an rvalue reference to indicate intent of moving
//           the object
Formula::Formula (Formula&& other) noexcept
{
    ReassignData(std::move(other));
    ResetContainer ();
}

//[DESC]: Move assignment operator for the Formula class, transferring ownership of resources from
//        another Formula object.
//
//[PARAM]: other The Formula object to be moved from.
//
//[PRE]: None.
//
//[POST]: The current Formula object is assigned the data (shallow copy) from the 'other' Formula
//        object.
//        The 'other' object will be in a valid but unspecified state after the move.
//
//[NOTE]: Using 'std::move()' to actually pass in an rvalue reference
//        - 'std::move() converts the param <T> to an rvalue reference to indicate intent of moving
//           the object
[[nodiscard]]Formula& Formula::operator=(Formula&& other) noexcept
{
    if (this == &other) { return *this; }
    SwapData(std::move(other));
    return *this;
}

//[DESC]: Copies data from another Formula object to the current object.
//
//[PARAM]: other The Formula object from which data is to be copied.
//
//[PRE]: The 'other' Formula object must be valid and contain data.
//
//[POST]: The data from the 'other' Formula object is copied to the current object.
//        The current object's member variables are updated with deep copies of the 'other' object's
//        data.
void Formula::CopyData (const Formula& other)
{
    InputResourcesSize = other.InputResourcesSize;
    InputResources = new (std::nothrow) std::string[InputResourcesSize];
    for (size_t i = 0; i < InputResourcesSize; ++i)
    {
        InputResources[i] = other.InputResources[i];
    }
    
    InputQuantitiesSize = other.InputQuantitiesSize;
    InputQuantities = new (std::nothrow) unsigned int[InputQuantitiesSize];
    for (size_t i = 0; i < InputQuantitiesSize; ++i)
    {
        InputQuantities[i] = other.InputQuantities[i];
    }
    
    OutputResourcesSize = other.OutputResourcesSize;
    OutputResources = new (std::nothrow) std::string[OutputResourcesSize];
    for (size_t i = 0; i < OutputResourcesSize; ++i)
    {
        OutputResources[i] = other.OutputResources[i];
    }
    
    OutputQuantitiesSize = other.OutputQuantitiesSize;
    OutputQuantities = new (std::nothrow) unsigned int[OutputQuantitiesSize];
    for (size_t i = 0; i < OutputQuantitiesSize; ++i)
    {
        OutputQuantities[i] = other.OutputQuantities[i];
    }
    
    ResultArray = new (std::nothrow) unsigned int[OutputQuantitiesSize];
    for (size_t i = 0; i < OutputQuantitiesSize; ++i)
    {
        ResultArray[i] = other.ResultArray[i];
    }
    
    ProficiencyLevel = other.ProficiencyLevel;
}

//[DESC]: Clears and deallocates memory used by member variables.
//
//[PRE]: None.
//
//[POST]: Any dynamically allocated memory and resources used by member variables are released.
//        Member variables are set to nullptr or appropriate initial values.
inline void Formula::ClearContainer ()
{
    if (InputResources != nullptr) {
        delete[] InputResources;
        InputResources = nullptr;
    }
    
    if (InputQuantities != nullptr) {
        delete[] InputQuantities;
        InputQuantities = nullptr;
    }
    
    if (OutputResources != nullptr) {
        delete[] OutputResources;
        OutputResources = nullptr;
    }
    
    if (OutputQuantities != nullptr) {
        delete[] OutputQuantities;
        OutputQuantities = nullptr;
    }
    
    if (ResultArray != nullptr) {
        delete[] ResultArray;
        ResultArray = nullptr;
    }
}

//[DESC]: Resets the member variables of another Formula object.
//
//[PARAM]: other The Formula object whose member variables are to be reset.
//
//[PRE]: The 'other' Formula object must be valid.
//
//[POST]: The member variables of the 'other' Formula object are set to nullptr or appropriate initial
//        values.
inline void Formula::ResetContainer ()
{
    InputResources = nullptr;
    OutputResources = nullptr;
    InputQuantities = nullptr;
    OutputQuantities = nullptr;
    ResultArray = nullptr;
    
    InputResourcesSize = 0;
    InputQuantitiesSize = 0;
    OutputResourcesSize = 0;
    OutputQuantitiesSize = 0;
    
    ProficiencyLevel = 0;
}

inline void Formula::SwapData(Formula &&other) 
{
    std::swap(other.InputResources, InputResources);
    std::swap(other.InputQuantities, InputQuantities);
    std::swap(other.OutputResources, OutputResources);
    std::swap(other.OutputQuantities, OutputQuantities);
    
    std::swap(other.ResultArray, ResultArray);
    
    std::swap(other.InputResourcesSize, InputResourcesSize);
    std::swap(other.InputQuantitiesSize, InputQuantitiesSize);
    std::swap(other.OutputResourcesSize, OutputResourcesSize);
    std::swap(other.OutputQuantitiesSize, OutputQuantitiesSize);
    
    std::swap(other.ProficiencyLevel, ProficiencyLevel);
}

inline void Formula::ReassignData(Formula &&other) const
{
    other.InputResources = InputResources;
    other.InputQuantities = InputQuantities;
    other.OutputResources = OutputResources;
    other.OutputQuantities = OutputQuantities;
    
    other.ResultArray = ResultArray;
    
    other.InputResourcesSize = InputResourcesSize;
    other.InputQuantitiesSize = InputQuantitiesSize;
    other.OutputResourcesSize = OutputResourcesSize;
    other.OutputQuantitiesSize = OutputQuantitiesSize;
    
    other.ProficiencyLevel = ProficiencyLevel;
}

//[DESC]: Calculates and returns outcome modifiers based on the proficiency level.
//
//[PARAM]: Level The proficiency level of the formula (0 to 5).
//
//[PRE]: The 'Level' parameter must be between 0 and 5.
//
//[POST]: Outcome modifiers are calculated based on the proficiency level and returned as an
//        OutcomeModifiers object.
inline Formula::OutcomeModifiers Formula::GetOutcomeChances (const unsigned int &Level)
{
    const float DefaultFailureConst = 0.25f;
    const float DefaultPartialConst = 0.2f;
    const float DefaultBonusConst = 0.05f;
    const float DefaultNormalConst = 0.5f;
    
    OutcomeModifiers InitialValues (DefaultFailureConst,
                                    DefaultPartialConst,
                                    DefaultBonusConst,
                                    DefaultNormalConst);
    
    if (Level == 0)
    {
        return InitialValues;
    }
    
    float ModifiedFailure = InitialValues.Failure - (static_cast<float>(Level) * 0.05f);
    float ModifiedPartial = InitialValues.Partial - (static_cast<float>(Level) * 0.05f);
    float ModifiedBonus = InitialValues.Bonus + (static_cast<float>(Level) * 0.05f);
    float ModifiedNormal = InitialValues.Normal + (static_cast<float>(Level) * 0.05f);
    
    return OutcomeModifiers (ModifiedFailure,
                             ModifiedPartial,
                             ModifiedBonus,
                             ModifiedNormal);
}

//[DESC]: Checks if an array of strings contains null, empty, or whitespace strings.
//
//[PARAM]: Array The array of strings to be checked.
//[PARAM]: ArraySize The size of the array.
//
//[PRE]: The 'Array' parameter must point to a valid array of strings, and 'ArraySize' must be a
//       non-negative value.
//
//[POST]: Returns 'true' if the array contains any null, empty, or whitespace strings; otherwise,
//        returns 'false'.
//        The function does not modify the input array.
inline bool Formula::ContainsNullOrWhiteSpace (const std::string* Array,const size_t &ArraySize) const
{
    for (size_t i = 0; i < ArraySize; ++i)
    {
        if (Array[i].empty () || std::all_of (Array[i].begin (), Array[i].end (), ::isspace))
            return true;
    }
    return false;
}

//[DESC]: Applies the formula and computes the outcome based on proficiency level and chance modifiers.
//
//[PRE]: The Formula object must be properly initialized with valid data and proficiency level.
//
//[POST]: The formula is applied, and the outcome is computed according to the proficiency level and
//        chance modifiers.
//        The 'ResultArray' member variable is updated with the computed outcome.
//        The function does not modify other member variables or external state.
void Formula::Apply ()
{
    if (InputQuantities == nullptr  || 
        OutputQuantities == nullptr || 
        InputResources == nullptr   || 
        OutputResources == nullptr) 
    {
        throw std::invalid_argument("[F]Apply(): [Attempting to dereference nullptr in the 'Apply' Method]");
    }
    OutcomeModifiers OutcomeChances = GetOutcomeChances (ProficiencyLevel);
    
    float ChanceOfFailure = OutcomeChances.Failure;
    float ChanceOfPartial = OutcomeChances.Partial;
    float ChanceOfBonus = OutcomeChances.Bonus;
    float ChanceOfNormal = OutcomeChances.Normal;
    
    constexpr unsigned int UpperBound = 1;
    
    constexpr unsigned int FailedArraySize = 1;
    constexpr unsigned int FailedValue = 0;
    
    float RandomValue = 0.0f;
    
    unsigned int Count = 0;

    auto GetRandomFloat = [this, RandomValue]() mutable -> float {
        std::random_device RandomDevice;
        std::mt19937 Generator(RandomDevice());
        std::uniform_real_distribution<float> Distribution (0.0f, 1.0f);
        
        RandomValue = Distribution (Generator);
        return std::round (RandomValue * 100.0f) / 100.0f;
    };
    
    auto IncrementProficiencyLevel = [this, Count]() -> void {
        if(Count % 5 == 0 && ProficiencyLevel <= 5) 
        {
            ProficiencyLevel++;
        }
    };

    constexpr float BonusConstModifier = 1.1f;
    constexpr float PartialConstModifier = 0.75f;
    
    RandomValue = GetRandomFloat();
    
    //Failure Out -> 0.25
    if (RandomValue < ChanceOfFailure)
    {
        std::memset (ResultArray, FailedValue, FailedArraySize);
    }
    
    //Partial Out -> 0.2
    if (RandomValue > ChanceOfFailure &&
        RandomValue < (ChanceOfPartial + ChanceOfFailure))
    {
        for (size_t i = 0; i < OutputQuantitiesSize; ++i)
        {
            ResultArray[i] = static_cast<unsigned int>(std::floor (static_cast<float>(OutputQuantities[i]) * PartialConstModifier));
        }
    }
    
    //Bonus Out -> 0.05
    if (RandomValue > (ChanceOfPartial + ChanceOfFailure) &&
        RandomValue < ChanceOfNormal)
    {
        for (size_t i = 0; i < OutputQuantitiesSize; ++i)
        {
            ResultArray[i] = static_cast<unsigned int>(std::ceil (static_cast<float>(OutputQuantities[i]) * BonusConstModifier));
        }
    }
    
    //Normal Out -> 0.5
    if (RandomValue > (ChanceOfFailure + ChanceOfBonus + ChanceOfPartial) &&
        RandomValue < static_cast<double>(UpperBound))
    {
        for (size_t i = 0; i < OutputQuantitiesSize; ++i)
        {
            ResultArray[i] = OutputQuantities[i];
        }
    }
    Count++;
    IncrementProficiencyLevel();
}

//[DESC]: Displays the values of input and output resources for the Formula.
//
//[PARAM LIST]:
//      PrintResultArray - A boolean flag to determine whether to print the result array (if true) or
//                         the output quantities (if false).
//
//[PRE]: The 'ShouldPrintValues' member variable should be true to enable value printing.
//
//[POST]: The values of input resources, input quantities, output resources, and either the result
//        array or output quantities are displayed.
//
//[THROW]: std::invalid_argument - if the sizes of input resources and input quantities do not match.
//
//[NOTES]:
//        - This function respects the 'PrintResultArray' parameter. If 'PrintResultArray' is true, it
//          displays the result array; otherwise, it displays the output quantities.
//
//        - To enable value printing, ensure that the 'ShouldPrintValues' member variable is set to
//        true. If 'ShouldPrintValues' is false, no values will be displayed.
//
//        - If 'PrintResultArray' is set to true and the 'ResultArray' member variable has not been
//        properly initialized, the displayed result array may contain uninitialized data.
void Formula::DisplayFormulaValues(const bool PrintResultArray) const
{
    const std::string BLUE = "\033[1;34m";
    const std::string RED = "\033[1;31m";
    const std::string GREEN = "\033[1;32m";
    const std::string YELLOW = "\033[1;33m";

    const std::string RESET = "\033[0m";

    auto IsResultArrayZero = [this]() -> bool {
        if(ResultArray[0] == 0)
        {
            return true;
        }
        return false;
    };
    
    if(ShouldPrintValues)
    {
        if(InputResourcesSize != InputQuantitiesSize)
        {
            throw std::invalid_argument("[F]DisplayFormulaValues(...): [lengths of [IN] -> Resources array doesn't match the [IN] -> Quantity array]");
        }
        std::cout << " \n" << BLUE << "<[" << RESET;
        {
            
        };
        
        for(size_t i = 0; i < InputResourcesSize; i++)
        {
            std::cout  << "{" << InputResources[i] << "} : {" << YELLOW << InputQuantities[i] << RESET << "}";
        }
        std::cout  << BLUE << "]>" << RESET << GREEN << " <-+-> " << RESET << BLUE << " <[" << RESET;
        
        if(IsResultArrayZero() && PrintResultArray) { std::cout << RED << "FAILED]>" << RESET; return; }
        
        for(size_t i = 0; i < OutputResourcesSize; i++)
        {
            if(PrintResultArray && ResultArray != nullptr)
            {
                std::cout  << "{" << OutputResources[i] << RESET << "} : {" << YELLOW << ResultArray[i] << RESET << "}";
            } else {
                std::cout  << "{" << OutputResources[i] << RESET << "} : {" << YELLOW << OutputQuantities[i] << RESET << "}";
            }
        }
        std::cout << BLUE << "]>" << RESET << "\n";
    }
}

//[DESC]: A function to check if two arrays are equal in size and content.
//[PRE]: Assumes that the arrays are valid and properly allocated in memory.
//[POST]: Returns true if the arrays are equal, otherwise false.
//[PARAM]: 
// - T* First: A pointer to the first array.
// - T* Second: A pointer to the second array.
// - size_t FirstArraySize: Size of the first array.
// - size_t SecondArraySize: Size of the second array.
//
//[RETURN]: Returns a boolean value (true if arrays are equal, false if not).
//[NOTE]: This function is templated to work with either unsigned int or std::string arrays.
//[TEMPLATE]: Enables 'T' if 'T' is either a 'std::string' or a 'size_t'
template<typename T>
typename std::enable_if<std::is_same<T, unsigned int>::value || std::is_same<T, std::string>::value, bool>::type 
inline Formula::ArraysAreEqual(T* First, T* Second, size_t FirstArraySize, size_t SecondArraySize) const
{
    if(FirstArraySize != SecondArraySize) { return false; }
    for(size_t i = 0; i < FirstArraySize; i++)
    {
        if(First[i] != Second[i]) { return false; }
    }
    return true;
}

//[DESC]: Decrements each element in an unsigned int array by a specified value and checks for underflow.
//[PRE]: Expects a valid unsigned int array and a valid positive DecrementValue.
//[POST]: Decrements each element in the array and checks for underflow conditions.
//[PARAM]:
// - unsigned int* &Array: A reference to the unsigned int array to be modified.
// - size_t ArraySize: Size of the array.
// - unsigned int DecrementValue: The value to decrement each element in the array.
//
//[RETURN]: No return value; the array is modified in place.
//[NOTE]: This function decrements the elements in the array and checks for underflow. If any element becomes less than DecrementValue or is already 0, it throws an exception to indicate an underflow condition.

inline void Formula::Decrement(unsigned int* &Array, size_t ArraySize, unsigned int DecrementValue)
{
    for(size_t i = 0; i < ArraySize; i++)
    {
        if(Array[i] < DecrementValue) {throw std::invalid_argument("[F]Decrement(...): uint Underflow");}
        if(Array[i] != 0) { Array[i] -= DecrementValue; }
    }
}

//[DESC]: Increments each element in an unsigned int array by a specified value 
//        and checks for overflow.
//[PRE]: Expects a valid unsigned int array and a valid positive IncrementValue.
//[POST]: Increments each element in the array and checks for overflow conditions.
//
//[PARAM]:
// - unsigned int* &Array: A reference to the unsigned int array to be modified.
// - size_t ArraySize: Size of the array.
// - unsigned int IncrementValue: The value to increment each element in the array.
//
//[RETURN]: No return value; the array is modified in place.
//[NOTE]: This function increments the elements in the array and checks for overflow. 
//        If any element approaches the maximum value that can be held by an 
//        unsigned int (std::numeric_limits<unsigned int>::max()) or is already at 
//        the maximum value, it throws an exception to indicate an overflow condition.
inline void Formula::Increment(unsigned int* &Array, size_t ArraySize, unsigned int IncrementValue)
{
    for(size_t i = 0; i < ArraySize; i++)
    {
        if(std::numeric_limits<unsigned int>::max() - Array[i] <= IncrementValue) {throw std::invalid_argument("[F]Increment(...): uint Overflow");}
        if(Array[i] != 0) { Array[i] += IncrementValue; }
    }
}

//[DESC]: Checks if the current Formula object is equal to another Formula 
//        object by comparing arrays of input and output quantities and resources.
//[PRE]: Expects valid Formula objects with properly allocated arrays.
//[POST]: Compares the input and output arrays of both Formula objects for equality.
//
//[PARAM]: 
// - const Formula& other: A reference to another Formula object for comparison.
//[RETURN]: Returns a boolean value (true if the Formula objects are equal, false if not).
//
//[NOTE]: This overloaded equality operator checks for equality by comparing the 
//        input and output arrays of both Formula objects. It relies on the "ArraysAreEqual" 
//        function to perform the array comparisons.
bool Formula::operator==(const Formula& other) const
{
    bool In_Quantities = ArraysAreEqual(InputQuantities, 
                                        other.InputQuantities, 
                                        InputQuantitiesSize, 
                                        other.InputQuantitiesSize);

    bool In_Resources = ArraysAreEqual(InputResources, 
                                       other.InputResources, 
                                       InputResourcesSize, 
                                       other.InputResourcesSize);

    bool Out_Quantities = ArraysAreEqual(OutputQuantities, 
                                         other.OutputQuantities, 
                                         OutputQuantitiesSize, 
                                         other.OutputQuantitiesSize);

    bool Out_Resources = ArraysAreEqual(OutputResources, 
                                        other.OutputResources, 
                                        OutputResourcesSize, 
                                        other.OutputResourcesSize);

    return In_Quantities && In_Resources && Out_Quantities && Out_Resources;
}

//[DESC]: Checks if the current Formula object is not equal to another Formula object
//        by using the inequality operator.
//[PRE]: Expects valid Formula objects with properly allocated arrays.
//[POST]: Determines the inequality of the current Formula object with another Formula object.
//
//[PARAM]:
// - const Formula& other: A reference to another Formula object for comparison.
//[RETURN]: Returns a boolean value (true if the Formula objects are not equal, false if they are equal).
//[NOTE]: This overloaded inequality operator (!=) relies on the equality operator (==)
//        to determine inequality. It inverts the result of the equality comparison to 
//        determine inequality.
bool Formula::operator!=(const Formula& other) const
{
    bool PlaceHolderValue = operator==(other);
    return !PlaceHolderValue;
}

//[DESC]: Overloaded '+' operator, increments the Qunatity array elements by the 'IncrementValue'
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are incremented by 'IncrementValue'.
//[PARAM]: IncrementValue - The value by which to increment the arrays.
//[RETURN]: Current State of the Object (*this)
//[NOTE]: This overloaded inequality operator (+) relies on the Increment(...) utility method
Formula Formula::operator+(unsigned int IncrementValue)
{
    Increment(InputQuantities, InputQuantitiesSize, IncrementValue);
    Increment(OutputQuantities, OutputQuantitiesSize, IncrementValue);
    return *this;
}

//[DESC]: Overloaded '-' operator, decrements the Qunatity array elements by the 'DecrementValue'
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are decremented by 'DecrementValue'.
//[PARAM]: IncrementValue - The value by which to increment the arrays.
//[RETURN]: Current State of the Object (*this)
//[NOTE]: This overloaded inequality operator (+) relies on the Decrement(...) utility method
Formula Formula::operator-(unsigned int DecrementValue)
{
    Decrement(InputQuantities, InputQuantitiesSize, DecrementValue);
    Decrement(OutputQuantities, OutputQuantitiesSize, DecrementValue);
    return *this;
}

// [DESC]: Overloaded post-increment(++) operator for the Formula class.
// [PRE]:  None.
// [POST]: The InputQuantities and OutputQuantities arrays are incremented by the value of 1.
// [PARAM]: int DummyParameter (Unused parameter for the compiler).
// [RETURN]: The previous state of the object as a copy.
// [NOTE]: This overloaded post-increment operator (++) increments the InputQuantities and 
//         OutputQuantities arrays by a default increment value and returns the previous 
//         state of the object.
Formula Formula::operator++(int DummyParameter)
{
    Formula OldState = *this;
    constexpr unsigned int DefaultIncrementValue = 1;

    Increment(InputQuantities, InputQuantitiesSize, DefaultIncrementValue);
    Increment(OutputQuantities, OutputQuantitiesSize, DefaultIncrementValue);

    return OldState;
}

//[DESC]: Overloaded post-decrement(--) operator for the Formula class.
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are decremented by the value of 1.
//[PARAM]: int DummyParameter (Unused parameter for the compiler).
//[RETURN]: The previous state of the object as a copy.
//[NOTE]: This overloaded post-decrement operator (--) decrements the InputQuantities and 
//        OutputQuantities arrays by a default decrement value and returns the previous 
//        state of the object.
Formula Formula::operator--(int DummyParameter)
{
    Formula OldState = *this;
    constexpr unsigned int DefaultDecrementValue = 1;

    Decrement(InputQuantities, InputQuantitiesSize, DefaultDecrementValue);
    Decrement(OutputQuantities, OutputQuantitiesSize, DefaultDecrementValue);

    return OldState;
}


//[DESC]: Overloaded pre-increment(++) operator for the Formula class.
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are incremented by the value of 1.
//[RETURN]: A reference to the current state of the object (*this).
//[NOTE]: This overloaded pre-increment operator (++) increments the InputQuantities 
//        and OutputQuantities arrays by a default increment value and returns the 
//        current state of the object after the increment.
Formula& Formula::operator++()
{
    constexpr unsigned int DefaultIncrementValue = 1;

    Increment(InputQuantities, InputQuantitiesSize, DefaultIncrementValue);
    Increment(OutputQuantities, OutputQuantitiesSize, DefaultIncrementValue);
    return *this;
}

//[DESC]: Overloaded pre-decrement(--) operator for the Formula class.
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are decremented by the value of 1.
//[RETURN]: A reference to the current state of the object (*this).
//[NOTE]: This overloaded pre-decrement operator (--) decrements the InputQuantities 
//        and OutputQuantities arrays by a default decrement value and returns the 
//        current state of the object after the decrement.
Formula& Formula::operator--()
{
    constexpr unsigned int DefaultDecrementValue = 1;

    Decrement(InputQuantities, InputQuantitiesSize, DefaultDecrementValue);
    Decrement(OutputQuantities, OutputQuantitiesSize, DefaultDecrementValue);
    return *this;
}


//[DESC]: Overloaded compound addition operator for the Formula class.
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are incremented by the specified 
//        IncrementValue.
//[PARAM]: IncrementValue - The value by which to increment the arrays.
//[RETURN]: A reference to the current state of the object (*this).
//[NOTE]: This overloaded compound addition operator (+=) increments the
//        InputQuantities and OutputQuantities arrays by the provided 
//        IncrementValue and returns the current state of the object after the increment.
Formula& Formula::operator+=(unsigned int IncrementValue)
{
    Increment(InputQuantities, InputQuantitiesSize, IncrementValue);
    Increment(OutputQuantities, OutputQuantitiesSize, IncrementValue);
    return *this;
}

//[DESC]: Overloaded compound subtraction operator for the Formula class.
//[PRE]:  None.
//[POST]: The InputQuantities and OutputQuantities arrays are decremented by the specified 
//        DecrementValue.
//[PARAM]: DecrementValue - The value by which to decrement the arrays.
//[RETURN]: A reference to the current state of the object (*this).
//[NOTE]: This overloaded compound subtraction operator (-=) decrements the
//        InputQuantities and OutputQuantities arrays by the provided 
//        DecrementValue and returns the current state of the object after the decrement.
Formula& Formula::operator-=(unsigned int DecrementValue)
{
    Decrement(InputQuantities, InputQuantitiesSize, DecrementValue);
    Decrement(OutputQuantities, OutputQuantitiesSize, DecrementValue);
    return *this;
}
}//[NAMESPACE]: ResourceConversion