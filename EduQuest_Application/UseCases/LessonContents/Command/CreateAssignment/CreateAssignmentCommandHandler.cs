using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.LessonContents.Command.CreateAssignment
{
	public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, APIResponse>
	{
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public CreateAssignmentCommandHandler(IAssignmentRepository assignmentRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_assignmentRepository = assignmentRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
		{
			var newAssignment = _mapper.Map<Assignment>(request.Assignment);
			newAssignment.Id = Guid.NewGuid().ToString();
			newAssignment.UserId = request.UserId;
			await _assignmentRepository.Add(newAssignment);

			var result = await _unitOfWork.SaveChangesAsync();
			if (result > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.CreateSuccesfully,
					newAssignment,
					"name",
					"New Assignment"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.CreateFailed,
				"Saving Failed",
				"name",
				"New Assignment"
			);
		}
	}
}
