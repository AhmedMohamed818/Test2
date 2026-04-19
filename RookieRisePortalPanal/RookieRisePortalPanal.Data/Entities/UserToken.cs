using RookieRisePortalPanal.Data.Entities.Enums;

public class UserToken
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public TokenType Type { get; set; }   

    public DateTime ExpirationTime { get; set; }

    public bool IsUsed { get; set; } = false;

    public DateTime CreatedAt { get; set; }
}