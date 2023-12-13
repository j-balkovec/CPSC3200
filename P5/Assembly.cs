/// <file> Assembly.cs </file>
/// 
/// <brief>
/// [DESC]: Represents an assembly that stores resources like a Stockpile, supports queries, and quantity adjustments.
///			Additionally, it acts as both a Plan type, overseeing and adjusting Formulas, and a Stockpile, allowing for restocking
///			of initial quantities. The Apply() capability is supported via both the basic Plan type version and an overloaded
///			version. The Apply() implementation works similarly to ExecutablePlan's, but can use its own resources if the given
///			resources are insufficient to cover the current Formula's cost.
/// </brief>
/// 
/// <remarks>
/// * The Assembly class combines the functionalities of a Plan and a Stockpile, providing a composite type that
///   allows for dynamic adjustments and queries related to both resources and formulas.
/// </remarks>
/// 
/// <seealso cref="Plan"/>
/// <seealso cref="Stockpile"/>
/// 
/// <author>Jakob Balkovec (CPSC 3200)</author>
/// <instructor>A. Dingle (CPSC 3200)</instructor>
/// <date>Fri 27th Oct</date>
/// 
/// <version>
///     Revision History
///     - 1.0 [18/11/2023]: Initial class design
/// </version>
/// 
/// <invariant>
/// * The InternalPlanType field should always contain either a Plan or an ExecutablePlan instance. 
/// * The InternalStockpile field should always contain a non-null Stockpile instance. 
/// * The IsExecutable flag should correctly represent whether the InternalPlanType is an ExecutablePlan or not. 
/// * If IsExecutable is true, then InternalPlanType should be an instance of ExecutablePlan. 
/// * If IsExecutable is false, then InternalPlanType should be an instance of Plan. 
/// * When creating a new instance of Assembly through the constructor, if an ExecutablePlan is provided, the IsExecutable flag should be set to true. 
/// * When creating a new instance of Assembly through the constructor, if a Plan is provided, the IsExecutable flag should be set to false. 
/// * The ShallowCopy and DeepCopy methods should return a new instance of Assembly with the same InternalPlanType and a copied InternalStockpile.
/// * The AddFormula, RemoveLastFormula, ReplaceFormula, and Apply methods should behave correctly based on the state of the IsExecutable flag and the type of InternalPlanType. 
/// * The Apply method with the ConcurrentDictionary overload should only be called if the IsExecutable flag is true.
/// </invariant>
/// 
/// <dependencies>
///     <internal>
///         - Member, Helper, Utility methods
///         - Private Data Members
///     </internal>
///     <external>
///         - IStockpile, IPlan interface
///     </external>
/// </dependencies>
/// 
/// <namespace>
///     Might be deemed unnecessary, but despite the distinctive function names, global
///     namespace pollution is still in the picture.
///     - {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan' class.
/// </namespace>

using System;
using System.Collections.Concurrent;

namespace ResourceConversion
{
	public class Assembly : IPlan, IStockpile
	{
        /// <note>
        /// Generic object that either holds a 'Plan' or an 'ExecutablePlan'
        /// </note>
        /// 
        /// <remark>
        /// Using double composition and echoing the interface
        /// </remark>
		private object InternalPlanType;
		private Stockpile InternalStockpile;


        /// <note>
        /// * Flag that identifies if the Plan object is executable or not
        /// * Used in-place of 'as' and 'is' to improve performance and lower the run-time overhead
        /// </note>
        private bool IsExecutable = false;

        /// <summary>
        /// * Represents an assembly that combines a plan and a stockpile.
        /// </summary>
        /// 
        /// <remarks>
        /// * Assumption of ownership, the object passed in is constructed via the 'new' operator
        ///     ==> External instantiation, which allows for postoponed instantiation
        ///     
        /// * An assembly can be created with either a Plan or an ExecutablePlan.
        /// * If an ExecutablePlan is provided, the assembly is considered executable.
        /// </remarks>
        /// 
        /// <param name="InternalPlanType">The plan or executable plan to be associated with the assembly.</param>
        /// <param name="InternalStockpile">The stockpile to be associated with the assembly.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// Thrown when either of the following conditions is met:
        /// - InternalPlanType is a Plan and is null.
        /// - InternalPlanType is an ExecutablePlan and is null.
        /// - InternalStockpile is null.
        /// </exception>
        public Assembly(object InternalPlanType, Stockpile InternalStockpile)
		{
			if (InternalPlanType is Plan && InternalPlanType is null)
			{
				throw new ArgumentException("[A]Assembly => Called: Assembly(...) Plan object cannot be null");
			}

            if (InternalPlanType is ExecutablePlan && InternalPlanType is null)
			{
				throw new ArgumentException("[A]Assembly => Called: Assembly(...) ExecutablePlan object cannot be null");
			}

			if (InternalStockpile is null)
			{
				throw new ArgumentException("[A]Assembly => Called: Assembly(...) Stockpile object cannot be null");
			}

            if (InternalPlanType is ExecutablePlan) { this.IsExecutable = true; }

			this.InternalPlanType = InternalPlanType;
			this.InternalStockpile = InternalStockpile;
        }

        /// <summary>
        /// * Creates a shallow copy of the assembly.
        /// </summary>
        /// 
        /// <returns>
        /// * A shallow copy of the assembly.
        /// </returns>
        /// 
        /// <remarks>
        /// <para>
        /// * A shallow copy includes copies of the stockpile and the associated plan or executable plan.
        /// </para>
        /// <para>
        /// * If the assembly is not executable, the returned assembly will have a copied Plan.
        /// * If the assembly is executable, the returned assembly will have a copied ExecutablePlan.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public object? ShallowCopy()
		{
            Stockpile? CopiedStockpile = InternalStockpile.ShallowCopy() as Stockpile;

            if (!IsExecutable)
            {
                Plan? CopiedPlan = InternalPlanType as Plan;
                _ = CopiedPlan.ShallowCopy();
				
				return new Assembly(CopiedPlan, CopiedStockpile);
            }

			else if(IsExecutable)
			{
                ExecutablePlan? CopiedExecutablePlan = InternalPlanType as ExecutablePlan;
                _ = CopiedExecutablePlan.ShallowCopy();

				return new Assembly(CopiedExecutablePlan, CopiedStockpile);
            }
			throw new InvalidOperationException("[A]Assembly => Called: ShallowCopy() Bad ShallowCopy() call. Check InternalPlanType");
        }

        /// <summary>
        /// * Creates a deep copy of the assembly.
        /// </summary>
        /// 
        /// <returns>
        /// * A deep copy of the assembly.
        /// </returns>
        /// 
        /// <remarks>
        /// <para>
        /// * A deep copy includes copies of the stockpile and the associated plan or executable plan, 
        ///   including deep copies of any internal structures within the plan or executable plan.
        /// </para>
        /// <para>
        /// * If the assembly is not executable, the returned assembly will have a deep copied Plan.
        /// * If the assembly is executable, the returned assembly will have a deep copied ExecutablePlan.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public object? DeepCopy()
		{
            Stockpile? CopiedStockpile = InternalStockpile.DeepCopy() as Stockpile;

            if (!IsExecutable)
            {
                Plan? CopiedPlan = InternalPlanType as Plan;
                _ = CopiedPlan.DeepCopy();

                return new Assembly(CopiedPlan, CopiedStockpile);
            }

            else if (IsExecutable)
            {
                ExecutablePlan? CopiedExecutablePlan = InternalPlanType as ExecutablePlan;
                _ = CopiedExecutablePlan.DeepCopy();

                return new Assembly(CopiedExecutablePlan, CopiedStockpile);
            }
            throw new InvalidOperationException("[A]Assembly => Called: DeepCopy() Bad DeepCopy() call. Check InternalPlanType");
        }

        /// <summary>
        /// * Adds a formula to the associated plan or executable plan.
        /// </summary>
        /// 
        /// <param name="NewFormula">The formula to be added.</param>
        /// <remarks>
        /// <para>
        /// * If the assembly is not executable, the formula is added to the associated Plan.
        /// * If the assembly is executable, the formula is added to the associated ExecutablePlan.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public void AddFormula(in Formula NewFormula)
        {
            if (!IsExecutable)
			{
                Plan? PlanObj = InternalPlanType as Plan;
                PlanObj.AddFormula(NewFormula);
                return;
			}

			else if (IsExecutable)
			{
                ExecutablePlan? ExPlanObj = InternalPlanType as ExecutablePlan;
                ExPlanObj.AddFormula(NewFormula);
                return;
            }

            throw new InvalidOperationException("[A]Assembly => Called: AddFormula(...) Bad AddFormula(...) call. Check InternalPlanType");
        }

        /// <summary>
        /// * Removes the last formula from the associated plan or executable plan.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// * If the assembly is not executable, the last formula is removed from the associated Plan.
        /// * If the assembly is executable, the last formula is removed from the associated ExecutablePlan.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public void RemoveLastFormula()
		{
            if (!IsExecutable)
            {
                Plan? PlanObj = InternalPlanType as Plan;
                PlanObj.RemoveLastFormula();
                return;
            }

            else if (IsExecutable)
            {
                ExecutablePlan? ExPlanObj = InternalPlanType as ExecutablePlan;
                ExPlanObj.RemoveLastFormula();
                return;
            }

            throw new InvalidOperationException("[A]Assembly => Called: RemoveLastFormula() Bad RemoveLastFormula() call. Check InternalPlanType");
        }

        /// <summary>
        /// * Replaces a formula at the specified index in the associated plan or executable plan.
        /// </summary>
        /// 
        /// <param name="NewFormula">The new formula to be inserted.</param>
        /// <param name="Index">The index at which to replace the formula.</param>
        /// 
        /// <remarks>
        /// <para>
        /// * If the assembly is not executable, the formula at the specified index is replaced in the associated Plan.
        /// * If the assembly is executable, the formula at the specified index is replaced in the associated ExecutablePlan.
        /// </para>
        /// </remarks>
        /// 
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public void ReplaceFormula(Formula NewFormula, ushort Index)
		{
            if (!IsExecutable)
            {
                Plan? PlanObj = InternalPlanType as Plan;
                PlanObj.ReplaceFormula(NewFormula, Index);
                return;
            }

            else if (IsExecutable)
            {
                ExecutablePlan? ExPlanObj = InternalPlanType as ExecutablePlan;
                ExPlanObj.ReplaceFormula(NewFormula, Index);
                return;
            }

            throw new InvalidOperationException("[A]Assembly => Called: ReplaceFormula(...) Bad ReplaceFormula(...) call. Check InternalPlanType");
        }

        /// <summary>
        /// * Applies the plan or executable plan associated with the assembly.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public void Apply()
		{
            if (!IsExecutable)
            {
                Plan? PlanObj = InternalPlanType as Plan;
                PlanObj.Apply();
                return;
            }

            else if (IsExecutable)
            {
                ExecutablePlan? ExPlanObj = InternalPlanType as ExecutablePlan;
                ExPlanObj.Apply();
                return;
            }

            throw new InvalidOperationException("[A]Assembly => Called: Apply(...) Bad Apply(...) call. Check InternalPlanType");
        }

        /// <summary>
        /// * Applies the executable plan associated with the assembly to update the stockpile.
        /// </summary>
        /// 
        /// <param name="StockpileInstance">The current stockpile to be updated.</param>
        /// <returns>The updated stockpile after applying the executable plan.</returns>
        ///
        /// <exception cref="InvalidOperationException">
        /// * Thrown when the method is called with an invalid state. 
        /// * Check the state of InternalPlanType.
        /// </exception>
        public ConcurrentDictionary<string, uint> Apply(ConcurrentDictionary<string, uint> StockpileInstance)
        {
            if (IsExecutable)
            {
                ExecutablePlan? ExPlanObj = InternalPlanType as ExecutablePlan;
                return ExPlanObj.Apply(StockpileInstance);
            }

            throw new InvalidOperationException("[A]Assembly => Called: Apply(...) Bad [Overload]Apply(...) call. Check InternalPlanType");
        }

        /// <summary>
        /// Restocks the resources in the stockpile.
        /// </summary>
        public void Restock() => InternalStockpile.Restock();

        /// <summary>
        /// Increases the quantity of a specific resource in the stockpile.
        /// </summary>
        /// <param name="NameOfResource">The name of the resource.</param>
        /// <param name="NewIncreasedQuantity">The new increased quantity of the resource.</param>
        public void IncreaseQuantity(string NameOfResource, uint NewIncreasedQuantity) => InternalStockpile.IncreaseQuantity(NameOfResource, NewIncreasedQuantity);

        /// <summary>
        /// Decreases the quantity of a specific resource in the stockpile.
        /// </summary>
        /// <param name="NameOfResource">The name of the resource.</param>
        /// <param name="NewDecreasedQuantity">The new decreased quantity of the resource.</param>
        public void DecreaseQuantity(string NameOfResource, uint NewDecreasedQuantity) => InternalStockpile.DecreaseQuantity(NameOfResource, NewDecreasedQuantity);
    }
}

