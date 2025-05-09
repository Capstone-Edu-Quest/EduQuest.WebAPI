using EduQuest_Application.Helper;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Materials.Query.GetAllMyMaterial
{
	public class GetAllMyMaterialQueryHandler : IRequestHandler<GetAllMyMaterialQuery, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;

		public GetAllMyMaterialQueryHandler(IMaterialRepository materialRepository)
		{
			_materialRepository = materialRepository;
		}

		public async Task<APIResponse> Handle(GetAllMyMaterialQuery request, CancellationToken cancellationToken)
		{
			var listMaterial = await _materialRepository.GetByUserId(request.UserId);
			var response = new
			{
				Videos = new
				{
					Total = listMaterial.Count(m => m.Type == TypeOfMaterial.Video.ToString()),
					Items = listMaterial.Where(m => m.Type == TypeOfMaterial.Video.ToString())
				.Select(m => new
				{
					m.Id,
					m.Title,
					m.Description,
					Duration = m.Duration // Duration for each video
				}).ToList()
				},
				Document = new
				{
					Total = listMaterial.Count(m => m.Type == TypeOfMaterial.Document.ToString()),
					Items = listMaterial.Where(m => m.Type == TypeOfMaterial.Document.ToString())
					.Select(m => new
					{
						m.Id,
						m.Title,
						m.Description,
					})
				}/*,
				Quiz = new
				{
					Total = listMaterial.Count(m => m.Type == TypeOfMaterial.Quiz.ToString()),
					Items = listMaterial.Where(m => m.Type == TypeOfMaterial.Quiz.ToString())
					.Select(m => new
					{
						m.Id,
						m.Title,
						m.Description,
						m.Quiz.TimeLimit,
						m.Quiz.PassingPercentage,
						QuestionCount = m.Quiz.Questions.Count() // Duration for each video
					}).ToList()
				},
				Assignment = new
				{
					Total = listMaterial.Count(m => m.Type == TypeOfMaterial.Assignment.ToString()),
					Items = listMaterial.Where(m => m.Type == TypeOfMaterial.Assignment.ToString())
					.Select(m => new
					{
						m.Id,
						m.Title,
						m.Description,
						m.Assignment.TimeLimit,
						m.Assignment.AnswerLanguage,
						Language = m.Assignment!.AnswerLanguage
					})
				}*/
			};

			return GeneralHelper.CreateSuccessResponse(
				   HttpStatusCode.OK,
				   MessageCommon.GetSuccesfully,
				   response,
				   "name",
				   "Material"
			);
		}
	}
}
