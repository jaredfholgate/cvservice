using CvService.Repositories.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CvService.Services.UnitTests
{
  [TestClass]
  public class CvServiceTests : BaseTest
  {
    [TestMethod]
    public void AddAndRetieveCv()
    {
      //Arrange
      var cvService = new CvService(new CvRepository(GetSqlLiteContext()), new Mapper().GetMapper());
      var cv = new Models.Cv() { Name = "Jared Holgate", Blurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc." };

      //Act
      cvService.Add(cv);

      //Assert
      var cvs = cvService.Get();
      var result = cvs[0];

      Assert.AreEqual(cv.Name, result.Name);
      Assert.AreEqual(cv.Blurb, result.Blurb);
    }
  }
}
