/**
 @file Formula.h
 @author Jakob Balkovec (CPSC 3200)
 @instructor A. Dingle (CPSC 3200)

 @date Wed 20th Sep

 @revision
 Revision History
 - 1.0 [09/30/2023]: C# -> C++
 - 2.0 [01/10/2023]: Scrap and start over
 - 3.0 [02/10/2023]: Simplifed ArrayWrappers, handled memory, documentation

 @brief
 * This file contains the definition of the Formula class, which represents
 * a way of converting resources into other resources. The Formula class
 * encapsulates information about input resources, output resources,
 * chances of outcomes, proficiency levels, and the ability to apply the formula
 * to simulate outcomes.

 @invariant
 * Resource Arrays should contain valid Strings and should not be empty
 * Quantity Arrays should contain Non-Negative Quantities and should not be empty
 * Proficiency Level should be Non-Negative and within valid range

 @dependencies This class does not have any external dependencies
*/

#ifndef Formula_h
#define Formula_h

#include <string>
#include <random>

namespace ResourceConversion
{
    class Formula
    {
    private:
        std::string* InputResources;
        size_t InputResourcesSize;

        unsigned int* InputQuantities;
        size_t InputQuantitiesSize;

        std::string* OutputResources;
        size_t OutputResourcesSize;

        unsigned int* OutputQuantities;
        size_t OutputQuantitiesSize;

        unsigned int* ResultArray;

        unsigned int ProficiencyLevel = 0;
        std::mt19937 RandomNumberGeneratorSeed;



        struct OutcomeModifiers
        {
            float Failure;
            float Partial;
            float Bonus;
            float Normal;

            /**
             * @brief Constructor for OutcomeModifiers.
             * @param Failure_ The failure modifier.
             * @param Partial_ The partial modifier.
             * @param Bonus_ The bonus modifier.
             * @param Normal_ The normal modifier.
             */
            OutcomeModifiers (float Failure_, float Partial_, float Bonus_, float Normal_)
                : Failure (Failure_),
                Partial (Partial_),
                Bonus (Bonus_),
                Normal (Normal_) {}

        };

        inline OutcomeModifiers GetOutcomeChances (unsigned int Level);
        inline void CopyData (const Formula& other);
        inline void ClearContainer ();
        inline void ResetContainer (Formula& other);

    public:

        Formula ();
        Formula (std::string* InputResources_, size_t InputResourcesSize_,
            unsigned int* InputQuantities_, size_t InputQuantitiesSize_,
            std::string* OutputResources_, size_t OutputResourcesSize_,
            unsigned int* OutputQuantities_, size_t OutputQuantitiesSize_,
            unsigned int* ResultArray_, unsigned int ProficiencyLevel_ = 0);

        Formula (std::initializer_list<std::string>& InputResources_,
            std::initializer_list<unsigned int>& InputQuantities_,
            std::initializer_list<std::string>& OutputResources_,
            std::initializer_list<unsigned int>& OutputQuantities_,
            unsigned int* ResultArray, unsigned int ProficiencyLevel_ = 0);

        ~Formula ();

        Formula (const Formula& other);
        Formula& operator=(const Formula& other);

        Formula (Formula&& other) noexcept;
        Formula& operator=(Formula&& other);

        inline bool ContainsNullOrWhiteSpace (std::string* Array, size_t ArraySize);
        void Apply ();
        inline unsigned int* GetResultArray () { return ResultArray; }
    };
}
#endif /* Formula_h */
