/// <file> Plan.cs </file>
/// <author> Jakob Balkovec (CPSC 3200) </author>
/// <instructor> A. Dingle (CPSC 3200) </instructor>
/// 
/// <date> Wed 11th Oct </date>
/// 
/// <version>
/// Revision History
/// - [1.0] Class design, C++ --> C#
/// - [2.0] Refine documentation
/// - [3.0] Debugging
///
/// </version>
/// 
/// <summary>
/// The Plan class represents the management of an array of Formula o
/// bjects and provides various operations for working with this array.
/// The Plan class encapsulates a collection of formulas, offering
/// operations to manipulate and maintain this collection.
/// </summary>
/// 
/// <invariant> Invariant:
/// - FormulArrays should contain valid entries and should not be empty.
/// </invariant>
///
/// <note>
/// - The class is designed as closed and extensible.
/// - The class allows for Deep and Shallow copying of the object
/// </note>
///
/// <dependencies>
/// - Type: C-tor dependency injection
/// - To view the dependencies, please refer to the <see cref="Formula"/> class documentation.
/// </dependencies>

using System;

namespace ResourceConversion
{
    public class Plan
    {
        
        ///! Member
        protected Formula[] FormulaArray { get; private set; }

        /// <summary>
        /// - If set to true, it produces console output
        /// </summary>
        private readonly bool DisplayValues = true;
        //! Logs


        /// <summary>
        /// - Initializes a new instance of the Plan class with an array of formulas.
        /// </summary>
        /// 
        /// <param name="FormulaArray_">
        /// - The array of formulas.
        /// - The FormulaArray_ parameter is an array of Formula objects, each representing a formula
        ///   used within the Plan.
        /// </param>
        /// 
        /// <exception cref="ArgumentException">
        /// - Thrown if the provided array contains null values.
        /// </exception>
        /// 
        /// <pre>Pre-Condition:
        /// - The FormulaArray_ parameter must be instantiated before calling this constructor.
        /// - FormulaArray_ must not include any null values.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - An instance of the Plan object is created with the specified array of formulas.
        /// </post>
        /// 
        /// <remarks>
        /// - This constructor initializes a new instance of the Plan class by accepting an array of formulas
        ///   as a parameter. It ensures that the array is free of null values and throws an ArgumentException
        ///   if any null values are found. After validation, the constructor initializes the Plan object
        ///   and logs the construction if LogMessages is enabled.
        /// </remarks>
        public Plan(Formula[] FormulaArray_)
        {
            if (FormulaArray_.All(resource => resource == null))
            {
                throw new ArgumentException("Raise: => Plan() [Array cannot contain null values]");
            }
        
            this.FormulaArray = FormulaArray_;
        }

        /// <summary>
        /// - Creates a deep copy of the Plan instance, including deep copies of its formulas.
        /// </summary>
        /// 
        /// <returns>
        /// - A deep copy of the Plan instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The Clone method creates a deep copy of the current Plan instance, including creating
        ///   deep copies of all its associated Formula objects. This means that all Formula objects
        ///   within the new Plan instance are independent copies of the original, preserving the state
        ///   of each formula without any reference to the original Plan instance.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - The Plan instance must be properly initialized before invoking this method.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A new Plan instance is created as a deep copy of the current instance and its formulas.
        /// </post>
        private Plan Clone()
        {
            Formula[] ClonedFormulas = new Formula[FormulaArray.Length];
            for (int i = 0; i < FormulaArray.Length; i++)
            {
                ClonedFormulas[i] = FormulaArray[i].Clone();
            }
            return new Plan(ClonedFormulas);
        }

        /// <summary>
        /// - Creates a deep copy of the Plan instance, including deep copies of its formulas.
        /// </summary>
        /// 
        /// <returns>
        /// - A deep copy of the Plan instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The PlanDeepCopy method is used to create a deep copy of the current Plan instance,
        ///   including deep copies of all its associated Formula objects. The deep copy ensures that
        ///   the new Plan instance is entirely independent and self-contained, preserving the state of
        ///   each formula without any reference to the original Plan instance.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - The current Plan instance (this) cannot be null.
        /// - The Clone method must be available for invocation to create deep copies of the formulas.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A deep copy of the Plan instance, along with its associated formulas, is successfully created.
        /// </post>
        public Plan PlanDeepCopy() { return this.Clone(); }

        /// <summary>
        /// - Creates a shallow copy of the Plan instance. The formulas are not deep-copied.
        /// </summary>
        /// 
        /// <returns>
        /// - A shallow copy of the Plan instance.
        /// </returns>
        /// 
        /// <remarks>
        /// - The PlanShallowCopy method is used to create a shallow copy of the current Plan instance.
        /// - Unlike a deep copy, the formulas within the Plan are not deep-copied, and both the original
        ///   and copied Plan instances will share references to the same Formula objects.
        /// - This means changes to the formulas in the copied Plan will affect the original and vice versa.
        /// </remarks>
        /// 
        /// <pre>Pre-Condition:
        /// - The current Plan instance (this) cannot be null.
        /// - The `MemberwiseClone` method must be available for invocation to create shallow copies.
        /// </pre>
        /// 
        /// <post>Post-Condition:
        /// - A shallow copy of the Plan instance is successfully created.
        /// </post>
        public Plan PlanShallowCopy() { return (Plan)this.MemberwiseClone(); }

        /// <summary>
        /// - Determines whether the current Plan instance is equal to another Plan instance.
        /// </summary>
        /// 
        /// <param name="obj">
        /// - The Plan instance to compare with the current instance.
        /// </param>
        /// 
        /// <returns>
        /// <c>true</c>
        /// - if the two instances are considered equal;
        /// otherwise,
        /// <c>false</c>.
        /// </returns>
        /// 
        /// <remarks>
        /// Two Plan instances are considered equal if they have the same content in their FormulaArray property.
        /// This method overrides the <see cref="object.Equals(object)"/> method.
        /// </remarks>
        public bool Equals(Plan obj)
        {
            if (obj is Plan otherPlan)
            {
                if (ReferenceEquals(this, otherPlan)) { return true; }

                if (FormulaArray.Length != otherPlan.FormulaArray.Length) { return false; }

                for (int i = 0; i < FormulaArray.Length; i++)
                {
                    if (!FormulaArray[i].Equals(otherPlan.FormulaArray[i])) { return false; }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// - Adds a new formula to the plan.
        /// </summary>
        /// 
        /// <param name="NewFormula">
        /// - The formula to add.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// - Thrown if NewFormula is null.
        /// </exception>
        /// 
        /// <remarks>
        /// - The Method method is used to add a new formula to the Plan. This method allows you to expand
        ///   the Plan's list of formulas. The 'in' keyword specifies that NewFormula is passed by reference
        ///   and treated as a constant, meaning it cannot be modified within the method.
        /// </remarks>
        /// 
        /// <seealso cref="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in"/>
        /// <seealso cref="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges"/>
        /// 
        /// <pre>Pre-condition:
        /// - NewFormula must not be null.
        /// </pre>
        /// 
        /// <post>Post-condition:
        /// - The array of formulas will contain one additional formula.
        /// - The NewFormula will be added to the end of the FormulaArray.
        /// </post>
        /// 
        /// </remarks>
        public virtual void AddFormula(in Formula NewFormula) 
        {
            if(NewFormula == null)
            {
                throw new ArgumentNullException("Raise: AddFormula() => [NewFormula is 'null']");
            }

            Formula[] NewFormulaArray = new Formula[FormulaArray.Length + 1];

            for(uint i = 0; i < FormulaArray.Length; i++)
            {
                NewFormulaArray[i] = FormulaArray[i];
            }

            NewFormulaArray[^1] = NewFormula;
            FormulaArray = NewFormulaArray;
        }

        /// <summary>
        /// - Removes the last formula from the plan.
        /// </summary>
        /// 
        /// <exception cref="InvalidDataException">
        /// - Thrown if the array size is less than or equal to 0.
        /// </exception>
        /// 
        /// <pre>Pre-condition:
        /// - The array of formulas must not be empty.
        /// </pre>
        /// 
        /// <post>Post-condition:
        /// - The array of formulas will contain one less formula.
        /// - The last formula in the array will be removed.
        /// </post>
        /// 
        /// <remarks>
        /// - The Method method is used to remove the last formula from the Plan. This method allows you to
        ///   reduce the Plan's list of formulas by eliminating the last formula in the array.
        /// </remarks>
        public virtual void RemoveLastFormula()
        {
            if (FormulaArray.Length <= 0)
            {
                throw new InvalidDataException("Raise: RemoveLastFormula() => [Array size is less than or equal to 0]");
            }

            Formula[] NewFormulaArray = new Formula[FormulaArray.Length - 1];
            for (uint i = 0; i < NewFormulaArray.Length; i++)
            {
                NewFormulaArray[i] = FormulaArray[i];
            }

            FormulaArray = NewFormulaArray;
        }

        /// <summary>
        /// - Replaces a formula at the specified index with a new formula.
        /// </summary>
        /// 
        /// <param name="Index">
        /// - The index of the formula to replace.
        /// </param>
        /// 
        /// <param name="NewFormula">
        /// - The new formula to replace with.
        /// </param>
        /// 
        /// <exception cref="IndexOutOfRangeException">
        /// - Thrown if Index is out of bounds.
        /// </exception>
        /// 
        /// <exception cref="ArgumentNullException">
        /// - Thrown if NewFormula is 'null'.
        /// </exception>
        /// 
        /// <pre>Pre-condition:
        /// - Index must be a valid index within the array of formulas.
        /// - NewFormula must be passed in as a valid parameter and not be 'null'.
        /// </pre>
        /// 
        /// <post>Post-condition:
        /// - The formula at the specified Index will be replaced with NewFormula.
        /// </post>
        /// 
        /// <remarks>
        /// - The ReplaceFormula allows the client to replace a formula at the specified index within the Plan's
        ///   list of formulas with a new formula. This method ensures that the index is within bounds and
        ///   that the NewFormula parameter is not null before performing the replacement.
        /// </remarks>
        public virtual void ReplaceFormula( Formula NewFormula, ushort Index)
        {

            if(Index < 0 || Index >= FormulaArray.Length)
            {
                throw new IndexOutOfRangeException("Raise: ReplaceFormula() => [Index is out of bounds]");
            }

            if (NewFormula is null)
            {
                throw new ArgumentNullException("Raise: ReplaceFormula() => [NewFormula cannot be 'null']");
            }

            FormulaArray[Index] = NewFormula;
        }

        /// <summary>
        /// - Applies each formula within the Plan, triggering the calculation or processing
        ///   of each formula's output.
        /// </summary>
        /// 
        /// <remarks>
        /// - The PlanApply method iterates through each formula in the Plan's array and invokes
        ///   the Apply method of each formula. This method is typically used to calculate or
        ///   process the output of each formula within the Plan.
        /// </remarks>
        /// 
        /// <pre>Pre-condition:
        /// - The Plan instance must be properly initialized before calling this method.
        /// </pre>
        /// 
        /// <post>Post-condition:
        /// - Each formula's Apply method has been executed, potentially updating the formula's
        /// output.
        /// </post>
        public virtual void PlanApply()
        {
            for(int i = 0; i < FormulaArray.Length; i++) { FormulaArray[i].Apply(); }
        }

        /// <summary>
        ///  - Displays the resources used by each formula within the Plan, if DisplayValues is enabled.
        /// </summary>
        /// 
        /// <remarks>
        /// - The PlanDisplayResources method is responsible for displaying the resources used by
        ///   each formula in the Plan's array. This information is displayed only if the DisplayValues
        ///   option is enabled.
        /// </remarks>
        /// 
        /// <pre>Pre-condition:
        /// - The Plan instance must be properly initialized before calling this method.
        /// </pre>
        /// 
        /// <post>Post-condition:
        /// - If DisplayValues is enabled, the resources used by each formula are displayed.
        /// </post>
        public void PlanDisplayResources()
        {
            if (DisplayValues)
            {
                for (uint i = 0; i < FormulaArray.Length; i++)
                {
                    FormulaArray[i].DisplayResources();
                }
            }
            return;
        }

        /// <summary>
        /// - Displays the output of each formula within the Plan, if DisplayValues is enabled.
        /// </summary>
        /// 
        /// <remarks>
        /// - The PlanDisplayFormulaOutput method is responsible for displaying the output of each
        ///   formula in the Plan's array. This output is displayed only if the DisplayValues option is
        ///   enabled.
        /// </remarks>
        /// 
        /// <pre>Pre-condition:
        /// - The Plan instance must be properly initialized before calling this method.
        /// </pre>
        /// 
        /// <post>Post-condition:
        /// - If DisplayValues is enabled, the output of each formula is displayed.
        /// </post>
        public void PlanDisplayFormulaOutput()
        {
            if (DisplayValues)
            {
                for (uint i = 0; i < FormulaArray.Length; i++)
                {
                    FormulaArray[i].DisplayFormulaOutput();
                }
            }
            return;
        }

        /// <summary>
        /// - Gets an array of Formula objects.
        /// </summary>
        /// 
        /// <returns>
        /// - An array of Formula objects.
        /// </returns>
        public Formula[] GetFormulaArray() { return FormulaArray; }
    }
}

