"""Boundary tests for Entity.update(), from the cases shared with the C# tests.

Run from the repository folder (no extra packages needed):

    python -m unittest discover -s python -v
"""
import json
import os
import random
import unittest

from Minigame import Entity

CASES_FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "tests", "wrap_cases.json")

with open(CASES_FILE, encoding="utf-8") as f:
    CASES = json.load(f)["cases"]


def moved(case):
    """An entity placed and given a velocity as the case says, after one update."""
    entity = Entity(random.Random(0))
    entity.x, entity.y, entity.vx, entity.vy = case["x"], case["y"], case["vx"], case["vy"]
    entity.update()
    return entity


class WraparoundTests(unittest.TestCase):
    def test_shared_cases(self):
        self.assertEqual(len(CASES), 17)
        for case in CASES:
            with self.subTest(case["name"]):
                entity = moved(case)
                # exact comparison: every value in the cases is exactly representable
                self.assertEqual(entity.x, case["expect_x"])
                self.assertEqual(entity.y, case["expect_y"])

    def test_an_entity_started_inside_the_area_never_leaves_it(self):
        # the benchmark's own setup: positions 0..1000 and speeds -1..1
        generator = random.Random(42)
        entities = [Entity(generator) for _ in range(200)]
        for _ in range(2500):       # long enough for the fastest entities to cross the area
            for entity in entities:
                entity.update()
                self.assertTrue(0 <= entity.x <= 1000 and 0 <= entity.y <= 1000)


if __name__ == "__main__":
    unittest.main()
