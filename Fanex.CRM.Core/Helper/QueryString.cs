// --------------------------------------------------------------------------
// <copyright file="QueryString.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Helper
{
    using System;
    using System.Web;

    /// <summary>
    /// Query String.
    /// </summary>
    public class QueryString
    {
        /// <summary>
        /// Query String.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>Data value.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Cannot call QueryString when HttpContext. Current is null or not.
        /// </exception>
        public static T Value<T>(string parameterName) where T : IConvertible
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("Cannot call QueryString<T> when HttpContext.Current is null");
            }

            if (string.IsNullOrEmpty(HttpContext.Current.Request[parameterName]))
            {
                throw new InvalidOperationException(string.Format("Expected query string parameter named '{0}' to exist", parameterName));
            }

            return Value<T>(parameterName, default(T));
        }

        /// <summary>
        /// Values the specified parameter name.
        /// </summary>
        /// <typeparam name="T"> Type value.
        /// </typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Data value.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Cannot call QueryString when HttpContext. Current is null or not.
        /// </exception>
        public static T Value<T>(string parameterName, T defaultValue) where T : IConvertible
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("Cannot call QueryString<T> when HttpContext.Current is null");
            }

            HttpRequest request = HttpContext.Current.Request;

            string input;
            if (request.QueryString[parameterName] != null &&
                !string.IsNullOrEmpty(request.QueryString[parameterName]))
            {
                input = request.QueryString[parameterName];
            }
            else
            {
                return defaultValue;
            }

            T value;
            try
            {
                value = (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(
                    "Unable to convert query string parameter named '{0}' with value of '{1}' to type {2}",
                    parameterName,
                    input,
                    typeof(T).FullName);
                throw new InvalidOperationException(errorMessage, ex);
            }

            return value;
        }
    }
}