
namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }
        public string Target { get; set; }
        public int Order { get; set; }

        public List<MenuDTO> SubMenus;
    }
}
