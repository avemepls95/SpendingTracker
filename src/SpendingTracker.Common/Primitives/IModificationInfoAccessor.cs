namespace SpendingTracker.Common.Primitives
{
    public interface IModificationInfoAccessor
    {
        void SetCreated(DateTimeOffset now, UserKey createdBy);
        void SetModified(DateTimeOffset now, UserKey modifiedBy);
    }
}