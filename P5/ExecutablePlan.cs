/// <file> ExecutablePlan.cs </file>
/// <brief>
///        This file defines the derived class 'ExecutablePlan.' 'ExecutablePlan' extends the 
///        functionality of the base class 'Plan' through an "is a" relationship. It introduces the
///        concept of 'Step,' enhancing the capabilities of the 'Plan' class. The 'ExecutablePlan'
///        class maintains an array of 'Formula' objects and concurrently manages a 'Step' variable
///        along with a parallel boolean array indicating whether each formula has been completed. The
///        class efficiently supports both shallow and deep copying through move semantics. 
///        <see cref="[MOVE SEMANTICS]"/> The class also supports some simple mathematical 
///        operations through overloaded operators. <see cref="[OPERATORS]"/>
/// </brief>
/// 
/// <author>Jakob Balkovec (CPSC 3200)</author>
/// <instructor>A. Dingle (CPSC 3200)</instructor>
/// <date>Fri 14th Nov</date>
/// 
/// <version>
///     Revision History
///     - 1.0 [27/10/23] Initial Migration from C# => C++
///     - 2.0 [28/10/23] Optimization, Improved Copy, Move Semantics
///     - 3.0 [28/10/23] Debugging, Removing unnecessary allocations
///     - 4.0 [29/10/23] Refine documentation, fix error formatting
///     - 5.0 [29/10/23] More Debugging (std::move())
///     - 6.0 [14/11/23] C++ -> C# (Operators)
/// </version>
/// 
/// <invariant>Step cannot be negative (unsigned int)</invariant>
/// <invariant>'CompletedArray' matches the 'FormulaArray' size</invariant>
/// <invariant>FormulaArray is never NULL</invariant>
/// <invariant>'Step' should always be in bounds</invariant>
/// <invariant>CompletedArray is never null</invariant>
/// 
/// <moveSemantics>
///    ExecutablePlan Obj1 = new ExecutablePlan;
///    ExecutablePlan Obj2 = ExecutablePlan.EPDeepCopy();
///    ExecutablePlan Obj3 = ExecutablePlan.EPShallowCopy();
/// </moveSemantics>
/// 
/// <dependencies>
///     <internal>
///         - 'Formula' class
///         - Member, Helper, Utility methods
///         - Private Data Members
///     </internal>
///     <external>
///         - 'Plan' class
///     </external>
/// </dependencies>
/// 
/// <namespace>
///     Might be deemed unnecessary, but despite the distinctive function names, global
///     namespace pollution is still in the picture.
///     
///     {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan' class.</item>
/// </namespace>

using System.Collections.Concurrent;
using System.Drawing;

namespace ResourceConversion
{
    /// <summary>
    /// Inherits from the IPlan interface where the methods are defined
    /// Inherits from Plan because they share common data members
    /// </summary>
    public class ExecutablePlan : Plan
    {
        /// <note>
        /// - Public getter for external access
        /// - Private setter for internal access
        /// </note>
        protected uint PStep { get; private set; } = 0;
        private bool[] CompletedArray;

        /// <summary>
        /// - Initializes a new instance of the ExecutablePlan class with an array of formulas and the current step.
        /// </summary>
        /// 
        /// <param name="FormulaArray_">
        /// - An array of Formula objects used within the executable plan.
        /// </param>
        /// 
        /// <param name="CurrentStep_">
        /// - The current step within the plan.
        /// </param>
        /// 
        /// <exception cref="ArgumentException">
        /// - Thrown if the provided current step is invalid (outside the valid range).
        /// </exception>
        /// 
        /// <remarks>
        /// - The ExecutablePlan constructor initializes a new instance of the ExecutablePlan class. It takes
        ///   an array of formulas and the current step as parameters and ensures that the current step is within
        ///   a valid range. If logging is enabled, it logs the constructor invocation.
        /// </remarks>
        /// 
        /// <invariant>
        /// - The plan size should ideally be within the range of 0-25, but this range is subject to change
        ///   based on memory management constraints. [Subject to change]
        /// - For every child instantiation, the parent is instantiated. This behavior could lead to a memory
        ///   overflow if not managed carefully.
        /// </invariant>
        /// 
        /// <pre>Pre-Condition:
        /// - FormulaArray_ must be a valid array of Formula objects.
        /// - CurrentStep_ should be within the range of 1 to FormulaArray_.Length - 1.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - An instance of the ExecutablePlan class is successfully created with the provided parameters.
        /// </post>
        ///
        /// <note>
        /// '_' = discard / placeholder => <see cref="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards">  
        /// </note>
        public ExecutablePlan(Formula[] FormulaArray_, uint CurrentStep_ = 0) : base(FormulaArray_)
        {

            if (CurrentStep_ < 0 || CurrentStep_ >= FormulaArray_.Length)
            {
                throw new ArgumentException("Raise: => ExecutablePlan(...) [Current step is invalid]");
            }

            this.PStep = CurrentStep_;

            CompletedArray = new bool[FormulaArray_.Length];
            _ = CompletedArray.Select(_ => false).ToArray();
        }

        /// <summary>
        /// - Creates a deep copy of the current ExecutablePlan object, including cloned FormulaArray.
        /// </summary>
        /// 
        /// <returns>
        /// - A new ExecutablePlan object that is a deep copy of the current instance, including cloned FormulaArray.
        /// </returns>
        /// 
        /// <remarks>
        /// - The EPClone method creates a deep copy of the current ExecutablePlan instance, including a deep
        /// copy of its FormulaArray. The new ExecutablePlan is created with cloned formulas and the same _Step value.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - None
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A new ExecutablePlan is successfully created with cloned FormulaArray and the same _Step value.
        /// </post>
        /// 
        /// <exception cref="InvalidOperationException">
        /// - Thrown if the clone operation cannot be performed due to unexpected errors.
        /// </exception>
        ///
        /// <attention>
        /// - Declared Nullable
        /// - Client has to cast the object to the appropriaet type
        /// </attention>
        private object Clone()
        {
            Formula[] ClonedFormulas = new Formula[FormulaArray.Length];
            for (int i = 0; i < FormulaArray.Length; i++)
            {
                ClonedFormulas[i] = FormulaArray[i].Clone();
            }
            return new ExecutablePlan(ClonedFormulas, this.PStep);
        }

        /// <summary>
        /// - Creates a deep copy of the current ExecutablePlan object (alias for Clone method).
        /// </summary>
        /// 
        /// <returns>
        /// - A new ExecutablePlan object that is a deep copy of the current instance, including cloned FormulaArray.
        /// </returns>
        /// 
        /// <remarks>
        /// - The EPlanDeepCopy method is an alias for the EPClone method, which creates a deep copy of the current
        ///   ExecutablePlan instance, including cloned FormulaArray. The new ExecutablePlan is created with cloned formulas
        ///   and the same _Step value. This method is primarily used for creating independent copies of ExecutablePlan
        ///   instances for further processing.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - None
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A new ExecutablePlan is successfully created with cloned FormulaArray and the same _Step value.
        /// </post>
        /// 
        /// <exception cref="InvalidOperationException">
        /// - Thrown if the deep copy operation encounters unexpected errors during the cloning process.
        /// </exception>
        ///
        /// <attention>
        /// - Declared Nullable
        /// - Client has to cast the object to the appropriaet type
        /// </attention>
        public override object? DeepCopy() { return this.Clone() as ExecutablePlan; }

        /// <summary>
        /// Creates a shallow copy of the current ExecutablePlan object.
        /// </summary>
        /// 
        /// <returns>
        /// A new ExecutablePlan object that is a shallow copy of the current instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The EPlanShallowCopy method creates a shallow copy of the current ExecutablePlan instance, which means
        ///   that the new object references the same FormulaArray as the original object. Any changes made to the FormulaArray
        ///   in the new object will affect the original object and vice versa. This method is primarily used for quickly
        ///   duplicating the structure of an ExecutablePlan without deep cloning its contents.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - None
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A new ExecutablePlan is successfully created as a shallow copy of the current instance.
        /// </post>
        public override object? ShallowCopy() { return this.MemberwiseClone() as ExecutablePlan; }

        /// <summary>
        /// - Adds a new formula to the ExecutablePlan, updating the CompletedArray.
        /// </summary>
        /// 
        /// <param name="NewFormula">
        /// - The formula to add to the ExecutablePlan.
        /// </param>
        /// 
        /// <exception cref="InvalidDataException">
        /// - Thrown if NewFormula is 'null' (invalid data).
        /// </exception>
        /// 
        /// <remarks>
        /// - The AddFormula method allows you to add a new formula to the ExecutablePlan. This method first checks
        ///   if the provided NewFormula is not 'null', and if it is 'null', it logs an error and throws an InvalidDataException.
        ///   If NewFormula is valid, it is added to the ExecutablePlan's FormulaArray, and the CompletedArray is updated to reflect
        ///   the addition of the new formula.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - NewFormula must not be 'null'.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A new formula is added to the ExecutablePlan, and the CompletedArray is updated.
        /// </post>
        public override void AddFormula(in Formula NewFormula)
        {
            /// <summary>
            /// - Appends 'false' to the end of the provided array.
            /// - Made for readability, equivalent to a lambda in C++
            ///     - Action => no return value (void)
            ///     - Func<T, Tresult></L> => returns a value(T)
            /// </summary>
            Func<bool[], bool[]> AppendFalseToEnd = array =>
            {
                bool[] NewCompletedArray = new bool[array.Length + 1];
                Array.Copy(array, NewCompletedArray, array.Length);
                NewCompletedArray[array.Length] = false;
                return NewCompletedArray;
            };

            if (NewFormula == null)
            {
                throw new ArgumentNullException("Raise: => [EP]AddFormula(...) [NewFormula cannot be 'null']");
            }
            base.AddFormula(NewFormula);

            CompletedArray = AppendFalseToEnd(CompletedArray);
        }

        /// <summary>
        /// - Removes the last formula from the ExecutablePlan, subject to conditions.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException">
        /// - Thrown if the last formula cannot be removed due to specific conditions.
        /// </exception>
        /// 
        /// <remarks>
        /// - The RemoveLastFormula method removes the last formula from the ExecutablePlan, provided certain conditions
        ///   are met. The conditions include checking whether the current step is at the last element, ensuring that the last
        ///   formula is not marked as completed, and that there are no other exceptional conditions. If any of these conditions
        ///   are violated, an InvalidOperationException is thrown, and the last formula is not removed.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - None
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - The last formula is removed from the ExecutablePlan if the specified conditions are met.
        /// </post>
        public override void RemoveLastFormula()
        {
            uint LastElementIndex = (uint)FormulaArray.Length - 1;

            if (PStep - 1 == LastElementIndex && CompletedArray[^1] == true)
            {
                throw new InvalidOperationException("Raise: => [EP]RemoveLastFormula() [Cannot remove last 'Formula' object]");
            }

            base.RemoveLastFormula();
        }

        /// <summary>
        /// - Replaces a formula at the specified index in the ExecutablePlan, subject to conditions.
        /// </summary>
        /// 
        /// <param name="NewFormula">
        /// - The new formula to replace with.
        /// </param>
        /// 
        /// <param name="Index">
        /// - The index of the formula to replace.
        /// </param>
        /// 
        /// <exception cref="InvalidDataException">
        /// - Thrown if the provided index is less than the current step.
        /// </exception>
        /// 
        /// <exception cref="InvalidOperationException">
        /// - Thrown if the formula at the specified index has already been applied.
        /// </exception>
        ///
        /// <remarks>
        /// - The ReplaceFormula method allows you to replace a formula at the specified index in the ExecutablePlan, subject
        ///   to certain conditions.
        /// - It checks whether the provided index is valid and greater than or equal to the current step.
        /// - If the index is not valid, an InvalidDataException is thrown. Additionally, it ensures that the formula at the specified
        ///   index has not been marked as completed (applied) in the CompletedArray. If the formula is already applied, an InvalidOperationException
        ///   is thrown, and the replacement is prevented.
        /// - If all conditions are met, the formula is replaced at the specified index.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - Index must be greater than or equal to the current step.
        /// - The formula at the specified index must not have been applied (completed).
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - The formula at the specified index is replaced with the new formula.
        /// </post>
        public override void ReplaceFormula(Formula NewFormula, ushort Index)
        {
            //! Index, CurrentStep
            //! Edge case, if Index = Current step
            ///<note>
            /// Index can equal step
            ///     - Can still replace if Index = Step
            /// Cannot replace if Formula was already completed
            ///</note>
            if (Index < PStep)
            {
                throw new InvalidDataException("[EP]ReplaceFormula: Called => [EP]ReplaceFormula(...) [Index cannot be less than _Step]");
            }

            for (uint i = 0; i < CompletedArray.Length; i++)
            {
                //! Formula already completed
                if (CompletedArray[Index] == true)
                {
                    throw new InvalidOperationException("[EP]ReplaceFormula: Called => [EP]ReplaceFormula(...) [Cannot Replace, Formula was already applied]");
                }
            }
            base.ReplaceFormula(NewFormula, Index);
        }

        /// <summary>
        /// - Applies the current formula in the ExecutablePlan and marks it as completed.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException">
        /// - Thrown if the current formula has already been marked as completed (applied).
        /// </exception>
        /// 
        /// <remarks>
        /// - The PlanApply method is used to apply the current formula in the ExecutablePlan and mark it as completed. It checks
        ///   whether the formula at the current step has already been marked as completed. If the formula is already completed, an
        /// - InvalidOperationException is thrown, and the application is prevented. If the formula is not completed, it is applied, and
        ///   the CompletedArray is updated to mark the formula as completed. Finally, the _Step counter is incremented to move
        ///   to the next formula.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - None
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - The current formula is successfully applied and marked as completed in the ExecutablePlan.
        /// - The _Step counter is incremented.
        /// </post>
        ///
        /// <attention>
        /// Applys only one fomrula in the array
        ///     - Clients responsibility to wrap it inside a for loop!
        /// </attention>
        public override void Apply()
        {
            for (uint i = 0; i < CompletedArray.Length; i++)
            {
                if (CompletedArray[PStep] == true)
                {
                    throw new InvalidDataException("[EP]Apply: Called => [EP]PlanApply() [Cannot Apply, Formula was already applied]\n");
                }
            }
            FormulaArray[PStep].Apply();
            CompletedArray[PStep] = true;
            PStep += 1;
        }

        /// <summary>
        /// * Applies the formulas in the ExecutablePlan to the provided stockpile and returns the resulting stockpile.
        /// </summary>
        /// 
        /// <param name="stockpileInstance">
        /// * The stockpile to which the formulas should be applied. Must not be null.
        /// </param>
        /// <returns>
        /// * A new ConcurrentDictionary representing the stockpile after applying the formulas.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// * Thrown when the provided stockpileInstance is null.
        /// </exception>
        /// 
        /// <remarks>
        /// * This method iterates through the formulas in the ExecutablePlan, checks if the quantities
        ///   of input resources are sufficient in the provided stockpile, applies the formulas to
        ///   the stockpile, and returns the resulting stockpile. The input stockpile is not modified.
        /// </remarks>

///<attention> disbale the "Possible null dereference warning" </attention>        
#pragma warning disable CS8602
        public ConcurrentDictionary<string, uint> Apply(ConcurrentDictionary<string, uint> StockpileInstance)
        {
            if (StockpileInstance is null) { throw new ArgumentException("[EP]Apply: Called => [EP]Apply(...) [Stockpile cannot be null]"); }

            ConcurrentDictionary<string, uint> ResultStockpile = new ConcurrentDictionary<string, uint>(StockpileInstance);

            for(uint i = 0; i < FormulaArray.Length; i++)
            {
                Formula.DataLoader D_Data = new Formula.DataLoader(FormulaArray[i]);
                bool AreSufficient = D_Data.D_InputResources.All(resource =>
                {
                    return ResultStockpile.TryGetValue(resource, out uint currentQuantity) &&
                           currentQuantity >= D_Data.D_InputQuantities?[Array.IndexOf(D_Data.D_InputQuantities, resource)];
                });

                if (AreSufficient)
                {
                    FormulaArray[i].Apply();

                    for (int j = 0; j < D_Data.D_InputResources.Length; j++)
                    {
                        _ = ResultStockpile.AddOrUpdate(D_Data.D_InputResources[j],
                                                        key => throw new InvalidOperationException($"[EP]Apply: Called => [EP]Apply(...) [Resource {key} not found in stockpile]"),
                                                        (key, oldValue) => oldValue - D_Data.D_InputQuantities[j]);
                    }

                    for (int j = 0; j < FormulaArray[i].GetOutputResources.Length; j++)
                    {
                        _ = ResultStockpile.AddOrUpdate(FormulaArray[i].GetOutputResources[j],
                                                        key => FormulaArray[i].GetOutputQuantities?[j] ?? 0,
                                                        (key, oldValue) => oldValue + FormulaArray[i].GetOutputQuantities[j]);
                    }
                }
            }
            return ResultStockpile;
        }
///<attention> restore the "Possible null dereference warning" </attention>
#pragma warning restore CS8602
    }
}