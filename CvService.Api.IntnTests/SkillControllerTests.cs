using CvService.Tests.Shared;
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
      var cv = new { Name = Constants.CvName, Blurb = Constants.CvBlurb };
      var postResponse  = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var newCompany = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Constants.Encoding, Constants.MediaType)).Result;

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
      var cv = new { Name =Constants.CvName, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var postSkillResponse = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Constants.Encoding, Constants.MediaType)).Result;
      dynamic newSkill = JObject.Parse(postSkillResponse.Content.ReadAsStringAsync().Result);
      var skillId = newSkill.id;

      //Act
      var skillUpdate = new { Name = "C#", Blurb = "Been using it since 2001.", Order = 24 };
      var putResponse = client.PutAsync($"/skill/{skillId}", new StringContent(JsonConvert.SerializeObject(skillUpdate), Constants.Encoding, Constants.MediaType)).Result;

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
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var postSkillResponse = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Constants.Encoding, Constants.MediaType)).Result;
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
