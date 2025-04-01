using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities;

public partial class Booster : BaseEntity
{
    public double BoostValue { get; set; }
    public DateTime DueDate { get; set; }
    public string UserId { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
