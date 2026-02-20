using Knowledge_Base_API.Entity;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Knowledge_Base_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IElasticClient _elastic;

    public SearchController(IElasticClient elastic)
    {
        _elastic = elastic;
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        var response = await _elastic.SearchAsync<Note>(s => s
            .Query(qr => qr
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(n => n.Title)
                        .Field(n => n.Content)
                        .Field(n => n.Tags)
                    )
                    .Query(q)
                )
            )
        );

        return Ok(response.Documents);
    }
}