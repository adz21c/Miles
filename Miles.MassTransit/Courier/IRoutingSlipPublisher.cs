using MassTransit.Courier.Contracts;

namespace Miles.MassTransit.Courier
{
    public interface IRoutingSlipPublisher
    {
        void Publish(RoutingSlip slip);
    }
}
