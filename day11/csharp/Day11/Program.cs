// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var directory = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day11-test.txt";

var lines = File.ReadAllLines(Path.Combine(directory, input));

var emptyRowsIndices = Utilities.FindEmptyRows(lines);

var emptyColumnsIndices = Utilities.FindEmptyColumns(lines);

var galaxies = Utilities.GetGalaxies(lines);

var emptyRowColumnScale = 2; // twice as big = 374

var part1 = Utilities.CalculateAllUniqueDistances(galaxies, emptyRowsIndices, emptyColumnsIndices, emptyRowColumnScale);

Console.WriteLine($"Part1: Distance: {part1}");

emptyRowColumnScale = 10; // ten times as big = 1030 

var part2 = Utilities.CalculateAllUniqueDistances(galaxies, emptyRowsIndices, emptyColumnsIndices, emptyRowColumnScale);

Console.WriteLine($"Part2: Distance: {part2}");

public static class Utilities {

    public static long CalculateAllUniqueDistances(List<Point> points, List<int> emptyRows, List<int> emptyCols, int scale)
    {
        scale -= 1;
        
        long distance = 0;

        HashSet<Point> visitedGalaxy = new HashSet<Point>();

        for (int i = 0; i < points.Count; i++)
        {
            var galaxy = points[i];

            visitedGalaxy.Add(galaxy);

            // iterate other galaxies
            for (int j = 0; j < points.Count; j++)
            {
                var otherGalaxy = points[j];
                
                // prevent reverse comparison
                if (visitedGalaxy.Contains(otherGalaxy))
                {
                    continue;
                }

                var manhattanDistance = ManhattanDistance(galaxy, otherGalaxy);

                distance += manhattanDistance;

                // if row of galaxy and row of other galaxy cross any of the indices in emptyRowsIndices list
                // or if column of galaxy and column of other galaxy cross any of the indices in emptyColumnsIndices
                // then add scale to distance to account for the empty space
                
                var emptyRowsCrossed = emptyRows.Where(index => index > galaxy.Row && index < otherGalaxy.Row ||
                                                                index < galaxy.Row && index > otherGalaxy.Row).ToList();
                
                var emptyColsCrossed = emptyCols.Where(index => index > galaxy.Column && index < otherGalaxy.Column ||
                                                                index < galaxy.Column && index > otherGalaxy.Column).ToList();
            
                
                if (emptyRowsCrossed.Any())
                    distance += scale * emptyRowsCrossed.Count();
                
                if (emptyColsCrossed.Any())
                    distance += scale * emptyColsCrossed.Count();
                
                
            }
            
        }

        return distance;
    }

    public static int ManhattanDistance(Point p1, Point p2)
    {
        return Math.Abs(p1.Row - p2.Row) + Math.Abs(p1.Column - p2.Column);
    }
    
    public static List<Point> GetGalaxies(string[] lines)
    {
        List<Point> points = new List<Point>();

        for(int y = 0; y < lines.Length; y++)
        {
            for(int x = 0; x < lines[y].Length; x++)
            {
                if(lines[y][x] == '#')
                {
                    points.Add(new Point(y, x));
                }
            }
        }

        return points;
    }

    public static List<int> FindEmptyRows(string[] lines)
    {
        return lines
            .Select((line, index) => new { Line = line, Index = index })
            .Where(item => item.Line.All(c => c == '.'))
            .Select(item => item.Index)
            .ToList();
    }
    
    public static List<int> FindEmptyColumns(string[] lines)
    {
        int numCols = lines.Max(line => line.Length);

        return Enumerable.Range(0, numCols)
            .Where(colIndex => lines.All(line => colIndex >= line.Length || line[colIndex] == '.'))
            .ToList();
    }
    
}

public class Point(int row, int column)
{
    public int Row { get; set; } = row;
    public int Column { get; set; } = column;

    public override bool Equals(object? obj)
    {
        return obj is Point point &&
               Row == point.Row &&
               Column == point.Column;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
    
    public override string ToString()
    {
        return $"({Row}, {Column})";
    }
}