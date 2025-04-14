using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Materials.DetailMaterials;

public class AssignmentTypeDto : IMapFrom<Assignment>, IMapTo<Assignment>
{
    public string Id { get; set; }
    public int? TimeLimit { get; set; }
    public string? Question { get; set; }
    public string? AnswerLanguage { get; set; }
    public string? ExpectedAnswer { get; set; }
}
