using System.Data;
using AffiliationProgram.Models;
using Dapper;
using MySqlConnector;

namespace AffiliationProgram.Data;

public sealed class UserRepository(MySqlDataSource dataSource) : IUserRepository
{
    public async Task<RegistrationOut> RegisterAsync(
        string email,
        string passwordHash,
        long? affiliateId,
        Guid? trackingId,
        DateTime? clickedAt,
        CancellationToken ct)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ct);

        var command = new CommandDefinition(
            "sp_user_register",
            new
            {
                _email = email,
                _password_hash = passwordHash,
                _affiliate_id = affiliateId,
                _tracking_id = trackingId?.ToString(),
                _clicked_at = clickedAt
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct);

        return await connection.QuerySingleAsync<RegistrationOut>(command);
    }
}