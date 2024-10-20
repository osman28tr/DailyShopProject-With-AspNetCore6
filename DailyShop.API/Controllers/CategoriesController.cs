using DailyShop.API.Helpers;
using DailyShop.Business.Features.Categories.Dtos;
using DailyShop.Business.Features.Categories.Queries.GetListCategory;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
	        var result = await Mediator?.Send(new GetListCategoryQuery())!;
            var getListCategoryDtos = result.Where(t => t.ParentCategoryId == null).ToList();
            return Ok(getListCategoryDtos);
        }
    }
}
