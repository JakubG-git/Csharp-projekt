using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TMS.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<TMS.Models.Klient> Klient { get; set; } = default!;
    public DbSet<TMS.Models.Task_> Task_ { get; set; }= default!;
    public DbSet<TMS.Models.Comments> Comments { get; set; }= default!;
    public DbSet<TMS.Models.Description> Description { get; set; }= default!;
}