using Ctodo.Models;
using Microsoft.EntityFrameworkCore;

namespace Ctodo.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Todo> Todos { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}