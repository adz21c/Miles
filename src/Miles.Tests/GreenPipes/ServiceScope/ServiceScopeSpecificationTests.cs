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
    public class ServiceScopeSpecificationTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Given_ScopeServiceAndRoot_When_Validate_Then_Validation(bool hasRoot)
        {
            var rootServiceProvider = A.Dummy<IServiceProvider>();

            var sut = new ServiceScopeSpecification<PipeContext>(hasRoot ? rootServiceProvider : null);

            var result = sut.Validate();

            Assert.That(result, Is.Not.Null);

            var resultList = result.ToList();

            Assert.That(result, Is.Not.Null);

            if (hasRoot)
            {
                Assert.That(resultList, Is.Empty);
            }
            else
            {
                Assert.That(resultList.Count, Is.EqualTo(1));

                var validationResult = resultList.Single();
                Assert.That(validationResult.Key, Is.EqualTo("rootServiceProvider"));
                Assert.That(validationResult.Disposition, Is.EqualTo(ValidationResultDisposition.Warning));
                Assert.That(validationResult.Message, Is.EqualTo(ServiceScopeSpecification<PipeContext>.MustHaveServiceProvider));
            }
        }
        [TestCase(true)]
        [TestCase(false)]
        public void Given_ScopeServiceAndRoot_When_Apply_Then_Filter(bool hasRoot)
        {
            var rootServiceProvider = A.Dummy<IServiceProvider>();
            var pipeBuilder = A.Fake<IPipeBuilder<PipeContext>>();

            var sut = new ServiceScopeSpecification<PipeContext>(hasRoot ? rootServiceProvider : null);
            sut.Apply(pipeBuilder);

            if (hasRoot)
            {
                A.CallTo(() => pipeBuilder.AddFilter(
                    A<ServiceScopeFilter<PipeContext>>.That.NullCheckedMatches(
                        f => f.RootServiceProvider == rootServiceProvider,
                        w => w.Write("Filter"))));
            }
            else
            {
                A.CallTo(() => pipeBuilder.AddFilter(
                    A<ServiceScopeFilter<PipeContext>>.That.NullCheckedMatches(
                        f => f.RootServiceProvider == null,
                        w => w.Write("Filter"))));
            }
        }
    }

}
