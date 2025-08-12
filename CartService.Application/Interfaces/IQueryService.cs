using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces
{
    public interface IQueryService

    { 
        Task<T> GetCartPage<T>(Guid userId, int pageNumber, int pageSize) where T : class;
    }
}
