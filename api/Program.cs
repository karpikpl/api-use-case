using api.Services.EastCoast;
using api.Services.WestCoast;
using common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register all dependencies as always
builder.Services.AddScoped<SeattleService>();
builder.Services.AddScoped<NewYorkService>();

// register all services for the use case
builder.Services.AddHttpContextAccessor();
builder.Services.AddUseCaseServices();

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

app.Run();
