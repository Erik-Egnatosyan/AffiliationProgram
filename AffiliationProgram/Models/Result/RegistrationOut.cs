namespace AffiliationProgram.Models;

public enum RegistrationStatus
{
    Created = 0,
    EmailTaken = 1,
    Failed = 2
}

public sealed record RegistrationOut(RegistrationStatus Status, long? UserId, bool Attributed = false);