/// <file> IPlan.cs </file>
/// <brief>
/// [DESC]: Defines the IPlan interface for resource conversion.
/// </brief>
/// 
/// <author>Jakob Balkovec (CPSC 3200)</author>
/// <instructor>A. Dingle (CPSC 3200)</instructor>
/// <date> 18th Nov </date>
/// 
/// <version>
///     Revision History
///     - 1.0 [18/11/2023]: Initial definition of the interface and the methods
/// </version>
/// 
/// <dependencies>
///     <none>This interface has no dependencies</none>
/// </dependencies>
/// 
/// <namespace>
///     Might be deemed unnecessary, but despite the distinctive function names, global
///     namespace pollution is still in the picture.
///     - {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan' class.
/// </namespace>

namespace ResourceConversion
{
    /// <summary>
    /// Represents the IPLan interface for resource conversion.
    /// </summary>
    public interface IPlan
    {
        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /*object Clone();*/

        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object? DeepCopy();

        /// <summary>
        /// Creates a shallow copy of the object.
        /// </summary>
        /// <returns>A new object that is a shallow copy of this instance.</returns>
        object? ShallowCopy();

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        bool Equals(object? obj);

        /// <summary>
        /// Adds a formula to the IPLan.
        /// </summary>
        void AddFormula(in Formula NewFormula);

        /// <summary>
        /// Removes the last formula from the IPLan.
        /// </summary>
        void RemoveLastFormula();

        /// <summary>
        /// Replaces the existing formula in the IPLan.
        /// </summary>
        void ReplaceFormula(Formula NewFormula, ushort Index);

        /// <summary>
        /// Applies the formulas in the IPLan.
        /// </summary>
        void Apply();
    }
}

