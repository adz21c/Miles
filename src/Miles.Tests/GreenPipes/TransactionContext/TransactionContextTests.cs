using FakeItEasy;
using GreenPipes;
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.TransactionContext
{
    [TestFixture]
    public class TransactionContextTests
    {
        //[Test]
        //public async Task Given_ABusWithTransactionContext_When_Successful_Then_TransactionContextCommitted()
        //{
        //    // Arrange
        //    var transaction = A.Fake<ITransaction>();
        //    A.CallTo(() => transaction.CommitAsync()).Returns(Task.CompletedTask);

        //    var transactionContext = A.Fake<ITransactionContext>();
        //    A.CallTo(() => transactionContext.BeginAsync(A<IsolationLevel?>._)).Returns(transaction);

        //    var container = A.Fake<IContainer>();
        //    A.CallTo(() => container.Resolve<ITransactionContext>(null)).Returns(transactionContext);
        //    var containerScopeContext = A.Fake<ContainerScopeContext>();
        //    A.CallTo(() => containerScopeContext.Container).Returns(container);

        //    // Act
        //    var pipe = Pipe.New<TestContext>(pc =>
        //    {
        //        pc.UseInlineFilter((ctx, next) =>
        //        {
        //            ctx.GetOrAddPayload(() => containerScopeContext);
        //            return next.Send(ctx);
        //        });

        //        pc.UseTransactionContext();

        //        pc.UseInlineFilter((ctx, next) =>
        //        {
        //            return next.Send(ctx);
        //        });
        //    });

        //    await pipe.Send(new TestContext());

        //    // Assert
        //    A.CallTo(() => transactionContext.BeginAsync(A<IsolationLevel?>._)).MustHaveHappenedOnceExactly();
        //    A.CallTo(() => transaction.CommitAsync()).MustHaveHappenedOnceExactly();
        //}

        //[Test]
        //public void Given_ABusWithTransactionContext_When_Failure_Then_TransactionContextRolledback()
        //{
        //    // Arrange
        //    var transaction = A.Fake<ITransaction>();
        //    A.CallTo(() => transaction.RollbackAsync()).Returns(Task.CompletedTask);

        //    var transactionContext = A.Fake<ITransactionContext>();
        //    A.CallTo(() => transactionContext.BeginAsync(A<IsolationLevel?>._)).Returns(transaction);

        //    var container = A.Fake<IContainer>();
        //    A.CallTo(() => container.Resolve<ITransactionContext>(null)).Returns(transactionContext);
        //    var containerScopeContext = A.Fake<ContainerScopeContext>();
        //    A.CallTo(() => containerScopeContext.Container).Returns(container);

        //    // Act
        //    var pipe = Pipe.New<TestContext>(pc =>
        //    {
        //        pc.UseInlineFilter((ctx, next) =>
        //        {
        //            ctx.GetOrAddPayload(() => containerScopeContext);
        //            return next.Send(ctx);
        //        });

        //        pc.UseTransactionContext();

        //        pc.UseInlineFilter((ctx, next) =>
        //        {
        //            throw new Exception();
        //        });
        //    });

        //    Assert.ThrowsAsync<Exception>(() => pipe.Send(new TestContext()));

        //    // Assert
        //    A.CallTo(() => transactionContext.BeginAsync(A<IsolationLevel?>._)).MustHaveHappenedOnceExactly();
        //    A.CallTo(() => transaction.RollbackAsync()).MustHaveHappenedOnceExactly();
        //}

        //public class TestContext : BasePipeContext, PipeContext
        //{ }
    }
}
