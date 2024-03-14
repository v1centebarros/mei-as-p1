using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using api.Data;
using Microsoft.AspNetCore.Identity;
using api.Models.Contracts;
using api.Repositories;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Net.Http.Headers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddOpenTelemetryTracing(budiler =>
            {
                budiler
                    .AddAspNetCoreInstrumentation(opt =>
                    {
                        opt.RecordException = true;
                    })
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("API_Service")
                        .AddTelemetrySdk()
                    )
                    .SetErrorStatusOnException(true)
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317");
                    });
            }).AddOpenTelemetryMetrics(options =>
            {
                options
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("Worker")
                        .AddEnvironmentVariableDetector()
                        .AddTelemetrySdk()
                    )
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317");
                    });
            });

builder.Services.AddOpenTelemetryMetrics(options =>
{
    options
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService("Worker")
            .AddEnvironmentVariableDetector()
            .AddTelemetrySdk()
        )
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:4317");
        });
});

builder.Services.AddOpenTelemetryTracing(budiler =>
{
    budiler
        .AddAspNetCoreInstrumentation(opt =>
        {
            opt.RecordException = true;
        })
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService("Org.WebAPI")
            .AddTelemetrySdk()
        )
        .SetErrorStatusOnException(true)
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:4317"); // Signoz Endpoint
        });
});



//Starting
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection String is not found"));
});

//Add Identity & JWT authentication
//Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>();

// JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

//Add authentication to Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddOpenTelemetryTracing(budiler =>
    {
        budiler
            .AddAspNetCoreInstrumentation()
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("MyWebApp")
                .AddTelemetrySdk()
            )
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:4317"); // Signoz Endpoint
            });
    });

builder.Services.AddScoped<IUserAuth, AuthRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
//Ending...
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        policy.WithOrigins("http://localhost:7254", "https://localhost:7254", "http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders(HeaderNames.ContentType);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();