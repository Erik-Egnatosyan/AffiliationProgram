using System.ComponentModel.DataAnnotations;

namespace AffiliationProgram.Models;

public sealed record RegisterRequest
{
    [Required] [EmailAddress] public required string Email { get; init; }

    [Required] public required string Password { get; init; }

    public Guid? TrackingId { get; init; }
}