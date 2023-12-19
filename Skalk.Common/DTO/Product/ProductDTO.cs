using Newtonsoft.Json;

namespace Skalk.Common.DTO.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Mpn { get; set; }
        public string? ManufacturerName { get; set; }
        public CompanyDTO? Company { get; set; }
        public List<OfferDTO>? Offers { get; set; }
    }


    public class OfferDTO
    {
        public int? Id { get; set; }
        public string? ClickUrl { get; set; }
        public int InventoryLevel { get; set; }
        public int? Moq { get; set; }    
        public List<PriceDTO>? Prices { get; set; }
    }

    public class PriceDTO
    {
        public decimal PriceValue { get; set; }
        public string? Currency { get; set; }
        public int Quantity { get; set; }
    }

    public class CompanyDTO
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
    }
}