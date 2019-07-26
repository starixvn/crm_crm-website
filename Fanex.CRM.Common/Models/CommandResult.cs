namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class CommandResult
    {
        public CommandResult()
        {
        }

        public CommandResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public object ExtendedData { get; set; }

        public bool IsSuccessful => Code == 0;
    }
}