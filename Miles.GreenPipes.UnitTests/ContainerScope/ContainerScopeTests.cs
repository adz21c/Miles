using FakeItEasy;
using GreenPipes;
using Microsoft.Practices.ServiceLocation;
using Miles.GreenPipes.ContainerScope;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.GreenPipes.UnitTests.ContainerScope
{
    [TestFixture]
    public class ContainerScopeTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task Given_ABus_When_XScopesConfigured_Then_XScopesPushed(int targetDepth)
        {
            // Arrange
            int containerDepth = 0;
            var resolveStack = new Stack<Resolved>();

            var containerStack = A.Fake<IContainerStack>();
            A.CallTo(() => containerStack.PushScope(A<ExistingContext>._)).Invokes(() =>
            {
                containerDepth++;
                resolveStack.Push(new Resolved { Value = containerDepth });
            });
            A.CallTo(() => containerStack.PopScope()).Invokes(() =>
            {
                containerDepth--;
                resolveStack.Pop();
            });
            A.CallTo(() => containerStack.GetInstance<Resolved>()).ReturnsLazily(() => resolveStack.Peek());

            var containerStackFactory = A.Fake<IContainerStackFactory>();
            A.CallTo(() => containerStackFactory.Create(A<ExistingContext>.Ignored)).Returns(containerStack);

            // Act and Assert
            var pipe = Pipe.New<ExistingContext>(pc =>
                {
                    for (var i = 0; i < targetDepth; ++i)
                        pc.UseContainerScope(containerStackFactory);

                    pc.UseInlineFilter((ctx, next) =>
                    {
                        if (!ctx.TryGetPayload(out IServiceLocator locator))
                            Assert.Fail("No service locator");

                        var resolved = locator.GetInstance<Resolved>();
                        Assert.AreEqual(targetDepth, resolved.Value);

                        return next.Send(ctx);
                    });
                });

            await pipe.Send(new ExistingContextImp());
        }

        public class Resolved
        {
            public int Value { get; set; }
        }

        public class ExistingContextImp : BasePipeContext, ExistingContext
        { }

        public interface ExistingContext : PipeContext
        { }
    }

}
