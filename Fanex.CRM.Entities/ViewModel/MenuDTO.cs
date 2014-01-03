// --------------------------------------------------------------------------
// <copyright file="MenuDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Menu DTO class.
    /// </summary>
    [Serializable]
    public class MenuDTO
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int ParentId { get; set; }

        public string Target { get; set; }

        public int Order { get; set; }

        public List<MenuDTO> SubMenus { get; set; }
    }
}
