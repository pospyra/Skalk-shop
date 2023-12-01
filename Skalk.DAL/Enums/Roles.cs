using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Skalk.DAL.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Roles
    {
        Manager,
        User
    }
}
