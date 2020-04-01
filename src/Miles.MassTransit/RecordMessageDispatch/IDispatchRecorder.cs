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
using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.RecordMessageDispatch
{
    /// <summary>
    /// Use to record dispatch of messages.
    /// <see cref="RecordMessageDispatchFilter{TContext}"/>.
    /// </summary>
    public interface IDispatchRecorder
    {
        /// <summary>
        /// Records a message as being dispatched. This should assume it is operating outside
        /// a transaction (the dispatch has happened, we wouldn't want the record to rollback).
        /// This class should also expect to work as a singleton instance, so think about
        /// thread safety.
        /// </summary>
        /// <param name="context">The message context details.</param>
        /// <returns></returns>
        Task RecordAsync(SendContext context);
    }
}