using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Repositories;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.EndPoints
{
    public static class GenresEndPoints
    {

        public static RouteGroupBuilder MapGenres( this RouteGroupBuilder group)
        {
            //Get All Genre
            group.MapGet("/", GetGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));

            //Get Genre By Its Id
            group.MapGet("/{id:int}", GetById);

            //Create (Post) All Genre 
            group.MapPost("/", Create);

            // Update Genre
            group.MapPut("/{id:int}", Update);

            //Delete 
            group.MapDelete("/{id:int}", Delete);

            return group;
        }

        static async Task<Ok<List<Genre>>> GetGenres(IGenresRepository repository)
        {
            var genre = await repository.GetAll();
            return TypedResults.Ok(genre);
        }



        static async Task<Results<Ok<Genre>, NotFound>> GetById(int id, IGenresRepository repository)
        {
            var genre = await repository.GetById(id);
            if (genre is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(genre);
        }




        static async Task<Created<Genre>> Create(Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore)
        {
            var id = await repository.Create(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.Created($"/genres/{id}", genre);
        }




        static async Task<Results<NoContent, NotFound>> Update(int id, Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }
            await repository.Update(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }




        static async Task<Results<NoContent, NotFound>> Delete(int id, IGenresRepository repository, IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }
            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }
    }

}
