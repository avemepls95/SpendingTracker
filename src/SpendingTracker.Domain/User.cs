using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain
{
    public sealed class User
    {
        public User(UserKey id, Currency currency)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            Id = id;
            Currency = currency;
        }

        public UserKey Id { get; }

        public Currency Currency { get; set; }
        
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public static UserKey SystemUserId => new(Guid.Parse("de86f691-6ec3-401b-b371-2ddc9554147c"));
    }
}