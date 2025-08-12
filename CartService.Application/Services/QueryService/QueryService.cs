using CartService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Services.QueryService
{
    public sealed class QueryService : IQueryService
    {
        public Task<T> GetCartPage<T>(Guid userId, int pageNumber, int pageSize) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
