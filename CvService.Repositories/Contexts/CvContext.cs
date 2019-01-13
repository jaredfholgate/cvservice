using CvService.Repositories.Pocos;
using Microsoft.EntityFrameworkCore;
using System;

namespace CvService.Repositories.Contexts
{
  public class CvContext : DbContext
  {
    public DbSet<Cv> Cvs { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public CvContext()
    {

    }

    public CvContext(DbContextOptions<CvContext> options) : base(options)
    {

    }
  }
}
