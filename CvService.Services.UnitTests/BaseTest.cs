using CvService.Repositories.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.UnitTests
{
  public class BaseTest
  {
    protected CvContext GetSqlLiteContext()
    {
      var connection = new SqliteConnection("DataSource=:memory:");
      connection.Open();
      var options = new DbContextOptionsBuilder<CvContext>().UseSqlite(connection).Options;
      var context = new CvContext(options);
      context.Database.EnsureCreated();
      return context;
    }
  }
}
