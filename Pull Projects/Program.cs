using System.Text;
using System.Text.RegularExpressions;

namespace Pull_Projects
{
    internal class Program
    {
        private static void Main()
        {
            var pathMy = @"C:\Users\dante\Desktop\Istotne\source\Visual Studio\Test\RegexProcessTests\TestProject1\UnitTest1.cs";
            var contentMy = File.ReadAllLines(pathMy);

            string input = "Assert.AreEqual(\"aaa\", \"bbb\");";
            var mmm = ConvertToFluentAssertion(input);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var line in contentMy)
            {
                if (!line.Contains("Assert.AreEqual"))
                {
                    stringBuilder.AppendLine(line);
                }
                else
                {
                    var newLine = ExtractArguments(line);
                    stringBuilder.AppendLine(newLine);
                }
            }

            var myContentNew = stringBuilder.ToString();
            myContentNew = myContentNew.Replace("[TestClass]", string.Empty).Replace("[TestMethod]", "[Fact]");

            File.WriteAllText(pathMy, myContentNew);
            //Assert.AreEqual

            string baseDir = @"C:\Users\dante\Desktop\Istotne\source\Visual Studio\Main";
        }

        private static string ConvertToFluentAssertion(string input)
        {
            string pattern = @"Assert\.AreEqual\(""([^""]*)"", ""([^""]*)""\);";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string firstValue = match.Groups[1].Value;
                string secondValue = match.Groups[2].Value;

                return $"{secondValue}.Should().Be({firstValue});";
            }

            throw new ArgumentException("No match found. Assert.AreEqual");
        }

        private static string ExtractArguments(string input)
        {
            string pattern = @"Assert\.AreEqual\(\s*(?:(?<firstArg>""[^""]*"")|(?<firstArg>[^,]+)),\s*(?:(?<secondArg>""[^""]*"")|(?<secondArg>[^)]+))\s*\);";

            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string firstArg = match.Groups["firstArg"].Value.Trim();
                string secondArg = match.Groups["secondArg"].Value.Trim();

                return $"{secondArg}.Should().Be({firstArg});";
            }
            throw new ArgumentException("No match found. Assert.AreEqual");
        }
    }
}