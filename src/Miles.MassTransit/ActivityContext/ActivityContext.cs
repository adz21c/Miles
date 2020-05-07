using System;

namespace Miles.MassTransit.ActivityContext
{
    class ActivityContext : IActivityContext
    {
        public Guid ActivityId { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
