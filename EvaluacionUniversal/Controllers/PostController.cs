using EvaluacionUniversal.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EvaluacionUniversal.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("getPost")]
        public async Task<IActionResult> GetPost()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Error al obtener los posts.");

            var contenido = await response.Content.ReadAsStringAsync();
            return Ok(contenido);
        }
    

        [HttpPost("createPost")]
        public async Task<IActionResult> CreatePost([FromBody] PostDto post)
        {
            using var httpClient = new HttpClient();
            var contenido = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(post),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", contenido);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Error al crear el post.");

            var resultado = await response.Content.ReadAsStringAsync();
            return Ok(resultado);
        }

    } 
}
