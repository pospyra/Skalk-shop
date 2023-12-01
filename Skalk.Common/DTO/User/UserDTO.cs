using Skalk.DAL.Enums;

namespace Skalk.Common.DTO.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? ContactInfo { get; set; }
        public Roles Role { get; set; }
    }
}
