using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class CvRepositoryTests
  {
    [TestMethod]
    public void CanPersistACv()
    {
      var connection = new SqliteConnection("DataSource=:memory:");
      connection.Open();

      try
      {
        var options = new DbContextOptionsBuilder<CvContext>().UseSqlite(connection).Options;

        var context = new CvContext(options);
        context.Database.EnsureCreated();

        var cvRepository = new CvRepository(context);

        var cv = new Cv() { Name = "Jared Holgate", Blurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc." };

        cvRepository.Add(cv);

        var cvs = cvRepository.Get();

        var result = cvs[0];

        Assert.AreEqual(cv.Name, result.Name);
        Assert.AreEqual(cv.Blurb, result.Blurb);
      }
      finally
      {
        connection.Close();
      }
    }
  }
}
