using FakeItEasy;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using Miles.GreenPipes.TransactionContext;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.TransactionContext
{
    [TestFixture]
    public class TransactionContextExtensionsTests
    {
        [Test]
        public void Given_PipeConfigurator_When_UseTransactionContext_Then_Specification()
        {
            var pipeConfigurator = A.Fake<IPipeConfigurator<PipeContext>>();
            var config = A.Dummy<Action<ITransactionContextConfigurator>>();

            pipeConfigurator.UseTransactionContext(config);

            A.CallTo(() => config.Invoke(A<ITransactionContextConfigurator>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => pipeConfigurator.AddPipeSpecification(A<TransactionContextSpecification<PipeContext>>.That.IsNotNull())).MustHaveHappenedOnceExactly();
        }
    }

}
