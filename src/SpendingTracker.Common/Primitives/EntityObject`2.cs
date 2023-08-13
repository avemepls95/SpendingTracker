namespace SpendingTracker.Common.Primitives
{
    public abstract class EntityObject<T, TKey> : IEquatable<T>, IModificationInfoAccessor
        where T : EntityObject<T, TKey>
    {
        public abstract TKey GetKey();

        public bool Equals(T other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return ReferenceEquals(this, other) || GetKey().Equals(other.GetKey());
        }

        public override bool Equals(object obj)
        {
            return obj is T other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(0, GetKey());
        }

        public DateTimeOffset CreatedDate { get; private set; }
        public UserKey CreatedBy { get; private set; }
        public DateTimeOffset? ModifiedDate { get; private set; }
        public UserKey? ModifiedBy { get; private set; }

        void IModificationInfoAccessor.SetCreated(DateTimeOffset now, UserKey createdBy)
        {
            CreatedDate = now;
            CreatedBy = createdBy;
            ModifiedDate = now;
            ModifiedBy = createdBy;
        }

        void IModificationInfoAccessor.SetModified(DateTimeOffset now, UserKey modifiedBy)
        {
            ModifiedDate = now;
            ModifiedBy = modifiedBy;
        }
    }
}