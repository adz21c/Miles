/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.Tests
{
    [TestFixture]
    public class HookTests
    {
        [Test]
        public async Task Given_Callback_When_Execute_Then_Called()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();

            // Act
            hook.Register(callback);
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Given_MultipleCallbacks_When_Execute_Then_AllCalled()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var callback2 = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback2.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();

            // Act
            hook.Register(callback);
            hook.Register(callback2);
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => callback2.Invoke(A<object>._, A<EventArgs>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Given_Callback_When_RegisterMultipleTimes_Then_RegisteredOnce()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();

            // Act
            hook.Register(callback);
            hook.Register(callback);
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Given_NotRegisteredCallback_When_UnRegister_Then_HasNoAffect()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();

            // Act
            hook.UnRegister(callback);
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MultipleRegisteredCallback_When_UnRegisterOne_Then_OthersUnaffected()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var callback2 = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback2.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();
            hook.Register(callback);
            hook.Register(callback2);

            // Act
            hook.UnRegister(callback2);
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => callback2.Invoke(A<object>._, A<EventArgs>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_RegisteredCallback_When_UnRegister_Then_Unaffected()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();
            hook.Register(callback);

            // Act
            hook.UnRegister(callback);
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Given_RegisteredCallback_When_ExecuteMultipleTimes_Then_ExecutedMultipleTimes()
        {
            // Arrange
            var callback = A.Fake<Func<object, EventArgs, Task>>();
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).Returns(Task.CompletedTask);

            var hook = new Hook<object, EventArgs>();
            hook.Register(callback);

            // Act
            await hook.ExecuteAsync(new object(), new EventArgs());
            await hook.ExecuteAsync(new object(), new EventArgs());

            // Assert
            A.CallTo(() => callback.Invoke(A<object>._, A<EventArgs>._)).MustHaveHappenedTwiceExactly();
        }
    }
}
