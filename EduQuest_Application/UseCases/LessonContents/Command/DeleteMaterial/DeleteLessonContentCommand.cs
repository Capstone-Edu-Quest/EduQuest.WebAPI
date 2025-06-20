﻿using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Materials.Command.DeleteMaterial
{
	public class DeleteLessonContentCommand : IRequest<APIResponse>
	{
        public string MaterialId { get; set; }
        public string UserId { get; set; }

		public DeleteLessonContentCommand(string materialId, string userId)
		{
			MaterialId = materialId;
			UserId = userId;
		}
	}
}
