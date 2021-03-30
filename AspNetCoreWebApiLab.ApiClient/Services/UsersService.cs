using System;
using RestSharp;
using System.Linq;
using System.Collections.Generic;
using AspNetCoreWebApiLab.ApiClient.DTOs;

namespace AspNetCoreWebApiLab.ApiClient.Services
{
    public class UsersService
    {
        private RestClient _restClient;

        private string _authorizationToken;

        private const string ResourceName = "users";

        public UsersService(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public UsersService(string baseUrl, string authorizationToken) : this(baseUrl)
        {
            _authorizationToken = authorizationToken;
        }

        public Jwt SignIn(SignIn signInData)
        {
            var request = new RestRequest($"{ResourceName}/signin", Method.POST);
            request.AddJsonBody(signInData);
            var response = _restClient.Execute<Jwt>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public User GetUser(int id)
        {
            var request = BuildRequest($"{ResourceName}/{id}", Method.GET);
            var response = _restClient.Execute<User>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public User PostUser(User user)
        {
            var request = BuildRequest($"{ResourceName}", Method.POST, user);
            var response = _restClient.Execute<User>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public User PutUser(User user)
        {
            var request = BuildRequest($"{ResourceName}", Method.PUT, user);
            var response = _restClient.Execute<User>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public User PatchUser(int id, IEnumerable<PatchOperation> patchOperations)
        {
            var request = BuildRequest($"{ResourceName}/{id}", Method.PATCH, patchOperations);
            var response = _restClient.Execute<User>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public bool DeleteUser(int id)
        {
            var request = BuildRequest($"{ResourceName}/{id}", Method.DELETE);
            var response = _restClient.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public string OptionsUser()
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

        private RestRequest BuildRequest(string resource, Method method, object bodyData)
        {
            var request = BuildRequest(resource, method);
            request.AddJsonBody(bodyData);

            return request;
        }

    }
}
