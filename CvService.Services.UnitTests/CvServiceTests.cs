using CvService.Repositories.Repositories;
using CvService.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
      var cv = new Models.CvData() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };

      //Act
      cvService.Add(cv, Constants.RootUrl);

      //Assert
      var cvs = cvService.Get(Constants.RootUrl);
      var result = cvs[0];

      Assert.AreEqual(cv.Name, result.Name);
      Assert.AreEqual(cv.Blurb, result.Blurb);
    }

    [TestMethod]
    public void UpdateCv()
    {
      //Arrange
      var cvService = new CvService(new CvRepository(GetSqlLiteContext()), new Mapper().GetMapper());
      var cv = new Models.Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvService.Add(cv, Constants.RootUrl);
      var cvId = cvService.Get(Constants.RootUrl)[0].Id;

      //Act
      var cvUpdate = new Models.CvData() { Name = Constants.CvNameUpdate, TagLine = Constants.CvTagLineUpdate, Blurb = Constants.CvBlurbUpdate };
      cvService.Update(cvId, cvUpdate);

      //Assert
      var result = cvService.Get(cvId, Constants.RootUrl);
      Assert.AreEqual(Constants.CvNameUpdate, result.Name);
      Assert.AreEqual(Constants.CvTagLineUpdate, result.TagLine);
      Assert.AreEqual(Constants.CvBlurbUpdate, result.Blurb);
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
        cvService.Add(cv, Constants.RootUrl);
      }

      //Assert
      var cvs = cvService.Get(Constants.RootUrl);
      
      foreach(var cv in cvs)
      {
        Assert.AreEqual($"{Constants.RootUrl}/cv/{cv.Id}", cv.Links.Single(o => o.Rel == "self").Href);
        Assert.AreEqual($"{Constants.RootUrl}/cv/{cv.Id}/companies", cv.Links.Single(o => o.Rel == "companies").Href);
        Assert.AreEqual($"{Constants.RootUrl}/cv/{cv.Id}/skills", cv.Links.Single(o => o.Rel == "skills").Href);
      }
    }
  }
}
