using AffiliationProgram.Data;
using AffiliationProgram.Models;
using StackExchange.Redis;

namespace AffiliationProgram.Services;

public sealed class RegistrationService(
    IUserRepository users,
    IClickTrackingStore clickStore,
    ILogger<RegistrationService> logger) : IRegistrationService
{
    public async Task<RegistrationOut> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var click = await ResolveClickAsync(request.TrackingId, ct);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var outcome = await users.RegisterAsync(
            request.Email.Trim().ToLowerInvariant(),
            passwordHash,
            click?.AffiliateId,
            request.TrackingId,
            click?.ClickedAt,
            ct);

        if (outcome.Status == RegistrationStatus.Created)
        {
            if (outcome.Attributed)
            {
                logger.LogInformation(
                    "User {UserId} attributed to affiliate {AffiliateId} ({Code})",
                    outcome.UserId, click!.AffiliateId, click.Code);

                await ConsumeClickAsync(request.TrackingId!.Value, ct);
            }
            else if (click is not null)
            {
                logger.LogWarning(
                    "User {UserId} created without attribution: affiliate {AffiliateId} no longer exists or click {TrackingId} was already used",
                    outcome.UserId, click.AffiliateId, request.TrackingId);
            }
            else if (request.TrackingId is not null)
            {
                logger.LogWarning(
                    "User {UserId} created without attribution: tracking {TrackingId} not resolved",
                    outcome.UserId, request.TrackingId);
            }
        }

        return outcome;
    }

    private async Task<TrackedClick?> ResolveClickAsync(Guid? trackingId, CancellationToken ct)
    {
        if (trackingId is null) return null;

        try
        {
            return await clickStore.GetAsync(trackingId.Value, ct);
        }
        catch (Exception ex) when (ex is RedisException or TimeoutException)
        {
            logger.LogWarning(ex,
                "Redis unavailable during registration, proceeding without attribution. TrackingId: {TrackingId}",
                trackingId);
            return null;
        }
    }

    private async Task ConsumeClickAsync(Guid trackingId, CancellationToken ct)
    {
        try
        {
            await clickStore.RemoveAsync(trackingId, ct);
        }
        catch (Exception ex) when (ex is RedisException or TimeoutException)
        {
            logger.LogWarning(ex, "Failed to remove tracking key {TrackingId}", trackingId);
        }
    }
}