using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials;

public class UploadVideoRequest
{
    public IFormFile Chunk { get; set; }

    public string FileId { get; set; }

    public int ChunkIndex { get; set; }

    public int TotalChunks { get; set; }
}

