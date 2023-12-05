// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;

Console.WriteLine("Hello, World!");

var directory = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day5-test.txt";

var lines = File.ReadAllText(Path.Combine(directory, input));

var splitbyNewLine = lines.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

Console.WriteLine();

var part1Seeds = splitbyNewLine[0].Split(":")[1].Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));


#region Build and cache the mapping processes

var mappingProcesses = new List<MappingProcess>();

foreach (var map in splitbyNewLine.Skip(1))
{
    var name = map.Split(':')[0].Trim();
    
    var mappingProcess = map.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

    var rangeMaps = mappingProcess.Skip(1).Select(Utilities.Convert);
    
    var p = new MappingProcess()
    {
        Name = name,
        RangeMaps = rangeMaps.ToList()
    };
    
    mappingProcesses.Add(p);
}

#endregion

// Part 1
Part1();

// Part 2
Part2();


void Part1()
{
    var part1absoluteMin = LocateMinimumMappedSeed(part1Seeds, mappingProcesses);
    Console.WriteLine($"Part 1 : {part1absoluteMin}");
}

void Part2()
{
    // nope, this is not the way to do it this took just over 4 minutes to run on a ryzen 5 2600x
    Console.WriteLine($"starting part 2: {DateTime.UtcNow}");
    
    // group the pairs of seeds and their range
    var part2Seeds = part1Seeds.Select((x, i) => new {x, i}).GroupBy(x => x.i / 2)
        .Select(x => x.Select(v => v.x).ToList()).ToList();

    // lets burn all the cores
    var parallel = ParallelFindMinimumMappedSeedRange(part2Seeds, mappingProcesses);
    
    IList<long> ParallelFindMinimumMappedSeedRange(List<List<long>> seedRanges, List<MappingProcess> mp)
    {
        var bag = new ConcurrentBag<long>();

        Parallel.ForEach(seedRanges, p =>
        {
            List<long> seeds = new List<long>();
            var start = p[0];
            var end = start + p[1];
            for (var i = start; i < end; i++)
            {
                seeds.Add(i);
            }

            var minimumMappedSeed = LocateMinimumMappedSeed(seeds, mp);

            bag.Add(minimumMappedSeed);
        });

        return bag.ToList();
    }

    Console.WriteLine($"Part 2 : {parallel.Min()}");
    Console.WriteLine($"ending part 2: {DateTime.UtcNow}");
}

long LocateMinimumMappedSeed(IEnumerable<long> seeds, List<MappingProcess> mp)
{
    long minimum = long.MaxValue;

    Parallel.ForEach(seeds, seed =>
    {
        var mappedSeed = MapSeed(seed, mp);
        
        if (Interlocked.Read(ref minimum) > mappedSeed)
        {
            Interlocked.Exchange(ref minimum, mappedSeed);
        }
    });

    return minimum;
}

long MapSeed(long seed, List<MappingProcess> mp)
{
    long mappedSeed = seed;

    // i.e. 
    // 1. seed-to-soil map
    // 2. soil-to-fertilizer-map
    // 3. ...
    //foreach (var map in splitbyNewLine.Skip(1))
    foreach (var process in mp)
    {
        foreach (var rangeMap in process.RangeMaps)
        {
            if (mappedSeed < rangeMap.Lower || mappedSeed >= rangeMap.Upper) continue;
                
            var difference = rangeMap.Src - rangeMap.Dst;
                
            var newMapped = mappedSeed - difference;

            mappedSeed = newMapped;

            // break on first match - this works but why? this is not mentioned in the problem statement
            // but using 14 on the test data breaks on water-to-light mapping, it maps twice first is correct but second is not.
            // this now works for test data and my input data
            break;
        }
    }

    return mappedSeed;

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
            Range = split[2],
            
            Lower = split[1],
            Upper = split[1] + split[2]
        };
    }
}

public class RangeMap
{
    public long Src { get; set; }
    public long Dst { get; set; }
    public long Range { get; set; }
    
    // range
    public long Lower { get; set; }
    public long Upper { get; set; }
}

public class MappingProcess
{
    public string Name { get; set; }
    public List<RangeMap> RangeMaps { get; set; }
}