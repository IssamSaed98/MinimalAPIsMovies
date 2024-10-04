using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies;
using MinimalAPIsMovies.EndPoints;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Migrations;
using MinimalAPIsMovies.Repositories;

var builder = WebApplication.CreateBuilder(args);



// Services Zoon
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
options.UseSqlServer("name=DefaultConnection");
});
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["allowedOrigins"]!).AllowAnyMethod().AllowAnyHeader();
    });
    option.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGenresRepository, GenresRepository>();

// End S Zoon



var app = builder.Build();



//Middelware Zoon
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseOutputCache();











app.MapGet("/", () => "Hello World!");
app.MapGroup("/genres").MapGenres(); 



// End MW Zoon
app.Run();




