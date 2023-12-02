using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Part2Tests
    {
        [TestCase("jrpxsrqgr9hqsddmscmsrsbhkdc63eightxfscd", 98)]
        [TestCase("x3five", 35)]
        [TestCase("5twonemt", 51)]
        public void Test(string input, int expected)
        {
            var sjiot = Utilities.ReplaceWordWithDigit(input);

            var total = Utilities.LineTotal(sjiot);

            Assert.That(total == expected);
        }

    }
}
