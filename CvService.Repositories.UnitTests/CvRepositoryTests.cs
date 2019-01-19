using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using CvService.Tests.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class CvRepositoryTests : BaseTest
  {
    [TestMethod]
    public void CanPersistACv()
    {
      //Arrange
      var cvRepository = new CvRepository(GetSqlLiteContext());
      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };

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
      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;

      //Act
      var cvUpdate = new Cv() { Id = cvId, Name = Constants.CvNameUpdate, TagLine = Constants.CvTagLineUpdate, Blurb = Constants.CvBlurbUpdate };
      cvRepository.Update(cvUpdate);

      //Assert
      var result = cvRepository.Get(cvId);
      Assert.AreEqual(Constants.CvNameUpdate, result.Name);
      Assert.AreEqual(Constants.CvTagLineUpdate, result.TagLine);
      Assert.AreEqual(Constants.CvBlurbUpdate, result.Blurb);
    }

    [TestMethod]
    public void CanDeleteCv()
    {
      //Arrange
      var cvRepository = new CvRepository(GetSqlLiteContext());
      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;

      //Act
      cvRepository.Delete(cvId);

      //Assert
      Assert.AreEqual(0, cvRepository.Get().Count);
    }
  }
}
