using AutoMapper;
using EduQuest_Application.DTO.Response.Materials.DetailMaterials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetLessonContentById
{
	public class GetLessonContentByIdQueryHandler : IRequestHandler<GetLessonContentByIdQuery, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly ILessonContentRepository _lessonContentRepository;
		private readonly IMapper _mapper;

		public GetLessonContentByIdQueryHandler(IMaterialRepository materialRepository, IQuizRepository quizRepository, IAssignmentRepository assignmentRepository, ILessonContentRepository lessonContentRepository, IMapper mapper)
		{
			_materialRepository = materialRepository;
			_quizRepository = quizRepository;
			_assignmentRepository = assignmentRepository;
			_lessonContentRepository = lessonContentRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetLessonContentByIdQuery request, CancellationToken cancellationToken)
		{
			var type = await _lessonContentRepository.GetMaterialTypeByIdAsync(request.LessonContentId);
			var Response = new DetailMaterialResponseDto();
			switch (type)
			{
				case GeneralEnums.TypeOfMaterial.Document:
				case GeneralEnums.TypeOfMaterial.Video:
					Material material = await _materialRepository.GetById(request.LessonContentId);
					if (material == null)
					{
						return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Material ID {request.LessonContentId}");
					}
					Response = _mapper.Map<DetailMaterialResponseDto>(material);
					return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK, MessageCommon.GetSuccesfully, Response, "name", "Material"
				);
				case GeneralEnums.TypeOfMaterial.Quiz:
					Quiz quiz = await _quizRepository.GetById(request.LessonContentId);
					if (quiz == null)
					{
						return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Quiz ID {request.LessonContentId}");
					}
					var quizResponse = _mapper.Map<QuizTypeDto>(quiz);
					Response.Id = quiz.Id;
					Response.Type = GeneralEnums.TypeOfMaterial.Quiz.ToString();
					Response.Title = quiz.Title;
					Response.Description = quiz.Description;
					Response.Duration = quiz.TimeLimit;
					Response.Quiz = quizResponse;
					return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK, MessageCommon.GetSuccesfully, Response, "name", "Quiz"
				);
				case GeneralEnums.TypeOfMaterial.Assignment:
					Assignment assignment = await _assignmentRepository.GetById(request.LessonContentId);
					if (assignment == null)
					{
						return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Assignment ID {request.LessonContentId}");
					}
					var assignmentResponse = _mapper.Map<AssignmentTypeDto>(assignment);
					Response.Id = assignment.Id;
					Response.Type = GeneralEnums.TypeOfMaterial.Assignment.ToString();
					Response.Title = assignment.Title;
					Response.Description = assignment.Description;
					Response.Duration = assignment.TimeLimit;
					Response.Assignment = assignmentResponse;
					return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK, MessageCommon.GetSuccesfully, Response, "name", "Assignment"
				);
				default:
					return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK, MessageCommon.GetSuccesfully, null, "name", "Lesson Content");

			}
		}
	}
}
