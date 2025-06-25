using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO.Collaborators
{
    public class CollabData
    {
        public Guid Id { get; }
        public PeriodDateTime PeriodDateTime { get; private set; }

        public CollabData(Guid collabId, PeriodDateTime periodDateTime)
        {
            Id = collabId;
            PeriodDateTime = periodDateTime;
        }
    }
}