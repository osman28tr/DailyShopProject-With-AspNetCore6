﻿using Core.Persistence.Repositories;
using DailyShop.Entities.Concrete;

namespace DailyShop.Business.Services.Repositories
{
    public interface IProductColorRepository:IAsyncRepository<ProductColor>,IRepository<ProductColor>
    {
    }
}
