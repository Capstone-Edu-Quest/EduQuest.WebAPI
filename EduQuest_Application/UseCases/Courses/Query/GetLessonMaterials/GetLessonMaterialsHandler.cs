using AutoMapper;
using EduQuest_Application.DTO.Response.Materials.DetailMaterials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
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

namespace EduQuest_Application.UseCases.Courses.Query.GetLessonMaterials;

public class GetLessonMaterialsHandler : IRequestHandler<GetLessonMaterialsQuery, APIResponse>
{
    private readonly ILearnerRepository _learnerRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;
    private const string key = "name";
    private const string value = "materials";
    public GetLessonMaterialsHandler(ILearnerRepository learnerRepository,ILessonRepository lessonRepository, 
        IMaterialRepository materialRepository, IMapper mapper)
    {
        _learnerRepository = learnerRepository;
        _lessonRepository = lessonRepository;
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetLessonMaterialsQuery request, CancellationToken cancellationToken)
    {
        var lessonMaterials = await _lessonRepository.GetMaterialsByLessonId(request.LessonId);
        var Materials = await _materialRepository.GetMaterialsByIds(lessonMaterials.Select(l => l.MaterialId).ToList());
        var lesson = await _lessonRepository.GetById(request.LessonId);
        
        if (lesson == null)
        {
            GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.GetFailed, MessageCommon.NotFound, key, value);
        }
        var learner = await _learnerRepository.GetByUserIdAndCourseId(request.UserId, lesson.CourseId);
        var currentLesson = await _lessonRepository.GetById(learner.CurrentLessonId);

        List<LearnerMaterialResponseDto> mapper = _mapper.Map<List<LearnerMaterialResponseDto>>(Materials);
        var currentMaterial = lessonMaterials.FirstOrDefault(m => m.MaterialId == learner.CurrentMaterialId);
        if (currentLesson != null && currentLesson.Index > lesson.Index)
        {
            foreach(var item in mapper)
            {
                item.Status = StatusMaterial.Done.ToString();
            }
        }
        else if(currentLesson != null && currentLesson.Index < lesson.Index || currentLesson == null)
        {
            foreach (var item in mapper)
            {
                item.Status = StatusMaterial.Locked.ToString();
            }
        }
        else if(currentLesson != null && currentLesson.Index == lesson.Index)
        {
            foreach(var item in mapper)
            {
                var Material = lessonMaterials.FirstOrDefault(m => m.MaterialId == item.Id);
                if (currentMaterial.Index < Material.Index)
                {
                    item.Status += StatusMaterial.Locked.ToString();
                }else if(currentMaterial.Index > Material.Index)
                {
                    item.Status += StatusMaterial.Done.ToString();
                }
                else
                {
                    item.Status += StatusMaterial.Current.ToString();
                }
            }
        }

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, mapper, key, value);
    }
}
