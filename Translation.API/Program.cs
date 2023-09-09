using Translation.Application.Commands;
using Translation.Application.Queries;
using Translation.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Translation Services
builder.Services.AddTranslationServices(builder.Configuration);
// Add Application CQ's
builder.Services.AddSingleton<CreateTranslationJobCommand>();
builder.Services.AddSingleton<ReadTranslationQuery>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use Azure Storage Services
app.UseAzureStorageServices(builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
