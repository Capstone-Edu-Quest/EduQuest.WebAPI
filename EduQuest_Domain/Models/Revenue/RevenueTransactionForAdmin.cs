using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Revenue
{
	public class RevenueTransactionForAdmin
	{
        public string? InstructorName { get; set; }
        public int? TransactionType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
