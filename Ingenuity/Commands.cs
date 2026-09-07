namespace Ingenuity;

interface ICommand
{
    char Letter { get; }
    void Execute(World world, Robot robot);
}

public class TurnLeftCommand : ICommand
{
    public char Letter => 'L';

    void ICommand.Execute(World world, Robot robot)
    {
        robot.TurnLeft();
    }
}
public class TurnRightCommand : ICommand
{
    public char Letter => 'R';

    void ICommand.Execute(World world, Robot robot)
    {
        robot.TurnRight();
    }
}
public class MoveForwardCommand : ICommand
{
    public char Letter => 'F';

    void ICommand.Execute(World world, Robot robot)
    {
        robot.MoveForward(world);
    }
}

// ---

class CommandSet
{
    public readonly Dictionary<char, ICommand> Commands = new();

    public CommandSet(IEnumerable<ICommand> commands)
    {
        foreach(ICommand c in commands)
        {
            Commands[c.Letter] = c;
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

    public bool IsKnown(char letter)
    {
        return Commands.ContainsKey(letter);
    }
    public IEnumerable<char> Letters()
    {
        return Commands.Keys;
    }

    public ICommand? GetValue(char letter)
    {
        return Commands.TryGetValue(letter, out ICommand? cmd) ? cmd : null;
    }

    public ICommand this[char letter] => Commands.TryGetValue(letter, out ICommand? cmd)
    ? cmd
    : throw new ArgumentException($"Invalid Command {letter}");
}