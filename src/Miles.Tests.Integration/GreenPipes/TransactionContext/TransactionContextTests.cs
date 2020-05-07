using FakeItEasy;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Tests.Integration.GreenPipes.TransactionContext
{
    [TestFixture]
    class TransactionContextTests
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private ServiceProvider _initialServiceProvider;
        private readonly ITransactionContext _transactionContext = A.Fake<ITransactionContext>();
        private readonly ITransaction _transaction = A.Fake<ITransaction>();

        [OneTimeSetUp]
        public void Initialise()
        {
            _serviceCollection.AddSingleton(_transactionContext);
            _initialServiceProvider = _serviceCollection.BuildServiceProvider();

            A.CallTo(() => _transactionContext.BeginAsync(null)).Returns(_transaction);
        }

        [Test]
        public async Task ApplyTransactionContext()
        {
            var pipe = Pipe.New<TestContext>(p =>
            {
                p.UseInlineFilter((ctx, next) =>
                {
                    ctx.GetOrAddPayload(() => _initialServiceProvider);
                    return next.Send(ctx);
                });

                p.UseTransactionContext();

                p.UseInlineFilter((ctx, next) =>
                {
                    return next.Send(ctx);
                });
            });

            await pipe.Send(new TestContextImp());
        }

        public interface TestContext : PipeContext
        { }

        class TestContextImp : BasePipeContext, TestContext
        {

        }
    }
}
