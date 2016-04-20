namespace cs_challenge.Services
{
    public interface ITokenService
    {
        string GenerateToken(string identifier, string securityKey);
        bool ValidateToken(string token);
    }
}