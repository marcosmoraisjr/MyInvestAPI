using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//cors
var OriginsWithAllowedAccess = "OriginsWithAllowedAccess";

builder.Services.AddCors(options =>
    options.AddPolicy(name: OriginsWithAllowedAccess,
    policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:9090")
            .WithHeaders("Content-Type");

    })
);

//Disable the automatic redirect to Https
builder.Services.AddHttpsRedirection(options => options.HttpsPort = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//database
string postgreSqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyInvestContext>(options =>
    options.UseNpgsql(postgreSqlConnection));

var app = builder.Build();

//update the database
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<MyInvestContext>();
//    context.Database.Migrate();
//}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyInvestAPI");
        c.RoutePrefix = "swagger";
    });
    app.ConfigureExceptionHandler();
}

if (app.Environment.IsProduction())
{
    app.ActiveUpdateDatabaseMigrations();
}

//app.UseHttpsRedirection();
app.UseCors(OriginsWithAllowedAccess);

app.UseAuthorization();

app.MapControllers();

app.Run();