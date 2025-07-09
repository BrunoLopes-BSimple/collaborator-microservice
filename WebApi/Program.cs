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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<UserService>();

builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();

//Repositories
builder.Services.AddTransient<IUserRepository, UserRepositoryEF>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepositoryEF>();
builder.Services.AddTransient<ICollaboratorWithoutUserRepository, CollaboratorWithoutUserRepositoryEF>();


//Factories
builder.Services.AddTransient<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddTransient<IUserFactory, UserFactory>();
builder.Services.AddTransient<ICollaboratorWithoutUserFactory, CollaboratorWithoutUserFactory>();


//Mappers
builder.Services.AddTransient<UserDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<CollaboratorWithoutUserDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<Collaborator, CollaboratorDTO>();
    cfg.CreateMap<CollaboratorWithoutUser, CollaboratorWithoutUserDTO>();
});

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<CollaboratorCreatedConsumer>();
    x.AddConsumer<CollaboratorUpdatedConsumer>();

    x.AddSagaStateMachine<CollaboratorCreatedStateMachine, CollaboratorCreatedState>()
        .InMemoryRepository();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        var instance = InstanceInfo.InstanceId;
        cfg.ReceiveEndpoint($"collaborators-cmd-{instance}", e =>
        {
            e.ConfigureConsumer<CollaboratorCreatedConsumer>(context);
            e.ConfigureConsumer<CollaboratorUpdatedConsumer>(context);
            e.ConfigureConsumer<UserCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint($"collaborators-cmd-saga-{instance}", e =>
        {
            e.StateMachineSaga<CollaboratorCreatedState>(context);
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

app.Run();

public partial class Program { }
