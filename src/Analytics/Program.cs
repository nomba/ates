using System.Text;
using Analytics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ATES Analytics API", Version = "v1" });
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
builder.Services.AddDbContext<AnalyticsDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? "Datasource=AtesAnalytics.db";
    opt.UseSqlite(connectionString);
});

var key = Encoding.ASCII.GetBytes(AnalyticsOptions.JWT_SECRET_KEY);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = AnalyticsOptions.ISSUER,
            ValidAudience = AnalyticsOptions.AUDIENCE,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddHostedService<StatisticsRefresher>();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient<ITaskTopCalculator, TaskTopCalculator>();

builder.Services.AddTransient<AnalyticsDbContextSeeder>();
builder.Services.Configure<SeedingOptions>(builder.Configuration.GetSection("Seeding"));
var app = builder.Build();

// Seed database

await using (var scope = app.Services.CreateAsyncScope())
{
    var analyticsDbContext = scope.ServiceProvider.GetRequiredService<AnalyticsDbContextSeeder>();
    await analyticsDbContext.SeedAsync();
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