namespace QTWithMe.GraphQL.QTs
{
    public record EditQTInput(
        string QtId,
        string? Passage,
        string? Content);
}