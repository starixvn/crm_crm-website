namespace Fanex.CRM.Remark.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]

    public class NewRemarkController : ReadOnlyBaseController
    {
        public ActionResult Index(int? custId=0, string customerName="")
        {
            var isGuest = AppSession.CurrentCustomer?.IsGuest;
            return Redirect(string.Format("{0}/NewRemark?custId={1}&customerName={2}&isGuest={3}", ExternalDomain, custId, customerName, isGuest));        
        }
    }
}
