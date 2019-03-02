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
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Miles.UnitTests.Persistence
{
    [TestFixture]
    public class TransactionContextBaseTests
    {
        [Test]
        public async Task Given_ASingleTransaction_When_Nothing_Then_RollsbackTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                // Do nothing
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MultipleNestedTransactions_When_Nothing_Then_RollsbackTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

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
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
        }

        [Test]
        public async Task Given_ASingleTransaction_When_RollbackAsyncCalledOnce_Then_RollsbackTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                await transaction.RollbackAsync();
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MultipleNestedTransactions_When_RollbackAsyncCalledOnceOnInnerTransaction_Then_RollsbackTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

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
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
        }

        [Test]
        public async Task Given_MultipleNestedTransactions_When_RollbackAsyncCalledOnceForEachTransaction_Then_RollsbackTransactionContextOnce()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.RollbackAsync();
                }

                await transaction.RollbackAsync();
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
        }


        [Test]
        public async Task Given_ASingleTransaction_When_CommitAsyncCalledOnce_Then_CommitsTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

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
        public async Task Given_MultipleNestedTransactions_When_CommitAsyncCalledOnceOnEachTransaction_Then_CommitsTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

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

        [Test]
        public async Task Given_MultipleNestedTransactions_When_CommitAsyncOnceOnTheInnerTransaction_Then_RollsbackTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.CommitAsync();
                }
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
        }

        [Test]
        public async Task Given_MultipleNestedTransactions_When_RollbackAsyncIsCalledAtleastOnce_Then_RollsbackTransactionContext()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.CommitAsync();
                }

                await transaction.RollbackAsync();
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
        }

        [Test]
        public async Task Given_MultipleNestedTransactions_When_CommitAsyncAfterRollbackAsync_Then_CommitThrowsException()
        {
            // Arrange
            var transactionContext = A.Fake<TransactionContextBase>();

            // Act
            using (var transaction = await transactionContext.BeginAsync(new IsolationLevel?()))
            {
                using (var innerTransaction = await transactionContext.BeginAsync(new IsolationLevel?()))
                {
                    await innerTransaction.RollbackAsync();
                }

                Assert.ThrowsAsync<InvalidOperationException>(async () => await transaction.CommitAsync());
            }

            // Assert
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoBeginAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoCommitAsync").WithReturnType<Task>().MustNotHaveHappened();
            A.CallTo(transactionContext).Where(x => x.Method.Name == "DoRollbackAsync").WithReturnType<Task>().MustHaveHappened(Repeated.NoMoreThan.Once);
        }
    }
}
