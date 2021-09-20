using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using QTWithMe.Data;
using QTWithMe.Models;

namespace QTWithMe.GraphQL.Comments
{
    public class CommentType: ObjectType<Comment>
    {
        protected override void Configure(IObjectTypeDescriptor<Comment> descriptor)
        {
            descriptor.Field(s => s.Id).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.Content).Type<NonNullType<StringType>>();

            descriptor
                .Field(s => s.Qt)
                .ResolveWith<Resolvers>(r => r.GetProject(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<CommentType>>();

            descriptor
                .Field(s => s.User)
                .ResolveWith<Resolvers>(r => r.GetStudent(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<CommentType>>();

            descriptor.Field(p => p.Modified).Type<NonNullType<DateTimeType>>();
            descriptor.Field(p => p.Created).Type<NonNullType<DateTimeType>>();

        }

        private class Resolvers
        {
            public async Task<QT> GetProject(Comment comment, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.QTs.FindAsync(new object[] { comment.QtId }, cancellationToken);
            }

            public async Task<User> GetStudent(Comment comment, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Users.FindAsync(new object[] { comment.UserId }, cancellationToken);
            }
        }
    }
}