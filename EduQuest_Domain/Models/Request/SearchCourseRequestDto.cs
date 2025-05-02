using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Request;

public class SearchCourseRequestDto
{
    public string? KeywordName { get; set; }
    public DateTime? DateTo { get; set; }
    public DateTime? DateFrom { get; set; }
    public bool? IsPublic { get; set; }
    public bool? IsStudying { get; set; }
    public List<string?>? TagListId { get; set; }
    public string? Author { get; set; }
    public int? Rating { get; set; }
    public int? Sort { get; set; }
    public int? TagType { get; set; }

}
