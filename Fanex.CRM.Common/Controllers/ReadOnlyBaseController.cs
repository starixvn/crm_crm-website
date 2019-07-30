namespace Fanex.CRM.Common.Controllers
{
    using System.Web.Mvc;

    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ReadOnlyBaseController : BaseController
    {
    }
}