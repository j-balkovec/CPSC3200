/// <file> Formula.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Wed 20th Sep </date>
/// 
/// <revision>
/// Revision History 
/// - 1.0 [09/20/2023]: Initial skeleton
/// - 2.0 [09/22/2023]: Helper Methods
/// - 3.0 [09/23/2023]: Apply Method, Refine Documentation
/// - 4.0 [09/24/2023]: Fix Apply Method
/// </revision>
///
/// <summary>
/// * This file contains the definition of the Formula class, which represents 
/// * a way of converting resources into other resources. The Formula class 
/// * encapsulates information about input resources, output resources, 
/// * chances of outcomes, proficiency levels, and the ability to apply the formula 
/// * to simulate outcomes.
/// </summary>
///
/// <invariant>
/// * Resource Arrays should contain valid Strings and should not be empty
/// * Quantity Arrays should contain Non-Negative Quantities and should not be empty
/// * Proficiency Level should be Non-Negative and within valid range
/// </invariant>
/// 
/// <dependencies> This class does not have any external dependencies </dependencies>

namespace ResourceConversion
{
    public class Formula
    {
        private readonly string[] InputResources; //nullable?
        private readonly uint[] InputQuantities; //nullable

        private readonly string[] OutputResources; //nullable -> readonly
        private readonly uint[] OutputQuantities; //nullable -> readonly

        private uint ProficiencyLevel = 0;

        private readonly Random RandomGenNumber = new Random();

        /// <summary>
        /// * Represents modifiers for different outcome types based on
        ///   proficiency level.
        /// </summary>
        private struct OutcomeModifiers
        {
            /// <summary>
            /// * Initializes a new instance of the OutcomeModifiers struct.
            /// </summary>
            /// 
            /// <param name="failure">The modifier for the failure outcome.</param>
            /// <param name="partial">The modifier for the partial outcome.</param>
            /// <param name="bonus">The modifier for the bonus outcome.</param>
            /// <param name="normal">The modifier for the normal outcome.</param>
            public OutcomeModifiers(double failure, double partial, double bonus, double normal)
            {
                Failure = failure;
                Partial = partial;
                Bonus = bonus;
                Normal = normal;
            }

            public double Failure;
            public double Partial;
            public double Bonus;
            public double Normal;
        }

        /// <summary>
        /// * Initializes a new instance of the Formula class with specified input and output parameters.
        /// </summary>
        /// 
        /// <param name="InputResources">An array of input resource names.</param>
        /// <param name="InputQuantities">An array of input resource quantities.</param>
        /// <param name="OutputResources">An array of output resource names.</param>
        /// <param name="OutputQuantities">An array of output resource quantities.</param>
        /// <param name="ProficiencyLevel">The proficiency level of the formula.</param>
        /// 
        /// <remarks>
        /// * Initializes a new instance of the Formula class with the specified input and output parameters.
        ///
        /// <precondition>
        /// * The InputResources, InputQuantities, OutputResources, OutputQuantities arrays must not be null.
        /// * The lengths of InputResources, InputQuantities, OutputResources, OutputQuantities arrays must be the same.
        /// * InputQuantities and OutputQuantities must contain non-negative quantities (uint values).
        /// * ProficiencyLevel should be a non-negative integer.
        /// </precondition>
        /// 
        /// <postcondition>
        /// Postconditions:
        /// * The Formula object is successfully instantiated.
        /// * The InputResources, InputQuantities, OutputResources, OutputQuantities,
        ///   and ProficiencyLevel fields are assigned their respective values.
        /// * The Formula object is ready for use in applying the formula to simulate outcomes.
        /// </postcondition>
        /// 
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// * InputResources is null, empty, or contains only whitespace.
        /// * OutputResources is null, empty, or contains only whitespace.
        /// * ProficiencyLevel exceeds the maximum allowed value of 4.
        /// </exception>
        /// </remarks>
        public Formula(string[] InputResources,
                       uint[] InputQuantities,
                       string[] OutputResources,
                       uint[] OutputQuantities,
                       uint ProficiencyLevel)
        {
            if (InputResources.Any(string.IsNullOrEmpty) || InputResources.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("[InputResources must not be null, empty, or contain only whitespace]");
            }

            if (OutputResources.Any(string.IsNullOrEmpty) || OutputResources.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("[OutputResources must not be null, empty, or contain only whitespace]");
            }

            if (InputResources.Length != InputQuantities.Length)
            {
                throw new ArgumentException(
                                            $"[lengths of [IN] -> Resources array doesn't match " +
                                            $"the [IN] -> Quantity array]"
                                            );
            }

            if (OutputResources.Length != OutputQuantities.Length)
            {
                throw new ArgumentException(
                                            $"[lengths of [OUT] -> Resources array doesn't match " +
                                            $"the [OUT] -> Quantity array]"
                                            );
            }

            if (ProficiencyLevel > 5) //capped at 5
            {
                throw new ArgumentException("[ProficiencyLevel must not exceed 5]");

            }

            this.InputResources = InputResources;
            this.InputQuantities = InputQuantities;
            this.OutputResources = OutputResources;
            this.OutputQuantities = OutputQuantities;
            this.ProficiencyLevel = ProficiencyLevel;
        }

        /// <summary>
        /// * Retrieves the outcome modifiers (chances) based on the proficiency level.
        /// </summary>
        /// 
        /// <param name="Level">The proficiency level, which affects outcome chances.</param>
        /// 
        /// <returns>An OutcomeModifiers struct containing the modified outcome chances.</returns>
        /// 
        /// <remarks>
        /// <precondition>
        /// * The <paramref name="Level"/> should be a non-negative integer.
        /// </precondition>
        /// 
        /// <postcondition>
        /// * The returned OutcomeModifiers struct contains the modified outcome chances
        /// </postcondition>
        /// </remarks>
        private OutcomeModifiers GetOutcomeChances(uint Level)
        {
            const double DefaultFailureConst = 0.25;
            const double DefaultPartialConst = 0.2;
            const double DefaultBonusConst = 0.05;
            const double DefaultNormalConst = 0.5;

            OutcomeModifiers InitialValues = new OutcomeModifiers(DefaultFailureConst,
                                                            DefaultPartialConst,
                                                            DefaultBonusConst,
                                                            DefaultNormalConst);

            if (Level == 0)
            {
                return InitialValues;
            }
            else
            {
                double modifiedFailure = InitialValues.Failure - (Level * 0.05);
                double modifiedPartial = InitialValues.Partial - (Level * 0.05);
                double modifiedBonus = InitialValues.Bonus + (Level * 0.05);
                double modifiedNormal = InitialValues.Normal + (Level * 0.05);

                return new OutcomeModifiers(modifiedFailure,
                                            modifiedPartial,
                                            modifiedBonus,
                                            modifiedNormal);
            }
        }

        ///<summary>
        /// * Applies the formula to simulate resource conversion outcomes based on the proficiency level.
        ///</summary>
        ///
        ///<returns>
        /// * An array of output resource quantities representing the result of applying the formula.
        ///</returns>
        ///
        ///<remarks>
        /// * This method calculates the output quantities of resources based on the proficiency level
        ///   and random chance factors. The outcome may vary depending on the proficiency level set
        ///   for the Formula instance.
        ///
        /// <precondition>
        /// * The ProficiencyLevel should be a non-negative integer.
        /// * The OutputQuantities array should not be null and should have the same length as the
        ///   outputResources array.
        /// </precondition>
        ///
        /// <postcondition>
        /// * The returned array should not be null.
        /// * The length of the returned array should match the number of output resources defined
        ///   in the formula.
        /// </postcondition>
        ///</remarks>
        ///
        /// <bug>
        /// The Ranges contain bugs
        /// </bug>
        public uint[] Apply()
        {
            OutcomeModifiers OutcomeChances = GetOutcomeChances(ProficiencyLevel);
            double ChanceOfFailure = OutcomeChances.Failure;
            double ChanceOfPartial = OutcomeChances.Partial;
            double ChanceOfBonus = OutcomeChances.Bonus;
            double ChanceOfNormal = OutcomeChances.Normal;

            const uint UpperBound = 1;
            double RandomValue = RandomGenNumber.NextDouble();

            const double BonusConstModifier = 1.1;
            const double PartialConstModifier = 0.75;

            switch(RandomValue)
            {
                //Fail Out -> 0.25
                case double value when (value < ChanceOfFailure):
                {
                    return new uint[0];
                }

                //Partial Out -> 0.2
                case double value when (value > ChanceOfFailure && value < (ChanceOfPartial + ChanceOfFailure)):
                    {
                        uint[] RoundedPartialOutputQuantities = new uint[OutputQuantities.Length];
                        for (uint i = 0; i < OutputQuantities.Length; i++)
                        {
                            RoundedPartialOutputQuantities[i] = (uint)Math.Floor(OutputQuantities[i] * PartialConstModifier);
                        }
                        return RoundedPartialOutputQuantities;
                    }

                //Bonus Out -> 0.05
                case double value when (value > (ChanceOfPartial + ChanceOfFailure) &&  value < ChanceOfNormal):
                {
                        uint[] RoundedBonusOutputQuantities = new uint[OutputQuantities.Length];
                        for (uint i = 0; i < OutputQuantities.Length; i++)
                        {
                            RoundedBonusOutputQuantities[i] = (uint)Math.Ceiling(OutputQuantities[i] * BonusConstModifier);
                        }
                        return RoundedBonusOutputQuantities;
                }

                //Normal Out -> 0.5
                case double value when (value > (ChanceOfFailure + ChanceOfBonus + ChanceOfPartial) && value < (double)UpperBound):
                {
                        return OutputQuantities;
                } 
            }
            return new uint[0];
        }
    }
}
