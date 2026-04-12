using Club100API.Models;
using Club100API.Repositories;
using Club100API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                              policy =>
                              {
                                  policy.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                              });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My Super Cat API", Version = "v1" });

opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    In = ParameterLocation.Header,
    Description = "Please enter token as: Bearer {your_token}",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey, // Vi bruger ApiKey for at kunne skrive 'Bearer ' manuelt
    BearerFormat = "JWT",
    Scheme = "Bearer"
});


    // Kravet om at bruge ovenstående definition på endpoints
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Repository toggle
var storageType = builder.Configuration["StorageType"];
if (storageType == "Db")
{
    builder.Services.AddDbContext<MembersDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IMembersRepository, MembersRepositoryDb>();
}
else
{
    builder.Services.AddSingleton<IMembersRepository>(
        new MembersRepositoryList(includeData: true));
}

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseCors("AllowAll");

app.UseAuthentication(); // Checks "Who are you?"
app.UseAuthorization();  // Checks "Are you allowed to be here?"

app.MapControllers();

app.Run();