
namespace Fanex.CRM.Remark.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class NewRemarkController : ReadOnlyBaseController
    {
        public ActionResult Index(int customerId)
        {
            return Redirect(string.Format("{0}/NewRemark", ExternalDomain));
        }
        
    }
}
