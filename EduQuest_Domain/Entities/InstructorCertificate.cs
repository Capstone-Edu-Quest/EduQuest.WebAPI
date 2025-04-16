using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EduQuest_Domain.Entities;

[Table("InstructorCertificate")]
public class InstructorCertificate : BaseEntity
{
    public string UserId { get; set; } = null!; 
    public string CertificateUrl { get; set; } = null!; 

    public virtual User User { get; set; } = null!;
}
