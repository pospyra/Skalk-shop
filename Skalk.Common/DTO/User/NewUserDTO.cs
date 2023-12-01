using Skalk.DAL.Enums;

namespace Skalk.Common.DTO.User
{
    public class NewUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public Roles Role { get; set; }
    }
}
