namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetRoleByPicType : CriteriaBase
    {
        public int PicType { get; set; }
        public override string GetSettingKey()
        {
            return "Acc_CRM_PicRoleSel";
        }

        public override bool IsValid() => true;
    }
}
