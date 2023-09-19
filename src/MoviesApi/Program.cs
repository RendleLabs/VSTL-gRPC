using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using MoviesApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString("Sqlite");

builder.Services.AddDbContextPool<MovieContext>(options =>
{
  options.UseSqlite(connectionString);
});

var app = builder.Build();

app.UseCors(c =>
{
  c.AllowAnyOrigin();
  c.AllowAnyHeader();
  c.AllowAnyMethod();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/movies/genre/{genre}", async (string genre, [FromQuery] int? page, MovieContext db) =>
{
  int skip = ((page ?? 1) - 1) * 100;
  var movies = await db.Movies
    .Include(m => m.Genres)
    .Where(m => m.Genres.Any(g => g.Name == genre))
    .OrderBy(m => m.Title)
    .Skip(skip)
    .Take(100)
    .ToListAsync();

  var result = movies.Select(m => MoviesApi.Models.Movie.FromEntity(m)).ToList();

  return result;
});

app.Run();

