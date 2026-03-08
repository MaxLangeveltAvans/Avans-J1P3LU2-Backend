using Microsoft.AspNetCore.Mvc;
using Moq;
using MySecureBackend.WebApi.Controllers;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;
using MySqlX.XDevAPI.Common;

namespace MySecureBackend.Tests
{
    [TestClass]
    public sealed class EnvironmentControllerTests
    {
         private EnvironmentController _controller;
         private Mock<IEnvironmentRepository> _environmentRepository;
         private Mock<IAuthenticationService> _authenticationService;

         [TestInitialize]
         public void Setup()
         {
             _environmentRepository = new Mock<IEnvironmentRepository>();
             _authenticationService = new Mock<IAuthenticationService>();
             
             _controller = new EnvironmentController(_environmentRepository.Object, _authenticationService.Object);
         }

         [TestMethod]
         public async Task AddEnvironments_Succeeds()
         {
             // Arrange
             string userId = Guid.NewGuid().ToString();
             Environment2D testEnvironment = new Environment2D
             {
                 Id = Guid.NewGuid().ToString(),
                 Name = "Test",
                 MaxHeight = 10,
                 MaxLength = 10,
                 OwnerId = userId
             };
             _authenticationService.Setup(p => p.GetCurrentAuthenticatedUserId()).Returns(userId);
             
             // Act
             var result = await _controller.AddAsync(testEnvironment);
             
             // Assert Repository Call
             _environmentRepository.Verify(r => r.InsertAsync(testEnvironment), Times.Once);
             
             // Assert result
             var createdResult = Assert.IsInstanceOfType<CreatedAtRouteResult>(result.Result);

             Assert.AreEqual("GetEnvironmentById", createdResult.RouteName);
             Assert.AreEqual(testEnvironment.Id, createdResult.RouteValues["id"]);
             Assert.AreSame(testEnvironment, createdResult.Value);

         }

         [TestMethod]
         public async Task AddEnvironments_Fails_When_Because_Name_Exists()
         {
             // Arrange
             string userId = Guid.NewGuid().ToString();
             Environment2D testEnvironment = new Environment2D
             {
                 Id = Guid.NewGuid().ToString(),
                 Name = "Test",
                 MaxHeight = 10,
                 MaxLength = 10,
                 OwnerId = userId
             };
             _authenticationService.Setup(p => p.GetCurrentAuthenticatedUserId()).Returns(userId);
             _environmentRepository.Setup(x => x.SelectByOwnerIdAsync(userId)).ReturnsAsync(new List<Environment2D> { testEnvironment });
             
             // Act
             var response = await _controller.AddAsync(testEnvironment);
             
             // Assert
             Assert.IsInstanceOfType<ConflictObjectResult>(response.Result);

             var result = response.Result as ConflictObjectResult;
             Assert.IsNotNull(result);

             var details = result.Value as ProblemDetails;
             Assert.IsNotNull(details);
             Assert.AreEqual($"environment {testEnvironment.Name} is already assigned", details.Detail);
         }
         
         [TestMethod]
         public async Task AddEnvironments_Fails_When_Limit_Of_Five_Is_Reached()
         {
             string userId = Guid.NewGuid().ToString();
             Environment2D testEnvironment = new Environment2D
             {
                 Id = Guid.NewGuid().ToString(),
                 Name = "Test",
                 MaxHeight = 10,
                 MaxLength = 10,
                 OwnerId = userId
             };
             var existingEnvironments = Enumerable.Range(1, 5).Select(_ => new Environment2D()).ToList();
             
             _authenticationService.Setup(p => p.GetCurrentAuthenticatedUserId()).Returns(userId);
             _environmentRepository.Setup(x => x.SelectByOwnerIdAsync(userId)).ReturnsAsync(existingEnvironments);
             
             var response = await _controller.AddAsync(testEnvironment);
             
             Assert.IsInstanceOfType<BadRequestObjectResult>(response.Result);

             var result = response.Result as BadRequestObjectResult;
             Assert.IsNotNull(result);

             var details = result.Value as ProblemDetails;
             Assert.IsNotNull(details);
             Assert.AreEqual("Maximum limit of 5 environments reached", details.Detail);
         }
    }
    
}

