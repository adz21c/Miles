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

namespace Miles.GreenPipes.Serilog.LogContext
{
    public interface ILogContextConfigurator<TContext>
    {
        /// <summary>
        /// Push another property into the <see cref="Serilog.LogContext"/>
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyAccessor">Function returns value based on the supplied context.</param>
        /// <param name="destructureObjects">If true, and the value is a non-primitive, non-array type,
        /// then the value will be converted to a structure; otherwise, unknown types will
        /// be converted to scalars, which are generally stored as strings.</param>
        void PushProperty(string name, Func<TContext, object> propertyAccessor, bool destructureObjects = false);
    }
}
