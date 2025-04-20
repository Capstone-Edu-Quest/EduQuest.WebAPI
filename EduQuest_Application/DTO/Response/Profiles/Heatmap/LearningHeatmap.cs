using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles.Heatmap;

public class LearningHeatmap : IMapFrom<StudyTime>, IMapTo<StudyTime> 
{
    public string Date { get; set; }
    public double Count { get; set; }
}
