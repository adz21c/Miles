using GreenPipes;
using System;
using System.Linq.Expressions;

namespace Miles.GreenPipes.Junction
{
    public interface IJunctionConfigurator<TContext> where TContext : class, PipeContext
    {
        /// <summary>
        /// Sets a value indicating whether the context should continue traveling on the original pipe
        /// if it does not match the conditions of any exit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [continue on no match]; otherwise, <c>false</c>.
        /// </value>
        bool ContinueOnNoMatch { set; }

        /// <summary>
        /// Defines a junction exit.
        /// </summary>
        /// <param name="pipe">The pipe.</param>
        /// <param name="condition">The condition that allows the context to take this exit.</param>
        void Exit(IPipe<TContext> pipe, Expression<Func<TContext, bool>> condition);
    }
}