using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
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
            var qt = new QT
            {
                Passage = input.Passage,
                PassageText = "", // TODO: implement loading functionalities
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

            qt.Passage = input.Passage ?? qt.Passage;
            qt.Content = input.Content ?? qt.Content;
            qt.Modified = DateTime.Now;
            
            await context.SaveChangesAsync(cancellationToken);

            return qt;
        }
    }
}