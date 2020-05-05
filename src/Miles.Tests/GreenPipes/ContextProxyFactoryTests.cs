using GreenPipes;
using Miles.GreenPipes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Miles.Tests.GreenPipes
{
    [TestFixture]
    public class ContextProxyFactoryTests
    {
        [Test]
        public void Given_NullNewContextType_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(null, new NewContextImp(existingContext), typeof(ExistingContext), existingContext));
        }

        [Test]
        public void Given_NonPipeContextNewContextType_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(Interface), new NewContextImp(existingContext), typeof(ExistingContext), existingContext));
        }

        [Test]
        public void Given_NullNewContext_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(typeof(NewContext), null, typeof(ExistingContext), existingContext));
        }

        [Test]
        public void Given_NewContextNotNewContextType_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), new ContextImp(), typeof(ExistingContext), existingContext));
        }

        [Test]
        public void Given_NullExistingContextType_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(typeof(NewContext), new NewContextImp(existingContext), null, existingContext));
        }

        [Test]
        public void Given_NonPipeContextExistingContextType_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), new NewContextImp(existingContext), typeof(Interface), existingContext));
        }

        [Test]
        public void Given_NullExistingContext_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(typeof(NewContext), new NewContextImp(existingContext), typeof(ExistingContext), null));
        }

        [Test]
        public void Given_ExistingContextNotExistingContextType_When_Create_Then_ThrowException()
        {
            var existingContext = new ExistingContextImp();
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), new NewContextImp(existingContext), typeof(ExistingContext), new ContextImp()));
        }

        [Test]
        public void Given_ValidArguments_When_Create_Then_NewContext()
        {
            var existingContext = new ExistingContextImp();

            var newContext = ContextProxyFactory.Create(typeof(NewContext), new NewContextImp(existingContext), typeof(ExistingContext), existingContext);

            Assert.That(newContext, Is.Not.Null);
            Assert.That(newContext, Is.InstanceOf<NewContext>());
            Assert.That(newContext, Is.InstanceOf<ExistingContext>());
        }


        [Test]
        public void Given_AnExistingProxy_When_Create_Then_NewContext()
        {
            var existingContext = new ExistingContextImp();

            var newContext = ContextProxyFactory.Create(typeof(NewContext), new NewContextImp(existingContext), typeof(ExistingContext), existingContext);

            Assert.That(newContext, Is.Not.Null);
            Assert.That(newContext, Is.InstanceOf<NewContext>());
            Assert.That(newContext, Is.InstanceOf<ExistingContext>());

            var secondNewContext = ContextProxyFactory.Create(typeof(NewContext), new NewContextImp((NewContext)newContext), typeof(NewContext), (NewContext)newContext);


            Assert.That(secondNewContext, Is.Not.Null);
            Assert.That(secondNewContext, Is.InstanceOf<NewContext>());
            Assert.That(secondNewContext, Is.Not.InstanceOf<ExistingContext>());
        }

        #region MockTypes

        public interface Interface
        {

        }

        public class ContextImp : PipeContext
        {
            public CancellationToken CancellationToken => throw new NotImplementedException();

            public T AddOrUpdatePayload<T>(PayloadFactory<T> addFactory, UpdatePayloadFactory<T> updateFactory) where T : class
            {
                throw new NotImplementedException();
            }

            public TPayload GetOrAddPayload<TPayload>(PayloadFactory<TPayload> payloadFactory) where TPayload : class
            {
                throw new NotImplementedException();
            }

            public bool HasPayloadType(Type payloadType)
            {
                throw new NotImplementedException();
            }

            public bool TryGetPayload<TPayload>(out TPayload payload) where TPayload : class
            {
                throw new NotImplementedException();
            }
        }

        public interface NewContext : PipeContext
        {

        }

        public class NewContextImp : BasePipeContext, NewContext
        {
            public NewContextImp(PipeContext ctx) : base(ctx)
            { }
        }

        public interface ExistingContext : PipeContext
        {

        }

        public class ExistingContextImp : BasePipeContext, ExistingContext
        {

        }

        #endregion
    }
}
