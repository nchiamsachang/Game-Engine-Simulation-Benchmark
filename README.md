# Game Engine Simulation Benchmark (C# vs Python)

A benchmark by Nathan Chiamsachang and Trey Rajsombath for a Principles of Programming Languages class. The assignment was to compare two programming languages, so this project runs the same game-engine-style update loop in C# and in Python to compare how fast each one handles it.

## What it does

Both versions run the same simulation:

1. Create 10,000 entities, each with a random position (0 to 1000 on X and Y) and a random velocity (-1 to 1 on X and Y).
2. Run 1,000 frames. In each frame, every entity moves by its velocity and wraps around to the other side if it leaves the 1000 x 1000 area.
3. Print how long entity creation and the simulation took, the time per frame, and the number of updates per second.

That is 10,000,000 entity updates per run. The update loop is the part being measured.

## Results

| Measurement           | C#          | Python     |
|-----------------------|-------------|------------|
| Entity creation time  | 15 ms       | 13 ms      |
| Total simulation time | 408 ms      | 5,401 ms   |
| Time per frame        | 0.408 ms    | 5.401 ms   |
| Updates per second    | 24,509,804  | 1,851,616  |

C# ran the simulation about 13 times faster than Python.

These numbers are the median of three runs on one Windows 11 machine, using .NET 8.0 (Release build) and Python 3.13. Across the three runs C# took 343 to 435 ms and Python took 4,822 to 6,466 ms, so expect different numbers on other hardware.

### Why the difference

- C# is compiled to machine code by the .NET JIT, so `X += VX` becomes a few CPU instructions on plain `double` values.
- Python is interpreted. Every `self.x += self.vx` goes through attribute lookups and creates a new float object, and every `entity.update()` is a dynamically dispatched method call.

### Note on the final positions

Both versions seed their random number generator with 42, so each one prints the same final positions every time it runs. The C# and Python positions do not match each other, because the two languages use different random number generators.

## How to run

### C#

Requires the .NET 8 SDK. Open `Minigame.sln` in Visual Studio 2022, switch the configuration to **Release**, and press **Ctrl+F5**. Or from a terminal in the repository folder:

```
dotnet run -c Release --project "C#-Minigame"
```

Use Release, not Debug. Debug builds are not optimized and will give slower times.

### Python

Requires Python 3. No extra packages are needed.

```
python python/Minigame.py
```

## Sample output

```
C# Game Engine Simulation
Entities: 10,000
Frames: 1,000
Total Updates: 10,000,000

Creating entities...
Creation time: 15 ms

Running simulation...
  Frame 100/1000
  ...
  Frame 1000/1000

=== RESULTS ===
Total simulation time: 408 ms
Time per frame: 0.408 ms
Updates per second: 24,509,804
Approximate memory used: 1 MB

Sample entity final positions:
  Entity 0: (919.14, 186.44)
  Entity 1: (617.25, 288.44)
  Entity 2: (642.83, 275.89)
```

## Files

| File                          | Purpose                          |
|-------------------------------|----------------------------------|
| `C#-Minigame/Program.cs`      | C# version of the simulation     |
| `C#-Minigame/Minigame.csproj` | C# project file (.NET 8)         |
| `Minigame.sln`                | Visual Studio solution           |
| `python/Minigame.py`          | Python version of the simulation |
