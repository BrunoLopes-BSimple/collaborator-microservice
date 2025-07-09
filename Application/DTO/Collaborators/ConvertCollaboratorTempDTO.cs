using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO.Collaborators;

public class ConvertCollaboratorTempDTO
{
    public Guid UserId { get; }
    public string Email { get; private set; }

    public ConvertCollaboratorTempDTO(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}