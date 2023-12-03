// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

string inputDir = "F:\\Documents\\GitHub\\Aoc2023\\input";

string inputFile = "day3.txt";

string inputPath = Path.Combine(inputDir, inputFile);


// part 1
long total = 0;

// part 2 
Dictionary<Point, List<int>> GearNeibhours = new();

char[,] map = Utilities.ReadInput(inputPath);

for (int y = 1; y < map.GetLength(1) - 1; y++)
{
    bool hasMarker = false;
    bool hasGearMarker = false;

    StringBuilder possible = new StringBuilder();
    //List<Point> possibleGears = new List<Point>();

    for (int x = 1; x < map.GetLength(0) - 1; x++)
    {
        var current = map[x, y];

        if (Utilities.IsDigit(current))
        {
            // possible candidate
            possible.Append(map[x, y]);

            var neighbors = Utilities.GetNeighbors(map, x, y);

            // remove all digits and . from neighbors
            neighbors = neighbors.Where(ch => !Utilities.IsDigit(ch) && !ch.Equals('.')).ToArray();

            if (neighbors.Any())
            {
                hasMarker = true;
            }

            if (neighbors.Contains('*'))
            {
                hasGearMarker = true;
            }
        }
        else
        {
            // no longer a number
            // check if we have a marker
            if (hasMarker)
            {
                // convert possible to number
                total += long.Parse(possible.ToString());
            }

            // if (hasGearMarker)
            // {
            //     PossibleGear gear = new PossibleGear { Number = int.Parse(possible.ToString()), Points = possibleGears };
            // }

            // reset
            possible.Clear();
            hasMarker = false;

        }
    }

    // end of line - no longer a number
    // check if we have a marker
    if (hasMarker)
    {
        // convert possible to number
        total += long.Parse(possible.ToString());

    }

    // reset
    possible.Clear();
    hasMarker = false;
}

// Utilities.PrintMap(map);

Console.WriteLine($"Day 1 : {total}");


// public class PossibleGear
// {
//     public int Number { get; set; }
//     public List<Point> Points { get; set; } = new();
// }

public struct Point(int x, int y)
{
    public int x = x;
    public int y = y;
}

public class Utilities
{
    // read file and build 2d array
    // with padding on the borders using a . character
    // to simplify boundary checks
    public static char[,] ReadInput(string path)
    {
        string[] lines = System.IO.File.ReadAllLines(path);
        int width = lines[0].Length + 2;
        int height = lines.Length + 2;
        char[,] map = new char[width, height];
        for (int y = 0; y < height; y++)
        {
            map[0, y] = '.';
            map[width - 1, y] = '.';
        }

        for (int x = 0; x < width; x++)
        {
            map[x, 0] = '.';
            map[x, height - 1] = '.';
        }

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[x + 1, y + 1] = lines[y][x];
            }
        }
        return map;
    }

    // find all neighbors of a point
    /*
     * ###
     * #x#
     * ###
     */
    public static char[] GetNeighbors(char[,] map, int x, int y)
    {
        char[] neighbors = new char[8];
        neighbors[0] = map[x-1, y-1];
        neighbors[1] = map[x-1, y];
        neighbors[2] = map[x-1, y+1];

        neighbors[3] = map[x, y-1];
        neighbors[4] = map[x, y+1];
        
        neighbors[5] = map[x+1, y-1];
        neighbors[6] = map[x+1, y];
        neighbors[7] = map[x+1, y+1];
        return neighbors;
    }

    // print 2d array
    public static void PrintMap(char[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            string line = "";
            for (int x = 0; x < width; x++)
            {
                line += map[x, y];
            }
            Console.WriteLine(line);
        }
    }

    public static bool IsDigit(char c)
    {
        return c is >= '0' and <= '9';
    }
}