using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pull_Projects
{
    public class CustomerPartConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CustomerPart);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var customerPart = new CustomerPart();

            // Try to read the single CustomerData object
            var customerData = jsonObject["CustomerData"]?.ToObject<CustomerData>(serializer);
            if (customerData != null)
            {
                customerPart.Customers = new List<CustomerData>();
                customerPart.Customers.Add(customerData);
            }

            // Try to read the list of CustomerData objects
            var customersData = jsonObject["Customers"]?.ToObject<List<CustomerData>>(serializer);
            if (customersData != null)
            {
                customerPart.Customers = customersData;
            }

            return customerPart;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var customerPart = (CustomerPart)value;

            writer.WriteStartObject();

            // Write the single CustomerData object for backward compatibility
            if (customerPart.Customers != null && customerPart.Customers.Count > 0)
            {
                writer.WritePropertyName("CustomerData");
                serializer.Serialize(writer, customerPart.Customers.First());
            }

            // Write the list of CustomerData objects
            writer.WritePropertyName("Customers");
            serializer.Serialize(writer, customerPart.Customers);

            writer.WriteEndObject();
        }
    }

    public class CustomerData
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }

        public CustomerData()
        {
        }
    }

    public class CustomerPart
    {
        public CustomerPart()
        {
        }

        //[JsonIgnore]
        public List<CustomerData> Customers { get; set; }

        [JsonIgnore]
        public CustomerData Customer
        {
            get
            {
                return Customers.First();
            }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var json = "{\"CustomerData\":{\"Name\":\"Jan\",\"Surname\":\"Kowalski\",\"Address\":\"Warszawa\"}}";

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new CustomerPartConverter());

            var customerPart = JsonConvert.DeserializeObject<CustomerPart>(json, settings);

            var jsonPath = @"C:\Users\dante\Desktop\Istotne\MojeDane\2024\lipiec\19_07_2024\Inne\serialized.json";
            var serialized = JsonConvert.SerializeObject(customerPart);
            File.WriteAllText(jsonPath, serialized);

            var customerPartNew = JsonConvert.DeserializeObject<CustomerPart>(json, settings);

            Console.WriteLine("Hello, World!");
        }
    }
}