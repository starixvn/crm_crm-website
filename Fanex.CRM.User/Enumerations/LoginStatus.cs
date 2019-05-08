namespace Fanex.CRM.User.Enumerations
{
    public enum LoginStatus
    {
        LoginSuccessful,

        InvalidUsernameOrPassword,

        UserSuspended,

        UserNoAuthorization,

        UserNoMenuPermission,

        ValidatedSuccess,

        NotKnowUserRole
    }
}
