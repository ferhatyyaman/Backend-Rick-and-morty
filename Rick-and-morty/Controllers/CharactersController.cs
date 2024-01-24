using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rick_and_morty.Models;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static Rick_and_morty.Models.CharacterApiResponse;

namespace Rick_and_morty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CharactersController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("GetAndSaveRemoteCharacters")]
        public async Task<IActionResult> GetAndSaveRemoteCharacters()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync("https://rickandmortyapi.com/api/character");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                try
                {
                    var apiResponse =JsonSerializer.Deserialize<CharacterApiResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var characters = apiResponse.Results;
                foreach (var apiCharacter in characters)
                {
                    var character = new Character
                    {
                        Name = apiCharacter.Name,
                        Status = apiCharacter.Status,
                        Species = apiCharacter.Species,
                        Type = apiCharacter.Type,
                        Gender = apiCharacter.Gender,
                        Url = apiCharacter.Url,
                        Created = apiCharacter.Created
                    };

                    _context.Characters.Add(character);
                }

                await _context.SaveChangesAsync();

                return Ok("Veriler başarıyla veritabanına eklendi.");
                }
                catch (JsonException ex)
                {
                    return BadRequest($"JSON deserialization error: {ex.Message}");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCharacters")]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            return await _context.Characters.ToListAsync();
        }
    }
}
