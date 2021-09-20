using QTWithMe.Models;

namespace QTWithMe.GraphQL.Users
{
    public record LoginPayload(
        User User,
        string Jwt);
}