/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
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
using Miles.Persistence;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework
{
    public class EFTransactionContext : TransactionContextBase
    {
        private readonly DbContext dbContext;
        private DbContextTransaction transaction = null;

        public EFTransactionContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task DoBeginAsync(IsolationLevel? hintIsolationLevel = null)
        {
            if (hintIsolationLevel.HasValue)
                transaction = dbContext.Database.BeginTransaction(hintIsolationLevel.Value);
            else
                transaction = dbContext.Database.BeginTransaction();

            return Task.CompletedTask;
        }

        protected override Task DoCommitAsync()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }

            return Task.CompletedTask;
        }

        protected override Task DoRollbackAsync()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }

            return Task.CompletedTask;
        }
    }
}
