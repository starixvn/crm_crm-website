using System.Linq;
using Fanex.CRM.Common.Models;
using Fanex.CRM.Common.Services;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;
using Fanex.CRM.Repositories;
using Fanex.CRM.Resources;

namespace Fanex.CRM.Services
{
    public class PicReportService : IPicReportService
    {
        private readonly IPicReportRepository picReportRepository;
        private readonly IBaseService baseService;

        public PicReportService()
            : this(null, null)
        {
        }

        public PicReportService(IPicReportRepository picReportRepository, IBaseService baseService)
        {
            this.picReportRepository = picReportRepository ?? new PicReportRepository();
            this.baseService = baseService ?? new BaseService();
        }

        public PicReport GetReport(GetReportCriteria criteria)
        {
            if (criteria.Username == null)
            {
                criteria.Username = string.Empty;
            }

            return new PicReport
            {
                DataList = picReportRepository.GetReport(criteria).ToList(),
                Total = criteria.TotalRows
            };
        }

        public CommandResult InsertPic(InsertPicCriteria criteria)
        {
            if (criteria.Remark == null)
            {
                criteria.Remark = string.Empty;
            }

            var result = baseService.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = Labels.InsertPicSuccess;
            }

            return result;
        }

        public CommandResult ChangePicState(ChangePicStateCriteria criteria)
        {
            var result = baseService.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = criteria.IsActive ? Labels.ActivePicSuccess : Labels.DeactivePicSuccess;
            }

            return result;
        }

        public CommandResult EditPic(EditPicCriteria criteria)
        {
            if (criteria.Remark == null)
            {
                criteria.Remark = string.Empty;
            }

            var result = baseService.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = Labels.EditPicSuccess;
            }

            return result;
        }

        public string ValidateEditPic(ValidateEditPicCriteria criteria)
        {
            var result = baseService.GetDataEntity<string>(criteria);
            return result;
        }
    }
}