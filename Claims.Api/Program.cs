using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Repositories;
using Claims.Api.Modules.Claims.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Claims.Api.Modules.Claims.Interface.Service;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Dashboards.Interface.Services;
using Claims.Api.Modules.Dashboards.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter());
        }); ;

builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IInformationRequestService, InformationRequestService>();
builder.Services.AddScoped<IClaimantService, ClaimantService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IClaimsOfficerService, ClaimsOfficerService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<ISettlementService, SettlementService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();


builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IInformationRequestRepository, InformationRequestRepository>();
builder.Services.AddScoped<IClaimantRepository, ClaimantRepository>();
builder.Services.AddScoped<IClaimsOfficerRepository, ClaimsOfficerRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<ISettlementRepository, SettlementRepository>();

builder.Services.AddScoped<IClaimStatusHistoryRepository, ClaimStatusHistoryRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "ClaimsApi",

            ValidateAudience = true,
            ValidAudience = "ClaimsClient",

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            ),

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Run();