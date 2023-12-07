// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var directory = "F://Documents//GitHub//Aoc2023//input";
var input = "day7.txt";


var lines = File.ReadAllLines(Path.Combine(directory, input));


var hands = new List<Hand>();

foreach (var line in lines)
{
    hands.Add(Utilities.ParseHand(line));
}

hands.Sort(new HandComparer());

var tally = 0;
for(int i = 1; i <= hands.Count; i++)
{
    var currentHand = hands[i - 1];
    var win = currentHand.Bid * i;

    tally += win;
}

Console.WriteLine($"Part 1: {tally}");


public class Utilities
{
    public static Hand ParseHand(string line)
    {
        var split = line.Split(" ");
        var rHand = split[0];
        var rbid = split[1];


        List<Card> cards = new List<Card>();
        foreach (var c in rHand.Trim())
        {
            Card card = new Card();

            switch (c)
            {
                case 'A':
                    card.Type = Card.CardType.A;
                    break;
                case 'K':
                    card.Type = Card.CardType.K;
                    break;
                case 'Q':
                    card.Type = Card.CardType.Q;
                    break;
                case 'J':
                    card.Type = Card.CardType.J;
                    break;
                case 'T':
                    card.Type = Card.CardType.T;
                    break;
                case '9':
                    card.Type = Card.CardType._9;
                    break;
                case '8':
                    card.Type = Card.CardType._8;
                    break;
                case '7':
                    card.Type = Card.CardType._7;
                    break;
                case '6':
                    card.Type = Card.CardType._6;
                    break;
                case '5':
                    card.Type = Card.CardType._5;
                    break;
                case '4':
                    card.Type = Card.CardType._4;
                    break;
                case '3':
                    card.Type = Card.CardType._3;
                    break;
                default:
                    card.Type = Card.CardType._2;
                    break;
            }

            cards.Add(card);
        }

        return new Hand(cards, int.Parse(rbid));
    }
}

public class Hand
{
    [Flags]
    public enum HandType
    {
        FiveOfAKind = 9,
        FourOfAKind = 8,
        FullHouse = 7,
        ThreeOfAKind = 6,
        TwoPair = 5,
        OnePair = 4,
        HighCard = 3
    }

    public List<Card> Cards { get; private set; }

    public HandType Part1HandType { get; private set; }
    public HandType Part2HandType { get; private set; }

    public int Bid { get; private set; }

    public Hand(List<Card> cards, int bid)
    {
        Cards = cards;
        Bid = bid;
        Part1HandType = ClassifyRulePart1();
        //Part2HandType = ClassifyRulePart2();
    }

    private HandType ClassifyRulePart1()
    {
        // if all cards are the same, it's a five of a kind :: AAAAA
        if (Cards.All(c => c.Type == Cards[0].Type))
        {
            return HandType.FiveOfAKind;
        }

        // if there are 4 of the same type, it's a four of a kind :: AA8AA
        if (Cards.GroupBy(c => c.Type).Any(g => g.Count() == 4))
        {
            return HandType.FourOfAKind;
        }

        // if there are 3 of the same type and 2 of the same type, it's a full house :: 23332
        if (Cards.GroupBy(c => c.Type).Any(g => g.Count() == 3) && Cards.GroupBy(c => c.Type).Any(g => g.Count() == 2))
        {
            return HandType.FullHouse;
        }

        // if there are 3 of the same type, it's a three of a kind :: TTT98
        if (Cards.GroupBy(c => c.Type).Any(g => g.Count() == 3))
        {
            return HandType.ThreeOfAKind;
        }

        // if there are 2 of the same type and 2 of the same type, it's a two pair :: 23432
        if (Cards.GroupBy(c => c.Type).Count(g => g.Count() == 2) == 2)
        {
            return HandType.TwoPair;
        }

        // if there are 2 of the same type, it's a one pair :: A23A4
        if (Cards.GroupBy(c => c.Type).Any(g => g.Count() == 2))
        {
            return HandType.OnePair;
        }

        // otherwise, it's a high card :: 23456
        return HandType.HighCard;
    }

    // private HandType ClassifyRulePart2()
    // {
    //     // todo
    // }
}

public class HandComparer : IComparer<Hand>
{
    public int Compare(Hand x, Hand y)
    {
        if (x.Part1HandType > y.Part1HandType)
        {
            return 1;
        }

        if (x.Part1HandType < y.Part1HandType)
        {
            return -1;
        }

        // if both hands are the same type, compare the cards
        for (int i = 0; i < x.Cards.Count; i++)
        {
            if (x.Cards[i].Type > y.Cards[i].Type)
            {
                return 1;
            }

            if (x.Cards[i].Type < y.Cards[i].Type)
            {
                return -1;
            }
        }

        return 0;
    }
}


public class Card
{
    [Flags]
    public enum CardType
    {
        A = 13,
        K = 12,
        Q = 11,
        J = 10,
        T = 9,
        _9 = 8,
        _8 = 7,
        _7 = 6,
        _6 = 5,
        _5 = 4,
        _4 = 3,
        _3 = 2,
        _2 = 1
    }

    public CardType Type { get; set; }
}