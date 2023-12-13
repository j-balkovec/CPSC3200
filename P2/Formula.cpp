/**
 @file Formula.cpp
 @author Jakob Balkovec (CPSC 3200)
 @instructor A. Dingle (CPSC 3200)

 @date Wed 20th Sep

 @version
 Revision History
 - 1.0 [09/30/2023]: C# -> C++
 - 2.0 [01/10/2023]: Scrap and start over
 - 3.0 [02/10/2023]: Simplifed ArrayWrappers, handled memory, documentation

 @brief
 * This file contains the implementation of the Formula class, which represents
 * a way of converting resources into other resources. The Formula class
 * encapsulates information about input resources, output resources,
 * chances of outcomes, proficiency levels, and the ability to apply the formula
 * to simulate outcomes.

 @invariant
 * Resource Arrays should contain valid Strings and should not be empty
 * Quantity Arrays should contain Non-Negative Quantities and should not be empty
 * Proficiency Level should be Non-Negative and within valid range
*/

#include <stdexcept>
#include <algorithm>
#include <cstring>
#include <iostream>

#include "Formula.h"

namespace ResourceConversion
{
    /**
     * @brief Constructor for the Formula class, initializing member variables.
     *
     * @pre None.
     *
     * @post A new Formula object is created with member variables initialized as follows:
     *       - InputResourcesSize and InputQuantitiesSize are set to 0.
     *       - InputResources, InputQuantities, OutputResources, OutputQuantities, and ResultArray are set to nullptr.
     *       - ProficiencyLevel is set to 0.
     */
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

    /**
     * @brief Constructor for the Formula class, initializing member variables with provided values.
     *
     * @param InputResources_ An array of input resource names.
     * @param InputResourcesSize_ The size of the input resource array.
     * @param InputQuantities_ An array of input resource quantities.
     * @param InputQuantitiesSize_ The size of the input quantity array.
     * @param OutputResources_ An array of output resource names.
     * @param OutputResourcesSize_ The size of the output resource array.
     * @param OutputQuantities_ An array of output resource quantities.
     * @param OutputQuantitiesSize_ The size of the output quantity array.
     * @param ResultArray_ An array for storing calculation results.
     * @param ProficencyLevel_ The proficiency level of the formula (0 to 5).
     *
     * @pre - InputResources_ and OutputResources_ must not contain null, empty, or whitespace-only strings.
     *      - InputResourcesSize_ should match InputQuantitiesSize_.
     *      - OutputResourcesSize_ should match OutputQuantitiesSize_.
     *      - ProficencyLevel_ must not exceed 5.
     *
     * @post A new Formula object is created with member variables initialized as follows:
     *       - InputResources and InputQuantities are initialized based on the provided arrays.
     *       - OutputResources and OutputQuantities are initialized based on the provided arrays.
     *       - ResultArray is allocated based on the size of OutputQuantities.
     *       - ProficiencyLevel is set to the provided value.
     *
     * @throw std::invalid_argument if any preconditions are violated.
     */
    Formula::Formula (std::string* InputResources_, size_t InputResourcesSize_,
        unsigned int* InputQuantities_, size_t InputQuantitiesSize_,
        std::string* OutputResources_, size_t OutputResourcesSize_,
        unsigned int* OutputQuantities_, size_t OutputQuantitiesSize_,
        unsigned int* ResultArray_, unsigned int ProficencyLevel_)
    {
        if (ContainsNullOrWhiteSpace (InputResources_, InputResourcesSize_)) {
            throw std::invalid_argument ("[InputResources must not be null, empty, or contain only whitespace]");
        }

        if (ContainsNullOrWhiteSpace (OutputResources_, OutputResourcesSize_)) {
            throw std::invalid_argument ("[OutputResources must not be null, empty, or contain only whitespace]");
        }

        if (InputResourcesSize_ != InputQuantitiesSize_) {
            throw std::invalid_argument ("[lengths of [IN] -> Resources array doesn't match the [IN] -> Quantity array]");
        }

        if (OutputResourcesSize_ != OutputQuantitiesSize_) {
            throw std::invalid_argument ("[lengths of [OUT] -> Resources array doesn't match the [OUT] -> Quantity array]");
        }

        if (ProficencyLevel_ > 5) {
            throw std::invalid_argument ("[ProficiencyLevel must not exceed 5]");
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

    /**
     * @brief Constructor for the Formula class, initializing member variables with initializer lists.
     *
     * @param InputResources_ An initializer list of input resource names.
     * @param InputQuantities_ An initializer list of input resource quantities.
     * @param OutputResources_ An initializer list of output resource names.
     * @param OutputQuantities_ An initializer list of output resource quantities.
     * @param ResultArray_ An array for storing calculation results.
     * @param ProficiencyLevel_ The proficiency level of the formula (0 to 5).
     *
     * @pre None.
     *
     * @post A new Formula object is created with member variables initialized based on the provided initializer lists
     *       and values. The sizes of arrays are determined by the sizes of the initializer lists.
     *
     * @note The constructor is designed to accept initializer lists, making it convenient to initialize Formula objects.
     */
    Formula::Formula (std::initializer_list<std::string>& InputResources_,
        std::initializer_list<unsigned int>& InputQuantities_,
        std::initializer_list<std::string>& OutputResources_,
        std::initializer_list<unsigned int>& OutputQuantities_,
        unsigned int* ResultArray_, unsigned int ProficiencyLevel_)
    {

        InputResources = new std::string[InputResources_.size ()];
        InputResourcesSize = InputResources_.size ();
        for (size_t i = 0; i < InputResourcesSize; ++i) {
            InputResources[i] = *(InputResources_.begin () + i);
        }

        InputQuantities = new unsigned int[InputQuantities_.size ()];
        InputQuantitiesSize = InputQuantities_.size ();
        for (size_t i = 0; i < InputQuantitiesSize; ++i) {
            InputQuantities[i] = *(InputQuantities_.begin () + i);
        }

        OutputResources = new std::string[OutputResources_.size ()];
        OutputResourcesSize = OutputResources_.size ();
        for (size_t i = 0; i < OutputResourcesSize; ++i) {
            OutputResources[i] = *(OutputResources_.begin () + i);
        }

        OutputQuantities = new unsigned int[OutputQuantities_.size ()];
        OutputQuantitiesSize = OutputQuantities_.size ();
        for (size_t i = 0; i < OutputQuantitiesSize; ++i) {
            OutputQuantities[i] = *(OutputQuantities_.begin () + i);
        }

        ResultArray = new unsigned int[OutputQuantitiesSize];
        ProficiencyLevel = ProficiencyLevel_;
    }

    /**
     * @brief Destructor for the Formula class, cleaning up resources.
     *
     * @pre None.
     *
     * @post The Formula object is destroyed, and any dynamically allocated memory and resources are released.
     */
    Formula::~Formula () { ClearContainer (); }

    /**
     * @brief Copy constructor for the Formula class, creating a copy of another Formula object.
     *
     * @param other The Formula object to be copied.
     *
     * @pre None.
     *
     * @post A new Formula object is created as a deep copy of the provided 'other' Formula object.
     *       Both objects will have independent copies of data.
     */
    Formula::Formula (const Formula& other) { CopyData (other); }

    /**
     * @brief Copy assignment operator for the Formula class, assigning the data of another Formula object.
     *
     * @param other The Formula object to be assigned from.
     *
     * @pre None.
     *
     * @post The current Formula object is assigned the data (deep copy) from the 'other' Formula object.
     *       Both objects will have independent copies of data.
     *
     * @return A reference to the current Formula object after the copy assignment.
     */
    Formula& Formula::operator=(const Formula& other)
    {
        if (this == &other) { return *this; }
        ClearContainer ();
        CopyData (other);
        return *this;
    }

    /**
     * @brief Move constructor for the Formula class, transferring ownership of resources from another Formula object.
     *
     * @param other The Formula object to be moved.
     *
     * @pre None.
     *
     * @post A new Formula object is created by moving the data (shallow copy) from the 'other' Formula object.
     *       The 'other' object will be in a valid but unspecified state after the move.
     *
     * @note The move constructor allows for efficient transfer of resources without deep copying.
     */
    Formula::Formula (Formula&& other) noexcept
    {
        CopyData (other);
        ResetContainer (other);
    }

    /**
     * @brief Move assignment operator for the Formula class, transferring ownership of resources from another Formula object.
     *
     * @param other The Formula object to be moved from.
     *
     * @pre None.
     *
     * @post The current Formula object is assigned the data (shallow copy) from the 'other' Formula object.
     *       The 'other' object will be in a valid but unspecified state after the move.
     *
     * @return A reference to the current Formula object after the move assignment.
     *
     * @note The move assignment operator allows for efficient transfer of resources without deep copying.
     */
    Formula& Formula::operator=(Formula&& other)
    {
        if (this == &other) { return *this; }

        ClearContainer ();
        CopyData (other);
        ResetContainer (other);

        return *this;
    }

    /**
     * @brief Copies data from another Formula object to the current object.
     *
     * @param other The Formula object from which data is to be copied.
     *
     * @pre The 'other' Formula object must be valid and contain data.
     *
     * @post The data from the 'other' Formula object is copied to the current object.
     *       The current object's member variables are updated with deep copies of the 'other' object's data.
     */
    void Formula::CopyData (const Formula& other)
    {
        InputResourcesSize = other.InputResourcesSize;
        InputResources = new std::string[InputResourcesSize];
        for (size_t i = 0; i < InputResourcesSize; ++i)
        {
            InputResources[i] = other.InputResources[i];
        }

        InputQuantitiesSize = other.InputQuantitiesSize;
        InputQuantities = new unsigned int[InputQuantitiesSize];
        for (size_t i = 0; i < InputQuantitiesSize; ++i)
        {
            InputQuantities[i] = other.InputQuantities[i];
        }

        OutputResourcesSize = other.OutputResourcesSize;
        OutputResources = new std::string[OutputResourcesSize];
        for (size_t i = 0; i < OutputResourcesSize; ++i)
        {
            OutputResources[i] = other.OutputResources[i];
        }

        OutputQuantitiesSize = other.OutputQuantitiesSize;
        OutputQuantities = new unsigned int[OutputQuantitiesSize];
        for (size_t i = 0; i < OutputQuantitiesSize; ++i)
        {
            OutputQuantities[i] = other.OutputQuantities[i];
        }

        ResultArray = new unsigned int[OutputQuantitiesSize];
        for (size_t i = 0; i < OutputQuantitiesSize; ++i)
        {
            ResultArray[i] = other.ResultArray[i];
        }

        ProficiencyLevel = other.ProficiencyLevel;
    }

    /**
     * @brief Clears and deallocates memory used by member variables.
     *
     * @pre None.
     *
     * @post Any dynamically allocated memory and resources used by member variables are released.
     *       Member variables are set to nullptr or appropriate initial values.
     */
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

    /**
     * @brief Resets the member variables of another Formula object.
     *
     * @param other The Formula object whose member variables are to be reset.
     *
     * @pre The 'other' Formula object must be valid.
     *
     * @post The member variables of the 'other' Formula object are set to nullptr or appropriate initial values.
     */
    inline void Formula::ResetContainer (Formula& other)
    {
        other.InputResources = nullptr;
        other.OutputResources = nullptr;
        other.InputQuantities = nullptr;
        other.OutputQuantities = nullptr;
        other.ResultArray = nullptr;

        other.InputResourcesSize = 0;
        other.InputQuantitiesSize = 0;
        other.OutputResourcesSize = 0;
        other.OutputQuantitiesSize = 0;

        other.ProficiencyLevel = 0;
    }

    /**
     * @brief Calculates and returns outcome modifiers based on the proficiency level.
     *
     * @param Level The proficiency level of the formula (0 to 5).
     *
     * @pre The 'Level' parameter must be between 0 and 5.
     *
     * @post Outcome modifiers are calculated based on the proficiency level and returned as an OutcomeModifiers object.
     */
    inline Formula::OutcomeModifiers Formula::GetOutcomeChances (unsigned int Level)
    {
        const float DefaultFailureConst = 0.25;
        const float DefaultPartialConst = 0.2;
        const float DefaultBonusConst = 0.05;
        const float DefaultNormalConst = 0.5;

        OutcomeModifiers InitialValues (DefaultFailureConst,
            DefaultPartialConst,
            DefaultBonusConst,
            DefaultNormalConst);

        if (Level == 0)
        {
            return InitialValues;
        }

        float ModifiedFailure = InitialValues.Failure - (Level * 0.05);
        float ModifiedPartial = InitialValues.Partial - (Level * 0.05);
        float ModifiedBonus = InitialValues.Bonus + (Level * 0.05);
        float ModifiedNormal = InitialValues.Normal + (Level * 0.05);

        return OutcomeModifiers (ModifiedFailure,
            ModifiedPartial,
            ModifiedBonus,
            ModifiedNormal);
    }

    /**
     * @brief Checks if an array of strings contains null, empty, or whitespace strings.
     *
     * @param Array The array of strings to be checked.
     * @param ArraySize The size of the array.
     *
     * @pre The 'Array' parameter must point to a valid array of strings, and 'ArraySize' must be a non-negative value.
     *
     * @post Returns 'true' if the array contains any null, empty, or whitespace strings; otherwise, returns 'false'.
     *       The function does not modify the input array.
     *
     * @return 'true' if the array contains null, empty, or whitespace strings; 'false' otherwise.
     */
    inline bool Formula::ContainsNullOrWhiteSpace (std::string* Array, size_t ArraySize)
    {
        for (size_t i = 0; i < ArraySize; ++i)
        {
            if (Array[i].empty () || std::all_of (Array[i].begin (), Array[i].end (), ::isspace))
                return true;
        }
        return false;
    }

    /**
     * @brief Applies the formula and computes the outcome based on proficiency level and chance modifiers.
     *
     * @pre The Formula object must be properly initialized with valid data and proficiency level.
     *
     * @post The formula is applied, and the outcome is computed according to the proficiency level and chance modifiers.
     *       The 'ResultArray' member variable is updated with the computed outcome.
     *       The function does not modify other member variables or external state.
     *
     * @bug the Result array is 0 --> teh OutcomeQuantities is 0 for some reason
     */
    void Formula::Apply ()
    {
        OutcomeModifiers OutcomeChances = GetOutcomeChances (ProficiencyLevel);

        float ChanceOfFailure = OutcomeChances.Failure;
        float ChanceOfPartial = OutcomeChances.Partial;
        float ChanceOfBonus = OutcomeChances.Bonus;
        float ChanceOfNormal = OutcomeChances.Normal;

        const unsigned int UpperBound = 1;

        const unsigned int FailedArraySize = 1;
        const unsigned int FailedValue = 0;

        auto GetRandomFloat = [this]() {
            std::uniform_real_distribution<float> Distribution (0.0, 1.0);
            float RandomValue = Distribution (RandomNumberGeneratorSeed);
            return std::round (RandomValue * 100.0) / 100.0; // Round to two decimal places
            };

        const float BonusConstModifier = 1.1;
        const float PartialConstModifier = 0.75;

        float RandomValue = GetRandomFloat ();

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
                ResultArray[i] = static_cast<unsigned int>(std::floor (OutputQuantities[i] * PartialConstModifier));
            }
        }

        //Bonus Out -> 0.05
        if (RandomValue > (ChanceOfPartial + ChanceOfFailure) &&
            RandomValue < ChanceOfNormal)
        {
            for (size_t i = 0; i < OutputQuantitiesSize; ++i)
            {
                ResultArray[i] = static_cast<unsigned int>(std::ceil (OutputQuantities[i] * BonusConstModifier));
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
    }
}
