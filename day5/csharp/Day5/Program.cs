// See https://aka.ms/new-console-template for more information


using System.Collections.Concurrent;

Console.WriteLine("Hello, World!");

var directory = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day5.txt";

var lines = File.ReadAllText(Path.Combine(directory, input));

var splitbyNewLine = lines.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

Console.WriteLine();

var part1Seeds = splitbyNewLine[0].Split(":")[1].Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

// Part 1

var part1absoluteMin = MapSeeds(part1Seeds).Min();
Console.WriteLine($"Part 1 : {part1absoluteMin}");


// Part 2

// nope, this is not the way to do it
// create pairs from part1seeds, index 0  an 1, index 2 and 3, etc
// var part2Seeds = part1Seeds.Select((x, i) => new { x, i }).GroupBy(x => x.i / 2).Select(x => x.Select(v => v.x).ToList()).ToList();
//
//
// List<long> part2absoluteMin = new List<long>();
//
// var parallel = GetMinMappedSeed(part2Seeds);
//
// IList<long> GetMinMappedSeed(List<List<long>> seedRanges)
// {
//     var bag = new ConcurrentBag<long>();
//
//     Parallel.ForEach(seedRanges, p =>
//     {
//         List<long> seeds = new List<long>();
//         var start = p[0];
//         var end = start + p[1];
//         for (var i = start; i < end; i++)
//         {
//             seeds.Add(i);
//         }
//
//         var mappedSeeds = MapSeeds(seeds);
//
//         var min = mappedSeeds.Min();
//
//         bag.Add(min);
//     });
//
//     return bag.ToList();
// }
//
//
//
//
// Console.WriteLine($"Part 2 : {parallel.Min()}");

List<long> MapSeeds(IEnumerable<long> seeds)
{
    var mappedSeeds = new List<long>();

    foreach (var seed in seeds)
    {
        long mappedSeed = seed;

        // i.e. 
        // 1. seed-to-soil map
        // 2. soil-to-fertilizer-map
        // 3. ...
        foreach (var map in splitbyNewLine.Skip(1))
        {
            var mappingProcess = map.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            var rangeMaps = mappingProcess.Skip(1).Select(Utilities.Convert);

            foreach (var rangeMap in rangeMaps)
            {
                var lowerBound = rangeMap.Src;
                var upperBound = rangeMap.Src + rangeMap.Range;

                var difference = rangeMap.Src - rangeMap.Dst;

                if (mappedSeed < lowerBound || mappedSeed >= upperBound) continue;
                
                var newMapped = mappedSeed - difference;

                mappedSeed = newMapped;

                // break on first match - this works but why? this is not mentioned in the problem statement
                // but using 14 on the test data breaks on water-to-light mapping, it maps twice first is correct but second is not.
                // this now works for test data and my input data
                break;
            }
        }

        mappedSeeds.Add(mappedSeed);
    }

    return mappedSeeds;
}




public static class Utilities
{

    public static RangeMap Convert(string input)
    {
        // input is 1 2 3 and should be mapped to a RangeMap where Dst = 1, Src = 3, Range = 3
        var split = input.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim())).ToList();
        
        return new RangeMap()
        {
            Dst = split[0],
            Src = split[1],
            Range = split[2]
        };
    }
    
    
    
}

public class RangeMap
{
    public long Src { get; set; }
    public long Dst { get; set; }
    public long Range { get; set; }
}