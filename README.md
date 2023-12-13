# CPSC 3200 Object-Oriented Development

- This repository showcases my work and projects related to the course, focusing on class design and relationships in the context of object-oriented development.

## Course Overview

This course emphasizes object-oriented development principles, covering essential topics such as interfaces, contracts, containment, composition, inheritance, overloading, and polymorphism. The goal is to analyze alternative design choices, enhance software maintainability, and promote code reuse.
---

## Programming Assignments

### P1(C#)
- Class design
- Unit testing

#### P1 Description
Class named `Formula` (in Formula.cs) that represents a mechanism for converting resources into other resources. Each formula specifies input and output resources along with their respective quantities. Examples include converting iron ore into iron bars or combining various ingredients to produce cookies.

Additionally, each formula has a chance of failure or producing a different quantity of outputs than expected. This chance is not visible or modifiable by clients, but the class provides a method called `Apply()` to simulate the outcome of applying the formula.

##### Key Features:
- Input and output resources with quantities.
- Possibility of partial failure or overproduction.
- Immutable values for inputs and outputs after instantiation.
- Proficiency level that influences the chances of outcomes.
- Proficiency level can be increased by the client, affecting outcome probabilities.
- Clients can query the names and quantities of the formula's inputs and outputs but cannot modify them. The proficiency level, its increase, and the chances of different outcomes are encapsulated within the class, providing a versatile mechanism for simulating resource conversion with varying proficiency levels.
 
### P2(C++)
- Composition
- Move semantics

#### P2 Description
In Part I of the project, you are tasked with enhancing the existing C++ Formula class and introducing a new class called `Plan`. The Formula class is to be rewritten, maintaining the same type definition as in P1. The `Plan` class represents a sequence of `Formulas` and offers functionalities such as adding a new `Formula` to the end, removing the last `Formula`, and replacing a `Formula` anywhere in the sequence.

To achieve this, dependency injection is employed to provide an initial sequence of `Formulas`. Notably, the `Plan` class implements C++ copy and move semantics. The copy constructor and copy assignment operator ensure deep copying of internal Formulas to prevent unintended aliasing. This means that actions on one `Plan` object, such as leveling up a `Formula`, should not affect any copies of that object.

Additionally, the `Plan` class implements a destructor to appropriately dispose of owned resources. Each `Plan` object encapsulates a variable number of distinct `Formula` objects.

##### Key Features:
- Dynamic Addition
- Dynmaic Removal
- Replacement

### P3(C#)
- Inheritance
- Exceptions
  
#### P3 Description
In Part I of the assignment, the task is to redesign the Plan class in C#. Omitting C++-specific concepts like copy or move constructors, assignment operators, and destructors, the goal is to introduce a `DeepCopy()` method equivalent to a copy operation in C#. The decision to support or omit `ShallowCopy()` is left to the implementer. Exceptions should be implemented where appropriate in the class designs.

Additionally, a new subclass, `ExecutablePlan`, is to be created as a specialization of `Plan`. This subclass introduces the following functionalities:

##### Key Features:
- Clients can query the current step.
- Clients can apply the current formula, advancing the plan to the next step.
- Clients cannot replace formulas that have already been applied.
- Clients cannot remove the last formula if it has already been completed.
  
### P4(C++)
- Operator overloading
- Smart pointers
  
#### P4 Description
This C++ program, a continuation from a previous assignment in C#, focuses on enhancing object-oriented design principles, incorporating copy and move semantics, operator overloading, and the implementation of a new class called Stockpile.

##### Key Features:
- **Class Design**:
  - Reimplementation of the `ExecutablePlan` class in C++ along with its base classes, Plan and Formula.
  - Introduction of C++ copy and move semantics for appropriate classes.
  - Adjustment of public interfaces with overloaded operators, including ==, !=, and additional operators for Plan and ExecutablePlan.
  
- **Stockpile Class**:
  - Creation of a new class called `Stockpile` that encapsulates a set of resources and related quantities.
  - Supports client queries, allows quantity increases, and, if possible, decreases.
  - Implements suppression of copying to manage resources efficiently.
    
- **Overloaded Apply Method**:
  - The `Apply()` method of ExecutablePlan is overloaded to take and return smart pointers to a Stockpile.
  - Checks inputs of the current formula, deducts associated quantities from the input Stockpile, and applies the formula.
  - Ensures no change if resources are insufficient.

- **Implementation Details**:
  - The program demonstrates design modifications and newly introduced features, including the `Stockpile` class and the overloaded `Apply()` method.
  - Extensive use of smart pointers ensures efficient resource management.
  - The driver illustrates these modifications, tests overloaded operators, and validates class move semantics.

### P5(C#)
- Simulated multiple inheritance
- Interfacaes

#### P5 Description
This program showcases an implementation in C# that exercises the principles of multiple inheritance and C# interfaces, as required by CPSC 3200's Object-Oriented Development course.

##### Class Design

In this part, the Stockpile class is reimagined as a C# class, incorporating the new capability to restock() initial quantities. Additionally, a new class, Assembly, is introduced, acting as a hybrid of a Plan type and a Stockpile. This class supports queries, quantity adjustments, and the application of formulas. The Apply() capability is implemented via both the basic Plan type version and an overloaded version. The Assembly class adjusts resources, utilizing its own resources if the given ones are insufficient to cover the current formula's cost. The design leverages C# interfaces where appropriate, ensuring a versatile and cohesive structure.

##### Driver - External Perspective of Client

The P5 driver serves as an external client, testing the use of the 'multiply-inherited' types together. It defines and uses heterogeneous collections (HCs) for both Plan and Stockpile, allowing a reference from either HC to address an Assembly object. The driver triggers various mode changes, testing the flexibility and robustness of the inheritance hierarchy design.


## How to Use
- Clone the repository to your local machine using the following command:
```bash
git clone [repository_url]
```
- Navigate to the specific program folder to access relevant files and documentation.
- If applicable, follow any instructions provided in the README files within each program folder.
Contributing

##### Feel free to contribute to this repository by:

- Submitting bug reports or issues.
- Proposing improvements or optimizations.
- Adding additional notes or explanations to existing programs.
- Please adhere to the contribution guidelines outlined in the repository.

## Notes

This repository is for **educational purposes. Avoid copying solutions verbatim for graded assignments or exams**.
If you have questions or need clarification on any topic, feel free to open an issue or reach out for discussion.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
