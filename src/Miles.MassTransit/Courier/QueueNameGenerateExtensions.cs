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
using Miles.Meta.Messaging;
using System;
using System.Reflection;

namespace Miles.MassTransit.Courier
{
    public static class QueueNameGenerateExtensions
    {
        private const string Arguments = "Arguments";
        private const string Log = "Log";
        private const string Compensate = "Compensate";

        public static string GenerateExecutionQueueName(this Type type)
        {
            return GenerateQueueName(type, Arguments);
        }

        public static string GenerateCompensationQueueName(this Type type)
        {
            return GenerateQueueName(type, Log, Compensate);
        }

        private static string GenerateQueueName(Type type, string remove, string append = "")
        {
            string baseQueueName;

            var queueNameAttrib = type.GetCustomAttribute<QueueNameAttribute>();
            if (queueNameAttrib == null)
                baseQueueName = (type.Name.EndsWith(remove) ? type.Name.Substring(0, type.Name.Length - remove.Length) : type.Name) + append;
            else
                baseQueueName = queueNameAttrib.Name;

            return type.GetQueuePrefix() + baseQueueName + type.GetQueueSuffix();
        }
    }
}
