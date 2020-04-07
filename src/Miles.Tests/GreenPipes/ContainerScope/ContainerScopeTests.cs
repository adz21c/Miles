using FakeItEasy;
using GreenPipes;
using Miles.DependencyInjection;
using Miles.GreenPipes.ContainerScope;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.ContainerScope
{
    [TestFixture]
    public class ContainerScopeTests
    {
        [Test]
        public async Task Given_ABus_When_XScopesConfigured_Then_XScopesPushed()
        {
            var innerInnerGuid = Guid.NewGuid();
            var innerInnerContainer = A.Fake<IContainer>();
            A.CallTo(() => innerInnerContainer.Resolve<Resolved>(A<string>._)).Returns(new Resolved { Value = innerInnerGuid });
            var innerInnerContainerBuilder = A.Fake<IContainerBuilder>();
            A.CallTo(() => innerInnerContainerBuilder.CreateContainer()).Returns(innerInnerContainer);

            var innerGuid = Guid.NewGuid();
            var innerContainer = A.Fake<IContainer>();
            A.CallTo(() => innerContainer.CreateChildScopeBuilder()).Returns(innerInnerContainerBuilder);
            A.CallTo(() => innerContainer.Resolve<Resolved>(A<string>._)).Returns(new Resolved { Value = innerGuid });
            var innerContainerBuilder = A.Fake<IContainerBuilder>();
            A.CallTo(() => innerContainerBuilder.CreateContainer()).Returns(innerContainer);

            var rootGuid = Guid.NewGuid();
            var rootContainer = A.Fake<IContainer>();
            A.CallTo(() => rootContainer.CreateChildScopeBuilder()).Returns(innerContainerBuilder);
            A.CallTo(() => rootContainer.Resolve<Resolved>(A<string>._)).Returns(new Resolved { Value = rootGuid });

            // Act and Assert
            var pipe = Pipe.New<ExistingContext>(pc =>
                {
                    pc.UseContainerScope(rootContainer);
                    pc.UseInlineFilter((ctx, next) =>
                    {
                        Assert.IsTrue(ctx.TryGetPayload(out ContainerScopeContext containerContext), "No ContainerScopeContext locator");

                        var resolved = containerContext.Container.Resolve<Resolved>();
                        Assert.AreEqual(rootGuid, resolved.Value);

                        return next.Send(ctx);
                    });

                    pc.UseContainerScope();
                    pc.UseInlineFilter((ctx, next) =>
                    {
                        Assert.IsTrue(ctx.TryGetPayload(out ContainerScopeContext containerContext), "No ContainerScopeContext locator");

                        var resolved = containerContext.Container.Resolve<Resolved>();
                        Assert.AreEqual(innerGuid, resolved.Value);

                        return next.Send(ctx);
                    });

                    pc.UseContainerScope();
                    pc.UseInlineFilter((ctx, next) =>
                    {
                        Assert.IsTrue(ctx.TryGetPayload(out ContainerScopeContext containerContext), "No ContainerScopeContext locator");

                        var resolved = containerContext.Container.Resolve<Resolved>();
                        Assert.AreEqual(innerInnerGuid, resolved.Value);

                        return next.Send(ctx);
                    });
                });

            await pipe.Send(new ExistingContextImp());
        }

        public class Resolved
        {
            public Guid Value { get; set; }
        }

        public class ExistingContextImp : BasePipeContext, ExistingContext
        { }

        public interface ExistingContext : PipeContext
        { }
    }

}
