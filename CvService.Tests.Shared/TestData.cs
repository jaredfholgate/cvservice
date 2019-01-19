using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Tests.Shared
{
  public static class TestData
  {
    public static List<dynamic> MultipleSkills = new List<dynamic>
    {
      new { Name = "Continuous Delivery", Blurb = "Expert •••••", Order = 5 },
      new { Name = "DevOps", Blurb = "Expert •••••", Order = 3 },
      new { Name = "Agile", Blurb = "Experienced ••••", Order = 8 },
      new { Name = "Software Engineering", Blurb = "Expert •••••", Order = 1 },
      new { Name = "People Development", Blurb = "Expert •••••", Order = 2 },
      new { Name = "Continuous Integration", Blurb = "Expert •••••", Order = 4 },
      new { Name = "Infrastructure Automation", Blurb = "Good ••••", Order = 6 },
      new { Name = "Containers and Orchestration", Blurb = "Good ••••", Order = 7 },
      new { Name = "Problem Solving", Blurb = "Expert •••••", Order = 9 },
      new { Name = "Scalable Architecture", Blurb = "Experienced ••••", Order = 10 },
      new { Name = "Quality", Blurb = "Expert •••••", Order = 11 },
      new { Name = "Security", Blurb = "Experienced ••••", Order = 12 },
      new { Name = "TDD, Solid and Design Patterns", Blurb = "Expert •••••", Order = 13 },
      new { Name = "Microsoft .NET Stack, TFS, Azure DevOps and Octopus Deploy", Blurb = "Expert •••••", Order = 14 },
      new { Name = "Java, Ruby, Go, NodeJS, React, Angular, Jenkins", Blurb = "Learning ••••", Order = 15 },
      new { Name = "Microsoft Azure", Blurb = "Experienced ••••", Order = 16 },
      new { Name = "AWS and Google Cloud", Blurb = "Learning •••", Order = 17 }
    };

    public static List<dynamic> MultipleCompanies = new List<dynamic>
    {
      new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" },
      new { Start = DateTime.Parse("2017-01-01"), End = (DateTime?)null, CompanyName = "Maples Group", Role = "Senior Software Engineering Manager (DevOps)", Location = "Montreal", Blurb = @"• Introduced micro service and event sourcing architecture.
• Innovated with infrastructure automation,
        containers and cloud.
• Implemented application monitoring and identity management.
• Reduced cycle time to < 2 weeks for all apps.
• Reduced production bugs to < 1 per deployment across all apps.
 • Introduced skills matrix and development plans for Engineers.
 • Matured test automation and significantly reduced regression time." },
      new { Start = DateTime.Parse("2012-11-01"), End = DateTime.Parse("2017-01-01"), CompanyName = "Maples and Calder", Role = "Software Development Manger", Location = "Cayman Islands", Blurb = @"• Introduced Continuous Integration and Continuous Delivery.
• Reduced cycle time to 4 weeks from over 6 months.
• Reduced production bugs from 30+ to <5 per deployment.
• Trained engineers in TDD, SOLID and Design Patterns.
• Introduced coding and architectural standards and pull requests.
• Introduced Git, trunk-based development and database source control." },
      new { Start = DateTime.Parse("2003-12-01"), End = DateTime.Parse("2007-04-01"), CompanyName = "Carlsberg UK", Role = "Systems Engineer", Location = "Leeds" },
      new { Start = DateTime.Parse("2002-05-01"), End = DateTime.Parse("2003-12-01"), CompanyName = "Carlsberg UK", Role = "Quality System Manager", Location = "Leeds" },
      new { Start = DateTime.Parse("2007-04-01"), End = DateTime.Parse("2007-11-01"), CompanyName = "Skipton Financial Services", Role = "Business Systems Developer", Location = "Skipton" },
      new { Start = DateTime.Parse("2007-11-01"), End = DateTime.Parse("2009-12-01"), CompanyName = "Cascade HR", Role = "Project Web Developer", Location = "Leeds" },
      new { Start = DateTime.Parse("2010-01-01"), End = DateTime.Parse("2011-12-01"), CompanyName = "Maples and Calder", Role = "Software Developer", Location = "Leeds" },
      new { Start = DateTime.Parse("2011-12-01"), End = DateTime.Parse("2012-11-01"), CompanyName = "Maples and Calder", Role = "eCommerce Software Team Leader", Location = "Leeds" }
    };
  }
}
