using Microsoft.EntityFrameworkCore;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Application.Services;
using SplitCorrect.Domain.Services;
using SplitCorrect.Infrastructure.Persistence;
using SplitCorrect.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SplitCorrect API", Version = "v1" });
});

// Database
builder.Services.AddDbContext<SplitCorrectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Domain Services
builder.Services.AddScoped<IBalanceCalculator, BalanceCalculator>();

// Application Services
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<ExpenseService>();

// CORS (for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Don't require HTTPS in development
    // app.UseHttpsRedirection();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
