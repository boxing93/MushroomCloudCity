namespace MushroomCloud.Common.Events.IdentityEvents
{
    public class PasswordReseted : IEvent
    {
        public string Email {get; set;}

        public PasswordReseted(string email)
        {
            Email = email;
        }
    }
}