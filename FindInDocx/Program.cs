using System.Xml.Linq;

namespace FindInDocx
{
    internal class Program
    {
        private static void Main()
        {
            var filePath = @"C:\Users\Jarek\Desktop\Istotne\source\Visual Studio\Test\maui-samples\8.0\Animations\Animations\Animations.csproj";
            var doc = XDocument.Load(filePath);

            var packageNames = doc.Descendants("PackageReference")
                .Select(e => e.Attribute("Include")?.Value)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();

            foreach (var name in packageNames)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine("Conversion completed.");
        }
    }
}