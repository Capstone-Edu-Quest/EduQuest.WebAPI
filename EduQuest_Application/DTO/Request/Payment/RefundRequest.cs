using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Payment
{
	public class RefundRequest
	{
        public string TransactionId { get; set; }
        public string CourseId { get; set; }
    }
}
