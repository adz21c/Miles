/*
 *     Copyright 2019 Adam Burton (adz21c@gmail.com)
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

using Microsoft.Practices.Unity;
using Miles.DependencyInjection;

namespace Miles.Unity.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Returns a <see cref="IContainerBuilder"/> for the provided <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static IContainerBuilder ToContainerBuilder(this IUnityContainer container)
        {
            return new UnityContainerWrapper(container);
        }

        /// <summary>
        /// Returns a <see cref="IContainer"/> for the provided <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static IContainer ToContainer(this IUnityContainer container)
        {
            return new UnityContainerWrapper(container);
        }
    }
}
