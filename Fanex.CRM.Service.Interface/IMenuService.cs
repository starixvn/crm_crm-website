// -----------------------------------------------------------------------
// <copyright file="IMenuService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMenuService
    {
        MenuItem GetMenuById(int menuId);

        MenuItem GetMenuByURL(string menuURL);

        MenuItem GetMenuByURL(string menuURL, IList<MenuItem> menuList);

        string GetMenuNameByURL(string menuURL);
    }
}
