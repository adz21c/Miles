using FakeItEasy;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.ServiceScope
{
    [TestFixture]
    public class ServiceScopeFilterTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Given_ScopeServiceFilterAndRoot_When_Probe_Then_Config(bool hasRoot)
        {
            var probeContext = A.Fake<ProbeContext>();
            var filtersProbeContext = A.Fake<ProbeContext>();
            A.CallTo(() => probeContext.CreateScope(A<string>._)).Returns(filtersProbeContext);
            
            var rootServiceProvider = A.Dummy<IServiceProvider>();
            
            var sut = new ServiceScopeFilter<PipeContext>(hasRoot ? rootServiceProvider : null);

            sut.Probe(probeContext);

            A.CallTo(() => probeContext.CreateScope("filters")).MustHaveHappenedOnceExactly();
            A.CallTo(() => filtersProbeContext.Add("filterType", "service-scope")).MustHaveHappenedOnceExactly();
            A.CallTo(() => filtersProbeContext.Add("hasRoot", hasRoot)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Given_ScopeServiceFilter_When_Send_Then_ContextHasChildServiceScopeContext()
        {
            var rootServiceProvider = A.Fake<IServiceProvider>();
            var rootServiceScopeFactory = A.Fake<IServiceScopeFactory>();
            A.CallTo(() => rootServiceProvider.GetService(typeof(IServiceScopeFactory))).Returns(rootServiceScopeFactory);

            var childServiceScope = A.Dummy<IServiceScope>();
            A.CallTo(() => rootServiceScopeFactory.CreateScope()).Returns(childServiceScope);

            var childServiceProvider = A.Dummy<IServiceProvider>();
            A.CallTo(() => childServiceScope.ServiceProvider).Returns(childServiceProvider);
            
            var pipe = Pipe.New<ExistingContext>(p =>
            {
                // No service scope
                p.UseInlineFilter((ctx, next) =>
                {
                    Assert.That(ctx.TryGetPayload(out ServiceScopeContext scopeContext), Is.False);
                    Assert.That(scopeContext, Is.Null);
                    Assert.That(ctx.TryGetPayload(out IServiceProvider serviceProvider), Is.False);
                    Assert.That(serviceProvider, Is.Null);
                    return next.Send(ctx);
                });

                p.UseFilter(new ServiceScopeFilter<ExistingContext>(rootServiceProvider));

                // Has service scope
                p.UseInlineFilter((ctx, next) =>
                {
                    Assert.That(ctx.TryGetPayload(out ServiceScopeContext scopeContext), Is.True);
                    Assert.That(scopeContext, Is.Not.Null);
                    Assert.That(scopeContext.ServiceProvider, Is.SameAs(childServiceProvider));
                    Assert.That(ctx.TryGetPayload(out IServiceProvider serviceProvider), Is.True);
                    Assert.That(serviceProvider, Is.Not.Null);
                    Assert.That(serviceProvider, Is.SameAs(childServiceProvider));
                    return next.Send(ctx);
                });
            });

            await pipe.Send(new ExistingContextImp());
        }

        public class ExistingContextImp : BasePipeContext, ExistingContext
        { }

        public interface ExistingContext : PipeContext
        { }
    }

}
