using RestSharp;
using System;

namespace AspNetCoreWebApiLab.ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new RestClient("https://localhost:44325/api/");
            var request = new RestRequest("roles/7", Method.GET);

            var response = client.Execute(request);
        }
    }
}
