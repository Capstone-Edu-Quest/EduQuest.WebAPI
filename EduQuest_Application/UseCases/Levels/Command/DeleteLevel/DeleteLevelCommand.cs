using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Level.Command.DeleteLevel;

public class DeleteLevelCommand : IRequest<APIResponse>
{
    public string Id { get; set; }
    public string UserId { get; set; }
}
