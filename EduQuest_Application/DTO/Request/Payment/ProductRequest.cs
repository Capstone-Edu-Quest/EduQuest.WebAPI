using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Payment
{
	public class ProductRequest
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
