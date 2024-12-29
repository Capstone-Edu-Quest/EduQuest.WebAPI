using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
		}

		private void ApplyMappingsFromAssembly(Assembly assembly)
		{
			var types = assembly
				.GetExportedTypes()
				.Where(
					t =>
						t.GetInterfaces()
							.Any(
								i =>
									i.IsGenericType
									&& (
										i.GetGenericTypeDefinition() == typeof(IMapFrom<>)
										|| i.GetGenericTypeDefinition() == typeof(IMapTo<>)
									)
							)
				)
				.ToList();

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type);
				var mapTo =
					type.GetMethod("MappingTo")
					?? instance!.GetType().GetInterface("IMapTo`1")?.GetMethod("MappingTo");
				var mapFrom =
					type.GetMethod("MappingFrom")
					?? instance!.GetType().GetInterface("IMapFrom`1")?.GetMethod("MappingFrom");

				mapTo?.Invoke(instance, new object[] { this });
				mapFrom?.Invoke(instance, new object[] { this });
			}
		}
	}
}
