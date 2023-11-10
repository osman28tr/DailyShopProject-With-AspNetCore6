﻿using AutoMapper;
using DailyShop.Business.Features.Products.Models;
using DailyShop.Business.Services.Repositories;
using DailyShop.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyShop.Business.Features.Products.Queries.GetByIdProduct
{
    public class GetByIdProductQuery:IRequest<List<GetListProductByCategoryAndIsDeleteViewModel>>
    {
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, List< GetListProductByCategoryAndIsDeleteViewModel>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public GetByIdProductQueryHandler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<List<GetListProductByCategoryAndIsDeleteViewModel>> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.Query().Where(p => p.CategoryId == request.CategoryId && p.IsDeleted == request.IsDeleted).Include(r => r.Reviews).ThenInclude(ru => ru.AppUser).Include(u => u.User).Include(c => c.Colors).Include(s => s.Sizes).Include(pi => pi.ProductImages).ToListAsync();

                var mappedProduct = _mapper.Map<List<GetListProductByCategoryAndIsDeleteViewModel>>(product);

                for (int i = 0; i < product.Count; i++)
                {
                    foreach (var review in product[i].Reviews)
                    {
                        GetListReviewByProductViewModel reviewModel = new() { Name = review.Name, ReviewDescription = review.Description, ReviewRating = review.Rating, ReviewAvatar = review.Avatar, ReviewStatus = review.Status, ReviewCreatedDate = review.CreatedAt, ReviewUpdatedDate = review.UpdatedAt, UserName = review.AppUser.FirstName + " " + review.AppUser.LastName };
                        mappedProduct[i].ReviewsModel.Add(reviewModel);
                    }
                }
                return mappedProduct;
            }
        }
    }
}