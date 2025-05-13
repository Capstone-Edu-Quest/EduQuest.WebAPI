using AutoMapper;
using EduQuest_Application.DTO.Response.Materials.DetailMaterials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetAssignmentById
{
	public class GetAssignmentByIdQueryHandler : IRequestHandler<GetAssignmentByIdQuery, APIResponse>
	{
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IMapper _mapper;

		public GetAssignmentByIdQueryHandler(IAssignmentRepository assignmentRepository, IMapper mapper)
		{
			_assignmentRepository = assignmentRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
		{
			var existedAssignment = await _assignmentRepository.GetById(request.AssignmentId);
			if (existedAssignment == null)
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Assignment ID {request.AssignmentId}");
			}

			var response = _mapper.Map<AssignmentTypeDto>(existedAssignment);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", $"Assignment ID {request.AssignmentId}");
		}
	}
}
