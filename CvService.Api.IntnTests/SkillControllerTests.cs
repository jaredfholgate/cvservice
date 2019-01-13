using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class SkillControllerTests
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";

    private readonly CustomWebApplicationFactory<Startup> _factory;

    public SkillControllerTests()
    {
      _factory = new CustomWebApplicationFactory<Startup>();
    }

    [TestMethod]
    public void CanAddSkill()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = "Test CV 1", Blurb = "Testing 1234567" };
      var postResponse  = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var newCompany = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Encoding.UTF8, "application/json")).Result;

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}/skills").Result;
      var getResult = getResponse.Content.ReadAsStringAsync().Result;
      dynamic skills = JArray.Parse(getResponse.Content.ReadAsStringAsync().Result);
      var result = skills[0];

      Assert.AreEqual(skill.Name, (string)result.name);
      Assert.AreEqual(skill.Blurb, (string)result.blurb);
      Assert.AreEqual(skill.Order, (int)result.order);
      Assert.AreEqual((int)cvId, (int)result.cvId);
    }

    [TestMethod]
    public void UpdateSkill()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = "Test CV 1", Blurb = "Testing 1234567" };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var postSkillResponse = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Encoding.UTF8, "application/json")).Result;
      dynamic newSkill = JObject.Parse(postSkillResponse.Content.ReadAsStringAsync().Result);
      var skillId = newSkill.id;

      //Act
      var skillUpdate = new { Name = "C#", Blurb = "Been using it since 2001.", Order = 24 };
      var putResponse = client.PutAsync($"/skill/{skillId}", new StringContent(JsonConvert.SerializeObject(skillUpdate), Encoding.UTF8, "application/json")).Result;

      //Assert
      var getResponse = client.GetAsync($"/skill/{skillId}").Result;
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(skillUpdate.Name, (string)result.name);
      Assert.AreEqual(skillUpdate.Blurb, (string)result.blurb);
      Assert.AreEqual(skillUpdate.Order, (int)result.order);
      Assert.AreEqual((int)cvId, (int)result.cvId);
    }

    [TestMethod]
    public void DeleteSkill()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var postSkillResponse = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Encoding.UTF8, "application/json")).Result;
      dynamic newSkill = JObject.Parse(postSkillResponse.Content.ReadAsStringAsync().Result);
      var skillId = newSkill.id;

      //Act
      var putResponse = client.DeleteAsync($"/skill/{skillId}").Result;

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}/skills").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;
      Assert.AreEqual("[]", result);
    }
  }
}
