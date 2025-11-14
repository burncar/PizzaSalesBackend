using Microsoft.EntityFrameworkCore;
using OrderDetailSalesBackend.Application.Services;
using OrderSalesBackend.Application.Services;
using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Application.Repository;
using PizzaSalesBackend.Application.Services;
using PizzaSalesBackend.Data;
using PizzaTypeSalesBackend.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PizzaAppConnection"));
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICsvHelper, CsvHelperService>();
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<PizzaTypeService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderDetailService>();
builder.Services.AddScoped<ReportService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
