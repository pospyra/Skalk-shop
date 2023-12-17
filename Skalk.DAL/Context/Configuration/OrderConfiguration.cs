using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Skalk.DAL.Entities;

namespace Skalk.DAL.Context.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        private const int InitialId = 679;
        public void Configure(EntityTypeBuilder<Order> builder)
        {

        builder.HasKey(x => x.Id);

            builder.HasData(new Order { Id = InitialId});
        }
    }
}
