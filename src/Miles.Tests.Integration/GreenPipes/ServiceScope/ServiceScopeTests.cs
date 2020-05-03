using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.Tests.Integration.GreenPipes.ServiceScope
{
    [TestFixture]
    public class ServiceScopeTests
    {
        [Test]
        public async Task ScopeFromPayload()
        {
            var serviceCollection = new ServiceCollection();
            var initialServiceProvider = serviceCollection.BuildServiceProvider();
            
            var pipe = Pipe.New<TestContext>(p =>
            {
                p.UseInlineFilter((ctx, next) =>
                {
                    ctx.GetOrAddPayload(() => initialServiceProvider);
                    return next.Send(ctx);
                });

                p.UseServiceScope();

                p.UseInlineFilter((ctx, next) =>
                {
                    Assert.That(ctx.TryGetPayload(out ServiceScopeContext serviceContext), Is.True);
                    Assert.That(serviceContext?.ServiceProvider, Is.Not.Null);
                    Assert.That(serviceContext.ServiceProvider, Is.Not.SameAs(initialServiceProvider));
                    Assert.That(ctx.TryGetPayload(out IServiceProvider payload), Is.True);
                    Assert.That(payload, Is.Not.Null);
                    Assert.That(payload, Is.Not.SameAs(initialServiceProvider));
                    return next.Send(ctx);
                });
            });

            await pipe.Send(new TestContextImp());
        }

        [Test]
        public async Task ScopeFromSuppliedServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var initialServiceProvider = serviceCollection.BuildServiceProvider();

            var pipe = Pipe.New<TestContext>(p =>
            {
                p.UseServiceScope(initialServiceProvider);

                p.UseInlineFilter((ctx, next) =>
                {
                    Assert.That(ctx.TryGetPayload(out ServiceScopeContext serviceContext), Is.True);
                    Assert.That(serviceContext?.ServiceProvider, Is.Not.Null);
                    Assert.That(serviceContext.ServiceProvider, Is.Not.SameAs(initialServiceProvider));
                    Assert.That(ctx.TryGetPayload(out IServiceProvider payload), Is.True);
                    Assert.That(payload, Is.Not.Null);
                    Assert.That(payload, Is.Not.SameAs(initialServiceProvider));
                    return next.Send(ctx);
                });
            });

            await pipe.Send(new TestContextImp());
        }

        [Test]
        public async Task ScopeFromAnotherScope()
        {
            var serviceCollection = new ServiceCollection();
            var initialServiceProvider = serviceCollection.BuildServiceProvider();
            ServiceScopeContext firstScope = null;
            IServiceProvider firstServiceProvider = null;

            var pipe = Pipe.New<TestContext>(p =>
            {
                p.UseServiceScope(initialServiceProvider);

                p.UseInlineFilter((ctx, next) =>
                {
                    Assert.That(ctx.TryGetPayload(out firstScope), Is.True);
                    firstServiceProvider = firstScope?.ServiceProvider;
                    Assert.That(firstScope?.ServiceProvider, Is.Not.SameAs(initialServiceProvider));
                    return next.Send(ctx);
                });

                p.UseServiceScope();

                p.UseInlineFilter((ctx, next) =>
                {
                    Assert.That(ctx.TryGetPayload(out ServiceScopeContext serviceContext), Is.True);
                    Assert.That(serviceContext, Is.Not.SameAs(firstScope));
                    Assert.That(serviceContext?.ServiceProvider, Is.Not.Null);
                    Assert.That(serviceContext.ServiceProvider, Is.Not.SameAs(initialServiceProvider));
                    Assert.That(serviceContext.ServiceProvider, Is.Not.SameAs(firstServiceProvider));
                    Assert.That(ctx.TryGetPayload(out IServiceProvider payload), Is.True);
                    Assert.That(payload, Is.Not.Null);
                    Assert.That(payload, Is.Not.SameAs(initialServiceProvider));
                    Assert.That(payload, Is.Not.SameAs(firstServiceProvider));
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
