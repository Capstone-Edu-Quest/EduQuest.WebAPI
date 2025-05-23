﻿using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.DTO.Request.Courses
{
    public class SearchCourseRequest
    {
        public string? KeywordName { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public bool? IsPublic { get; set; }
        public List<string?>? TagListId { get; set; }
        public string? Author { get; set; }
        public int? Rating { get; set; }
        public int? Sort { get; set; } 

    }
}
