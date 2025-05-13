using AutoMapper;
using EduQuest_Application.UseCases.LessonContents.Command.UpdateQuiz;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduQuest_Domain.Entities;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.LessonContents.Command.UpdateAssignment
{
	public class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, APIResponse>
	{
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILessonMaterialRepository _lessonMaterialRepository;

		public UpdateAssignmentCommandHandler(IAssignmentRepository assignmentRepository, 
			IUnitOfWork unitOfWork, IMapper mapper, 
			ILessonMaterialRepository lessonMaterialRepository)
		{
			_assignmentRepository = assignmentRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_lessonMaterialRepository = lessonMaterialRepository;
		}

		public async Task<APIResponse> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
		{
			var isUsed = await _lessonMaterialRepository.IsMaterialUsed(request.Assignment.Id);
			if (isUsed)
			{
				return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.UpdateFailed,
				MessageError.UsedContent,
				"name",
				$"Quiz ID {request.Assignment.Id}"
			);
			}

			var assignment = _mapper.Map<Assignment>(request.Assignment);
			assignment.Id = request.Assignment?.Id ?? Guid.NewGuid().ToString();

			await _assignmentRepository.Update(assignment);
			var result = await _unitOfWork.SaveChangesAsync();
			if (result > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.UpdateSuccesfully,
					assignment,
					"name",
					$"Assignment ID {request.Assignment.Id}"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.UpdateFailed,
				"Saving Failed",
				"name",
				$"Assignment ID {request.Assignment.Id}"
			);
		}
	}
}
