using Fanex.CRM.Infrastructure.Models;

namespace Fanex.CRM.Common.Models
{
    public class Province : BaseModel
    {
        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public int CountryId { get; set; }
    }
}
