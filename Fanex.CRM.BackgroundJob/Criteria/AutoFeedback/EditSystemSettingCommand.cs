namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class EditSystemSettingCommand : NonQueryCommand
    {
        public string Settings { get; set; }

        public override string GetSettingKey() => "Acc_CRM_SystemSettingUpd";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Settings);
    }
}