using CartService.Application.Features.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces
{
    public interface IQueryService

    {
        Task<CartDto> GetCartPage(Guid userId, int pageNumber, int pageSize);
    }
}
