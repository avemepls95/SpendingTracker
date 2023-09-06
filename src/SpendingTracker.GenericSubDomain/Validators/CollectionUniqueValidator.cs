using FluentValidation;

namespace SpendingTracker.GenericSubDomain.Validators
{
    public static class CollectionUniqueValidator
    {
        private const string Separator = ",";
        private const string ErrorMessageForInstance = "Содержит дубликаты: {0}.";

        public static IRuleBuilderOptionsConditions<TSource, IEnumerable<TElement>> Unique<TSource, TElement, TKey>(
            this IRuleBuilder<TSource, IEnumerable<TElement>> ruleBuilder,
            Func<TElement, TKey> propertySelector)
        {
            return ruleBuilder.Custom((collection, context) =>
                UniqueValidator(collection, context, propertySelector, ErrorMessageForInstance));
        }

        private static void UniqueValidator<TSource, TElement, TKey>(
            IEnumerable<TElement> instanceToValidate,
            ValidationContext<TSource> customContext,
            Func<TElement, TKey> keySelector,
            string errorMessage)
        {
            TKey[] duplicateItems = instanceToValidate
                .GroupBy(keySelector)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToArray();

            if (duplicateItems.Length > 0)
            {
                customContext.AddFailure(
                    customContext.PropertyName,
                    string.Format(errorMessage, string.Join(Separator, duplicateItems)));
            }
        }
    }
}
