namespace Server.Interfaces;

public interface ISessionService
{
    public string? GetClaim(string claimType);
}
