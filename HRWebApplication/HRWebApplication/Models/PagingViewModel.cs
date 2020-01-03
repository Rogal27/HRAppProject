using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRWebApplication.Models
{
    public class PagingViewModel<T>
    {
        public IEnumerable<T> Collection { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public PagingViewModel(IEnumerable<T> col, int count, int pageSize)
        {
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Collection = col;
        }
    }
}
