using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCorsPolicy();
builder.Services.AddIdentityServices();
builder.Services.AddSignalRServices();
builder.Services.AddQuartzScheduler(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // This allows serving files from wwwroot.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityEndpoints();
app.MapSignalRHubs();

await app.InitializeDatabaseAsync();

app.Run();
