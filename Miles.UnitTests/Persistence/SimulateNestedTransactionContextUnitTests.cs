﻿/*
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
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Miles.UnitTests.Persistence
{
    [TestFixture]
    public class SimulateNestedTransactionContextUnitTests
    {
        [Test]
        public async Task BeginAsync_CreatesAndRollsbackTransaction_WhenCommitOrRollbackNotCalled()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                // Do nothing
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened();
        }

        [Test]
        public async Task BeginAsync_CreatesAndRollsbackTransactionOnce_WhenCommitOrRollbackNotCalledAndTwoTransactionsExist()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    // Do nothing
                }
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
        }

        [Test]
        public async Task RollbackAsync_CreatesAndRollsbackTransaction_WhenOnlyOnceTransactionInstance()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                await transaction.RollbackAsync();
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened();
        }

        [Test]
        public async Task RollbackAsync_CreatesAndRollsbackTransactionOnce_WhenMoreThanOneTransactionInstanceExists()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.RollbackAsync();
                }
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
        }

        [Test]
        public async Task RollbackAsync_ThrowsExceptionOnSecondRollbackAttempt_WhenTransactionIsAlreadyRolledback()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.RollbackAsync();
                }

                Assert.ThrowsAsync<InvalidOperationException>(async () => await transaction.RollbackAsync());
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
        }


        [Test]
        public async Task CommitAsync_CreatesAndCommitsTransaction_WhenOnlyOnceTransactionInstance()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustNotHaveHappened();
        }

        [Test]
        public async Task CommitAsync_CreatesAndCommitsTransactionOnce_WhenMoreThanOneTransactionInstanceExists()
        {
            // Arrange
            var transactionContext = A.Fake<SimulateNestedTransactionContext>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.CommitAsync();
                }

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustNotHaveHappened();
        }
    }
}
