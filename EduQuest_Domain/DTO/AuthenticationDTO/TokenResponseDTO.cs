using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.DTO.AuthenticationDTO
{
	public class TokenResponseDTO
	{
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
	}
}
