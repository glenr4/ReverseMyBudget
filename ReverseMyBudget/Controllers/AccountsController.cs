using Microsoft.AspNetCore.Mvc;

namespace ReverseMyBudget.Controllers
{
    public class AccountsController : AuthoriseControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new string[] {
                "Item 1",
                "Item 2",
                "Item 3",
            });
        }
    }
}