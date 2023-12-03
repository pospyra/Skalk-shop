using System.Xml.Serialization;

namespace Skalk.Common.DTO.Currency
{
    [Serializable]
    [XmlRoot("ValCurs")]
    public class ValCursDTO
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } 

        [XmlAttribute("Date")]
        public string Date { get; set; }

        [XmlElement("Valute")]
        public List<CurrencyDTO> Valutes { get; set; }
    }
}