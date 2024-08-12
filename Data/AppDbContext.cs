using Microsoft.EntityFrameworkCore;
using studentAPI.Models;
namespace studentAPI.Data;

public class AppDbContext: DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

  public DbSet<Student> Students { get; set; }
}
