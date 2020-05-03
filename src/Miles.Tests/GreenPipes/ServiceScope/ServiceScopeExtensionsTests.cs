using FakeItEasy;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using Miles.GreenPipes.ServiceScope;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes.ServiceScope
{
    [TestFixture]
    public class ServiceScopeExtensionsTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Given_PipeConfigurator_When_UseServiceScope_Then_Specification(bool hasRoot)
        {
            var rootServiceProvider = A.Dummy<IServiceProvider>();

            var pipeConfigurator = A.Fake<IPipeConfigurator<PipeContext>>();

            pipeConfigurator.UseServiceScope(hasRoot ? rootServiceProvider : null);

            if (hasRoot)
            {
                A.CallTo(() => pipeConfigurator.AddPipeSpecification(
                    A<ServiceScopeSpecification<PipeContext>>.That.NullCheckedMatches(
                        s => s.RootServiceProvider == rootServiceProvider,
                        w => w.Write("Specification")))).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => pipeConfigurator.AddPipeSpecification(
                    A<ServiceScopeSpecification<PipeContext>>.That.NullCheckedMatches(
                        s => s.RootServiceProvider == null,
                        w => w.Write("Specification")))).MustHaveHappenedOnceExactly();
            }    
        }
    }

}
