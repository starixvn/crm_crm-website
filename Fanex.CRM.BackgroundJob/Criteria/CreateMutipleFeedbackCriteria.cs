namespace Fanex.CRM.BackgroundJob.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CreateMutipleFeedbackCriteria : NonQueryCommand
    {
        public short UserRole { get; set; }
        public int CreatorId { get; set; }
        public int UserId { get; set; }
        public string IP { get; set; } = string.Empty;
        public DateTime FeedbackDate { get; set; }
        public DateTime ResolutionDate { get; set; }
        public string Subject { get; set; }
        public string StationId { get; set; }
        public int ResponseId { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public string AutoField { get; set; }

        // use in case apply default communication tool
        public string AutoField1 { get; set; }

        public string AutoField2 { get; set; }
        public string AutoField3 { get; set; }

        [SpParam(SqlParamName = "Product")]
        public string ProductXml { get; set; }

        [SpParam(SqlParamName = "Category")]
        public string CategoryXml { get; set; }

        [SpParam(SqlParamName = "Categorystr")]
        public string CategoryIds { get; set; }

        [SpParam(SqlParamName = "SubCategorystr")]
        public string SubCategoryIds { get; set; }

        [SpParam(SqlParamName = "ListCustId")]
        public string CustomerXml { get; set; }

        public bool IsCheckedDefaultComm { get; set; }

        [SpParam(SqlParamName = "CommunicationId")]
        public string CommunicationIds { get; set; }

        public int NoInquiries { get; set; }

        [SpParam(SqlParamName = "RelatedCustomers")]
        public string RelatedCustomerXml { get; set; }

        public bool IsReviewed { get; set; }

        public int MessageId { get; set; }
        public string ListOrigCustIds { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_GuestFeedbackIns_Multiple";

        public override bool IsValid() => true;
    }
}