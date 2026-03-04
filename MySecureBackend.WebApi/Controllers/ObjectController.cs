using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class ObjectController : ControllerBase
{
    private readonly IObjectRepository _objectRepository;
    private readonly IAuthenticationService _authenticationService;
    
    public ObjectController(IObjectRepository objectRepository, IAuthenticationService authenticationService)
    {
        _objectRepository = objectRepository;
        _authenticationService = authenticationService;
    }

    [HttpGet(Name = "GetObjects")]
    public async Task<ActionResult<List<Object2D>>> GetAsync()
    {
        var object2Ds = await _objectRepository.SelectAsync();
        return Ok(object2Ds);
    }

    [HttpGet("{objectId}", Name = "GetObjectById")]
    public async Task<ActionResult<Object2D>> GetByIdAsync(string objectId)
    {
        var object2D = await _objectRepository.SelectAsync(objectId);

        if (object2D == null)
            return NotFound(new ProblemDetails { Detail = $"object {objectId} not found" });
        
        return Ok(object2D);
    }

    [HttpPost(Name = "AddObject")]
    public async Task<ActionResult<Object2D>> AddAsync(Object2D object2D)
    {
        if (object2D.Id == null)
            object2D.Id = Guid.NewGuid().ToString();
        
        await _objectRepository.InsertAsync(object2D);
        
        return CreatedAtRoute("GetObjectById", new { id = object2D.Id }, object2D);
    }

    [HttpPut("{objectId}", Name = "UpdateObject")]
    public async Task<ActionResult<Object2D>> UpdateAsync(string objectId, Object2D object2D)
    {
        var existingObject2D = await _objectRepository.SelectAsync(objectId);

        if (existingObject2D == null)
            return NotFound(new ProblemDetails { Detail = $"object {objectId} not found" });

        if (object2D.Id != objectId)
            return Conflict(new ProblemDetails
                { Detail = "The id of the object in the route does not match the id of the object in the body" });

        await _objectRepository.UpdateAsync(object2D);

        return Ok(object2D);
    }

    [HttpDelete("{objectId}", Name = "DeleteObject")]
    public async Task<ActionResult<Object2D>> DeleteAsync(string objectId)
    {
        var object2D = await _objectRepository.SelectAsync(objectId);

        if (object2D == null)
            return NotFound(new ProblemDetails { Detail = $"object {objectId} not found" });
        
        await _objectRepository.DeleteAsync(objectId);
        
        return Ok();
    }
}