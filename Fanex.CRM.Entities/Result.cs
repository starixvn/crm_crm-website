// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    /// <summary>
    /// Result information.
    /// </summary>
    public class Result
    {
        public Result()
        {
        }

        public Result(int errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool IsSuccessfull
        {
            get
            {
                return ErrCode == 0 && string.IsNullOrEmpty(ErrMsg);
            }
        }
    }
}
