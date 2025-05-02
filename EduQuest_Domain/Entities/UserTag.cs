using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuest_Domain.Entities;

[Table("UserTag")]
public class UserTag : BaseEntity
{
    public string UserId { get; set; }
    public string TagId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(TagId))]
    public virtual Tag Tag { get; set; } = null!;
}
