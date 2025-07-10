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
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Publishers;
using Application.IPublishers;
using Domain.Factory.CollabWithoutUserFactory;
using InterfaceAdapters.Saga;
using Application.ISender;
using InterfaceAdapters.Sender;
using InterfaceAdapters.Activities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddScoped<ICollaboratorService, CollaboratorService>();
builder.Services.AddScoped<ICollaboratorTempService, CollaboratorTempService>();

builder.Services.AddTransient<UserService>();

builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();
builder.Services.AddTransient<IMessageSender, MassTransitSender>();

builder.Services.AddScoped<CreateTempCollabActivity>();
builder.Services.AddScoped<FinalizeCollaboratorActivity>();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepositoryEF>();
builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepositoryEF>();
builder.Services.AddScoped<ICollaboratorWithoutUserRepository, CollaboratorWithoutUserRepository>();


//Factories
builder.Services.AddScoped<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddScoped<ICollaboratorWithoutUserFactory, CollaboratorWithoutUserFactory>();


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
});

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddHealthChecks();

    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<CollaboratorConsumer>();
    x.AddConsumer<CollaboratorUpdatedConsumer>();

    x.AddSagaStateMachine<CollaboratorCreationSagaStateMachine, CollaboratorCreationSagaState>()
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
            e.ConfigureConsumers(context);
        });

        cfg.ReceiveEndpoint("collaborator-creation-saga", e =>
        {
            e.ConfigureSaga<CollaboratorCreationSagaState>(context);
        });
    });
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Bloco de depuração de DI temporário
using (var scope = app.Services.CreateScope())
{
    try
    {
        Console.WriteLine("--- A VERIFICAR DEPENDÊNCIAS DA SAGA ---");
        var activity = scope.ServiceProvider.GetRequiredService<FinalizeCollaboratorActivity>();
        Console.WriteLine("--- DEPENDÊNCIAS DA FINALIZE ACTIVITY OK ---");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--- !!! ERRO DE INJEÇÃO DE DEPENDÊNCIA: {ex.Message} ---");
    }
}

// ... resto do seu Program.cs

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

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
