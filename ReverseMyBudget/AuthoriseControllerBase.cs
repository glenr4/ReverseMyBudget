using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReverseMyBudget
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthoriseControllerBase : ControllerBase
    {
    }
}