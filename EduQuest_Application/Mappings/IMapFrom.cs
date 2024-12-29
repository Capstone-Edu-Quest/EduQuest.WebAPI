using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduQuest_Application.Mappings
{
	public interface IMapFrom<T>
	{
		void MappingFrom(Profile profile) => profile.CreateMap(typeof(T), GetType())
			   .ForAllMembers(opt => opt.Condition(
		   (srs, dest, sourceMember) => sourceMember != null
		   )
	   );
	}
}
