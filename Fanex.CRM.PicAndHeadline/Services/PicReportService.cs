using Fanex.CRM.Common.Models;
using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;
using Fanex.CRM.PicAndHeadline.Repositories;
using Fanex.CRM.PicAndHeadline.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.CRM.PicAndHeadline.Services
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

            return new PicReport
            {
                DataList = new List<PicInfo> {
                    new PicInfo { Username = "a" },
                    new PicInfo { Username = "b" },
                    new PicInfo { Username = "c" }
                },
                Total = 21
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

            return new CommandResult
            {
                Message = Labels.InsertPicSuccess,
                Code = 0
            };
        }

        public CommandResult ChangePicState(ChangePicStateCriteria criteria){
            var result = baseService.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = criteria.IsActive ? Labels.ActivePicSuccess : Labels.DeactivePicSuccess;
            }

            return result;
        }

        public CommandResult EditPic(EditPicCriteria criteria)
        {
            if(criteria.Remark == null)
            {
                criteria.Remark = string.Empty;
            }

            var result = baseService.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = Labels.EditPicSuccess;
            }

            return result;

            return new CommandResult
            {
                Message = Labels.InsertPicSuccess,
                Code = 0
            };
        }
    }
}
