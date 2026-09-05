namespace Ingenuity;

enum Orientation { N, E, S, W }

class World
{
    public int MxX {get;}
    public int MxY {get;}
    
    public World(int maxX, int maxY)
    {
        MxX = maxX;
        MxY = maxY;
    }

    public bool Contains(int x, int y)
    {
        return x >= 0 && y >= 0 && x <= MxX && y <= MxY;
    }
}
class Robot
{
    public int X {get; private set;}
    public int Y {get; private set;}
    public Orientation orientation {get; private set;}
    public bool IsLost {get; private set;}

    public Robot(int x, int y, Orientation o)
    {
        X = x;
        Y = y;
        orientation = o;
    }

    //TODO: test orientation
    public void TurnRight() => orientation = (Orientation)(((int)orientation +1) % 4);
    public void TurnLeft() => orientation = (Orientation)(((int)orientation +3) % 4);

    public void MoveForward(World world)
    {
        (int dx, int dy) = orientation switch
        {
            Orientation.N => (0, 1),
            Orientation.E => (1, 0),
            Orientation.S => (0, -1),
            Orientation.W => (-1, 0),
            _ => throw new InvalidOperationException()
        };
        int nx = X + dx, ny = Y + dy;
        
        if(world.Contains(nx, ny)) // still on map
        {
            X = nx;
            Y = ny;
        }

    }

    public override string ToString()
    {
        return $"{X} {Y} {orientation}" + (IsLost? " LOST": "");
    }    
}
static class MissionControl
{
    public static void Navigate(World world, Robot robot, string commands)
    {
        foreach(char c in commands)
        {
            if (robot.IsLost) break;   

            switch(c)
            {
                case 'L' : robot.TurnLeft(); break;
                case 'R' : robot.TurnRight(); break;
                case 'F' : robot.MoveForward(world); break;
                default : throw new ArgumentException($"Invalid Command: {c}.");
            }
        }
    }
}
class Program
{
    const int MaxCoordinate = 50;
    const int MaxInstructionLength = 100;

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Mars!");
        World world = ReadWorld();
        var journeys = new List<Robot>();
        
        Console.WriteLine("Enter empty record to finish.");
        while (true)
        {
            
            Console.Write($"Robot {journeys.Count +1} position x y (N|E|S|W) :");
            string? position = Console.ReadLine();

            Console.Write("Instructions (L|R|F) :");
            string? instructions = Console.ReadLine();

            if(String.IsNullOrWhiteSpace(position) || String.IsNullOrWhiteSpace(instructions)) break;

            if(!TryParseRobot(position, instructions, out Robot robot, out string commands, out string err))
            {
                Console.WriteLine(err);
                continue; // load next robot
            }

            // Plot robot journey on world-map 
            MissionControl.Navigate(world, robot, commands);
            journeys.Add(robot);
        }
        
        foreach(Robot r in journeys)
        Console.WriteLine(r);
    }




    static World ReadWorld()
    {
        while(true)
        {
            Console.Write("Enter the grid size coordinates, Upper Right (x y) :");
            string? line = Console.ReadLine();
            if(String.IsNullOrWhiteSpace(line)) throw new InvalidOperationException("No input!");
            
            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length == 2 
            && int.TryParse(parts[0],out int x) 
            && int.TryParse(parts[1],out int y)
            && x is >=0 and <MaxCoordinate
            && y is >=0 and <MaxCoordinate)
            {
                return new World(x, y);
            }
            
            Console.WriteLine($"Expecting two integer values between 0 and {MaxCoordinate}.");            
        }
    }

    static bool TryParseRobot(string position, string instructions, out Robot robot, out string commands, out string error)
    {
        robot = null!;
        commands = "";
        error = "";
        
        string[] parts = position.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if(parts.Length != 3 
        || !int.TryParse(parts[0],out int x) 
        || !int.TryParse(parts[1],out int y)
        || !Enum.TryParse(parts[2], ignoreCase: true, out Orientation o))
        {
            error = "Robot position must be formatted: (x-coordinate) (y-coordinate) (orientation).\nSeperated with spaces and use only one char (N|E|S|W) for Orientation.\n Please try again.";
            return false; 
        }
        if (x is <0 or >MaxCoordinate || y is <0 or >MaxCoordinate)
        {
            error = $"Coordinate must range from 0 to {MaxCoordinate}.";
            return false;   
        }
        commands = instructions.Trim().ToUpper();
        if(commands.Length > MaxInstructionLength || commands.Any(c => !"LRF".Contains(c)))
        { 
            error = "Invalid commands. Use only L|R|F characters. Max number of commands {MaxInstructionLength}.";
            return false;
        }

        robot = new Robot(x, y, o);
        return true;
    }


}

