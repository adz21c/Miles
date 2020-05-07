using MassTransit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Miles.MassTransit.RecordMessageDispatch
{
    class RecordMessageDispatchObserver : ISendObserver
    {
        private readonly IDispatchRecorder _dispatchRecorder;

        public RecordMessageDispatchObserver(IDispatchRecorder dispatchRecorder)
        {
            _dispatchRecorder = dispatchRecorder ?? throw new ArgumentNullException(nameof(dispatchRecorder));
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            return _dispatchRecorder.RecordAsync(context);
        }

        [ExcludeFromCodeCoverage]
        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        [ExcludeFromCodeCoverage]
        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
