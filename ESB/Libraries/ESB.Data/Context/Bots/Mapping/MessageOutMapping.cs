
namespace ESB.Data.Context.Bots.Mapping
{
    using ESB.Domain.Entities.Bots;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MessageOutMapping : IEntityTypeConfiguration<MessageOut>
    {
        public void Configure(EntityTypeBuilder<MessageOut> builder)
        {
            builder.ToTable("MessageOut");
            builder.HasKey(o => o.MessageOutId);
        }
    }
}
