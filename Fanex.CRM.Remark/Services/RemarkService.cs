namespace Fanex.CRM.Remark.Services
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Repositories;
    using Fanex.Data.Repository;

    public class RemarkService : IRemarkService
    {
        private readonly IRemarkRepository _remarkRepository;
        private readonly IBaseService baseService;

        public RemarkService() : this(null, null)
        {
        }

        public RemarkService(IRemarkRepository remarkRepository, IBaseService baseService)
        {
            _remarkRepository = remarkRepository ?? new RemarkRepository();
            this.baseService = baseService ?? new BaseService();
        }

        public Remarks GetRemarks(ICriteria criteria)
        {
            return _remarkRepository.GetRemarks(criteria);
        }

        public CommandResult UpdateRemarks(Remarks criteria)
        {
            var xmlContent = new StringBuilder(1024);
            xmlContent.Append("<Remarks>");
            if (criteria.SpecialRemarks != null)
            {
                BuildRemarksXml(xmlContent, criteria.SpecialRemarks, true);
            }

            if (criteria.NormalRemarks != null)
            {
                BuildRemarksXml(xmlContent, criteria.NormalRemarks, false);
            }

            xmlContent.Append("</Remarks>");

            var updateCriteria = new UpdateRemarkCriteria
            {
                CustId = criteria.CustId,
                Remarks = xmlContent.ToString()
            };

            return baseService.ExecuteNonQuery(updateCriteria);
        }

        private static void BuildRemarksXml(StringBuilder xmlContent, IEnumerable<string> remarks, bool isSpecial)
        {
            var isSpecialText = isSpecial ? " IsSpecial=\"1\" />" : " IsSpecial=\"0\" />";
            foreach (var remark in remarks)
            {
                if (!string.IsNullOrWhiteSpace(remark))
                {
                    xmlContent.Append("<Remark Content=\"").Append(HtmlHelper.EncodeXml(remark)).Append("\"")
                        .Append(isSpecialText);
                }
            }
        }
    }
}