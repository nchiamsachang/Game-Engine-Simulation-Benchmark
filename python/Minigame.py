
import time
import random
import sys

# ENTITY CLASS
# Represents a single game object with position and velocity
# Python classes are reference types (objects stored on heap)
class Entity:
    """
    A simple game entity with position and velocity.
    Demonstrates Python's OOP capabilities and dynamic typing.
    """
    
    # CONSTRUCTOR (__init__)
    # Called when creating new Entity instances
    # Note: 'self' is explicit in Python (unlike 'this' being implicit in C#)
    def __init__(self, random_gen):
        """Initialize entity with random position and velocity"""
        # Dynamic typing: no type declarations needed
        # All numbers are objects (more overhead than C# primitives)
        self.x = random_gen.random() * 1000    # Random X: 0-1000
        self.y = random_gen.random() * 1000    # Random Y: 0-1000
        self.vx = random_gen.random() * 2 - 1  # Random VX: -1 to 1
        self.vy = random_gen.random() * 2 - 1  # Random VY: -1 to 1
    
    # UPDATE METHOD
    # Called each frame to move entity based on velocity
    def update(self):
        """Update entity position based on velocity"""
        # Arithmetic operations on float objects
        # Each operation involves Python interpreter overhead
        self.x += self.vx
        self.y += self.vy
        
        # BOUNDARY WRAPPING
        # Keep entities within bounds (wrap around edges)
        if self.x < 0:
            self.x += 1000
        if self.x > 1000:
            self.x -= 1000
        if self.y < 0:
            self.y += 1000
        if self.y > 1000:
            self.y -= 1000


# MAIN SIMULATION FUNCTION
def run_simulation():
    """Run the game engine simulation and measure performance"""
    
    # CONFIGURATION
    ENTITY_COUNT = 10000   # Number of entities to simulate
    FRAME_COUNT = 1000     # Number of frames to update
    
    print("Python Game Engine Simulation")
    print(f"Entities: {ENTITY_COUNT:,}")
    print(f"Frames: {FRAME_COUNT:,}")
    print(f"Total Updates: {ENTITY_COUNT * FRAME_COUNT:,}")
    print()
    
    # ENTITY CREATION PHASE
    # Create list (dynamic array) to hold all entities
    # Python lists are flexible but have more overhead than C# List<T>
    random_gen = random.Random(42)  # Fixed seed for reproducibility
    entities = []
    
    print("Creating entities...")
    creation_start = time.perf_counter()  # High-resolution timer
    
    # List comprehension (Pythonic way to create lists)
    # Alternative to explicit for loop
    entities = [Entity(random_gen) for _ in range(ENTITY_COUNT)]
    
    creation_time = time.perf_counter() - creation_start
    print(f"Creation time: {creation_time * 1000:.0f} ms")
    print()
    
    # SIMULATION PHASE
    # Main game loop: update all entities for multiple frames
    print("Running simulation...")
    simulation_start = time.perf_counter()
    
    # Outer loop: iterate through frames
    # range() creates iterator (memory efficient)
    for frame in range(FRAME_COUNT):
        # Inner loop: update each entity in this frame
        # This is the PERFORMANCE-CRITICAL section
        # Python's dynamic dispatch and interpreted nature will be slower here
        for entity in entities:
            entity.update()
        
        # Progress reporting every 100 frames
        if (frame + 1) % 100 == 0:
            print(f"  Frame {frame + 1}/{FRAME_COUNT}")
    
    simulation_time = time.perf_counter() - simulation_start
    
    # RESULTS
    print()
    print("=== RESULTS ===")
    print(f"Total simulation time: {simulation_time * 1000:.0f} ms")
    print(f"Time per frame: {simulation_time * 1000 / FRAME_COUNT:.3f} ms")
    print(f"Updates per second: {(ENTITY_COUNT * FRAME_COUNT) / simulation_time:,.0f}")
    
    # MEMORY USAGE
    # Python's sys.getsizeof() only shows size of list container, not contents
    # Actual memory usage is much higher due to object overhead
    entity_memory = sys.getsizeof(entities) / 1024 / 1024
    print(f"List container memory: {entity_memory:.2f} MB")
    print(f"Note: Actual memory usage is higher (Python object overhead)")
    
    # Sample entity positions (verify simulation ran correctly)
    print()
    print("Sample entity final positions:")
    for i in range(min(3, len(entities))):
        print(f"  Entity {i}: ({entities[i].x:.2f}, {entities[i].y:.2f})")


# ENTRY POINT
# Python's standard way to check if script is run directly (not imported)
if __name__ == "__main__":
    run_simulation()

