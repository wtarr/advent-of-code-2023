// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

string inputDir = "F:\\Documents\\GitHub\\Aoc2023\\input";

string inputFile = "day3.txt";

string inputPath = Path.Combine(inputDir, inputFile);

char[,] map = Utilities.ReadInput(inputPath);

// part 1
long part1Answer = 0;

// part 2 
Dictionary<Point, List<int>> gearNeighborCache = new();
long part2Answer = 0;


for (int y = 1; y < map.GetLength(1) - 1; y++)
{
    bool hasAnyAdjacentSymbolMarker = false;
    bool hasAdjacentGearMarker = false;

    StringBuilder possible = new StringBuilder();
    Point gearPos = new Point(-1, -1);

    for (int x = 1; x < map.GetLength(0) - 1; x++)
    {
        var current = map[x, y];
        var neighbors = Utilities.GetNeighbors(map, x, y);

        foreach (var n in neighbors)
        {
            if (n.Value == '*')
            {
                if (!gearNeighborCache.ContainsKey(new Point(n.X, n.Y)))
                {
                    gearNeighborCache.Add(new Point(n.X, n.Y), new List<int>());
                }
            }
        }

        if (Utilities.IsDigit(current))
        {
            // possible candidate
            possible.Append(map[x, y]);

            // remove all digits and . from neighbors
            neighbors = neighbors.Where(ch => !Utilities.IsDigit(ch.Value) && !ch.Value.Equals('.')).ToArray();

            if (neighbors.Any())
            {
                hasAnyAdjacentSymbolMarker = true;
            }

            // part 2 - check if we have a gear marker
            if (neighbors.Any(ch => ch.Value.Equals('*')))
            {
                var first = neighbors.First(ch => ch.Value.Equals('*')); // this works just because the data is set that way

                hasAdjacentGearMarker = true;
                gearPos = new Point(first.X, first.Y);
            }
        }
        else
        {
            // no longer a number
            // check if we have a marker
            if (hasAnyAdjacentSymbolMarker)
            {
                // convert possible to number
                part1Answer += long.Parse(possible.ToString());
            }

            if (hasAdjacentGearMarker)
            {
                if (gearNeighborCache.ContainsKey(gearPos))
                {
                    gearNeighborCache[gearPos].Add(int.Parse(possible.ToString()));
                }
                else
                {
                    gearNeighborCache.Add(gearPos, new List<int> { int.Parse(possible.ToString()) });
                }
            }

            // reset
            possible.Clear();
            hasAnyAdjacentSymbolMarker = false;

            gearPos = new Point(-1, -1);
            hasAdjacentGearMarker = false;
        }
    }

    // end of line - no longer a number
    // check if we have a marker
    if (hasAnyAdjacentSymbolMarker)
    {
        // convert possible to number
        part1Answer += long.Parse(possible.ToString());

    }

    if (hasAdjacentGearMarker)
    {
        if (gearNeighborCache.ContainsKey(gearPos))
        {
            gearNeighborCache[gearPos].Add(int.Parse(possible.ToString()));
        }
        else
        {
            gearNeighborCache.Add(gearPos, new List<int> { int.Parse(possible.ToString()) });
        }
    }
}

// Utilities.PrintMap(map);

// part 1 - answer
Console.WriteLine($"Day 1 : {part1Answer}");


// part 2 - answer
foreach (var gearNeighbour in gearNeighborCache)
{
    if (gearNeighbour.Value.Count > 1)
    {
        var product = gearNeighbour.Value.Aggregate((a, b) => a * b);

        part2Answer += product;
    }
}
Console.WriteLine($"Day 2 : {part2Answer}");


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
    public static Character[] GetNeighbors(char[,] map, int x, int y)
    {
        Character[] neighbors = new Character[8];
        neighbors[0] = new Character(x - 1, y - 1, map[x - 1, y - 1]);
        neighbors[1] = new Character(x - 1, y, map[x - 1, y]);
        neighbors[2] = new Character(x - 1, y + 1, map[x - 1, y + 1]);

        neighbors[3] = new Character(x, y - 1, map[x, y - 1]);
        neighbors[4] = new Character(x, y + 1, map[x, y + 1]);

        neighbors[5] = new Character(x + 1, y - 1, map[x + 1, y - 1]);
        neighbors[6] = new Character(x + 1, y, map[x + 1, y]);
        neighbors[7] = new Character(x + 1, y + 1, map[x + 1, y + 1]);
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

public class Character
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Value { get; set; }

    public Character(int x, int y, char value)
    {
        X = x;
        Y = y;
        Value = value;
    }
}