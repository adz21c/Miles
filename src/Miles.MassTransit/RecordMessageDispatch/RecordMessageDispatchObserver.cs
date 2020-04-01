using MassTransit;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit.RecordMessageDispatch
{
    class RecordMessageDispatchObserver : ISendObserver
    {
        private readonly IDispatchRecorder dispatchRecorder;

        public RecordMessageDispatchObserver(IDispatchRecorder dispatchRecorder)
        {
            this.dispatchRecorder = dispatchRecorder;
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            return dispatchRecorder.RecordAsync(context);
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
