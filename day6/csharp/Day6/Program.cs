// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

// test data
List<Race> races = new List<Race>
{
    new Race(7, 9),
    new Race(15, 40),
    new Race(30,200)
};

// part 1
Console.WriteLine($"Part 1: {FindNumberOfRecordBeatingStrategies(races)}");


// part 2 
var mergeTimes = string.Join("", (races.Select(x => x.Time)).ToList());
var mergeDistance = string.Join("", (races.Select(x => x.RecordDistance)).ToList());

var part2Race = new Race(long.Parse(mergeTimes), long.Parse(mergeDistance));

Console.WriteLine($"Part 2: {FindNumberOfRecordBeatingStrategies(new List<Race>{part2Race})}");


return;


long FindNumberOfRecordBeatingStrategies(List<Race> r)
{
    foreach (var race in r)
    {
        for (var i = 1; i < race.Time; i++)
        {
            var timeLeft = race.Time - i;

            var achievableDistance = i * timeLeft;

            if (achievableDistance > race.RecordDistance)
            {
                race.DistanceBeatableCount += 1;
            }
        }
    }

    return r.Select(z => z.DistanceBeatableCount).Aggregate((x, y) => x * y);
}


public class Race(long time, long recordDistance)
{
    public long Time { get; set; } = time;
    public long RecordDistance { get; set; } = recordDistance;

    public long DistanceBeatableCount { get; set; }
}