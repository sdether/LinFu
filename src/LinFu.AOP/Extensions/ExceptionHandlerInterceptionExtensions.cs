﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.AOP.Cecil.Interfaces;
using Mono.Cecil;

namespace LinFu.AOP.Cecil.Extensions
{
    /// <summary>
    /// Represents an extension class that adds support for intercepting exceptions thrown at runtime.
    /// </summary>
    public static class ExceptionHandlerInterceptionExtensions
    {
        /// <summary>
        /// Enables exception interception on the given type.
        /// </summary>
        /// <param name="visitable">The target type.</param>
        public static void InterceptAllExceptions(this IReflectionVisitable visitable)
        {
            var filter = GetMethodFilter();
            InterceptExceptions(visitable, filter);
        }

        /// <summary>
        /// Enables exception interception on the given type.
        /// </summary>
        /// <param name="visitable">The target type.</param>
        public static void InterceptAllExceptions(this IReflectionStructureVisitable visitable)
        {
            var filter = GetMethodFilter();
            InterceptExceptions(visitable, filter);
        }

        /// <summary>
        /// Enables exception interception on the given type.
        /// </summary>
        /// <param name="visitable">The target type.</param>
        /// <param name="methodFilter">The <see cref="IMethodFilter"/> instance that will determine which methods should support exception interception.</param>
        public static void InterceptExceptions(this IReflectionVisitable visitable, IMethodFilter methodFilter)
        {
            visitable.InterceptExceptions(methodFilter.ShouldWeave);
        }

        /// <summary>
        /// Enables exception interception on the given type.
        /// </summary>
        /// <param name="visitable">The target type.</param>
        /// <param name="methodFilter">The <see cref="IMethodFilter"/> instance that will determine which methods should support exception interception.</param>
        public static void InterceptExceptions(this IReflectionStructureVisitable visitable, IMethodFilter methodFilter)
        {
            visitable.InterceptExceptions(methodFilter.ShouldWeave);
        }

        /// <summary>
        /// Enables exception interception on the given type.
        /// </summary>
        /// <param name="visitable">The target type.</param>
        /// <param name="methodFilter">The method filter functor that will determine which methods should support exception interception.</param>
        public static void InterceptExceptions(this IReflectionStructureVisitable visitable, Func<MethodReference, bool> methodFilter)
        {
            if (visitable == null)
                throw new ArgumentNullException("visitable");

            var catchAllThrownExceptions = new CatchAllThrownExceptions();
            visitable.WeaveWith(catchAllThrownExceptions, methodFilter);
        }

        /// <summary>
        /// Enables exception interception on the given type.
        /// </summary>
        /// <param name="visitable">The target type.</param>
        /// <param name="methodFilter">The method filter functor that will determine which methods should support exception interception.</param>
        public static void InterceptExceptions(this IReflectionVisitable visitable, Func<MethodReference, bool> methodFilter)
        {
            if (visitable == null)
                throw new ArgumentNullException("visitable");

            var catchAllThrownExceptions = new CatchAllThrownExceptions();
            visitable.WeaveWith(catchAllThrownExceptions, methodFilter);
        }

        private static Func<MethodReference, bool> GetMethodFilter()
        {
            return method =>
            {
                var actualMethod = method.Resolve();
                return actualMethod.HasBody;
            };
        }
    }
}
