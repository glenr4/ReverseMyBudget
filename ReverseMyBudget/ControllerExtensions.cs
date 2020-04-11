using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReverseMyBudget.Persistence.Sql;

namespace ReverseMyBudget
{
    public static class ControllerExtensions
    {
        public static IActionResult PagedOk<T>(this ControllerBase controller, PagedList<T> result)
        {
            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            controller.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return controller.Ok(result);
        }
    }
}