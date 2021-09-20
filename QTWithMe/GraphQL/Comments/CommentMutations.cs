using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using QTWithMe.Data;
using QTWithMe.Extensions;
using QTWithMe.Models;

namespace QTWithMe.GraphQL.Comments
{
    [ExtendObjectType(name: "Mutation")]
    public class CommentMutations
    {
        [UseAppDbContext]
        [Authorize]
        public async Task<Comment> AddCommentAsync(AddCommentInput input, ClaimsPrincipal claimsPrincipal, 
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userIdStr = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var comment = new Comment
            {
                Content = input.Content,
                QtId = int.Parse(input.QtId),
                UserId = int.Parse(userIdStr),
                Modified = DateTime.Now,
                Created = DateTime.Now
            };
            context.Comments.Add(comment);

            await context.SaveChangesAsync(cancellationToken);

            return comment;
        }

        [UseAppDbContext]
        [Authorize]
        public async Task<Comment> EditCommentAsync(EditCommentInput input, ClaimsPrincipal claimsPrincipal,
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userIdStr = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var comment = await context.Comments.FindAsync(int.Parse(input.CommentId));

            if (comment.UserId != int.Parse(userIdStr))
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Not owned by the current user.")
                    .SetCode("AUTH_NOT_AUTHORIZED")
                    .Build());
            }
            
            comment.Content = input.Content ?? comment.Content;

            await context.SaveChangesAsync(cancellationToken);

            return comment;
        }
    }
}