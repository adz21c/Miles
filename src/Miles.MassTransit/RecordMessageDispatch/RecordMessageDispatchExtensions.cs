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
using Miles.MassTransit.RecordMessageDispatch;
using System;

namespace MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public static class RecordMessageDispatchExtensions
    {
        /// <summary>
        /// Registers a filter on send pipes to attempt to record the dispatch of any message.
        /// </summary>
        /// <typeparam name="TConfigurator">The type of the configurator.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="dispatchRecorder">The dispatch recorder.</param>
        public static void ConnectRecordMessageDispatchObserver(this ISendObserverConnector connector, IDispatchRecorder dispatchRecorder)
        {
            var observer = new RecordMessageDispatchObserver(dispatchRecorder ?? throw new ArgumentNullException(nameof(dispatchRecorder)));
            connector.ConnectSendObserver(observer);
        }
    }
}
