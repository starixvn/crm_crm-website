namespace Fanex.CRM.Feedback.Models
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class AllSettingToAddFeedback
    {
        public IEnumerable<SettingRelation> ExtensionList { get; set; }

        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<Province> ProvinceList { get; set; }

        public IEnumerable<Setting> CommunicationToolList { get; set; }

        public IEnumerable<Setting> LanguageList { get; set; }

        public IEnumerable<Setting> MainCategoryList { get; set; }

        public IEnumerable<Setting> SubCategoryList { get; set; }

        public IEnumerable<Product> ProductList { get; set; }

        public IEnumerable<Product> SubProductList { get; set; }

        public IEnumerable<Product> BettypeList { get; set; }

        public IEnumerable<SiteSetting> SiteList { get; set; }

        public IEnumerable<SettingRelation> ResponseList { get; set; }

        public IEnumerable<BaseModel> StatusList { get; set; }

        public IEnumerable<SettingRelation> TitleList { get; set; }

        public IEnumerable<SettingRelation> RoleList { get; set; }

        public IEnumerable<BaseModel> LicenseeType { get; set; }

        public Product ProductNA { get; set; }

        public Product SubProductNA { get; set; }

        public Product BettypeNA { get; set; }

        public SettingRelation ExtensionNA { get; set; }
    }
}