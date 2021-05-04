namespace Consul.Net.Util.Threading
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provide extensions for <see cref="Task"/>.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Turns a asynchronous task into a synchronous processes.
        /// </summary>
        /// <remarks>This extension should be used only when asynchronous methods is not supported in anyway.</remarks>
        /// <typeparam name="TResult">The asynchronous task result.</typeparam>
        /// <param name="task">The <see cref="Task"/>.</param>
        /// <returns>The task result.</returns>
        public static TResult ToSync<TResult>(this Task<TResult> task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            return task
                .GetAwaiter()
                .GetResult();
        }
    }
}
