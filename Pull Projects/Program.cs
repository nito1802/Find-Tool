using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml.Serialization;

namespace Pull_Projects
{
    public class IgnoreByteArrayContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            // Ignoruj właściwości typu byte[]
            properties = properties.Where(p => p.PropertyType != typeof(byte[])).ToList();
            return properties;
        }
    }

    public class MyModel
    {
        public byte[] Content { get; set; }
        public byte[] Conten2t { get; set; }
        public string Name { get; set; }
    }

    internal class Program
    {
        private static void Main()
        {
            var myModel = new MyModel
            {
                Name = "Built With Science 5 Minute Daily Stretch Routine.pdf"
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new IgnoreByteArrayContractResolver(),
            };

            var serialized = JsonConvert.SerializeObject(myModel, settings);

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