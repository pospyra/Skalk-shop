
namespace Skalk.DAL.Entities
{
    public class OrderContract
    {
        public int Id { get; set; }
        public byte[] Contract { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }
    }
}
