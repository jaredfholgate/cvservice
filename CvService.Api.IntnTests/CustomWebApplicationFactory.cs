using CvService.Repositories.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CvService.Api.IntnTests
{
  public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureServices(services =>
      {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        services.AddDbContext<CvContext>(options =>
        {
          options.UseSqlite(connection);
        });

        var sp = services.BuildServiceProvider();

        using (var scope = sp.CreateScope())
        {
          var scopedServices = scope.ServiceProvider;
          var db = scopedServices.GetRequiredService<CvContext>();
          db.Database.EnsureCreated();
        }
      });
    }
  }
}
