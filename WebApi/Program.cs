using System.Text;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi;
using WebApi.Configurations;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

var jwtConfiguration = builder.Configuration.GetEntry<JwtConfigurationEntry>();
byte[] keyBytes = Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfiguration.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = false
        };
    });

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

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Room Booking API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token directly below (without typing 'Bearer ')."
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

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