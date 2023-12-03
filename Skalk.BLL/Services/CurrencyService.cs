using Skalk.BLL.IServices;
using Skalk.Common.DTO.Currency;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Skalk.BLL.Services
{
    public class CurrencyService : ICurrencyService
    {
        private static readonly HttpClient httpClient = new();
        private readonly string url = "http://www.cbr.ru/scripts/XML_daily.asp";

        public async Task<ICollection<CurrencyDTO>> GetCurrenciesAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var xmlData = XDocument.Parse(responseBody);


            ValCursDTO valCurs = new();
            if (xmlData is not null)
            {
                XmlSerializer serializer = new(typeof(ValCursDTO));

                using var reader = xmlData.Root.CreateReader();
                valCurs = serializer.Deserialize(reader) as ValCursDTO;

            }
            return valCurs.Valutes;
        }
    }
}
