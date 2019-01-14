using CvService.Repositories.Repositories;
using CvService.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CvService.Services.UnitTests
{
  [TestClass]
  public class SkillServiceTests : BaseTest
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";
    private const string RootUrl = "http://testurl";

    [TestMethod]
    public void AddAndRetieveSkill()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var skillService = new SkillService(new SkillRepository(context), new Mapper().GetMapper());
      
      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var cvId = cvService.Add(cv, RootUrl).Id;

      //Act
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var newSkill = skillService.AddToCv(skill, cvId, RootUrl);

      //Assert
      var skills = skillService.GetForCv(cvId, RootUrl);
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

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var cvId = cvService.Add(cv, RootUrl).Id;
      var skill = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var newSkill = skillService.AddToCv(skill, cvId, RootUrl);
      var skillId = newSkill.Id;

      //Act
      var skillUpdate = new Skill() { Id = skillId, Name = "C#", Blurb = "Been using it since 2001.", Order = 24 };
      skillService.Update(skillUpdate);

      //Assert
      var result = skillService.Get(skillId, RootUrl);

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

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var cvId = cvService.Add(cv, RootUrl).Id;

      //Act
      var skill1 = new Skill() { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 5 };
      skillService.AddToCv(skill1, cvId, RootUrl);
      var skill2 = new Skill() { Name = "DevOps", Blurb = "DevOps Master", Order = 3 };
      skillService.AddToCv(skill2, cvId, RootUrl);
      var skill3 = new Skill() { Name = "Agile", Blurb = "Agile Expert", Order = 1 };
      skillService.AddToCv(skill3, cvId, RootUrl);
      var skill4 = new Skill() { Name = "Software Engineering", Blurb = "Since 2001", Order = 2 };
      skillService.AddToCv(skill4, cvId, RootUrl);

      //Assert
      var skills = skillService.GetForCv(cvId,RootUrl);
      
      foreach(var skill in skills)
      {
        Assert.AreEqual($"{RootUrl}/cv/{skill.CvId}", skill.CvUrl);
        Assert.AreEqual($"{RootUrl}/skill/{skill.Id}", skill.Url);
      }
    }
  }
}
