using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Visitor
{
    public interface ICollaboratorWithoutUserVisitor
    {
        Guid Id { get; }
        string Names { get; }
        string Surnames { get; }
        string Email { get; }
        DateTime? DeactivationDate { get; }
        PeriodDateTime PeriodDateTime { get; }
    }
}