## CV Service

[![Build status](https://jaredfholgate.visualstudio.com/CvService/_apis/build/status/CvService)](https://jaredfholgate.visualstudio.com/CvService/_build/latest?definitionId=4)

This service is built using ASP.NET Core. It uses SQL Server for persistence.

Go here to take a look at the API documentation;

https://cvserviceprod.azurewebsites.net/swagger

To see an example CV result, click here;

https://cvserviceprod.azurewebsites.net/cv/1/full


### Pre-requisites

The solution was built with the following tools;

1. Visual Studio Enterpise 2017 15.9.5
2. .Net Core SDK 2.2.102

These should be the only things required to build and debug locally.

### How To Build Locally

This solution uses Cake (https://cakebuild.net/) build, so that you can run the build locally the same as it runs on the server, enabling quick and easy debugging of build issues.

1. Open PowerShell Administrator Console
2. Set PowerShell Execution Policy
<code>Set-ExecutionPolicy Unrestricted</code>
3. Navigate to Root of Solution
4. Run <code>./Build.ps1</code>

To see what the build is doing, look at the content of the build.cake file.

It runs the following steps;

1. Prepare variables, including getting the version number from the build.
2. Clean folders and test results.
2. Restore Nuget packaged.
3. Build the code.
4. Run unit tests.
5. Run self hosted integration tests.
6. Publish the app ready for deployment.

### Source Control

The solution uses GitHub for source control and can be found here;

https://github.com/jaredfholgate/cvservice

### How CI and CD is setup

The solution uses Azure DevOps for CI and CD.

The CI Build can be found here, it is triggered on commit to GitHub;

https://jaredfholgate.visualstudio.com/CvService/_build?definitionId=4

The steps of the CI build are;

1. Get sources from GitHub.
2. Install the correct version of the .NET Core SDK on the Build Server.
3. Run the Cake build.
4. Publish Test Results.
5. Publish the artifacts.

The CD pipeline can be found here;

https://jaredfholgate.visualstudio.com/CvService/_release?definitionId=1&view=mine&_a=releases

The CD pipeline is triggered on a successful CI build, it has two stages;

1. Test
2. Production

The step of each stage is very simple, it just deploys to Azure App Service.

### Hosting

The app is hosted in Azure as an Azure App Service. The database is hosted by Azure SQL Service.

I chose Azure App Service due to time constraints, it is very easy to configure and get up and running.

The appllication gets it's connection string from Environment variables provided by the App Aervice.

The App Service urls are;

1. Test: https://cvservicetest.azurewebsites.net/
2. Production: https://cvserviceprod.azurewebsites.net/

### Documentation

The API uses swagger for documentation and manual testing, this can be found at /swagger.

For the two Azure instances, this is;

1. Test: https://cvservicetest.azurewebsites.net/swagger
2. Production: https://cvserviceprod.azurewebsites.net/swagger

### Testing

The API is fully tested as part of the CI build, you will see tests at the repository, service and web api level.

The tests use the SQLLite in memory database, so that they don't require a full version of SQL to run. This also avoids the need to mock the database layer.

The integration tests self host the API and inject SQLLite to ensure that the tests will run quickly and in any environment.

### ToDo

The following items would be worked on if I had more time;

1. Security. The app is currently unsecured, I would add OAuth authentication or use an API Gateway to secure it.
2. Performance and Load Testing.
3. Automated Acceptance testing at the Test stage if the CD pipeline.
4. Application Performance and Availability Monitoring. Azure Application Insights is configured and ready to go, but I haven't plugged it in at the code level yet.
5. Custom Url and SSL Cert for Production.
6. Hosting environment. I chose Azure App Service for simplicty, but I may want to containerise and use orchestration for the API if it becomes part of a bigger solution.
7. Persistence layer. I chose SQL Server for simplicity, but I would consider using a NoSQL database, such as CosmosDB depending on the predicted load and global reach of the API.
8. Dependency injection, I would consider another container that has more features, such as reflection.
9. Look to improve the performance of the CI build, specifically the Cake step.
10. More refactoring and tidying up of tests and code. 
11. A nice user interface.
12. Extending features to add qualifications, contact details and other ad hoc sections to the CV (such as Interests).
13. More testing for edge case and exceptions.
14. Validation of inputs. 
15. Wrapping requests and results in a standard object to report success or failure.
16. Write a client in C# or another language to make calling the API as easy as possible.
17. Convert the Azure DevOps Build definition to YAML and store in source control.
18. Add users and roles.
19. Enable updating / adding of multiple items in one put / post / delete.
20. Improve the swagger documentation by customising it.
21. Implement versioning in code or via gateway.
22. Proper exception handling and useful errror messages.
23. Extend the Ci / CD pipeline for static analysis, coverage and security teating.
24. Consider refactoring to use async methods to improve performance and scalability.
