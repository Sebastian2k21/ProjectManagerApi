using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectManagerApi.Data;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Data.Repositories.ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Profiles;
using ProjectManagerApi.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});



var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>(sql => sql.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<IBaseRepository<User, int>, UserRepository>();
builder.Services.AddScoped<IBaseRepository<Tech, int>, TechRepository>();
builder.Services.AddScoped<IBaseRepository<Language, int>, LanguageRepository>();
builder.Services.AddScoped<IBaseRepository<Project, int>, ProjectRepository>();
builder.Services.AddScoped<IBaseRepository<Team, int>, TeamRepository>();
builder.Services.AddScoped<IBaseRepository<Role, int>, RoleRepository>();
builder.Services.AddScoped<IBaseRepository<TeamUser, (int userId, int teamId, int RoleId)>, TeamUserRepository>();

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();

var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var key = builder.Configuration["Jwt:Key"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            ),
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
