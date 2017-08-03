namespace Miles.Sample.Persistence.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutgoingMessageDispatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutgoingMessages", "SourceAddress", c => c.String(maxLength: 255));
            AddColumn("dbo.OutgoingMessages", "DestinationAddress", c => c.String(maxLength: 255));
            AddColumn("dbo.OutgoingMessages", "ResponseAddress", c => c.String(maxLength: 255));
            AddColumn("dbo.OutgoingMessages", "FaultAddress", c => c.String(maxLength: 255));
            AddColumn("dbo.OutgoingMessages", "RequestId", c => c.Guid());
            AddColumn("dbo.OutgoingMessages", "ConversationId", c => c.Guid());
            AddColumn("dbo.OutgoingMessages", "InitiatorId", c => c.Guid());
            AddColumn("dbo.OutgoingMessages", "ScheduledMessageId", c => c.Guid());
            AddColumn("dbo.OutgoingMessages", "TimeToLive", c => c.Time(precision: 7));
            AddColumn("dbo.OutgoingMessages", "Durable", c => c.Boolean());
            AddColumn("dbo.OutgoingMessages", "Mandatory", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutgoingMessages", "Mandatory");
            DropColumn("dbo.OutgoingMessages", "Durable");
            DropColumn("dbo.OutgoingMessages", "TimeToLive");
            DropColumn("dbo.OutgoingMessages", "ScheduledMessageId");
            DropColumn("dbo.OutgoingMessages", "InitiatorId");
            DropColumn("dbo.OutgoingMessages", "ConversationId");
            DropColumn("dbo.OutgoingMessages", "RequestId");
            DropColumn("dbo.OutgoingMessages", "FaultAddress");
            DropColumn("dbo.OutgoingMessages", "ResponseAddress");
            DropColumn("dbo.OutgoingMessages", "DestinationAddress");
            DropColumn("dbo.OutgoingMessages", "SourceAddress");
        }
    }
}
