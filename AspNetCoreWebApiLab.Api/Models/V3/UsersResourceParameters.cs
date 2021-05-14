
namespace AspNetCoreWebApiLab.Api.Models.V3
{
    public class UsersResourceParameters
    {
        const int maxPageSize = 6;
        private int _pageSize = 4;

        public int PageNumber { get; set; } = 1;

        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value; 
        }

        public string OrderBy { get; set; } = "FirstName";
    }
}
