// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

var dir = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day4.txt";

var numbers = Utilities.ReadAllNumbers($"{dir}\\{input}");

var total = 0;

foreach (var card in numbers)
{
    var matches = Utilities.GetMatchingWinningNumbers(card);
    var score = Utilities.Score(matches.Count);

    total += score;
}

Console.WriteLine($"Part 1 : Total score: {total}");

public class Utilities
{

    public static List<Numbers> ReadAllNumbers(string path)
    {
        // Card   1: 95 57 30 62 11  5  9  3 72 87 | 94 72 74 98 23 57 62 14 30  3 73 49 80 96 20 60 17 35 11 63 87  9  6  5 95
        
        var lines = File.ReadAllLines(path);
        
        var numbers = new List<Numbers>();

        foreach (var line in lines)
        {
            // split on the colon
            var parts = line.Split(":");
            var id = ReadNumbers(parts[0].Trim())[0]; 
            var numbersParts = parts[1].Split("|");
            var winningNumbers = ReadNumbers(numbersParts[0].Trim());
            var yourNumbers = ReadNumbers(numbersParts[1].Trim());
            
            numbers.Add(new Numbers { Id = id, WinningNumbers = winningNumbers, YourNumbers = yourNumbers });
        }

        return numbers;
    }

    public static List<int> ReadNumbers(string line)
    {
        StringBuilder sb = new StringBuilder();
        var numbers = new List<int>();
        foreach(var c in line){
            if(IsDigit(c)){
                sb.Append(c);
            } 
            else {
                if (sb.Length > 0)
                    numbers.Add(int.Parse(sb.ToString()));
                sb.Clear();
            }
            
        }
        // add the last number before next line
        if (sb.Length > 0)
            numbers.Add(int.Parse(sb.ToString()));

        return numbers;
    }

    public static List<int> GetMatchingWinningNumbers(Numbers numbers)
    {
        // find the winning numbers that are in your numbers
        var winningNumbers = numbers.YourNumbers.Intersect(numbers.WinningNumbers).ToList();

        return winningNumbers;
    }
    public static bool IsDigit(char c) => c is >= '0' and <= '9';
    public static int Score(int matches){
        // 1 for first match, then doubled thereafter
        return (int)Math.Pow(2, matches - 1);
        
    }
    
}

public class Numbers
{
    public int Id { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> YourNumbers { get; set; }
}