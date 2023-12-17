using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Skalk.DAL.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        Processing,
        Sent,
        Delivered,
        Received
    }
}
