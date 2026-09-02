using System.Data;
using AffiliationProgram.Models;
using Dapper;
using MySqlConnector;

namespace AffiliationProgram.Data;

public sealed class AffiliateRepository(MySqlDataSource dataSource) : IAffiliateRepository
{
    public async Task<Affiliate?> FindByCodeAsync(string code, CancellationToken ct)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ct);

        var command = new CommandDefinition(
            "sp_affiliate_get_by_code",
            new { _code = code },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<Affiliate>(command);
    }
}