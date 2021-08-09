using System.Collections.Generic;
using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Common.Models;

namespace Fanex.CRM.Models.PicAndHeadline
{
    public class PicInfo
    {
        public int PicId { get; set; }

        public string Username { get; set; }

        public int PicType { get; set; }

        public string PicTypeName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Language { get; set; }

        public IEnumerable<Setting> Languages
        {
            get
            {
                return SerializerHelper.DeserializeList<Setting>(Language);
            }
        }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string Remark { get; set; }

        public string GroupHeadline { get; set; }

        public string Contact { get; set; }

        public List<CommunicationTool> CommunicationTools { get; set; }

        public bool IsActive { get; set; }
    }
}