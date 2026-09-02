using AffiliationProgram.Models;

namespace AffiliationProgram.Data;

public interface IUserRepository
{
    Task<RegistrationOut> RegisterAsync(
        string email,
        string passwordHash,
        long? affiliateId,
        Guid? trackingId,
        DateTime? clickedAt,
        CancellationToken ct);
}