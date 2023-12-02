// See https://aka.ms/new-console-template for more information


Console.WriteLine("Hello, World!");

var inputDir = "F:\\Documents\\GitHub\\Aoc2023\\input";
var inputPath = $"{inputDir}\\day2.txt";

var games = Utilities.ReadInput(inputPath);

// Part 1

var validGames = Utilities.ValidGames(games);

Console.WriteLine($"Part1 Valid games: {validGames}");


// Part 2
List<int> Products = new List<int>();
foreach (var game in games)
{
    var sum = Utilities.FindSumOfMaximumRGB(game);
    Products.Add(sum);
}
var summed = Products.Aggregate((a, b) => a + b);

Console.WriteLine($"Part2 Powers : {summed}");

public class Utilities
{
    public static bool ValidSet(GameSet set)
    {
        var maxRed = 12;
        var maxGreen = 13;
        var maxBlue = 14;

        return set.Red <= maxRed && set.Green <= maxGreen && set.Blue <= maxBlue;

    }

    public static bool ValidGame(Game game)
    {
        var valid = true;
        foreach (var set in game.Sets)
        {
            valid = valid && ValidSet(set);
        }

        return valid;
    }

    public static int ValidGames(List<Game> games)
    {
        var validIdTotal = 0;
        foreach (var game in games)
        {
            if (ValidGame(game))
            {
                validIdTotal += game.Id;
            }
        }

        return validIdTotal;
    }

    public static int FindSumOfMaximumRGB(Game game)
    {
        var maxRed = 0;
        var maxGreen = 0;
        var maxBlue = 0;

        foreach (var set in game.Sets)
        {
            maxRed = Math.Max(maxRed, set.Red);
            maxGreen = Math.Max(maxGreen, set.Green);
            maxBlue = Math.Max(maxBlue, set.Blue);
        }

        return maxRed * maxGreen * maxBlue;
    }

    public static List<Game> ReadInput(string path)
    {
        // line looks like: Game 52: 13 green, 15 blue; 6 blue, 4 red, 8 green; 6 red, 13 green, 11 blue; 2 red, 7 green, 13 blue; 12 green, 2 blue, 3 red; 6 red, 11 green, 1 blue
        var lines = File.ReadAllLines(path);
        var games = new List<Game>();

        foreach (var ln in lines)
        {
            var game = new Game();
            var parts = ln.Split(":");
            game.Id = int.Parse(parts[0].Split(" ")[1]);

            var setParts = parts[1].Split(";");
            foreach (var setPart in setParts)
            {
                var set = new GameSet();
                var colors = setPart.Split(",");
                foreach (var color in colors)
                {
                    var colorParts = color.Trim().Split(" ");
                    var count = int.Parse(colorParts[0]);
                    var colorName = colorParts[1];
                    switch (colorName)
                    {
                        case "red":
                            set.Red = count;
                            break;
                        case "green":
                            set.Green = count;
                            break;
                        case "blue":
                            set.Blue = count;
                            break;
                    }
                }

                game.Sets.Add(set);
            }

            games.Add(game);
        }

        return games;
    }
}




public class Game
{
    public Game()
    {
        Sets = new List<GameSet>();
    }

    public int Id { get; set; }
    public List<GameSet> Sets { get; set; }
}

public class GameSet
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
}