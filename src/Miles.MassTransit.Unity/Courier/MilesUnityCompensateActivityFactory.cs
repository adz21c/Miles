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
using GreenPipes;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Hosts;
using MassTransit.Logging;
using MassTransit.Util;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity.Courier
{
    public class MilesUnityCompensateActivityFactory<TActivity, TLog> : CompensateActivityFactory<TActivity, TLog>
        where TActivity : class, CompensateActivity<TLog>
        where TLog : class
    {
        private static readonly ILog log = Logger.Get<MilesUnityCompensateActivityFactory<TActivity, TLog>>();

        private readonly IUnityContainer container;

        public MilesUnityCompensateActivityFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public async Task<ResultContext<CompensationResult>> Compensate(CompensateContext<TLog> context, IRequestPipe<CompensateActivityContext<TActivity, TLog>, CompensationResult> next)
        {
            using (var childContainer = container.CreateChildContainer())
            {
                childContainer.RegisterInstance<ConsumeContext>(context.ConsumeContext)
                    .RegisterInstance<IPublishEndpoint>(context)
                    .RegisterInstance<ISendEndpointProvider>(context);

                var activity = childContainer.Resolve<TActivity>();
                if (activity == null)
                    throw new Exception($"Unable to resolve consumer type '{typeof(TActivity).FullName}'.");    // TODO: Better exception

                if (log.IsDebugEnabled)
                    log.DebugFormat("CompensateActivityFactory: Executing: {0}", TypeMetadataCache<TActivity>.ShortName);

                var activityContext = new HostCompensateActivityContext<TActivity, TLog>(activity, context);

                return await next.Send(activityContext).ConfigureAwait(false);
            }
        }
    }
}
