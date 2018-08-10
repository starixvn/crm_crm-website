namespace Fanex.CRM.Report.Helpers
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Resources;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    public static class FieldHelper
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Need Review.")]
        public static string GenerateContactInfo(BaseCustomer customerInfo)
        {
            string pattern = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}";

            int index = 0;
            string phone = string.Empty;
            foreach (var item in customerInfo.PhoneInfo)
            {
                phone += JoinCommunication(Labels.Phone + (++index).ToString(), item.ToString());
            }

            index = 0;
            string pic = string.Empty;
            foreach (var item in customerInfo.PersonalInfoContacts)
            {
                pic += JoinCommunication(Labels.PIC + (++index).ToString(), item.ToString());
            }

            return string.Format(
                    pattern,
                    phone,
                    JoinCommunication(Labels.Fax, customerInfo.FaxInfo.ToString()),
                    JoinCommunication(Labels.Mobile, customerInfo.MobilePhone.ToString()),
                    pic,
                    JoinCommunication(Labels.Skype, customerInfo.Skype),
                    JoinCommunication(Labels.Email, customerInfo.Email),
                    JoinCommunication(Labels.MSN, customerInfo.MSN),
                    JoinCommunication(Labels.WeChat, customerInfo.WeChat),
                    JoinCommunication(Labels.WhatsApp, customerInfo.WhatsApp),
                    JoinCommunication(Labels.Line, customerInfo.Line),
                    JoinCommunication(Labels.Yahoo, customerInfo.Yahoo),
                    JoinCommunication(Labels.QQ, customerInfo.QQ),
                    JoinCommunication(Labels.BBM, customerInfo.BBM));
        }

        private static string JoinCommunication(string label, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return string.Format("{0}: {1}{2}", label, HttpContext.Current.Server.HtmlEncode(value), "<br/>");
        }
    }
}
