// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Text;

Console.WriteLine("Hello, World!");

var inputDir = "F:\\Documents\\GitHub\\Aoc2023\\input";

var day1 = Path.Combine(inputDir, "day1.txt");

var day1Input = File.ReadAllLines(day1);

// part 1
Part(1);

// part 2
Part(2);


// Part 1
void Part(int part)
{
    int total = 0;

    foreach (var ln in day1Input)
    {
        var line = ln;
        if (part == 2)
        {
            line = Utilities.ReplaceWordWithDigit(ln);
        }

        

        total += Utilities.LineTotal(line);
    }

    Console.WriteLine($"part {part} : {total}");
}

public class Utilities
{
    public static int LineTotal(string line)
    {
        char left = '-';
        char right = left;

        foreach (var character in line)
        {
            if (Utilities.IsDigit(character))
            {
                if (left == '-')
                {
                    left = character;
                    right = left;
                }

                if (left != '-')
                {
                    right = character;
                }
            }
        }

        return int.Parse($"{left}{right}");
    }


    public static bool IsDigit(char character)
    {
        return character is >= '0' and <= '9';
    }

    public static string ReplaceWordWithDigit(string line)
    {
        var strb = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            if (IsDigit(line[i]))
            {
                strb.Append(line[i]);
            }
            else
            {
                // try build a word that matches a number (as a word)
                // if we find a word, append the number to the string builder
                // and move the index to the end of the word
                var word = new StringBuilder();
                word.Append(line[i]);

                for (int j = i + 1; j < line.Length; j++)
                {
                    if (IsDigit(line[j]) == false)
                    {
                        word.Append(line[j]);

                        if (word.ToString().Contains("one"))
                        {
                            strb.Append("1");
                            i += "one".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("two"))
                        {
                            strb.Append("2");
                            i += "two".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("three"))
                        {
                            strb.Append("3");
                            i += "three".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("four"))
                        {
                            strb.Append("4");
                            i += "four".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("five"))
                        {
                            strb.Append("5");
                            i += "five".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("six"))
                        {
                            strb.Append("6");
                            i += "six".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("seven"))
                        {
                            strb.Append("7");
                            i += "seven".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("eight"))
                        {
                            strb.Append("8");
                            i += "eight".Length - 2;
                            break;
                        }

                        if (word.ToString().Contains("nine"))
                        {
                            strb.Append("9");
                            i += "nine".Length - 2;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }


            }

        }

        return strb.ToString();

    }
}
