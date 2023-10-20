﻿using AutoMapper;
using DailyShop.Business.Features.Categories.DailyFrontendDtos;
using DailyShop.Business.Features.Categories.Models;
using DailyShop.Business.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyShop.Business.Features.Categories.Queries.GetListCategory
{
    public class GetListCategoryQuery:IRequest<List<GetListCategoryFrontendDto>>
    {
        public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, List<GetListCategoryFrontendDto>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;

            public GetListCategoryQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<List<GetListCategoryFrontendDto>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
            {
                var categories = await _categoryRepository.Query().ToListAsync();
                GetListCategoryModel mappedGetListCategoryModels =_mapper.Map<GetListCategoryModel>(categories);
                return mappedGetListCategoryModels.GetListCategoryFrontendDtos;
            }
        }
    }
}