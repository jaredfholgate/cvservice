using CvService.Repositories.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CvService.Services.UnitTests
{
  [TestClass]
  public class CvServiceTests : BaseTest
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";
    private const string RootUrl = "http://testurl";

    [TestMethod]
    public void AddAndRetieveCv()
    {
      //Arrange
      var cvService = new CvService(new CvRepository(GetSqlLiteContext()), new Mapper().GetMapper());
      var cv = new Models.Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };

      //Act
      cvService.Add(cv, RootUrl);

      //Assert
      var cvs = cvService.Get(RootUrl);
      var result = cvs[0];

      Assert.AreEqual(cv.Name, result.Name);
      Assert.AreEqual(cv.Blurb, result.Blurb);
    }

    [TestMethod]
    public void UpdateCv()
    {
      //Arrange
      var cvService = new CvService(new CvRepository(GetSqlLiteContext()), new Mapper().GetMapper());
      var cv = new Models.Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      cvService.Add(cv, RootUrl);
      var cvId = cvService.Get(RootUrl)[0].Id;

      //Act
      var cvUpdate = new Models.Cv() { Id = cvId, Name = "Jared Holgate 2", TagLine = "DevOps and Software Engineer 2", Blurb = "Blah, Blah, Blah." };
      cvService.Update(cvUpdate);

      //Assert
      var result = cvService.Get(cvId, RootUrl);
      Assert.AreEqual("Jared Holgate 2", result.Name);
      Assert.AreEqual("DevOps and Software Engineer 2", result.TagLine);
      Assert.AreEqual("Blah, Blah, Blah.", result.Blurb);
    }

    [TestMethod]
    public void CheckUrlsAreCorrect()
    {
      //Arrange
      var cvService = new CvService(new CvRepository(GetSqlLiteContext()), new Mapper().GetMapper());
      var testCvs = new List<Models.Cv>() {
        new Models.Cv { Name = "Cv1", Blurb = "Test 1" },
        new Models.Cv { Name = "Cv2", Blurb = "Test 2" },
        new Models.Cv { Name = "Cv3", Blurb = "Test 3" }
      };

      //Act
      foreach (var cv in testCvs)
      {
        cvService.Add(cv, RootUrl);
      }

      //Assert
      var cvs = cvService.Get(RootUrl);
      
      foreach(var cv in cvs)
      {
        Assert.AreEqual($"{RootUrl}/cv/{cv.Id}", cv.Url);
        Assert.AreEqual($"{RootUrl}/cv/{cv.Id}/companies", cv.CompaniesUrl);
        Assert.AreEqual($"{RootUrl}/cv/{cv.Id}/skills", cv.SkillsUrl);
      }
    }
  }
}
