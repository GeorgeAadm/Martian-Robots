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

    public override string ToString()
    {
        return $"{X} {Y} {orientation}" + (IsLost? " LOST": "");
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
        bool feed = true;

        while (feed)
        {
            Console.Write($"Robot {journeys.Count +1} position x y N|E|S|W ");
            string? position = Console.ReadLine();

            if(String.IsNullOrWhiteSpace(position)) feed=false;

            Console.Write("Instructions L|R|F ");
            string? instructions = Console.ReadLine();

            if(!TryParseRobot(position, instructions, out Robot robot, out string commands))
            {
                Console.Write("Error");
            }

            // compute journy for robot here

            journeys.Add(robot);
        }
        
        foreach(Robot r in journeys)
        Console.WriteLine(r);
    }




    static World ReadWorld()
    {
        Console.Write("Enter the max coordinates. Upper Right (x:y)");
        string? line = Console.ReadLine();
        if(String.IsNullOrWhiteSpace(line)) Console.Write("No input!");
        
        string[] parts = line.Split(' ');
        if(parts.Length == 2 && int.TryParse(parts[0],out int x) && int.TryParse(parts[1],out int y))
        {
            return new World(x, y);
        }
        // handle err better 
        else{ throw new InvalidOperationException(); }
    }

    static bool TryParseRobot(string position, string instructions, out Robot robot, out string commands)
    {
        robot = null;
        commands = "";
        
        string[] parts = position.Split(' ');
        if(parts.Length != 3 
        || int.TryParse(parts[0],out int x) 
        || int.TryParse(parts[1],out int y)
        || !Enum.TryParse(parts[2], out Orientation o))
        {
            return false; // position must be [x y N|E|S|W]
        }
        if (x is <0 or >MaxCoordinate || y is <0 or >MaxCoordinate)
        {
            return false; // Coordinate must range from 0 to Max   
        }
        commands = instructions.Trim().ToUpper();
        if(commands.Length > MaxInstructionLength || commands.Any(c => "LRF".Contains(c)))
        {
            return false; // only valid commands L|R|F - under {MaxInstructionLength} chars
        }

        robot = new Robot(x, y, o);
        return true;
    }


}

