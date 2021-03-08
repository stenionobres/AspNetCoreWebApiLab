using System;
using AspNetCoreWebApiLab.ApiClient.Services;

namespace AspNetCoreWebApiLab.ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var baseUrl = "https://localhost:44325/api/";
                var rolesService = new RolesService(baseUrl);
                var role = rolesService.GetRole(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
