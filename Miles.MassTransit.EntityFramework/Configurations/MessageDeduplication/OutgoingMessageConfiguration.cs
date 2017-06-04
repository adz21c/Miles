using Miles.MassTransit.EntityFramework.MessageDeduplication;
using System.Data.Entity.ModelConfiguration;

namespace Miles.MassTransit.EntityFramework.Configurations.MessageDeduplication
{
    public class OutgoingMessageConfiguration : EntityTypeConfiguration<OutgoingMessage>
    {
        public OutgoingMessageConfiguration()
        {
            HasKey(x => x.MessageId);
            Property(x => x.ClassTypeName).IsRequired().HasMaxLength(255);
        }
    }
}
