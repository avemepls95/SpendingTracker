namespace SpendingTracker.Common.Primitives
{
    public abstract class CustomKey<TKey, TType> : ValueObject<CustomKey<TKey, TType>>, IComparable, IComparable<TKey>
        where TKey : CustomKey<TKey, TType>
        where TType: struct
    {
        public TType Value { get; private set; }

        protected CustomKey(TType value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return $"(Id = {Value})";
        }

        public int CompareTo(object obj)
        {
            var otherKey = obj as TKey;
            return CompareTo(otherKey);
        }

        public int CompareTo(TKey other)
        {
            return Comparer<TType?>.Default.Compare(Value, other?.Value);
        }
    }
}