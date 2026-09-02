using AffiliationProgram.Models;

namespace AffiliationProgram.Data;

public interface IAffiliateRepository
{
    Task<Affiliate?> FindByCodeAsync(string code, CancellationToken ct);
}