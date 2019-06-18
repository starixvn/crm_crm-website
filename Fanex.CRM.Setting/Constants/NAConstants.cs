using Fanex.CRM.Common.Enumerations;

namespace Fanex.CRM.Setting.Constants
{
    public static class NAConstants
    {
        public static readonly int ProductNA = 46004;

        public static readonly int CN88ProductNA = 46011;

        public static readonly int SubProductNA = 8063;

        public static readonly int CN88SubProductNA = 8124;

        public static readonly int BettypeNA = 9144;

        public static readonly int CN88BettypeNA = 9282;

        public static readonly int CommunicationNA = 1005;

        public static readonly int CN88CommunicationNA = 1018;

        public static readonly int ExtentionNA = 2022;

        public static readonly int CN88ExtentionNA = 2055;

        public static int GetProductNA(UserRole userRole)
        {
            return userRole == UserRole.CN88Staff ? CN88ProductNA : ProductNA;
        }

        public static int GetSubProductNA(UserRole userRole)
        {
            return userRole == UserRole.CN88Staff ? CN88SubProductNA : SubProductNA;
        }

        public static int GetBettypeNA(UserRole userRole)
        {
            return userRole == UserRole.CN88Staff ? CN88BettypeNA : BettypeNA;
        }

        public static int GetCommunicationNA(UserRole userRole)
        {
            return userRole == UserRole.CN88Staff ? CN88CommunicationNA : CommunicationNA;
        }

        public static int GetExtentionNA(UserRole userRole)
        {
            return userRole == UserRole.CN88Staff ? CN88ExtentionNA : ExtentionNA;
        }
    }
}
