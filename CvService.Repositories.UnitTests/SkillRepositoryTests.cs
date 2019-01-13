using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class SkillRepositoryTests : BaseTest
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";

    [TestMethod]
    public void CanPersistASkill()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var skillRepository = new SkillRepository(context);

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
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

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb }; 
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

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
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
  }
}
