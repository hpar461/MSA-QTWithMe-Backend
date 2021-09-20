using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using QTWithMe.Data;
using QTWithMe.GraphQL.Comments;
using QTWithMe.GraphQL.QTs;
using QTWithMe.Models;
using User = QTWithMe.Models.User;

namespace QTWithMe.GraphQL.Users
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(u => u.Id).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.Name).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.GitHub).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.ImageURI).Type<NonNullType<StringType>>();

            descriptor
                .Field(s => s.Qts)
                .ResolveWith<Resolvers>(r => r.GetQTs(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<QTType>>>>();

            descriptor
                .Field(s => s.Comments)
                .ResolveWith<Resolvers>(r => r.GetComments(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<CommentType>>>>();
        }
        
        private class Resolvers
        {
            public async Task<IEnumerable<QT>> GetQTs(User user, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.QTs.Where(c => c.UserId == user.Id).ToArrayAsync(cancellationToken);
            }

            public async Task<IEnumerable<Comment>> GetComments(User user, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Comments.Where(c => c.UserId == user.Id).ToArrayAsync(cancellationToken);
            }
        }
    }
}