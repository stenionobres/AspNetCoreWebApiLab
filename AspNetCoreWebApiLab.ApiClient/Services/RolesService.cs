using System;
using RestSharp;
using System.Linq;
using AspNetCoreWebApiLab.ApiClient.DTOs;

namespace AspNetCoreWebApiLab.ApiClient.Services
{
    public class RolesService
    {
        private RestClient _restClient;

        private string _authorizationToken;

        private const string ResourceName = "roles";

        public RolesService(string baseUrl, string authorizationToken)
        {
            _restClient = new RestClient(baseUrl);
            _authorizationToken = authorizationToken;
        }

        public Role GetRole(int id)
        {
            var request = BuildRequest($"{ResourceName}/{id}", Method.GET);
            var response = _restClient.Execute<Role>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public Role PostRole(Role role)
        {
            var request = BuildRequest($"{ResourceName}", Method.POST, role);
            var response = _restClient.Execute<Role>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public Role PutRole(Role role)
        {
            var request = BuildRequest($"{ResourceName}", Method.PUT, role);
            var response = _restClient.Execute<Role>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public bool DeleteRole(int id)
        {
            var request = BuildRequest($"{ResourceName}/{id}", Method.DELETE);
            var response = _restClient.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public string OptionsRole()
        {
            var request = BuildRequest($"{ResourceName}", Method.OPTIONS);
            var response = _restClient.Execute(request);
            var allowHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("Allow"));

            return allowHeader.Value.ToString();
        }

        private RestRequest BuildRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Authorization", $"Bearer {_authorizationToken}");

            return request;
        }

        private RestRequest BuildRequest(string resource, Method method, Role role)
        {
            var request = BuildRequest(resource, method);
            request.AddJsonBody(role);

            return request;
        }
    }
}
