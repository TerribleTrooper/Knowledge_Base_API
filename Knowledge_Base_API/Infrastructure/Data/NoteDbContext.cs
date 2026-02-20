using Knowledge_Base_API.Entity;
using Microsoft.EntityFrameworkCore;

namespace Knowledge_Base_API;

public class NoteDbContext : DbContext
{
    public NoteDbContext(DbContextOptions<NoteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
}