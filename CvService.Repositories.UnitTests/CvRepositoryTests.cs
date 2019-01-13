using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class CvRepositoryTests : BaseTest
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";

    [TestMethod]
    public void CanPersistACv()
    {
      //Arrange
      var cvRepository = new CvRepository(GetSqlLiteContext());
      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };

      //Act
      cvRepository.Add(cv);

      //Assert
      var cvs = cvRepository.Get();
      var result = cvs[0];

      Assert.AreEqual(cv.Name, result.Name);
      Assert.AreEqual(cv.Blurb, result.Blurb);
    }

    [TestMethod]
    public void CanUpdateCv()
    {
      //Arrange
      var cvRepository = new CvRepository(GetSqlLiteContext());
      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;

      //Act
      var cvUpdate = new Cv() { Id = cvId, Name = "Jared Holgate 2", TagLine = "DevOps and Software Engineer 2", Blurb = "Blah, Blah, Blah." };
      cvRepository.Update(cvUpdate);

      //Assert
      var result = cvRepository.Get(cvId);
      Assert.AreEqual("Jared Holgate 2", result.Name);
      Assert.AreEqual("DevOps and Software Engineer 2", result.TagLine);
      Assert.AreEqual("Blah, Blah, Blah.", result.Blurb);
    }

    [TestMethod]
    public void CanDeleteCv()
    {
      //Arrange
      var cvRepository = new CvRepository(GetSqlLiteContext());
      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;

      //Act
      cvRepository.Delete(cvId);

      //Assert
      Assert.AreEqual(0, cvRepository.Get().Count);
    }
  }
}
