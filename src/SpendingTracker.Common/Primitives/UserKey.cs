namespace SpendingTracker.Common.Primitives
{
    public class UserKey : CustomKey<UserKey, Guid>
    {
        public UserKey(Guid value) : base(value)
        {
        }

        public static UserKey Parse(string idAsString)
        {
            if (!Guid.TryParse(idAsString, out var result))
            {
                throw new ArgumentException($"Не удалось преобразовать значение {idAsString} в {nameof(UserKey)}");
            }

            return new UserKey(result);
        }
    }
}