using Microsoft.EntityFrameworkCore;
using static Rick_and_morty.Models.CharacterApiResponse;

namespace Rick_and_morty.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
        }

        public DbSet<EpisodeApiResponse.Episode> Episodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpisodeApiResponse.Episode>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CharacterApiResponse.Character>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
        }

        public DbSet<CharacterApiResponse.Character> Characters { get; set; }


}
}
