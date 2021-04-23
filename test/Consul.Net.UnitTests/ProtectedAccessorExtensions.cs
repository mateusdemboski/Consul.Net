namespace Consul.Net
{
    using System;
    using NSubstitute.Core;
    using NSubstitute.Exceptions;
    using static System.Reflection.BindingFlags;

    /// <summary>
    /// HACK: Based on @dtchepak solution
    /// See more the original issue for more details: https://github.com/nsubstitute/NSubstitute/issues/222#issuecomment-193487145
    /// This implementation has a little improvements based on the original thread.
    /// </summary>
    internal static class ProtectedAccessorExtensions
    {
        public static object? Protected(this object target, string methodName, params object[] methodArguments)
        {
            _ = target ?? throw new ArgumentNullException(nameof(target));
            _ = methodName ?? throw new ArgumentNullException(nameof(methodName));
            _ = methodArguments ?? throw new ArgumentNullException(nameof(methodArguments));

            target.EnsureIsSubstitute();

            var method = target
                .GetType()
                .GetMethod(methodName, NonPublic | Instance);

            if (method is null)
            {
                throw new InvalidOperationException($"Non-public instance method '{methodName}' not found on target object.");
            }

            return method.Invoke(target, methodArguments);
        }

        private static void EnsureIsSubstitute(this object target)
        {
            try
            {
                _ = SubstitutionContext.Current.GetCallRouterFor(target);
            }
            catch (NotASubstituteException)
            {
                throw new ArgumentException("Instance is not a substitute", nameof(target));
            }
        }
    }
}
