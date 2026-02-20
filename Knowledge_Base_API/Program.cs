using Knowledge_Base_API;
using Knowledge_Base_API.Entity;
using Microsoft.EntityFrameworkCore;
using Nest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var elasticSettings = builder.Configuration.GetSection("Elastic");

var settings = new ConnectionSettings(
        new Uri(elasticSettings["Url"]!)
    )
    .DefaultIndex(elasticSettings["Index"]);

var client = new ElasticClient(settings);

builder.Services.AddSingleton<IElasticClient>(client);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var elastic = scope.ServiceProvider.GetRequiredService<IElasticClient>();

    var indexName = builder.Configuration["Elastic:Index"];

    var exists = await elastic.Indices.ExistsAsync(indexName);

    if (!exists.Exists)
    {
        await elastic.Indices.CreateAsync(indexName, c =>
            c.Map<Note>(m => m.AutoMap())
        );
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();

