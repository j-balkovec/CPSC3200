/// <file> IStockpile.cs </file>
/// <brief>
/// [DESC]: Defines the IStockpile interface for resource conversion.
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
    /// Represents the functionality of a stockpile for managing resources.
    /// </summary>
    public interface IStockpile
    {
        /// <summary>
        /// Restocks the stockpile.
        /// </summary>
        void Restock();

        /// <summary>
        /// Increases the quantity of a specific resource in the stockpile.
        /// </summary>
        void IncreaseQuantity(string NameOfResource, uint NewIncreasedQuantity);

        /// <summary>
        /// Decreases the quantity of a specific resource in the stockpile.
        /// </summary>>
        void DecreaseQuantity(string NameOfResource, uint NewDecreasedQuantity);
    }
}


