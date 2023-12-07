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

hands.Sort(new HandComparerP1());

var tally = 0;
for(int i = 1; i <= hands.Count; i++)
{
    var currentHand = hands[i - 1];
    var win = currentHand.Bid * i;

    tally += win;
}

Console.WriteLine($"Part 1: {tally}");

hands.Sort(new HandComparerP2());

tally = 0;
for(int i = 1; i <= hands.Count; i++)
{
    var currentHand = hands[i - 1];
    var win = currentHand.Bid * i;

    tally += win;
}

Console.WriteLine($"Part 2: {tally}");


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

        return new Hand(line, cards, int.Parse(rbid));
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
    
    public string CardRaw { get; set; }

    public HandType Part1HandType { get; private set; }
    public HandType Part2HandType { get; private set; }

    public int Bid { get; private set; }

    public Hand(string raw, List<Card> cards, int bid)
    {
        CardRaw = raw;
        Cards = cards;
        Bid = bid;
        Part1HandType = ClassifyRulePart1(cards);
        Part2HandType = ClassifyRulePart2(cards);
    }

    private HandType ClassifyRulePart1(List<Card> cards)
    {
        // if all cards are the same, it's a five of a kind :: AAAAA
        if (cards.All(c => c.Type == cards[0].Type))
        {
            return HandType.FiveOfAKind;
        }

        var grouped = cards.GroupBy(c => c.Type).ToList();

        // if there are 4 of the same type, it's a four of a kind :: AA8AA
        if (grouped.Any(g => g.Count() == 4))
        {
            return HandType.FourOfAKind;
        }

        // if there are 3 of the same type and 2 of the same type, it's a full house :: 23332
        if (grouped.Any(g => g.Count() == 3) && grouped.Any(g => g.Count() == 2))
        {
            return HandType.FullHouse;
        }

        // if there are 3 of the same type, it's a three of a kind :: TTT98
        if (grouped.Any(g => g.Count() == 3))
        {
            return HandType.ThreeOfAKind;
        }

        // if there are 2 of the same type and 2 of the same type, it's a two pair :: 23432
        if (grouped.Count(g => g.Count() == 2) == 2)
        {
            return HandType.TwoPair;
        }

        // if there are 2 of the same type, it's a one pair :: A23A4
        if (grouped.Any(g => g.Count() == 2))
        {
            return HandType.OnePair;
        }

        // otherwise, it's a high card :: 23456
        return HandType.HighCard;
    }

    private HandType ClassifyRulePart2(List<Card> cards)
    {
        if (cards.All(x => x.Type == Card.CardType.J))
        {
            return ClassifyRulePart1(cards);
        }
        
        // does the cards list have any J?
        // if so substitute it with the highest card in the list
        var jokers = cards.Any(x => x.Type == Card.CardType.J);

        if (!jokers) return ClassifyRulePart1(cards);
        {
            // excluding the joker what is the most common card?
            var grouped = cards.Where(x => x.Type != Card.CardType.J).GroupBy(x => x.Type).ToList();
            
            var mostCommon = grouped.OrderByDescending(x => x.Count()).First().Key;
            
            // replace the joker with the most common card
            var newCards = new List<Card>();
            foreach (var card in cards)
            {
                if (card.Type == Card.CardType.J)
                {
                    newCards.Add(new Card() {Type = mostCommon});
                }
                else
                {
                    newCards.Add(card);
                }
            }

            return ClassifyRulePart1(newCards);
        }

    }
}

public class HandComparerP1 : IComparer<Hand>
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
            var xCard = x.Cards[i].MapPart1();
            var yCard = y.Cards[i].MapPart1();
            
            if (xCard > yCard)
            {
                return 1;
            }

            if (xCard < yCard)
            {
                return -1;
            }
        }

        return 0;
    }
}

public class HandComparerP2 : IComparer<Hand>
{
    public int Compare(Hand x, Hand y)
    {
        if (x.Part2HandType > y.Part2HandType)
        {
            return 1;
        }

        if (x.Part2HandType < y.Part2HandType)
        {
            return -1;
        }

        // if both hands are the same type, compare the cards
        // highest first card breaks the tie
        
        for (int i = 0; i < x.Cards.Count; i++)
        {
            var a = x.Cards[i].MapPart2();
            var b = y.Cards[i].MapPart2();
            
            if (a > b)
            {
                return 1;
            }

            if (a < b)
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
        A,
        K,
        Q,
        J,
        T,
        _9,
        _8,
        _7,
        _6,
        _5,
        _4,
        _3,
        _2
    }

    private Dictionary<CardType, Tuple<int, int>> CardValues = new();
    // add the values
    public Card()
    {
        CardValues.Add(CardType.A, new Tuple<int, int>(13, 13));
        CardValues.Add(CardType.K, new Tuple<int, int>(12, 12));
        CardValues.Add(CardType.Q, new Tuple<int, int>(11, 11));
        CardValues.Add(CardType.J, new Tuple<int, int>(10, 1));
        CardValues.Add(CardType.T, new Tuple<int, int>(9, 10));
        CardValues.Add(CardType._9, new Tuple<int, int>(8, 9));
        CardValues.Add(CardType._8, new Tuple<int, int>(7, 8));
        CardValues.Add(CardType._7, new Tuple<int, int>(6, 7));
        CardValues.Add(CardType._6, new Tuple<int, int>(5, 6));
        CardValues.Add(CardType._5, new Tuple<int, int>(4, 5));
        CardValues.Add(CardType._4, new Tuple<int, int>(3, 4));
        CardValues.Add(CardType._3, new Tuple<int, int>(2, 3));
        CardValues.Add(CardType._2, new Tuple<int, int>(1, 2));
    }

    public int MapPart1() => CardValues[Type].Item1;
    public int MapPart2() => CardValues[Type].Item2;

    public CardType Type { get; set; }
}