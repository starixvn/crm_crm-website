namespace Fanex.CRM.Communication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helpers;
    using Criteria;
    using Infrastructure.Constants;
    using Infrastructure.Models;
    using Repositories;
    using Resources;
    using Setting.Constants;
    using Setting.Services;

    public class MultipleLogsService : IMultipleLogsService
    {
        private readonly IMultipleLogsRepository _multipleLogsRepository;

        private ISettingService _serviceSetting;

        public MultipleLogsService()
            : this(null, null)
        {
        }

        public MultipleLogsService(
            IMultipleLogsRepository multipleLogsRepository,
            ISettingService serviceSetting)
        {
            _multipleLogsRepository = multipleLogsRepository ?? new MultipleLogsRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public IList<BaseModel> AllStatusForInsert
        {
            get
            {
                return AllStatusForEdit.Where(s => !string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public IList<BaseModel> AllStatusForEdit
        {
            get
            {
                return _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool).ToList();
            }
        }

        public Result InsertLog(InsertMultipleLogsCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new Result
                {
                    Code = ErrorCodeConstants.ServerError,
                    Message = Labels.Communication_Log_ExcessCurrentTime
                };
            }

            var result = _multipleLogsRepository.InsertLog(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Communication_Logs_InsertSuccess;
            }

            return result;
        }
    }
}
