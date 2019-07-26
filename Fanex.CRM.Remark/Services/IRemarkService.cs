namespace Fanex.CRM.Remark.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Remark.Models;
    using Fanex.Data.Repository;

    public interface IRemarkService
    {
        Remarks GetRemarks(ICriteria criteria);

        CommandResult UpdateRemarks(Remarks criteria);
    }
}