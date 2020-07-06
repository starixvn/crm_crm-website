using Fanex.CRM.Common.Models;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Services
{
    public interface IBaseService
    {
        IEnumerable<T> GetData<T>(ICriteria criteria);

        T GetDataEntity<T>(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }


}
