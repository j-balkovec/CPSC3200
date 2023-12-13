/// <file> Stockpile.cs </file>
/// 
/// <summary>
/// * Represents the 'Stockpile' class, a derived class introducing the
///   concept of encapsulating a set of resources and related quantities.
/// </summary>
/// 
/// <remarks>
/// * The 'Stockpile' class offers limited functionality with methods to increase and
///   decrease quantities. It does not support copying but utilizes move semantics
///   for optimal performance, especially in the context of expensive Linq containers {[SEE]: [MOVE SEMANTICS]}.
/// </remarks>
/// 
/// <author>Jakob Balkovec (CPSC 3200)</author>
/// <instructor>A. Dingle (CPSC 3200)</instructor>
/// <date>Tue 31th Oct</date>
/// 
/// <version>
/// Revision History:
///   - 1.0 [10/31/23] - Initial class design
///   - 2.0 [10/31/23] - Documentation and Invariants
/// </version>
/// 
/// <invariant>
///  * The 'ResourcesMap' member variable is a non-null std::unordered_map containing resource names (keys) and their corresponding non-negative integer quantities (values).
///  * When the class is constructed or initialized, the 'ResourcesMap' contains valid resource name-quantity pairs.
///  * The ResourcesMap is modified only through the IncreaseQuantity and DecreaseQuantity member functions, ensuring that resource quantities remain non-negative.
///  * 'NewQuantities' should always be non-negative or the size_t(unsigned long) will overflow.
///  * The IncreaseQuantity and DecreaseQuantity member functions do not introduce any resource names that are not present in the ResourcesMap.
///  * After any operation (construction, move, etc.), the 'ResourcesMap' remains in a consistent and valid state.
///  * CompletedArray is never null.
/// </invariant>
/// 
/// <seealso cref="std::unordered_map">[SEE: std::unordered_map]</seealso>
/// <dependencies>
///   <internal>
///     - Member, Helper, Utility methods
///     - Private Data Members
///   </internal>
///   <external>
///     - 'std::unordered_map' - {SEE [<unordered_map>]}
///   </external>
/// </dependencies>
/// 
/// <namespace>
///   <description>
///   * Might be deemed unnecessary, but despite the distinctive function names,
///     global namespace pollution is still in the picture.
///   </description>
///   <name>{ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan', and 'Stockpile' class.</name>
/// </namespace>
using System.Collections.Concurrent;

namespace ResourceConversion
{
    public class Stockpile : IStockpile
    {
        private ConcurrentDictionary<string, uint>? ResourcesDictionary;
        private ConcurrentDictionary<string, uint>? BackupDictionary;
        /// <summary>
        /// * Initializes a new instance of the <see cref="Stockpile"/>
        ///   class with the specified <paramref name="ResourcesDictionary"/>.
        /// </summary>
        /// 
        /// <param name="ResourcesDictionary">A <see cref="ConcurrentDictionary{TKey, TValue}"/>
        ///     containing resource names (keys) and their corresponding non-negative integer quantities (values).</param>
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="ResourcesDictionary"/> is <c>null</c>.
        /// </exception>
        /// 
        /// <remarks>
        /// * The <paramref name="ResourcesDictionary"/> should not be <c>null</c>,
        ///   and it is expected to contain valid resource name-quantity pairs.
        /// </remarks>
        public Stockpile(ConcurrentDictionary<string, uint> ResourcesDictionary)
        {
            if (ResourcesDictionary == null)
            {
                throw new ArgumentNullException(nameof(ResourcesDictionary), "ResourcesDictionary cannot be null");
            }

            this.ResourcesDictionary = ResourcesDictionary;
            this.BackupDictionary = ResourcesDictionary;
        }

        /// <summary>
        /// - Creates a deep copy of the Stockpile instance, including deep copies of its data.
        /// </summary>
        /// 
        /// <returns>
        /// - A deep copy of the Stockpile instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The Clone method creates a deep copy of the current Stockpile instance, including creating
        ///   deep copies of all its associated data. This means that all data within the
        ///   new Plan instance are independent copies of the original, preserving the state
        ///   of each formula without any reference to the original Stockpile instance.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - The Stockpile instance must be properly initialized before invoking this method.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A new Stockpile instance is created as a deep copy of the current instance and its formulas.
        /// </post>
        ///
        /// <attention> Client has to cast the object to the appropriaet type </attention>
        private Stockpile Clone()
        {
            ConcurrentDictionary<string, uint> ClonedResourcesDictionary = new ConcurrentDictionary<string, uint>();

            foreach(var Kvp in this.ResourcesDictionary)
            {
                ClonedResourcesDictionary.TryAdd(Kvp.Key, Kvp.Value);
            }

            return new Stockpile(ClonedResourcesDictionary);
        }

        /// <summary>
        /// - Creates a deep copy of the Stockpile instance, including deep copies of its formulas.
        /// </summary>
        /// 
        /// <returns>
        /// - A deep copy of the Plan instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The DeepCopy method is used to create a deep copy of the current Stockpile instance,
        ///   including deep copies of all its associated data. The deep copy ensures that
        ///   the new Stockpile instance is entirely independent and self-contained, preserving the state of
        ///   each formula without any reference to the original Stockpile instance.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - The current Stockpile instance (this) cannot be null.
        /// - The Clone method must be available for invocation to create deep copies of the formulas.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A deep copy of the Stockpile instance, along with its associated formulas, is successfully created.
        /// </post>
        ///
        /// <attention>
        /// - Declared Nullable
        /// - Client has to cast the object to the appropriaet type
        /// </attention>
        public object DeepCopy() { return this.Clone() as Stockpile; }

        /// <summary>
        /// - Creates a shallow copy of the Stockpile instance. The data is not deep-copied.
        /// </summary>
        /// 
        /// <returns>
        /// - A shallow copy of the Stockpile instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The ShallowCopy method is used to create a shallow copy of the current Stockpile instance.
        /// - Unlike a deep copy, the data within the Stockpile is not deep-copied, and both the original
        ///   and copied Stockpile instances will share references to the same data.
        /// - This means changes to the data in the copied Stockpile will affect the original and vice versa.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - The current Stockpile instance (this) cannot be null.
        /// - The `MemberwiseClone` method must be available for invocation to create shallow copies.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A shallow copy of the Stockpile instance is successfully created.
        /// </post>
        ///
        /// <attention>
        /// - Declared Nullable
        /// - Client has to cast the object to the appropriaet type
        /// </attention>
        public object ShallowCopy() { return this.MemberwiseClone() as Stockpile; }

        /// <summary>
        /// * Clears the resources in the <see cref="ResourcesDictionary"/>
        ///   and sets it to its default value, effectively zeroing out the
        ///   <see cref="Stockpile"/> object.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// * The method clears all resources from the <see cref="ResourcesDictionary"/>
        ///   and sets it to its default value, which is typically <c>null</c> for reference types.
        /// </para>
        /// <para>
        /// * After calling this method, the <see cref="Stockpile"/> object will be in
        ///   a state similar to its initial state, with an empty <see cref="ResourcesDictionary"/>.
        /// </para>
        /// </remarks>
        public void ZeroOutObject()
        {
            ResourcesDictionary.Clear();
            BackupDictionary.Clear();

            BackupDictionary = default;
            ResourcesDictionary = default;
        }

        /// <summary>
        /// * Increases the quantity of a specified resource in the <see cref="Stockpile"/> object.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to increase the quantity for.</param>
        /// <param name="NewIncreasedQuantity">The amount by which to increase the quantity of the specified resource.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// * Thrown when the specified resource (<paramref name="NameOfResource"/>) is not found in the <see cref="ResourcesDictionary"/>.
        /// </exception>
        /// <remarks>
        /// 
        /// <para>
        /// * The method attempts to increase the quantity of the specified resource in the <see cref="Stockpile"/>. If the resource is found,
        ///   the quantity is increased by the specified amount. If the resource is not found, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        public void IncreaseQuantity(string NameOfResource, uint NewIncreasedQuantity)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            if (ResourcesDictionary.TryGetValue(NameOfResource, out uint CurrentQuantity))
            {
                ResourcesDictionary[NameOfResource] = CurrentQuantity + NewIncreasedQuantity;
            }
            else
            {
                throw new ArgumentException($"Key not found: {NameOfResource}", nameof(ResourcesDictionary));
            }
        }

        /// <summary>
        /// * Decreases the quantity of a specified resource in the <see cref="Stockpile"/> object.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to decrease the quantity for.</param>
        /// <param name="NewDecreasedQuantity">The amount by which to decrease the quantity of the specified resource.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// * Thrown when the specified resource (<paramref name="NameOfResource"/>) is not found in the <see cref="ResourcesDictionary"/>,
        ///   or when the requested decrease is greater than the current quantity.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method attempts to decrease the quantity of the specified resource in the <see cref="Stockpile"/>. If the resource is found,
        ///   the quantity is decreased by the specified amount. If the resource is not found, an <see cref="ArgumentException"/> is thrown.
        /// * If the requested decrease is greater than the current quantity, an <see cref="ArgumentException"/> is also thrown.
        /// </para>
        /// </remarks>
        public void DecreaseQuantity(string NameOfResource, uint NewDecreasedQuantity)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            if (ResourcesDictionary.TryGetValue(NameOfResource, out uint CurrentQuantity))
            {
                if (CurrentQuantity < NewDecreasedQuantity)
                {
                    throw new ArgumentException($"Decreased quantity cannot be greater than current quantity: {NameOfResource}", nameof(ResourcesDictionary));
                }
                ResourcesDictionary[NameOfResource] = CurrentQuantity - NewDecreasedQuantity;
            }
            else
            {
                throw new ArgumentException($"Key not found: {NameOfResource}", nameof(ResourcesDictionary));
            }
        }

        /// <summary>
        /// * Gets the quantity of a specified resource in the <see cref="Stockpile"/> object.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to retrieve the quantity for.</param>
        /// <returns>The quantity of the specified resource.</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// * Thrown when the specified resource (<paramref name="NameOfResource"/>) is not found in the <see cref="ResourcesDictionary"/>.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method retrieves the quantity of the specified resource in the <see cref="Stockpile"/>. If the resource is found,
        /// * the method returns the quantity. If the resource is not found, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        public uint GetResourceQuantity(string NameOfResource)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            if (ResourcesDictionary.TryGetValue(NameOfResource, out uint Quantity))
            {
                return Quantity;
            }
            throw new ArgumentException($"Key not found: {NameOfResource}", nameof(ResourcesDictionary));
        }

        /// <summary>
        /// * Checks if the <see cref="Stockpile"/> contains a specified resource.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to check for.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="Stockpile"/> contains the specified resource; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method checks if the <see cref="Stockpile"/> contains the specified resource by verifying its presence
        ///   in the <see cref="ResourcesDictionary"/>. It returns <c>true</c> if the resource is found; otherwise, it returns <c>false</c>.
        /// </para>
        /// </remarks>
        public bool ContainsResource(string NameOfResource)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            return ResourcesDictionary.ContainsKey(NameOfResource);
        }

        /// <summary>
        /// * Adds a new resource with the specified name and quantity to the <see cref="Stockpile"/>.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to add.</param>
        /// <param name="QuantityToAdd">The quantity of the resource to add.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// * Thrown when a resource with the specified name already exists in the <see cref="Stockpile"/>.
        /// </exception>
        /// <remarks>
        ///
        /// <para>
        /// * The method adds a new resource to the <see cref="Stockpile"/> with the specified name and quantity.
        /// * If a resource with the same name already exists, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        public void AddResource(string NameOfResource, uint QuantityToAdd)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            if (ResourcesDictionary.ContainsKey(NameOfResource))
            {
                throw new ArgumentException($"Key already exists: {NameOfResource}", nameof(ResourcesDictionary));
            }

            ResourcesDictionary.TryAdd(NameOfResource, QuantityToAdd);
        }

        /// <summary>
        /// * Restocks the resources dictionary by replacing its content with the
        ///   backup dictionary.
        /// </summary>
        /// 
        /// <remarks>
        /// * This method copies the key-value pairs from the backup dictionary to
        ///   the resources dictionary, ensuring that both dictionaries have the same
        ///   key-value pairs after the operation. The values are cast from uint to
        ///   int during the copy to match the data type of the resources dictionary.
        /// </remarks>
        ///
        /// <attention> Internal instantiation, hidden and abstracted to the client </attention>
        public void Restock()
        {

            ResourcesDictionary = new ConcurrentDictionary<string, uint>(
            BackupDictionary.Select(kv => new KeyValuePair<string, uint>(kv.Key, kv.Value))
            );
        }
        
        /// <summary>
        /// * Removes a resource with the specified name from the <see cref="Stockpile"/>.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to remove.</param>
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// * Thrown when a resource with the specified name does not exist in the <see cref="Stockpile"/>.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method removes a resource with the specified name from the <see cref="Stockpile"/>.
        /// * If a resource with the specified name does not exist, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        public void RemoveResource(string NameOfResource)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            if (!ResourcesDictionary.TryRemove(NameOfResource, out _))
            {
                throw new ArgumentException($"Key not found: {NameOfResource}", nameof(ResourcesDictionary));
            }
        }

        /// <summary>
        /// * Updates the quantity of a resource with the specified name in the <see cref="Stockpile"/>.
        /// </summary>
        /// 
        /// <param name="NameOfResource">The name of the resource to update.</param>
        /// <param name="NewQuantity">The new quantity for the resource.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NameOfResource"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// * Thrown when a resource with the specified name does not exist in the <see cref="Stockpile"/>.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method updates the quantity of a resource with the specified name in the <see cref="Stockpile"/>.
        /// * If a resource with the specified name does not exist, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        public void UpdateResourceQuantity(string NameOfResource, uint NewQuantity)
        {
            if (NameOfResource == null)
            {
                throw new ArgumentNullException(nameof(NameOfResource), "NameOfResource cannot be null");
            }

            if (!ResourcesDictionary.ContainsKey(NameOfResource))
            {
                throw new ArgumentException($"Key not found: {NameOfResource}", nameof(ResourcesDictionary));
            }

            ResourcesDictionary[NameOfResource] = NewQuantity;
        }

        /// <summary>
        /// * Merges the quantities of resources from another <see cref="Stockpile"/> into the current instance.
        /// </summary>
        /// 
        /// <param name="OtherStockpile">The <see cref="ConcurrentDictionary{TKey, TValue}"/> containing resource quantities to merge.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="OtherStockpile"/> is <c>null</c>.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method merges the quantities of resources from another <see cref="Stockpile"/> into the current instance.
        /// * If a resource already exists in the current instance, its quantity is updated with the sum of the existing
        ///   quantity and the quantity from the other stockpile. If a resource does not exist, it is added to the current instance.
        /// </para>
        /// </remarks>
        public void MergeStockpile(ConcurrentDictionary<string, uint> OtherStockpile)
        {
            if (OtherStockpile == null)
            {
                throw new ArgumentNullException(nameof(OtherStockpile), "OtherStockpile cannot be null");
            }

            foreach (KeyValuePair<string, uint> kvp in OtherStockpile)
            {
                ResourcesDictionary.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => oldValue + kvp.Value);
            }
        }

        /// <summary>
        /// * Asynchronously splits the quantities of specified resources from the current <see cref="Stockpile"/> into a new stockpile.
        /// </summary>
        /// 
        /// <param name="NewStockpile">The <see cref="ConcurrentDictionary{TKey, TValue}"/> to store the split resource quantities.</param>
        /// <param name="ResourcesToRemove">The list of resource names to be split from the current stockpile.</param>
        /// 
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// * Thrown when <paramref name="NewStockpile"/> or <paramref name="ResourcesToRemove"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// * Thrown when a resource specified in <paramref name="ResourcesToRemove"/> is not found in the current instance, or
        ///   when a resource already exists in the new stockpile.
        /// </exception>
        /// 
        /// <remarks>
        /// <para>
        /// * The method asynchronously splits the quantities of specified resources from the current <see cref="Stockpile"/>
        ///   into a new stockpile. The split resources are removed from the current instance and added to the new stockpile.
        /// * If a resource to be split does not exist in the current instance, an <see cref="ArgumentException"/> is thrown.
        /// * If a resource already exists in the new stockpile, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        public async Task SplitStockpile(ConcurrentDictionary<string, uint> NewStockpile, List<string> ResourcesToRemove)
        {
            if (NewStockpile == null)
            {
                throw new ArgumentNullException(nameof(NewStockpile), "NewStockpile cannot be null");
            }

            if (ResourcesToRemove == null)
            {
                throw new ArgumentNullException(nameof(ResourcesToRemove), "ResourcesToRemove cannot be null");
            }

            foreach (string resource in ResourcesToRemove)
            {
                if (!ResourcesDictionary.ContainsKey(resource))
                {
                    throw new ArgumentException($"Key not found: {resource}", nameof(ResourcesDictionary));
                }

                if (NewStockpile.ContainsKey(resource))
                {
                    throw new ArgumentException($"Key already exists: {resource}", nameof(NewStockpile));
                }

                uint quantity = ResourcesDictionary[resource];
                if (quantity > 0)
                {
                    await Task.Run(() => ResourcesDictionary.TryRemove(resource, out _));
                    NewStockpile.TryAdd(resource, quantity);
                }
            }
        }
    }
}




