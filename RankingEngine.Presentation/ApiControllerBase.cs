using Microsoft.AspNetCore.Mvc;

namespace RankingEngine.Presentation
{
    [Route("api/v1/[controller]")]
    [ApiController, Produces("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
