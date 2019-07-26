namespace Fanex.CRM.Remark.Repositories
{
    using Fanex.CRM.Remark.Models;
    using Fanex.Data.Repository;

    public interface IRemarkRepository
    {
        Remarks GetRemarks(ICriteria criteria);
    }
}