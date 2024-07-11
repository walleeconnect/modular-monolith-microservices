using DependentServices.Interfaces;
using DependentServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using ModuleA1.API;
using ModuleA2.API;
using ModuleB1.API;
using Monolith;
using Polly;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddModuleA1();
builder.Services.AddModuleA2();
builder.Services.AddModuleB1();
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleA1.Application.DIForMediatr).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleA2.Application.DIForMediatr).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleB1.Application.DIForMediatr).Assembly);
//HttpClient httpClient = new HttpClient()
//{
//    //BaseAddress = new Uri("http://modulebservice/"),
//    Timeout = TimeSpan.FromSeconds(30)
//};
//httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

//// Register HttpClient instance
//builder.Services.AddSingleton(httpClient);
builder.Services.AddHttpClient<IModuleAService, ModuleAMicroservices>()
     .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(new[]
    {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10)
    }))
    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)))
    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
        .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Fallback response")
        })); ;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("Authentication failed.", context.Exception);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Token validated.");
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero  // Adjust if necessary
        };
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, PermissionsHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();

//builder.Services.AddAuthorization();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("PermissionPolicy", policy =>
//        policy.Requirements.Add(new PermissionsRequirement(Permissions.None))); // Default to None
//});

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

app.UseAuthentication();
app.UseAuthorization();
app.UsePermissionMiddleware();  // Add this line to use the custom middleware
app.MapControllers();
app.Run();
