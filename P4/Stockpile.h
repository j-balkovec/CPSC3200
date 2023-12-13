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
//[INVARIANT]: The 'ResourcesMap' member variable is a non-null std::unordered_map containing resource names (keys) 
//             and their corresponding non-negative integer quantities (values).
//[INVARIANT]: When the class is constructed or initialized, the 'ResourcesMap' contains valid resource name-quantity pairs.
//[INVARIANT]: The ResourcesMap is modified only through the IncreaseQuantity and DecreaseQuantity 
//             member functions, ensuring that resource quantities remain non-negative.
//[INVARIANT]: 'NewQuantities' should always be non-negative or the size_t(unsigned long) will overflow
//[INVARIANT]: The IncreaseQuantity and DecreaseQuantity member functions do not introduce any 
//             resource names that are not present in the ResourcesMap.
//[INVARIANT]: After any operation (construction, move, etc.), the 'ResourcesMap' remains in a consistent and valid state.
//[INVARIANT]: CompletedArray is never null
//
//[MOVE SEMANTICS]
//{
// Stockpile Obj1(...); -> Initial Construction
//
// Stockpile Obj2 = Stockpile(std::move(Obj1)) -> Move Ctor
// Stockpile Obj3 = std::move(Obj2) -> Move Assignment Operator

//[DEPENDENCIES]:
//       [INTERNAL]:
//          - Member, Helper, Utility methods
//          - Private Data Members
//
//        [EXTERNAL]:
//          - 'std::unordered_map' - {SEE [<unordered_map>]}
//
//[NAMESPACE]: Might be deemed unnecessary, but despite the distinctive function names, global
//             namespace pollution is still in the picture.
//
//            - {ResourceConversion} Encapsulates the 'Formula', 'Plan', 'ExecutablePlan', and 'Stockpile' class.
#ifndef Stockpile_h
#define Stockpile_h

#include <memory>
#include <string>
#include <unordered_map>

//Client fills the map with appropriaet valeus -> exact copy
namespace ResourceConversion
{
  class Stockpile
  {
    private:
    std::unordered_map<std::string, size_t> ResourcesMap;

    inline void ReassignData(Stockpile&& other) const;
    inline void ResetData();
    inline void SwapData(Stockpile&& other);
    inline void ClearData();

    //[NOTE]: Copying is suppressed
    Stockpile(const Stockpile& other) = delete;
    Stockpile& operator=(const Stockpile& other) = delete;

    public:

    explicit Stockpile();
    Stockpile(const std::unordered_map<std::string, size_t>& ResourcesMap_);
    ~Stockpile();
    Stockpile(Stockpile&& other) noexcept;
    Stockpile& operator=(Stockpile&& other) noexcept;

    bool IncreaseQuantity(const std::string& NameOfResource, const size_t& NewIncreasedQuantity);
    bool DecreaseQuantity(const std::string& NameOfResource, const size_t& NewDecreasedQuantity);
    std::size_t GetResourceQuantity(const std::string& resource);
    bool HasResource(const std::string& resource) const; 

    //[NOTE]: Calling this function is expensive. 
    //        It should only be called when the 
    //        'ResourcesMap' needs to be used
    [[nodiscard]]inline std::unordered_map<std::string, size_t> GetResourcesMap() {return ResourcesMap;}
  };
}//[NAMESPACE]: ResourceConversion
#endif /*Stockpile_h*/