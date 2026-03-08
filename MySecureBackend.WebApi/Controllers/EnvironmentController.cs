using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class EnvironmentController : ControllerBase
{
    private readonly IEnvironmentRepository _environmentRepository;
    private readonly IAuthenticationService _authenticationService;

    public EnvironmentController(IEnvironmentRepository environmentRepository,
        IAuthenticationService authenticationService)
    {
        _environmentRepository = environmentRepository;
        _authenticationService = authenticationService;
    }

    /*[HttpGet(Name = "GetEnvironments")]
    public async Task<ActionResult<List<Environment2D>>> GetAsync()
    {
        var environments = await _environmentRepository.SelectAsync();
        return Ok(environments);
    }*/

    [HttpGet("{environmentId}", Name = "GetEnvironmentById")]
    public async Task<ActionResult<Environment2D>> GetByIdAsync(string environmentId)
    {
        var environment = await _environmentRepository.SelectAsync(environmentId);

        if (environment == null)
            return NotFound(new ProblemDetails { Detail = $"environment {environmentId} not found" });
        
        return Ok(environment);
    }

    [HttpGet(Name = "GetEnvironmentsByOwnerId")]
    public async Task<ActionResult<List<Environment2D>>> GetByOwnerIdAsync()
    {
        string? ownerId = _authenticationService.GetCurrentAuthenticatedUserId();
        
        if (ownerId == null)
            return NotFound(new ProblemDetails { Detail = $"environments not found" });
        
        var environments = await _environmentRepository.SelectByOwnerIdAsync(ownerId);
        
        if (environments.IsNullOrEmpty())
            return NotFound(new ProblemDetails { Detail = $"environments not found" });
        
        return Ok(environments);
    }

    [HttpPost(Name = "AddEnvironment")]
    public async Task<ActionResult<Environment2D>> AddAsync(Environment2D environment)
    {
        string? ownerId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (environment.OwnerId == null)
            environment.OwnerId = ownerId;
        
        var userEnvironments = await _environmentRepository.SelectByOwnerIdAsync(ownerId);

        if (userEnvironments.ToList().Count > 4)
            return BadRequest(new ProblemDetails { Detail = "Maximum limit of 5 environments reached" });
        
        if (userEnvironments.ToList().Any(x => x.Name == environment.Name))
            return Conflict(new ProblemDetails { Detail = $"environment {environment.Name} is already assigned" });
        
        if (environment.Id == null)
            environment.Id = Guid.NewGuid().ToString();
        
        await  _environmentRepository.InsertAsync(environment);
        return CreatedAtRoute("GetEnvironmentById", new { environmentId = environment.Id });
    }

    [HttpPut("{environmentId}", Name = "UpdateEnvironment")]
    public async Task<ActionResult<Environment2D>> UpdateAsync(string environmentId, Environment2D environment)
    {
        var existingEnvironment = await _environmentRepository.SelectAsync(environmentId);

        if (existingEnvironment == null)
            return NotFound(new ProblemDetails { Detail = $"environment {environmentId} not found" });

        if (environment.Id != environmentId)
            return Conflict(new ProblemDetails
                { Detail = "The id of the environment in the route does not match the id of the environment in the body" });
        
        await _environmentRepository.UpdateAsync(environment);
        
        return Ok(environment);
    }

    [HttpDelete("{environmentId}", Name = "DeleteEnvironment")]
    public async Task<ActionResult<Environment2D>> DeleteAsync(string environmentId)
    {
        var environment = await _environmentRepository.SelectAsync(environmentId);
        
        if (environment == null)
            return NotFound(new ProblemDetails { Detail = $"environment {environmentId} not found" });
        
        await  _environmentRepository.DeleteAsync(environmentId);
        
        return Ok();
    }
}