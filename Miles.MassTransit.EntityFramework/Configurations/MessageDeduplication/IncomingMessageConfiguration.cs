using Miles.MassTransit.EntityFramework.MessageDeduplication;
using System.Data.Entity.ModelConfiguration;

namespace Miles.MassTransit.EntityFramework.Configurations.MessageDeduplication
{
    public class IncomingMessageConfiguration : EntityTypeConfiguration<IncomingMessage>
    {
        public IncomingMessageConfiguration()
        {
            HasKey(x => new { x.MessageId, x.QueueName });
            Property(x => x.QueueName).HasMaxLength(255).IsRequired();
        }
    }
}
