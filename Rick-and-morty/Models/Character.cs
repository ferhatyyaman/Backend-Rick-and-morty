using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rick_and_morty.Models
{
    public class CharacterApiResponse
    {
        public CharacterInfo Info { get; set; }
        public List<Character> Results { get; set; }

        public class Character
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Species { get; set; }
            public string Type { get; set; }
            public string Gender { get; set; }

            public string Url { get; set; }
            public string Created { get; set; }
        }
    }

    public class CharacterInfo
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
    }
}