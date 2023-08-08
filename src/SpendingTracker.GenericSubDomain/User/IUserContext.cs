namespace SpendingTracker.GenericSubDomain.User
{
    public interface IUserContext
    {
        Domain.User CurrentUser { get; }
    }
}