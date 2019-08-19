namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using System.Collections.Generic;
    using Common.Helpers;
    using Common.Services;
    using Data.Repository;
    using Fanex.CRM.Common.Criteria;
    using Models;

    public class UpdateMultipleLogsCriteria : UpdateCriteria
    {
        public string ListFeedbackId { get; set; }

        public int CountryId { get; set; }
        [SpParam(SqlParamName = "IsUpdateCountryId")]
        public bool IsUpdateCountry { get; set; }

        public int ProvinceId { get; set; }
        [SpParam(SqlParamName = "IsUpdateProvinceId")]
        public bool IsUpdateProvince { get; set; }

        public int SiteId { get; set; }
        [SpParam(SqlParamName = "IsUpdateSiteId")]
        public bool IsUpdateSite { get; set; }

        public string Subject { get; set; }
        public bool IsUpdateSubject { get; set; }

        public int LanguageId { get; set; }
        [SpParam(SqlParamName = "IsUpdateLanguageId")]
        public bool IsUpdateLanguage { get; set; }

        [SpParam(SqlParamName = "FeedbackDate")]
        public DateTime StartTime { get; set; }
        [SpParam(SqlParamName = "IsUpdateFeedbackDate")]
        public bool IsUpdateStartTime { get; set; }

        [SpParam(SqlParamName = "ResolutionDate")]
        public DateTime EndTime { get; set; }
        [SpParam(SqlParamName = "IsUpdateResolutionDate")]
        public bool IsUpdateEndTime { get; set; }

        public int RoleId { get; set; }
        [SpParam(SqlParamName = "IsUpdateRoleId")]
        public bool IsUpdateRole { get; set; }

        public int ResponseId { get; set; }
        [SpParam(SqlParamName = "IsUpdateResponseId")]
        public bool IsUpdateResponse { get; set; }

        public int StatusId { get; set; }
        [SpParam(SqlParamName = "IsUpdateStatusId")]
        public bool IsUpdateStatus { get; set; }

        [SpParam(SqlParamName = "Description")]
        public string Remark { get; set; }
        [SpParam(SqlParamName = "IsUpdateDescription")]
        public bool IsUpdateRemark => IsUpdateAutoField;

        public string Solution { get; set; }
        public bool IsUpdateSolution { get; set; }

        [SpParam(SqlParamName = "LicenseeType")]
        public int LicenseeTypeId { get; set; }
        public bool IsUpdateLicenseeType { get; set; }

        public bool IsReviewed { get; set; }
        public string Review { get; set; }
        [SpParam(SqlParamName = "IsUpdateReview")]
        public bool IsUpdateReviewed { get; set; }

        public string AutoField { get; set; }
        public bool IsUpdateAutoField { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<ProductFilter> Products { get; set; }

        [SpParam(SqlParamName = "Product")]
        public string ProductsContent => SerializerHelper.SerializeList(Products);
        public bool IsUpdateProduct { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<CategoryFilter> Categories { get; set; }

        [SpParam(SqlParamName = "Category")]
        public string CategoriesContent => SerializerHelper.SerializeList(Categories);
        public bool IsUpdateCategory { get; set; }

        [SpParam(SqlParamName = "CommunicationId")]
        public string CommunicationIds { get; set; }
        [SpParam(SqlParamName = "IsUpdateCommunicationId")]
        public bool IsUpdateCommunication { get; set; }

        [SpParam(SqlParamName = "StationId")]
        public string ExtensionIds { get; set; }
        [SpParam(SqlParamName = "IsUpdateStationId")]
        public bool IsUpdateExtension { get; set; }

        public int ReviewerId => IsReviewed ? AppSession.CurrentUser.Id : 0;

        public int UpdateReviewedMode { get; set; }

        public override string GetSettingKey() => "MultipleLogsUpdate";

        public override bool IsValid() => true;
    }
}