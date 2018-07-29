using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService
{
    /// <summary>
    /// Classe de suporte
    /// </summary>
    public class Helper
    {
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var exp = (MemberExpression)expression.Body;
            return exp.Member.Name;
        }
    }
}
