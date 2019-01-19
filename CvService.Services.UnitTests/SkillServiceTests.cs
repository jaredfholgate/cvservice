using CvService.Repositories.Repositories;
using CvService.Services.Models;
using CvService.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CvService.Services.UnitTests
{
  [TestClass]
  public class SkillServiceTests : BaseTest
  {
    [TestMethod]
    public void AddAndRetieveSkill()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var skillService = new SkillService(new SkillRepository(context), new Mapper().GetMapper());
      
      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var cvId = cvService.Add(cv, Constants.RootUrl).Id;

      //Act
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var newSkill = skillService.AddToCv(skill, cvId, Constants.RootUrl);

      //Assert
      var skills = skillService.GetForCv(cvId, Constants.RootUrl);
      var result = skills[0];

      Assert.AreEqual(skill.Name, result.Name);
      Assert.AreEqual(skill.Blurb, result.Blurb);
      Assert.AreEqual(skill.Order, result.Order);
      Assert.AreEqual(cvId, result.CvId);
    }

    [TestMethod]
    public void UpdateSkills()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var skillService = new SkillService(new SkillRepository(context), new Mapper().GetMapper());

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var cvId = cvService.Add(cv, Constants.RootUrl).Id;
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var newSkill = skillService.AddToCv(skill, cvId, Constants.RootUrl);
      var skillId = newSkill.Id;

      //Act
      var skillUpdate = new Skill() { Id = skillId, Name = "C#", Blurb = "Been using it since 2001.", Order = 24 };
      skillService.Update(skillUpdate);

      //Assert
      var result = skillService.Get(skillId, Constants.RootUrl);

      Assert.AreEqual(skillUpdate.Name, result.Name);
      Assert.AreEqual(skillUpdate.Blurb, result.Blurb);
      Assert.AreEqual(skillUpdate.Order, result.Order);
      Assert.AreEqual(cvId, result.CvId);
    }

    [TestMethod]
    public void CheckUrlsAreCorrect()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var skillService = new SkillService(new SkillRepository(context), new Mapper().GetMapper());

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var cvId = cvService.Add(cv, Constants.RootUrl).Id;

      //Act
      var skill1 = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 5 };
      skillService.AddToCv(skill1, cvId, Constants.RootUrl);
      var skill2 = new Skill() { Name = "DevOps", Blurb = "DevOps Master", Order = 3 };
      skillService.AddToCv(skill2, cvId, Constants.RootUrl);
      var skill3 = new Skill() { Name = "Agile", Blurb = "Agile Expert", Order = 1 };
      skillService.AddToCv(skill3, cvId, Constants.RootUrl);
      var skill4 = new Skill() { Name = "Software Engineering", Blurb = "Since 2001", Order = 2 };
      skillService.AddToCv(skill4, cvId, Constants.RootUrl);

      //Assert
      var skills = skillService.GetForCv(cvId, Constants.RootUrl);
      
      foreach(var skill in skills)
      {
        Assert.AreEqual($"{Constants.RootUrl}/cv/{skill.CvId}", skill.Links.Single(o => o.Rel == "cv").Href);
        Assert.AreEqual($"{Constants.RootUrl}/skill/{skill.Id}", skill.Links.Single(o => o.Rel == "self").Href);
      }
    }
  }
}
