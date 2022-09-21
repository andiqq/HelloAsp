using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InfoDataContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("InfoDataContext")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.MapGet("/", () => "It works!");

app.MapGet("/hello", async (InfoDataContext context) => await context.infoDatas.ToListAsync());

app.MapGet("hello/{id}", async (InfoDataContext context, int id) =>
    await context.infoDatas.FindAsync(id) is InfoData infoData ?
    Results.Ok(infoData) :
    Results.NotFound("Name not found"));

app.MapPost("/hello", async (InfoDataContext context, InfoData infoData) =>
{
    if (infoData == null) return Results.NoContent();

    context.infoDatas.Add(infoData);
    await context.SaveChangesAsync();
    return Results.Ok(infoData);
});

app.MapPut("/hello/{id}", async (InfoDataContext context, InfoData infoData, int id) =>
{
    var dbName = await context.infoDatas.FindAsync(id);
    if (dbName == null) return Results.NotFound("Name not found.");

    dbName.name = infoData.name;
    await context.SaveChangesAsync();

    return Results.Ok(infoData);
});

app.MapDelete("/hello/{id}", async (InfoDataContext context, int id) =>
{
    var dbName = await context.infoDatas.FindAsync(id);
    if (dbName == null) return Results.NotFound("Name not found");

    context.infoDatas.Remove(dbName);
    await context.SaveChangesAsync();

    return Results.Ok("Entry deleted");
});

app.MapPost("info", (InfoData infoData) => $"Hello {infoData.name}!");

app.MapPost("JSONinfo", (InfoData infoData) => infoData);

app.Run();


