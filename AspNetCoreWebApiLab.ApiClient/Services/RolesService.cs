﻿using System;
using RestSharp;
using System.Linq;
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

        public Role PostRole(Role role)
        {
            var request = new RestRequest($"{ResourceName}", Method.POST);
            request.AddJsonBody(role);
            var response = _restClient.Execute<Role>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public Role PutRole(Role role)
        {
            var request = new RestRequest($"{ResourceName}", Method.PUT);
            request.AddJsonBody(role);
            var response = _restClient.Execute<Role>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public bool DeleteRole(int id)
        {
            var request = new RestRequest($"{ResourceName}/{id}", Method.DELETE);
            var response = _restClient.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }

        public string OptionsRole()
        {
            var request = new RestRequest($"{ResourceName}", Method.OPTIONS);
            var response = _restClient.Execute(request);
            var allowHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("Allow"));

            return allowHeader.Value.ToString();
        }
    }
}
