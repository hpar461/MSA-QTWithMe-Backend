using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Octokit;
using QTWithMe.Data;
using QTWithMe.Extensions;
using User = QTWithMe.Models.User;

namespace QTWithMe.GraphQL.Users
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseAppDbContext]
        [Authorize]
        public async Task<User> EditUserAsync(EditUserInput input, ClaimsPrincipal claimsPrincipal, 
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userIdStr = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var user = await context.Users.FindAsync(int.Parse(userIdStr), cancellationToken);

            user.Name = input.Name ?? user.Name;
            user.ImageURI = input.ImageURI ?? user.ImageURI;

            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        [UseAppDbContext]
        public async Task<LoginPayload> LoginAsync(LoginInput input, [ScopedService] AppDbContext context,
            CancellationToken cancellationToken)
        {
            var client = new GitHubClient(new ProductHeaderValue("QTWithMe"));

            var request = new OauthTokenRequest(Startup.Configuration["Github:ClientId"],
                Startup.Configuration["Github:ClientSecret"], input.Code);
            var tokenInfo = await client.Oauth.CreateAccessToken(request);

            if (tokenInfo.AccessToken == null)
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Bad Code")
                    .SetCode("AUTH_NOT_AUTHENTICATED")
                    .Build());
            }

            client.Credentials = new Credentials(tokenInfo.AccessToken);
            var githubUser = await client.User.Current();
            
            var user = await context.Users.FirstOrDefaultAsync(u => u.GitHub == githubUser.Login, cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    Name = githubUser.Name ?? githubUser.Login,
                    GitHub = githubUser.Login,
                    ImageURI = githubUser.AvatarUrl
                };

                context.Users.Add(user);
                await context.SaveChangesAsync(cancellationToken);
            }
            
            // authentication successful so generate jwt token
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new("userId", user.Id.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                "QTWithMe",
                "QTWithMe-User",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new LoginPayload(user, token);
        }
    }
}