namespace QTWithMe.GraphQL.Comments
{
    public record EditCommentInput(
        string CommentId,
        string? Content);
}