using ModuleA1.API;
using ModuleA2.API;
using ModuleB1.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddModuleA1();
builder.Services.AddModuleA2();
builder.Services.AddModuleB1();
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleA1.Application.DIForMediatr).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleA2.Application.DIForMediatr).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleB1.Application.DIForMediatr).Assembly);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
