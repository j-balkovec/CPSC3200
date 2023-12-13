CXX = g++
CXXFLAGS = -std=c++17 -Wall -Wextra -pedantic -Werror -Wshadow -Wconversion -Wuninitialized -Wunused -Wreorder -Woverloaded-virtual -Weffc++ -Wno-unused-parameter

DEBUG_FLAGS = -g -Og

#-Ofast -> caution
RELEASE_FLAGS = -O3 -funroll-loops -ffast-math

SRCFILES = Plan.cpp Formula.cpp ExecutablePlan.cpp P4.cpp Stockpile.cpp

EXECUTABLE = main

OBJFILES = $(SRCFILES:.cpp=.o)

all: debug

debug: CXXFLAGS += $(DEBUG_FLAGS)
debug: $(EXECUTABLE)

release: CXXFLAGS += $(RELEASE_FLAGS)
release: $(EXECUTABLE)

$(EXECUTABLE): $(OBJFILES)
	@echo "Linking $(EXECUTABLE)..."
	@$(CXX) $(CXXFLAGS) $^ -o $@
	@echo "Build complete: $(EXECUTABLE)"

%.o: %.cpp
	@echo "Compiling $<..."
	@$(CXX) $(CXXFLAGS) -c $< -o $@

clean:
	@echo "Cleaning..."
	@rm -f $(EXECUTABLE) $(OBJFILES)
	@echo "Clean complete"

run: all
	./$(EXECUTABLE)

.PHONY: all debug release clean



