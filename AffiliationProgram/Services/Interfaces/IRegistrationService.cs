using AffiliationProgram.Models;

namespace AffiliationProgram.Services;

public interface IRegistrationService
{
    Task<RegistrationOut> RegisterAsync(RegisterRequest request, CancellationToken ct);
}