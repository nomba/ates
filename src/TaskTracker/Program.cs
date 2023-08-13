using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskTracker;
using TaskTracker.Integration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<KafkaMessageListener>();
builder.Services.AddTransient<IKafkaMessageHandler, KafkaMessageHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ATES TaskTracker API", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. Ex: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new List<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskTrackerDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? "Datasource=TaskTracker.db";
    opt.UseSqlite(connectionString);
});

var key = Encoding.ASCII.GetBytes(AuthOptions.JWT_SECRET_KEY);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = AuthOptions.ISSUER,
            ValidAudience = AuthOptions.AUDIENCE,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddTransient<TaskTrackerDbContextSeeder>();
builder.Services.Configure<SeedingOptions>(builder.Configuration.GetSection("Seeding"));
var app = builder.Build();

// Seed database

await using (var scope = app.Services.CreateAsyncScope())
{
    var authDbContextSeeder = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContextSeeder>();
    await authDbContextSeeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();