﻿using System;
using RestSharp;
using AspNetCoreWebApiLab.ApiClient.DTOs;

namespace AspNetCoreWebApiLab.ApiClient.Services
{
    public class UsersService
    {
        private RestClient _restClient;

        private const string ResourceName = "users";

        public UsersService(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public User GetUser(int id)
        {
            var request = new RestRequest($"{ResourceName}/{id}", Method.GET);
            var response = _restClient.Execute<User>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new ApplicationException($"{Convert.ToInt32(response.StatusCode)}: {response.Content}");
        }
    }
}
