using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rick_and_morty.Models
{
    public class EpisodeApiResponse
    {
        public EpisodeInfo Info { get; set; }
        public List<Episode> Results { get; set; }

        public class Episode
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string Name { get; set; }
            public string? AirDate { get; set; } 
            public string? EpisodeCode { get; set; }
        }
    }

    public class EpisodeInfo
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
    }



}
