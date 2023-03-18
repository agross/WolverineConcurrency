using Microsoft.AspNetCore.Mvc;

using Wolverine;

using WolverineConcurrency.Handlers;
using WolverineConcurrency.Model;

namespace WolverineConcurrency.Controllers;

[ApiController]
[Route("")]
public class TestController : ControllerBase
{
  [HttpGet(Name = "test")]
  public async Task<IActionResult> Get([FromServices] IMessageBus bus,
                                       CancellationToken ct)
  {
    await bus.InvokeAsync<TheReturnValue>(new SomeCommand(), ct);

    return Ok();
  }
}
