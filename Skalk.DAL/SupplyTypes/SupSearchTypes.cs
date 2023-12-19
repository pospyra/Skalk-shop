using Newtonsoft.Json;

namespace Skalk.DAL.SupplyTypes
{
    public class Request
    {
        [JsonProperty("query")]
        public string? Query { get; set; }

        [JsonProperty("variables")]
        public Dictionary<string, object>? Variables { get; set; }
    }

    public class Response
    {
        [JsonProperty("data")]
        public Data? Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("supSearch")]
        public SupSearch? SupSearch { get; set; }

        [JsonProperty("supMultiMatch")]
        public List<SupMultiMatch> SupMultiMatch { get; set; }

    }

    public class SupMultiMatch
    {
        [JsonProperty("hits")]
        public int Hits { get; set; }

        [JsonProperty("parts")]
        public List<Part>? Parts { get; set; }
    }

    public class SupSearch
    {
        [JsonProperty("results")]
        public List<Result>? Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("part")]
        public Part? Part { get; set; }
    }

    public class Part
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mpn")]
        public string? Mpn { get; set; }

        [JsonProperty("manufacturer")]
        public Manufacturer? Manufacturer { get; set; }

        [JsonProperty("sellers")]
        public List<Seller>? Sellers { get; set; }
    }

    public class Manufacturer
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class Seller
    {
        [JsonProperty("company")]
        public Company Company { get; set; }

        [JsonProperty("offers")]
        public List<Offer>? Offers { get; set; }
    }

    public class Company
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class Offer
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("clickUrl")]
        public string? ClickUrl { get; set; }

        [JsonProperty("inventoryLevel")]
        public int InventoryLevel { get; set; }

        [JsonProperty("moq")]
        public int? Moq { get; set; }

        [JsonProperty("prices")]
        public List<Price>? Prices { get; set; }
    }

    public class Price
    {
        [JsonProperty("price")]
        public decimal PriceValue { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    // Additional functions to handle the retrieved data could be added here
}