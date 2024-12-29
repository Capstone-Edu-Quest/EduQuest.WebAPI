using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.Mappings
{
	public interface IMapTo<T>
	{
		void MappingTo(Profile profile) => profile.CreateMap(GetType(), typeof(T))
	   .ForAllMembers(opt => opt.Condition(
		   (srs, dest, sourceMember) => sourceMember != null
		   )
	   );
	}
}
