using System.Linq.Expressions;

namespace SpendingTracker.Dispatcher.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Отфильтровать коллекцию объектов с помощью другой коллекции по конкретным полям
        /// Используется в тех случаях когда необходимо имея два ключа получить выражение
        /// (A.a = B.a AND A.b = B.b) OR (A.a = C.a AND A.b = C.b) где A - коллекция в БД
        /// B и C  - данные из коллекции по которой нам нужно произвести фильтрацию. 
        /// </summary>
        /// <param name="query">Запрос на который накладываем фильтрацию</param>
        /// <param name="items">Коллекция которой фильтруем наши данные в БД</param>
        /// <param name="filterPattern">Шаблон (WHERE) которым фильтруем данные</param>
        /// <param name="isOr">Необходимо фильтровать с помощью OR или AND -
        /// имеется ввиду (A.a = B.a AND A.b = B.b) OR (A.a = C.a AND A.b = C.b) - true
        /// (A.a = B.a AND A.b = B.b) AND (A.a = C.a AND A.b = C.b) - false </param>
        /// <returns>Отфильтрованная IQueryable коллекция</returns>
        public static IQueryable<T> FilterByItems<T, TItem>(
            this IQueryable<T> query,
            IEnumerable<TItem> items,
            Expression<Func<T, TItem, bool>> filterPattern,
            bool isOr)
        {
            Expression predicate = null;
            foreach (var item in items)
            {
                var itemExpr = Expression.Constant(item);
                var itemCondition = ExpressionReplacer.Replace(
                    filterPattern.Body,
                    filterPattern.Parameters[1],
                    itemExpr);

                if (predicate == null)
                    predicate = itemCondition;
                else
                {
                    var binaryType = isOr ? ExpressionType.OrElse : ExpressionType.AndAlso;
                    predicate = Expression.MakeBinary(binaryType, predicate, itemCondition);
                }
            }

            predicate ??= Expression.Constant(false);
            var filterLambda = Expression.Lambda<Func<T, bool>>(predicate, filterPattern.Parameters[0]);

            return query.Where(filterLambda);
        }

        class ExpressionReplacer : ExpressionVisitor
        {
            readonly IDictionary<Expression, Expression> _replaceMap;

            public ExpressionReplacer(IDictionary<Expression, Expression> replaceMap)
            {
                _replaceMap = replaceMap ?? throw new ArgumentNullException(nameof(replaceMap));
            }

            public override Expression Visit(Expression exp)
            {
                if (exp != null && _replaceMap.TryGetValue(exp, out var replacement))
                {
                    return replacement;
                }

                return base.Visit(exp);
            }

            public static Expression Replace(Expression expr, Expression toReplace, Expression toExpr)
            {
                return new ExpressionReplacer(new Dictionary<Expression, Expression> {{toReplace, toExpr}}).Visit(expr);
            }

            public static Expression Replace(Expression expr, IDictionary<Expression, Expression> replaceMap)
            {
                return new ExpressionReplacer(replaceMap).Visit(expr);
            }

            public static Expression GetBody(LambdaExpression lambda, params Expression[] toReplace)
            {
                if (lambda.Parameters.Count != toReplace.Length)
                    throw new InvalidOperationException();

                return new ExpressionReplacer(Enumerable.Range(0, lambda.Parameters.Count)
                        .ToDictionary(i => (Expression) lambda.Parameters[i], i => toReplace[i]))
                    .Visit(lambda.Body);
            }
        }
    }
}