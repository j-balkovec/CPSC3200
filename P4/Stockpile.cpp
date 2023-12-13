//[FILE]: Stockpile.h
//[DESC]: This file defines the derived class 'Stockpile. It introduces the
//        concept of encapsulating a set of resources and related quantities. 
//        The 'Stockpile' class offers limted functionality with the increase and 
//        decrease quantity methods. The class does not support copying, but it 
//        supports move semantics. Objects of type 'Stockpile' should be moved when possible to perserve 
//        optimal performance (STL containers are expensive) {[SEE]: [MOVE SEMANTICS]}
//
//[AUTHOR]: Jakob Balkovec (CPSC 3200)
//[INSTRUCTOR]: A. Dingle (CPSC 3200)
//
//[DATE]: Tue 31th Oct
//[VERSION]: Revision History
//          - 1.0 [10/31/23] - Initial class design
//          - 2.0 [10/31/23] - Documentation and Invariants
//
//[INVARIANT]: The ResourcesMap is modified only through the IncreaseQuantity and DecreaseQuantity 
//             member functions, ensuring that resource quantities remain non-negative.
//[INVARIANT]: 'NewQuantities' should always be non-negative or the size_t(unsigned long) will overflow
//[INVARIANT]: The IncreaseQuantity and DecreaseQuantity member functions do not introduce any 
//             resource names that are not present in the ResourcesMap.
//[INVARIANT]: After any operation (construction, move, etc.), the 'ResourcesMap' remains in a consistent and valid state.

#include <unordered_map>
#include <algorithm>
#include <iterator>
#include <functional>

#include "Stockpile.h"

namespace ResourceConversion 
{
  //[DESC]: Reassigns the data encapsulated by the object upon calling the Move Constructor
  //[PRE]: 'other' has to be a valid object and an !rvalue! reference
  //[POST]: Data is re-assigned
  //[INVOKE]: Move-ctor
  inline void Stockpile::ReassignData(Stockpile&& other) const 
  {
    other.ResourcesMap = ResourcesMap;
  }

  //[DESC]: Resets the data encapsulated by the object upon calling the Move Constructor
  //[PRE]: None
  //[POST]: Data is cleared and the container ('ResourcesMap') is set to a default state
  //[INVOKE]: Move-ctor
  inline void Stockpile::ResetData()
  {
    if(ResourcesMap.empty()) {return;}

    ResourcesMap.clear();
  }

  //[DESC]: Resets the data encapsulated by the object upon calling the Move Assignemnet operator
  //[PRE]: 'other' has to be a valid object and an !rvalue! reference
  //[POST]: Data is swapped
  //[INVOKE]: operator=(...&&)
  inline void Stockpile::SwapData(Stockpile&& other)
  {
    std::swap(other.ResourcesMap, ResourcesMap);
  }

  //[DESC]: Clear the data encapsulated by the object upon the object going out of scope
  //[PRE]: Object has to go out of scope
  //[POST]: Data is cleared
  //[INVOKE]: Dtor
  inline void Stockpile::ClearData() { ResourcesMap.clear(); }

  //[DESC]: Default constructor for the 'Stockpile' class
  //[NOTE]: Only use case should be heap allocations or allocating arrays
  //        Strongly discouraged since it encapsulates an 'std::unordered_map'
  //[PRE]: None
  //[POST]: Object is constructed
  Stockpile::Stockpile() : ResourcesMap() {}

  //[DESC]: Non-Default constructor for the 'Stockpile' class
  //[PARAM]: 'const std::unordered_map<std::string, size_t>& ResourcesMap_' 
  //         Container to be assigned to the encapsulated map
  //[PRE]: Resources map cannot be empty
  //[THROW]: 'std::invalid_argument' if the Map is empty
  //[POST]: Object is constructed
  Stockpile::Stockpile(const std::unordered_map<std::string, size_t>& ResourcesMap_) : ResourcesMap(ResourcesMap_)
  {
    if(ResourcesMap_.empty()) 
    {
      throw std::invalid_argument("[S]Stockpile(...) [Map cannot be empty]");
    }
  }

  //[DESC]: Destructor for the 'Stockpile' class
  //[PRE]: Object has to go out of scope
  //[POST]: Encapsulated data is cleared
  Stockpile::~Stockpile() { ClearData(); }

  //[DESC]: Move Constrctor for the 'Stockpile' class. 
  //        Offers efficient and cheap transfer of ownership
  //[PRE]: 'other' has to be an !rvalue! reference
  //[POST]: Ownership is transfered
  //[THROW]: Tagged as noexcept
  //[PRAGMA GCC diagnostic ignored "-Weffc++"]: 
  //         Pushes the directive onto the stack in order to avoid in-filed initialization compiler warining
  //         Ignored due to "-Werror" flag which treats warnings as errors
#pragma GCC diagnostic push
#pragma GCC diagnostic ignored "-Weffc++"
  Stockpile::Stockpile(Stockpile&& other) noexcept
  {
    ReassignData(std::move(other));
    ResetData();
  }
#pragma GCC diagnostic pop

  //[DESC]: Move Assignment operator for the 'Stockpile' class. 
  //        Offers efficient and cheap transfer of ownership
  //[PRE]: 'other' has to be an !rvalue! reference
  //[POST]: Ownership is transfered
  //[THROW]: Tagged as noexcept
  //[[[nodiscard]]]: Returned reference should not be discared
  [[nodiscard]]Stockpile& Stockpile::operator=(Stockpile&& other) noexcept
  {
    if(this == &other) { return *this; }

    SwapData(std::move(other));

    return *this;
  }

  //[DESC]: Increase the quantity of a resource in the Stockpile
  //        This function increases the quantity of a specific resource in the Stockpile. It performs
  //        various checks to ensure the validity of the operation.
  //[PARAM]: 'NameOfQuantity' The name of the resource to increase
  //[PARAM]: 'NewIncreasedQuantity' The new quantity to set for the resource.
  //
  //[PRE]: The 'Stockpile' object must be properly initialized.
  //[PRE]: The resource with the specified name ('NameOfQuantity') must exist in the 'Stockpile'.
  //[PRE]: 'NewIncreasedQuantity' must be greater than the current quantity of the resource.
  //
  //[POST]: The quantity of the specified resource is updated to 'NewIncreasedQuantity'.
  //
  //[THROW]: 'std::runtime_error' If the specified resource doesn't exist in the Stockpile.
  //[THROW]: 'std::domain_error' If NewIncreasedQuantity is equal to the current quantity.
  //[THROW]: 'std::runtime_error' If NewIncreasedQuantity is less than the current quantity.
  //
  //[RETURN]: 'true' if the quantity was sucessfuly increased, 'false' if otherwise
  bool Stockpile::IncreaseQuantity(const std::string& NameOfResource, const size_t& NewIncreasedQuantity)
  {
    auto it = ResourcesMap.find(NameOfResource);
    size_t InitialQuantityInMap = it -> second;

    if(it == ResourcesMap.end()) 
    { 
      throw std::runtime_error("[S]IncreaseQuantity(...) [Invalid Key, Key does not exist]");
    }

    if(NewIncreasedQuantity < it -> second)
    {
      throw std::runtime_error("[S]IncreaseQuantity(...) [Invalid Quantity Parameter]");
    }

    it -> second = NewIncreasedQuantity;

    if(it -> second >= InitialQuantityInMap) { return true; }
    return false;
  }

  //[DESC]: Decrease the quantity of a resource in the Stockpile
  //        This function decreases the quantity of a specific resource in the Stockpile. It performs
  //        various checks to ensure the validity of the operation.
  //[PARAM]: 'NameOfQuantity' The name of the resource to increase
  //[PARAM]: 'NewDecreasedQuantity' The new quantity to set for the resource.
  //
  //[PRE]: The 'Stockpile' object must be properly initialized.
  //[PRE]: The resource with the specified name ('NameOfQuantity') must exist in the 'Stockpile'.
  //[PRE]: 'NewDecreasedQuantity' must be greater than the current quantity of the resource.
  //
  //[POST]: The quantity of the specified resource is updated to 'NewDecreasedQuantity'.
  //
  //[THROW]: 'std::runtime_error' If the specified resource doesn't exist in the Stockpile.
  //[THROW]: 'std::domain_error' If NewIncreasedQuantity is equal to the current quantity.
  //[THROW]: 'std::runtime_error' If NewIncreasedQuantity is less than the current quantity.
  //
  //[RETURN]: 'true' if the quantity was sucessfuly increased, 'false' if otherwise
  bool Stockpile::DecreaseQuantity(const std::string& NameOfResource, const size_t& NewDecreasedQuantity)
  {
    auto it = ResourcesMap.find(NameOfResource);
    size_t InitialQuantityInMap = it -> second;
    if(it == ResourcesMap.end()) 
    { 
      throw std::runtime_error("[S]IncreaseQuantity(...) [Invalid Key, Key does not exist]");
    }

    if(NewDecreasedQuantity > it -> second)
    {
      throw std::runtime_error("[S]IncreaseQuantity(...) [Invalid Quantity Parameter]");
    }

    it -> second = NewDecreasedQuantity;

    if(it -> second >= InitialQuantityInMap) { return true; }
    return false;
  }

  //[DESC]: Get the quantity of a specific resource in the stockpile.
  //[PRE]: The stockpile should be properly initialized and contain the resource.
  //[POST]: None.
  //[RETURN]: The quantity of the specified resource. If the resource is not found, it returns 0.
  std::size_t Stockpile::GetResourceQuantity(const std::string& Resource)
  {
    if (ResourcesMap.find(Resource) != ResourcesMap.end()) { return ResourcesMap[Resource]; }
    return 0;
  }

  //[DESC]: Check if the stockpile contains a specific resource.
  //[PRE]: The stockpile should be properly initialized.
  //[POST]: None.
  //[RETURN]: True if the stockpile contains the specified resource, false otherwise.
  bool Stockpile::HasResource(const std::string& Resource) const {
    if(ResourcesMap.find(Resource) != ResourcesMap.end()) { return true; }
    return false;
  }
}//[NAMESPACE]: ResourceConversion

