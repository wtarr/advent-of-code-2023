// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var directory = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day9.txt";

var lines = File.ReadAllLines(Path.Combine(directory, input));

var numberLine1 = lines.Select(
        line => line.Split(Array.Empty<char>()))
    .Select(split => split.Select(int.Parse).ToList())
    .ToList();

var part1Tally = ReduceDeduct(numberLine1);

Console.WriteLine($"Part 1 : {part1Tally}");


var numberLine2 = lines.Select(
        line => line.Split(Array.Empty<char>()))
    .Select(split => split.Select(int.Parse).ToList())
    .ToList();

// reverse each line in numberLine2
var reversed = numberLine2.Select(nl => nl.Reverse<int>().ToList()).ToList();

var part2Tally = ReduceDeduct(reversed);

Console.WriteLine($"Part 2 : {part2Tally}");

int ReduceDeduct(List<List<int>> numberLine)
{
    var tally = 0;
    
    foreach (var nl in numberLine)
    {
        var reductions = new List<List<int>>();

        reductions.Add(nl);
    
        var reduced = false;
        while (!reduced)
        {
            // take last from reductions
            var lastAdded = reductions.Last();
        
            var current = new List<int>();
    
            var zipped = lastAdded.Zip(lastAdded.Skip(1), (a, b) => (a, b));
        
            foreach (var (a, b) in zipped)
            {
                current.Add(b - a);
            }
        
            reductions.Add(current);

            // if current is all same
            if (current.All(i => i == current.First()))
            {
                reduced = true;
            }
        
            //reduced = current.All(i => i == 0);
        }
    
        var reversed = reductions.Reverse<List<int>>().ToList();

        for (var i = 1; i < reversed.Count(); i++)
        {
            var current = reversed.ElementAt(i).Last();
            var previous = reversed.ElementAt(i - 1).Last();
        
            reversed[i].Add(current + previous);
        }

        var last = reversed.Last().Last();

        tally+= last;
    
        Console.WriteLine(last);
    
    }

    return tally;
}
