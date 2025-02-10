
using System.ComponentModel.DataAnnotations;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.DTO.Request.Feedbacks;

public class UpdateFeedbackRequest
{

    [Required(ErrorMessage = MessageError.ValueRequired)]
    [Range(0, 5, ErrorMessage = MessageError.RatingLimit)]
    public int Rating { get; set; }

    [Required(ErrorMessage = MessageError.ValueRequired)]
    [MaxLength(2500, ErrorMessage = MessageError.FeedbackMaxLength)]
    public string Comment { get; set; } = string.Empty;
}
