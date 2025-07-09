using Application.DTO.Collaborators;
using Application.Interfaces;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using WebApi.Consumers;
using WebApi.Publishers;
using Application.IPublishers;
using WebApi.Saga;
using Domain.Factory.CollaboratorTempFactory;
using Application.ISenders;
using WebApi.Sender;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// read env variables for connection string
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<ICollaboratorTempService, CollaboratorTempService>();
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<UserService>();

builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();
builder.Services.AddTransient<IMessageSender, MassTransitSender>();

//Repositories
builder.Services.AddTransient<IUserRepository, UserRepositoryEF>();
builder.Services.AddTransient<ICollaboratorTempRepository, CollaboratorTempRepositoryEF>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepositoryEF>();


//Factories
builder.Services.AddTransient<ICollaboratorTempFactory, CollaboratorTempFactory>();
builder.Services.AddTransient<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddTransient<IUserFactory, UserFactory>();


//Mappers
builder.Services.AddTransient<UserDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<CollaboratorTempDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<Collaborator, CollaboratorDTO>();
});

// MassTransit Configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();

    x.AddSagaStateMachine<CollaboratorCreatedStateMachine, CollaboratorCreatedState>()
     .InMemoryRepository();

    x.AddActivitiesFromNamespaceContaining<CreateTempCollaboratorActivity>();
    x.AddActivitiesFromNamespaceContaining<ConvertIntoCollabActivity>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        
        var instanceId = Guid.NewGuid();

        cfg.ReceiveEndpoint($"collaborators-cmd-{instanceId}", e =>
        {
            e.ConfigureConsumers(context);
        });

        cfg.ReceiveEndpoint("collab-user-saga", e =>
        {
            e.ConfigureSaga<CollaboratorCreatedState>(context);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();
    dbContext.Database.Migrate();
}

app.Run();

public partial class Program { }
