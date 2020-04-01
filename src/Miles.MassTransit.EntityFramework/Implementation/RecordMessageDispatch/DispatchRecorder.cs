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
using MassTransit;
using Miles.MassTransit.RecordMessageDispatch;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.Implementation.RecordMessageDispatch
{
    public class DispatchRecorder : IDispatchRecorder
    {
        private readonly string connectionString;

        public DispatchRecorder(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task RecordAsync(SendContext context)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("UPDATE [dbo].[OutgoingMessages] SET [DispatchedDate] = @DispatchedDate WHERE MessageId = @MessageId", connection))
            {
                command.Parameters.AddWithValue("@DispatchedDate", DateTime.Now);
                command.Parameters.AddWithValue("@MessageId", context.MessageId.Value);

                await connection.OpenAsync().ConfigureAwait(false);
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}
