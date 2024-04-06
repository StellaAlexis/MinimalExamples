using Marten;
using Wolverine.Http;

namespace WolverineMiddlewareApply;

public class TrainerDelete
{
    [Tags("Trainer")]
    [WolverineDelete("/api/trainer")]
    public async Task<IResult> Delete(UserId userId, IDocumentSession session, CancellationToken ct)
    {
        if (await session.LoadAsync<Trainer>(userId.Id, ct) != null)
        {
            session.HardDelete<Trainer>(userId.Id);
            await session.SaveChangesAsync(ct);
        }

        return Results.Empty;
    }
}