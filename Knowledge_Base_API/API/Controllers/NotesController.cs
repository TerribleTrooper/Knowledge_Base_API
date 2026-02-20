using Knowledge_Base_API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Knowledge_Base_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly NoteDbContext _context;
    private readonly IElasticClient _elastic;

    public NotesController(NoteDbContext context, IElasticClient elastic)
    {
        _context = context;
        _elastic = elastic;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await _context.Notes.ToListAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var note = await _context.Notes.FindAsync(id);

        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Note note)
    {
        note.Id = Guid.NewGuid();

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        await _elastic.IndexDocumentAsync(note);

        return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Note updated)
    {
        var note = await _context.Notes.FindAsync(id);

        if (note == null)
            return NotFound();

        note.Title = updated.Title;
        note.Content = updated.Content;
        note.Tags = updated.Tags;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var note = await _context.Notes.FindAsync(id);

        if (note == null)
            return NotFound();

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}