using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using QTWithMe.Data;
using QTWithMe.Models;

namespace QTWithMe.GraphQL.Users
{
    [ExtendObjectType(name: "Query")]
    public class UserQueries
    {
        public IQueryable<User> GetUsers([ScopedService] AppDbContext context)
        {
            return context.Users;
        }
    }
}