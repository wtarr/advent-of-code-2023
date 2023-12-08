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



var found = false;
var start = "AAA";
var end = "ZZZ";

var current = graph[start];

var steps = 0;

while (!found)
{
    steps++;
    
    var p = path.Dequeue();
    
   
    
    if (p == 'L')
    {
        current = graph[current.Left];
    }
    else
    {
        current = graph[current.Right];
    }

    if (current.Name == end)
    {
        found = true;
    }
    
    path.Enqueue(p);
    
}

Console.WriteLine($"Part 1: steps required {steps}");


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