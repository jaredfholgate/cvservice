using CvService.Tests.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CvService.Api.IntnTests
{
  public class BaseTest
  {
    protected int CreateCvAndGetId(HttpClient client)
    {
      var cv = new { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Constants.Encoding, Constants.MediaType)).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      return cvId;
    }

    protected int CreateCompanyAndGetId(HttpClient client, int cvId)
    {
      var company = new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds", Blurb = "Blah, Blah, Blah" };
      var postCompanyResponse = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Constants.Encoding, Constants.MediaType)).Result;
      dynamic newCompany = JObject.Parse(postCompanyResponse.Content.ReadAsStringAsync().Result);
      var companyId = newCompany.id;
      return companyId;
    }
    protected int CreateSkillAndGetId(HttpClient client, int cvId)
    {
      var skill = new { Name = "Continuous Delivery", Blurb = "Awesome at CI and CD", Order = 12 };
      var postSkillResponse = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Constants.Encoding, Constants.MediaType)).Result;
      dynamic newSkill = JObject.Parse(postSkillResponse.Content.ReadAsStringAsync().Result);
      var skillId = newSkill.id;
      return skillId;
    }
  }
}
