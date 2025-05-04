using System.Linq.Expressions;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

namespace TodoWeb.Application.Extensions
{
    public static class QueryableExtensions
    {
        //Cache MethodInfo để không phải dùng reflection nhiều lần
        private static readonly MethodInfo ContainsMethod =
            typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;//"!" bảo rằng đảm bảo không có null
        private static readonly MethodInfo ToLowerMethod =
            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;


        /// <summary>
        /// Lọc query theo searchTerm trên một hoặc nhiều trường string.
        /// Mỗi selector phải trả về string (ví dụ: u => u.FirstName).
        /// Kết quả là OR giữa tất cả: x => x.F1.Contains(term) || x.F2.Contains(term) || ...
        /// </summary>
        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? searchTerm,
            params Expression<Func<T, string>>[] fieldSelectors)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || fieldSelectors.IsNullOrEmpty())
                return query;

            // Parameter chung cho tất cả
            var parameter = Expression.Parameter(typeof(T), "x"); // x => 
            var constant = Expression.Constant(searchTerm.ToLower(), typeof(string));

            // Xây dựng biểu thức OR
            Expression? combined = null;
            foreach (var selector in fieldSelectors)
            {
                // Invoke selector trên cùng parameter
                var memberAccess = Expression.Invoke(selector, parameter); //x => x.Field
                //Xử lý null: x.Field != null
                var notNull = Expression.NotEqual(
                memberAccess,
                Expression.Constant(null, typeof(string)));
                //x.Fiedl.ToLower()
                var lowerField = Expression.Call(memberAccess, ToLowerMethod);
                // x.Field.ToLower().Contains(searchTerm.ToLower())
                var containsCall = Expression.Call(
                    lowerField,
                    ContainsMethod, 
                    constant
                );
                //x.Field.ToLower().Contains(searchTerm.ToLower()) && x.Field != null
                var safeContains = Expression.AndAlso(notNull, containsCall);
                // Kết hợp với các selector trước đó
                combined = combined == null
                    ? containsCall
                    : Expression.OrElse(combined, safeContains);
            }

            // Tạo Lambda: x => (…||…||…)
            var lambda = Expression.Lambda<Func<T, bool>>(combined!, parameter);
            return query.Where(lambda);
        }




        /// <summary>
        /// Tách searchTerm thành nhiều token, rồi với mỗi token xây predicate:
        ///    x => (F1.Contains(token) || F2.Contains(token) || ...)
        /// Cuối cùng AND tất cả token lại:
        ///    Tập hợp record sao cho mỗi token ít nhất match 1 field.
        /// </summary>
        public static IQueryable<T> ApplyRelatedSearch<T>(
            this IQueryable<T> query,
            string? searchTerm,
            params Expression<Func<T, string>>[] fieldSelectors)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)
                || fieldSelectors.IsNullOrEmpty())
            {
                return query;
            }

            // 1. ToLower, Split thành token, loại bỏ khoảng trắng thừa
            var tokens = searchTerm.ToLower()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
                return query;

            // Parameter chung
            var parameter = Expression.Parameter(typeof(T), "x");//x =>

            Expression? combinedOr = null;

            // 2. Với mỗi token, build sub‐predicate (OR giữa tất cả fieldSelectors)
            foreach (var token in tokens)
            {
                var constant = Expression.Constant(token, typeof(string));
                Expression? combined = null;

                foreach (var selector in fieldSelectors)
                {
                    
                    var invoked = Expression.Invoke(selector, parameter);//x => x.Field
                    //Xử lý null: x.Field != null
                    var notNull = Expression.NotEqual(
                    invoked,
                    Expression.Constant(null, typeof(string)));
                    //x.Fiedl.ToLower()
                    var lowerField = Expression.Call(invoked, ToLowerMethod);
                    // x => selector(x).Contains(token)
                    var containsCall = Expression.Call(
                        lowerField,
                        ContainsMethod,
                        constant
                    );
                    //x.Field.ToLower().Contains(searchTerm.ToLower()) && x.Field != null
                    var safeContains = Expression.AndAlso(notNull, containsCall);
                    combined = combined == null
                        ? containsCall
                        : Expression.OrElse(combined, safeContains);
                }

                // Kết quả OR cho token này
                // Nếu chỉ có 1 fieldSelector thì combinedOr == containsCall
                combinedOr = combinedOr == null
                    ? combined
                    : Expression.OrElse(combinedOr, combined!);
            }

            // 3. Biểu thức cuối: x => ( (F1||F2||... for token1) || (F1||F2||... for token2) || ... )
            var lambda = Expression.Lambda<Func<T, bool>>(combinedOr!, parameter);
            return query.Where(lambda);
        }


        public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        bool descending = false,
        params Expression<Func<T, object>>[] fieldSelectors)
        {
            if (fieldSelectors.IsNullOrEmpty())
                return query;

            // Khởi tạo OrderBy/OrderByDescending với selector đầu tiên
            IOrderedQueryable<T> orderedQuery = descending
                ? query.OrderByDescending(fieldSelectors[0])
                : query.OrderBy(fieldSelectors[0]);

            // Chain ThenBy/ThenByDescending cho các selector tiếp theo
            for (int i = 1; i < fieldSelectors.Length; i++)
            {
                orderedQuery = descending
                    ? orderedQuery.ThenByDescending(fieldSelectors[i])
                    : orderedQuery.ThenBy(fieldSelectors[i]);
            }

            return orderedQuery;
        }


        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int? pageIndex, int? pageSize)
        {
            if (pageIndex == null || pageSize == null)
                return query;
            return query.Skip((int)((pageIndex - 1) * pageSize)).Take((int)pageSize);
        }

        /// <summary>
        /// Tìm kiếm với các token của searchTerm trên nhiều trường string,
        /// có thể ignore case và chọn logic kết hợp token (AND hoặc OR).
        /// </summary>
        /// <param name="query">Query gốc</param>
        /// <param name="searchTerm">Chuỗi search, sẽ bị split theo dấu cách</param>
        /// <param name="ignoreCase">
        ///   Nếu true thì convert cả field và token về lower-case trước khi Contains
        /// </param>
        /// <param name="combineWithAnd">
        ///   Nếu true thì mỗi token phải match ít nhất 1 field (AND giữa token),
        ///   nếu false thì bất kỳ token nào match là được (OR giữa token).
        /// </param>
        /// <param name="fieldSelectors">
        ///   Các selector trả về string (có thể là .ToString() nếu field gốc không phải string)
        /// </param>
        //public static IQueryable<T> ApplyRelatedSearch<T>(
        //    this IQueryable<T> query,
        //    string? searchTerm,
        //    bool ignoreCase = true,
        //    bool combineWithAnd = false,
        //    params Expression<Func<T, string>>[] fieldSelectors)
        //{
        //    if (string.IsNullOrWhiteSpace(searchTerm)
        //        || fieldSelectors?.Length == 0)
        //        return query;

        //    // Split thành token, bỏ các mục rỗng
        //    var tokens = searchTerm
        //        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        //    if (tokens.Length == 0)
        //        return query;

        //    var parameter = Expression.Parameter(typeof(T), "x");
        //    Expression? combinedTokens = null;

        //    foreach (var rawToken in tokens)
        //    {
        //        // Chuẩn hóa token
        //        var tokenValue = ignoreCase
        //            ? rawToken.ToLowerInvariant()
        //            : rawToken;
        //        var constant = Expression.Constant(tokenValue, typeof(string));

        //        Expression? combinedFields = null;
        //        foreach (var selector in fieldSelectors)
        //        {
        //            // Invoke selector: x => selector(x)
        //            var invoked = Expression.Invoke(selector, parameter);

        //            // Nếu ignoreCase: thêm .ToLower() trước khi Contains
        //            Expression target = invoked;
        //            if (ignoreCase)
        //            {
        //                target = Expression.Call(
        //                    invoked,
        //                    typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);
        //            }

        //            // target.Contains(constant)
        //            var containsCall = Expression.Call(
        //                target,
        //                typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!,
        //                constant);

        //            combinedFields = combinedFields == null
        //                ? containsCall
        //                : Expression.OrElse(combinedFields, containsCall);
        //        }

        //        // Kết hợp token này với các token trước đó
        //        if (combinedTokens == null)
        //        {
        //            combinedTokens = combinedFields;
        //        }
        //        else
        //        {
        //            combinedTokens = combineWithAnd
        //                ? Expression.AndAlso(combinedTokens, combinedFields)
        //                : Expression.OrElse(combinedTokens, combinedFields);
        //        }
        //    }

        //    var lambda = Expression.Lambda<Func<T, bool>>(combinedTokens!, parameter);
        //    return query.Where(lambda);
        //}

    }
}
