
namespace ESB.Data.Context.Bots.Mapping
{
    using ESB.Domain.Entities.Bots;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MessageInMapping : IEntityTypeConfiguration<MessageIn>
    {
        public void Configure(EntityTypeBuilder<MessageIn> builder)
        {
            builder.ToTable("Message");
            builder.HasKey(o => o.MessageInId);
        }
    }
}
