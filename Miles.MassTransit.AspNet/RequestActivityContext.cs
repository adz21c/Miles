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
using System;
using System.Web;

namespace Miles.MassTransit.AspNet
{
    public class RequestActivityContext : IActivityContext
    {
        // Matches the Serilog naming and implementation
        public const string RequestIdItemName = "HttpRequestId";

        public const string CorrelationIdItemName = "CorrelationId";

        public RequestActivityContext()
        {
            var requestIdItem = HttpContext.Current.Items[RequestIdItemName];
            if (requestIdItem == null)
                HttpContext.Current.Items[RequestIdItemName] = ActivityId = Guid.NewGuid();
            else
                ActivityId = (Guid)requestIdItem;

            var correlationIdItem = HttpContext.Current.Items[CorrelationIdItemName];
            if (correlationIdItem == null)
                HttpContext.Current.Items[CorrelationIdItemName] = CorrelationId = Guid.NewGuid();
            else
                CorrelationId = (Guid)correlationIdItem;
        }

        public Guid ActivityId { get; private set; }

        public Guid CorrelationId { get; private set; }
    }
}
