﻿using Miles.MassTransit.MessageDeduplication;
using System.Data.Entity.ModelConfiguration;

namespace Miles.MassTransit.EntityFramework.Configurations.Miles.MassTransit.MessageDeduplication
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
