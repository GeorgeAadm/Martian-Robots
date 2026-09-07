namespace Ingenuity;

interface ICommand
{
    char Letter { get; }
    void Execute(World world, Robot robot);
}

class TurnLeftCommand : ICommand
{
    public char Letter => 'L';

    public void Execute(World world, Robot robot)
    {
        robot.TurnLeft();
    }
}
class TurnRightCommand : ICommand
{
    public char Letter => 'R';

    public void Execute(World world, Robot robot)
    {
        robot.TurnRight();
    }
}
class MoveForwardCommand : ICommand
{
    public char Letter => 'F';

    public void Execute(World world, Robot robot)
    {
        robot.MoveForward(world);
    }
}

/* No Implementation - Example Only!

class MoveBackwardCommand: ICommand
{
    public char Letter => 'B';

    public void Execute(World world, Robot robot)
    {
        robot.TurnLeft();
        robot.TurnLeft();
        robot.MoveForward(world);
        robot.TurnRight();
        robot.TurnRight();
    }
}
*/


class CommandSet
{
    readonly Dictionary<char, ICommand> Commands = new();

    public CommandSet(IEnumerable<ICommand> commands)
    {
        foreach(ICommand c in commands)
        {
            Commands.Add(c.Letter, c);
        }        
    }

    public static CommandSet Default()
    {
        var commands = new List<ICommand>{
            new TurnLeftCommand(), 
            new TurnRightCommand(), 
            new MoveForwardCommand()
            }; 
        return new CommandSet(commands);
    }


    public bool IsKnown(char letter) => Commands.ContainsKey(letter);
    public IEnumerable<char> Letters() => Commands.Keys;

    public ICommand? GetValue(char letter) => Commands.TryGetValue(letter, out ICommand? cmd) ? cmd : null;

    public ICommand this[char letter] => Commands.TryGetValue(letter, out ICommand? cmd)
    ? cmd
    : throw new ArgumentException($"Invalid Command {letter}");
}