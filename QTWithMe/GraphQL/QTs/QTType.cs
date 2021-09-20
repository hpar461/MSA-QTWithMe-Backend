using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using QTWithMe.Data;
using QTWithMe.GraphQL.Comments;
using QTWithMe.GraphQL.Users;
using QTWithMe.Models;

namespace QTWithMe.GraphQL.QTs
{
    public class QTType : ObjectType<QT>
    {
        protected override void Configure(IObjectTypeDescriptor<QT> descriptor)
        {
            descriptor.Field(q => q.Id).Type<NonNullType<IdType>>();
            descriptor.Field(q => q.Passage).Type<NonNullType<StringType>>();
            descriptor.Field(q => q.PassageText).Type<NonNullType<StringType>>();
            descriptor.Field(q => q.Content).Type<NonNullType<StringType>>();

            descriptor
                .Field(q => q.User)
                .ResolveWith<Resolvers>(r => r.GetUser(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<UserType>>();

            descriptor
                .Field(q => q.Comments)
                .ResolveWith<Resolvers>(r => r.GetComments(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<CommentType>>>>();

            descriptor.Field(q => q.Created).Type<NonNullType<DateTimeType>>();
            descriptor.Field(q => q.Modified).Type<NonNullType<DateTimeType>>();
        }

        private class Resolvers
        {
            public async Task<User> GetUser(QT qt, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Users.FindAsync(new object[] {qt.UserId}, cancellationToken);
            }

            public async Task<IEnumerable<Comment>> GetComments(QT qt, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Comments.Where(c => c.QtId == qt.Id).ToArrayAsync(cancellationToken);
            }
        }
    }
}