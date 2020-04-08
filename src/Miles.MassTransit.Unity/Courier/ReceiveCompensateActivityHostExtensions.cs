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
using MassTransit.Courier;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Unity.Courier;
using System;

namespace MassTransit
{
    public static class ReceiveCompensateActivityHostExtensions
    {
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, IUnityContainer container, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            var factory = new MilesUnityCompensateActivityFactory<TActivity, TLog>(container);
            configurator.CompensateActivityHost(factory, configure);
        }
    }
}
