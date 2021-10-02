using Microsoft.AspNetCore.Mvc;
using ReverseMyBudget.Persistence.Sql;
using System.Text.Json;

namespace ReverseMyBudget
{
    public static class ControllerExtensions
    {
        public static IActionResult PagedOk<T>(this ControllerBase controller, PagedList<T> result)
        {
            string headerName = "x-pagination";

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            controller.Response.Headers.Add(headerName, JsonSerializer.Serialize(metadata, options));

            return controller.Ok(result);
        }
    }
}