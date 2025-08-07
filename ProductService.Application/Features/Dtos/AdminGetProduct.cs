using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Dtos
{
    public class AdminGetProduct
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
       public AdminGetProduct(int? pageNumber = null, int? pageSize = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public AdminGetProduct()
        {
        }
    }
}
