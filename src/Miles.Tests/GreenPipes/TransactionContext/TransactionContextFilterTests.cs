using FakeItEasy;
using GreenPipes;
using Miles.GreenPipes.TransactionContext;
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.TransactionContext
{
    [TestFixture]
    public class TransactionContextFilterTests
    {
        private IServiceProvider _serviceProvider = A.Fake<IServiceProvider>();
        private readonly ITransactionContext _transactionContext = A.Fake<ITransactionContext>();
        private readonly ITransaction _transaction = A.Fake<ITransaction>();
        private readonly TestContext _context = A.Fake<TestContext>();
        private readonly IPipe<TestContext> _pipe = A.Fake<IPipe<TestContext>>();

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            A.CallTo(() => _context.TryGetPayload(out _serviceProvider)).Returns(true);
            A.CallTo(() => _serviceProvider.GetService(typeof(ITransactionContext))).Returns(_transactionContext);
            A.CallTo(() => _transactionContext.BeginAsync(A<IsolationLevel?>._)).Returns(_transaction);
        }

        [TearDown]
        public void TearDown()
        {
            Fake.ClearRecordedCalls(_serviceProvider);
            Fake.ClearRecordedCalls(_transactionContext);
            Fake.ClearRecordedCalls(_transaction);
            Fake.ClearRecordedCalls(_pipe);
            Fake.ClearRecordedCalls(_context);
        }

        [Test]
        public async Task Given_TransactionContext_When_Send_Then_TransactionCreatedAndCommitted()
        {
            var sut = new TransactionContextFilter<TestContext>(null);
            await sut.Send(_context, _pipe);

            A.CallTo(() => _transactionContext.BeginAsync(A<IsolationLevel?>.That.IsNull())).MustHaveHappenedOnceExactly()
                .Then(A.CallTo(() => _pipe.Send(_context)).MustHaveHappenedOnceExactly())
                .Then(A.CallTo(() => _transaction.CommitAsync()).MustHaveHappenedOnceExactly())
                .Then(A.CallTo(() => _transaction.Dispose()).MustHaveHappenedOnceExactly());
        }

        [Test]
        public void Given_TransactionContext_When_Send_Then_TransactionCreatedAndRollback()
        {
            const IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;

            A.CallTo(() => _pipe.Send(_context)).Throws<InvalidOperationException>();

            var sut = new TransactionContextFilter<TestContext>(isolationLevel);
            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Send(_context, _pipe));

            A.CallTo(() => _transactionContext.BeginAsync(A<IsolationLevel?>.That.IsEqualTo(isolationLevel))).MustHaveHappenedOnceExactly()
                .Then(A.CallTo(() => _pipe.Send(_context)).MustHaveHappenedOnceExactly())
                .Then(A.CallTo(() => _transaction.RollbackAsync()).MustHaveHappenedOnceExactly())
                .Then(A.CallTo(() => _transaction.Dispose()).MustHaveHappenedOnceExactly());
        }

        public interface TestContext : PipeContext
        { }
    }
}
