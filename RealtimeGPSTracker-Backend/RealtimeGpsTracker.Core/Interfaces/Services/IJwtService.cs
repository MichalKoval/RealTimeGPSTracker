namespace RealtimeGpsTracker.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateSecurityToken(string userId);
    }
}
