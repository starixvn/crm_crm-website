using Fanex.NAP.Core.Models;
using System.Collections.Generic;

namespace Fanex.CRM.Dashboard.Services
{
    public interface IHomeService
    {
        IList<MenuItem> UpdateMenuLanguage(IList<MenuItem> menus);
    }
}
