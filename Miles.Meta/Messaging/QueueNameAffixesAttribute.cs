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
using System.Reflection;

namespace Miles.Meta.Messaging
{
    /// <summary>
    /// Can declare queue name prefixes and suffixes at an assembly level and override at a class level.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QueueNameAffixesAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the queue name prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the queue name suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix { get; set; }
    }

    public static class QueueAffixReflectionExtensions
    {
        /// <summary>
        /// Gets the queue prefix for the supplied type, else falls back to the assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetQueuePrefix(this Type type)
        {
            var attrib = type.GetCustomAttribute<QueueNameAffixesAttribute>();
            return attrib?.Prefix ?? type.Assembly.GetQueuePrefix();
        }

        /// <summary>
        /// Gets the queue prefix for the supplied assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string GetQueuePrefix(this Assembly assembly)
        {
            var attrib = assembly.GetCustomAttribute<QueueNameAffixesAttribute>();
            return attrib?.Prefix ?? string.Empty;
        }
        /// <summary>
        /// Gets the queue prefix for the supplied type, else falls back to the assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetQueueSuffix(this Type type)
        {
            var attrib = type.GetCustomAttribute<QueueNameAffixesAttribute>();
            return attrib?.Suffix ?? type.Assembly.GetQueueSuffix();
        }

        /// <summary>
        /// Gets the queue prefix for the supplied assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string GetQueueSuffix(this Assembly assembly)
        {
            var attrib = assembly.GetCustomAttribute<QueueNameAffixesAttribute>();
            return attrib?.Suffix ?? string.Empty;
        }
    }
}
