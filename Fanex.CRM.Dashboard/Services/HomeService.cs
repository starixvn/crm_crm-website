namespace Fanex.CRM.Dashboard.Services
{
    using System.Collections.Generic;
    using System.Resources;
    using System.Text.RegularExpressions;
    using Fanex.NAP.Core.Models;

    public class HomeService : IHomeService
    {
        public IList<MenuItem> UpdateMenuLanguage(IList<MenuItem> menus)
        {
            ResourceManager rm = new ResourceManager(typeof(Resources.Menus));
            var newMenus = new List<MenuItem>();
            for (var i = 0; i < menus.Count; i++)
            {
                var menuItem = menus[i];
                var newMenuItem = new MenuItem
                {
                    Name = UpdateMenuName(menuItem.Name, rm),
                    CreatedDate = menuItem.CreatedDate,
                    Creator = menuItem.Creator,
                    Description = menuItem.Description,
                    DisplayOrder = menuItem.DisplayOrder,
                    Environment = menuItem.Environment,
                    Function = menuItem.Function,
                    Id = menuItem.Id,
                    IsActive = menuItem.IsActive,
                    ModifiedDate = menuItem.ModifiedDate,
                    Modifier = menuItem.Modifier,
                    Parent = menuItem.Parent,
                    Policy = menuItem.Policy,
                    Site = menuItem.Site,
                    Url = menuItem.Url
                };

                newMenus.Add(newMenuItem);
            }

            return newMenus;
        }

        private static string UpdateMenuName(string name, ResourceManager rm)
        {
            var key = Regex.Replace(name.Trim(), "-| ", "_");
            var newName = rm.GetString(key);

            return newName ?? name;
        }
    }
}