// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

var dir = "F:\\Documents\\GitHub\\Aoc2023\\input";

var input = "day4.txt";

var allCards = Utilities.ReadAllNumbers($"{dir}\\{input}");

int Part1()
{
    var total = 0;

    foreach (var card in allCards)
    {
        var matches = Utilities.GetMatchingWinningNumbers(card);
        var score = Utilities.Score(matches.Count());

        total += score;
    }

    return total;
}

Console.WriteLine($"Part 1 : Total score: {Part1()}");


// needed help with this one to wrap my head around the problem - (hyper-neutrino) https://youtu.be/uxShpk__9xE?si=a75x3BDIrERsD3VYp
int Part2()
{
    var map = new Dictionary<int, int>();

    foreach (var card in allCards)
    {
        map.TryAdd(card.Id, 1); // first encounter?

        var matches = Utilities.GetMatchingWinningNumbers(card);

        foreach (var winningCopyId in Enumerable.Range(card.Id + 1, matches.Count()))
        {
            map.TryAdd(winningCopyId, 1);

            map[winningCopyId] += map[card.Id];
        }
    }
    
    return map.Values.Sum();
}

Console.WriteLine($"Part 2 : Total score: {Part2()}");


// recursively solving it ...
// memory + speed is not good
int Part2Recursively()
{
    var tally = 0;

    foreach (var card in allCards)
    {
        tally += 1; // current card
        RecurseGames(card);
    }

    void RecurseGames(ScratchCard game)
    {
        var wins = Utilities.GetMatchingWinningNumbers(game);

        var enumerable = wins as int[] ?? wins.ToArray();
        
        tally += enumerable.Length; // this card wins x amount of copies

        // for each of the id's of the winning copies, recurse (where we have in our collection of all cards)
        foreach (var winningCopyId in Enumerable.Range(game.Id + 1, enumerable.Length))
        {
            var winningCopy = Utilities.SelectById(allCards, winningCopyId).FirstOrDefault();

            if (winningCopy == null)
                return;

            RecurseGames(winningCopy);
        }
    }

    return tally;
}

Console.WriteLine($"Part 2 (recursive) : Total score: {Part2Recursively()}");

public class Utilities
{
    public static List<ScratchCard> SelectById(List<ScratchCard> numbers, int id)
    {
        return numbers.Where(n => n.Id == id).ToList();
    }
    
    public static List<ScratchCard> SelectByIds(List<ScratchCard> numbers, List<int> ids)
    {
        return numbers.Where(n => ids.Contains(n.Id)).ToList();
    }

    public static List<ScratchCard> ReadAllNumbers(string path)
    {
        // Card   1: 95 57 30 62 11  5  9  3 72 87 | 94 72 74 98 23 57 62 14 30  3 73 49 80 96 20 60 17 35 11 63 87  9  6  5 95
        
        var lines = File.ReadAllLines(path);
        
        var numbers = new List<ScratchCard>();

        foreach (var line in lines)
        {
            // split on the colon
            var parts = line.Split(":");
            var id = ReadNumbers(parts[0].Trim())[0]; 
            var numbersParts = parts[1].Split("|");
            var winningNumbers = ReadNumbers(numbersParts[0].Trim());
            var yourNumbers = ReadNumbers(numbersParts[1].Trim());
            
            numbers.Add(new ScratchCard { Id = id, WinningNumbers = winningNumbers, YourNumbers = yourNumbers });
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

    public static IEnumerable<int> GetMatchingWinningNumbers(ScratchCard scratchCard)
    {
        // find the winning numbers that are in your numbers
        var winningNumbers = scratchCard.YourNumbers.Intersect(scratchCard.WinningNumbers);

        return winningNumbers;
    }
    public static bool IsDigit(char c) => c is >= '0' and <= '9';
    public static int Score(int matches){
        // 1 for first match, then doubled thereafter
        return (int)Math.Pow(2, matches - 1);
        
    }
    
}

public class ScratchCard
{
    public int Id { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> YourNumbers { get; set; }
}