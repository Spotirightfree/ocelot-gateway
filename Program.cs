using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseIISIntegration();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

if (builder.Environment.IsDevelopment())
{
    IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.Development.json")
                            .Build();
    builder.Services.AddOcelot(configuration);
}
else
{
    IConfiguration configuration = new ConfigurationBuilder()
                           .AddJsonFile("ocelot.Production.json")
                           .Build();
    builder.Services.AddOcelot(configuration);
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot();
//app.UseHttpsRedirection();
app.Run();