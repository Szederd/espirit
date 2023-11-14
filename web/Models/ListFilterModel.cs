using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapp.Models
{
    public class ListFilterModel
    {
        

        public string SortCol { get; set; }
        public SortOrderEnum SortOrder { get; set; }
        public int PageIndex { get; set; }
        public int AllCount { get; set; }
        public int PageSize { get; set; }
        public string SearchVal { get; set; }
        public IEnumerable<SlowDataSource.Domain.SampleData> Items { get; set; }

    }

    public enum SortOrderEnum
    {
        Unsorted = 0,
        Asc = 1,
        Desc = 2
    }
}