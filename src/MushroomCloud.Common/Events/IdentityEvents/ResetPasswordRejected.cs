namespace MushroomCloud.Common.Events.IdentityEvents
{
    public class ResetPasswordRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string ErrorCode { get; }

        protected ResetPasswordRejected()
        {
        }

        public ResetPasswordRejected(string email, string reason, string errorCode)
        {   
            Email = email;
            Reason = reason;
            ErrorCode = errorCode;
        }
    }
}