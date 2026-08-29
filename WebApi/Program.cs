using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    dbContextOptionsBuilder =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Database");
        dbContextOptionsBuilder.UseSqlServer(connectionString);
    });

builder
    .Services
    .Scan(selector => selector
        .FromAssemblies(AssemblyReference.Assembly)
        .AddClasses(false)
        .AsImplementedInterfaces()
        .WithScopedLifetime());

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();