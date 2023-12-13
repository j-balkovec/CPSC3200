/// <file> Formula.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Wed 11th Oct </date>
/// 
/// <revision>
/// Revision History
/// - [1.0 - 4.0]: See P1
/// - [5.0]: Refine Function Decomposition
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

using System;

namespace ResourceConversion
{
    public class Formula
    {
        /// <summary>
        /// - If set to true, it produces console output
        /// </summary>
        private readonly bool DisplayValues = true;
        //! Logs

        private readonly Random RandomGenNumber = new Random();

        private readonly string[] InputResources;
        private readonly uint[] InputQuantities;
        private readonly string[] OutputResources;
        private readonly uint[] OutputQuantities;
        private uint[] ResultArray;
        private uint ProficiencyLevel = 0;

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
        public Formula(string[] InputResources_,
                       uint[] InputQuantities_,
                       string[] OutputResources_,
                       uint[] OutputQuantities_,
                       uint[] ResultArray_,
                       uint ProficiencyLevel_ = 0)
        {
            if (InputResources_.Any(resource => string.IsNullOrEmpty(resource) || string.IsNullOrWhiteSpace(resource)))
            {
                throw new ArgumentException("Raise Formula(...) => [InputResources must not be null, empty, or contain only whitespace]");
            }

            if (OutputResources_.Any(string.IsNullOrEmpty) || OutputResources_.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Raise Formula(...) =>[OutputResources must not be null, empty, or contain only whitespace]");
            }

            if (InputResources_.Length != InputQuantities_.Length)
            {
                throw new ArgumentException(
                                            $"Raise Formula(...) => [lengths of [IN] -> Resources array doesn't match " +
                                            $"the [IN] -> Quantity array]"
                                            );
            }

            if (OutputResources_.Length != OutputQuantities_.Length)
            {
                throw new ArgumentException(
                                            $"Raise Formula(...) => [lengths of [OUT] -> Resources array doesn't match " +
                                            $"the [OUT] -> Quantity array]"
                                            );
            }

            if (ProficiencyLevel_ > 5) //capped at 5
            {
                throw new ArgumentException("Raise Formula(...) => [ProficiencyLevel must not exceed 5]");

            }

            this.InputResources = InputResources_;
            this.InputQuantities = InputQuantities_;
            this.OutputResources = OutputResources_;
            this.OutputQuantities = OutputQuantities_;
            this.ResultArray = ResultArray_;
            this.ProficiencyLevel = ProficiencyLevel_;
        }

        /// <summary>
        /// * Gets the result array representing the outcome of applying the formula.
        /// </summary>
        /// 
        /// <remarks>
        /// * This property provides access to the result array, which contains the output resource quantities
        ///   after applying the formula. The array represents the outcome of the formula's simulation.
        /// </remarks>
        /// 
        /// <precondition>
        /// * None
        /// </precondition>
        /// 
        /// <postcondition>
        /// * The returned array should not be null.
        /// * The array should match the number of output resources defined in the formula.
        /// </postcondition>
        public uint[] GetResultArray { get {return ResultArray; } }

        /// <summary>
        /// * Creates a deep clone of the Formula instance.
        /// </summary>
        /// 
        /// <returns>
        /// * A deep clone of the Formula instance with copied arrays and properties.
        /// </returns>
        /// 
        /// <remarks>
        /// * This method creates a new Formula instance with deep copies of its arrays and properties.
        /// * The cloned Formula is an independent instance that is separate from the original Formula.
        /// </remarks>
        /// 
        /// <precondition>
        /// * None
        /// </precondition>
        /// 
        /// <postcondition>
        /// * The returned Formula instance is a deep copy of the original Formula.
        /// * The arrays and properties of the cloned Formula are distinct from the original.
        /// </postcondition>
        public Formula Clone()
        {
            string[] InputResourcesClone = new string[InputResources.Length];
            for (int i = 0; i < InputResources.Length; i++)
            {
                InputResourcesClone[i] = InputResources[i];
            }

            uint[] InputQuantitiesClone = new uint[InputQuantities.Length];
            for (int i = 0; i < InputQuantities.Length; i++)
            {
                InputQuantitiesClone[i] = InputQuantities[i];
            }

            string[] OutputResourcesClone = new string[OutputResources.Length];
            for (int i = 0; i < OutputResources.Length; i++)
            {
                OutputResourcesClone[i] = OutputResources[i];
            }

            uint[] OutputQuantitiesClone = new uint[OutputQuantities.Length];
            for (int i = 0; i < OutputQuantities.Length; i++)
            {
                OutputQuantitiesClone[i] = OutputQuantities[i];
            }

            uint[] ResultArrayClone = new uint[ResultArray.Length];
            for (int i = 0; i < ResultArray.Length; i++)
            {
                ResultArrayClone[i] = ResultArray[i];
            }

            uint ProficiencyLevelClone = ProficiencyLevel;

            Formula clone = new Formula(InputResourcesClone,
                                        InputQuantitiesClone,
                                        OutputResourcesClone,
                                        OutputQuantitiesClone,
                                        ResultArrayClone,
                                        ProficiencyLevelClone);

            return clone;
        }

        /// <summary>
        /// * Determines whether the current Formula instance is equal to another Formula instance.
        /// </summary>
        /// 
        /// <param name="obj">
        /// * The Formula object to compare with the current instance.
        /// </param>
        /// 
        /// <returns>
        /// <c>true</c> if the two instances are considered equal; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <remarks>
        /// * Two Formula instances are considered equal if their properties, including InputResources,
        /// * InputQuantities, OutputResources, OutputQuantities, ResultArray, and ProficiencyLevel, match. This method provides a deep equality check.
        /// </remarks>
        /// 
        /// <param name="obj">
        /// * The Formula object to compare with the current instance.
        /// </param>
        /// 
        /// <returns>
        /// <c>true</c> if the two instances are considered equal; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Formula obj)
        {
            if (obj is Formula otherFormula)
            {
                if (ReferenceEquals(this, otherFormula)) { return true; }

                return InputResources.SequenceEqual(otherFormula.InputResources) &&
                       InputQuantities.SequenceEqual(otherFormula.InputQuantities) &&
                       OutputResources.SequenceEqual(otherFormula.OutputResources) &&
                       OutputQuantities.SequenceEqual(otherFormula.OutputQuantities) &&
                       ResultArray.SequenceEqual(otherFormula.ResultArray) &&
                       ProficiencyLevel == otherFormula.ProficiencyLevel;
            }

            return false;
        }

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
        private static OutcomeModifiers GetOutcomeChances(uint Level)
        {
            if(Level < 0 || Level > 6)
            {
                throw new InvalidDataException("Raise GetOutcomeChances(...) => [Level has to be 0 <-> 5]");
            }
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
                double modifiedFailure = InitialValues.Failure - (Level * 0.05);
                double modifiedPartial = InitialValues.Partial - (Level * 0.05);
                double modifiedBonus = InitialValues.Bonus + (Level * 0.05);
                double modifiedNormal = InitialValues.Normal + (Level * 0.05);

                return new OutcomeModifiers(modifiedFailure,
                                            modifiedPartial,
                                            modifiedBonus,
                                            modifiedNormal);
        }

        /// <summary>
        /// * Displays the input and output values for a resource conversion.
        /// </summary>
        /// 
        /// <param name="InputResources">An array of input resource names.</param>
        /// <param name="InputQuantities">An array of input resource quantities.</param>
        /// <param name="OutputResources">An array of output resource names.</param>
        /// <param name="OutputQuantities">An array of output resource quantities.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// * Thrown if the lengths of InputResources and InputQuantities arrays do not match.
        /// </exception>
        /// 
        /// <remarks>
        /// <precondition>
        /// * The InputResources and InputQuantities arrays must not be null.
        /// * The lengths of InputResources and InputQuantities arrays must match.
        /// </precondition>
        /// 
        /// <postcondition>
        /// * If the precondition is met, the method displays the input and output values.
        /// </postcondition>
        /// </remarks>
        public void DisplayResources()
        {
            if (DisplayValues)
            {
                if (InputResources.Length != InputQuantities.Length)
                {
                    throw new ArgumentException(
                                                $"[lengths of [IN] -> Resources array doesn't match " +
                                                $"the [IN] -> Quantity array]"
                                                );
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" <[ ");
                Console.ResetColor();
                for (ushort i = 0; i < InputResources.Length; i++)
                {
                    Console.Write($"{InputResources[i]}: {InputQuantities[i]} ");
                }

                Console.Write("] ---> [");

                for (ushort i = 0; i < OutputResources.Length; i++)
                {
                    Console.Write($"{OutputResources[i]}: {OutputQuantities[i]} ");
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("]>\n");
                Console.ResetColor();
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void DisplayFormulaOutput()
        {
            if (DisplayValues)
            {
                if (InputResources.Length != InputQuantities.Length)
                {
                    throw new ArgumentException(
                                                $"[lengths of [IN] -> Resources array doesn't match " +
                                                $"the [IN] -> Quantity array]"
                                                );
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" <[ ");
                Console.ResetColor();
                for (ushort i = 0; i < InputResources.Length; i++)
                {
                    Console.Write($"{InputResources[i]}: {InputQuantities[i]} ");
                }
                Console.Write("] ---> [");


                if (ResultArray.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAILED");
                    Console.ResetColor();
                    Console.Write("]>\n");

                }
                else
                {
                    for (ushort i = 0; i < OutputResources.Length; i++)
                    {
                        Console.Write($"{OutputResources[i]}: {ResultArray[i]} ");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("]>\n");
                    Console.ResetColor();
                }
            }
            return;
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
        public void Apply()
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
                     ResultArray = new uint[0];
                     break;
                }

                //Partial Out -> 0.2
                case double value when (value > ChanceOfFailure && value < (ChanceOfPartial + ChanceOfFailure)):
                {
                        uint[] RoundedPartialOutputQuantities = new uint[OutputQuantities.Length];
                        for (uint i = 0; i < OutputQuantities.Length; i++)
                        {
                            RoundedPartialOutputQuantities[i] = (uint)Math.Floor(OutputQuantities[i] * PartialConstModifier);
                        }
                        ResultArray = RoundedPartialOutputQuantities;
                     break;
                }

                //Bonus Out -> 0.05
                case double value when (value > (ChanceOfPartial + ChanceOfFailure) &&  value < ChanceOfNormal):
                {
                        uint[] RoundedBonusOutputQuantities = new uint[OutputQuantities.Length];
                        for (uint i = 0; i < OutputQuantities.Length; i++)
                        {
                            RoundedBonusOutputQuantities[i] = (uint)Math.Ceiling(OutputQuantities[i] * BonusConstModifier);
                        }
                        ResultArray = RoundedBonusOutputQuantities;
                     break;
                }

                //Normal Out -> 0.5
                case double value when (value > (ChanceOfFailure + ChanceOfBonus + ChanceOfPartial) && value < (double)UpperBound):
                {
                        ResultArray = OutputQuantities;
                    break;
                }
                default:
                    throw new InvalidProgramException("Raise: Apply() => [Switch reached 'default' -> Random failed]");
            }
        }
    }
}
