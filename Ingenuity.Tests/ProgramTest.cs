using Ingenuity;

namespace Ingenuity.Tests;

public class RobotTest
{
    [Fact]
    public void LostRobot_LeavesScentAtLastSafeCoordinates_NotOffWorld()
    {
        var world = new World(5, 3);
        var robot = new Robot(3, 3, Orientation.N);

        robot.MoveForward(world);

        Assert.True(robot.IsLost);
        Assert.Equal((3, 3), (robot.X, robot.Y));
        Assert.True(world.HasScent(3, 3));
        Assert.False(world.HasScent(3, 4));
    }

    [Fact]
    public void SampleInput_ProducesSampleOutput()
    {
        var world = new World(5, 3);
        var inputs = new []
        {
            (new Robot(1, 1, Orientation.E), "RFRFRFRF"),
            (new Robot(3, 2, Orientation.N), "FRRFLLFFRRFLL"),
            (new Robot(0, 3, Orientation.W), "LLFFFLFLFL"),
        };

        var results = new List<string>();
        foreach(var (robot, commands) in inputs)
        {
            MissionControl.Navigate(world, robot, commands);
            results.Add(robot.ToString());
        }

        Assert.Equal(new [] {"1 1 E", "3 3 N LOST", "2 3 S"}, results);
    }
}
