namespace QTWithMe.GraphQL.Comments
{
    public record AddCommentInput(
        string Content,
        string QtId,
        string UserId);
}