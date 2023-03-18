using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;

using Oakton;

using Weasel.Core;

using Wolverine;
using Wolverine.Marten;

using WolverineConcurrency.Model;

var builder = WebApplication.CreateBuilder(args: args);

// Add services to the container.

builder
  .Services
  .AddMarten(m =>
  {
    m.Connection(connectionString: "Server=127.0.0.1; Port=5432; Database=dev; User Id=dev; Password=dev;");
    m.DatabaseSchemaName = "testing";
    m.AutoCreateSchemaObjects = AutoCreate.All;

    m.Schema
     .For<SomeAggregateView>()
     .UseOptimisticConcurrency(enabled: true);

    m.Projections.Add<SomeAggregateViewProjection>(lifecycle: ProjectionLifecycle.Inline);
  })
  .IntegrateWithWolverine(schemaName: "testing_wolverine")
  // Just letting Marten build out known database schema elements upfront
  // Helps with Wolverine integration in development.
  .ApplyAllDatabaseChangesOnStartup();
;

builder.Host.UseWolverine(w =>
{
  w.Policies.AutoApplyTransactions();
  w.Policies.UseDurableLocalQueues();
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunOaktonCommands(args);
