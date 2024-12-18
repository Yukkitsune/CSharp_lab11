using Microsoft.EntityFrameworkCore;
using WebAppNorthwind.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NWContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindDatabase")));
builder.Services.AddControllers();
builder.Services.AddSingleton<DbConnectionTest>();
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
var dbConnectionTest = app.Services.GetRequiredService<DbConnectionTest>();
dbConnectionTest.TestConnection();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
