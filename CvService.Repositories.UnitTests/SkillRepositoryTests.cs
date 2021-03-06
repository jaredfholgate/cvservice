using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using CvService.Tests.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class SkillRepositoryTests : BaseTest
  {
    [TestMethod]
    public void CanPersistASkill()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var skillRepository = new SkillRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;
      
      //Act
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD" };
      skillRepository.AddToCv(skill, cvId);

      //Assert
      var result = skillRepository.GetForCv(cvId)[0];

      Assert.AreEqual(skill.Name, result.Name);
      Assert.AreEqual(skill.Blurb, result.Blurb);
      Assert.AreEqual(cvId, result.Cv.Id);
    }

    [TestMethod]
    public void CanUpdateSkill()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var skillRepository = new SkillRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb }; 
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD" };
      skillRepository.AddToCv(skill, cvId);
      var skillId = skillRepository.GetForCv(cvId)[0].Id;

      //Act
      var skillUpdate = new Skill() { Id = skillId, Name = "C#", Blurb = "Been using it since 2001." };
      skillRepository.Update(skillUpdate);

      //Assert
      var result = skillRepository.Get(skillId);

      Assert.AreEqual(skillUpdate.Name, result.Name);
      Assert.AreEqual(skillUpdate.Blurb, result.Blurb);
      Assert.AreEqual(cvId, result.Cv.Id);
    }

    [TestMethod]
    public void CanDeleteSkill()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var skillRepository = new SkillRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD" };
      skillRepository.AddToCv(skill, cvId);
      var skillId = skillRepository.GetForCv(cvId)[0].Id;

      //Act
      skillRepository.Delete(skillId);

      //Assert
      Assert.AreEqual(0, skillRepository.GetForCv(cvId).Count);
    }

    [TestMethod]
    public void MultipleSkillsAreReturnedInOrder()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var skillRepository = new SkillRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;

      //Act
      var skill1 = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 5 };
      skillRepository.AddToCv(skill1, cvId);
      var skill2 = new Skill() { Name = "DevOps", Blurb = "DevOps Master", Order = 3 };
      skillRepository.AddToCv(skill2, cvId);
      var skill3 = new Skill() { Name = "Agile", Blurb = "Agile Expert", Order = 1 };
      skillRepository.AddToCv(skill3, cvId);
      var skill4 = new Skill() { Name = "Software Engineering", Blurb = "Since 2001", Order = 2 };
      skillRepository.AddToCv(skill4, cvId);

      //Assert
      var results = skillRepository.GetForCv(cvId);

      var order = 0;
      foreach(var result in results)
      {
        Assert.IsTrue(result.Order > order);
        order = result.Order;
      }
    }
  }
}
