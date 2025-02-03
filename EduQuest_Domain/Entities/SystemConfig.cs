using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Entities
{
	[Table("SystemConfig")]
	public partial class SystemConfig : BaseEntity
	{
        public string? Name { get; set; }
        public double? Value { get; set; }
        public string? Description { get; set; }
        
    }
}
