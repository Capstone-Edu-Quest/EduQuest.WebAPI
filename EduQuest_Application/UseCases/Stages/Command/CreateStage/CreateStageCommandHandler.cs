using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Stages.Command.CreateStage
{
	public class CreateStageCommandHandler : IRequestHandler<CreateStageCommand, APIResponse>
	{
		public Task<APIResponse> Handle(CreateStageCommand request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
