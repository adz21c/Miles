using FakeItEasy;
using GreenPipes;
using Miles.GreenPipes.ServiceScope;
using NUnit.Framework;
using System;
using System.Linq;

namespace Miles.Tests.GreenPipes.ServiceScope
{
    [TestFixture]
    public class ServiceScopeContextImpTests
    {
        [Test]
        public void Given_ScopeServiceContext_When_AssignServiceProvider_Then_ServiceProviderAssigned()
        {
            var serviceProvider = A.Dummy<IServiceProvider>();
            var innerPipeContext = A.Dummy<PipeContext>();

            var sut = new ServiceScopeContextImp(innerPipeContext);
            Assume.That(sut.ServiceProvider, Is.Null);

            sut.AssignContainer(serviceProvider);

            Assert.That(sut.ServiceProvider, Is.SameAs(serviceProvider));
            Assert.That(sut.TryGetPayload(out IServiceProvider payload), Is.True);
            Assert.That(payload, Is.SameAs(serviceProvider));
        }

        [Test]
        public void Given_ScopeServiceContext_When_Ctor_Then_ServiceProviderNotAssigned()
        {
            var innerPipeContext = A.Dummy<PipeContext>();

            var sut = new ServiceScopeContextImp(innerPipeContext);
            
            Assert.That(sut.ServiceProvider, Is.Null);
        }

        [Test]
        public void Given_ScopeServiceContextWithProvider_When_AssignServiceProvider_Then_Exception()
        {
            var serviceProvider = A.Dummy<IServiceProvider>();
            var innerPipeContext = A.Dummy<PipeContext>();

            var sut = new ServiceScopeContextImp(innerPipeContext);
            Assume.That(sut.ServiceProvider, Is.Null);
            sut.AssignContainer(serviceProvider);
            Assume.That(sut.ServiceProvider, Is.Not.Null);

            var ex = Assert.Throws<InvalidOperationException>(() => sut.AssignContainer(serviceProvider));
            Assert.That(ex.Message, Is.EqualTo("ServiceProvider already assigned"));
        }
    }
}
