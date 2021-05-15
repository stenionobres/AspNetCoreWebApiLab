using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebApiLab.Api.Models.V3;

namespace AspNetCoreWebApiLab.Api.Tools
{
    public class UsersResourceURI
    {
        private readonly IUrlHelper _url;
        private readonly UsersResourceParameters _usersResourceParameters;
        private readonly bool _hasPreviousPage; 
        private readonly bool _hasNextPage;

        public UsersResourceURI(IUrlHelper url, 
                                UsersResourceParameters usersResourceParameters,
                                bool hasPreviousPage,
                                bool hasNextPage)
        {
            _url = url;
            _usersResourceParameters = usersResourceParameters;
            _hasPreviousPage = hasPreviousPage;
            _hasNextPage = hasNextPage;
        }

        public string PreviousPageLink
        {
            get
            {
                var previousPageLink = string.Empty;

                if (_hasPreviousPage)
                {
                    var parameters = new
                    {
                        pageNumber = _usersResourceParameters.PageNumber - 1,
                        pageSize = _usersResourceParameters.PageSize
                    };
                    previousPageLink = _url.Link("GetUsers", parameters);
                }

                return previousPageLink;
            }
        }

        public string NextPageLink
        {
            get
            {
                var nextPageLink = string.Empty;

                if (_hasNextPage)
                {
                    var parameters = new
                    {
                        pageNumber = _usersResourceParameters.PageNumber + 1,
                        pageSize = _usersResourceParameters.PageSize
                    };
                    nextPageLink = _url.Link("GetUsers", parameters);
                }

                return nextPageLink;
            }
        }
    }
}
