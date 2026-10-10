using System.Text.Json;

namespace GameEngineSimulation.Tests
{
    // Boundary tests for Entity.Update(), from the cases shared with the Python tests
    // (tests/wrap_cases.json).  Run from the repository folder:  dotnet test
    public class WraparoundTests
    {
        // name, x, y, vx, vy, expected x, expected y
        public static IEnumerable<object[]> SharedCases()
        {
            using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "wrap_cases.json")));
            List<object[]> cases = new List<object[]>();
            foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
            {
                cases.Add(new object[]
                {
                    c.GetProperty("name").GetString()!,
                    c.GetProperty("x").GetDouble(), c.GetProperty("y").GetDouble(),
                    c.GetProperty("vx").GetDouble(), c.GetProperty("vy").GetDouble(),
                    c.GetProperty("expect_x").GetDouble(), c.GetProperty("expect_y").GetDouble(),
                });
            }
            return cases;
        }

        [Fact]
        public void EveryCaseInTheSharedFileIsRun()
        {
            Assert.Equal(17, SharedCases().Count());
        }

        [Theory]
        [MemberData(nameof(SharedCases))]
        public void SharedCase(string name, double x, double y, double vx, double vy, double expectX, double expectY)
        {
            Entity entity = new Entity(new Random(0)) { X = x, Y = y, VX = vx, VY = vy };

            entity.Update();

            // exact comparison: every value in the cases is exactly representable
            Assert.True(entity.X == expectX, $"{name}: X is {entity.X:R}, expected {expectX:R}");
            Assert.True(entity.Y == expectY, $"{name}: Y is {entity.Y:R}, expected {expectY:R}");
        }

        [Fact]
        public void AnEntityStartedInsideTheAreaNeverLeavesIt()
        {
            // the benchmark's own setup: positions 0..1000 and speeds -1..1
            Random random = new Random(42);
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < 200; i++)
                entities.Add(new Entity(random));

            for (int frame = 0; frame < 2500; frame++)      // long enough for the fastest entities to cross the area
            {
                foreach (Entity entity in entities)
                {
                    entity.Update();
                    Assert.True(entity.X >= 0 && entity.X <= 1000 && entity.Y >= 0 && entity.Y <= 1000);
                }
            }
        }
    }
}
