using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Common
{
    public class PagedList<T>
    {
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<T> List { get; set; }
        public PagedList(List<T> items, int pageSize, int totalCount)
        {
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
            List = items;
        }

        public PagedList()
        {
        }
    }
}
