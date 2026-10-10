# Boundary tests

The benchmark moves each entity by its velocity and wraps it to the other side when it
leaves the 1000 × 1000 area. These tests pin down exactly what "wraps" means at the edges,
and check that the C# and Python versions do the same thing.

## The convention the code implements

Both versions contain the same four lines (`Program.cs`, `Minigame.py`):

```
if x < 0:     x += 1000
if x > 1000:  x -= 1000        (and the same for y)
```

So, after the move:

- a coordinate **below 0** has 1000 added; a coordinate **above 1000** has 1000 subtracted;
- **exactly 0 and exactly 1000 are both left alone.** The area is the closed range 0 to
  1000 on each axis, so its two opposite edges are separate positions;
- the adjustment is made **once per update**. A coordinate more than 1000 outside the area
  is moved 1000 closer, not all the way back. The benchmark never creates one: entities
  start between 0 and 1000 and move at most 1 per update.

The tests describe this behaviour as it is. Nothing in the benchmark was changed.

## One set of cases, two languages

`tests/wrap_cases.json` holds 17 cases (start position, velocity, expected position).
Both test suites read that file, so they cannot drift apart:

| Group | Cases |
|---|---|
| Inside the area | 1 |
| Crossing the right, left, bottom and top edges; a corner; one axis only | 6 |
| Exact boundary: landing on 0 or 1000, resting on them, stepping off them | 6 |
| Negative and just-out-of-range coordinates, including a negative too small to change 1000 | 3 |
| More than one width outside (one wrap per update) | 1 |

Every number in the file is exactly representable as a double, so the tests compare with
`==`, not with a tolerance.

Each suite also has one test that runs the benchmark's own setup (seed 42, 200 entities,
2,500 updates) and checks that no entity ever leaves the area.

## Running them

From the repository folder.

```
dotnet test
python -m unittest discover -s python -v
```

The C# tests need the .NET 8 SDK and, the first time, a download of the xUnit packages.
The Python tests need only Python 3 (use `py` in place of `python` if that is how Python
starts on your machine; the results below were run with `py`).

## Results, 2026-10-09

Commit `74097c6` plus the additions that came with these tests: the test project
(`C#-Minigame.Tests/`), `python/test_wraparound.py`, `tests/wrap_cases.json`, one
`InternalsVisibleTo` line in `C#-Minigame/Minigame.csproj` so the tests can see the
`Entity` class, and the test project's entry in `Minigame.sln`.

| Suite | Environment | Result |
|---|---|---|
| C# (xUnit 2.4.2) | .NET SDK 8.0.101, Windows 11 | 19 passed, 0 failed (17 shared cases, 1 check that all 17 were loaded, 1 never-leaves-the-area test) |
| Python (unittest) | Python 3.13, Windows 11 | 2 tests passed: the 17 shared cases as sub-tests, and the never-leaves-the-area test |

**The two languages agree on all 17 cases.** No discrepancy was found, and the benchmark's
behaviour was not changed.

The benchmark timings in `README.md` were not re-measured.
