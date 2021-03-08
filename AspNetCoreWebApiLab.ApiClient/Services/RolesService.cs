using System;
using RestSharp;
using AspNetCoreWebApiLab.ApiClient.DTOs;

namespace AspNetCoreWebApiLab.ApiClient.Services
{
    public class RolesService
    {
        private RestClient _restClient;

        private const string ResourceName = "roles";

        public RolesService(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public Role GetRole(int id)
        {
            var request = new RestRequest($"{ResourceName}/{id}", Method.GET);
            var response = _restClient.Execute<Role>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }
    }
}
