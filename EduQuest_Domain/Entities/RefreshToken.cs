namespace EduQuest_Domain.Entities;

public partial class RefreshToken : BaseEntity
{
    public string? UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime? ExpireAt { get; set; }

    public virtual User? User { get; set; }
}
