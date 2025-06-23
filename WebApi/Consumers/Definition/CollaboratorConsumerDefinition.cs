using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace WebApi.Consumers.Definition
{
    public class CollaboratorConsumerDefinition : ConsumerDefinition<CollaboratorConsumer>
    {
        public CollaboratorConsumerDefinition()
        {
            EndpointName = "cmd-collaborator-created-events";
        }
    }
}