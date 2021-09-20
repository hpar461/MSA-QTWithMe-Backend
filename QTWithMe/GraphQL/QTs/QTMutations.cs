using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Newtonsoft.Json;
using QTWithMe.Data;
using QTWithMe.Extensions;
using QTWithMe.Models;

namespace QTWithMe.GraphQL.QTs
{
    [ExtendObjectType(name: "Mutation")]
    public class QTMutations
    {
        [UseAppDbContext]
        public async Task<QT> AddQTAsync(AddQTInput input, [ScopedService] AppDbContext context,
            CancellationToken cancellationToken)
        {
            string passageText = FetchPassageText(input.Passage);

            var qt = new QT
            {
                Passage = input.Passage,
                PassageText = passageText,
                Content = "",
                UserId = int.Parse(input.UserId),
                Modified = DateTime.Now,
                Created = DateTime.Now,
            };
            context.QTs.Add(qt);

            await context.SaveChangesAsync(cancellationToken);

            return qt;
        }

        [UseAppDbContext]
        public async Task<QT> EditQTAsync(EditQTInput input, [ScopedService] AppDbContext context,
            CancellationToken cancellationToken)
        {
            var qt = await context.QTs.FindAsync(int.Parse(input.QtId));

            if (input.Passage != null)
            {
                string passageText = FetchPassageText(input.Passage);
                qt.PassageText = passageText;
            }

            qt.Passage = input.Passage ?? qt.Passage;
            qt.Content = input.Content ?? qt.Content;
            qt.Modified = DateTime.Now;

            await context.SaveChangesAsync(cancellationToken);

            return qt;
        }

        private string FetchPassageText(string passage)
        {
            var passageRetrieverClient = new WebClient();

            passageRetrieverClient.Headers.Add(
                "Authorization",
                "Token 7be615b302c99c7f95fe7c40d286e13fc4e3dc7a"
            );

            string fetchAddress = "https://api.esv.org/v3/passage/text/?q=" +
                                  passage +
                                  "&include-footnotes=false" +
                                  "&include-passage-references=false" +
                                  "&include-short-copyright=false";
            string fetchedJSON = passageRetrieverClient.DownloadString(fetchAddress);

            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(fetchedJSON);

            return jsonObj["passages"][0];
        }
    }
}