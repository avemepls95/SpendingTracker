namespace SpendingTracker.GenericSubDomain.User
{
    public interface IUserContext
    {
        Common.User CurrentUser { get; }
    }
}