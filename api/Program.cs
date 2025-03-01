using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add database
string connStringName = builder.Configuration.GetConnectionString("DefaultConnectionMySQL")!;
builder.Services.AddDbContext<ApplicationDBContext>(options => {
    //plugin database that want to use
    options.UseMySql(connStringName, ServerVersion.AutoDetect(connStringName));
});

builder.Services.AddScoped<IStockRepository, StockRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//To give Swagger run, if not -> https redirect error 
app.MapControllers();

app.Run();

