using System;
using System.Web;

namespace Miles.MassTransit.AspNet
{
    public class RequestActivityContext : IActivityContext
    {
        // Matches the Serilog naming and implementation
        public const string RequestIdItemName = "HttpRequestId";

        public const string CorrelationIdItemName = "CorrelationId";

        public RequestActivityContext(HttpContextBase httpContext)
        {
            var requestIdItem = httpContext.Items[RequestIdItemName];
            if (requestIdItem == null)
                httpContext.Items[RequestIdItemName] = ActivityId = Guid.NewGuid();
            else
                ActivityId = (Guid)requestIdItem;

            var correlationIdItem = httpContext.Items[CorrelationIdItemName];
            if (correlationIdItem == null)
                httpContext.Items[CorrelationIdItemName] = CorrelationId = Guid.NewGuid();
            else
                CorrelationId = (Guid)correlationIdItem;
        }

        public Guid ActivityId { get; private set; }

        public Guid CorrelationId { get; private set; }
    }
}
