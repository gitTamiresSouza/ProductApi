using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductApi.Application.Mappings;
using ProductApi.Application.Services;
using ProductApi.Application.Services.Interfaces;
using ProductApi.Domain.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAutoMapper(typeof(DTOToEntityMapping));
builder.Services.AddAutoMapper(typeof(EntityToDTO));

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ProductApi"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(p =>
{
    p.DefaultApiVersion = new ApiVersion(1, 0);
    p.ReportApiVersions = true;
    p.AssumeDefaultVersionWhenUnspecified = true;
})
                .AddApiExplorer(p =>
                {
                    p.GroupNameFormat = "'v'VVV";
                    p.SubstituteApiVersionInUrl = true;
                });

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Api",
        Version = "v1"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    SeedData.Initialize(services);
}

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("./v1/swagger.json", "Senff BFF App Cliente");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
