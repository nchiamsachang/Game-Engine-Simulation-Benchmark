// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace GameEngineSimulation
{
    // ENTITY CLASS
    // Represents a single game object with position and velocity
    // Using a class (reference type) - could also use struct (value type) for better performance
    class Entity
    {
        // Position coordinates
        public double X { get; set; }
        public double Y { get; set; }

        // Velocity (speed and direction)
        public double VX { get; set; }
        public double VY { get; set; }

        // Constructor: Initialize entity with random position and velocity
        public Entity(Random random)
        {
            X = random.NextDouble() * 1000;   // Random X position: 0-1000
            Y = random.NextDouble() * 1000;   // Random Y position: 0-1000
            VX = random.NextDouble() * 2 - 1; // Random X velocity: -1 to 1
            VY = random.NextDouble() * 2 - 1; // Random Y velocity: -1 to 1
        }

        // UPDATE METHOD
        // Called each frame to move the entity based on its velocity
        public void Update()
        {
            X += VX;  // Move in X direction
            Y += VY;  // Move in Y direction

            // BOUNDARY WRAPPING
            // Keep entities within bounds by wrapping around edges
            if (X < 0) X += 1000;
            if (X > 1000) X -= 1000;
            if (Y < 0) Y += 1000;
            if (Y > 1000) Y -= 1000;
        }
    }

    // MAIN SIMULATION CLASS
    class GameSimulation
    {
        static void Main(string[] args)
        {
            // CONFIGURATION
            const int ENTITY_COUNT = 10000;  // Number of entities to simulate
            const int FRAME_COUNT = 1000;    // Number of update frames to run

            Console.WriteLine("C# Game Engine Simulation");
            Console.WriteLine($"Entities: {ENTITY_COUNT:N0}");
            Console.WriteLine($"Frames: {FRAME_COUNT:N0}");
            Console.WriteLine($"Total Updates: {(long)ENTITY_COUNT * FRAME_COUNT:N0}");
            Console.WriteLine();

            // ENTITY CREATION PHASE
            // Create all entities and store in a List (dynamic array)
            Random random = new Random(42);  // Fixed seed for reproducibility
            List<Entity> entities = new List<Entity>(ENTITY_COUNT);

            Console.WriteLine("Creating entities...");
            Stopwatch creationTimer = Stopwatch.StartNew();

            for (int i = 0; i < ENTITY_COUNT; i++)
            {
                entities.Add(new Entity(random));
            }

            creationTimer.Stop();
            Console.WriteLine($"Creation time: {creationTimer.ElapsedMilliseconds} ms");
            Console.WriteLine();

            // SIMULATION PHASE
            // Main game loop: update all entities for multiple frames
            Console.WriteLine("Running simulation...");
            Stopwatch simulationTimer = Stopwatch.StartNew();

            // Outer loop: iterate through frames
            for (int frame = 0; frame < FRAME_COUNT; frame++)
            {
                // Inner loop: update each entity in this frame
                // This is the performance-critical section
                foreach (Entity entity in entities)
                {
                    entity.Update();
                }

                // Optional: Print progress every 100 frames
                if ((frame + 1) % 100 == 0)
                {
                    Console.WriteLine($"  Frame {frame + 1}/{FRAME_COUNT}");
                }
            }

            simulationTimer.Stop();

            // RESULTS
            Console.WriteLine();
            Console.WriteLine("=== RESULTS ===");
            Console.WriteLine($"Total simulation time: {simulationTimer.ElapsedMilliseconds} ms");
            Console.WriteLine($"Time per frame: {simulationTimer.ElapsedMilliseconds / (double)FRAME_COUNT:F3} ms");
            Console.WriteLine($"Updates per second: {((long)ENTITY_COUNT * FRAME_COUNT) / (simulationTimer.ElapsedMilliseconds / 1000.0):N0}");

            // MEMORY USAGE (approximate)
            long memoryUsed = GC.GetTotalMemory(false) / 1024 / 1024;
            Console.WriteLine($"Approximate memory used: {memoryUsed} MB");

            // Sample entity positions (verify simulation ran correctly)
            Console.WriteLine();
            Console.WriteLine("Sample entity final positions:");
            for (int i = 0; i < Math.Min(3, entities.Count); i++)
            {
                Console.WriteLine($"  Entity {i}: ({entities[i].X:F2}, {entities[i].Y:F2})");
            }
        }
    }
}
