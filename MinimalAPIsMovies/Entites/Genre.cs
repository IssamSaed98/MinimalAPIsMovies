using System.ComponentModel.DataAnnotations;

namespace MinimalAPIsMovies.Entites
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
