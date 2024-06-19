using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Pull_Projects
{
    internal class Program
    {
        private static void Main()
        {
            string xml = @"
        <Textbolds>
            <Textbold>
                <Width>45in</Width>
            </Textbold>
            <Textbold>
                <Width>2in</Width>
            </Textbold>
            <Textbold>
                <Width>12in</Width>
            </Textbold>
        </Textbolds>";

            XmlSerializer serializer = new XmlSerializer(typeof(Textbolds));
            using (StringReader reader = new StringReader(xml))
            {
                Textbolds result = (Textbolds)serializer.Deserialize(reader);
                foreach (var textbold in result.TextboldList)
                {
                    Console.WriteLine(textbold.Width);
                }
            }
        }
    }
}