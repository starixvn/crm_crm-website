// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Common.Controllers
{
    using System.Web.Mvc;
    using ONEbook.UM.Filters;

    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ReadOnlyBaseController : BaseController
    {
    }
}