using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using QTWithMe.Data;
using QTWithMe.Extensions;
using QTWithMe.Models;

namespace QTWithMe.GraphQL.QTs
{
    [ExtendObjectType(name: "Query")]
    public class QTQueries
    {
        [UseAppDbContext]
        [UsePaging]
        public IQueryable<QT> GetQTs([ScopedService] AppDbContext context)
        {
            return context.QTs.OrderBy(q => q.Created);
        }

        [UseAppDbContext]
        public QT GetQT(int id, [ScopedService] AppDbContext context)
        {
            return context.QTs.Find(id);
        }
    }
}