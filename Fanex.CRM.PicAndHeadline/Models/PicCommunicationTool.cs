using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.CRM.PicAndHeadline.Models
{
    [XmlType("CommunicationTool")]
    public class PicCommunicationTool
    {
        [XmlAttribute("SettingID")]
        public int Type { get; set; }

        [XmlAttribute("SettingValue")]
        public string Value { get; set; }
    }
}
