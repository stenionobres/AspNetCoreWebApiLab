using System.Dynamic;
using System.Collections.Generic;

namespace AspNetCoreWebApiLab.Api.Models.V3
{
    public class UserPaginationMetadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }
        public string OrderBy { get; set; }
        public IEnumerable<ExpandoObject> Users { get; set; }

    }
}
