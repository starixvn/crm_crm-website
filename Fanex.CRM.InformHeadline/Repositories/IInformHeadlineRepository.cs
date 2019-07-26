namespace Fanex.CRM.InformHeadline.Repositories
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;

    public interface IInformHeadlineRepository
    {
        CommandResult InsertInformHeadline(InsertInformHeadlineCriteria criteria);
    }
}