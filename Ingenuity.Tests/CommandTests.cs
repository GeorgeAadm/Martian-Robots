namespace Ingenuity.Tests;

public class CommandTests
{
    private static World TestWold = new World(3, 5);

    [Theory]
    [InlineData(Orientation.N, Orientation.W)]
    [InlineData(Orientation.W, Orientation.S)]
    [InlineData(Orientation.S, Orientation.E)]
    [InlineData(Orientation.E, Orientation.N)]
    public void TurnLeft_RotatesAnitClockwise(Orientation from, Orientation expeccted)
    {
        var robot = new Robot(1,1,  from);
        ICommand cmd = new TurnLeftCommand();

        cmd.Execute(TestWold, robot);

        Assert.Equal(expeccted, robot.orientation);
        Assert.Equal('L', cmd.Letter);
        Assert.Equal((1, 1), (robot.X, robot.Y)); // robot stays put
    }

    [Fact]
    public void DuplicateCommandLetter_ThrowsException()
    {
        var commands = new List<ICommand>{
            new TurnLeftCommand(), 
            new TurnLeftCommand()
            }; 
        Assert.ThrowsAny<ArgumentException>(
            () => new CommandSet(commands));
    }

    [Fact]
    public void OnlyUse_AvailableCommands()
    {
        var commands = new List<ICommand>{
            new TurnRightCommand()
            }; 
        var set = new CommandSet(commands);
        var robot = new Robot(0, 0, Orientation.N);

        Navigator.Run(TestWold, robot, "RR", set);
        Assert.Equal(Orientation.S, robot.orientation);

        Assert.ThrowsAny<ArgumentException>(() => Navigator.Run(TestWold, robot, "F", set));
    }

}