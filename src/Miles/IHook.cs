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
using System;
using System.Threading.Tasks;

namespace Miles
{
    /// <summary>
    /// Subset of <see cref="Miles.Hook{TSender, TEventArgs}"/>'s public interface that exposes only registration to the hook.
    /// The idea is to make the interface public and only use the full implementation within a class
    /// to prevent calling code executing hooks.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public interface IHook<TSender, TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        /// Registers the specified hook.
        /// </summary>
        /// <param name="hook">The hook.</param>
        void Register(Func<TSender, TEventArgs, Task> hook);

        /// <summary>
        /// Unregisters the specified hook.
        /// </summary>
        /// <param name="hook">The hook.</param>
        void UnRegister(Func<TSender, TEventArgs, Task> hook);
    }
}
