namespace SITracker.Exceptions
{
    public class PasswordsDoNotMatchException : Exception
    {
        public PasswordsDoNotMatchException(string message) : base(message)
        {
        }
    }
}
