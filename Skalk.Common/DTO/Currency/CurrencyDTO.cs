using System.Xml.Serialization;

namespace Skalk.Common.DTO.Currency
{
    public class CurrencyDTO
    {
        [XmlAttribute(AttributeName = "ID")]
        public string Id { get; set; } = String.Empty;

        public int NumCode { get; set; }

        public string CharCode { get; set; } = String.Empty;

        public int Nominal { get; set; }

        public string Name { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;

        public string VunitRate { get; set; } = String.Empty;

    }
}
