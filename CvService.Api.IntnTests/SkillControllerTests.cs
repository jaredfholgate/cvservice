using CvService.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class SkillControllerTests : BaseTest
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
      var cvId = CreateCvAndGetId(client);

      //Act
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var postResponse = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Constants.Encoding, Constants.MediaType)).Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);
      var getResponse = client.GetAsync($"/cv/{cvId}/skills").Result;
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      dynamic skills = JArray.Parse(getResponse.Content.ReadAsStringAsync().Result);
      var result = skills[0];

      Assert.AreEqual(skill.Name, (string)result.name);
      Assert.AreEqual(skill.Blurb, (string)result.blurb);
      Assert.AreEqual(skill.Order, (int)result.order);
      Assert.AreEqual(cvId, (int)result.cvId);
    }

    [TestMethod]
    public void UpdateSkill()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cvId = CreateCvAndGetId(client);
      var skillId = CreateSkillAndGetId(client, cvId);

      //Act
      var skillUpdate = new { Name = "C#", Blurb = "Been using it since 2001.", Order = 24 };
      var putResponse = client.PutAsync($"/skill/{skillId}", new StringContent(JsonConvert.SerializeObject(skillUpdate), Constants.Encoding, Constants.MediaType)).Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
      var getResponse = client.GetAsync($"/skill/{skillId}").Result;
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(skillUpdate.Name, (string)result.name);
      Assert.AreEqual(skillUpdate.Blurb, (string)result.blurb);
      Assert.AreEqual(skillUpdate.Order, (int)result.order);
      Assert.AreEqual(cvId, (int)result.cvId);
    }

    [TestMethod]
    public void DeleteSkill()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cvId = CreateCvAndGetId(client);
      var skillId = CreateSkillAndGetId(client, cvId);

      //Act
      var deleteResponse = client.DeleteAsync($"/skill/{skillId}").Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
      var getResponse = client.GetAsync($"/cv/{cvId}/skills").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;
      Assert.AreEqual("[]", result);
    }
  }
}
