namespace Fanex.CRM.Common.ActionResults
{
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class JsonWithoutNullField : ActionResult
    {
        public JsonWithoutNullField(object value)
        {
            this._value = value;
        }

        private object _value { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var jsonString = JsonConvert.SerializeObject(
                this._value,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            context.HttpContext.Response.AddHeader("content-type", "application/json");
            context.HttpContext.Response.Write(jsonString);
        }
    }
}