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
            int containerDepth = 0;

            var containerStack = A.Fake<IContainerStack>();
            A.CallTo(() => containerStack.PushScope(A<TestContext>._)).Invokes(() => containerDepth++);
            A.CallTo(() => containerStack.PopScope()).Invokes(() => containerDepth--);

            var containerStackFactory = A.Fake<IContainerStackFactory>();
            A.CallTo(() => containerStackFactory.Create(A<TestContext>.Ignored)).Returns(containerStack);

            var pipe = Pipe.New<TestContext>(pc =>
                {
                    for (var i = 0; i < targetDepth; ++i)
                        pc.UseContainerScope(containerStackFactory);

                    pc.UseInlineFilter((ctx, next) =>
                    {
                        containerDepth.ShouldBe(targetDepth);
                        return next.Send(ctx);
                    });
                });

            await pipe.Send(new TestContext());
        }

        public class TestContext : BasePipeContext, PipeContext
        { }
    }

}
