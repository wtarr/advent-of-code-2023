// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var directory = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day8.txt";

var lines = File.ReadAllLines(Path.Combine(directory, input));

var path = new Queue<char>();

var instructions = lines.First();

foreach (var p in instructions.Trim())
{
    path.Enqueue(p);
}

var graph = new Dictionary<string, Node>();

foreach (var line in lines.Skip(2))
{
    // AAA = (BBB, CCC)
    var parts = line.Split(" = ");
    var name = parts[0].Trim();

    // remove the brackets
    var left = parts[1].Trim().Substring(1, parts[1].Length - 2).Split(", ")[0];
    var right = parts[1].Trim().Substring(1, parts[1].Length - 2).Split(", ")[1];

    if (!graph.ContainsKey(name))
    {
        graph.Add(name, new Node(name));
    }


    graph[name].Left = left;
    graph[name].Right = right;
}


// Part 1
var start = "AAA";
var end = "ZZZ";

var p1Start = graph[start];

var p1Steps = StepsUntil(p1Start.Name, s => s.Equals(end), path);

Console.WriteLine($"Part 1: steps required {p1Steps}");

// part 2

var startsWithA = graph.Keys.Where(x => x.EndsWith('A')).ToList();
var p2Steps = new List<ulong>();

foreach (var n in startsWithA)       
{
    path.Clear();
    foreach (var p in instructions.Trim())
    {
        path.Enqueue(p);
    }
    
    var steps = StepsUntil(n, s => s.EndsWith('Z'), path);
    
    p2Steps.Add(steps);
}

// get the lowest common multiple of p2Steps
var lowestCommonMultiple = p2Steps.Aggregate((a, b) => (a * b) / GreatestCommonDenominator(a, b));


ulong GreatestCommonDenominator(ulong a, ulong b)
{
    while (b != 0)
    {
        var temp = b;
        b = a % b;
        a = temp;
    }

    return a;
}

Console.WriteLine($"Part 2 : steps required {lowestCommonMultiple}");

ulong StepsUntil(string startNode, Func<string, bool> endCondition, Queue<char> path)
{
    var found = false;

    var current = graph[startNode];
    ulong steps = 0;
    while (!found)
    {
        steps++;

        var p = path.Dequeue();

        current = p == 'L' ? graph[current.Left] : graph[current.Right];

        if (endCondition.Invoke(current.Name))
        {
            found = true;
        }

        path.Enqueue(p);
    }

    return steps;
}

public class Node
{
    public Node(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public string Left { get; set; }
    public string Right { get; set; }
}