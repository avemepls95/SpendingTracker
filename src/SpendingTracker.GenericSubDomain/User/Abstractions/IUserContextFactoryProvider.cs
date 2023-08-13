namespace SpendingTracker.GenericSubDomain.User.Abstractions;

public interface IUserContextFactoryProvider
{
    IUserContextFactory GetSuitableFactory();
}