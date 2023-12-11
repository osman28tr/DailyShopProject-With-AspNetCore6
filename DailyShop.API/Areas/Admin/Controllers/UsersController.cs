﻿using Core.CrossCuttingConcerns.Exceptions;
using DailyShop.API.Helpers;
using DailyShop.Business.Features.Products.Queries.GetListProductByUserId;
using DailyShop.Business.Features.Reviews.Commands.UpdateStatusReview;
using DailyShop.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyShop.API.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly ImageHelper _imageHelper;

        public UsersController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }
        [HttpGet("{userId:int}/Products")]
        public async Task<IActionResult> GetProductByUserId(int userId)
        {
            var productValues = await Mediator?.Send(new GetListProductByUserIdQuery() { UserId = userId })!;
            if (productValues == null)
                throw new BusinessException("Bu kullanıcıya ait bir ürün bulunamadı veya kaldırıldı! ");
            foreach (var product in productValues)
            {
                product.BodyImage = GetImageByHelper(product.BodyImage);
                if (product.ProductImages != null)
                {
                    product.ProductImages = product.ProductImages.Select(x =>
                        GetImageByHelper(x)).ToList();
                }
            }
            return Ok(new { data = productValues, message = "Ürün verileri başarıyla getirildi." });
        }

        [HttpPut("UpdateReviewStatus/{reviewId:int}")]
        public async Task<IActionResult> UpdateReviewStatus(int reviewId, string status)
        {
            await Mediator.Send(new UpdateStatusReviewCommand() { ReviewId = reviewId, Status = status });
            return Ok(new { Message = "Yorum durumu başarıyla güncellendi." });
        }

        [NonAction]
        public string GetImageByHelper(string imageName)
        {
            string getImage = _imageHelper.GetImage(Request.Scheme, Request.Host, Request.PathBase, imageName);
            return getImage;
        }
    }
}
