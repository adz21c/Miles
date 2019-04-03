using FakeItEasy;
using GreenPipes;
using Miles.GreenPipes.ContainerScope;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.GreenPipes.UnitTests.ContainerScope
{
    [TestFixture]
    public class ContainerScopeTests
    {
        [Test]
        public async Task Given_ABus_When_XScopesConfigured_Then_XScopesPushed()
        {
            var innerInnerGuid = Guid.NewGuid();
            var innerInnerContainer = A.Fake<IScopedServiceLocator>();
            A.CallTo(() => innerInnerContainer.GetInstance<Resolved>()).Returns(new Resolved { Value = innerInnerGuid });
            A.CallTo(() => innerInnerContainer.ContainerType).Returns("Fake");

            var innerGuid = Guid.NewGuid();
            var innerContainer = A.Fake<IScopedServiceLocator>();
            A.CallTo(() => innerContainer.GetInstance<Resolved>()).Returns(new Resolved { Value = innerGuid });
            A.CallTo(() => innerContainer.CreateChildScope(A<Type>._, A<PipeContext>._)).Returns(innerInnerContainer);
            A.CallTo(() => innerContainer.ContainerType).Returns("Fake");

            var rootGuid = Guid.NewGuid();
            var rootContainer = A.Fake<IScopedServiceLocator>();
            A.CallTo(() => rootContainer.GetInstance<Resolved>()).Returns(new Resolved { Value = rootGuid });
            A.CallTo(() => rootContainer.CreateChildScope(A<Type>._, A<PipeContext>._)).Returns(innerContainer);
            A.CallTo(() => rootContainer.ContainerType).Returns("Fake");

            // Act and Assert
            var pipe = Pipe.New<ExistingContext>(pc =>
                {
                    pc.UseContainerScope(rootContainer);
                    pc.UseContainerScope();
                    pc.UseContainerScope();

                    pc.UseInlineFilter((ctx, next) =>
                    {
                        Assert.IsTrue(ctx.TryGetPayload(out ContainerScopeContext containerContext), "No ContainerScopeContext locator");

                        var resolved = containerContext.ServiceLocator.GetInstance<Resolved>();
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
