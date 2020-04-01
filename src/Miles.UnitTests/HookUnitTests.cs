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
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.UnitTests
{
    [TestFixture]
    public class HookUnitTests
    {
        [Test]
        public void Register_RegistersProcessor_WhenNoProcessorsRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            // Act
            bool executed = false;
            hook.Register((s, e) =>
            {
                executed = true;
                return Task.FromResult(0);
            });
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executed, Is.True);
        }

        [Test]
        public void Register_RegistersProcessors_WhenNoProcessorsRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            // Act
            bool executedOne = false;
            hook.Register((s, e) =>
            {
                executedOne = true;
                return Task.FromResult(0);
            });
            bool executedTwo = false;
            hook.Register((s, e) =>
            {
                executedTwo = true;
                return Task.FromResult(0);
            });

            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executedOne, Is.True);
            Assert.That(executedTwo, Is.True);
        }

        [Test]
        public void Register_DoesNotRegister_IfAlreadyRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();
            int calls = 0;
            var handler = new Func<object, EventArgs, Task>((s, e) =>
            {
                ++calls;
                return Task.FromResult(0);
            });

            // Act
            hook.Register(handler);
            hook.Register(handler);

            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(calls, Is.EqualTo(1));
        }

        [Test]
        public void UnRegister_HasNoAffect_WhenNoProcessorsRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            bool executed = false;
            Func<object, EventArgs, Task> method = (s, e) =>
            {
                executed = true;
                return Task.FromResult(0);
            };

            // Act
            hook.UnRegister(method);
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executed, Is.False);
        }

        [Test]
        public void UnRegister_HasNoAffect_WhenOthersRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            bool executedOne = false;
            Func<object, EventArgs, Task> methodOne = (s, e) =>
            {
                executedOne = true;
                return Task.FromResult(0);
            };
            hook.Register(methodOne);

            bool executedTwo = false;
            Func<object, EventArgs, Task> methodTwo = (s, e) =>
            {
                executedTwo = true;
                return Task.FromResult(0);
            };

            // Act
            hook.UnRegister(methodTwo);
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executedOne, Is.True);
            Assert.That(executedTwo, Is.False);
        }

        [Test]
        public void UnRegister_UnRegisters_WhenRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            bool executed = false;
            Func<object, EventArgs, Task> method = (s, e) =>
            {
                executed = true;
                return Task.FromResult(0);
            };
            hook.Register(method);

            // Act
            hook.UnRegister(method);
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executed, Is.False);
        }

        [Test]
        public void UnRegister_UnRegistersButLeavesOthers_WhenManyRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            bool executedOne = false;
            Func<object, EventArgs, Task> methodOne = (s, e) =>
            {
                executedOne = true;
                return Task.FromResult(0);
            };
            hook.Register(methodOne);

            bool executedTwo = false;
            Func<object, EventArgs, Task> methodTwo = (s, e) =>
            {
                executedTwo = true;
                return Task.FromResult(0);
            };
            hook.Register(methodTwo);

            // Act
            hook.UnRegister(methodOne);
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executedOne, Is.False);
            Assert.That(executedTwo, Is.True);
        }

        [Test]
        public void ExecuteAsync_RunsEachHook_WhenManyRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            bool executedOne = false;
            Func<object, EventArgs, Task> methodOne = (s, e) =>
            {
                executedOne = true;
                return Task.FromResult(0);
            };
            hook.Register(methodOne);

            bool executedTwo = false;
            Func<object, EventArgs, Task> methodTwo = (s, e) =>
            {
                executedTwo = true;
                return Task.FromResult(0);
            };
            hook.Register(methodTwo);

            // Act
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(executedOne, Is.True);
            Assert.That(executedTwo, Is.True);
        }

        [Test]
        public void ExecuteAsync_RunsEachHookMultipleTimes_WhenManyRegistered()
        {
            // Arrange
            var hook = new Hook<object, EventArgs>();

            int callOneCount = 0;
            Func<object, EventArgs, Task> methodOne = (s, e) =>
            {
                ++callOneCount;
                return Task.FromResult(0);
            };
            hook.Register(methodOne);

            int callTwoCount = 0;
            Func<object, EventArgs, Task> methodTwo = (s, e) =>
            {
                ++callTwoCount;
                return Task.FromResult(0);
            };
            hook.Register(methodTwo);

            // Act
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            // Assert
            Assert.That(callOneCount, Is.EqualTo(2));
            Assert.That(callTwoCount, Is.EqualTo(2));
        }
    }
}
