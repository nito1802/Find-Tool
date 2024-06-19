using System.Xml.Serialization;

namespace Pull_Projects
{
    [XmlRoot("Textbolds")]
    public class Textbolds
    {
        [XmlElement("Textbold")]
        public List<Textbold> TextboldList { get; set; }
    }

    public class Textbold
    {
        public string Width { get; set; }
    }
}