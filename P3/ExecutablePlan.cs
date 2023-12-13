/// <file> Plan.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Wed 11th Oct </date>
/// 
/// <version>
/// Revision History
/// - [1.0] Class design, Methods
/// - [2.0] Deep, Shallow copying
/// - [3.0] Debugging
///
/// </version>
/// 
/// <summary>
/// - ExecutablePlan is a derived class from the Plan base class and extends its
///   functionality to provide a framework for managing and executing a sequence of formulas.
/// </summary>
/// 
/// <invariant> Invariant:
/// - Formulas added to the ExecutablePlan must not have already been applied or completed.
/// - The client is restricted from replacing formulas that have already been applied or removing the last formula if it has been marked as completed.
/// - The client may query the current step using the Step property.
/// - The client can apply the current formula using the PlanApply method, which advances the plan to the next step.
/// - The ExecutablePlan class supports deep copying through the EPlanDeepCopy method, which includes a deep copy of its variables as well as those of the base class.
/// </invariant>
///
/// <note>
/// - The class is designed as closed and extensible.
/// - The class allows for Deep and Shallow copying of the object
/// </note>
///
/// <dependencies>
/// - Type: Inheritance => Plan (Super class), ExecutablePlan (Derived class)
/// - To view the dependencies, please refer to the <see cref="Plan"/> class documentation.
/// </dependencies>
///
/// <seealso cref="Plan"/>

using System;

namespace ResourceConversion
{

    public class ExecutablePlan : Plan
    {
        /// <note>
        /// - Public getter for external access
        /// - Private setter for internal access
        /// </note>
        protected uint _Step { get; private set; } = 0;
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

            this._Step = CurrentStep_;

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
        private ExecutablePlan EPClone()
        {
            Formula[] ClonedFormulas = new Formula[FormulaArray.Length];
            for (int i = 0; i < FormulaArray.Length; i++)
            {
                ClonedFormulas[i] = FormulaArray[i].Clone();
            }
            return new ExecutablePlan(ClonedFormulas, this._Step);
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
        public ExecutablePlan EPlanDeepCopy() { return this.EPClone(); }

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
        public ExecutablePlan EPlanShallowCopy() { return (ExecutablePlan)this.MemberwiseClone(); }

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

            if (_Step - 1 == LastElementIndex && CompletedArray[^1] == true)
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
            if(Index < _Step)
            {
                throw new InvalidDataException("[EP]ReplaceFormula: Called => [EP]ReplaceFormula(...) [Index cannot be less than _Step]");
            }

            for(uint i = 0; i < CompletedArray.Length; i++)
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
        public override void PlanApply()
        {
            for (uint i = 0; i < CompletedArray.Length; i++)
            {
                if (CompletedArray[_Step] == true)
                {
                    throw new InvalidDataException("[EP]PlanApply: Called => [EP]PlanApply() [Cannot Apply, Formula was already applied]\n");
                }
            }
            FormulaArray[_Step].Apply();
            CompletedArray[_Step] = true;
            _Step += 1;
        }
    }
}