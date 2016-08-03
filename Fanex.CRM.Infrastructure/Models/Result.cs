// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System;

    /// <summary>
    /// Result Model.
    /// </summary>
    [Serializable]
    public class Result
    {
        public Result()
        {
        }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public object ExtendedData { get; set; }
    }
}
