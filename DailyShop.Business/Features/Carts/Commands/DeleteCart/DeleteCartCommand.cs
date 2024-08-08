﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Exceptions;
using DailyShop.Business.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DailyShop.Business.Features.Carts.Commands.DeleteCart
{
	public class DeleteCartCommand : IRequest
	{
		public int CartItemId { get; set; }
		public int UserId { get; set; }
		public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand>
		{
			private readonly ICartRepository _cartRepository;

			public DeleteCartCommandHandler(ICartRepository cartRepository)
			{
				_cartRepository = cartRepository;
			}

			public async Task Handle(DeleteCartCommand request, CancellationToken cancellationToken)
			{
				var cart = await _cartRepository.Query().Where(x => x.UserId == request.UserId)
							   .Include(ci => ci.CartItems).FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
						   throw new BusinessException("Ürün sepetinizde bulunamadı.");

				var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == request.CartItemId) ??
							   throw new BusinessException("Ürün sepetinizde bulunamadı.");

				cart.CartItems.Remove(cartItem);
				await _cartRepository.UpdateAsync(cart);

			}

		}
	}
}
