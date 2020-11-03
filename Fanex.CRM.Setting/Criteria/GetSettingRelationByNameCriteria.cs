namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetSettingRelationByNameCriteria : BasedLanguageCriteria
    {
        [SpParam(SqlParamName = "settingname")]
        public string SettingName { get; set; }

        [SpParam(SqlParamName = "parentid")]
        public int ParentId { get; set; }

        public byte Active { get; set ; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingGetRelationBySettingName";
        }

        public override bool IsValid() => true;
    }
}