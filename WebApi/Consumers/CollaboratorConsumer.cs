using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Messaging;
using Application.Services;
using Domain.Models;
using MassTransit;

namespace WebApi.Consumers
{
    public class CollaboratorConsumer : IConsumer<CollaboratorCreatedEvent>
    {
        private readonly ICollaboratorService _collabService;

        public CollaboratorConsumer(ICollaboratorService collabService)
        {
            _collabService = collabService;
        }

        public async Task Consume(ConsumeContext<CollaboratorCreatedEvent> context)
        {
            await _collabService.AddCollaboratorReferenceAsync(context.Message.Id, context.Message.UserId, context.Message.PeriodDateTime);
        }
    }
}