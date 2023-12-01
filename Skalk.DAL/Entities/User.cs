using Skalk.DAL.Enums;

namespace Skalk.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string ContactInfo { get; set; }
        public Roles Role { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
