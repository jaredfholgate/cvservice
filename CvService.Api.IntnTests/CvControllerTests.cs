using CvService.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class CvControllerTests
  {
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public CvControllerTests()
    {
      _factory = new CustomWebApplicationFactory<Startup>();
    }

    [TestMethod]
    public void CanAddCv()
    {
      //Arrage
      var client = _factory.CreateClient();
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };

      //Act
      var postResponse  = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv),Constants.Encoding, Constants.MediaType)).Result;
      dynamic result = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);

      //Assert
      Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);
      Assert.AreEqual(Constants.CvName, result.name.ToString());
      Assert.AreEqual(Constants.CvTagLine, result.tagLine.ToString());
      Assert.AreEqual(Constants.CvBlurb, result.blurb.ToString());

      var getResponse = client.GetAsync("/cv").Result;
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      result = JArray.Parse(getResponse.Content.ReadAsStringAsync().Result)[0];

      Assert.AreEqual(Constants.CvName, result.name.ToString());
      Assert.AreEqual(Constants.CvTagLine, result.tagLine.ToString());
      Assert.AreEqual(Constants.CvBlurb, result.blurb.ToString());
    }

    [TestMethod]
    public void UpdateCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv),Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var cvUpdate = new { Name = Constants.CvNameUpdate, TagLine = Constants.CvTagLineUpdate, Blurb = Constants.CvBlurbUpdate };
      var putResponse = client.PutAsync($"/cv/{cvId}", new StringContent(JsonConvert.SerializeObject(cvUpdate),Constants.Encoding, Constants.MediaType)).Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
      var getResponse = client.GetAsync($"/cv/{cvId}").Result;
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);
      Assert.AreEqual(Constants.CvNameUpdate, result.name.ToString());
      Assert.AreEqual(Constants.CvTagLineUpdate, result.tagLine.ToString());
      Assert.AreEqual(Constants.CvBlurbUpdate, result.blurb.ToString());
    }

    [TestMethod]
    public void DeleteCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv),Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var deleteResponse = client.DeleteAsync($"/cv/{cvId}").Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
      var getResponse = client.GetAsync($"/cv").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;
      Assert.AreEqual("[]", result);
    }

    [TestMethod]
    public void CanGetFullCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv),Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      foreach(var company in TestData.MultipleCompanies)
      {
        var newCompany = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company),Constants.Encoding, Constants.MediaType)).Result;
      }
      foreach(var skill in TestData.MultipleSkills)
      {
        var newSkill = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill),Constants.Encoding, Constants.MediaType)).Result;
      }

      //Act
      var getResponse = client.GetAsync($"/cv/{cvId}/full").Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      var getResult = getResponse.Content.ReadAsStringAsync().Result;
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(Constants.CvName, result.name.ToString());
      Assert.AreEqual(Constants.CvTagLine, result.tagLine.ToString());
      Assert.AreEqual(Constants.CvBlurb, result.blurb.ToString());
      Assert.AreEqual(9, result.companies.Count);
      Assert.AreEqual(17, result.skills.Count);

      var date = DateTime.Parse("3000-01-01");
      foreach (var company in result.companies)
      {
        Assert.IsTrue((DateTime)company.start < date);
        date = (DateTime)company.start;
      }

      var order = 0;
      foreach (var skill in result.skills)
      {
        Assert.IsTrue((int)skill.order > order);
        order = (int)skill.order;
      }
    }
       
    //[TestMethod] //This method is used as a one off to populate some example data.
    public void GenerateData()
    {
      var client = new HttpClient
      {
        BaseAddress = new Uri("https://cvservicetest.azurewebsites.net")
      };
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv),Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      foreach (var company in TestData.MultipleCompanies)
      {
        var newCompany = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company),Constants.Encoding, Constants.MediaType)).Result;
      }
           
      foreach (var skill in TestData.MultipleSkills)
      {
        var newSkill = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill),Constants.Encoding, Constants.MediaType)).Result;
      }
    }
  }
}
