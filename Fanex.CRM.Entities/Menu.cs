// --------------------------------------------------------------------------
// <copyright file="Menu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Menu information.
    /// </summary>
    [Serializable]
    public class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Menu Parent { get; set; }

        public string Target { get; set; }

        public int Order { get; set; }

        public IEnumerable<Menu> GetChildList(IEnumerable<Menu> menuItemCollection)
        {
            if (menuItemCollection != null && menuItemCollection.Count() > 0)
            {
                return menuItemCollection
                    .Select(menu => menu)
                    .Where(menuItem => (menuItem.Parent != null && menuItem.Parent.Id == this.Id));
            }

            return new List<Menu>();
        }
    }
}
