using GreenPipes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Miles.GreenPipes.UnitTests
{
    [TestFixture]
    public class ContextProxyFactoryTests
    {
        [Test]
        public void Given_NullNewContext_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(null, typeof(NewContextImp), new ExistingContextImp()));
        }

        [Test]
        public void Given_ClassNewContext_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContextImp), typeof(NewContextImp), new ExistingContextImp()));
        }

        [Test]
        public void Given_NonPipeContextNewContext_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(Interface), typeof(NewContextImp), new ExistingContextImp()));
        }

        [Test]
        public void Given_NullNewContextMixin_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(typeof(NewContext), null, new ExistingContextImp()));
        }

        [Test]
        public void Given_NewContextMixinInterface_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(Interface), new ExistingContextImp()));
        }

        [Test]
        public void Given_NewContextMixinGenericTypeDef_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(GenericTypeDef<>), new ExistingContextImp()));
        }

        [Test]
        public void Given_NewContextMixinNotImplementingNewContextType_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(ContextImp), new ExistingContextImp()));
        }
        [Test]
        public void Given_NewContextMixinNonBasePipeContext_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(BaselessContextImp), new ExistingContextImp()));
        }

        [Test]
        public void Given_NullContext_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(NewContextImp), null));
        }

        [Test]
        public void Given_NullConstructorArgs_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(NewContextImp), new ExistingContextImp(), null));
        }

        [Test]
        public void Given_ContextlessConstructorArgs_When_Create_Then_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextProxyFactory.Create(typeof(NewContext), typeof(NewContextImp), new ExistingContextImp(), 1));
        }

        [Test]
        public void Given_ValidArguments_When_Create_Then_NewContext()
        {
            var existingContext = new ExistingContextImp();

            var newContext = ContextProxyFactory.Create(typeof(NewContext), typeof(NewContextImp), existingContext, existingContext);

            Assert.IsNotNull(newContext as NewContext);
            Assert.IsNotNull(newContext as ExistingContext);
        }

        #region MockTypes

        public interface Interface
        {

        }

        public class GenericTypeDef<T>
        {

        }

        public class ContextImp : BasePipeContext
        {

        }

        public class BaselessContextImp : NewContext
        {
            public CancellationToken CancellationToken => throw new NotImplementedException();

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
