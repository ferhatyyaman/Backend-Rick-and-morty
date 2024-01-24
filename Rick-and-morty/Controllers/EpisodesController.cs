using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rick_and_morty.Models;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static Rick_and_morty.Models.EpisodeApiResponse;

namespace Rick_and_morty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public EpisodesController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("GetAndSaveRemoteEpisodes")]
        public async Task<IActionResult> GetAndSaveRemoteEpisodes()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync("https://rickandmortyapi.com/api/episode");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                try
                {
                    var apiResponse = JsonSerializer.Deserialize<EpisodeApiResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var episodes = apiResponse.Results;

                    foreach (var apiEpisode in episodes)
                    {
                        var episode = new Episode
                        {
                            Name = apiEpisode.Name,
                            AirDate = apiEpisode.AirDate,
                            EpisodeCode = apiEpisode.EpisodeCode
                        };

                        _context.Episodes.Add(episode);
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

        [HttpGet("GetEpisodes")]
        public async Task<ActionResult<IEnumerable<Episode>>> GetEpisodes()
        {
            return await _context.Episodes.ToListAsync();
        }
    }
}
